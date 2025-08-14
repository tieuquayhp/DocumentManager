using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentFormatViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Tên Kiểu văn bản")]
        public string OutgoingDocumentFormatName { get; set; }
        public int OutgoingDocumentTypeID { get; set; }
        [Display(Name = "Loại Văn bản đi")]
        public string OutgoingDocumentTypeName { get; set; }
    }
}
