using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cognition.Shared.Changes;
using Cognition.Shared.Documents;
using Cognition.Shared.Permissions;
using Cognition.Shared.Users;
using Cognition.Web.Controllers;
using Cognition.Web.Tests.Mocks;
using Cognition.Web.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper.Ui;
using Ploeh.AutoFixture;
using Rhino.Mocks;
using MvcContrib.TestHelper;

namespace Cognition.Web.Tests.Controllers
{
    [TestClass]
    public class DocumentControllerTest
    {
        private DocumentController sut;
        private IFixture fixture;
        private IDocumentTypeResolver documentTypeResolver;
        private IUserAuthenticationService userAuthenticationService;
        private IDocumentUpdateNotifier documentUpdateNotifier;
        private IPermissionService permissionService;
        private MockDocumentService documentService;

        private const string typeName = "test";
        private const string userEmail = "user@email.com";

        [TestInitialize]
        public void Init()
        {
            documentTypeResolver = MockRepository.GenerateMock<IDocumentTypeResolver>();
            userAuthenticationService = MockRepository.GenerateMock<IUserAuthenticationService>();
            permissionService = MockRepository.GenerateMock<IPermissionService>();
            documentUpdateNotifier = new MockDocumentUpdateNotifier();
            userAuthenticationService.Stub(u => u.GetCurrentUserEmail()).Return(userEmail);
            userAuthenticationService.Stub(u => u.GetUserByEmail(userEmail)).Return(new User());
            documentService = new MockDocumentService();
            sut = new DocumentController(documentTypeResolver, documentService, userAuthenticationService, documentUpdateNotifier, permissionService);
            documentTypeResolver.Stub(d => d.GetDocumentType(typeName)).Return(typeof (TestDocument));
            fixture = new Fixture();
        }

        [TestMethod]
        public void Create_NewsUpNewInstanceOfDocumentTypeAndSetsViewModelType()
        {
            var result = (ViewResult) sut.Create(typeName);

            Assert.IsInstanceOfType(result.Model, typeof (TestDocument));

        }

        [TestMethod]
        public async Task Create_Post_SetsValuesOnNewDocument()
        {
            const string testTitle = "test title";
            const string propOne = "property one";
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() {{"Title", testTitle}, {"Type", typeName}, {"PropertyOne", propOne}};
            sut.ValueProvider = formValues.ToValueProvider();

            await sut.Create(formValues);
            dynamic document = documentService.Documents.Single();

            Assert.AreEqual(testTitle, document.Title);
            Assert.AreEqual(propOne, document.PropertyOne);
        }

        [TestMethod]
        public async Task Create_Post_SetsCurrentUserEmailAddressToCreatedByField()
        {
            userAuthenticationService.Stub(s => s.GetCurrentUserEmail()).Return(userEmail);
            var formValues = SetupCreateAction();

            await sut.Create(formValues);

            var document = documentService.Documents.Single();
            Assert.AreEqual(userEmail, document.CreatedByUserId);

        }

        [TestMethod]
        public async Task Create_Post_AddsDocumentToDocumentService()
        {
            var formValues = SetupCreateAction();

            await sut.Create(formValues);

            var document = documentService.Documents.Single();
            Assert.IsInstanceOfType(document, typeof(TestDocument));

        }

        private FormCollection SetupCreateAction()
        {
            const string testTitle = "test title";
            const string propOne = "property one";
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() {{"Title", testTitle}, {"Type", typeName}, {"PropertyOne", propOne}};
            sut.ValueProvider = formValues.ToValueProvider();
            return formValues;
        }

        [TestMethod]
        public async Task Create_Post_RedirectsToViewOfDocumentOnPost()
        {
            const string testTitle = "test title";
            const string propOne = "property one";
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() { { "Title", testTitle }, { "Type", typeName }, { "PropertyOne", propOne } };
            sut.ValueProvider = formValues.ToValueProvider();

            var result = (RedirectToRouteResult)await sut.Create(formValues);

            var document = documentService.Documents.Single();
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(document.Id, result.RouteValues["id"]);
            Assert.AreEqual(document.Type, result.RouteValues["type"]);

        }

        private void StubAllPermissionsTrue()
        {
            permissionService.Stub(s => s.CurrentUserCanEdit()).Return(true);
            permissionService.Stub(s => s.CurrentUserCanViewInternal()).Return(true);
            permissionService.Stub(s => s.CurrentUserCanViewPublic()).Return(true);
            permissionService.Stub(s => s.CanUserRegister(null)).IgnoreArguments().Return(true);
        }

        [TestMethod]
        public void Index_GetsDocumentFromServiceAndSetsItAsModel()
        {
            var id = fixture.Create<string>();
            var document = new TestDocument { Id = id };
            documentService.Documents.Add(document);
            StubAllPermissionsTrue();

            var result = (ViewResult)sut.Index(id, typeName).Result;

            var viewModel = (DocumentViewViewModel) result.Model;

            Assert.AreEqual(document, viewModel.Document);

        }

        [TestMethod]
        public async Task Edit_Get_SetsModelToRetrievedDocument()
        {
            var document = fixture.Create<TestDocument>();
            documentService.Documents.Add(document);

            var result = (ViewResult)await sut.Edit(document.Id, document.Type);

            Assert.AreEqual(document, result.Model);

        }

        [TestMethod]
        public async Task Edit_Post_UpdatesDocumentWithFormCollectionValuesThenRedirectsToView()
        {
            var existingDocument = fixture.Create<TestDocument>();
            documentService.Documents.Add(existingDocument);
            var newTitle = fixture.Create<string>();
            var newPropertyOne = fixture.Create<string>();
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() { { "Title", newTitle }, {"Id", existingDocument.Id}, { "Type", typeName }, { "PropertyOne", newPropertyOne } };
            sut.ValueProvider = formValues.ToValueProvider();

            var redirectResult = (RedirectToRouteResult)await sut.Edit(formValues);

            var newDocument = (TestDocument)documentService.Documents.Single();
            Assert.AreEqual(newTitle, newDocument.Title);
            Assert.AreEqual(newPropertyOne, newDocument.PropertyOne);

            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);
            Assert.AreEqual(newDocument.Id, redirectResult.RouteValues["id"]);
            Assert.AreEqual(newDocument.Type, redirectResult.RouteValues["type"]);

        }

        [TestMethod]
        public async Task Edit_Post_SetsUpdatedByUserIdOnDocument()
        {
            var existingDocument = fixture.Create<TestDocument>();
            documentService.Documents.Add(existingDocument);
            var newTitle = fixture.Create<string>();
            var newPropertyOne = fixture.Create<string>();
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() { { "Title", newTitle }, { "Id", existingDocument.Id }, { "Type", typeName }, { "PropertyOne", newPropertyOne } };
            sut.ValueProvider = formValues.ToValueProvider();
            userAuthenticationService.Stub(u => u.GetCurrentUserEmail()).Return(userEmail);

            await sut.Edit(formValues);

            var newDocument = (TestDocument) documentService.Documents.Single();

            Assert.AreEqual(userEmail, newDocument.LastUpdatedByUserId);




        }

        [TestMethod]
        public async Task List_SetsViewTypeToCorrectViewModel()
        {
            var result = (ViewResult) await sut.List(typeName);
            Assert.IsInstanceOfType(result.Model, typeof(DocumentListViewModel));
        }

        [TestMethod]
        public async Task List_SetsDocumentsFromServiceResult()
        {
            var model = await GetDefaultListActionModelResult();

            CollectionAssert.AreEquivalent(documentService.Documents.Select(d => d.Id).ToList(), 
                model.Documents.Select(d => d.Id).ToList());
        }

        private async Task<DocumentListViewModel> GetDefaultListActionModelResult()
        {
            var documents = fixture.CreateMany<TestDocument>();
            documentService.Documents.AddRange(documents);
            foreach (var doc in documentService.Documents) doc.Type = typeName;

            var result = (ViewResult) await sut.List(typeName, documentService.Documents.Count, 0);
            var model = (DocumentListViewModel) result.Model;
            return model;
        }

        [TestMethod]
        public async Task List_SetsPropertiesFromServiceResult()
        {
            var model = await GetDefaultListActionModelResult();

            Assert.AreEqual(documentService.Documents.Count, model.TotalDocuments);
            Assert.AreEqual(documentService.Documents.Count, model.PageSize);
            Assert.AreEqual(0, model.PageIndex);
        }

        [TestMethod]
        public async Task List_SetsTypeNameInViewModel()
        {
            var model = await GetDefaultListActionModelResult();

            Assert.AreEqual(typeName, model.TypeName);
        }

        [TestMethod]
        public async Task List_SetsFullTypeNameInViewModel()
        {
            var typeFullName = fixture.Create<string>();
            documentTypeResolver.Stub(t => t.GetDocumentTypeFullName(typeName)).Return(typeFullName);

            var model = await GetDefaultListActionModelResult();

            Assert.AreEqual(typeFullName, model.TypeFullName);
        }


    }
}
