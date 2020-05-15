using System;

namespace WorkplaceService.Models.RabbitMQ
{
    //Model for getting Space from SpaceService with RabbitMQ
    public class GetSpaceRequest
    {
        public Guid OfficeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
    }
}
