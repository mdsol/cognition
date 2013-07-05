using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cognition.Shared.Documents
{
    // Used to persist attachments by the CouchDB document service
    public class DocumentAttachment
    {
        [JsonProperty(PropertyName = "content_type")]
        public string ContentType { get; set; }

        [JsonProperty(PropertyName = "revpos")]
        public int RevPos { get; set; }

        [JsonProperty(PropertyName = "digest")]
        public string Digest { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        [JsonProperty(PropertyName = "stub")]
        public bool Stub { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content
        {
            get;
            set;
        }
    }
}
