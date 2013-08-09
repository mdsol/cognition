using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Changes;

namespace Cognition.Changes.Entity
{
    public class EFDocumentChangeService : IDocumentChangeService
    {
        public async Task<IEnumerable<DocumentChangeNotification>> GetLatestChangesAsync(int limit)
        {
            using (var context = new ChangesContext())
            {
                return
                    await context.DocumentChangeNotifications.OrderByDescending(c => c.DateTime).Take(limit).ToListAsync();
            }
        }

        public IEnumerable<DocumentChangeNotification> GetLatestChanges(int limit)
        {
            using (var context = new ChangesContext())
            {
                return
                    context.DocumentChangeNotifications.OrderByDescending(c => c.DateTime).Take(limit).ToList();
            }
        }

        public async Task AddChange(DocumentChangeNotification change)
        {
            using (var context = new ChangesContext())
            {
                context.DocumentChangeNotifications.Add(change);
                await context.SaveChangesAsync();
            }
        }
    }
}
