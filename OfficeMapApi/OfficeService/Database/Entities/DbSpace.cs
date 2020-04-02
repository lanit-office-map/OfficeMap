using System;
using System.Collections.Generic;

namespace OfficeService.Database.Entities
{
    public partial class DbSpace
    {
        public DbSpace()
        {
        }
        public int OfficeId { get; set; }
        public virtual ICollection<DbOffice> Offices { get; set; }
        public virtual DbSpace Parent { get; set; }
        public virtual ICollection<DbSpace> InverseParent { get; set; }
    }
}
