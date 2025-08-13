// File: API/Controllers/RelatedProjectsController.cs
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
    public class RelatedProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RelatedProjectsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RelatedProjectDto>>> GetRelatedProjects(
            [FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.RelatedProjects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(p => p.RelatedProjectName.Contains(searchQuery));
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(p => p.RelatedProjectName)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .AsNoTracking()
                                   .ToListAsync();

            var dtos = _mapper.Map<List<RelatedProjectDto>>(items);
            return Ok(new PagedResult<RelatedProjectDto>(dtos, totalCount, pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RelatedProjectDto>> GetRelatedProject(int id)
        {
            var relatedProject = await _context.RelatedProjects.FindAsync(id);
            if (relatedProject == null) return NotFound();
            return Ok(_mapper.Map<RelatedProjectDto>(relatedProject));
        }

        [HttpPost]
        public async Task<ActionResult<RelatedProjectDto>> PostRelatedProject(RelatedProjectForCreationDto creationDto)
        {
            var relatedProject = _mapper.Map<RelatedProject>(creationDto);
            _context.RelatedProjects.Add(relatedProject);
            await _context.SaveChangesAsync();
            var dto = _mapper.Map<RelatedProjectDto>(relatedProject);
            return CreatedAtAction(nameof(GetRelatedProject), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelatedProject(int id, RelatedProjectForUpdateDto updateDto)
        {
            var relatedProjectFromDb = await _context.RelatedProjects.FindAsync(id);
            if (relatedProjectFromDb == null) return NotFound();
            _mapper.Map(updateDto, relatedProjectFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelatedProject(int id)
        {
            var relatedProject = await _context.RelatedProjects.FindAsync(id);
            if (relatedProject == null) return NotFound();
            _context.RelatedProjects.Remove(relatedProject);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}