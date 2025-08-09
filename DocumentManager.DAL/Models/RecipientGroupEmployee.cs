using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    [Table("RecipientGroupEmployees")]
    public class RecipientGroupEmployee
    {
        //[Key]
        //public int Id { get; set; } // Sử dụng khóa chính riêng theo sơ đồ
        [Required]
        public int EmployeeID { get; set; }
        
        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; } = null!;
        [Required]
        public int RecipientGroupID { get; set; }
        [ForeignKey("RecipientGroupID")]
        public RecipientGroup RecipientGroup { get; set; } = null!;
    }
}
