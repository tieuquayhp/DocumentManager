using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class IncomingDocumentDto
    {
        public int ID { get; set; }
        public string IncomingDocumentNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DocumentCodeFromIssuer { get; set; }
        public DateTime ReleaseDateFromIssuer { get; set; }
        public string DocumentContent { get; set; }
        public string DocumentFile { get; set; }

        // Dữ liệu từ các bảng liên quan
        public int IssuingUnitID { get; set; }
        public string IssuingUnitName { get; set; }

        public int RelatedProjectID { get; set; }
        public string RelatedProjectName { get; set; }

        public int RecipientGroupID { get; set; }
        public string RecipientGroupName { get; set; }
    }
    public class IncomingDocumentForCreationDto
    {
        [Required]
        public string IncomingDocumentNumber { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public string DocumentCodeFromIssuer { get; set; }
        public DateTime ReleaseDateFromIssuer { get; set; }
        [Required]
        public string DocumentContent { get; set; }
        public string DocumentFile { get; set; }
        [Required]
        public int IssuingUnitID { get; set; }
        [Required]
        public int RelatedProjectID { get; set; }
        [Required]
        public int RecipientGroupID { get; set; }
    }
    public class IncomingDocumentForUpdateDto : IncomingDocumentForCreationDto
    {
        [Required]
        public string IncomingDocumentNumber { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public string DocumentCodeFromIssuer { get; set; }
        public DateTime ReleaseDateFromIssuer { get; set; }
        [Required]
        public string DocumentContent { get; set; }
        public string DocumentFile { get; set; }
        [Required]
        public int IssuingUnitID { get; set; }
        [Required]
        public int RelatedProjectID { get; set; }
        [Required]
        public int RecipientGroupID { get; set; }
    }

}
