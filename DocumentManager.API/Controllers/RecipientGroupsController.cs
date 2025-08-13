// File: API/Controllers/RecipientGroupsController.cs
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
    public class RecipientGroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RecipientGroupsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RecipientGroupDto>>> GetRecipientGroups(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.RecipientGroups.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(rg => rg.RecipientGroupName.Contains(searchQuery));
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(rg => rg.RecipientGroupName)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .AsNoTracking()
                                   .ToListAsync();

            var dtos = _mapper.Map<List<RecipientGroupDto>>(items);
            return Ok(new PagedResult<RecipientGroupDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientGroupDto>> GetRecipientGroup(int id)
        {
            var recipientGroup = await _context.RecipientGroups.FindAsync(id);
            if (recipientGroup == null) return NotFound();
            return Ok(_mapper.Map<RecipientGroupDto>(recipientGroup));
        }

        [HttpPost]
        public async Task<ActionResult<RecipientGroupDto>> PostRecipientGroup(RecipientGroupForCreationDto creationDto)
        {
            var recipientGroup = _mapper.Map<RecipientGroup>(creationDto);
            _context.RecipientGroups.Add(recipientGroup);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<RecipientGroupDto>(recipientGroup);
            return CreatedAtAction(nameof(GetRecipientGroup), new { id = dto.ID }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipientGroup(int id, RecipientGroupForUpdateDto updateDto)
        {
            var recipientGroupFromDb = await _context.RecipientGroups.FindAsync(id);
            if (recipientGroupFromDb == null) return NotFound();
            _mapper.Map(updateDto, recipientGroupFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipientGroup(int id)
        {
            var recipientGroup = await _context.RecipientGroups.FindAsync(id);
            if (recipientGroup == null) return NotFound();
            _context.RecipientGroups.Remove(recipientGroup);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}