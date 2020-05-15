using System;

namespace WorkplaceService.Models
{
    public class Workplace
    {
        public Guid EmployeeGuid { get; set; }
        internal Guid Guid { get; set; }
        internal int SpaceId { get; set; }
        internal int EmployeeId { get; set; }

        public Map Map { get; set; }
    }
}
