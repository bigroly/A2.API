using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Services
{
  public interface ILeaveRequestService
  {

  }

  public class LeaveRequestService: ILeaveRequestService
  {
    private IUtilityService _utils;

    public LeaveRequestService(IUtilityService utils)
    {
      _utils = utils;
    }


  }
}
