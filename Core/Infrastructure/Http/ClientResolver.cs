using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IClientResolver
{
    string ResolveClientId();
}

namespace Core.Infrastructure.Http
{
    public class ClientResolver : IClientResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ResolveClientId()
        {
            var host = _httpContextAccessor.HttpContext?.Request.Host.Value;
            if (string.IsNullOrEmpty(host))
                return null;
            return host.Split('.')[0]; // Assuming subdomain is used for client identification
        }
    }
}