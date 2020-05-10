using SpaceService.Models;
using System;
using System.Threading.Tasks;

namespace SpaceService.Clients.Interfaces
{
    public interface IOfficeServiceClient
    {
        Task<Office> GetOfficeAsync(Guid officeGuid);
    }
}
