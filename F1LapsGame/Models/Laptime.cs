using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace F1LapsGame.Models;

public class Laptime
{
    // raceId,driverId,lap,position,time,milliseconds
    //  567466 rows
    public int RowId { get; set; }
    public int? RaceId { get; set; }
    public int? DriverId { get; set; }
    public int Lap { get; set; }
    public int Position { get; set; }
    public string Time { get; set; } = string.Empty;
    public int Milliseconds { get; set; }

    // if the foreign key property is non-nullable, then the reference navigation may be nullable or not
    public Race Race { get; set; } = null!;
    public Driver Driver { get; set; } = null!;
}

public class DriverEntityConfigration : IEntityTypeConfiguration<Laptime>
{
    public void Configure(EntityTypeBuilder<Laptime> builder)
    {
        builder.HasKey(e => e.RowId);
        builder.HasOne(e => e.Driver).WithMany().OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(e => e.Race).WithMany().OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.DriverId).IsUnique(false);
        builder.HasIndex(e => e.RaceId).IsUnique(false);

    }
}