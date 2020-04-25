using System;

namespace MapService.Repository.Filters
{
    public class MapFilesFilter
    {
        public Guid MapGuid { get; set; }
        public int MapId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Content { get; set; }

        public MapFilesFilter(
            Guid mapGuid,
            int mapId,
            string name,
            string extension,
            byte[] content = null)
        {
            MapGuid = mapGuid;
            MapId = mapId;
            Name = name;
            Content = content;
            Extension = extension;
        }
    }
}
