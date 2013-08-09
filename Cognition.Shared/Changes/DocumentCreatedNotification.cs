using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;
using Cognition.Shared.Users;

namespace Cognition.Shared.Changes
{
    public class DocumentCreatedNotification : DocumentChangeNotification
    {
        public DocumentCreatedNotification()
        {
            ChangeType = ChangeType.Created;
        }

        public DocumentCreatedNotification(Document document, User user)
            : this()
        {
            Id = Guid.NewGuid();
            DocumentId = document.Id;
            Type = document.Type;
            Title = document.Title;
            ByUserId = user.Email;
            ByUserName = user.FullName;
            DateTime = System.DateTime.UtcNow;
        }
    }
}
