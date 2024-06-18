using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace F1LapsGame.Models;

public class Race
{
    //  raceId,year,round,circuitId,name,date,time,    url,fp1_date,fp1_time,fp2_date,fp2_time,fp3_date,fp3_time,quali_date,quali_time,sprint_date,sprint_time
    //  16,2009,16,18,"Brazilian Grand Prix","2009-10-18","16:00:00",    "http...",\N,\N,\N,\N,\N,\N,\N,\N,\N,\N
    // 1144 rows

    // primary columns only
    public int RaceId { get; set; }
    public int Year { get; set; }
    public int Round { get; set; }
    public int CircuitId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

public class RaceEntityConfigration : IEntityTypeConfiguration<Race>
{
    public void Configure(EntityTypeBuilder<Race> builder)
    {
        builder.HasKey(e => e.RaceId);
    }
}


