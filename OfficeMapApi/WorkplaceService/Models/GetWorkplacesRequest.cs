using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkplaceService.Models
{
    //Model for getting workplaces by externial services with RabbitMQ
    public class GetWorkplacesRequest
    {
        public Guid OfficeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
    }
}
