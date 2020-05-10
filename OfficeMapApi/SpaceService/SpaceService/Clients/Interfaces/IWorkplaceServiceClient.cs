using SpaceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceService.Clients.Interfaces
{
    public interface IWorkplaceServiceClient
    {
        Task<IEnumerable<WorkplaceResponse>> GetWorkplacesAsync(Guid spaceGuid);
    }
}
