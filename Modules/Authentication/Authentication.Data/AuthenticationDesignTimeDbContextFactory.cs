using Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Authentication.Data
{
    public class AuthenticationDesignTimeDbContextFactory : GenericDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        public AuthenticationDesignTimeDbContextFactory()
            : base(GetAppSettingsPath(), "AuthenticationConnection")
        {
        }

        private static string GetAppSettingsPath()
        {
            // Find the solution root by traversing up until Condominio.sln is found
            var dir = AppContext.BaseDirectory;
            while (!System.IO.File.Exists(System.IO.Path.Combine(dir, "Condominio.sln")))
            {
                var parent = System.IO.Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            var appSettingsPath = System.IO.Path.Combine(dir, "Modules", "Authentication", "Authentication.Api", "appsettings.json");
            return appSettingsPath;
        }
    }
}
