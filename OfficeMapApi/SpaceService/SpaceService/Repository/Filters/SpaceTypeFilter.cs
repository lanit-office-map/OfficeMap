using System;
namespace SpaceService.Repository.Filters
{
    public class SpaceTypeFilter
    {
        public int TypeId { get; set; }
        public bool Bookable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid SpaceTypeGuid { get; set; }

        public SpaceTypeFilter(
            int typeId,
            Guid spacetypeGuid,
            bool bookable,
            string name = null,
            string description = null)
        {
            TypeId = typeId;
            SpaceTypeGuid = spacetypeGuid;
            Name = name;
            Description = description;
            Bookable = bookable;
        }
    }
}
