using System;
using System.Runtime.Serialization;

namespace MapService.Models
{
    public partial class MapFiles
    {
        [IgnoreDataMember]
        public Guid MapGuid { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }
        public bool Obsolete { get; set; }
    }
}
