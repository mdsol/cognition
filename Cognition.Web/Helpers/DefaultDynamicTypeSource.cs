using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Cognition.Web.Properties;

namespace Cognition.Web.Helpers
{
    public class DefaultDynamicTypeSource
    {
        public static string DefaultSource
        {
            get
            {
                return Resources.DefaultTypeSource;
            }
        }
    }
}