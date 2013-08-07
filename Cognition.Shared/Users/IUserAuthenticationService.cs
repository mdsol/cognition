using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognition.Shared.Users
{
    public interface IUserAuthenticationService
    {
        string GetCurrentUserEmail();
    }
}
