using System;

namespace SpaceService.Filters
{
    public class SpaceFilter
    {
        public int OfficeId { get; set; }
        public Guid OfficeGuid { get; set; }

        public SpaceFilter(int officeId, Guid officeGuid)
          {
            OfficeId = officeId;
            OfficeGuid = officeGuid;
        }
    }
}
