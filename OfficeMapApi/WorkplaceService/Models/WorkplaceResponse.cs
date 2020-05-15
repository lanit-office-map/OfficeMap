using System;

namespace WorkplaceService.Models
{
    public class WorkplaceResponse
    {
        public Guid Guid { get; set; }
        public Guid EmployeeGuid { get; set; }
        public Guid SpaceGuid { get; set; }

        public MapResponse Map { get; set; }
    }
}
