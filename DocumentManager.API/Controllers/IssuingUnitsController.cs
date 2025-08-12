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
    public class IssuingUnitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public IssuingUnitsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/IssuingUnits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IssuingUnitDto>>> GetIssuingUnits()
        {
            var issuingUnits = await _context.IssuingUnits.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<IssuingUnitDto>>(issuingUnits));
        }
        // GET: api/IssuingUnits/id
        [HttpGet("{id}")]
        public async Task<ActionResult<IssuingUnitDto>> GetIssuingUnit(int id)
        {
            var issuingUnit = await _context.IssuingUnits.FindAsync(id);
            if (issuingUnit == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IssuingUnitDto>(issuingUnit));
        }
        // GET: api/IssuingUnits/search?query=searchTerm
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<IssuingUnitDto>>> SearchIssuingUnits([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var issuingUnits = await _context.IssuingUnits
                .Where(iu => iu.IssuingUnitName.Contains(query))
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<IssuingUnitDto>>(issuingUnits));
        }
        // POST: api/IssuingUnits
        [HttpPost]
        public async Task<ActionResult<IssuingUnitDto>> PostIssuingUnit(IssuingUnitForCreationDto creationDto)
        {
            var issuingUnit = _mapper.Map<IssuingUnit>(creationDto);
            _context.IssuingUnits.Add(issuingUnit);
            var issuingUnitDto = _mapper.Map<IssuingUnitDto>(issuingUnit);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIssuingUnit), new { id = issuingUnit.Id }, _mapper.Map<IssuingUnitDto>(issuingUnit));
        }
        // PUT: api/IssuingUnits/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssuingUnit(int id, IssuingUnitForUpdateDto updateDto)
        {
            var issuingUnitFromDB = await _context.IssuingUnits.FindAsync(id);
            if (issuingUnitFromDB == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, issuingUnitFromDB);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: api/IssuingUnits/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssuingUnit(int id)
        {
            var issuingUnit = await _context.IssuingUnits.FindAsync(id);
            if (issuingUnit == null)
            {
                return NotFound();
            }
            _context.IssuingUnits.Remove(issuingUnit);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
