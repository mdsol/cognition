using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cognition.Shared.Changes;
using Cognition.Shared.Documents;
using Cognition.Shared.Permissions;
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
        private readonly IPermissionService permissionService;

        public DocumentController(IDocumentTypeResolver documentTypeResolver, IDocumentService documentService, IUserAuthenticationService userAuthenticationService, IDocumentUpdateNotifier documentUpdateNotifier, IPermissionService permissionService)
        {
            this.documentTypeResolver = documentTypeResolver;
            this.documentService = documentService;
            this.userAuthenticationService = userAuthenticationService;
            this.documentUpdateNotifier = documentUpdateNotifier;
            this.permissionService = permissionService;
        }

        private bool CurrentUserCanViewDocument(Document document)
        {
            switch (document.Visibility)
            {
                case Document.VisibilityStatus.Internal:
                    return permissionService.CurrentUserCanViewInternal();
                case Document.VisibilityStatus.Public:
                    return permissionService.CurrentUserCanViewPublic();
                default:
                    throw new Exception("Unknown Document visibility status.");
            }
        }

        private bool CurrentUserCanEdit()
        {
            return permissionService.CurrentUserCanEdit();
        }

        public async Task<ActionResult> Index(string id, string type)
        {
            var result = await documentService.GetDocumentAsType(id, documentTypeResolver.GetDocumentType(type));
            var availableVersionResult = await documentService.CountAvailableVersions(id);

            if (!CurrentUserCanViewDocument(result.Document)) return new HttpUnauthorizedResult();

            var viewModel = new DocumentViewViewModel
            {
                Document = result.Document,
                PreviousVersionCount = availableVersionResult.Amount,
                CanEdit = CurrentUserCanEdit()
            };

            return View(viewModel);
        }

        public async Task<ActionResult> IndexPartial(string id, string type)
        {
            var indexResult = await Index(id, type);

            return PartialView("_Index", ((ViewResult)indexResult).Model);
        }

        public ActionResult Create(string type)
        {
            var newDocument = GetNewDocument(type);
            
            if (!CurrentUserCanEdit()) return new HttpUnauthorizedResult();

            return View(newDocument);
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

            if (!CurrentUserCanEdit()) return new HttpUnauthorizedResult();

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
            if (!CurrentUserCanEdit()) return new HttpUnauthorizedResult();

            var result = await documentService.GetDocumentAsType(id, documentTypeResolver.GetDocumentType(type));

            return View(result.Document);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Edit(FormCollection formCollection)
        {
            if (!CurrentUserCanEdit()) return new HttpUnauthorizedResult();
            
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
                            GetCurrentUser()));

                    return RedirectToAction("Index", new { id = formCollection["Id"], type = formCollection["Type"] });
                }
            }

            return View(existingDocument);
        }

        private User GetCurrentUser()
        {
            return userAuthenticationService.GetUserByEmail(userAuthenticationService.GetCurrentUserEmail());
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

            throw new Exception(result.ErrorReason);
        }

        public async Task<ActionResult> Version(string id, string type, string v = null)
        {
            if (!CurrentUserCanEdit()) return new HttpUnauthorizedResult();

            var documentType = documentTypeResolver.GetDocumentType(type);
            var currentResult = await documentService.GetDocumentAsType(id, documentType);
            var versionsResult =  await documentService.GetAvailableVersions(id);
            if (!currentResult.Success) throw new Exception("Could not load current version.");
            if (!versionsResult.Success) throw new Exception("Could not load versions.");

            var viewModel = new DocumentVersionViewModel();
            viewModel.AvailableVersions = versionsResult.Versions.OrderByDescending(ver => ver.DateTime);
            viewModel.CurrentVersion = currentResult.Document;

            if (!String.IsNullOrWhiteSpace(v))
            {
                var versionResult = await documentService.GetDocumentVersionAsType(id, documentType, v);
                if (versionResult.Success)
                {
                    viewModel.SelectedVersionId = v;
                    viewModel.SelectedVersion = versionResult.Document;
                }
            }
            else
            {
                viewModel.SelectedVersion = viewModel.CurrentVersion;
            }

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RestoreVersion(string id, string type, string versionId)
        {
            if (!CurrentUserCanEdit()) return new HttpUnauthorizedResult();

            var documentType = documentTypeResolver.GetDocumentType(type);
            var restoreResult = await documentService.RestoreDocumentVersion(id, documentType, versionId);

            if (!restoreResult.Success) throw new Exception("Could not restore document version.");

            // TODO: Change this to "restore" event, not updated
            await documentUpdateNotifier.DocumentUpdated(new DocumentUpdateNotification(restoreResult.RestoredDocument,
                GetCurrentUser()));

            return RedirectToAction("Index", new {id, type});

        }

        public async Task<ActionResult> Search(string query, string type = null, int page = 0, int pageSize = 20)
        {
            var viewModel = new DocumentSearchViewModel();

            var documentType = documentTypeResolver.GetDocumentType(type);

            var result = await documentService.SearchAllDocumentsByTitle(query, pageSize, page);

            if (result.Success)
            {
                viewModel.Results = result.Result;
                viewModel.Query = query;
                viewModel.PageIndex = page;
                viewModel.PageSize = pageSize;
                viewModel.TotalResult = result.TotalRecords;

                return View(viewModel);
            }
            else
            {
                throw new Exception("Could not load search results.");
            }

            

        }
    }
}