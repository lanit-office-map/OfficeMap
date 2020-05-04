using SpaceService.Database.Entities;
using SpaceService.Interfaces;
using SpaceService.Filters;
using System;
using SpaceService.Models;

namespace SpaceService.Repository.Interfaces
{
    public interface ISpaceRepository :

        ICreate<DbSpace>,
        IDelete<DbSpace>,
        IFind<DbSpace, SpaceFilter>,
        IGet<DbSpace, Guid>,
        IUpdate<DbSpace>


    {
    }
}
