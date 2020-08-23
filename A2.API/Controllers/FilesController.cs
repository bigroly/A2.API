using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.API.Models.Files;
using A2.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A2.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FilesController : ControllerBase
  {
    private IFileService _fileService;

    public FilesController(IFileService fileService)
    {
      _fileService = fileService;
    }

    [HttpPost]
    public async Task<PostFileResponse> PostFile(PostFileRequest request)
    {
      try
      {
        var postComplete = await _fileService.PostFile(request.FileBase64, request.FileKey);
        return new PostFileResponse()
        {
          Success = true
        };
      }
      catch(Exception e)
      {
        return new PostFileResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

    [HttpGet]
    public GetFileResponse GetFile(string fileKey)
    {
      try
      {
        return new GetFileResponse()
        {
          Success = true,
          FileUrl = _fileService.GenerateLinkToFile(fileKey)
        };
      }
      catch(Exception e)
      {
        return new GetFileResponse()
        {
          Success = false,
          ErrorMessage = e.Message
        };
      }
    }

  }
}
