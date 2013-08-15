using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Shared.DocumentService
{
    public class DocumentSearchResult
    {
        public bool Success { get; set; }
        public IEnumerable<Document> Result { get; set; }
        public long TotalRecords { get; set; }
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string Query { get; set; }
        public bool IncludesPrivate { get; set; }
    }
}
