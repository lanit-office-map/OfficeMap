using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Database.Entities;
using UserService.Repositories.Interfaces;

namespace UserService.Repositories
{
  public class UserRepository : IUserRepository
  {
    #region private fields
    private readonly UserServiceDbContext dbContext;
    #endregion

    #region public methods
    public UserRepository(
      [FromServices] UserServiceDbContext dbContext)
    {
      this.dbContext = dbContext;
    }
    public async Task<DbUser> GetAsync(Guid key)
    {
      return await dbContext.Users
        .Include(u => u.Employee)
        .Select(u => new DbUser
        {
          Id = u.Id,
          Employee = new DbEmployee
          {
            EmployeeGuid = u.Employee.EmployeeGuid,
            EmployeeId = u.Employee.EmployeeId,
            FirstName = u.Employee.FirstName,
            SecondName = u.Employee.SecondName
          }
        })
        .FirstOrDefaultAsync(u => u.Id == key.ToString());
    }

    public async Task<IEnumerable<DbUser>> FindAllAsync()
    {
      return await dbContext.Users
        .Include(u => u.Employee)
        .Where(u => u.Employee != null)
        .Select(u => new DbUser()
        {
          Id = u.Id,
          Employee = new DbEmployee
          {
            EmployeeGuid = u.Employee.EmployeeGuid,
            EmployeeId = u.Employee.EmployeeId,
            FirstName = u.Employee.FirstName,
            SecondName = u.Employee.SecondName
          }
        })
        .AsNoTracking()
        .ToListAsync();
    }
    #endregion
  }
}
