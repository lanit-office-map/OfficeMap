using System;
using SpaceService.Models;
using SpaceService.Repository.Filters;
using SpaceService.Interfaces;

namespace SpaceService.Services.Interface
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