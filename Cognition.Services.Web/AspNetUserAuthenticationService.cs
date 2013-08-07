using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Cognition.Shared.Users;
using Microsoft.AspNet.Identity;

namespace Cognition.Services.Web
{
    public class AspNetUserAuthenticationService : IUserAuthenticationService
    {
        public string GetCurrentUserEmail()
        {
            return HttpContext.Current.User.Identity.GetUserName();
        }
    }
}
