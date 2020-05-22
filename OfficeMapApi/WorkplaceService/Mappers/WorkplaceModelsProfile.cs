using AutoMapper;
using System.Text;
using WorkplaceService.Database.Entities;
using WorkplaceService.Models;

namespace WorkplaceService.Mappers
{
    public class WorkplaceModelsProfile : Profile
    {
        public WorkplaceModelsProfile()
        {
            #region Map
            CreateMap<DbMapFile, MapResponse>()
                .ForMember(m => m.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Content)));
            CreateMap<Map, DbMapFile>()
                .ForMember(m => m.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Content)));
            #endregion

            #region Workplace
            CreateMap<DbWorkplace, WorkplaceResponse>(); //TODO: SpaceId ?-> SpaceGUID, EmployeeId ?-> EmployeeGUID
            CreateMap<DbWorkplace, Workplace>(); //TODO: EmployeeId ?-> EmployeeGUID

            CreateMap<Workplace, DbWorkplace>(); //TODO: EmployeeGUID ?-> EmployeeId
            #endregion
        }
    }
}
