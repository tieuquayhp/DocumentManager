using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentFormatCreateViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Tên Định Dạng")]
        [Required]
        public string FormatName { get; set; }

        [Display(Name = "Loại Tài Liệu")]
        [Required]
        public int TypeID { get; set; }

        // Danh sách cho dropdown list
        public IEnumerable<SelectListItem>? DocumentTypes { get; set; }
    }
}
