using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Webapia.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(GetApiProjectPath())
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException(
                                   "No 'DefaultConnection' connection string found for design-time migration generation.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    // dotnet ef runs from the solution/infra project dir, not the API project's dir,
    // so we need to explicitly point at where Webapia.Api's appsettings.json lives.
    private static string GetApiProjectPath()
    {
        var infraProjectDir = Directory.GetCurrentDirectory();
        return Path.Combine(infraProjectDir, "..", "Webapia.Api");
    }
}