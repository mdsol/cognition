using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Documents.Library
{
    public class KnownIssue : Document
    {
        public override string Type { get { return "issue"; } }

        public override string GetFullName()
        {
            return "Known Issue";
        }

        public string Symptoms { get; set; }

        public string Workarounds { get; set; }

        public string AppliesTo { get; set; }
    }
}
