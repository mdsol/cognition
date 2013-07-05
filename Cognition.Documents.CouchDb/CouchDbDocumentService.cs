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

                var result = await db.Entities.PutAsync(asDocument);
                var updateResult = new DocumentUpdateResult();
                if (result.IsSuccess)
                {
                    updateResult.Success = true;

                    // add previous version as an attachment
                    var versionAttachment = new DocumentVersionAttachment
                    {
                        Content = previousVersion.Content,
                        VersionId = result.Rev,
                        DateTime = DateTime.UtcNow
                    };

                    var attachmentCommand = new PutAttachmentCommand(id, result.Rev, "version-" + result.Rev, HttpContentTypes.Json,
                        Encoding.UTF8.GetBytes (await JsonConvert.SerializeObjectAsync(versionAttachment)));

                    await db.Attachments.PutAsync(attachmentCommand);
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
