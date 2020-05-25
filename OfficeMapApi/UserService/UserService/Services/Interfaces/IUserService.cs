using System;
using System.Collections.Generic;
using Common.Interfaces;
using UserService.Models;
using Common.Response;

namespace UserService.Services.Interfaces
{
  public interface IUserService:
    IGet<Guid, Response<UserResponse>>,
    IDelete<Guid>,
    IFindAll<Response<IEnumerable<UserResponse>>>,
    ICreate<RegisterUserModel, Response<UserResponse>>,
    IUpdate<User, Response<UserResponse>>
  {
  }
}
