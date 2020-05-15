using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceService.Models
{
    public class WorkplaceRequest
    {
        public Guid OfficeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
    }
}
