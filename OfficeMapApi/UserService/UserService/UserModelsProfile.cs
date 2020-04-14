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

            /*
             * Возможно пригодится в разговоре:
                .ForPath(user => user.Employee.EmployeeGuid, opt => opt.MapFrom(src => src.Employee.EmployeeGuid))
                .ForAllOtherMembers(opt => opt.Ignore());
                .ForMember(user => user.EmployeeId, opt => opt.Ignore())
                .ForPath(user => user.Employee.EmployeeId, opt => opt.Ignore())
                CreateMap<DbUser, User>().ReverseMap();
             */
        }
    }
}
