using System;
using System.Collections.Generic;
using OfficeService.Database.Entities;
using Common.Interfaces;

namespace OfficeService.Repository.Interfaces
{
    public interface IOfficeRepository :
      IGet<Guid, DbOffice>,
      IFindAll<IEnumerable<DbOffice>>,
      IUpdate<DbOffice, DbOffice>,
      ICreate<DbOffice, DbOffice>,
      IDelete<DbOffice>
    {
    }
}