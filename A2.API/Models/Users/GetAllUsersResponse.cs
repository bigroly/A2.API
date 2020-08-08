using A2.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Models.Users
{
  public class GetAllUsersResponse: BaseResponse
  {
    public List<User> Users { get; set; }
  }
}
