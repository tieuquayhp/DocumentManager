// File: API/Controllers/IssuingUnitsController.cs
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
    public class IssuingUnitsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IssuingUnitsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<IssuingUnitDto>>> GetIssuingUnits(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.IssuingUnits.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(iu => iu.IssuingUnitName.Contains(searchQuery));
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(iu => iu.IssuingUnitName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var dtos = _mapper.Map<List<IssuingUnitDto>>(items);
            return Ok(new PagedResult<IssuingUnitDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IssuingUnitDto>> GetIssuingUnit(int id)
        {
            var issuingUnit = await _context.IssuingUnits.FindAsync(id);
            if (issuingUnit == null) return NotFound();
            return Ok(_mapper.Map<IssuingUnitDto>(issuingUnit));
        }

        [HttpPost]
        public async Task<ActionResult<IssuingUnitDto>> PostIssuingUnit(IssuingUnitForCreationDto creationDto)
        {
            var issuingUnit = _mapper.Map<IssuingUnit>(creationDto);
            _context.IssuingUnits.Add(issuingUnit);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<IssuingUnitDto>(issuingUnit);
            return CreatedAtAction(nameof(GetIssuingUnit), new { id = dto.ID }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssuingUnit(int id, IssuingUnitForUpdateDto updateDto)
        {
            var issuingUnitFromDb = await _context.IssuingUnits.FindAsync(id);
            if (issuingUnitFromDb == null) return NotFound();
            _mapper.Map(updateDto, issuingUnitFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssuingUnit(int id)
        {
            var issuingUnit = await _context.IssuingUnits.FindAsync(id);
            if (issuingUnit == null) return NotFound();
            _context.IssuingUnits.Remove(issuingUnit);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}