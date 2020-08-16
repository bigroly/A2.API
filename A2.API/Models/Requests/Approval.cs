using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace A2.API.Models.Requests
{
  public class Approval
  {  
    public string Approver { get; set; }
    public string Requestor { get; set; }
    public string RequestGuid { get; set; }
    public string Status { get; set; }
    public bool Approved { get; set; }    
  }
}
