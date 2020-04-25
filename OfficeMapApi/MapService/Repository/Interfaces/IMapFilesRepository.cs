using MapService.Database.Entities;
using MapService.Interfaces;
using MapService.Repository.Filters;
using System;


namespace MapService.Repository.Interfaces
{
    public interface IMapFilesRepository :
        IGet<DbMapFiles, Guid>,
        IDelete<DbMapFiles>,
        IFind<DbMapFiles, MapFilesFilter>,
        ICreate<DbMapFiles>,
        IUpdate<DbMapFiles>
    {
    }
}
