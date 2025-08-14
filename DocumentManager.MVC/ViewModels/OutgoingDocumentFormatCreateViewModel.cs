using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentFormatCreateViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Kiểu công văn đi")]
        [Required]
        public string OutgoingDocumentFormatName { get; set; }

        [Display(Name = "Loại Công văn đi")]
        [Required]
        public int OutgoingDocumentTypeID { get; set; }

        // Danh sách cho dropdown list
        public IEnumerable<SelectListItem>? OutgoingDocumentType { get; set; }
    }
}
