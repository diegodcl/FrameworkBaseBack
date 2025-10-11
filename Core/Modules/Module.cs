using Core.Modules.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Core.Modules
{
  public class ModMonModule
  {
    public IConfiguration _configuration;

    public ModMonModule(string modulePath)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(modulePath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        _configuration = builder.Build();
    }

    // public void Start(WebApplicationBuilder builder)
    // {
    //     ConfigureServices(builder);
    // }

    // public abstract void ConfigureServices(WebApplicationBuilder builder);

    // Virtual async method for running database seeders after DI container is built
    // public virtual Task RunSeederAsync(IServiceProvider services)
    // {
    //     return Task.CompletedTask;
    // }
  }
}