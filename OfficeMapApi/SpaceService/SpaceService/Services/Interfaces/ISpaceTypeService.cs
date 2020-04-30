using System;
using SpaceService.Models;
using SpaceService.Filters;
using SpaceService.Interfaces;

namespace SpaceService.Services.Interfaces
{
    public interface ISpaceTypeService :
        IGet<SpaceType, Guid>,
        IDelete<Guid>,
        IFind<SpaceType, SpaceTypeFilter>,
        ICreate<SpaceType>,
        IUpdate<SpaceType>
    {

    }
}