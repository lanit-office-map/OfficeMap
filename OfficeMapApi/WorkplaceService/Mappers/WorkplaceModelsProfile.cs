using AutoMapper;
using WorkplaceService.Database.Entities;
using WorkplaceService.Models;

namespace WorkplaceService.Mappers
{
    public class WorkplaceModelsProfile : Profile
    {
        public WorkplaceModelsProfile()
        {
            CreateMap<DbMapFile, MapResponse>();
            CreateMap<Map, DbMapFile>();

            CreateMap<DbWorkplace, WorkplaceResponse>();
            CreateMap<Workplace, DbWorkplace>();
        }
    }
}
