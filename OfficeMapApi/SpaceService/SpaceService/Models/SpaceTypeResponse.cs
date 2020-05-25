using System;

namespace SpaceService.Models
{
    public class SpaceTypeResponse
    {
        public Guid SpaceTypeGuid { get; set; }
        public bool Bookable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
