using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Models.Files
{
  public class PostFileRequest
  {
    public string FileKey { get; set; }
    public string FileBase64 { get; set; }
  }
}
