using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [StringLength(250)]
        public string EmployeeName { get; set; } = null!;
        [Required]
        [StringLength(250)]
        [EmailAddress]
        public string? Email { get; set; }
        // Foreign Key cho Department
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        // Navigation property cho mối quan hệ nhiều-nhiều với RecipientGroup
        public virtual ICollection<RecipientGroupEmployee> RecipientGroupEmployees { get; set; } = new List<RecipientGroupEmployee>();
    }
}
