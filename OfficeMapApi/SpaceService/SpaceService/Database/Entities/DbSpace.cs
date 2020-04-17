﻿using System;
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

        public virtual DbMapFile Maps { get; set; }
        public virtual DbOffice Offices { get; set; }
        public virtual DbSpace Parents { get; set; }
        public virtual DbSpaceType Types { get; set; }
        public virtual ICollection<DbSpace> InverseParent { get; set; }
    }
}