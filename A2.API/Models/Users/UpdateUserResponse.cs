﻿using A2.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Models.Users
{
  public class UpdateUserResponse: BaseResponse
  {
    public User User { get; set; }
  }
}
