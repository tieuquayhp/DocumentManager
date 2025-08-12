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
    public class GroupManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GroupManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/group-management/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignEmployeeToGroup(AssignEmployeeToGroupDto assignDto)
        {
            // Kiểm tra sự tồn tại
            if (!await _context.Employees.AnyAsync(e => e.Id == assignDto.EmployeeID) ||
                !await _context.RecipientGroups.AnyAsync(g => g.Id == assignDto.GroupID))
            {
                return BadRequest("Employee hoặc Group không tồn tại.");
            }

            // Kiểm tra mối quan hệ đã có chưa
            var relationshipExists = await _context.RecipientGroupEmployees
                .AnyAsync(re => re.EmployeeID == assignDto.EmployeeID && re.RecipientGroupID == assignDto.GroupID);
            if (relationshipExists)
            {
                return Conflict("Nhân viên đã ở trong nhóm này."); // Conflict (409) hợp lý hơn BadRequest
            }

            var newRelationship = new RecipientGroupEmployee
            {
                EmployeeID = assignDto.EmployeeID,
                RecipientGroupID = assignDto.GroupID
            };

            _context.RecipientGroupEmployees.Add(newRelationship);
            await _context.SaveChangesAsync();

            return Ok("Gán nhân viên vào nhóm thành công.");
        }

        // POST: api/group-management/unassign
        [HttpPost("unassign")] // Sử dụng POST hoặc DELETE đều được
        public async Task<IActionResult> UnassignEmployeeFromGroup(AssignEmployeeToGroupDto assignDto)
        {
            var relationship = await _context.RecipientGroupEmployees
                .FirstOrDefaultAsync(re => re.EmployeeID == assignDto.EmployeeID && re.RecipientGroupID == assignDto.GroupID);

            if (relationship == null)
            {
                return NotFound("Không tìm thấy mối quan hệ này để hủy.");
            }

            _context.RecipientGroupEmployees.Remove(relationship);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/group-management/group/{groupId}/employees
        [HttpGet("group/{groupId}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesInGroup(int groupId)
        {
            var employees = await _context.RecipientGroupEmployees
               .Where(re => re.RecipientGroupID == groupId)
               .Include(re => re.Employee) // Nạp thông tin nhân viên
                   .ThenInclude(e => e.Department) // Nạp cả phòng ban của nhân viên
               .Select(re => re.Employee) // Chỉ chọn đối tượng Employee
               .AsNoTracking()
               .ToListAsync();

            // Cần một IMapper ở đây để chuyển đổi Employee -> EmployeeDto
            // Để đơn giản, ví dụ này sẽ bỏ qua bước đó, nhưng trong thực tế bạn nên tiêm IMapper vào
            return Ok(employees); // Nên map sang EmployeeDto
        }
    }
}
