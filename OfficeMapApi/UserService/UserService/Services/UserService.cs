using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UserService.Database.Entities;
using UserService.Models;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services
{
  public class UserService : IUserService
  {
    #region private fields
    private readonly UserManager<DbUser> userManager;
    private readonly IUserRepository userRepository;
    private readonly IMapper autoMapper;
    #endregion

    #region public methods
    public UserService(
      [FromServices] UserManager<DbUser> userManager,
      [FromServices] IUserRepository userRepository,
      [FromServices] IMapper autoMapper)
    {
      this.userManager = userManager;
      this.userRepository = userRepository;
      this.autoMapper = autoMapper;
    }

    public async Task<Response<UserResponse>> CreateAsync(RegisterUserModel entity)
    {
      var newUser = autoMapper.Map<DbUser>(entity);
      var result = await userManager.CreateAsync(newUser, entity.Password);
      if (!result.Succeeded)
      {
        return new Response<UserResponse>(
          HttpStatusCode.BadRequest,
          JsonConvert.SerializeObject(result.Errors));
      }

      var createdUser = await userManager.Users
        .Include(u => u.Employee)
        .FirstOrDefaultAsync(u => u.Email.Equals(entity.Email));

      if (createdUser == null)
      {
        return new Response<UserResponse>(
          HttpStatusCode.NotFound,
          $"User with email '{entity.Email}' was not found.");
      }

      return new Response<UserResponse>(autoMapper.Map<UserResponse>(createdUser));
    }

    public async Task DeleteAsync(Guid input)
    {
      var user = await userRepository.GetAsync(input);
      if (user != null)
      {
        await userManager.DeleteAsync(user);
      }
    }

    public async Task<Response<IEnumerable<UserResponse>>> FindAllAsync()
    {
      var users = await userRepository.FindAllAsync();

      return new Response<IEnumerable<UserResponse>>(
        autoMapper.Map<IEnumerable<UserResponse>>(users));
    }

    public async Task<Response<UserResponse>> GetAsync(Guid key)
    {
      var user = await userRepository.GetAsync(key);

      if (user == null)
      {
        return new Response<UserResponse>(
          HttpStatusCode.NotFound,
          $"User with guid '{key}' was not found.");
      }

      return new Response<UserResponse>(autoMapper.Map<UserResponse>(user));
    }

    public async Task<Response<UserResponse>> UpdateAsync(User target)
    {
      var source = await userRepository.GetAsync(target.UserGuid);

      if (source == null)
      {
        return new Response<UserResponse>(
          HttpStatusCode.NotFound,
          $"User with guid '{target.UserGuid}' was not found.");
      }

      if (target.Employee != null)
      {
        source.Employee.FirstName = target.Employee.FirstName;
        source.Employee.SecondName = target.Employee.SecondName;
      }

      var result = await userManager.UpdateAsync(source);

      if (!result.Succeeded)
      {
        return new Response<UserResponse>(
          HttpStatusCode.BadRequest,
          JsonConvert.SerializeObject(result.Errors));
      }

      var updatedUser = await userRepository.GetAsync(target.UserGuid);

      return new Response<UserResponse>(autoMapper.Map<UserResponse>(updatedUser));
    }
    #endregion

  }
}
