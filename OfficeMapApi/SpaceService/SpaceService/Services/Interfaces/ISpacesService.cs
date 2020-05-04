using SpaceService.Interfaces;
using SpaceService.Models;
using SpaceService.Filters;
using System;

namespace SpaceService.Services.Interfaces
{
    public interface ISpacesService :
        IGet<SpaceResponse, Guid>,
        IDelete<Guid>,
        IFind<SpaceResponse, SpaceFilter>,
        ICreate<Space>,
        IUpdate<Space>
    {

    }
}
