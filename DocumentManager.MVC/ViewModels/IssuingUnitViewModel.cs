using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class IssuingUnitViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Đơn Vị Phát Hành")]
        [Required]
        public string IssuingUnitName { get; set; }
    }
}
