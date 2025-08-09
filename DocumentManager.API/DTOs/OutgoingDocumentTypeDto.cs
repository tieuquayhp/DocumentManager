using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class OutgoingDocumentTypeDto
    {
        public Guid ID { get; set; }
        public string OutgoingDocumentTypeName { get; set; }
    }
    // Dùng để tạo mới (Create)
    public class OutgoingDocumentTypeForCreationDto
    {
        [Required(ErrorMessage = "Tên loại văn bản đi không được để trống")]
        [StringLength(250)]
        public string OutgoingDocumentTypeName { get; set; }
    }
    // Dùng để cập nhật (Update)
    public class OutgoingDocumentTypeForUpdateDto
    {
        [Required(ErrorMessage = "Tên loại văn bản đi không được để trống")]
        [StringLength(250)]
        public string OutgoingDocumentTypeName { get; set; }
    }
}
