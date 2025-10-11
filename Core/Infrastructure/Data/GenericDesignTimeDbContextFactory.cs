using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Core.Infrastructure.Data
{
    public class GenericDesignTimeDbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly string _appSettingsPath;
        private readonly string _connectionStringName;

        public GenericDesignTimeDbContextFactory(string appSettingsPath = null, string connectionStringName = "DefaultConnection")
        {
            _appSettingsPath = appSettingsPath ?? FindAppSettingsPath();
            _connectionStringName = connectionStringName;
        }

        public TContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(_appSettingsPath))
                .AddJsonFile(Path.GetFileName(_appSettingsPath), optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString(_connectionStringName);
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
        }

        protected static string FindAppSettingsPath(string relativePath = "Modules/Authentication/Authentication.Api/appsettings.json")
        {
            // Find the solution root by traversing up until Condominio.sln is found
            var dir = AppContext.BaseDirectory;
            while (!File.Exists(Path.Combine(dir, "Condominio.sln")))
            {
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            var appSettingsPath = Path.Combine(dir, relativePath.Replace('/', Path.DirectorySeparatorChar));
            return appSettingsPath;
        }
    }
}
