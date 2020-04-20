using System;
using System.Runtime.Serialization;

namespace MapService.Database.Entities
{
    public partial class DbMapFiles
    {
        [IgnoreDataMember]
        public Guid MapGuid { get; set; }
        public int MapId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public bool Obsolete { get; set; }
    }
}
