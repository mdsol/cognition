using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cognition.Shared.Documents;

namespace Cognition.Web.ViewModels
{
    public class DocumentListViewModel
    {
        public List<DocumentReference> Documents { get; set; }
        public long TotalDocuments { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string TypeName { get; set; }
        public string TypeFullName { get; set; }
    }
}