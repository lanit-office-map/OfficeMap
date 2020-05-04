using System;

namespace SpaceService.Models
{
    public class MapResponse
    {
        public Guid MapGuid { get; set; }
        public byte[] Content { get; set; }
    }
}
