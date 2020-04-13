using AutoMapper;
using UserService.Database.Entities;
using UserService.Models;

namespace UserService
{
    public class UserModelsProfile : Profile
    {
        public UserModelsProfile()
        {
            CreateMap<User, DbUser>();
        }
    }
}
