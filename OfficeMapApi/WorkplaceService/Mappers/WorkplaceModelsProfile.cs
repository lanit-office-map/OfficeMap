using System.Collections.Generic;
using AutoMapper;
using Common.RabbitMQ.Models;
using Common.Response;
using WorkplaceService.Database.Entities;
using WorkplaceService.Models;

namespace WorkplaceService.Mappers
{
    public class WorkplaceModelsProfile : Profile
    {
        public WorkplaceModelsProfile()
        {
            #region Map
            CreateMap<DbMapFile, MapResponse>();
            CreateMap<Map, DbMapFile>();
            #endregion

            #region Workplace
            CreateMap<DbWorkplace, WorkplaceResponse>();
            CreateMap<Workplace, DbWorkplace>();
            #endregion

            CreateMap<Response<IEnumerable<WorkplaceResponse>>,
              Response<IEnumerable<GetWorkplaceResponse>>>();
            CreateMap<WorkplaceResponse, GetWorkplaceResponse>();
        }
    }
}
