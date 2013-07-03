using System;

namespace Cognition.Shared.Documents
{
    public class Document
    {
        public virtual string Type { get { return "document"; } }

        public virtual string GetFullName()
        {
            return "Document";
        }

        public string Id { get; set; }
        public string Ref { get; set; }
        public string CreatedByUserId { get; set; }
        public string LastUpdatedByUserId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
