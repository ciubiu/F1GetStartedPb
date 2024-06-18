using CsvHelper.Configuration;
using F1LapsGame.Models;

namespace EFGetStarted.Models;

public class DriverMap : ClassMap<Driver>
{
    public DriverMap()
    {
        Map(m => m.DriverId);
        Map(m => m.DriverRef);
        Map(m => m.Number).TypeConverter<NullInt32Converter>();
        Map(m => m.Code).TypeConverter<NullStringConverter>();
        Map(m => m.Forename);
        Map(m => m.Surname);
        Map(m => m.Dob);
        Map(m => m.Nationality);
        Map(m => m.Url);

    }
}