using System;
using System.Linq;
using System.Threading.Tasks;

namespace WorkplaceService.Clients
{
    interface ISpaceServiceClient
    {
        Task<IQueryable<Guid>> GetSpaceGuidsAsync(Guid officeGuid);
    }
}
