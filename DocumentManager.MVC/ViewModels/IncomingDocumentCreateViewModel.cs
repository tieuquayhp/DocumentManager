using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class IncomingDocumentCreateViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Số Tài Liệu Đến")]
        [Required]
        public string IncomingDocumentNumber { get; set; }

        [Display(Name = "Ngày Phát Hành")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Mã Tài Liệu (Bên Gửi)")]
        public string DocumentCodeFromIssuer { get; set; }

        [Display(Name = "Ngày Phát Hành (Bên Gửi)")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDateFromIssuer { get; set; }

        [Display(Name = "Nội dung")]
        [Required]
        public string DocumentContent { get; set; }

        [Display(Name = "Tệp đính kèm")]
        public string DocumentFile { get; set; }

        // --- Foreign Keys ---
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
        public IEnumerable<SelectListItem>? IssuingUnits { get; set; }
        public IEnumerable<SelectListItem>? RelatedProjects { get; set; }
        public IEnumerable<SelectListItem>? RecipientGroups { get; set; }
    }
}
