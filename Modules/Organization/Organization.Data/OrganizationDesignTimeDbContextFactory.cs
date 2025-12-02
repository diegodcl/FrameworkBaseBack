using Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Organization.Data
{
    public class OrganizationDesignTimeDbContextFactory : GenericDesignTimeDbContextFactory<OrganizationDbContext>
    {
        public OrganizationDesignTimeDbContextFactory()
            : base(GetAppSettingsPath(), "OrganizationConnection")
        {
        }

        private static string GetAppSettingsPath()
        {
            // Find the solution root by traversing up until backendapp.sln is found
            var dir = AppContext.BaseDirectory;
            while (!System.IO.File.Exists(System.IO.Path.Combine(dir, "backendapp.sln")))
            {
                var parent = System.IO.Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            var appSettingsPath = System.IO.Path.Combine(dir, "Modules", "Organization", "Organization.Api", "appsettings.json");
            return appSettingsPath;
        }
    }
}