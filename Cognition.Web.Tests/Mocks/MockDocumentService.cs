using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;
using Cognition.Shared.DocumentService;

namespace Cognition.Web.Tests.Mocks
{
    public class MockDocumentService : IDocumentService
    {
        public List<Document> Documents = new List<Document>();

        public async Task<DocumentCreateResult> CreateNewDocument(dynamic document)
        {
            document.Id = Guid.NewGuid().ToString();
            Documents.Add(document);
            return new DocumentCreateResult() {NewId = document.Id, Success = true};
        }

        public async Task<Document> GetDocumentAsType(string id, Type type)
        {
            return Documents.Single(d => d.Id == id);
        }

    }
}
