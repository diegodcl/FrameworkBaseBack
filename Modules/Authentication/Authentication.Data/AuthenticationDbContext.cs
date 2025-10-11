using Microsoft.EntityFrameworkCore;
using Authentication.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Authentication.Application.Data;
using Core.Infrastructure.Http;


namespace Authentication.Data
{
    public class AuthenticationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IAuthenticationDbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity mappings here
            // modelBuilder.Entity<User>().HasQueryFilter(u => u.Client == GetCurrentTenantId());

        }

        // private string GetCurrentClientId()
        // {
        //     return ClientResolver.ResolveClientId();
        // }
    }
}
