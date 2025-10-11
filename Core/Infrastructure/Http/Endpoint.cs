using Core.Infrastructure.Http.Interface;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace Core.Infrastructure.Http
{
  public abstract class Endpoint : IEndpoint
  {
    protected string? _prefix { get; private set;}
    public Endpoint()
    {
      _prefix = Assembly.GetExecutingAssembly().FullName;
    }
    
    public Endpoint(string prefix)
    {
      _prefix = string.IsNullOrEmpty(prefix) ? prefix : Assembly.GetExecutingAssembly().FullName;
    }
    
    public abstract void MapEndpoints (WebApplication app);
  }
}