using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Configuration;
using Cognition.Shared.Permissions;
using Cognition.Shared.Users;

namespace Cognition.Services.Permissions
{
    public class TokenStringPermissionService : IPermissionService
    {
        private readonly IUserAuthenticationService userAuthenticationService;
        private readonly IPermissionTokenProvider permissionTokenProvider;

        public TokenStringPermissionService(IUserAuthenticationService userAuthenticationService, IPermissionTokenProvider permissionTokenProvider)
        {
            this.userAuthenticationService = userAuthenticationService;
            this.permissionTokenProvider = permissionTokenProvider;
        }

        public bool CurrentUserCanViewPublic()
        {
            var tokens = permissionTokenProvider.GetTokensForViewPublic();
            return ParseTokensAndValidate(tokens);
        }

        public bool ParseTokensAndValidate(string tokens, string userId = null)
        {
            var tokenSplit = tokens.Split(',');

            foreach (var trimmedToken in tokenSplit.Select(token => token.Trim()))
            {
                if (trimmedToken == "anonymous") return true; // all users can do this action
                if (trimmedToken == "registered" && userAuthenticationService.IsAuthenticated) return true; // all registered users can do this action

                string email = null;
                if (userId != null) email = userId;
                else
                {
                    if (userAuthenticationService.IsAuthenticated)
                    {
                        email = userAuthenticationService.GetCurrentUserEmail();
                    }
                }
                if (String.IsNullOrWhiteSpace(email)) continue;

                if (trimmedToken == "all") return true;

                // do a filter on the email address.
                // for example, if the token is @mdsol.com, all mdsol.com emails will pass
                // if the token is eandersen@mdsol.com, emails ending with eandersen@mdsol.com will pass
                if (email.EndsWith(trimmedToken)) return true;
            }

            return false;
        }


        public bool CurrentUserCanViewInternal()
        {
            var tokens = permissionTokenProvider.GetTokensForViewInternal();
            return ParseTokensAndValidate(tokens);
        }

        public bool CurrentUserCanEdit()
        {
            var tokens = permissionTokenProvider.GetTokenForEdit();
            return ParseTokensAndValidate(tokens);
        }

        public bool CanUserRegister(string userId)
        {
            var tokens = permissionTokenProvider.GetTokenForRegistration();
            return ParseTokensAndValidate(tokens);
        }

        public bool CanUserAdmin()
        {
            var tokens = permissionTokenProvider.GetTokenForAdmin();
            return ParseTokensAndValidate(tokens);
        }
    }
}
