using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognition.Shared.DynamicTypes
{
    public class DynamicTypeCodeCompileError
    {
        public long LineNumber { get; set; }
        public long Column { get; set; }
        public string ErrorText { get; set; }
        public bool IsWarning { get; set; }

        public override string ToString()
        {
            return String.Format("Line {0} Col {1}: {2}", LineNumber, Column, ErrorText);
        }
    }
}
