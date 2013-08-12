using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Documents.Library
{

    public class KnownIssue : Document
    {
        public KnownIssue()
        {
            Type = "issue";
        }

        public override string GetFullName()
        {
            return "Known Issue";
        }

        [Required]
        public string Symptoms { get; set; }

        public string Workarounds { get; set; }

        [Display(Name = "Applies to version")]
        public string AppliesTo { get; set; }

        [Display(Name = "Date acknowledged")]
        [DataType(DataType.Date)]
        public DateTime? DateAcknowledged { get; set; }

        public enum Severity
        {
            Low = 0,
            Medium = 5,
            High = 10
        }

        [Display(Name = "Issue Severity")]
        [DataType("Enum")]
        public Severity IssueSeverity { get; set; }
        
    }

}
