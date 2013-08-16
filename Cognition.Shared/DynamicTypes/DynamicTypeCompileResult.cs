using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cognition.Shared.DynamicTypes
{
    public class DynamicTypeCompileResult
    {
        public bool Success { get; set; }
        public IEnumerable<DynamicTypeCodeCompileError> Errors { get; set; }
        public Type Result { get; set; }


    }
}
