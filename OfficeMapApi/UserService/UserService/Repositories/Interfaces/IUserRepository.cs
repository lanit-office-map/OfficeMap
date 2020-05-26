using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Interfaces;
using UserService.Database.Entities;

namespace UserService.Repositories.Interfaces
{
  public interface IUserRepository:
    IGet<Guid, DbUser>,
    IFindAll<IEnumerable<DbUser>>
  {
  }
}
