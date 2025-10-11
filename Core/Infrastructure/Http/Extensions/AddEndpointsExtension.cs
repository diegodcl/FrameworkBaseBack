using System.ComponentModel;
using System.Reflection;
using Core.Infrastructure.Http.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Infrastructure.Http.Extensions;

public static class AddEndpointExtension 
{
  public static IServiceCollection AddEndpoints(this IServiceCollection services)
  {
    Type interfaceType = typeof(IEndpoint); 
    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies(); 

    foreach (Assembly assembly in assemblies) 
    { 
      ServiceDescriptor[] endpointServiceDescriptors = assembly
        .DefinedTypes
        .Where(type => type is {IsAbstract: false, IsInterface: false} &&
                       type.IsAssignableTo(typeof(IEndpoint)))
        .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
        .ToArray();

        services.TryAddEnumerable(endpointServiceDescriptors);  
    }
    return services;
  }

  public static IApplicationBuilder MapEndpoints(this WebApplication app)
  {
    // IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
    // foreach (IEndpoint endpoint in endpoints)
    // {
    //   endpoint.MapEndpoints(app);
    // }

    using (var scope = app.Services.CreateScope())
    {
        var endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(app);
        }
    }
    return app;
  }
}