using AutoMapper;
using UserService.Database.Entities;
using UserService.Models;

namespace UserService
{
    public class UserModelsProfile : Profile
    {
        public UserModelsProfile()
        {
            CreateMap<DbUser, User>();

            CreateMap<User, DbUser>();

            CreateMap<DbEmployee, Employee>();

            CreateMap<Employee, DbEmployee>();

            CreateMap<RegisterUserModel, DbUser>()
                .BeforeMap((src, dest) =>
                {
                    dest.Employee = new DbEmployee();
                })
                .ForPath(user => user.Employee.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(user => user.Employee.SecondName, opt => opt.MapFrom(src => src.SecondName));
        }
    }
}
