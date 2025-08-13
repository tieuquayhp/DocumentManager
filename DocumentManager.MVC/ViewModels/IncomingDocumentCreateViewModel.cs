using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class IncomingDocumentCreateViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Số Tài Liệu Đến")]
        [Required]
        public string IncomingDocumentNumber { get; set; } = string.Empty;

        // ... (các thuộc tính khác giữ nguyên)

        [Display(Name = "Đơn Vị Phát Hành")]
        [Required]
        public int IssuingUnitID { get; set; }

        [Display(Name = "Dự Án Liên Quan")]
        [Required]
        public int RelatedProjectID { get; set; }

        // --- SỬA ĐỔI QUAN TRỌNG ---
        // Thay thế RecipientGroupID bằng một danh sách các ID được chọn
        [Display(Name = "Các Nhóm Nhận")]
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một nhóm nhận.")]
        public List<int> SelectedRecipientGroupIDs { get; set; } = new List<int>();

        // --- Lists for Dropdowns / Multi-select ---
        public IEnumerable<SelectListItem>? IssuingUnits { get; set; }
        public IEnumerable<SelectListItem>? RelatedProjects { get; set; }
        // Đổi tên để rõ ràng hơn, dùng cho multi-select box
        public IEnumerable<SelectListItem>? AllRecipientGroups { get; set; }
    }
}