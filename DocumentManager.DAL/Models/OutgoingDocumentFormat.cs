using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    [Table("OutgoingDocumentFormats")]
    public class OutgoingDocumentFormat
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên kiểu văn bản đi không được để trống")]
        [StringLength(250)]
        public string OutgoingDocumentFormatName { get; set; } = null!;
        // Foreign Key cho OutgoingDocumentType
        public int OutgoingDocumentTypeId { get; set; }
        [ForeignKey("OutgoingDocumentTypeId")]
        public OutgoingDocumentType OutgoingDocumentType { get; set; } = null!;
        // Navigation property
        public virtual ICollection<OutgoingDocument> OutgoingDocuments { get; set; } = new List<OutgoingDocument>();

    }
}
