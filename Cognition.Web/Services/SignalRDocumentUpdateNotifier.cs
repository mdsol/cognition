using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Cognition.Shared.Changes;
using Cognition.Web.Hubs;

namespace Cognition.Web.Services
{
    public class SignalRDocumentUpdateNotifier : IDocumentUpdateNotifier
    {
        private readonly IDocumentChangeService documentChangeService;

        public SignalRDocumentUpdateNotifier(IDocumentChangeService documentChangeService)
        {
            this.documentChangeService = documentChangeService;
        }

        public async Task DocumentUpdated(DocumentUpdateNotification notification)
        {
            await documentChangeService.AddChange(notification);
            await PageUpdater.Instance.PageUpdated(notification);
        }
    }
}