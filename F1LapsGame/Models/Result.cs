using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace F1LapsGame.Models;

public class Result
{
    // resultId,raceId,driverId,constructorId,number,grid,position,positionText,positionOrder,
    // points,laps,time,milliseconds,fastestLap,rank,fastestLapTime,fastestLapSpeed,statusId

    // 25896,1100,822,51,77,0,11,"11",11,0,58,"+6.513",9164884,46,17,"1:22.233","231.060",1
    public int ResultId { get; set; }
    public int? RaceId { get; set; }
    public int? DriverId { get; set; }
    public int? ConstructorId { get; set; }
    public int? Number { get; set; }  // car number
    public int Grid { get; set; }  // starting position
    public int? Position { get; set; }
    public string PositionText { get; set; } = string.Empty;
    public int PositionOrder { get; set; }
    public float Points { get; set; }
    public int Laps { get; set; }
    public string Time { get; set; } = string.Empty;
    public int? Milliseconds { get; set; }
    public int? FastestLap { get; set; }
    public int? Rank { get; set; }
    public string FastestLapTime { get; set; } = string.Empty;
    public string FastestLapSpeed { get; set; } = string.Empty;
    public int StatusId { get; set; }

    public Race Race { get; set; } = null!;
    public Driver Driver { get; set; } = null!;
    public Constructor Constructor { get; set; } = null!;
}

public class ResultEntityConfigration : IEntityTypeConfiguration<Result>
{
    public void Configure(EntityTypeBuilder<Result> builder)
    {
        builder.HasKey(e => e.ResultId);
        // One-to-many without navigation to dependents
        builder.HasOne(e => e.Race).WithMany().OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(e => e.Driver).WithMany().OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(e => e.Constructor).WithMany().OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.RaceId).IsUnique(false);
        builder.HasIndex(e => e.DriverId).IsUnique(false);
        builder.HasIndex(e => e.ConstructorId).IsUnique(false);

    }
}