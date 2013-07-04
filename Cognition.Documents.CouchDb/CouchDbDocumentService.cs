using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Configuration;
using Cognition.Shared.Documents;
using MyCouch;
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

        public async Task CreateNewDocument(dynamic document)
        {
            using (var db = GetDb())
            {
                await db.Entities.PostAsync(document);
            }
        }

        public async Task<Document> GetDocumentAsType(string id, Type type)
        {
            using (var db = GetDb())
            {
                var response = await db.Documents.GetAsync(id);
                return await JsonConvert.DeserializeObjectAsync(response.Content, type, new JsonSerializerSettings() ) as Document;
            }
        }

        private Client GetDb()
        {
            return new MyCouch.Client(appSettingProvider.GetString("CouchDb"));
        }
    }
}
