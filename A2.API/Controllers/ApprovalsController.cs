using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.API.Models.Approvals;
using A2.API.Models.Requests;
using A2.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A2.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ApprovalsController : ControllerBase
  {
    private IApprovalsService _approvalService;
    public ApprovalsController(IApprovalsService approvalsService)
    {
      _approvalService = approvalsService;
    }

    [HttpPost]
    public async Task<AddUpdateApprovalResponse> AddUpdateApproval(Approval request)
    {
      try
      {
        return new AddUpdateApprovalResponse()
        {
          Success = await _approvalService.AddUpdateApproval(request)
        };
      }
      catch(Exception e)
      {
        return new AddUpdateApprovalResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpGet]
    public async Task<GetApprovalsForUserResponse> GetApprovalsForUser(string userEmail)
    {
      try
      {
        return new GetApprovalsForUserResponse()
        {
          Success = true,
          Approvals = await _approvalService.GetApprovalsForUser(userEmail)
        };
      }
      catch(Exception e)
      {
        return new GetApprovalsForUserResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

  }
}
