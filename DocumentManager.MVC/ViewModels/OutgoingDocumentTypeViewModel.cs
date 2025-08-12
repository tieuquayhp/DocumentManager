using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentTypeViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Loại Văn bản Đi")]
        [Required]
        public string TypeName { get; set; }
    }
}
