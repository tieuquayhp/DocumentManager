using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    public class RecipientGroupEmployee
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int RecipientGroupID { get; set; }
        public Employee Employee { get; set; } = null!;
        public RecipientGroup RecipientGroup { get; set; } = null!;
    }
}
