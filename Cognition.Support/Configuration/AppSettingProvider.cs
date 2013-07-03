using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Configuration;

namespace Cognition.Support.Configuration
{
    public class AppSettingProvider : IAppSettingProvider
    {
        public bool GetBool(string key)
        {
            return bool.Parse(ConfigurationManager.AppSettings[key]);
        }
    }
}
