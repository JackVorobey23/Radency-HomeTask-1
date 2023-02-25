using System.Text.Json;
namespace HomeTask1 
{
    class FileReader
    {
        private IReadingStrategy _strategy;
        private Config _config;
        private MidnightWaiter _waiter;
        public FileReader(MidnightWaiter waiter, Config config)
        {
            _waiter = waiter;
            _config = config;
        }
        public void SetStrategy(IReadingStrategy strategy)
        {
            _strategy = strategy;
        }
        public void Start()
        {
            List<string> FilePaths = FindInputFiles();

            List<CityData> cityDataList = new();

            MetaLogData metaLogData = new();

            FilePaths.ForEach(async fP => 
            {
                var procFileData = await ExecuteReading(fP);

                cityDataList = cityDataList.Concat(procFileData.cityDatas).ToList();

                if(procFileData.foundErrors > 0) 
                    metaLogData.invalid_files.Add(fP);

                metaLogData.found_errors += procFileData.foundErrors;
                metaLogData.parsed_lines += procFileData.parsedLines;
                metaLogData.parsed_files++;
            });

            DataToJsonProcessor processor = new();

            string toWrite = JsonSerializer.Serialize<List<CityData>>(
                processor.CompareCityDatas(cityDataList), new JsonSerializerOptions{WriteIndented = true}
                );

            WriteOutput(toWrite, metaLogData);
            System.Console.WriteLine(String.Join(',', cityDataList));
        }
        private async Task<ProcessedFileInfo> ExecuteReading(string path)
        {
            IReadingStrategy _strategy = null;

            if (path.EndsWith("txt"))
            {
                _strategy = new ReadTxtStrategy(path);
            } 
            else if (path.EndsWith("csv"))
            {
                _strategy = new ReadCsvStrategy(path);
            }
            else 
            {
                throw new Exception("Invalid file types.");
            }

            var fileInfo = new ProcessedFileInfo();

            fileInfo = await _strategy.Reading();
            
            return fileInfo;
        }
        
        private void WriteOutput(string output, MetaLogData fileMLD)
        {
            string Day = DateTime.Now.Date.ToString().Split(' ')[0];

            if(_config.paths.OutputPath.Split('\\').Last() != Day) // check, if we need to create new day folder
            {
                _config.CreateOutputDirectory(Day);
            }

            int count = Directory.GetFiles(_config.paths.OutputPath).Length + 1;

            StreamWriter outputWriter = new StreamWriter(_config.paths.OutputPath + "\\output" + count + ".json");
            outputWriter.Write(output);
            outputWriter.Close();

            
            _waiter.metaLogData.invalid_files = 
                _waiter.metaLogData.invalid_files.Concat(fileMLD.invalid_files).ToList();
            
            _waiter.metaLogData.parsed_files += fileMLD.parsed_files;
            _waiter.metaLogData.parsed_lines += fileMLD.parsed_lines;
            _waiter.metaLogData.found_errors += fileMLD.found_errors;
        }

        private List<string> FindInputFiles()
        {
            _config.GetPathes();
            DirectoryInfo dInfo = new DirectoryInfo(_config.paths.InputPath);

            List<FileInfo> inputFiles = dInfo.GetFiles("*.*")
                .Where(file => file.FullName.ToLower().EndsWith("txt") || file.FullName.ToLower().EndsWith("csv"))
                .ToList();

            List<string> inputPaths = new();
            inputFiles.ForEach(iF => inputPaths.Add(iF.FullName));

            return inputPaths;
        }
    }
}