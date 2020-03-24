using UserService.Models;
using UserService.Repositories.Filters;

namespace UserService.Repositories.Interfaces
{
    /// <summary>
    /// User repository interface.
    /// </summary>
    public interface IUserRepository : 
        ICreate<UserRequest>,
        IGet<User, UserFilter>,
        IUpdate<UserRequest>,
        IDelete<User>
    {
        /// <summary>
        /// Get DB user model.
        /// </summary>
        object GetDbUser(UserFilter filter);

        //object нужно будет заменить на DbUser
    }
}
