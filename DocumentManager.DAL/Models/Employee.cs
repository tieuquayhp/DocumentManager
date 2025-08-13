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
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        [EmailAddress]
        public string Email { get; set; }

        // Bỏ DepartmentID, thay bằng trường DepartmentName kiểu string
        [StringLength(250)]
        public string DepartmentName { get; set; }

        // Bỏ navigation property đến Department
        // public virtual Department Department { get; set; }

        public virtual ICollection<RecipientGroupEmployee> RecipientGroupEmployees { get; set; }
    }
}
