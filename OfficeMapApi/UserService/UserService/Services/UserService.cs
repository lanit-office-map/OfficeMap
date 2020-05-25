using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Database.Entities;
using UserService.Services.Interfaces;

namespace UserService.Services
{
  public class UserService : IUserService
  {
    #region private fields
    private readonly UserManager<DbUser> userManager;
    private readonly IMapper mapper;
    #endregion

    #region public methods
    public UserService(
      [FromServices] UserManager<DbUser> userManager,
      [FromServices] IMapper mapper)
    {
      this.userManager = userManager;
      this.mapper = mapper;
    }


    #endregion

  }
}
