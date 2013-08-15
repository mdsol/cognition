using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognition.Shared.DynamicTypes;

namespace Cognition.Documents.Entity
{
    public class EFDynamicTypeService : IDynamicTypeService
    {
        public async Task<IEnumerable<DynamicTypeDefinition>> GetAll()
        {
            using (var context = new TypeContext())
            {
                return await context.DynamicTypes.ToListAsync();
            }

        }

        public async Task<DynamicTypeDefinition> GetTypeById(Guid id)
        {
            using (var context = new TypeContext())
            {
                return await context.DynamicTypes.FindAsync(id);
            }
        }
    }
}
