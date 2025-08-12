using AutoMapper;
using DocumentManager.API.DTOs;
using DocumentManager.DAL.Data;
using DocumentManager.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DocumentManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public EmployeesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // HTTP GET api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            // QUAN TRỌNG: Nạp dữ liệu Department để map được DepartmentName
            var employees = await _context.Employees
                .Include(e => e.Department)
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }
        // GET: api/employees/id
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EmployeeDto>(employee));
        }
        // GET: api/employees/search?query=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> SearchEmployees([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Cần cung cấp từ khóa tìm kiếm.");
            }
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.EmployeeName.Contains(query) || e.Email.Contains(query))
                .AsNoTracking()
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }
        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(EmployeeForCreationDto creationDto)
        {
            if (!await _context.Departments.AnyAsync(d => d.Id == creationDto.DepartmentID))
            {
                return BadRequest($"Department với ID {creationDto.DepartmentID} không tồn tại.");
            }

            var employee = _mapper.Map<Employee>(creationDto);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Nạp lại dữ liệu Department để trả về DTO hoàn chỉnh
            await _context.Entry(employee).Reference(e => e.Department).LoadAsync();
            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return CreatedAtAction(nameof(GetEmployee), new { id = employeeDto.ID }, employeeDto);
        }
        // PUT: api/employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeForUpdateDto updateDto)
        {
            var employeeFromDb = await _context.Employees.FindAsync(id);
            if (employeeFromDb == null)
            {
                return NotFound();
            }
            if (!await _context.Departments.AnyAsync(d => d.Id == updateDto.DepartmentID))
            {
                return BadRequest($"Department với ID {updateDto.DepartmentID} không tồn tại.");
            }
            _mapper.Map(updateDto, employeeFromDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
