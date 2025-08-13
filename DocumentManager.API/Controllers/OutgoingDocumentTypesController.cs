// File: API/Controllers/OutgoingDocumentTypesController.cs
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
    public class OutgoingDocumentTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OutgoingDocumentTypesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OutgoingDocumentTypeDto>>> GetOutgoingDocumentTypes(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.OutgoingDocumentTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(t => t.OutgoingDocumentTypeName.Contains(searchQuery));
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(t => t.OutgoingDocumentTypeName)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .AsNoTracking()
                                   .ToListAsync();

            var dtos = _mapper.Map<List<OutgoingDocumentTypeDto>>(items);
            return Ok(new PagedResult<OutgoingDocumentTypeDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OutgoingDocumentTypeDto>> GetOutgoingDocumentType(int id)
        {
            var docType = await _context.OutgoingDocumentTypes.FindAsync(id);
            if (docType == null) return NotFound();
            return Ok(_mapper.Map<OutgoingDocumentTypeDto>(docType));
        }

        [HttpPost]
        public async Task<ActionResult<OutgoingDocumentTypeDto>> PostOutgoingDocumentType(OutgoingDocumentTypeForCreationDto creationDto)
        {
            var docType = _mapper.Map<OutgoingDocumentType>(creationDto);
            _context.OutgoingDocumentTypes.Add(docType);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<OutgoingDocumentTypeDto>(docType);
            return CreatedAtAction(nameof(GetOutgoingDocumentType), new { id = dto.ID }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutgoingDocumentType(int id, OutgoingDocumentTypeForUpdateDto updateDto)
        {
            var docTypeFromDb = await _context.OutgoingDocumentTypes.FindAsync(id);
            if (docTypeFromDb == null) return NotFound();
            _mapper.Map(updateDto, docTypeFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutgoingDocumentType(int id)
        {
            var docType = await _context.OutgoingDocumentTypes.FindAsync(id);
            if (docType == null) return NotFound();
            _context.OutgoingDocumentTypes.Remove(docType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}