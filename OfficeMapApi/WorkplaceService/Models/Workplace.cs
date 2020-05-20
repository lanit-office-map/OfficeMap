using System;

namespace WorkplaceService.Models
{
    public class Workplace
    {
        public Guid WorkplaceGuid { get; set; }
        public Guid EmployeeGuid { get; set; }
        public Guid SpaceGuid { get; set; }
        public Map WorkplaceMap { get; set; }
    }
}
