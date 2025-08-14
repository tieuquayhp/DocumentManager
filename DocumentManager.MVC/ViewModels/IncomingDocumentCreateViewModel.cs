using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class IncomingDocumentCreateViewModel
    {
        public int ID { get; set; } // Sẽ không được dùng khi tạo mới

        [Display(Name = "Số Tài Liệu Đến")]
        [Required(ErrorMessage = "Số tài liệu đến là bắt buộc.")]
        public string IncomingDocumentNumber { get; set; } = string.Empty;

        [Display(Name = "Ngày Phát Hành")]
        [Required(ErrorMessage = "Ngày phát hành là bắt buộc.")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } = DateTime.Today;

        [Display(Name = "Mã Tài Liệu (Bên Gửi)")]
        public string? DocumentCodeFromIssuer { get; set; }

        [Display(Name = "Ngày Phát Hành (Bên Gửi)")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDateFromIssuer { get; set; }

        [Display(Name = "Nội dung / Trích yếu")]
        [Required(ErrorMessage = "Nội dung là bắt buộc.")]
        [DataType(DataType.MultilineText)]
        public string DocumentContent { get; set; } = string.Empty;

        // Tạm thời để dưới dạng text, sẽ nâng cấp thành upload file sau
        [Display(Name = "Tệp đính kèm")]
        public string? DocumentFile { get; set; }

        // --- Foreign Keys ---
        [Display(Name = "Đơn Vị Phát Hành")]
        [Required(ErrorMessage = "Vui lòng chọn đơn vị phát hành.")]
        public int IssuingUnitID { get; set; }

        [Display(Name = "Dự Án Liên Quan")]
        [Required(ErrorMessage = "Vui lòng chọn dự án.")]
        public int RelatedProjectID { get; set; }

        // Dùng để nhận danh sách các ID được chọn từ form
        [Display(Name = "Các Nhóm Nhận")]
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một nhóm nhận.")]
        public List<int> SelectedRecipientGroupIDs { get; set; } = new List<int>();

        // --- Thuộc tính để điền dữ liệu cho các control trên View ---
        public IEnumerable<SelectListItem>? IssuingUnits { get; set; }
        public IEnumerable<SelectListItem>? RelatedProjects { get; set; }
        public IEnumerable<SelectListItem>? AllRecipientGroups { get; set; }
    }
}