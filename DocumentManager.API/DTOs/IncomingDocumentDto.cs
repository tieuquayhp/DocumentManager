// File: API/DTOs/IncomingDocumentDto.cs
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    // DTO để hiển thị
    public class IncomingDocumentDto
    {
        public int ID { get; set; }
        public string IncomingDocumentNumber { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string DocumentCodeFromIssuer { get; set; } = string.Empty;
        public DateTime ReleaseDateFromIssuer { get; set; }
        public string DocumentContent { get; set; } = string.Empty;
        public string DocumentFile { get; set; } = string.Empty;

        // Dữ liệu từ các bảng liên quan (đã bỏ RecipientGroup)
        public int IssuingUnitID { get; set; }
        public string IssuingUnitName { get; set; } = string.Empty;
        public int RelatedProjectID { get; set; }
        public string RelatedProjectName { get; set; } = string.Empty;
    }

    // DTO để tạo mới
    public class IncomingDocumentForCreationDto
    {
        [Required]
        public string IncomingDocumentNumber { get; set; } = string.Empty;
        [Required]
        public DateTime ReleaseDate { get; set; }
        public string DocumentCodeFromIssuer { get; set; } = string.Empty;
        public DateTime ReleaseDateFromIssuer { get; set; }
        [Required]
        public string DocumentContent { get; set; } = string.Empty;
        public string DocumentFile { get; set; } = string.Empty;
        [Required]
        public int IssuingUnitID { get; set; }
        [Required]
        public int RelatedProjectID { get; set; }

        // Thay RecipientGroupID bằng danh sách các ID
        [Required]
        public List<int> RecipientGroupIDs { get; set; } = new List<int>();
    }

    // DTO để cập nhật (không cần định nghĩa lại các thuộc tính)
    public class IncomingDocumentForUpdateDto : IncomingDocumentForCreationDto
    {
        // Kế thừa tất cả thuộc tính từ lớp cha là đủ
    }
}