using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Core.Infrastructure.Data;
using Property.Application.Data;
using Microsoft.EntityFrameworkCore;
using Property.Data;
using Property.Application.Services.Interfaces;
using Property.Application.Services;
using Organization.Shared.Interfaces;

namespace Property.Api
{
    public static class Property
    {
        public static IServiceCollection AddPropertyModule(this IServiceCollection services, IConfiguration configuration)
        {
            var factory = new GenericDesignTimeDbContextFactory<PropertyDbContext>(null, "PropertyConnection");
            var context = factory.CreateDbContext(new string[] { });

            services.AddDbContext<PropertyDbContext>((sp, options) => options.UseNpgsql(configuration.GetConnectionString("PropertyConnection")).EnableSensitiveDataLogging());
            // .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>()));

            // services.AddDbContext<PropertyDbContext>((sp, options) =>
            // {
            //     options.UseNpgsql(configuration.GetConnectionString("PropertyConnection"))
            //            .EnableSensitiveDataLogging();
            // }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            // services.AddScoped<PropertyDbContext>(sp =>
            // {
            //     var options = sp.GetRequiredService<DbContextOptions<PropertyDbContext>>();
            //     var customer = sp.GetRequiredService<ICustomer>();
            //     return new PropertyDbContext(options, customer);
            // });

            services.AddScoped<IPropertyDbContext, PropertyDbContext>();
            // services.AddTransient<IPropertyDbContext, PropertyDbContext>();
            services.AddScoped<IAreaTypeService, AreaTypeService>();
            services.AddScoped<IPropertyService, PropertyService>();

            return services;
        }

        public static IApplicationBuilder UsePropertyModule(this IApplicationBuilder app)
        {
            // Configure middleware related to the Property module here
            return app;
        }
    }
}