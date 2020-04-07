using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeService.Models
{
    public class OfficeRequest : Office
    {
        internal Guid OfficeGuid { get; set; }
        internal int SpaceId { get; set; }
    }
}
