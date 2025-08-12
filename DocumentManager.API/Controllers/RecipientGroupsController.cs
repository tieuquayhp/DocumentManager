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
    public class RecipientGroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RecipientGroupsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/RecipientGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipientGroupDto>>> GetRecipientGroups()
        {
            var recipientGroups = await _context.RecipientGroups.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RecipientGroupDto>>(recipientGroups));
        }
        // GET: api/RecipientGroups/id
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipientGroupDto>> GetRecipientGroup(int id)
        {
            var recipientGroup = await _context.RecipientGroups.FindAsync(id);
            if (recipientGroup == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RecipientGroupDto>(recipientGroup));
        }
        // GET: api/RecipientGroups/search?query=searchTerm
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RecipientGroupDto>>> SearchRecipientGroups([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var recipientGroups = await _context.RecipientGroups
                .Where(rg => rg.RecipientGroupName.Contains(query))
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RecipientGroupDto>>(recipientGroups));
        }
        // POST: api/RecipientGroups
        [HttpPost]
        public async Task<ActionResult<RecipientGroupDto>> PostRecipientGroup(RecipientGroupForCreationDto creationDto)
        {
            var recipientGroup = _mapper.Map<RecipientGroup>(creationDto);
            _context.RecipientGroups.Add(recipientGroup);
            var recipientGroupDto = _mapper.Map<RecipientGroupDto>(recipientGroup);
            return CreatedAtAction(nameof(GetRecipientGroup), new { id = recipientGroup.Id }, recipientGroup);
        }
        // PUT: api/RecipientGroups/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipientGroup(int id, RecipientGroupForUpdateDto updateDto)
        {
            var recipientGroupFromDb = await _context.RecipientGroups.FindAsync(id);
            if (recipientGroupFromDb == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, recipientGroupFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: api/RecipientGroups/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipientGroup(int id)
        {
            var recipientGroup = await _context.RecipientGroups.FindAsync(id);
            if (recipientGroup == null)
            {
                return NotFound();
            }
            _context.RecipientGroups.Remove(recipientGroup);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
