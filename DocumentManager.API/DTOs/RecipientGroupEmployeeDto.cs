using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    // Dùng để gán hoặc hủy gán một nhân viên vào một nhóm
    public class AssignEmployeeToGroupDto
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public int GroupID { get; set; }
    }

}
