using Common.Interfaces;
using WorkplaceService.Models;
using WorkplaceService.Models.Services;

namespace WorkplaceService.Services
{
    public interface IWorkplaceService :
        IFindAll<WorkplaceRequest, WorkplaceResponse>,
        IGet<WorkplaceRequest, WorkplaceResponse>,
        ICreate<WorkplaceRequest, Workplace>,
        IUpdate<WorkplaceRequest, Workplace>,
        IDelete<WorkplaceRequest, WorkplaceResponse>
    {
    }
    /*public interface IWorkplaceService :
        IFindAll<WorkplaceRequest>,
        IGet<WorkplaceRequest>,
        ICreate<WorkplaceRequest>,
        IUpdate<WorkplaceRequest>,
        IDelete<WorkplaceRequest>
    {
    }*/
}
