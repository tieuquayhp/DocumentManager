using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.DAL.Models
{
    [Table("OutgoingDocuments")]
    public class OutgoingDocument
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Số văn bản đi không được để trống")]
        public int OutgoingDocumentNumber { get; set; }
        [Required(ErrorMessage = "Ngày phát hành văn bản đi không được để trống")]
        public DateTime ReleaseDate { get; set; }
        public string? DocumentContent { get; set; }
        public string? DocumentFile { get; set; }   // Có thể là đường dẫn file hoặc tên file
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
        public int OutgoingDocumentFormatID { get; set; }
        [ForeignKey("OutgoingDocumentFormatID")]
        public OutgoingDocumentFormat OutgoingDocumentFormat { get; set; } = null!;
        public int OutgoingDocumentTypeID { get; set; }
        [ForeignKey("OutgoingDocumentTypeID")]
        public OutgoingDocumentType OutgoingDocumentType { get; set; } = null!;
        // Thêm navigation property cho mối quan hệ nhiều-nhiều
        public virtual ICollection<OutgoingDocumentRecipientGroup> OutgoingDocumentRecipientGroups { get; set; }
    }
}
