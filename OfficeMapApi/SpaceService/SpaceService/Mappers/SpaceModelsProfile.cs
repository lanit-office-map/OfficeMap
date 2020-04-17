using AutoMapper;
using SpaceService.Database.Entities;
using SpaceService.Models;

namespace SpaceService.Mappers
{
    public class SpaceModelsProfile : Profile
    {
        public SpaceModelsProfile()
        {
            CreateMap<DbSpaceType, SpaceType>();

            CreateMap<SpaceType, DbSpaceType>()
                .ForMember(dbspacetype => dbspacetype.Spaces, opt => opt.Ignore())
                .ForMember(dbspacetype => dbspacetype.Obsolete, opt => opt.Ignore())
                .ForMember(dbspacetype => dbspacetype.TypeId, opt => opt.Ignore());

            CreateMap<DbSpace, Space>();

            CreateMap<Space, DbSpace>()
                .ForMember(dbspace => dbspace.Offices, opt => opt.Ignore())
                .ForMember(dbspace => dbspace.Obsolete, opt => opt.Ignore());
        }
    }
}
