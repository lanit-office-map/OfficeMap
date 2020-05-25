using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
  public class UserResponse
  {
    public Guid UserGuid { get; set; }

    public EmployeeResponse Employee { get; set; }
  }
}
