using CsvHelper.Configuration;
using F1LapsGame.Models;

namespace EFGetStarted.Models;

public class LaptimeMap : ClassMap<Laptime>
{
    public LaptimeMap()
    {
        Map(m => m.RaceId);
        Map(m => m.DriverId);
        Map(m => m.Lap);
        Map(m => m.Position);
        Map(m => m.Time);
        Map(m => m.Milliseconds);

    }
}