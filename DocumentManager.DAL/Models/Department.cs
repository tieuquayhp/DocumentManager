using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.DAL.Models
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên phòng ban không được để trống")]
        [StringLength(250)]
        public string DepartmentName { get; set; } = null!;
        // Navigation property: Một phòng ban có nhiều nhân viên
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
