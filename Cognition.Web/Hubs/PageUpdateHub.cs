using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Cognition.Web.Hubs
{
    [HubName("pageUpdate")]
    public class PageUpdateHub : Hub
    {
        private readonly PageUpdater _pageUpdater;

        public PageUpdateHub() : this(PageUpdater.Instance) { }

        public PageUpdateHub(PageUpdater stockTicker)
        {
            _pageUpdater = stockTicker;
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }
    }
}