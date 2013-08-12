using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Shared.DocumentService
{
    public class DocumentGetVersionResult
    {
        public bool Success { get; set; }
        public Document Document { get; set; }
        public string VersionId { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
    }
}
