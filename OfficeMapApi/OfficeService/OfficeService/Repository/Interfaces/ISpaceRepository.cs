using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeService.Database.Entities;
using OfficeService.Models;
using OfficeService.Repository.Filters;

namespace OfficeService.Repository.Interfaces
{
    public interface ISpaceRepository : IGet<Space, SpaceFilter>
    {

    }
}
