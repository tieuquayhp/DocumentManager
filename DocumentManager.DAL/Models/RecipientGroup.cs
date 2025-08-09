using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    [Table("RecipientGroups")]
    public class RecipientGroup
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên nhóm người nhận không được để trống")]
        [StringLength(250)]
        public string RecipientGroupName { get; set; } = null!;
        // Navigation property: Một nhóm nhận có thể liên quan đến nhiều tài liệu đến
        public virtual ICollection<IncomingDocument> IncomingDocuments { get; set; } = new List<IncomingDocument>();
        // Navigation property: Một nhóm nhận có thể liên quan đến nhiều tài liệu đi
        public virtual ICollection<OutgoingDocument> OutgoingDocuments { get; set; } = new List<OutgoingDocument>();
        // Navigation property cho mối quan hệ nhiều-nhiều với Employee
        public ICollection<RecipientGroupEmployee> RecipientGroupEmployees { get; set; } = new List<RecipientGroupEmployee>();

    }
}
