using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên phòng ban không được để trống")]
        [StringLength(250)]
        public string DepartmentName { get; set; } = null!;
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
