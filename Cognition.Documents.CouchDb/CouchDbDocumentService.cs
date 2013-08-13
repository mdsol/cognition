using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Configuration;
using Cognition.Shared.Documents;
using Cognition.Shared.DocumentService;
using MyCouch;
using MyCouch.Commands;
using MyCouch.Net;
using Newtonsoft.Json;

namespace Cognition.Documents.CouchDb
{
    public class CouchDbDocumentService : IDocumentService
    {
        private readonly IAppSettingProvider appSettingProvider;

        public CouchDbDocumentService(IAppSettingProvider appSettingProvider)
        {
            this.appSettingProvider = appSettingProvider;
        }

        public async Task<DocumentCreateResult> CreateNewDocument(dynamic document)
        {
            var asDocument = (Document) document;
            
            using (var db = GetDb())
            {
                var result = await db.Entities.PostAsync(asDocument);
                var createResult = new DocumentCreateResult();
                if (result.IsSuccess)
                {
                    createResult.Success = true;
                    createResult.NewId = result.Id;
                }

                return createResult;
            }
        }

        public async Task<DocumentUpdateResult> UpdateDocument(string id, dynamic document)
        {
            var asDocument = (Document) document;
            asDocument.Id = id;
            
            using (var db = GetDb())
            {
                var previousVersion = await db.Documents.GetAsync(id);

                // add the previous version as an attachment
                var versionAttachment = new DocumentVersionAttachment
                {
                    Content = previousVersion.Content,
                    VersionId = previousVersion.Rev,
                    DateTime = DateTime.UtcNow,
                    UserId = asDocument.LastUpdatedByUserId
                };

                var attachment = new DocumentAttachment
                {
                    ContentType = HttpContentTypes.Json,
                    Data = Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(await JsonConvert.SerializeObjectAsync(versionAttachment)))
                };

                if (asDocument.Attachments == null) asDocument.Attachments = new Dictionary<string, DocumentAttachment>();
                asDocument.Attachments.Add("version-" + previousVersion.Rev, attachment);

                var result = await db.Entities.PutAsync(asDocument);
                var updateResult = new DocumentUpdateResult();
                if (result.IsSuccess)
                {
                    updateResult.Success = true;
                }

                return updateResult;

            }
        }

        public async Task<DocumentCountAvailableVersionsResult> CountAvailableVersions(string id)
        {
            using (var db = GetDb())
            {
                var documentResponse = await db.Documents.GetAsync(new GetDocumentCommand(id));
                var result = new DocumentCountAvailableVersionsResult();
                if (documentResponse.IsSuccess)
                {
                    var document = await JsonConvert.DeserializeObjectAsync<Document>(documentResponse.Content);
                    result.Amount = document.Attachments.Count(d => d.Key.StartsWith("version-"));

                    result.Success = true;
                }

                return result;
            }
        }

        public async Task<DocumentGetVersionResult> GetDocumentVersionAsType(string id, Type type, string versionId)
        {
            using (var db = GetDb())
            {
                var result = new DocumentGetVersionResult();

                var versionAttachmentResult =
                            await
                                db.Connection.SendAsync(new HttpRequestMessage(HttpMethod.Get, appSettingProvider.GetString("CouchDb") +
                                    String.Format("/{0}/{1}", id, "version-" + versionId)));

                if (versionAttachmentResult.IsSuccessStatusCode)
                {
                    result.Success = true;

                    var versionAttachment = await JsonConvert.DeserializeObjectAsync<DocumentVersionAttachment>(await versionAttachmentResult.Content.ReadAsStringAsync());
                    result.DateTime = versionAttachment.DateTime;
                    result.UserId = versionAttachment.UserId;
                    result.VersionId = versionAttachment.VersionId;

                    result.Document = await JsonConvert.DeserializeObjectAsync(versionAttachment.Content, type, new JsonSerializerSettings()) as Document;

                }


                return result;

            }
        }

        public async Task<DocumentAvailableVersionsResult> GetAvailableVersions(string id)
        {
            using (var db = GetDb())
            {
                var documentResponse = await db.Documents.GetAsync(new GetDocumentCommand(id));
                var result = new DocumentAvailableVersionsResult();
                if (documentResponse.IsSuccess)
                {
                    var document = await JsonConvert.DeserializeObjectAsync<Document>(documentResponse.Content);

                    var versionList = new List<DocumentAvailableVersion>();
                    result.Versions = versionList;
                    foreach (var attachment in document.Attachments)
                    {
                        var versionAttachmentResult =
                            await
                                db.Connection.SendAsync(new HttpRequestMessage(HttpMethod.Get,appSettingProvider.GetString("CouchDb") +
                                    String.Format("/{0}/{1}", id, attachment.Key)));
                        if (versionAttachmentResult.IsSuccessStatusCode)
                        {
                            var versionAttachment = await JsonConvert.DeserializeObjectAsync<DocumentVersionAttachment>(await versionAttachmentResult.Content.ReadAsStringAsync());
                            var version = new DocumentAvailableVersion
                            {
                                VersionId = versionAttachment.VersionId,
                                DateTime = versionAttachment.DateTime,
                                UserId = versionAttachment.UserId
                            };

                            versionList.Add(version);
                        }
                        
                    }

                    result.Success = true;
                }

                return result;
            }
        }


        public async Task<DocumentGetResult> GetDocumentAsType(string id, Type type)
        {
            using (var db = GetDb())
            {
                var response = await db.Documents.GetAsync(id);
                var getResult = new DocumentGetResult();
                if (response.IsSuccess)
                {
                    getResult.Success = true;
                    getResult.Document = await JsonConvert.DeserializeObjectAsync(response.Content, type, new JsonSerializerSettings()) as Document;
                }

                return getResult;

            }
        }

        private async Task UpdateViewsIfRequired(string typeName, Client db)
        {

            var id = "_design/" + CreateDesignDocumentNameForTypeName(typeName);
            // check to see if the design document already exists
            var existsResponse = await db.Documents.ExistsAsync(id);

            if (existsResponse.IsSuccess && "_design/" + existsResponse.Id == id) return;

            // create the view to query on
            var viewDocument = CreateViewJsonForTypeName(typeName);

            var viewPostResult = await db.Documents.PostAsync(viewDocument);
            if (!viewPostResult.IsSuccess)
            {
                throw new Exception(String.Format("Error creating view: ({0} {1}) {2}", viewPostResult.StatusCode, viewPostResult.Reason, viewPostResult.Error));
            }
        }

        public async Task<DocumentListResult> GetDocumentList(Type type, string typeName, int pageSize, int pageIndex)
        {
            
            using (var db = GetDb())
            {

                await UpdateViewsIfRequired(typeName, db);

                var query =
                    new ViewQuery(CreateDesignDocumentNameForTypeName(typeName), "types").Configure(
                        c => c.Skip(pageIndex*pageSize).Limit(pageSize));

                var response = await db.Views.RunQueryAsync(query);

                var result = new DocumentListResult();
                result.PageSize = pageSize;
                result.PageIndex = pageIndex;
                if (response.IsSuccess)
                {
                    result.Success = true;
                    result.TotalDocuments = response.TotalRows;
                    result.Documents =
                        response.Rows.Select(
                            r => new DocumentReference() { Id = r.Id, Title = JsonConvert.DeserializeObject<string>(r.Value), Type = typeName });
                }
                else
                {
                    result.ErrorReason = response.StatusCode + " " + response.Reason + " - " + response.Error;
                }

                return result;

            }

        }

        public async Task<DocumentRestoreVersionResult> RestoreDocumentVersion(string id, Type type, string versionId)
        {
            using (var db = GetDb())
            {
                var result = new DocumentRestoreVersionResult();

                var previousDocumentResult = await GetDocumentVersionAsType(id, type, versionId);
                var currentDocumentResult = await GetDocumentAsType(id, type);

                if (previousDocumentResult.Success && currentDocumentResult.Success)
                {
                    previousDocumentResult.Document.Attachments = currentDocumentResult.Document.Attachments;
                    previousDocumentResult.Document.Rev = currentDocumentResult.Document.Rev;
                    var createDocumentResult = await UpdateDocument(id, previousDocumentResult.Document);
                    if (createDocumentResult.Success)
                    {
                        result.Success = true;
                        result.RestoredDocument = previousDocumentResult.Document;
                    }
                }


                return result;
            }
        }

        private string CreateViewJsonForTypeName(string typeName)
        {
            const string viewString = @"{{
                                        ""_id"": ""_design/{0}"",
                                        ""language"": ""javascript"",
                                        ""views"": {{
                                            ""types"": {{
                                                ""map"": ""function(doc) {{  if(doc.type !== '{1}') return;  emit(doc._id, doc.title);}}"" 
                                            }}
                                        }}
                                    }}";

            return string.Format(viewString, CreateDesignDocumentNameForTypeName(typeName), typeName);

        }

        private string CreateDesignDocumentNameForTypeName(string typeName)
        {
            var name = "document-view-{0}";
            return String.Format(name, typeName);
        }

        private Client GetDb()
        {
            return new MyCouch.Client(appSettingProvider.GetString("CouchDb"));
        }
    }
}
