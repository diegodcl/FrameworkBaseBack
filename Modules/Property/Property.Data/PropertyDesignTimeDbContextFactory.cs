using Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Property.Data
{
    public class PropertyDesignTimeDbContextFactory : GenericDesignTimeDbContextFactory<PropertyDbContext>
    {
        public PropertyDesignTimeDbContextFactory()
            : base(GetAppSettingsPath(), "PropertyConnection")
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
            var appSettingsPath = System.IO.Path.Combine(dir, "Modules", "Property", "Property.Api", "appsettings.json");
            return appSettingsPath;
        }
    }
}