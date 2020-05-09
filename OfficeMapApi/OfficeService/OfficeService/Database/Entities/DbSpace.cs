using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OfficeService.Database.Entities
{
    public partial class DbSpace
    {
        public DbSpace()
        {
        }

        [Key]
        public Guid Guid { get; set; }
        public int SpaceId { get; set; }
        public int TypeId { get; set; }
        public int ParentId { get; set; }
        public int MapId { get; set; }
        public int OfficeId { get; set; }

        public virtual DbOffice Office { get; set; }
        public virtual DbSpace Parent { get; set; }
        public virtual ICollection<DbSpace> InverseParent { get; set; }
    }
}