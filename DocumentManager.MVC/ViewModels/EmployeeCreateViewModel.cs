using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class EmployeeCreateViewModel
    {
        public int ID { get; set; } // Giữ lại cho chức năng Edit

        [Display(Name = "Tên Nhân Viên")]
        [Required(ErrorMessage = "Tên là bắt buộc.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phòng Ban")]
        public string DepartmentName { get; set; } = string.Empty; // Thay DepartmentID bằng DepartmentName
    }
}