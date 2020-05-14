using System;
using System.Linq;
using System.Threading.Tasks;
using WorkplaceService.Models;

namespace WorkplaceService.Clients
{
    public interface ISpaceServiceClient
    {
        Task<Space> GetSpaceGuidsAsync(Guid officeGuid, Guid spaceGuid);
    }
}
