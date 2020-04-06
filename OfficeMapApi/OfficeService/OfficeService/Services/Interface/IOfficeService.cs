using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeService.Models;

namespace OfficeService.Services.Interface
{
  public interface IOfficeService
  {
    Task<IEnumerable<Office>> GetOfficesAsync();
  }
}