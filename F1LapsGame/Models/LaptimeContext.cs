using Microsoft.EntityFrameworkCore;

namespace F1LapsGame.Models;

public class LaptimeContext : DbContext
{
    public LaptimeContext(DbContextOptions<LaptimeContext> options) : base(options)
    {
        this.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Laptime> Laptimes { get; set; }
    public DbSet<Race> Races { get; set; }
    public DbSet<Constructor> Constructors { get; set; }
    public DbSet<Result> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // looks for all classes that implement IEntityTypeConfiguration and applies them
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LaptimeContext).Assembly);
    }
}
