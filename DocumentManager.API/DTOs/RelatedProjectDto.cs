using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class RelatedProjectDto
    {
        public int Id { get; set; }
        public string RelatedProjectName { get; set; }
    }
    // Dùng để tạo mới (Create)
    public class RelatedProjectForCreationDto
    {
        [Required(ErrorMessage = "Tên dự án không được để trống")]
        [StringLength(250)]
        public string RelatedProjectName { get; set; }
    }
    // Dùng để cập nhật (Update)
    public class RelatedProjectForUpdateDto
    {
        [Required(ErrorMessage = "Tên dự án không được để trống")]
        [StringLength(250)]
        public string RelatedProjectName { get; set; }
    }
}
