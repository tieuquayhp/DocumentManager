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
    public class IncomingDocumentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IncomingDocumentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private IQueryable<IncomingDocument> GetFullDocumentsQuery()
        {
            // Helper để tránh lặp lại các .Include()
            return _context.IncomingDocuments
                .Include(d => d.IssuingUnit)
                .Include(d => d.RelatedProject)
                .Include(d => d.RecipientGroup)
                .AsNoTracking();
        }

        // GET: api/incomingdocuments
        // GET: api/incomingdocuments?limit=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomingDocumentDto>>> GetIncomingDocuments([FromQuery] int? limit)
        {
            var query = GetFullDocumentsQuery().OrderByDescending(d => d.ReleaseDate); // Sắp xếp theo ngày mới nhất

            if (limit.HasValue && limit > 0)
            {
                var limitedDocuments = await query.Take(limit.Value).ToListAsync();
                return Ok(_mapper.Map<IEnumerable<IncomingDocumentDto>>(limitedDocuments));
            }

            var documents = await query.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<IncomingDocumentDto>>(documents));
        }

        // GET: api/incomingdocuments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomingDocumentDto>> GetIncomingDocument(int id)
        {
            var document = await GetFullDocumentsQuery().FirstOrDefaultAsync(d => d.Id == id);
            if (document == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IncomingDocumentDto>(document));
        }

        // GET: api/incomingdocuments/search?query=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<IncomingDocumentDto>>> SearchIncomingDocuments([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var documents = await GetFullDocumentsQuery()
                .Where(d => d.IncomingDocumentNumber.ToString().Contains(query) || d.DocumentContent.Contains(query))
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<IncomingDocumentDto>>(documents));
        }
        // POST: api/incomingdocuments
        [HttpPost]
        public async Task<ActionResult<IncomingDocumentDto>> PostIncomingDocument(IncomingDocumentForCreationDto creationDto)
        {
            // Kiểm tra sự tồn tại của các khóa ngoại
            if (!await _context.IssuingUnits.AnyAsync(i => i.Id == creationDto.IssuingUnitID) ||
                !await _context.RelatedProjects.AnyAsync(p => p.Id == creationDto.RelatedProjectID) ||
                !await _context.RecipientGroups.AnyAsync(g => g.Id == creationDto.RecipientGroupID))
            {
                return BadRequest("Một hoặc nhiều ID liên quan (IssuingUnit, RelatedProject, RecipientGroup) không hợp lệ.");
            }

            var document = _mapper.Map<IncomingDocument>(creationDto);
            _context.IncomingDocuments.Add(document);
            await _context.SaveChangesAsync();

            var createdDocument = await GetFullDocumentsQuery().FirstOrDefaultAsync(d => d.Id == document.Id);
            var documentDto = _mapper.Map<IncomingDocumentDto>(createdDocument);

            return CreatedAtAction(nameof(GetIncomingDocument), new { id = documentDto.ID }, documentDto);
        }

        // PUT: api/incomingdocuments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncomingDocument(int id, IncomingDocumentForUpdateDto updateDto)
        {
            var documentFromDb = await _context.IncomingDocuments.FindAsync(id);
            if (documentFromDb == null)
            {
                return NotFound();
            }
            // (Tương tự POST, kiểm tra các ID khóa ngoại)

            _mapper.Map(updateDto, documentFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/incomingdocuments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncomingDocument(int id)
        {
            var document = await _context.IncomingDocuments.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            _context.IncomingDocuments.Remove(document);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
