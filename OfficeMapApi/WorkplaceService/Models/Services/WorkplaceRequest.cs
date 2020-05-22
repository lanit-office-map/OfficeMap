using Microsoft.AspNetCore.Mvc;
using System;

namespace WorkplaceService.Models.Services
{
    public class WorkplaceRequest
    {
        [FromRoute]
        internal Guid OfficeGuid { get; set; }
        [FromRoute]
        internal Guid SpaceGuid { get; set; }
        [FromRoute]
        internal Guid WorkplaceGuid { get; set; }
        [FromBody]
        internal Workplace Workplace { get; set; }
    }
}
