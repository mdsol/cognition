using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.DynamicTypes;

namespace Cognition.Documents.Entity
{
    public class TypeContext : DbContext
    {
        public DbSet<DynamicTypeDefinition> DynamicTypes { get; set; }

        public TypeContext()
            : base("DefaultConnection")
        { }
    }
}
