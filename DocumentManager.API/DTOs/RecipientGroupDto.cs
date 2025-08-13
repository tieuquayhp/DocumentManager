// File: API/DTOs/RecipientGroupDto.cs
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class RecipientGroupDto
    {
        public int ID { get; set; }
        // Sửa lại tên thuộc tính
        public string RecipientGroupName { get; set; } = string.Empty;
    }

    // Dùng để tạo mới (Create)
    public class RecipientGroupForCreationDto
    {
        [Required(ErrorMessage = "Tên nhóm là bắt buộc.")]
        [StringLength(250)]
        public string RecipientGroupName { get; set; } = string.Empty;
    }

    // Dùng để cập nhật (Update)
    public class RecipientGroupForUpdateDto
    {
        [Required(ErrorMessage = "Tên nhóm là bắt buộc.")]
        [StringLength(250)]
        public string RecipientGroupName { get; set; } = string.Empty;
    }
}