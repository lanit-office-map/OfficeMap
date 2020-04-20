using AutoMapper;
using MapService.Database.Entities;
using MapService.Models;
using System;

namespace OfficeService.Mappers
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
