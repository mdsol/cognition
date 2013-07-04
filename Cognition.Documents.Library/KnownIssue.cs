﻿using System;
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
        [ScaffoldColumn(false)]
        public override string Type
        {
            get;
            set;
        }

        public KnownIssue()
        {
            Type = "issue";
        }

        public override string GetFullName()
        {
            return "Known Issue";
        }

        [DataType(DataType.Text)]
        public string Symptoms { get; set; }

        [DataType(DataType.Text)]
        public string Workarounds { get; set; }

        [Display(Name = "Applies to version")]
        public string AppliesTo { get; set; }

        [Display(Name = "Date acknowledged")]
        [DataType(DataType.Date)]
        public DateTime? DateAcknowledged { get; set; }
    }
}
