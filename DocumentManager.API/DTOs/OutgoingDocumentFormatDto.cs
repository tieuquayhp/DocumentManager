using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class OutgoingDocumentFormatDto
    {
        public int Id { get; set; }

        public string OutgoingDocumentFormatName { get; set; }
        
        public int OutgoingDocumentTypeId { get; set; }
        public string OutgoingDocumentTypeName { get; set; }
    }
    // Dùng để tạo mới
    public class OutgoingDocumentFormatForCreationDto
    {
        [Required(ErrorMessage = "Tên định dạng là bắt buộc.")]
        [StringLength(250)]
        public string OutgoingDocumentFormatName { get; set; }

        [Required(ErrorMessage = "ID loại tài liệu là bắt buộc.")]
        public int OutgoingDocumentTypeId { get; set; }
    }

    // Dùng để cập nhật
    public class OutgoingDocumentFormatForUpdateDto
    {
        [Required(ErrorMessage = "Tên định dạng là bắt buộc.")]
        [StringLength(250)]
        public string OutgoingDocumentFormatName { get; set; }

        [Required(ErrorMessage = "ID loại tài liệu là bắt buộc.")]
        public int OutgoingDocumentTypeId { get; set; }
    }
}
