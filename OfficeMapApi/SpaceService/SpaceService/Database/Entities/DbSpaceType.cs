using System;
using System.Collections.Generic;

namespace SpaceService.Database.Entities
{
    public partial class DbSpaceType
    {
        public DbSpaceType()
        {
            Spaces = new HashSet<DbSpace>();
        }

        public int TypeId { get; set; }
        public bool Bookable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid SpaceTypeGuid { get; set; }
        public bool Obsolete { get; set; }

        public virtual ICollection<DbSpace> Spaces { get; set; }
    }
}
