using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkplaceService.Database.Entities;

namespace WorkplaceService.Repository.Interfaces
{
    public interface IWorkplaceRepository
    {
        public Task<IEnumerable<DbWorkplace>> FindAllAsync(Expression<Func<DbWorkplace, bool>> expression);
        public Task<DbWorkplace> GetAsync(Guid workplaceGuid);
        public Task<DbWorkplace> CreateAsync(DbWorkplace workplace);
        public Task<DbWorkplace> UpdateAsync(DbWorkplace workplace);
        public Task DeleteAsync(DbWorkplace workplace);
    }
}
