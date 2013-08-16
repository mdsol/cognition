using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Cognition.Shared.DynamicTypes
{
    public class DynamicTypeDefinition
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Tenant { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [AllowHtml]
        public string Code { get; set; }
    }
}
