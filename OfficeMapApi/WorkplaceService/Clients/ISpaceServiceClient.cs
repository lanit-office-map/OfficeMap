using System;
using System.Threading.Tasks;
using WorkplaceService.Models.RabbitMQ;

namespace WorkplaceService.Clients
{
    public interface ISpaceServiceClient
    {
        Task<int> GetSpaceIdAsync(Guid officeGuid, Guid spaceGuid);
    }
}
