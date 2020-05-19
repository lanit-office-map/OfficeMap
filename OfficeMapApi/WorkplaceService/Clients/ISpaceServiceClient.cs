﻿using System;
using System.Threading.Tasks;
using WorkplaceService.Models.RabbitMQ;

namespace WorkplaceService.Clients
{
    public interface ISpaceServiceClient
    {
        Task<Space> GetSpaceGuidsAsync(Guid officeGuid, Guid spaceGuid);
    }
}