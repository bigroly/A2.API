using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Contracts.Response
{
  public class BaseResponse
  {
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
  }
}
