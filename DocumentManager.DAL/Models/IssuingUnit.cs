using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    [Table("IssuingUnits")]
    public class IssuingUnit
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên đơn vị không được để trống")]
        [StringLength(250)]
        public string IssuingUnitName { get; set; } = null!;
        // Navigation property
        public virtual ICollection<IncomingDocument> IncomingDocuments { get; set; } = new List<IncomingDocument>();
        public virtual ICollection<OutgoingDocument> OutgoingDocuments { get; set; } = new List<OutgoingDocument>();
    }
}
