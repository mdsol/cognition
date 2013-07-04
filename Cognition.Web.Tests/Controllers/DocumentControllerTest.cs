using System;
using System.Collections.Specialized;
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

        const string typeName = "test";

        [TestInitialize]
        public void Init()
        {
            documentTypeResolver = MockRepository.GenerateMock<IDocumentTypeResolver>();
            sut = new DocumentController(documentTypeResolver);
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
        public void Create_Post_SetsValuesOnNewDocument()
        {
            const string testTitle = "test title";
            const string propOne = "property one";
            new TestControllerBuilder().InitializeController(sut);
            var formValues = new FormCollection() { { "Title", testTitle }, {"Type", typeName}, {"PropertyOne", propOne} };
            sut.ValueProvider = formValues.ToValueProvider();
            var result = (ViewResult)sut.Create(formValues);
            var document = (TestDocument)result.Model;

            Assert.AreEqual(testTitle, document.Title);
            Assert.AreEqual(propOne, document.PropertyOne);
        }
    }
}
