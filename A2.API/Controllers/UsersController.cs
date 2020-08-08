using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.API.Models;
using A2.API.Models.Users;
using A2.API.Services;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace A2.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;    

    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet("")]
    public async Task<GetAllUsersResponse> GetAllUsers()
    {
      try
      {
        return new GetAllUsersResponse()
        {
          Success = true,
          Users = await _userService.GetAllUsers()
        };
      }
      catch(Exception e)
      {
        return new GetAllUsersResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpPost()]
    public async Task<CreateUserResponse> CreateUser(User user)
    {
      try
      {
        return new CreateUserResponse()
        {
          Success = await _userService.CreateUser(user)
        };
      }
      catch(Exception e)
      {
        return new CreateUserResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpPut]
    public async Task<UpdateUserResponse> UpdateUser(User user)
    {
      try
      {
        return new UpdateUserResponse()
        {
          Success = await _userService.UpdateUser(user)
        };
      }
      catch(Exception e)
      {
        return new UpdateUserResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

  }
}
