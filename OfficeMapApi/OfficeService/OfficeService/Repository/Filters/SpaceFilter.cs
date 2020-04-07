using System;

namespace OfficeService.Repository.Filters
{
    public class SpaceFilter
    {
        public Guid SpaceGuid { get; }

        public SpaceFilter(Guid spaceguid)
        {
            SpaceGuid = spaceguid;
        }

    }
}
