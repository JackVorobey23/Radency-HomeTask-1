using System.Text.Json;
namespace HomeTask1
{
    class MidnightWaiter
    {
        private Config _config;
        public MetaLogData metaLogData {get; set;} = new();
        public MidnightWaiter(Config config)
        {
            _config = config;
        }
        public void WriteMetaData()
        {
            string metaData = JsonSerializer.Serialize(
                    metaLogData, new JsonSerializerOptions{WriteIndented = true}
                    );

            metaLogData = new();
            
            using (var writer = new StreamWriter(@"config.json", false))
            {
                writer.WriteLine(JsonSerializer.Serialize<Paths>(_config.paths));
            }

            using (var writer = new StreamWriter(_config.paths.OutputPath + "\\meta.log", false))
            {
                writer.WriteLine(metaData);
            }

            System.Console.WriteLine($"Meta.log for {_config.paths.OutputPath.Split('\\').Last()} created:");
            System.Console.WriteLine(metaData);
        }
    }
}