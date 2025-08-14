using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class IncomingDocumentViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Số Tài Liệu Đến")]
        public string IncomingDocumentNumber { get; set; } = string.Empty;
        [Display(Name = "Ngày Phát Hành")]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "Mã Tài Liệu (Bên Gửi)")]
        public string DocumentCodeFromIssuer { get; set; } = string.Empty;
        [Display(Name = "Ngày Phát Hành (Bên Gửi)")]
        public DateTime ReleaseDateFromIssuer { get; set; }
        [Display(Name = "Nội dung")]
        public string DocumentContent { get; set; } = string.Empty;
        [Display(Name = "Tệp đính kèm")]
        public string DocumentFile { get; set; } = string.Empty;
        public int IssuingUnitID { get; set; }
        public int RelatedProjectID { get; set; }
        [Display(Name = "Đơn Vị Phát Hành")]
        public string IssuingUnitName { get; set; } = string.Empty;
        [Display(Name = "Dự Án Liên Quan")]
        public string RelatedProjectName { get; set; } = string.Empty;

        // Bỏ GroupName vì giờ có thể có nhiều nhóm
        // Thay vào đó, chúng ta có thể thêm một danh sách tên nhóm nếu cần hiển thị chi tiết
        [Display(Name = "Nhóm người nhận")]
        public List<string> RecipientGroupNames { get; set; } = new List<string>();
    }
}