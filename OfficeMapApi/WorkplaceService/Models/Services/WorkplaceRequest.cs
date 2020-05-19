using System;

namespace WorkplaceService.Models.Services
{
    public class WorkplaceRequest
    {
        internal Guid OfficeGuid { get; set; }
        internal Guid SpaceGuid { get; set; }
        internal Guid WorkplaceGuid { get; set; }
        internal Workplace Workplace { get; set; }
    }
}
