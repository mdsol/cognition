using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cognition.Shared.Documents;

namespace Cognition.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentTypeResolver documentTypeResolver;
        private readonly IDocumentService documentService;

        public DocumentController(IDocumentTypeResolver documentTypeResolver, IDocumentService documentService)
        {
            this.documentTypeResolver = documentTypeResolver;
            this.documentService = documentService;
        }

        //
        // GET: /Document/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string type)
        {
            return View(GetNewDocument(type));
        }

        private dynamic GetNewDocument(string type)
        {
            var documentType = documentTypeResolver.GetDocumentType(type);
            return Activator.CreateInstance(documentType);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection formCollection)
        {
            var newDocument = GetNewDocument(formCollection["Type"]);

            if (ModelState.IsValid)
            {
                UpdateModel(newDocument);
                await documentService.CreateNewDocument(newDocument);
            }

            return View(newDocument);
        }
    }
}