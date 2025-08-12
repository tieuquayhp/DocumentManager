using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class IncomingDocumentViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Số Tài Liệu Đến")]
        public string IncomingDocumentNumber { get; set; }
        [Display(Name = "Ngày Phát Hành")]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "Mã Tài Liệu (Bên Gửi)")]
        public string DocumentCodeFromIssuer { get; set; }
        [Display(Name = "Ngày Phát Hành (Bên Gửi)")]
        public DateTime ReleaseDateFromIssuer { get; set; }
        [Display(Name = "Nội dung")]
        public string DocumentContent { get; set; }
        [Display(Name = "Tệp đính kèm")]
        public string DocumentFile { get; set; }
        [Display(Name = "Đơn Vị Phát Hành")]
        public string IssuingUnitName { get; set; }
        [Display(Name = "Dự Án Liên Quan")]
        public string ProjectName { get; set; }
        [Display(Name = "Nhóm Nhận")]
        public string GroupName { get; set; }
    }
}
