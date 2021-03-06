﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Models.Users
{
  public class User
  {
    public string EmailAddress { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ContactNumber { get; set; }
    public string ManagerEmail { get; set; }
    public DateTime StartDate { get; set; }
    public string JobTitle { get; set; }
  }
}
