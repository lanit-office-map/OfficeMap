using SpaceService.Interfaces;
using SpaceService.Models;
using SpaceService.Filters;
using System;

namespace SpaceService.Services.Interfaces
{
    public interface ISpacesService :
        IGet<Space, Guid>,
        IDelete<Guid>,
        IFind<Space, SpaceFilter>,
        ICreate<Space>,
        IUpdate<Space>
    {

    }
}
