using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Common.Response;

namespace WorkplaceService.Clients.Interfaces
{
    public interface IUserServiceClient
    {
        Task<Response<GetUserResponse>> GetUserAsync(GetUserRequest request);
    }
}
