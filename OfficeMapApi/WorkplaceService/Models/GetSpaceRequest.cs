using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkplaceService.Models
{
    //Model for getting Space from SpaceService with RabbitMQ
    public class GetSpaceRequest
    {
        public Guid OfficeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
    }
}
