using System;
using System.Collections.Generic;

namespace OfficeService.Database.Entities
{
    public partial class DbOffice
    {
        public DbOffice()
        {
           Spaces = new HashSet<DbSpace>(); 
        }

        public int OfficeId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Building { get; set; }
        public string PhoneNumber { get; set; }
        public Guid OfficeGuid { get; set; }
        public bool Obsolete { get; set; }

        public virtual ICollection<DbSpace> Spaces { get; set; }
    }

}
