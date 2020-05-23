using SpaceService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Common.Response;

namespace SpaceService.Clients.Interfaces
{
  public interface IWorkplaceServiceClient
  {
    Task<Response<IEnumerable<GetWorkplaceResponse>>> GetWorkplacesAsync(GetWorkplacesRequest request);
  }
}
