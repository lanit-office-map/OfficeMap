using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceService.Repository.Filters
{
    public class SpaceFilter
    {
        public int SpaceId { get; set; }
        public int OfficeId { get; set; }
        public int? ParentId { get; set; }
        public int? MapId { get; set; }
        public int? TypeId { get; set; }
        public string SpaceName { get; set; }
        public string Description { get; set; }
        public int? Capacity { get; set; }
        public int? Floor { get; set; }
        public Guid SpaceGuid { get; set; }

        public SpaceFilter(
            int spaceId,
            int officeId,
            Guid spaceguid,
            int? parentId = null,
            int? mapId = null,
            int? typeId = null,
            string spacename = null,
            string description = null,
            int? capacity = null,
            int? floor = null)
        {
            SpaceId = spaceId;
            OfficeId = officeId;
            SpaceGuid = spaceguid;
            ParentId = parentId;
            MapId = mapId;
            TypeId = typeId;
            SpaceName = spacename;
            Description = description;
            Capacity = capacity;
            Floor = floor;
        }
    }
}
