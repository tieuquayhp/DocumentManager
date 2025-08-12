using AutoMapper;
using DocumentManager.API.DTOs;
using DocumentManager.DAL.Data;
using DocumentManager.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutgoingDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OutgoingDocumentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Phương thức trợ giúp private để tránh lặp lại mã nguồn.
        /// Nó tạo một câu truy vấn IQueryable đã bao gồm tất cả các bảng liên quan
        /// cần thiết để ánh xạ sang OutgoingDocumentDto một cách hoàn chỉnh.
        /// </summary>
        private IQueryable<OutgoingDocument> GetFullDocumentsQuery()
        {
            return _context.OutgoingDocuments
                .Include(d => d.OutgoingDocumentType)
                .Include(d => d.OutgoingDocumentFormat)
                .Include(d => d.IssuingUnit)
                .Include(d => d.RelatedProject)
                .Include(d => d.RecipientGroup)
                .AsNoTracking(); // Sử dụng AsNoTracking() để tăng hiệu suất cho các truy vấn chỉ đọc.
        }

        // GET: api/outgoingdocuments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutgoingDocumentDto>>> GetOutgoingDocuments()
        {
            var documents = await GetFullDocumentsQuery().ToListAsync();
            var documentDtos = _mapper.Map<IEnumerable<OutgoingDocumentDto>>(documents);
            return Ok(documentDtos);
        }

        // GET: api/outgoingdocuments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OutgoingDocumentDto>> GetOutgoingDocument(int id)
        {
            var document = await GetFullDocumentsQuery().FirstOrDefaultAsync(d => d.Id == id);

            if (document == null)
            {
                return NotFound($"Không tìm thấy tài liệu đi với ID {id}.");
            }

            var documentDto = _mapper.Map<OutgoingDocumentDto>(document);
            return Ok(documentDto);
        }

        // GET: api/outgoingdocuments/search?query=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OutgoingDocumentDto>>> SearchOutgoingDocuments([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }

            var documents = await GetFullDocumentsQuery()
                .Where(d =>
                    d.OutgoingDocumentNumber.ToString().Contains(query) ||
                    d.DocumentContent.Contains(query) ||
                    d.RelatedProject.RelatedProjectName.Contains(query) // Tìm kiếm cả theo tên dự án liên quan
                )
                .ToListAsync();

            var documentDtos = _mapper.Map<IEnumerable<OutgoingDocumentDto>>(documents);
            return Ok(documentDtos);
        }

        // POST: api/outgoingdocuments
        [HttpPost]
        public async Task<ActionResult<OutgoingDocumentDto>> PostOutgoingDocument(OutgoingDocumentForCreationDto creationDto)
        {
            // --- Validation quan trọng cho các khóa ngoại ---
            if (!await _context.OutgoingDocumentTypes.AnyAsync(t => t.Id == creationDto.OutgoingDocumentTypeID) ||
                !await _context.OutgoingDocumentFormats.AnyAsync(f => f.Id == creationDto.OutgoingDocumentFormatID) ||
                !await _context.IssuingUnits.AnyAsync(i => i.Id == creationDto.IssuingUnitID) ||
                !await _context.RelatedProjects.AnyAsync(p => p.Id == creationDto.RelatedProjectID) ||
                !await _context.RecipientGroups.AnyAsync(g => g.Id == creationDto.RecipientGroupID))
            {
                return BadRequest("Một hoặc nhiều ID liên quan không hợp lệ.");
            }

            var document = _mapper.Map<OutgoingDocument>(creationDto);

            _context.OutgoingDocuments.Add(document);
            await _context.SaveChangesAsync();

            // Lấy lại đối tượng vừa tạo bao gồm các bảng liên quan để trả về DTO hoàn chỉnh
            var createdDocument = await _context.OutgoingDocuments
                .Include(d => d.OutgoingDocumentType)
                .Include(d => d.OutgoingDocumentFormat)
                .Include(d => d.IssuingUnit)
                .Include(d => d.RelatedProject)
                .Include(d => d.RecipientGroup)
                .FirstOrDefaultAsync(d => d.Id == document.Id);

            var documentDto = _mapper.Map<OutgoingDocumentDto>(createdDocument);

            return CreatedAtAction(nameof(GetOutgoingDocument), new { id = documentDto.ID }, documentDto);
        }

        // PUT: api/outgoingdocuments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutgoingDocument(int id, OutgoingDocumentForUpdateDto updateDto)
        {
            // Lấy đối tượng cần cập nhật từ DB (đang được theo dõi bởi DbContext)
            var documentFromDb = await _context.OutgoingDocuments.FindAsync(id);
            if (documentFromDb == null)
            {
                return NotFound($"Không tìm thấy tài liệu đi với ID {id} để cập nhật.");
            }

            // (Tùy chọn) Validation khóa ngoại tương tự như POST để đảm bảo dữ liệu mới hợp lệ
            if (!await _context.OutgoingDocumentTypes.AnyAsync(t => t.Id == updateDto.OutgoingDocumentTypeID) ||
                !await _context.OutgoingDocumentFormats.AnyAsync(f => f.Id == updateDto.OutgoingDocumentFormatID) ||
                !await _context.IssuingUnits.AnyAsync(i => i.Id == updateDto.IssuingUnitID) ||
                !await _context.RelatedProjects.AnyAsync(p => p.Id == updateDto.RelatedProjectID) ||
                !await _context.RecipientGroups.AnyAsync(g => g.Id == updateDto.RecipientGroupID))
            {
                return BadRequest("Một hoặc nhiều ID liên quan không hợp lệ.");
            }

            // AutoMapper sẽ cập nhật các thuộc tính của 'documentFromDb' từ 'updateDto'
            _mapper.Map(updateDto, documentFromDb);

            await _context.SaveChangesAsync();

            // Trả về 204 No Content là chuẩn cho một yêu cầu PUT thành công
            return NoContent();
        }

        // DELETE: api/outgoingdocuments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutgoingDocument(int id)
        {
            var document = await _context.OutgoingDocuments.FindAsync(id);
            if (document == null)
            {
                return NotFound($"Không tìm thấy tài liệu đi với ID {id} để xóa.");
            }

            _context.OutgoingDocuments.Remove(document);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
