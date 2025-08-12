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
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DepartmentsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments()
        {
            var departments = await _context.Departments.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DepartmentDto>>(departments));
        }
        // GET: api/Departments/id
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<DepartmentDto>(department));
        }
        // GET: api/Departments/search?query=searchTerm
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> SearchDepartments([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var departments = await _context.Departments
                .Where(d => d.DepartmentName.Contains(query))
                .AsNoTracking()
                .ToListAsync();
             return Ok(_mapper.Map<IEnumerable<DepartmentDto>>(departments));
        }
        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> PostDepartment(DepartmentForCreationDto creationDto)
        {
            var department = _mapper.Map<Department>(creationDto);
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return CreatedAtAction(nameof(GetDepartment), new { id = departmentDto.Id }, departmentDto);
        }
        // PUT: api/Departments/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, DepartmentForUpdateDto updateDto)
        {
            var departmentFromDb = await _context.Departments.FindAsync(id);
            if (departmentFromDb == null)
            {
                return NotFound();
            }
            _mapper.Map(updateDto, departmentFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // DELETE: api/Departments/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
