using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Organization.Application.Services;
using Organization.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Organization.Shared.Interfaces;

namespace Organization.Api.Middleware
{
    public class CustomerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public CustomerMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context, ICustomer customer)
        {
            var user = context.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var customerClaim = user.FindFirst("Customer")?.Value;
                if (Guid.TryParse(customerClaim, out var customerId))
                {
                    customer.Id = customerId;
                    customer.Alias = "user"; // or fetch from db
                }
            }
            else
            {
                // For unauthenticated, perhaps default or something
                customer.Id = Guid.Empty;
                customer.Alias = "guest";
            }

            await _next(context);            
        }
    }
}