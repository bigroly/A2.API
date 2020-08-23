using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.API.Models.Requests;
using A2.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A2.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LeaveRequestsController : ControllerBase
  {
    private ILeaveRequestService _leaveReqService;
    public LeaveRequestsController(ILeaveRequestService leaveReqService)
    {
      _leaveReqService = leaveReqService;
    }

    [HttpGet]
    public async Task<GetLeaveRequestsResponse> GetLeaveRequests(string requestorEmail)
    {
      try
      {
        return new GetLeaveRequestsResponse()
        {
          Success = true,
          Requests = await _leaveReqService.GetLeaveRequests(requestorEmail)
        };
      }
      catch(Exception e)
      {
        return new GetLeaveRequestsResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpPost]
    public async Task<AddUpdateLeaveRequestResponse> AddLeaveRequest(LeaveRequest request)
    {
      try
      {
        var updateReq = await _leaveReqService.AddUpdateLeaveRequest(request);
        //todo: create an approval in starting state
        
        return new AddUpdateLeaveRequestResponse()
        {
          Success = true
        };
      }
      catch(Exception e)
      {
        return new AddUpdateLeaveRequestResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }


  }
}
