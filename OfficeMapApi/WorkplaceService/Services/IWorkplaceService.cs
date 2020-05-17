using Common.Interfaces;
using System;
using WorkplaceService.Filters;
using WorkplaceService.Models;

namespace WorkplaceService.Services
{
    public interface IWorkplaceService :
        IGet<WorkplaceResponse, Guid>,
        IDelete<Workplace, Guid>,
        IFindAll<WorkplaceResponse, WorkplaceFilter>,
        ICreate<Workplace>,
        IUpdate<Workplace>
    {
    }
}
