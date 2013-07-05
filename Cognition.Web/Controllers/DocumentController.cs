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

        public async Task<ActionResult> Index(string id, string type)
        {
            var document = await documentService.GetDocumentAsType(id, documentTypeResolver.GetDocumentType(type));
            return View(document);
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
                var result = await documentService.CreateNewDocument(newDocument);

                if (result.Success)
                {
                    return RedirectToAction("Index", new {id = result.NewId, type = newDocument.Type});
                }
                
            }

            return View(newDocument);
        }
    }
}