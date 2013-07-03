using System;

namespace Cognition.Shared.Documents
{
    public interface IDocumentTypeResolver
    {
        Type GetDocumentType(string type);
    }
}