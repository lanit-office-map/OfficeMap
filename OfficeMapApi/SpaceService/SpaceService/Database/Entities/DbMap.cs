﻿using System;
using System.Collections.Generic;

namespace SpaceService.Database.Entities
{
    public partial class DbMap
    {
        public DbMap()
        {
            Spaces = new HashSet<DbSpace>();
        }

        public int MapId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public Guid MapGuid { get; set; }
        public bool Obsolete { get; set; }

        public virtual ICollection<DbSpace> Spaces { get; set; }
    }
}
