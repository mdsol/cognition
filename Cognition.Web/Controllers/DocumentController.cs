using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cognition.Shared.Documents;
using Cognition.Shared.Users;
using Cognition.Web.ViewModels;

namespace Cognition.Web.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentTypeResolver documentTypeResolver;
        private readonly IDocumentService documentService;
        private readonly IUserAuthenticationService userAuthenticationService;

        public DocumentController(IDocumentTypeResolver documentTypeResolver, IDocumentService documentService, IUserAuthenticationService userAuthenticationService)
        {
            this.documentTypeResolver = documentTypeResolver;
            this.documentService = documentService;
            this.userAuthenticationService = userAuthenticationService;
        }

        public async Task<ActionResult> Index(string id, string type)
        {
            var result = await documentService.GetDocumentAsType(id, documentTypeResolver.GetDocumentType(type));
            return View(result.Document);
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
                ((Document) newDocument).CreatedByUserId = userAuthenticationService.GetCurrentUserEmail();
                var result = await documentService.CreateNewDocument(newDocument);

                if (result.Success)
                {
                    return RedirectToAction("Index", new { id = result.NewId, type = newDocument.Type });
                }

            }

            return View(newDocument);
        }

        public async Task<ActionResult> Edit(string id, string type)
        {
            var result = await documentService.GetDocumentAsType(id, documentTypeResolver.GetDocumentType(type));

            return View(result.Document);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(FormCollection formCollection)
        {

            var existingDocumentGetResult = await documentService.GetDocumentAsType(formCollection["Id"],
                documentTypeResolver.GetDocumentType(formCollection["Type"]));

            dynamic existingDocument = existingDocumentGetResult.Document;

            if (ModelState.IsValid)
            {
                UpdateModel(existingDocument);
                ((Document) existingDocument).LastUpdatedByUserId = userAuthenticationService.GetCurrentUserEmail();
                var result = await documentService.UpdateDocument(formCollection["Id"], existingDocument);
                if (result.Success)
                {
                    return RedirectToAction("Index", new { id = formCollection["Id"], type = formCollection["Type"] });
                }
            }

            return View(existingDocument);
        }

        public async Task<ActionResult> List(string type, int pageSize = 20, int pageIndex = 0)
        {
            
            var result =
                await
                    documentService.GetDocumentList(documentTypeResolver.GetDocumentType(type), type, pageSize,
                        pageIndex);

            if (result.Success)
            {
                var viewModel = new DocumentListViewModel();

                viewModel.Documents = result.Documents.ToList();
                viewModel.PageIndex = result.PageIndex;
                viewModel.PageSize = result.PageSize;
                viewModel.TotalDocuments = result.TotalDocuments;
                viewModel.TypeName = type;
                viewModel.TypeFullName = documentTypeResolver.GetDocumentTypeFullName(type);

                return View(viewModel);
            }
            else
            {
                throw new Exception(result.ErrorReason);
            }

            
        }
    }
}