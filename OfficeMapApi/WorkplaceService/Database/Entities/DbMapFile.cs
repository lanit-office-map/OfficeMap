using System;
using System.ComponentModel.DataAnnotations;

namespace WorkplaceService.Database.Entities
{
    public partial class DbMapFile
    {
        [Key]
        public Guid MapGuid { get; set; }
        public int MapId { get; set; }
        public byte[] Content { get; set; }
        public bool Obsolete { get; set; }

        public virtual DbWorkplace Workplace { get; set; }
    }
}
