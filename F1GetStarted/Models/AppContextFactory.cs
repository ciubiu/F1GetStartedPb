using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using F1LapsGame.Models;

namespace EFGetStarted.Models;

// for importing csv files project
public class AppContextFactory : IDesignTimeDbContextFactory<LaptimeContext>
{
    public LaptimeContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<LaptimeContext>();
        optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));

        return new LaptimeContext(optionsBuilder.Options);
    }
}