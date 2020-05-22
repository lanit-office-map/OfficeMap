using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkplaceService.Database;
using WorkplaceService.Database.Entities;
using WorkplaceService.Repository.Interfaces;

namespace WorkplaceService.Repository
{
    public class WorkplaceRepository : IWorkplaceRepository
    {
        private WorkplaceServiceDbContext dbContext;

        public WorkplaceRepository([FromServices] WorkplaceServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<DbWorkplace>> FindAllAsync(Expression<Func<DbWorkplace, bool>> expression)
        {
            return await dbContext.Workplaces.AsNoTracking()
                .Include(w => w.Map)
                .Where(expression)
                .ToListAsync();
        }

        public async Task<DbWorkplace> GetAsync(Guid workplaceGuid)
        {
            return await dbContext.Workplaces
                .Include(w => w.Map)
                .SingleAsync(w => w.Guid == workplaceGuid);
        }

        public async Task<DbWorkplace> CreateAsync(DbWorkplace workplace)
        {
            var entity = await dbContext.Workplaces.AddAsync(workplace);
            dbContext.SaveChanges();
            return entity.Entity;
        }

        public async Task<DbWorkplace> UpdateAsync(DbWorkplace workplace)
        {
            var entity = dbContext.Workplaces.Update(workplace).Entity;
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(DbWorkplace workplace)
        {
            dbContext.Workplaces.Remove(workplace);
            await dbContext.SaveChangesAsync();
        }
    }
}
