﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceService.Models
{
    public class WorkplaceResponse
    {
        public Guid Guid { get; set; }
        public MapResponse WorkplaceMap { get; set; }
    }
}
