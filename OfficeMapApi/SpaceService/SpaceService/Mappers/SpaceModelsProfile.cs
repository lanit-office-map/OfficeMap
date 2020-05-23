using AutoMapper;
using SpaceService.Database.Entities;
using SpaceService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.RabbitMQ.Models;
using Common.Response;

namespace SpaceService.Mappers
{
  public class SpaceModelsProfile : Profile
  {
    public SpaceModelsProfile()
    {
      CreateMap<DbSpaceType, SpaceType>();

      CreateMap<SpaceType, DbSpaceType>(MemberList.Source);

      CreateMap<DbSpace, Space>()
        .ForMember(space => space.SpaceTypeGuid, opt => opt.MapFrom(src => src.SpaceType.SpaceTypeGuid))
        .ForMember(space => space.Map, opt => opt.MapFrom(src => src.MapFile));
      CreateMap<Space, DbSpace>(MemberList.Source);

      CreateMap<DbSpace, SpaceResponse>()
        .ForMember(response => response.Map, opt => opt.MapFrom(src => src.MapFile));

      CreateMap<DbSpaceType, SpaceTypeResponse>();

      CreateMap<DbMapFile, MapResponse>()
        .ForMember(response => response.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Content)));

      CreateMap<DbMapFile, Map>()
        .ForMember(m => m.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetString(src.Content)));

      CreateMap<Map, DbMapFile>()
        .ForMember(m => m.Content, opt => opt.MapFrom(src => Encoding.UTF8.GetBytes(src.Content)));

      CreateMap<Response<SpaceResponse>, Response<GetSpaceResponse>>();
      CreateMap<SpaceResponse, GetSpaceResponse>();

    }
  }
}
