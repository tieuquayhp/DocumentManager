using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        [StringLength(250)]
        public string EmployeeName { get; set; } = null!;
        public string? Email { get; set; }
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public Department Department { get; set; }
        public virtual ICollection<RecipientGroupEmployee> RecipientGroupEmployees { get; set; } = new List<RecipientGroupEmployee>();
    }
}
