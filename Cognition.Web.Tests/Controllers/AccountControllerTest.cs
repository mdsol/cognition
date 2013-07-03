using System;
using System.Web.Mvc;
using Cognition.Shared.Configuration;
using Cognition.Web.Controllers;
using Cognition.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace Cognition.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        private IAppSettingProvider appSettingProvider;
        private AccountController sut;
        private IFixture fixture;

        [TestInitialize]
        public void Init()
        {
            appSettingProvider = MockRepository.GenerateMock<IAppSettingProvider>();
            sut = new AccountController(appSettingProvider);
            fixture = new Fixture();
        }

        [TestMethod]
        public void Register_ReturnsNotAuthorizedWhenCannotRegister()
        {
            appSettingProvider.Stub(a => a.GetBool("AllowRegistration")).Return(false);

            var result = sut.Register();
            Assert.IsInstanceOfType(result, typeof(HttpUnauthorizedResult));

            var postResult = sut.Register(fixture.Create<RegisterViewModel>()).Result;
            Assert.IsInstanceOfType(postResult, typeof(HttpUnauthorizedResult));


        }
    }
}
