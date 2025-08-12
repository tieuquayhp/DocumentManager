using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class EmployeeDto
    {
        public int ID { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; } // Thêm tên phòng ban để tiện hiển thị
    }
    // Dùng để tạo mới (Create)
    public class EmployeeForCreationDto
    {
        [Required(ErrorMessage = "Tên nhân viên là bắt buộc.")]
        [StringLength(250)]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [StringLength(250)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ID phòng ban là bắt buộc.")]
        public int DepartmentID { get; set; }
    }

    // Dùng để cập nhật (Update)
    public class EmployeeForUpdateDto
    {
        [Required(ErrorMessage = "Tên nhân viên là bắt buộc.")]
        [StringLength(250)]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [StringLength(250)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ID phòng ban là bắt buộc.")]
        public int DepartmentID { get; set; }
    }
}
