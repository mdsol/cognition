using System;

namespace Cognition.Shared.Changes
{
    public enum ChangeType
    {
        Created = 1,
        Updated = 2,
    }

    public class DocumentChangeNotification
    {
        public Guid Id { get; set; }
        public string DocumentId { get; set; }
        public string Type { get; set; }
        public ChangeType ChangeType { get; set; }
        public string Title { get; set; }
        public string ByUserId { get; set; }
        public string ByUserName { get; set; }
        public DateTime DateTime { get; set; }
    }
}
