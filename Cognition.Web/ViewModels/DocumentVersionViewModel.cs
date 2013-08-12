using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cognition.Shared.Documents;
using Cognition.Shared.DocumentService;

namespace Cognition.Web.ViewModels
{
    public class DocumentVersionViewModel
    {
        public IEnumerable<DocumentAvailableVersion> AvailableVersions { get; set; }
        public Document CurrentVersion { get; set; }
        public Document SelectedVersion { get; set; }
        public string SelectedVersionId { get; set; }
    }
}