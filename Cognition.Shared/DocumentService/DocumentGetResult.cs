using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Shared.DocumentService
{
    public class DocumentGetResult
    {
        public bool Success { get; set; }
        public Document Document { get; set; }
    }
}
