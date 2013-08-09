using System;
using Cognition.Shared.Documents;
using Cognition.Shared.Users;

namespace Cognition.Shared.Changes
{
    public class DocumentUpdateNotification : DocumentChangeNotification
    {
        public DocumentUpdateNotification()
        {
            ChangeType = ChangeType.Updated;
        }

        public DocumentUpdateNotification(Document document, User user) : this()
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
