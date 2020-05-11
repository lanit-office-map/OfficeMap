using System;

namespace WorkplaceService.Models
{
    public class WorkplaceResponse
    {
        public Guid Guid { get; set; }
        public int? EmployeeId { get; set; }
        public int? SpaceId { get; set; }
        public int? MapId { get; set; }

        public MapResponse Map { get; set; }
    }
}
