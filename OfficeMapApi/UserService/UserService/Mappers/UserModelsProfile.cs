using AutoMapper;
using Common.RabbitMQ.Models;
using Common.Response;
using UserService.Database.Entities;
using UserService.Models;

namespace UserService.Mappers
{
    public class UserModelsProfile : Profile
    {
        public UserModelsProfile()
        {
            CreateMap<DbUser, UserResponse>()
                .ForPath(user => user.UserGuid, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, DbUser>();

            CreateMap<DbEmployee, EmployeeResponse>();

            CreateMap<Employee, DbEmployee>();

            CreateMap<RegisterUserModel, DbUser>()
                .BeforeMap((src, dest) =>
                {
                    dest.Employee = new DbEmployee();
                })
                .ForPath(user => user.Employee.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(user => user.Employee.SecondName, opt => opt.MapFrom(src => src.SecondName));

            CreateMap<Response<UserResponse>, Response<GetUserResponse>>();

            CreateMap<UserResponse, GetUserResponse>();

            CreateMap<EmployeeResponse, GetEmployeeResponse>();
        }
    }
}
