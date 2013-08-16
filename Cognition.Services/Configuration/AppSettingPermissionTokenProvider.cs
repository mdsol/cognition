using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Configuration;

namespace Cognition.Services.Configuration
{
    public class AppSettingPermissionTokenProvider : IPermissionTokenProvider
    {
        private readonly IAppSettingProvider appSettingProvider;

        public AppSettingPermissionTokenProvider(IAppSettingProvider appSettingProvider)
        {
            this.appSettingProvider = appSettingProvider;
        }

        public string GetTokensForViewPublic()
        {
            return appSettingProvider.GetString("ViewPublicPermissions");
        }

        public string GetTokensForViewInternal()
        {
            return appSettingProvider.GetString("ViewInternalPermissions");
        }

        public string GetTokenForEdit()
        {
            return appSettingProvider.GetString("EditPermissions");
        }

        public string GetTokenForRegistration()
        {
            return appSettingProvider.GetString("RegistrationPermissions");
        }

        public string GetTokenForAdmin()
        {
            return appSettingProvider.GetString("AdminPermissions");
        }
    }
}
