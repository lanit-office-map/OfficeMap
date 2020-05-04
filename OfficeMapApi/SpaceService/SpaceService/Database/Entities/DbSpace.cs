using System;
using System.Collections.Generic;

namespace SpaceService.Database.Entities
{
    public partial class DbSpace
    {
        public DbSpace()
        {
            InverseParent = new HashSet<DbSpace>();
        }

        public int SpaceId { get; set; }
        public int OfficeId { get; set; }
        public int? ParentId { get; set; }
        public int? MapId { get; set; }
        public int? TypeId { get; set; }
        public string SpaceName { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }
        public int? Floor { get; set; }
        public Guid SpaceGuid { get; set; }
        public bool Obsolete { get; set; }

        public DbMapFile MapFiles { get; set; }
        public DbOffice Offices { get; set; }
        public DbSpace Parents { get; set; }
        public DbSpaceType SpaceTypes { get; set; }
        public ICollection<DbSpace> InverseParent { get; set; }
    }
}
