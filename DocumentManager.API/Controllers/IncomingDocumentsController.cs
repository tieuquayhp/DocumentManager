// File: API/Controllers/IncomingDocumentsController.cs
using AutoMapper;
using DocumentManager.API.DTOs;
using DocumentManager.API.Helpers;
using DocumentManager.DAL.Data;
using DocumentManager.DAL.Models;
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
        private readonly IWebHostEnvironment _env;

        public IncomingDocumentsController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        private IQueryable<IncomingDocument> GetFullDocumentsQuery()
        {
            return _context.IncomingDocuments
                .Include(d => d.IssuingUnit)
                .Include(d => d.RelatedProject)
                .Include(d => d.IncomingDocumentRecipientGroups)
                .ThenInclude(idrg => idrg.RecipientGroup)
                .AsNoTracking();
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<IncomingDocumentDto>>> GetIncomingDocuments(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? limit = null)
        {
            var query = GetFullDocumentsQuery();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(d => d.IncomingDocumentNumber.ToString().Contains(searchQuery) || d.DocumentContent.Contains(searchQuery));
            }

            query = query.OrderByDescending(d => d.ReleaseDate);

            if (limit.HasValue && limit > 0)
            {
                var limitedItems = await query.Take(limit.Value).ToListAsync();
                return Ok(_mapper.Map<List<IncomingDocumentDto>>(limitedItems));
            }

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var dtos = _mapper.Map<List<IncomingDocumentDto>>(items);
            return Ok(new PagedResult<IncomingDocumentDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IncomingDocumentDto>> GetIncomingDocument(int id)
        {
            var document = await GetFullDocumentsQuery().FirstOrDefaultAsync(d => d.Id == id);
            if (document == null) return NotFound();
            return Ok(_mapper.Map<IncomingDocumentDto>(document));
        }


        [HttpPost]
        public async Task<ActionResult<IncomingDocumentDto>> PostIncomingDocument([FromForm] IncomingDocumentForCreationDto creationDto,
IFormFile file) // Nhận file như một tham số riêng
        {// --- BƯỚC 1: VALIDATION ---
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError(nameof(file), "File đính kèm là bắt buộc.");
            }
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- BƯỚC 2: LƯU FILE ---
            string fileUrl;
            try
            {
                var uploadsFolderPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                fileUrl = $"/uploads/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server khi lưu file: {ex.Message}");
            }

            // --- BƯỚC 3: TẠO BẢN GHI TRONG CSDL ---
            var document = _mapper.Map<IncomingDocument>(creationDto);
            document.DocumentFile = fileUrl; // Gán đường dẫn file đã lưu

            // Lấy danh sách ID từ chính DTO
            foreach (var groupId in creationDto.RecipientGroupIDs)
            {
                document.IncomingDocumentRecipientGroups.Add(new IncomingDocumentRecipientGroup { RecipientGroupID = groupId });
            }

            _context.IncomingDocuments.Add(document);
            await _context.SaveChangesAsync();

            // Lấy lại dữ liệu để trả về DTO hoàn chỉnh
            var createdDocumentWithDetails = await GetFullDocumentsQuery()
.FirstOrDefaultAsync(d => d.Id == document.Id);
            //await _context.Entry(document).Reference(d => d.IssuingUnit).LoadAsync();
            //await _context.Entry(document).Reference(d => d.RelatedProject).LoadAsync();

            var dto = _mapper.Map<IncomingDocumentDto>(createdDocumentWithDetails);
            return CreatedAtAction(nameof(GetIncomingDocument), new { id = dto.ID }, dto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncomingDocument(int id, [FromForm] IncomingDocumentForUpdateDto updateDto, [FromForm] List<int> recipientGroupIDs,IFormFile? file)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // THÊM VALIDATION NÀY
            if (!await _context.IssuingUnits.AnyAsync(i => i.Id == updateDto.IssuingUnitID) ||
                !await _context.RelatedProjects.AnyAsync(p => p.Id == updateDto.RelatedProjectID))
            {
                return BadRequest("IssuingUnitID hoặc RelatedProjectID không hợp lệ.");
            }
            // BƯỚC 1: TÌM ĐỐI TƯỢNG CẦN CẬP NHẬT (Không cần Include nữa)
            var documentFromDb = await _context.IncomingDocuments.FindAsync(id);
if (documentFromDb == null)
            {
                return NotFound($"Không tìm thấy tài liệu với ID {id}.");
            }

            // BƯỚC 2: XỬ LÝ FILE ĐÍNH KÈM (Giữ nguyên logic cũ)
            string? newFileUrl = null;
            if (file != null && file.Length > 0)
            {
                try
                {
                    if (!string.IsNullOrEmpty(documentFromDb.DocumentFile))
                    {
                        var oldFilePath = Path.Combine(_env.WebRootPath, documentFromDb.DocumentFile.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    // ... (Logic lưu file mới)
                    var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(_env.WebRootPath, "uploads", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create)) { await file.CopyToAsync(stream); }
                    newFileUrl = $"/uploads/{uniqueFileName}";
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Lỗi server khi cập nhật file: {ex.Message}");
                }
            }

            // BƯỚC 3: CẬP NHẬT DỮ LIỆU TỪ DTO VÀO ĐỐI TƯỢNG TỪ DB
            _mapper.Map(updateDto, documentFromDb);
            if (newFileUrl != null)
            {
                documentFromDb.DocumentFile = newFileUrl;
            }

            // --- BƯỚC 4: CẬP NHẬT QUAN HỆ NHIỀU-NHIỀU (LOGIC MỚI) ---

            // 4a. Lấy tất cả các mối quan hệ hiện có của tài liệu này
            var existingRelations = _context.IncomingDocumentRecipientGroups
                .Where(r => r.IncomingDocumentID == id);

            // 4b. Xóa chúng đi
            _context.IncomingDocumentRecipientGroups.RemoveRange(existingRelations);

            // 4c. Thêm lại các mối quan hệ mới từ danh sách ID người dùng đã chọn
            if (recipientGroupIDs != null && recipientGroupIDs.Any())
            {
                var newRelations = recipientGroupIDs.Select(groupId => new IncomingDocumentRecipientGroup
                {
                    IncomingDocumentID = id,
                    RecipientGroupID = groupId
                });
                await _context.IncomingDocumentRecipientGroups.AddRangeAsync(newRelations);
            }

            // BƯỚC 5: LƯU THAY ĐỔI
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Xử lý trường hợp đối tượng đã bị người khác sửa đổi
                if (!_context.IncomingDocuments.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về 204 No Content là chuẩn cho một yêu cầu PUT thành công
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncomingDocument(int id)
        {
            // BƯỚC 1: TÌM BẢN GHI CẦN XÓA
            var document = await _context.IncomingDocuments.FindAsync(id);
            if (document == null)
            {
                return NotFound($"Không tìm thấy tài liệu với ID {id} để xóa.");
            }

            var filePathToDelete = document.DocumentFile;

            try
            {
                // --- BƯỚC 2 (MỚI): XÓA CÁC BẢN GHI PHỤ THUỘC TRONG BẢNG NỐI ---
                var relationsToDelete = _context.IncomingDocumentRecipientGroups
                    .Where(r => r.IncomingDocumentID == id);

                if (relationsToDelete.Any())
                {
                    _context.IncomingDocumentRecipientGroups.RemoveRange(relationsToDelete);
                }

                // --- BƯỚC 3: XÓA BẢN GHI CHA ---
                _context.IncomingDocuments.Remove(document);

                // --- BƯỚC 4: LƯU TẤT CẢ THAY ĐỔI VÀO CSDL ---
                // EF Core sẽ thực hiện DELETE từ bảng nối trước, sau đó mới DELETE từ bảng chính
                await _context.SaveChangesAsync();

                // --- BƯỚC 5: XÓA FILE VẬT LÝ SAU KHI XÓA CSDL THÀNH CÔNG ---
                if (!string.IsNullOrEmpty(filePathToDelete))
                {
                    var physicalPath = Path.Combine(_env.WebRootPath, filePathToDelete.TrimStart('/'));
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có vấn đề
                return StatusCode(500, $"Lỗi server khi xóa tài liệu: {ex.Message}");
            }

            return NoContent();
        }
    }
}