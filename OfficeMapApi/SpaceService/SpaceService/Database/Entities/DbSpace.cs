using System;
using System.Collections.Generic;

namespace SpaceService.Database.Entities
{
    public partial class DbSpace
    {
        public DbSpace()
        {
            Spaces = new HashSet<DbSpace>();
        }

        public int SpaceId { get; set; }
        public int OfficeId { get; set; }
        public int MapId { get; set; }
        public int? TypeId { get; set; }
        public string SpaceName { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }
        public int? Floor { get; set; }
        public Guid SpaceGuid { get; set; }
        public bool Obsolete { get; set; }
        public int? ParentId { get; set; }
        public DbSpace Parent { get; set; }
        public DbMapFile MapFile { get; set; }
        public DbSpaceType SpaceType { get; set; }
        public ICollection<DbSpace> Spaces { get; set; }
    }
}
