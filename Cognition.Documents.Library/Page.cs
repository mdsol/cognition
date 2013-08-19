using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Documents.Library
{
    public class Page : Document
    {
        public Page()
        {
            Type = "page";
        }

        public override string GetFullName()
        {
            return "Page";
        }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Body text")]
        public string Body { get; set; }

        public override Func<Document, string> GetMarkdownTemplate()
        {
            return (d => ((Page) d).Body);
        }
    }
}
