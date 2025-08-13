// File: API/Controllers/GroupManagementController.cs
using AutoMapper;
using DocumentManager.API.DTOs;
using DocumentManager.DAL.Data;
using DocumentManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentManager.API.Controllers
{
    [Route("api/group-management")]
    [ApiController]
    public class GroupManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GroupManagementController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignEmployeeToGroup(AssignEmployeeToGroupDto assignDto)
        {
            if (!await _context.Employees.AnyAsync(e => e.ID == assignDto.EmployeeID) ||
                !await _context.RecipientGroups.AnyAsync(g => g.Id == assignDto.GroupID))
            {
                return BadRequest("Employee hoặc Group không tồn tại.");
            }

            var relationshipExists = await _context.RecipientGroupEmployees
                .AnyAsync(re => re.EmployeeID == assignDto.EmployeeID && re.RecipientGroupID == assignDto.GroupID);
            if (relationshipExists)
            {
                return Conflict("Nhân viên đã ở trong nhóm này.");
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

        [HttpPost("unassign")]
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

        [HttpGet("group/{groupId}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesInGroup(int groupId)
        {
            var employees = await _context.RecipientGroupEmployees
               .Where(re => re.RecipientGroupID == groupId)
               .Include(re => re.Employee)
               .Select(re => re.Employee)
               .AsNoTracking()
               .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }
    }
}