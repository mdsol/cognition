using System;
using System.ComponentModel.DataAnnotations;

namespace Cognition.Shared.DynamicTypes
{
    public class DynamicTypeDefinition
    {
        [Key]
        public Guid Id { get; set; }

        public string Tenant { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
