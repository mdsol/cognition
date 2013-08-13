using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognition.Shared.Users
{
    public interface IUserAuthenticationService
    {
        bool IsAuthenticated { get; }
        string GetCurrentUserEmail();
        User GetUserByEmail(string email);
    }
}
