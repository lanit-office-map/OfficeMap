using UserService.Repositories.Filters;
using UserServiceApi.Database.Entities;

namespace UserService.Repositories.Interfaces
{
    /// <summary>
    /// User repository interface.
    /// </summary>
    public interface IUserRepository : 
        ICreate<DbUser>,
        IGet<DbUser, UserFilter>,
        IUpdate<DbUser>,
        IDelete<DbUser>,
        IFind<DbUser, UserFilter>
    { }
}