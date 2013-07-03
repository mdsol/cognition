using System;
using System.Web.Mvc;
using Cognition.Shared.Documents;
using Cognition.Web.Controllers;
using Cognition.Web.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace Cognition.Web.Tests.Controllers
{
    [TestClass]
    public class DocumentControllerTest
    {
        private DocumentController sut;
        private IFixture fixture;
        private IDocumentTypeResolver documentTypeResolver;

        [TestInitialize]
        public void Init()
        {
            documentTypeResolver = MockRepository.GenerateMock<IDocumentTypeResolver>();
            sut = new DocumentController(documentTypeResolver);
            fixture = new Fixture();
        }

        [TestMethod]
        public void Create_NewsUpNewInstanceOfDocumentTypeAndSetsViewModelType()
        {
            const string typeName = "test";
            documentTypeResolver.Stub(d => d.GetDocumentType(typeName)).Return(typeof(TestDocument));

            var result = (ViewResult)sut.Create(typeName);

            Assert.IsInstanceOfType(result.Model, typeof(TestDocument));

        }
    }
}
