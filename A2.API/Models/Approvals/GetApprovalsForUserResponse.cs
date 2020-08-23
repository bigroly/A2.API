using A2.API.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Models.Approvals
{
  public class GetApprovalsForUserResponse: BaseResponse
  {
    public List<Approval> Approvals { get; set; }
  }
}
