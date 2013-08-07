using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Cognition.Shared.Users;

namespace Cognition.Web.Controllers
{
    public class UserController : AsyncController
    {
        public readonly IUserAuthenticationService UserAuthenticationService;

        public UserController(IUserAuthenticationService userAuthenticationService)
        {
            UserAuthenticationService = userAuthenticationService;
        }

        public async Task<PartialViewResult> UserInfo(string id)
        {
            return PartialView(UserAuthenticationService.GetUserByEmail(id));
        }
    }
}