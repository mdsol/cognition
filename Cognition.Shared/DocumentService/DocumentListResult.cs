using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Shared.DocumentService
{
    public class DocumentListResult
    {
        public bool Success { get; set; }
        public IEnumerable<DocumentReference> Documents { get; set; } 
        public long TotalDocuments { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ErrorReason { get; set; }
    }
}
