using System.ComponentModel.DataAnnotations;

namespace DocumentManager.MVC.ViewModels
{
    public class OutgoingDocumentFormatViewModel
    {
        public int ID { get; set; }
        [Display(Name = "Tên Kiểu văn bản")]
        public string FormatName { get; set; }
        public int TypeID { get; set; }
        [Display(Name = "Loại Văn bản đi")]
        public string TypeName { get; set; }
    }
}
