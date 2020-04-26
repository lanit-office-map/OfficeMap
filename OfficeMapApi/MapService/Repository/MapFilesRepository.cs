using MapService.Database;
using MapService.Database.Entities;
using MapService.Repository.Filters;
using MapService.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Repository
{
    public class MapFilesRepository : IMapFilesRepository
    {
        private readonly MapServiceDbContext dbContext;

        public MapFilesRepository(
          [FromServices] MapServiceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<DbMapFiles> CreateAsync(DbMapFiles mapFile)
        {
            var result = dbContext.MapFiles.Add(mapFile);
            dbContext.SaveChanges();

            return Task.FromResult(result.Entity);
        }

        public Task DeleteAsync(DbMapFiles mapFile)
        {
            dbContext.MapFiles.Remove(mapFile);
            dbContext.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<DbMapFiles>> FindAsync(MapFilesFilter filter = null)
        {
            if (filter != null)
            {
                throw new NotImplementedException();
            }

            return Task.FromResult(dbContext.MapFiles.Where(x => x.Obsolete == false).AsEnumerable());
        }

        public Task<DbMapFiles> GetAsync(Guid id)
        {
            return Task.FromResult(dbContext.MapFiles.FirstOrDefault(x =>
                x.MapGuid == id && x.Obsolete == false));
        }

        public Task<DbMapFiles> UpdateAsync(DbMapFiles mapFile)
        {
            var result = dbContext.MapFiles.Update(mapFile);
            dbContext.SaveChanges();

            return Task.FromResult(result.Entity);
        }
    }
}
