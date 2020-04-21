using SpaceService.Database.Entities;
using SpaceService.Interfaces;
using SpaceService.Repository.Filters;
using System;

namespace SpaceService.Repository.Interfaces
{
    public interface ISpaceTypeRepository :
        IGet<DbSpaceType, Guid>,
        IDelete<DbSpaceType>,
        IFind<DbSpaceType, SpaceTypeFilter>,
        ICreate<DbSpaceType>,
        IUpdate<DbSpaceType>
    {

    }
}