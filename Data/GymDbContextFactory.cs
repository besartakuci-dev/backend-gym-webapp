using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace backend_gym_webapp.Data
{
    /// <summary>
    /// GymDbContextFactory - This helps Entity Framework create the DbContext at design time
    /// It's used by EF migrations to work without needing dependency injection
    /// This is required for 'dotnet ef' commands to work properly
    /// </summary>
    public class GymDbContextFactory : IDesignTimeDbContextFactory<GymDbContext>
    {
        // This method is called by EF migrations when you run:
        // - dotnet ef migrations add
        // - dotnet ef database update
        public GymDbContext CreateDbContext(string[] args)
        {
            // Create a configuration builder to read appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Create DbContext options with SQL Server provider
            var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Return a new instance of GymDbContext with these options
            return new GymDbContext(optionsBuilder.Options);
        }
    }
}
