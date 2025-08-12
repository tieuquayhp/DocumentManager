using AutoMapper;
using DocumentManager.API.DTOs;
using DocumentManager.API.Helpers;
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
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments([FromQuery] string? searchQuery,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            // Bắt đầu với một câu truy vấn cơ sở
            var query = _context.Departments.AsQueryable();

            // 1. Áp dụng bộ lọc tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(d => d.DepartmentName.Contains(searchQuery));
            }

            // 2. Lấy tổng số bản ghi (trước khi phân trang)
            var totalCount = await query.CountAsync();

            // 3. Áp dụng phân trang
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var departmentDtos = _mapper.Map<List<DepartmentDto>>(items);

            // 4. Tạo đối tượng PagedResult để trả về
            var pagedResult = new PagedResult<DepartmentDto>(departmentDtos, totalCount, pageNumber, pageSize);

            return Ok(pagedResult);
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
