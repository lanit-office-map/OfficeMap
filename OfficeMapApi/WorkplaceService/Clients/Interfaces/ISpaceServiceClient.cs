using System;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Common.Response;

namespace WorkplaceService.Clients.Interfaces
{
    public interface ISpaceServiceClient
    {
        Task<Response<GetSpaceResponse>> GetSpaceAsync(GetSpaceRequest request);
    }
}
