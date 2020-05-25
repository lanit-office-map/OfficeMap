using System;

namespace WorkplaceService.Models
{
    public class WorkplaceResponse
    {
        public Guid WorkplaceGuid { get; set; }
        public Guid? EmployeeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
        public MapResponse Map { get; set; }
    }
}
