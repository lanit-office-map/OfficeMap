using System;
using System.Runtime.Serialization;

namespace SpaceService.Models
{
    public class Space
    {
        [IgnoreDataMember]
        public Guid SpaceGuid { get; set; }
        public int OfficeId { get; set; }
        public int? ParentId { get; set; }
        public int? MapId { get; set; }
        public int? TypeId { get; set; }
        public string SpaceName { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }
        public int? Floor { get; set; }
        
    }
}
