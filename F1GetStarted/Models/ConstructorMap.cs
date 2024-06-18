using CsvHelper.Configuration;
using F1LapsGame.Models;

namespace EFGetStarted.Models;

public class ConstructorMap : ClassMap<Constructor>
{
    public ConstructorMap()
    {
        Map(m => m.ConstructorId);
        Map(m => m.ConstructorRef);
        Map(m => m.Name);
        Map(m => m.Nationality);
        Map(m => m.Url);
    }
}
