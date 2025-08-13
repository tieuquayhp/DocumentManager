// File: API/DTOs/OutgoingDocumentDto.cs
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    // DTO để hiển thị
    public class OutgoingDocumentDto
    {
        public int ID { get; set; }
        public string OutgoingDocumentNumber { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string DocumentContent { get; set; } = string.Empty;
        public string DocumentFile { get; set; } = string.Empty;

        // Dữ liệu từ các bảng liên quan (đã bỏ RecipientGroup)
        public int OutgoingDocumentTypeID { get; set; }
        public string OutgoingDocumentTypeName { get; set; } = string.Empty;
        public int OutgoingDocumentFormatID { get; set; }
        public string OutgoingDocumentFormatName { get; set; } = string.Empty;
        public int IssuingUnitID { get; set; }
        public string IssuingUnitName { get; set; } = string.Empty;
        public int RelatedProjectID { get; set; }
        public string RelatedProjectName { get; set; } = string.Empty;
    }

    // DTO để tạo mới
    public class OutgoingDocumentForCreationDto
    {
        [Required]
        public string OutgoingDocumentNumber { get; set; } = string.Empty;
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string DocumentContent { get; set; } = string.Empty;
        public string DocumentFile { get; set; } = string.Empty;
        [Required]
        public int OutgoingDocumentTypeID { get; set; }
        [Required]
        public int OutgoingDocumentFormatID { get; set; }
        [Required]
        public int IssuingUnitID { get; set; }
        [Required]
        public int RelatedProjectID { get; set; }

        // Thay RecipientGroupID bằng danh sách các ID
        [Required]
        public List<int> RecipientGroupIDs { get; set; } = new List<int>();
    }

    // DTO để cập nhật
    public class OutgoingDocumentForUpdateDto : OutgoingDocumentForCreationDto
    {
        // Kế thừa là đủ
    }
}