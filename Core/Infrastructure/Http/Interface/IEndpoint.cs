using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Core.Infrastructure.Http.Interface
{
  public interface IEndpoint {
    void MapEndpoints (WebApplication app);
  }

}