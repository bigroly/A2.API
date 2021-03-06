using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace A2.API
{
  public class Startup
  {
    public const string AppS3BucketKey = "AppS3Bucket";
    public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public static IConfiguration Configuration { get; private set; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddHttpClient();

      services.AddCors(options =>
      {
        options.AddPolicy(MyAllowSpecificOrigins,
              builder => builder.WithOrigins("*"));
      });

      // Add S3 to the ASP.NET Core dependency injection framework.
      services.AddAWSService<Amazon.S3.IAmazonS3>();
      services.AddAWSService<Amazon.CognitoIdentityProvider.IAmazonCognitoIdentityProvider>();
      services.AddAWSService<Amazon.DynamoDBv2.IAmazonDynamoDB>();

      services.AddSingleton<IUtilityService, UtilityService>();
      services.AddSingleton<IUserService, UserService>();
      services.AddSingleton<ILeaveRequestService, LeaveRequestService>();
      services.AddSingleton<IFileService, FileService > ();
      services.AddSingleton<IApprovalsService, ApprovalsService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();
      app.UseCors(MyAllowSpecificOrigins);
      app.Use((context, next) =>
      {
        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
        return next.Invoke();
      });

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
      //app.UseMvc();
    }
  }
}
