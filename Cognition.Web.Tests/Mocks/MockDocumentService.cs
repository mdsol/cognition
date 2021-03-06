﻿using System;
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
        public Dictionary<string, IList<Document>> Versions = new Dictionary<string, IList<Document>>();

        private readonly IDocumentTypeResolver documentTypeResolver;

        public MockDocumentService(IDocumentTypeResolver documentTypeResolver)
        {
            this.documentTypeResolver = documentTypeResolver;
        }

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
            if (!Versions.ContainsKey(id))
            {
                Versions[id] = new List<Document>() { existing };
            }
            else
            {
                Versions[id].Add(existing);
            }
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
                Documents = Documents.Where(d => d.Type == typeName).Skip(pageSize * pageIndex).Take(pageSize).Select(r => new DocumentReference() { Id = r.Id, Title = r.Title, Type = r.Type }),
                TotalDocuments = Documents.Count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Success = true
            };
        }

        public async Task<DocumentCountAvailableVersionsResult> CountAvailableVersions(string id)
        {
            return !Versions.ContainsKey(id) ? new DocumentCountAvailableVersionsResult() {Amount = 0, Success = true} :
                new DocumentCountAvailableVersionsResult() { Amount = Versions[id].Count(), Success = true };
        }

        public async Task<DocumentAvailableVersionsResult> GetAvailableVersions(string id)
        {
            return new DocumentAvailableVersionsResult();
        }

        public async Task<DocumentGetVersionResult> GetDocumentVersionAsType(string id, Type type, string versionId)
        {
            return new DocumentGetVersionResult();
        }

        public async Task<DocumentRestoreVersionResult> RestoreDocumentVersion(string id, Type type, string versionId)
        {
            return new DocumentRestoreVersionResult();
        }

        public async Task<DocumentSearchResult> SearchAllDocumentsByTitle(string query, int pageSize, int pageIndex)
        {
            var results = query == null ? Enumerable.Empty<Document>() : Documents.Where(t => t.Title.ToLower().Contains(query.ToLower()));
            return new DocumentSearchResult()
            {
                Result = results.Skip(pageSize * pageIndex).Take(pageSize),
                IncludesPrivate = true,
                Query = query,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = results.Count(),
                Success = true
            };
        }
    }
}
