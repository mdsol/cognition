﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognition.Shared.Configuration
{
    public interface IAppSettingProvider
    {
        bool GetBool(string key);
        string GetString(string key);
    }
}
