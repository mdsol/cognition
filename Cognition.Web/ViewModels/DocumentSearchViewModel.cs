using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cognition.Shared.Documents;

namespace Cognition.Web.ViewModels
{
    public class DocumentSearchViewModel
    {
        public IEnumerable<Document> Results { get; set; }
        public string Query { get; set; }
        public long TotalResult { get; set; }
        public long PageSize { get; set; }
        public long PageIndex { get; set; }
    }
}