using System;
using System.Threading.Tasks;
using WorkplaceService.Models.RabbitMQ;

namespace WorkplaceService.Clients
{
    public interface ISpaceServiceClient
    {
        Task<Space> GetSpaceIdAsync(Guid officeGuid, Guid spaceGuid);
    }
}
