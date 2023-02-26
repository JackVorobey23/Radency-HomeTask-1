using CsvHelper;
using System.Globalization;
namespace HomeTask1
{
    class ReadTxtStrategy : IReadingStrategy
    {
        private string _path;
        private StreamReader _reader;
        public ReadTxtStrategy(string path)
        {
            _path = path;
            _reader = new StreamReader(_path);
        }
        
        //async Task<ProcessedFileInfo> IReadingStrategy.Reading()
        public ProcessedFileInfo Reading()
        {
            ProcessedFileInfo fileInfo = new();
            while(_reader.Peek() != -1)
            {
                string line = _reader.ReadLine();

                (CityData newCityData, bool isLineAvailable) = LineToCityData(line);

                
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
        private (CityData, bool) LineToCityData(string line)
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

            serviceData.payers.Add(payer);
            
            cityData.services.Add(serviceData);
            
            return (cityData, true);
        }
    }
}