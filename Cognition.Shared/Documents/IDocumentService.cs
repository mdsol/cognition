using System;
using System.Threading.Tasks;
using Cognition.Shared.DocumentService;

namespace Cognition.Shared.Documents
{
    public interface IDocumentService
    {
        Task<DocumentCreateResult> CreateNewDocument(dynamic document);

        Task<Document> GetDocumentAsType(string id, Type type);
    }
}