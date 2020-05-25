using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
  public class EmployeeResponse
  {
    public Guid EmployeeGuid { get; set; }

    public string FirstName { get; set; }

    public string SecondName { get; set; }
  }
}
