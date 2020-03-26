using OfficeService.Database.Models;
using OfficeService.Models;
using OfficeService.Repositories.Filters;

namespace OfficeService.Repositories.Interfaces
{
    /// <summary>
    /// ReportRepository interface.
    /// </summary>
    public interface IOfficeRepository :

      IGet<Office, OfficeFilter>,
      ICreate<OfficeRequest>,
      IDelete<Office>,
      IUpdate<OfficeRequest>
    {

        DbOffice GetDbReport(OfficeFilter filter);
    }
}
