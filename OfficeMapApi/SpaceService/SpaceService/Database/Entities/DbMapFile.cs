using System;
using System.Collections.Generic;

namespace SpaceService.Database.Entities
{
    public partial class DbMapFile
    {
        public int MapId { get; set; }
        public byte[] Content { get; set; }
        public Guid MapGuid { get; set; }
        public bool Obsolete { get; set; }
    }
}
