using CsvHelper;
using System.Globalization;
namespace HomeTask1
{
    public interface IReadingStrategy
    {
        Task<ProcessedFileInfo> Reading();
        (CityData, bool) ConvertLineToJson(string line);
    }
    class ReadTxtStrategy : IReadingStrategy
    {
        private string _path;
        private StreamReader _reader;
        public ReadTxtStrategy(string path)
        {
            _path = path;
            _reader = new StreamReader(_path);
        }
        
        async Task<ProcessedFileInfo> IReadingStrategy.Reading()
        {

            ProcessedFileInfo fileInfo = new();
            while(_reader.Peek() != -1)
            {
                string line = await _reader.ReadLineAsync();

                (CityData newCityData, bool isLineAvailable) = ConvertLineToJson(line);

                
                if(isLineAvailable)
                {
                    fileInfo.cityDatas.Add(newCityData);
                    fileInfo.parsedLines++;
                }

                else
                {
                    fileInfo.foundErrors++;
                }
            }

            _reader.Close();

            return fileInfo;
        }
        public (CityData, bool) ConvertLineToJson(string line)
        {
            Payer payer = new();
            CityData cityData = new();
            ServiceData serviceData = new();

            string[] data = new string[7];
            string[] splLine = line.Split(',');
            
            try
            {
                payer.name = splLine[0]+splLine[1];
                payer.payment = decimal.Parse(splLine[^4], CultureInfo.InvariantCulture);
                payer.account_number = int.Parse(splLine[^2].Replace("\"",""));

                serviceData.name = splLine[^1];

                DateTime.TryParseExact(splLine[^3].Substring(1), "yyyy-dd-MM", null,
                                        DateTimeStyles.None, out DateTime payerDate);
                payer.date = payerDate;
                
                cityData.city = splLine[2].Substring(2);
            }
            catch (Exception) 
            {
                return (null, false);
            }
            
            cityData.services = new();
            cityData.services.Add(serviceData);
            
            serviceData.payers = new();
            serviceData.payers.Add(payer);

            return (cityData, true);
        }
    }

    class ReadCsvStrategy : IReadingStrategy
    {
        private CsvReader _csvReader;
        private string _path;
        public ReadCsvStrategy(string path)
        {
            _path = path;
            StreamReader reader = new StreamReader(_path);
            _csvReader = new CsvReader(reader, CultureInfo. InvariantCulture);
            reader.Close();
        }


        async Task<ProcessedFileInfo> IReadingStrategy.Reading()
        {
            await _csvReader.ReadAsync();
            string text = _csvReader.GetField("nam");
            return new ProcessedFileInfo();
        }
        public (CityData, bool) ConvertLineToJson(string line)
        {
            throw new NotImplementedException();
        }
    }    
}