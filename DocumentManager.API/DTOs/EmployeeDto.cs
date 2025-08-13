// File: API/DTOs/EmployeeDto.cs
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    // Lớp DTO để hiển thị dữ liệu Employee
    public class EmployeeDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty; // Thuộc tính này vẫn đúng
    }

    // Lớp DTO để tạo mới Employee
    public class EmployeeForCreationDto
    {
        [Required(ErrorMessage = "Tên nhân viên là bắt buộc.")]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [StringLength(250)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        // Thêm trường DepartmentName, bỏ DepartmentID
        [StringLength(250)]
        public string DepartmentName { get; set; } = string.Empty;
    }

    // Lớp DTO để cập nhật Employee
    public class EmployeeForUpdateDto
    {
        [Required(ErrorMessage = "Tên nhân viên là bắt buộc.")]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [StringLength(250)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(250)]
        public string DepartmentName { get; set; } = string.Empty;
    }
}