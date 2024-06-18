using CsvHelper.Configuration;
using F1LapsGame.Models;

namespace EFGetStarted.Models;

public class RaceMap : ClassMap<Race>
{
    public RaceMap()
    {
        Map(m => m.RaceId);
        Map(m => m.Year);
        Map(m => m.Round);
        Map(m => m.CircuitId);
        Map(m => m.Name);
        Map(m => m.Date);
    }
}