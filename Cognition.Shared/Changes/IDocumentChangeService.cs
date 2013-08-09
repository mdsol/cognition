using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognition.Shared.Changes
{
    public interface IDocumentChangeService
    {
        IEnumerable<DocumentChangeNotification> GetLatestChanges(int limit);
        Task<IEnumerable<DocumentChangeNotification>> GetLatestChangesAsync(int limit);
        Task AddChange(DocumentChangeNotification change);
        Task<DocumentChangeNotification> GetSingleAsync(Guid id);
    }
}