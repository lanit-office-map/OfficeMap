using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpaceService.Models
{
    public class Space
    {
        [IgnoreDataMember]
        public Guid SpaceGuid { get; set; }
        
        [IgnoreDataMember]
        public int OfficeGuid { get; set; }
        public int OfficeId { get; set; }
        public Space Parents { get; set; }
        public ICollection<SpaceResponse> Spaces { get; set; }
        public Map Map { get; set; }
        public Guid SpaceTypeGuid { get; set; }
        public string SpaceName { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }
        public int? Floor { get; set; }

    }
}