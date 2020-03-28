using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserServiceApi.Database.Entities;

namespace UserService.Repositories.Filters
{
    /// <summary>
    /// Filter used to retrieve a list of users.
    /// </summary>
    public class UserFilter
    {
        public int? EmployeeId { get; }

        public UserFilter(int? employeeId = null)
        {
            EmployeeId = employeeId;
        }
    }
}