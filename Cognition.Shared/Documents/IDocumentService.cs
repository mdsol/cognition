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

        Task<DocumentListResult> GetDocumentList(Type type, string typeName, int pageSize, int pageIndex);

        Task<DocumentCountAvailableVersionsResult> CountAvailableVersions(string id);

        Task<DocumentAvailableVersionsResult> GetAvailableVersions(string id);

        Task<DocumentGetVersionResult> GetDocumentVersionAsType(string id, Type type, string versionId);

        Task<DocumentRestoreVersionResult> RestoreDocumentVersion(string id, Type type, string versionId);

        Task<DocumentSearchResult> SearchAllDocumentsByTitle(string query, int pageSize,
            int pageIndex);
    }
}