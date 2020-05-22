﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFindAll<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> FindAllAsync();
    }
}