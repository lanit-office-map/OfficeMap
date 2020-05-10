﻿using SpaceService.Database.Entities;
using System;
using System.Collections.Generic;

namespace SpaceService.Models
{
    public class SpaceResponse
    {
        // GET/spaces
        // GET/spaces/{spaceGuid}

        public Guid SpaceGuid { get; set; }
        public Guid OfficeGuid { get; set; }
        public ICollection<SpaceResponse> Spaces { get; set; }
        public MapResponse Map { get; set; }
        public SpaceTypeResponse SpaceType { get; set; }
        public IEnumerable<WorkplaceResponse> Workplaces { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public int Floor { get; set; }
    }
}
