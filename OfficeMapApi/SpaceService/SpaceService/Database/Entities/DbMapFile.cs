using System;
using System.Collections.Generic;

namespace SpaceService.Database.Entities
{
    public partial class DbMapFile
    {
        public DbMapFile()
        {
            Spaces = new HashSet<DbSpace>();
        }
        public int MapId { get; set; }
        public byte[] Content { get; set; }
        public Guid MapGuid { get; set; }
        public bool Obsolete { get; set; }

        public virtual ICollection<DbSpace> Spaces { get; set; }
    }
}
