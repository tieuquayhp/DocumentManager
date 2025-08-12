using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentCreateViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Số Tài Liệu Đi")]
        [Required]
        public string OutgoingDocumentNumber { get; set; }

        [Display(Name = "Ngày Phát Hành")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Nội dung")]
        [Required]
        public string DocumentContent { get; set; }

        [Display(Name = "Tệp đính kèm")]
        public string DocumentFile { get; set; }

        // --- Foreign Keys ---
        [Display(Name = "Loại Tài Liệu")]
        [Required]
        public int OutgoingDocumentTypeID { get; set; }

        [Display(Name = "Định Dạng")]
        [Required]
        public int OutgoingDocumentFormatID { get; set; }

        [Display(Name = "Đơn Vị Phát Hành")]
        [Required]
        public int IssuingUnitID { get; set; }

        [Display(Name = "Dự Án Liên Quan")]
        [Required]
        public int RelatedProjectID { get; set; }

        [Display(Name = "Nhóm Nhận")]
        [Required]
        public int RecipientGroupID { get; set; }

        // --- Lists for Dropdowns ---
        public IEnumerable<SelectListItem>? OutgoingDocumentTypes { get; set; }
        public IEnumerable<SelectListItem>? OutgoingDocumentFormats { get; set; }
        public IEnumerable<SelectListItem>? IssuingUnits { get; set; }
        public IEnumerable<SelectListItem>? RelatedProjects { get; set; }
        public IEnumerable<SelectListItem>? RecipientGroups { get; set; }
    }
}
