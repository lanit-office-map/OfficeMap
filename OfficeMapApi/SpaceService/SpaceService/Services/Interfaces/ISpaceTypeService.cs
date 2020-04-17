using SpaceService.Models;
using SpaceService.Repository.Filters;
using SpaceService.Interfaces;

namespace SpaceService.Services.Interface
{
    public interface ISpaceTypeService :
          IFind<SpaceType, SpaceTypeFilter>
    {

    }
}