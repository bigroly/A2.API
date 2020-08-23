using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace A2.API.Services
{
  public interface IFileService
  {
    Task<bool> PostFile(string base64Content, string fileKey);
    string GenerateLinkToFile(string fileKey);
  }

  public class FileService: IFileService
  {
    private readonly IAmazonS3 _s3Client;
    private IConfiguration _config;

    public FileService(IAmazonS3 s3Client, IConfiguration config)
    {
      _s3Client = s3Client;
      _config = config;
    }

    public async Task<bool> PostFile(string base64Content, string fileKey)
    {
      using (var inputStream = new MemoryStream(Convert.FromBase64String(base64Content)))
      {
        var success = await _s3Client.PutObjectAsync(new PutObjectRequest
        {
          InputStream = inputStream,
          ContentType = "application/pdf",
          BucketName = _config.GetValue<string>("S3:DocumentsBucketName"),
          Key = fileKey,
          CannedACL = S3CannedACL.BucketOwnerFullControl
        });
      }

      return true;        
    }

    public string GenerateLinkToFile(string fileKey)
    {
      try
      {
        GetPreSignedUrlRequest s3Request = new GetPreSignedUrlRequest()
        {
          BucketName = _config.GetValue<string>("S3:DocumentsBucketName"),
          Key = fileKey,
          Expires = DateTime.Now.AddMinutes(5)
        };
        return _s3Client.GetPreSignedURL(s3Request);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

  }
}
