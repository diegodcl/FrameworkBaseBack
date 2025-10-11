using Microsoft.AspNetCore.Builder;

namespace Core.Modules.Interface
{
    public interface IModMonModule
    {
        public void Start(WebApplicationBuilder builder);
    }
}