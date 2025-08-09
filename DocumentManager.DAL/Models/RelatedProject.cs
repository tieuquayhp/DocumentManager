using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    public class RelatedProject
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên dự án không được để trống")]
        [StringLength(250)]
        public string ProjectName { get; set; } = null!;
        public virtual ICollection<IncomingDocument> IncomingDocuments { get; set; } = new List<IncomingDocument>();
        public virtual ICollection<OutgoingDocument> OutgoingDocuments { get; set; } = new List<OutgoingDocument>();
    }
}
