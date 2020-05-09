using System;

namespace WorkplaceService.Models
{
    public class Workplace
    {
        public Guid WorkplaceGuid { get; set; }
        public int? EmployeeId { get; set; }
        public int? SpaceId { get; set; }
        public int? MapId { get; set; }
    }
}
