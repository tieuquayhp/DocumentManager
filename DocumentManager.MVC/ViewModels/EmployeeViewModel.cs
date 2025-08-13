using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class EmployeeViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Tên Nhân Viên")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phòng Ban")]
        public string DepartmentName { get; set; } = string.Empty;
    }
}