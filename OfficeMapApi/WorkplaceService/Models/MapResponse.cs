using System;

namespace WorkplaceService.Models
{
    public class MapResponse
    {
        public Guid MapGuid { get; set; }
        public byte[] Content { get; set; }
    }
}
