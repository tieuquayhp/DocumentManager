using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.DAL.Models
{
    [Table("OutgoingDocumentTypes")]
    public class OutgoingDocumentType
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên loại văn bản đi không được để trống")]
        [StringLength(250)]
        public string OutgoingDocumentTypeName { get; set; } = null!;
        // Navigation property
        public virtual ICollection<OutgoingDocument> OutgoingDocuments { get; set; } = new List<OutgoingDocument>();
        public virtual ICollection<OutgoingDocumentFormat> OutgoingDocumentFormats { get; set; } = new List<OutgoingDocumentFormat>();


    }
}
