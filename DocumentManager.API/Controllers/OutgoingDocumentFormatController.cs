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
    public class OutgoingDocumentFormatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OutgoingDocumentFormatController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // HTTP GET api/outgoingdocumentformat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutgoingDocumentFormatDto>>> GetOutgoingDocumentFormats()
        {
            var formats = await _context.OutgoingDocumentFormats
                .Include(f => f.OutgoingDocumentType)
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<OutgoingDocumentFormatDto>>(formats));
        }
        // GET: api/outgoingdocumentformat/id
        [HttpGet("{id}")]
        public async Task<ActionResult<OutgoingDocumentFormatDto>> GetOutgoingDocumentFormat(int id)
        {
            var format = await _context.OutgoingDocumentFormats
                .Include(f => f.OutgoingDocumentType)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
            if (format == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OutgoingDocumentFormatDto>(format));
        }
        // GET: api/outgoingdocumentformat/search?query=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OutgoingDocumentFormatDto>>> SearchOutgoingDocumentFormats([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var formats = await _context.OutgoingDocumentFormats
                .Include(f => f.OutgoingDocumentType)
                .Where(f => f.OutgoingDocumentFormatName.Contains(query))
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<OutgoingDocumentFormatDto>>(formats));
        }
        // POST: api/outgoingdocumentformat
        [HttpPost]
        public async Task<ActionResult<OutgoingDocumentFormatDto>> PostOutgoingDocumentFormat(OutgoingDocumentFormatDto formatDto)
        {
            if (!await _context.OutgoingDocumentTypes.AnyAsync(t => t.Id == formatDto.OutgoingDocumentTypeId))
            {
                return BadRequest($"Loại văn bản đi với ID {formatDto.OutgoingDocumentTypeId} không tồn tại.");
            }
            var format = _mapper.Map<OutgoingDocumentFormat>(formatDto);
            _context.OutgoingDocumentFormats.Add(format);
            await _context.SaveChangesAsync();
            //Nạp lại dữ liệu OutgoingDocumentType để trả về DTO hoàn chỉnh
            await _context.Entry(format)
                .Reference(f => f.OutgoingDocumentType)
                .LoadAsync();
            var outgoingDocumentFormatDto = _mapper.Map<OutgoingDocumentFormatDto>(format);
            return CreatedAtAction(nameof(GetOutgoingDocumentFormat), new { id = outgoingDocumentFormatDto.Id }, outgoingDocumentFormatDto);
        }
        // PUT: api/outgoingdocumentformat/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutgoingDocumentFormat(int id, OutgoingDocumentFormatDto updateDto)
        {
            var outgoingDocumentFormatFromDb = await _context.OutgoingDocumentFormats.FindAsync(id);
            if (outgoingDocumentFormatFromDb == null)
            {
                return NotFound();
            }
            if (!await _context.OutgoingDocumentTypes.AnyAsync(t => t.Id == updateDto.OutgoingDocumentTypeId))
            {
                return BadRequest($"Loại văn bản đi với ID {updateDto.OutgoingDocumentTypeId} không tồn tại.");
            }
            _mapper.Map(updateDto, outgoingDocumentFormatFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: api/outgoingdocumentformat/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutgoingDocumentFormat(int id)
        {
            var outgoingDocumentFormat = await _context.OutgoingDocumentFormats.FindAsync(id);
            if (outgoingDocumentFormat == null)
            {
                return NotFound();
            }
            _context.OutgoingDocumentFormats.Remove(outgoingDocumentFormat);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
