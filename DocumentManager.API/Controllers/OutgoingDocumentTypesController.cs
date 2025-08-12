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
    public class OutgoingDocumentTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public OutgoingDocumentTypesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/OutgoingDocumentTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OutgoingDocumentTypeDto>>> GetOutgoingDocumentTypes()
        {
            var outgoingDocumentTypes = await _context.OutgoingDocumentTypes.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<OutgoingDocumentTypeDto>>(outgoingDocumentTypes));
        }
        // GET: api/OutgoingDocumentTypes/id
        [HttpGet("{id}")]
        public async Task<ActionResult<OutgoingDocumentTypeDto>> GetOutgoingDocumentType(int id)
        {
            var outgoingDocumentType = await _context.OutgoingDocumentTypes.FindAsync(id);
            if (outgoingDocumentType == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OutgoingDocumentTypeDto>(outgoingDocumentType));
        }
        // GET: api/OutgoingDocumentTypes/search?query=searchTerm
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<OutgoingDocumentTypeDto>>> SearchOutgoingDocumentTypes([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var outgoingDocumentTypes = await _context.OutgoingDocumentTypes
                .Where(d => d.OutgoingDocumentTypeName.Contains(query))
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<OutgoingDocumentTypeDto>>(outgoingDocumentTypes));
        }
        // POST: api/OutgoingDocumentTypes
        [HttpPost]
        public async Task<ActionResult<OutgoingDocumentTypeDto>> PostOutgoingDocumentType(OutgoingDocumentTypeForCreationDto creationDto)
        {
            var outgoingDocumentType = _mapper.Map<OutgoingDocumentType>(creationDto);
            _context.OutgoingDocumentTypes.Add(outgoingDocumentType);
            await _context.SaveChangesAsync();
            var outgoingDocumentTypeDto = _mapper.Map<OutgoingDocumentTypeDto>(outgoingDocumentType);
            return CreatedAtAction(nameof(GetOutgoingDocumentType), new { id = outgoingDocumentTypeDto.ID }, outgoingDocumentTypeDto);
        }
        // PUT: api/OutgoingDocumentTypes/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutgoingDocumentType(int id, OutgoingDocumentTypeForUpdateDto updateDto)
        {
            var outgoingDocumentTypeFromDb = await _context.OutgoingDocumentTypes.FindAsync(id);
            if (outgoingDocumentTypeFromDb == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, outgoingDocumentTypeFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: api/OutgoingDocumentTypes/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutgoingDocumentType(int id)
        {
            var outgoingDocumentType = await _context.OutgoingDocumentTypes.FindAsync(id);
            if (outgoingDocumentType == null)
            {
                return NotFound();
            }
            _context.OutgoingDocumentTypes.Remove(outgoingDocumentType);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
