using SpaceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceService.Clients
{
    interface IOfficeServiceClient
    {
        Task<Office> GetOfficeAsync(Guid officeGuid);
    }
}
