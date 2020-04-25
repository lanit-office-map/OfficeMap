using MapService.Interfaces;
using MapService.Models;
using MapService.Repository.Filters;
using System;

namespace MapService.Services.Interfaces
{
    public interface IMapService :
        IGet<MapFiles, Guid>,
        IDelete<Guid>,
        IFind<MapFiles, MapFilesFilter>,
        ICreate<MapFiles>,
        IUpdate<MapFiles>
    {
    }
}
