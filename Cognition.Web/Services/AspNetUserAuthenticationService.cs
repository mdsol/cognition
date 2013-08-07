using System;
using System.Linq;
using System.Web;
using Cognition.Shared.Users;
using Cognition.Web.Models;
using Microsoft.AspNet.Identity;

namespace Cognition.Web.Services
{
    public class AspNetUserAuthenticationService : IUserAuthenticationService
    {
        public string GetCurrentUserEmail()
        {
            return HttpContext.Current.User.Identity.GetUserName();
        }

        public User GetUserByEmail(string email)
        {
            var context = new CognitionIdentityDbContext();
            var user = context.Users.Single(u => u.UserName == email);
            return new User() { Email = email, FullName = user.Name };
        }
    }
}
