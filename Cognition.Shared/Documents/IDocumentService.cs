using System;
using System.Threading.Tasks;

namespace Cognition.Shared.Documents
{
    public interface IDocumentService
    {
        Task CreateNewDocument(dynamic document);

        Task<Document> GetDocumentAsType(string id, Type type);
    }
}