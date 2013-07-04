using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Configuration;
using Cognition.Shared.Documents;
using MyCouch;

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

        private Client GetDb()
        {
            return new MyCouch.Client(appSettingProvider.GetString("CouchDb"));
        }
    }
}
