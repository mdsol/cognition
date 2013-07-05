using System;
using System.Collections.Generic;
using System.Linq;
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
                    DateTime = DateTime.UtcNow
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

        private Client GetDb()
        {
            return new MyCouch.Client(appSettingProvider.GetString("CouchDb"));
        }
    }
}
