using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Cognition.Shared.Changes;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Cognition.Web.Hubs
{
    public class PageUpdater
    {
        private readonly static Lazy<PageUpdater> _instance =
    new Lazy<PageUpdater>(() => new PageUpdater(GlobalHost.ConnectionManager.GetHubContext<PageUpdateHub>().Clients));

        private PageUpdater(IHubConnectionContext clients)
        {
            Clients = clients;
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        public async Task PageUpdated(DocumentUpdateNotification documentUpdateNotification)
        {
            // todo: filter by group
            await Clients.Group(documentUpdateNotification.DocumentId).pageUpdated(documentUpdateNotification.Type, documentUpdateNotification.Id);
        }

        public static PageUpdater Instance
        {
            get
            {
                return _instance.Value;
            }
        }

    }
}