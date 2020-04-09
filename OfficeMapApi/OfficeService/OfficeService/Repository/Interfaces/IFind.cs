using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeService.Repository.Interfaces
{
    public interface IFind<TEntity, TFilter>
            where TFilter : class
    {
        Task <IEnumerable<TEntity>> FindAsync(TFilter filter = null);
    }
}
