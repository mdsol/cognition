using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Cognition.Web.Models
{
    public class CognitionUser : User
    {
        public CognitionUser(string username, string name)
        {
            UserName = username;
            Name = name;
        }

        public string Name { get; set; }
        public string ProfilePictureUri { get; set; }
    }

    public class CognitionUserContext : IdentityStoreContext
    {
        public CognitionUserContext(DbContext db) : base(db)
        {
            Users = new UserStore<CognitionUser>(db);
        }
    }

    public class CognitionIdentityDbContext :
        IdentityDbContext<CognitionUser, UserClaim, UserSecret, UserLogin, Role, UserRole>
    {
        
    }
}