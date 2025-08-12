using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class DepartmentViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Tên Phòng Ban")]
        [Required(ErrorMessage = "Tên phòng ban là bắt buộc.")]
        [StringLength(250)]
        public string DepartmentName { get; set; }

    }
}
