using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cognition.Changes.Entity;
using Cognition.Documents.CouchDb;
using Cognition.Documents.Entity;
using Cognition.Services.Configuration;
using Cognition.Services.Documents;
using Cognition.Services.Permissions;
using Cognition.Shared.Changes;
using Cognition.Shared.Configuration;
using Cognition.Shared.Documents;
using Cognition.Shared.DynamicTypes;
using Cognition.Shared.Permissions;
using Cognition.Shared.Users;
using Cognition.Support.Configuration;
using Cognition.Web.Services;
using Cognition.Web.Unity;
using Microsoft.Practices.Unity;
using Owin;

namespace Cognition.Web
{
    public partial class Startup
    {
        public static IUnityContainer Container { get; set; }

        public void ConfigureUnity(IAppBuilder app)
        {
            Container = new UnityContainer();

            // set MVC4/5 resolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));

            Container.RegisterType<IAppSettingProvider, AppSettingProvider>();

            Container.RegisterType<IDocumentTypeResolver, StaticDocumentTypeResolver>();
            Container.RegisterType<IUserAuthenticationService, AspNetUserAuthenticationService>();
            Container.RegisterType<IDocumentService, CouchDbDocumentService>();
            Container.RegisterType<IDocumentUpdateNotifier, SignalRDocumentUpdateNotifier>();
            Container.RegisterType<IDocumentChangeService, EFDocumentChangeService>();
            
            Container.RegisterType<IPermissionService, TokenStringPermissionService>();
            Container.RegisterType<IPermissionTokenProvider, AppSettingPermissionTokenProvider>();

            Container.RegisterType<IDynamicTypeService, EFDynamicTypeService>();

        }
    }
}