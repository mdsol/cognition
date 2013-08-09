using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cognition.Shared.Changes;

namespace Cognition.Web.Controllers
{
    public class ChangesController : Controller
    {
        private readonly IDocumentChangeService documentChangeService;

        public ChangesController(IDocumentChangeService documentChangeService)
        {
            this.documentChangeService = documentChangeService;
        }

        public PartialViewResult AllRecentPartial()
        {
            var recent = documentChangeService.GetLatestChanges(20);
            return PartialView(recent);
        }

        public async Task<PartialViewResult> GetSingleChange(string id)
        {
            return PartialView("_Change", await documentChangeService.GetSingleAsync(new Guid(id)));
        }
    }
}