using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cognition.Shared.Documents;
using Cognition.Web.Controllers;
using Cognition.Web.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private MockDocumentService documentService;

        const string typeName = "test";

        [TestInitialize]
        public void Init()
        {
            documentTypeResolver = MockRepository.GenerateMock<IDocumentTypeResolver>();
            documentService = new MockDocumentService();
            sut = new DocumentController(documentTypeResolver, documentService);
            documentTypeResolver.Stub(d => d.GetDocumentType(typeName)).Return(typeof(TestDocument));
            fixture = new Fixture();
        }

        [TestMethod]
        public void Create_NewsUpNewInstanceOfDocumentTypeAndSetsViewModelType()
        {
            var result = (ViewResult)sut.Create(typeName);

            Assert.IsInstanceOfType(result.Model, typeof(TestDocument));

        }

        [TestMethod]
        public async Task Create_Post_SetsValuesOnNewDocument()
        {
            const string testTitle = "test title";
            const string propOne = "property one";
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() { { "Title", testTitle }, { "Type", typeName }, { "PropertyOne", propOne } };
            sut.ValueProvider = formValues.ToValueProvider();

            await sut.Create(formValues);
            dynamic document = documentService.Documents.Single();

            Assert.AreEqual(testTitle, document.Title);
            Assert.AreEqual(propOne, document.PropertyOne);
        }

        [TestMethod]
        public async Task Create_Post_AddsDocumentToDocumentService()
        {
            const string testTitle = "test title";
            const string propOne = "property one";
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() { { "Title", testTitle }, { "Type", typeName }, { "PropertyOne", propOne } };
            sut.ValueProvider = formValues.ToValueProvider();

            await sut.Create(formValues);

            var document = documentService.Documents.Single();
            Assert.IsInstanceOfType(document, typeof(TestDocument));

        }

        [TestMethod]
        public async Task Create_Post_RedirectsToViewOfDocumentOnPost()
        {
            const string testTitle = "test title";
            const string propOne = "property one";
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() { { "Title", testTitle }, { "Type", typeName }, { "PropertyOne", propOne } };
            sut.ValueProvider = formValues.ToValueProvider();

            var result = (RedirectToRouteResult) await sut.Create(formValues);

            var document = documentService.Documents.Single();
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(document.Id, result.RouteValues["id"]);
            Assert.AreEqual(document.Type, result.RouteValues["type"]);

        }

        [TestMethod]
        public void Index_GetsDocumentFromServiceAndSetsItAsModel()
        {
            var id = fixture.Create<string>();
            var document = new TestDocument {Id = id};
            documentService.Documents.Add(document);

            var result = (ViewResult) sut.Index(id, typeName).Result;

            Assert.AreEqual(document, result.Model);

        }
    }
}
