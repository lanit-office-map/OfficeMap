﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IFindAll<TEntity, TFilter>
        where TEntity : class
        where TFilter : class
    {
        Task<IEnumerable<TEntity>> FindAllAsync(TFilter filter = null);
    }
}
