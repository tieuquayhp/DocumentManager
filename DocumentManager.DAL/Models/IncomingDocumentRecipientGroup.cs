using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManager.DAL.Models
{
    public class IncomingDocumentRecipientGroup
    {
        public int IncomingDocumentID { get; set; }
        public virtual IncomingDocument IncomingDocument { get; set; }

        public int RecipientGroupID { get; set; }
        public virtual RecipientGroup RecipientGroup { get; set; }
    }
}
