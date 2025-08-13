using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    public class OutgoingDocumentRecipientGroup
    {
        public int OutgoingDocumentID { get; set; }
        public virtual OutgoingDocument OutgoingDocument { get; set; }

        public int RecipientGroupID { get; set; }
        public virtual RecipientGroup RecipientGroup { get; set; }
    }
}
