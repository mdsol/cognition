using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;
using Cognition.Shared.DynamicTypes;

namespace Cognition.Services.DynamicTypes
{
    public class DynamicDocumentTypeResolver : IDocumentTypeResolver
    {
        private static ConcurrentDictionary<string, DocumentTypeReference> types = new ConcurrentDictionary<string, DocumentTypeReference>();

        private readonly IDynamicTypeService dynamicTypeService;
        private readonly IDynamicTypeCompiler dynamicTypeCompiler;

        public DynamicDocumentTypeResolver(IDynamicTypeService dynamicTypeService, IDynamicTypeCompiler dynamicTypeCompiler)
        {
            this.dynamicTypeService = dynamicTypeService;
            this.dynamicTypeCompiler = dynamicTypeCompiler;
        }

        public Type GetDocumentType(string type)
        {
            var found = types.Values.FirstOrDefault(t => t.TypeName == type);
            if (found == null) return typeof(Document); // fall back if type is missing
            return found.Type;
        }

        public string GetDocumentTypeFullName(string typeName)
        {
            return types.Values.First(t => t.TypeName == typeName).FullTypeName;
        }

        private void UpdateAllTypes()
        {
            var dynamicTypes = dynamicTypeService.GetAll();

            var foundHashes = new List<string>();
            foreach (var dynamicType in dynamicTypes)
            {

                var hash = CalculateMD5Hash(dynamicType.Code);
                foundHashes.Add(hash);
                if (types.ContainsKey(hash)) continue; // already compiled
                
                var compileResult = dynamicTypeCompiler.Compile(dynamicType.Code,
                    System.AppDomain.CurrentDomain.BaseDirectory + "\\bin");

                if (compileResult.Success)
                {
                    var reference = new DocumentTypeReference();
                    var documentType = (Document)Activator.CreateInstance(compileResult.Result);
                    reference.Type = compileResult.Result;
                    reference.FullTypeName = documentType.GetFullName();
                    reference.TypeName = documentType.Type;

                    types[hash] = reference;
                }

            }

            // remove all no longer present hashes
            foreach (var key in types.Keys.Where(key => !foundHashes.Contains(key)))
            {
                DocumentTypeReference removed;
                types.TryRemove(key, out removed);
            }
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        public IEnumerable<DocumentTypeReference> AllAvailableTypes
        {
            get
            {
                UpdateAllTypes();
                return types.Values;
            }
        }
    }
}
