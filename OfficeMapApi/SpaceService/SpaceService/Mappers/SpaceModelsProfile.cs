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


            CreateMap<DbSpace, Space>()
                .ForMember(space => space.SpaceTypeGuid, opt => opt.MapFrom(src => src.SpaceTypes.SpaceTypeGuid))
                .ForMember(space => space.Map, opt => opt.MapFrom(src => src.MapFiles));
            CreateMap<Space, DbSpace>()
                .ForMember(dbspace => dbspace.Obsolete, opt => opt.Ignore())
                .ForMember(dbspace => dbspace.SpaceId, opt => opt.Ignore());


            CreateMap<DbSpace, SpaceResponse>()
                .ForMember(response => response.SpaceGuid, opt => opt.MapFrom(src => src.SpaceGuid))
                .ForMember(response => response.OfficeGuid, opt => opt.Ignore())
                .ForMember(response => response.Map, opt => opt.MapFrom(src => src.MapFiles))
                .ForMember(response => response.SpaceType, opt => opt.MapFrom(src => src.SpaceTypes))
                .ForMember(response => response.Capacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(response => response.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(response => response.Floor, opt => opt.MapFrom(src => src.Floor));

            CreateMap<DbSpaceType, SpaceTypeResponse>()
                .ForMember(response => response.SpaceTypeGuid, opt => opt.MapFrom(src => src.SpaceTypeGuid))
                .ForMember(response => response.Bookable, opt => opt.MapFrom(src => src.Bookable))
                .ForMember(response => response.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(response => response.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<DbMapFile, MapResponse>()
                .ForMember(response => response.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(response => response.MapGuid, opt => opt.MapFrom(src => src.MapGuid));

            CreateMap<DbMapFile, Map>()
                .ForMember(m => m.Content, opt => opt.MapFrom(src => src.Content));



        }
    }
}
