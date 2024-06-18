using CsvHelper.Configuration;
using F1LapsGame.Models;

namespace EFGetStarted.Models;

public class ResultMap : ClassMap<Result>
{
    public ResultMap()
    {
        Map(m => m.ResultId);
        Map(m => m.RaceId);
        Map(m => m.DriverId);
        Map(m => m.ConstructorId);
        Map(m => m.Number).TypeConverter<NullInt32Converter>();
        Map(m => m.Grid);
        Map(m => m.Position).TypeConverter<NullInt32Converter>();
        Map(m => m.PositionText);
        Map(m => m.PositionOrder);
        Map(m => m.Points);
        Map(m => m.Laps);
        Map(m => m.Time).TypeConverter<NullStringConverter>();
        Map(m => m.Milliseconds).TypeConverter<NullInt32Converter>();
        Map(m => m.FastestLap).TypeConverter<NullInt32Converter>();
        Map(m => m.Rank).TypeConverter<NullInt32Converter>();
        Map(m => m.FastestLapTime).TypeConverter<NullStringConverter>();
        Map(m => m.FastestLapSpeed).TypeConverter<NullStringConverter>();
        Map(m => m.StatusId);
    }
}

