using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Authentication.Data;
using Authentication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Core.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Authentication.Application.Data;
using Authentication.Application.Services.Interfaces;
using Authentication.Application.Services;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace Authentication.Api
{
    public static class Authentication
    {
        public static IServiceCollection AddAuthenticationModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AuthenticationConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback or throw to make debugging easier
                throw new InvalidOperationException("Connection string 'AuthenticationConnection' not found in configuration.");
            }

            services.AddDbContext<AuthenticationDbContext>(options =>
                options.UseNpgsql(connectionString));


            // services.AddIdentity<User, IdentityRole>()
            //     .AddEntityFrameworkStores<AuthenticationDbContext>()
            //     .AddDefaultTokenProviders();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerAccess", policy => policy.RequireClaim("Customer"));
                options.AddPolicy("AdminRole", policy => policy.RequireRole("Admin"));
                // Add more policies as needed
            });

            services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AuthenticationDbContext>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/access-denied";
                options.Cookie.HttpOnly = true;
                options.Cookie.MaxAge = TimeSpan.FromSeconds(3600 * 60);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/login"))
                        {
                            return Task.CompletedTask;
                        }

                        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                };
            });
            // services.AddAuthorizationBuilder();

            // services.AddIdentityCore<User>()
            //     .AddEntityFrameworkStores<AuthenticationDbContext>()
            //     .AddApiEndpoints();


            // services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            // })
            //     .AddJwtBearer(options =>
            //     {
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuer = false,
            //             ValidateAudience = false,
            //             ValidateLifetime = false,
            //             ValidateIssuerSigningKey = true,
            //             ValidIssuer = "https://localhost:5000",
            //             ValidAudience = "https://localhost:5000",
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sdfdf$%#765GFHD3245rRFgDSW#$@#erfDFS@#$%%^Y$FGD"))
            //         };
            //     });


            // Example: Design-time usage of the factory (for migrations, not runtime)
            // var factory = new Authentication.Data.AuthenticationDesignTimeDbContextFactory();
            // using var context = factory.CreateDbContext(Array.Empty<string>());
            // // Use 'context' for design-time operations (e.g., migrations)

            // services.AddScoped<IApplicationDbContext>(provider =>
            //     provider.GetRequiredService<AuthenticationDbContext>());

            services.AddTransient<IAuthenticationDbContext, AuthenticationDbContext>();
            services.AddScoped<IUserService, UserService>();

            // Register all IApplicationService implementations from Authentication.Application
            // var appAssembly = typeof(UserService).Assembly;
            // var interfaceType = typeof(IApplicationService);
            // var implementations = appAssembly.DefinedTypes
            //     .Where(t => interfaceType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
            // foreach (var impl in implementations)
            // {
            //     services.AddTransient(interfaceType, impl);
            // }

            return services;
        }

        public static IApplicationBuilder UseAuthenticationModule(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            // app.MapCustomIdentityApi<User>();

            return app;
        }
    }

    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<Guid>>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Customer", user.CustomerId.ToString()));
            return identity;
        }
    }
}