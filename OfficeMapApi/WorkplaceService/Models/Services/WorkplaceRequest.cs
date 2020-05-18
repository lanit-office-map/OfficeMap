using System;

namespace WorkplaceService.Models.Services
{
    public class WorkplaceRequest
    {
        public Guid OfficeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
        public Guid? WorkplaceGuid { get; set; }
        public Workplace Workplace { get; set; }
    }
}
