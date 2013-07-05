using System;
using System.Threading.Tasks;
using Cognition.Shared.DocumentService;

namespace Cognition.Shared.Documents
{
    public interface IDocumentService
    {
        Task<DocumentCreateResult> CreateNewDocument(dynamic document);

        Task<DocumentUpdateResult> UpdateDocument(string id, dynamic document);

        Task<DocumentGetResult> GetDocumentAsType(string id, Type type);
    }
}