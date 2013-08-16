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
        public async Task<IEnumerable<DynamicTypeDefinition>> GetAllAsync()
        {
            using (var context = new TypeContext())
            {
                return await context.DynamicTypes.ToListAsync();
            }

        }

        public IEnumerable<DynamicTypeDefinition> GetAll()
        {
            using (var context = new TypeContext())
            {
                return context.DynamicTypes.ToList();
            }
        }

        public async Task<DynamicTypeDefinition> GetTypeById(Guid id)
        {
            using (var context = new TypeContext())
            {
                return await context.DynamicTypes.FindAsync(id);
            }
        }

        public async Task AddOrUpdateType(DynamicTypeDefinition type)
        {
            // try to find existing
            using (var context = new TypeContext())
            {
                var existing = await context.DynamicTypes.FindAsync(type.Id);
                if (existing == null)
                {
                    context.DynamicTypes.Add(type);
                }
                else
                {
                    existing.Name = type.Name;
                    existing.Tenant = type.Tenant;
                    existing.Code = type.Code;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
