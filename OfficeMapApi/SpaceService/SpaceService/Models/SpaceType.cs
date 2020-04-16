using System;
using System.Runtime.Serialization;

namespace SpaceService.Models
{
    public class SpaceType
    {
        [IgnoreDataMember]
        public Guid SpaceTypeGuid { get; set; }
        public bool Bookable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}
