using AutoMapper;
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
        }
    }
}
