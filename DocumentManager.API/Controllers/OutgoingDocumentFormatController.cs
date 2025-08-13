// File: API/Controllers/OutgoingDocumentFormatsController.cs
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
    public class OutgoingDocumentFormatsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OutgoingDocumentFormatsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OutgoingDocumentFormatDto>>> GetOutgoingDocumentFormats(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.OutgoingDocumentFormats.Include(f => f.OutgoingDocumentType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(f => f.OutgoingDocumentFormatName.Contains(searchQuery));
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(f => f.OutgoingDocumentFormatName)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .AsNoTracking()
                                   .ToListAsync();

            var dtos = _mapper.Map<List<OutgoingDocumentFormatDto>>(items);
            return Ok(new PagedResult<OutgoingDocumentFormatDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OutgoingDocumentFormatDto>> GetOutgoingDocumentFormat(int id)
        {
            var format = await _context.OutgoingDocumentFormats
                .Include(f => f.OutgoingDocumentType)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);
            if (format == null) return NotFound();
            return Ok(_mapper.Map<OutgoingDocumentFormatDto>(format));
        }

        [HttpPost]
        public async Task<ActionResult<OutgoingDocumentFormatDto>> PostOutgoingDocumentFormat(OutgoingDocumentFormatForCreationDto creationDto)
        {
            if (!await _context.OutgoingDocumentTypes.AnyAsync(t => t.Id == creationDto.OutgoingDocumentTypeId))
                return BadRequest($"Loại tài liệu đi với ID {creationDto.OutgoingDocumentTypeId} không tồn tại.");

            var format = _mapper.Map<OutgoingDocumentFormat>(creationDto);
            _context.OutgoingDocumentFormats.Add(format);
            await _context.SaveChangesAsync();

            await _context.Entry(format).Reference(f => f.OutgoingDocumentType).LoadAsync();
            var dto = _mapper.Map<OutgoingDocumentFormatDto>(format);
            return CreatedAtAction(nameof(GetOutgoingDocumentFormat), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutgoingDocumentFormat(int id, OutgoingDocumentFormatForUpdateDto updateDto)
        {
            if (!await _context.OutgoingDocumentTypes.AnyAsync(t => t.Id == updateDto.OutgoingDocumentTypeId))
                return BadRequest($"Loại tài liệu đi với ID {updateDto.OutgoingDocumentTypeId} không tồn tại.");

            var formatFromDb = await _context.OutgoingDocumentFormats.FindAsync(id);
            if (formatFromDb == null) return NotFound();

            _mapper.Map(updateDto, formatFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutgoingDocumentFormat(int id)
        {
            var format = await _context.OutgoingDocumentFormats.FindAsync(id);
            if (format == null) return NotFound();
            _context.OutgoingDocumentFormats.Remove(format);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}