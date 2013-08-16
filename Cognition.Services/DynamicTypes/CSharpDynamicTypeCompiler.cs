using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;
using Cognition.Shared.DynamicTypes;
using Microsoft.CSharp;

namespace Cognition.Services.DynamicTypes
{
    public class CSharpDynamicTypeCompiler : IDynamicTypeCompiler
    {
        public DynamicTypeCompileResult Compile(string code, string binPath)
        {
            var result = new DynamicTypeCompileResult();

            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add(Path.Combine(binPath, "Cognition.Shared.dll"));
            parameters.ReferencedAssemblies.Add("mscorlib.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            parameters.ReferencedAssemblies.Add("System.ComponentModel.DataAnnotations.dll");
            parameters.ReferencedAssemblies.Add("System.Linq.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.Linq.dll");

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (results.Errors != null && results.Errors.HasErrors)
            {
                result.Errors = results.Errors.Cast<CompilerError>().Select(e =>
                    new DynamicTypeCodeCompileError()
                    {
                        Column = e.Column,
                        ErrorText = e.ErrorText,
                        IsWarning = e.IsWarning,
                        LineNumber = e.Line
                    });

                return result;
            }

            // success
            result.Success = true;

            result.Result = results.CompiledAssembly.GetTypes().First(t => t.IsSubclassOf(typeof (Document)));

            return result;
        }
    }
}
