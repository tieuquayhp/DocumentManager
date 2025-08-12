using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class RecipientGroupViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Nhóm Nhận")]
        [Required]
        public string GroupName { get; set; }

    }
}
