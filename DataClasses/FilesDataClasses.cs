namespace HomeTask1
{
    public class MetaLogData
    {
        public int parsed_files {get;set;} = 0;
        public int parsed_lines {get;set;} = 0;
        public int found_errors {get;set;} = 0;
        public List<string> invalid_files {get;set;} = new List<string>();
    }
    public class ProcessedFileInfo 
    {
        public int parsedLines { get; set; } = 0;
        public int foundErrors { get; set; } = 0;
        public List<CityData> cityDatas { get; set; } = new List<CityData>();
    }
    class Paths
    {
        public string InputPath {get;set;}
        public string OutputPath {get;set;}
    }
}