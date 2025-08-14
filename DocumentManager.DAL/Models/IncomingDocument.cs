using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.DAL.Models
{
    [Table("IncomingDocuments")]
    public class IncomingDocument
    {
        public IncomingDocument()
        {
            // Khởi tạo danh sách để đảm bảo nó không bao giờ bị null
            IncomingDocumentRecipientGroups = new List<IncomingDocumentRecipientGroup>();
        }
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Số văn bản đến không được để trống")]
        public int IncomingDocumentNumber { get; set; }
        [Required(ErrorMessage = "Ngày phát hành văn bản đến không được để trống")]
        public DateTime ReleaseDate { get; set; }
        public string? DocumentCodeFromIssuer { get; set; }
        public DateTime? ReleaseDateFromIssuer { get; set; }
        public string? DocumentContent { get; set; }
        public string? DocumentFile { get; set; } // Có thể là đường dẫn file hoặc tên file
         // Foreign Keys
        public int IssuingUnitID { get; set; }
        [ForeignKey("IssuingUnitID")]
        public IssuingUnit IssuingUnit { get; set; } = null!;
        public int RelatedProjectID { get; set; }
        [ForeignKey("RelatedProjectID")]
        public RelatedProject RelatedProject { get; set; } = null!;
        //public int RecipientGroupID { get; set; }
        //[ForeignKey("RecipientGroupID")]
        //public RecipientGroup RecipientGroup { get; set; } = null!;
        // Thêm navigation property cho mối quan hệ nhiều-nhiều
        public virtual ICollection<IncomingDocumentRecipientGroup> IncomingDocumentRecipientGroups { get; set; }

    }
}
