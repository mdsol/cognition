using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Web.Tests.Mocks
{
    public class MockDocumentService : IDocumentService
    {
        public List<dynamic> Documents = new List<dynamic>(); 
        public async Task CreateNewDocument(dynamic document)
        {
            Documents.Add(document);
        }
    }
}
