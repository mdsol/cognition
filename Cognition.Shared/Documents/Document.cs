using System;
using System.ComponentModel.DataAnnotations;

namespace Cognition.Shared.Documents
{
    public class Document
    {
        [ScaffoldColumn(false)]
        public virtual string Type
        {
            get;
            set;
        }

        public virtual string GetFullName()
        {
            return "Document";
        }

        public Document()
        {
            Type = "document";
        }

        public Document(string type)
        {
            Type = type;
        }

        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [ScaffoldColumn(false)]
        public string Ref { get; set; }

        [ScaffoldColumn(false)]
        public string CreatedByUserId { get; set; }

        [ScaffoldColumn(false)]
        public string LastUpdatedByUserId { get; set; }
        [Display(Order = -2)]
        [Required]
        public string Title { get; set; }
        [Display(Order = -1)]
        public string Subtitle { get; set; }
    }
}
