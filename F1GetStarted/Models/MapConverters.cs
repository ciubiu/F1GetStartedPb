using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace EFGetStarted.Models;

public class NullInt32Converter : DefaultTypeConverter
{
    public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text == "\\N" ? null : int.Parse(text);
    }
}

public class NullStringConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        return text == "\\N" ? String.Empty : (object)text;
    }
}