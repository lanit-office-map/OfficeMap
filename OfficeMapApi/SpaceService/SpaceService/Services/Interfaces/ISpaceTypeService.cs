using System;
using SpaceService.Models;
using SpaceService.Filters;
using SpaceService.Interfaces;

namespace SpaceService.Services.Interfaces
{
    public interface ISpaceTypeService :
        IGet<SpaceTypeResponse, SpaceTypeFilter, Guid>,
        IDelete<Guid>,
        IFind<SpaceTypeResponse, SpaceTypeFilter>,
        ICreate<SpaceType>,
        IUpdate<SpaceType>
    {

    }
}