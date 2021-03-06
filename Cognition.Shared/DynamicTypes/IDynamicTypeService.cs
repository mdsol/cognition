﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cognition.Shared.DynamicTypes
{
    public interface IDynamicTypeService
    {
        Task<IEnumerable<DynamicTypeDefinition>> GetAllAsync();
        IEnumerable<DynamicTypeDefinition> GetAll(); 
        Task<DynamicTypeDefinition> GetTypeById(Guid id);
        Task AddOrUpdateType(DynamicTypeDefinition type);
    }
}