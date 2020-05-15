using System;

namespace WorkplaceService.Models.RabbitMQ
{
    //Model for getting workplaces by externial services with RabbitMQ
    public class WorkplacesResponse
    {
        public Guid OfficeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
    }
}
