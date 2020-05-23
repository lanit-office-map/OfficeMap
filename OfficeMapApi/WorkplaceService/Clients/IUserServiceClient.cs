using System;
using System.Threading.Tasks;
using WorkplaceService.Models.RabbitMQ;

namespace WorkplaceService.Clients
{
    public interface IUserServiceClient
    {
        Task<Employee> GetUserIdAsync(Employee Request);
    }
}
