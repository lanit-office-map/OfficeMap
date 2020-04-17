using SpaceService.Database.Entities;
using SpaceService.Interfaces;
using SpaceService.Repository.Filters;

namespace SpaceService.Repository.Interfaces
{
    public interface ISpaceTypeRepository :
        IFind<DbSpaceType, SpaceTypeFilter>
    {

    }
}