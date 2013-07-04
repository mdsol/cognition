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
        public List<Document> Documents = new List<Document>(); 
        public async Task CreateNewDocument(dynamic document)
        {
            Documents.Add(document);
        }

        public async Task<Document> GetDocumentAsType(string id, Type type)
        {
            return Documents.Single(d => d.Id == id);
        }

    }
}
