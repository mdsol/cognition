using System;
using System.Collections;
using System.Collections.Generic;

namespace Cognition.Shared.Documents
{
    public interface IDocumentTypeResolver
    {
        Type GetDocumentType(string type);
        string GetDocumentTypeFullName(string typeName);
        IEnumerable<DocumentTypeReference> AllAvailableTypes { get; }
    }
}