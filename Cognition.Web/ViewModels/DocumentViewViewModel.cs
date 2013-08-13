using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cognition.Shared.Documents;

namespace Cognition.Web.ViewModels
{
    public class DocumentViewViewModel
    {
        public Document Document { get; set; }
        public long PreviousVersionCount { get; set; }
        public bool CanEdit { get; set; }
    }
}