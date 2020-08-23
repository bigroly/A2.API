using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Models.Requests
{
  public class LeaveRequest
  {
    public string Requestor { get; set; }
    public string Approver { get; set; }
    public string RequestGuid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LeaveType { get; set; }
    public string Note { get; set; }
    public string DocLink { get; set; }
  }
}
