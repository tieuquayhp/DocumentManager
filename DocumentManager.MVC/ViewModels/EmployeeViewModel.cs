using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class EmployeeViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Tên Nhân Viên")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public int DepartmentID { get; set; }
        [Display(Name = "Phòng Ban")]
        public string DepartmentName { get; set; } // Hiển thị tên thay vì ID
    }
}
