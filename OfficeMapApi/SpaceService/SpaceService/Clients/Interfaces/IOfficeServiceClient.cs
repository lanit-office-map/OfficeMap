﻿using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Common.Response;

namespace SpaceService.Clients.Interfaces
{
    public interface IOfficeServiceClient
    {
        Task<Response<GetOfficeResponse>> GetOfficeAsync(GetOfficeRequest request);
    }
}
