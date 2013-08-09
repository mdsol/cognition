using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.Documents;

namespace Cognition.Web.Tests.Mocks
{
    public class TestDocument : Document
    {
        public override string GetFullName()
        {
            return "Test Document";
        }

        public TestDocument()
        {
            Type = "test";
        }

        public string PropertyOne { get; set; }

        public string PropertyTwo { get; set; }
    }
}
