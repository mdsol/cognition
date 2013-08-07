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
            return new DocumentCreateResult() { NewId = document.Id, Success = true };
        }

        public async Task<DocumentUpdateResult> UpdateDocument(string id, dynamic document)
        {
            var existing = Documents.Single(d => d.Id == id);
            Documents.Remove(existing);
            Documents.Add((Document)document);
            return new DocumentUpdateResult() { Success = true };
        }

        public async Task<DocumentGetResult> GetDocumentAsType(string id, Type type)
        {
            return new DocumentGetResult() { Document = Documents.Single(d => d.Id == id), Success = true };
        }

        public async Task<DocumentListResult> GetDocumentList(Type type, string typeName, int pageSize, int pageIndex)
        {
            return new DocumentListResult()
            {
                Documents = Documents.Where(d => d.Type == typeName).Skip(pageSize*pageIndex).Take(pageSize).Select(r => new DocumentReference() { Id = r.Id, Title = r.Title, Type = r.Type}),
                TotalDocuments = Documents.Count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Success = true
            };
        }
    }
}
