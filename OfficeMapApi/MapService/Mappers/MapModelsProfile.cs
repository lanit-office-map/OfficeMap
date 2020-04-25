using AutoMapper;
using MapService.Database.Entities;
using MapService.Models;

namespace MapService.Mappers
{
    public class MapModelsProfile : Profile
    {
        public MapModelsProfile()
        {
            CreateMap<MapFiles, MapFiles>();

            CreateMap<MapFiles, DbMapFiles>();
        }
    }
}
