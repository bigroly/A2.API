using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Models
{
  public class BaseResponse
  {
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
  }
}
