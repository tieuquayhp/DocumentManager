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
    public class RelatedProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RelatedProjectsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/RelatedProjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelatedProjectDto>>> GetRelatedProjects()
        {
            var relatedProjects = await _context.RelatedProjects.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RelatedProjectDto>>(relatedProjects));
        }
        // GET: api/RelatedProjects/id
        [HttpGet("{id}")]
        public async Task<ActionResult<RelatedProjectDto>> GetRelatedProject(int id)
        {
            var relatedProject = await _context.RelatedProjects.FindAsync(id);
            if (relatedProject == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RelatedProjectDto>(relatedProject));
        }
        // GET: api/RelatedProjects/search?query=searchTerm
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<RelatedProjectDto>>> SearchRelatedProjects([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var relatedProject = await _context.Departments
                .Where(d => d.DepartmentName.Contains(query))
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RelatedProjectDto>>(relatedProject));
        }
        // POST: api/RelatedProjects
        [HttpPost]
        public async Task<ActionResult<RelatedProjectDto>> PostRelatedProject(RelatedProjectForCreationDto creationDto)
        {
            var relatedProject = _mapper.Map<RelatedProject>(creationDto);
            _context.RelatedProjects.Add(relatedProject);
            await _context.SaveChangesAsync();
            var relatedProjectDto = _mapper.Map<RelatedProjectDto>(relatedProject);
            return CreatedAtAction(nameof(GetRelatedProject), new { id = relatedProjectDto.Id }, relatedProjectDto);
        }
        // PUT: api/RelatedProjects/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelatedProject(int id, RelatedProjectForUpdateDto updateDto)
        {
            var relatedProjectFromDb = await _context.Departments.FindAsync(id);
            if (relatedProjectFromDb == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, relatedProjectFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: api/RelatedProjects/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelatedProject(int id)
        {
            var relatedProject = await _context.RelatedProjects.FindAsync(id);
            if (relatedProject == null)
            {
                return NotFound();
            }
            _context.RelatedProjects.Remove(relatedProject);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
