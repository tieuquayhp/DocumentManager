using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class RelatedProjectViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Dự Án Liên Quan")]
        [Required]
        public string RelatedProjectName { get; set; }
    }
}
