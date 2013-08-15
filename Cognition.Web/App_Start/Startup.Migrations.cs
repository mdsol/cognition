using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Cognition.Changes.Entity;
using Cognition.Documents.Entity;
using Cognition.Web.Migrations;
using Cognition.Web.Models;
using Owin;

namespace Cognition.Web
{
	public partial class Startup
	{
	    public void ConfigureMigrations(IAppBuilder app)
	    {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CognitionIdentityDbContext, Configuration>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChangesContext, Changes.Entity.Migrations.Configuration>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TypeContext, Documents.Entity.Migrations.Configuration>());
	  
        
        }

	}
}