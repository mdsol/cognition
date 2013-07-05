using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognition.Documents.CouchDb
{
    public class DocumentVersionAttachment
    {
        public string VersionId { get; set; }
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
    }
}
