using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceService.Models
{
  public class WorkplaceResponse
  {
    public Guid WorkplaceGuid { get; set; }
    public string Name { get; set; }

    public MapResponse Map { get; set; }
  }
}
