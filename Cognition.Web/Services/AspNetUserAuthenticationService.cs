using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Cognition.Shared.Users;
using Cognition.Web.Models;
using Microsoft.AspNet.Identity;

namespace Cognition.Web.Services
{
    public class AspNetUserAuthenticationService : IUserAuthenticationService
    {
        public bool IsAuthenticated { get { return HttpContext.Current.User.Identity.IsAuthenticated; } }

        public string GetCurrentUserEmail()
        {
            return HttpContext.Current.User.Identity.GetUserName();
        }

        public User GetUserByEmail(string email)
        {
            var context = new CognitionIdentityDbContext();
            var user = context.Users.SingleOrDefault(u => u.UserName == email);
            return user == null ? null : new User() { Email = email, FullName = user.Name };
        }
    }
}
