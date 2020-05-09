using Common.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkplaceService.Database.Entities
{
    public partial class DbMapFile : IEntity<Guid>
    {
        public DbMapFile()
        {
            Workplaces = new HashSet<DbWorkplace>();
        }

        [Key]
        public Guid Guid { get; set; }
        public int MapId { get; set; }
        public byte[] Content { get; set; }
        public bool Obsolete { get; set; }

        public virtual ICollection<DbWorkplace> Workplaces { get; set; }
    }
}
