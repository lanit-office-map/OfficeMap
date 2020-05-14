using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkplaceService.Filters
{
    public class WorkplaceFilter
    {
        public int SpaceId { get; }

        public WorkplaceFilter(int spaceId)
        {
            SpaceId = spaceId;
        }
    }
}
