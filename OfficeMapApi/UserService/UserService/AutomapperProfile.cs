using AutoMapper;
using UserService.Database.Entities;
using UserService.Models;

namespace UserService
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, DbUser>();
        }
    }
}
