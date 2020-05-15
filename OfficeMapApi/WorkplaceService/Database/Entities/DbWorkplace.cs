using Common.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

namespace WorkplaceService.Database.Entities
{
    public partial class DbWorkplace : IEntity<Guid>
    {
        [Key]
        public Guid Guid { get; set; }
        public int WorkplaceId { get; set; }
        public int? EmployeeId { get; set; }
        public int SpaceId { get; set; }
        public bool Obsolete { get; set; }
        public int MapId { get; set; }

        public virtual DbMapFile Map { get; set; }
    }
}
