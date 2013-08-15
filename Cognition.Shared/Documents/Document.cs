using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Cognition.Shared.Documents
{
    public class Document
    {
        public enum VisibilityStatus
        {
            Internal = 0,
            Public = 1,
        }

        [DataType("Enum")]
        [Display(Name = "Visibility", Order = -1)]
        public VisibilityStatus Visibility { get; set; }

        [ScaffoldColumn(false)]
        public string Type
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
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [ScaffoldColumn(false)]
        [JsonProperty(PropertyName = "_rev")]
        public string Rev { get; set; }

        [ScaffoldColumn(false)]
        [JsonProperty(PropertyName = "_attachments")]
        public Dictionary<string, DocumentAttachment> Attachments
        {
            get;
            set;
        }
        
        [ScaffoldColumn(false)]
        public string CreatedByUserId { get; set; }

        [ScaffoldColumn(false)]
        public string LastUpdatedByUserId { get; set; }

        [Display(Order = -3)]
        [Required]
        [MaxLength(100)]
        public virtual string Title { get; set; }

        [Display(Order = -2)]
        public string Subtitle { get; set; }

        [ScaffoldColumn(false)]
        public virtual Func<Document, string> MarkdownTemplate { get { return null; } } 
    }
}
