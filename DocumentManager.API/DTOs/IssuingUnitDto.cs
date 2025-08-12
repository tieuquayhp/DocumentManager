using System.ComponentModel.DataAnnotations;

namespace DocumentManager.API.DTOs
{
    public class IssuingUnitDto
    {
        public int ID { get; set; }
        public string IssuingUnitName { get; set; }
    }
    public class IssuingUnitForCreationDto
    {
        [Required(ErrorMessage = "Tên đơn vị phát hành là bắt buộc.")]
        [StringLength(250)]
        public string IssuingUnitName { get; set; }
    }

    public class IssuingUnitForUpdateDto
    {
        [Required(ErrorMessage = "Tên đơn vị phát hành là bắt buộc.")]
        [StringLength(250)]
        public string IssuingUnitName { get; set; }
    }
}
