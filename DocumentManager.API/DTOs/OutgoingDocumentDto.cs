using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class OutgoingDocumentDto
    {
        public int ID { get; set; }
        public string OutgoingDocumentNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DocumentContent { get; set; }
        public string DocumentFile { get; set; }

        // Dữ liệu từ các bảng liên quan
        public int OutgoingDocumentTypeID { get; set; }
        public string OutgoingDocumentTypeName { get; set; }

        public int OutgoingDocumentFormatID { get; set; }
        public string OutgoingDocumentFormatName { get; set; }

        public int IssuingUnitID { get; set; }
        public string IssuingUnitName { get; set; }

        public int RelatedProjectID { get; set; }
        public string RelatedProjectName { get; set; }

        public int RecipientGroupID { get; set; }
        public string RecipientGroupName { get; set; }
    }
    public class OutgoingDocumentForCreationDto
    {
        [Required]
        public string OutgoingDocumentNumber { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string DocumentContent { get; set; }
        public string DocumentFile { get; set; }
        [Required]
        public int OutgoingDocumentTypeID { get; set; }
        [Required]
        public int OutgoingDocumentFormatID { get; set; }
        [Required]
        public int IssuingUnitID { get; set; }
        [Required]
        public int RelatedProjectID { get; set; }
        [Required]
        public int RecipientGroupID { get; set; }
    }
    public class OutgoingDocumentForUpdateDto : OutgoingDocumentForCreationDto
    {
        [Required]
        public string OutgoingDocumentNumber { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string DocumentContent { get; set; }
        public string DocumentFile { get; set; }
        [Required]
        public int OutgoingDocumentTypeID { get; set; }
        [Required]
        public int OutgoingDocumentFormatID { get; set; }
        [Required]
        public int IssuingUnitID { get; set; }
        [Required]
        public int RelatedProjectID { get; set; }
        [Required]
        public int RecipientGroupID { get; set; }
    }
}
