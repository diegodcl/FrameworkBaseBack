using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Organization.Data;
using Organization.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Core.Modules;
using Core.Infrastructure.Data;
using System.Text;
using Organization.Application.Services.Interfaces;
using Organization.Application.Services;
using Organization.Application.Data;
using Organization.Api.Middleware;
using Organization.Shared.Interfaces;
using Organization.Domain.Entities;


namespace Organization.Api
{
    public static class Organization
    {
        public static IServiceCollection AddOrganizationModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Usa a factory genérica para criar o contexto
            var factory = new GenericDesignTimeDbContextFactory<OrganizationDbContext>(null, "OrganizationConnection");

            var context = factory.CreateDbContext(new string[] { });

            services.AddDbContext<OrganizationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("OrganizationConnection")));
            // Opcional: registrar o contexto criado pela factory como singleton
            // services.AddSingleton(context);

            // Register all IApplicationService implementations from Authentication.Application
            // var appAssembly = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly;
            // var interfaceType = typeof(IApplicationService);
            // var implementations = appAssembly.DefinedTypes
            //     .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            // foreach (var impl in implementations)
            // {
            //     services.AddTransient(interfaceType, impl);
            // }

            services.AddScoped<IOrganizationDbContext, OrganizationDbContext>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IPersonService, PersonService>();
            services.AddScoped<Customer>();
            services.AddScoped<ICustomer>(c => c.GetService<Customer>());

            return services;
        }

        public static IApplicationBuilder UseOrganizationModule(this IApplicationBuilder app)
        {

            app.UseMiddleware<CustomerMiddleware>();
            return app;
        }
    }
}