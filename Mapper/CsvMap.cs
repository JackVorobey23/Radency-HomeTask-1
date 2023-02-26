using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

namespace HomeTask1
{
    public class DateConverter<DateTime> : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return Convert.ToDateTime(text);
        }
    }
    class CsvMap: ClassMap<CsvLine>
    {
        public CsvMap()
        {
            Map(m => long.Parse(m.account_number.ToString().Replace("\"", "")));
        }
    }

    internal class CsvLine
    {
        [Index(0)]
        public string first_name {get; set;}

        [Index(1)]
        public string last_name {get; set;}

        [Index(2)]
        public string address {get; set;}

        [Index(5)]
        public decimal payment {get; set;}

        [Index(6)]
        public string date {get; set;}

        [Index(7)]
        public long account_number {get; set;}

        [Index(8)]
        public string service {get; set;}
    }
}
