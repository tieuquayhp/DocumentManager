using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class EmployeeCreateViewModel
    {
        public int ID { get; set; } // Cần cho việc Edit

        [Display(Name = "Tên Nhân Viên")]
        [Required(ErrorMessage = "Tên là bắt buộc.")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phòng Ban")]
        [Required(ErrorMessage = "Vui lòng chọn phòng ban.")]
        public int DepartmentID { get; set; }

        // Thuộc tính này dùng để chứa danh sách các phòng ban cho dropdown list
        // Controller sẽ điền dữ liệu vào đây từ API.
        public IEnumerable<SelectListItem>? Departments { get; set; }
    }
}
