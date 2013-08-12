using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognition.Shared.DocumentService
{
    public class DocumentAvailableVersionsResult
    {
        public bool Success { get; set; }
        public IEnumerable<DocumentAvailableVersion> Versions { get; set; }
    }

    public class DocumentAvailableVersion
    {
        public DateTime DateTime { get; set; }
        public string VersionId { get; set; }
        public string UserId { get; set; }
    }
}
