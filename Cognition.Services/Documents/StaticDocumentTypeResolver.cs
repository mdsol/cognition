using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Documents.Library;
using Cognition.Shared.Documents;

namespace Cognition.Services.Documents
{
    public class StaticDocumentTypeResolver : IDocumentTypeResolver
    {
        private static readonly Dictionary<string, Type> typeMaps = new Dictionary<string, Type>()
        {
            {"issue", typeof(KnownIssue)}
        };

        public Type GetDocumentType(string type)
        {
            return typeMaps[type];
        }
    }
}
