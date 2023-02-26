using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace HomeTask1 
{
    class ReadCsvStrategy : IReadingStrategy
    {
        private string _path;
        public ReadCsvStrategy(string path)
        {
            _path = path;
        }

        //async Task<ProcessedFileInfo> IReadingStrategy.Reading()
        public ProcessedFileInfo Reading()
        {
            ProcessedFileInfo fileInfo = new();
            var reader = new StreamReader(_path);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    BadDataFound = null
                };
            var csvReader = new CsvReader(reader, config);

            csvReader.Read();
            csvReader.ReadHeader();

            //while(csvReader.ReadAsync())
            while(csvReader.Read())
            {
                CsvLine csvLine = null;
                try
                {
                    csvLine = csvReader.GetRecord<CsvLine>();
                }
                catch (Exception)
                {
                    fileInfo.foundErrors++;
                    continue;
                }

                (CityData newCityData, bool isLineAvailable) = LineToCityData(csvLine);

                
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
            reader.Close();
            csvReader.Dispose();

            return fileInfo;
        }
        private (CityData, bool) LineToCityData(CsvLine csvLine)
        {
            Payer payer = new();
            CityData cityData = new();
            ServiceData serviceData = new();

            try
            {
                payer.account_number = csvLine.account_number;
                
                payer.name = csvLine.first_name + csvLine.last_name;

                DateTime.TryParseExact(csvLine.date, "yyyy-dd-MM", null,
                                        DateTimeStyles.None, out DateTime payerDate);
                payer.date = payerDate;

                payer.payment = csvLine.payment;
                
                serviceData.name = csvLine.service;

                cityData.city = csvLine.address.Split('“')[1];
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