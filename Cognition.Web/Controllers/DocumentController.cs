using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cognition.Shared.Changes;
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
        private readonly IDocumentUpdateNotifier documentUpdateNotifier;

        public DocumentController(IDocumentTypeResolver documentTypeResolver, IDocumentService documentService, IUserAuthenticationService userAuthenticationService, IDocumentUpdateNotifier documentUpdateNotifier)
        {
            this.documentTypeResolver = documentTypeResolver;
            this.documentService = documentService;
            this.userAuthenticationService = userAuthenticationService;
            this.documentUpdateNotifier = documentUpdateNotifier;
        }

        public async Task<ActionResult> Index(string id, string type)
        {
            var result = await documentService.GetDocumentAsType(id, documentTypeResolver.GetDocumentType(type));
            return View(result.Document);
        }

        public async Task<ActionResult> IndexPartial(string id, string type)
        {
            var result = await documentService.GetDocumentAsType(id, documentTypeResolver.GetDocumentType(type));
            return PartialView("_Index", result.Document);
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

            if (TryUpdateModel(newDocument))
            {
                ((Document) newDocument).CreatedByUserId = userAuthenticationService.GetCurrentUserEmail();
                var result = await documentService.CreateNewDocument(newDocument);

                if (result.Success)
                {
                    await documentUpdateNotifier.DocumentUpdated(new DocumentCreatedNotification(newDocument,
                        userAuthenticationService.GetUserByEmail(userAuthenticationService.GetCurrentUserEmail())));
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
            var modelType = documentTypeResolver.GetDocumentType(formCollection["Type"]);
            var existingDocumentGetResult = await documentService.GetDocumentAsType(formCollection["Id"],
                modelType);

            dynamic existingDocument = existingDocumentGetResult.Document;
            
            if (TryUpdateModel(existingDocument))
            {
                ((Document) existingDocument).LastUpdatedByUserId = userAuthenticationService.GetCurrentUserEmail();
                var result = await documentService.UpdateDocument(formCollection["Id"], existingDocument);
                if (result.Success)
                {
                    await documentUpdateNotifier.DocumentUpdated(new DocumentUpdateNotification(existingDocument,
                            userAuthenticationService.GetUserByEmail(userAuthenticationService.GetCurrentUserEmail())));

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
                var viewModel = new DocumentListViewModel
                {
                    Documents = result.Documents.ToList(),
                    PageIndex = result.PageIndex,
                    PageSize = result.PageSize,
                    TotalDocuments = result.TotalDocuments,
                    TypeName = type,
                    TypeFullName = documentTypeResolver.GetDocumentTypeFullName(type)
                };

                return View(viewModel);
            }
            else
            {
                throw new Exception(result.ErrorReason);
            }

            
        }
    }
}