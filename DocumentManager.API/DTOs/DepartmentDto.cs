using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
    }
    // Dùng để tạo mới (Create)
    public class DepartmentForCreationDto
    {
        [Required(ErrorMessage = "Tên phòng ban là bắt buộc.")]
        [StringLength(250, ErrorMessage = "Tên phòng ban không được vượt quá 250 ký tự.")]
        public string DepartmentName { get; set; }
    }

    // Dùng để cập nhật (Update)
    public class DepartmentForUpdateDto
    {
        [Required(ErrorMessage = "Tên phòng ban là bắt buộc.")]
        [StringLength(250, ErrorMessage = "Tên phòng ban không được vượt quá 250 ký tự.")]
        public string DepartmentName { get; set; }
    }
}
