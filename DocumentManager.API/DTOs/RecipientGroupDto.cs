using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class RecipientGroupDto
    {
        public int Id { get; set; }
        public string GroupNRecipientGroupNameame { get; set; }
    }
    // Dùng để tạo mới (Create)
    public class RecipientGroupForCreationDto
    {
        [Required(ErrorMessage = "Tên nhóm là bắt buộc.")]
        [StringLength(250)]
        public string RecipientGroupName { get; set; }
    }

    // Dùng để cập nhật (Update)
    public class RecipientGroupForUpdateDto
    {
        [Required(ErrorMessage = "Tên nhóm là bắt buộc.")]
        [StringLength(250)]
        public string RecipientGroupName { get; set; }
    }
}
