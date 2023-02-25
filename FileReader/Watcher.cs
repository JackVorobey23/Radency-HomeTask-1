using System.Text.Json;
namespace HomeTask1
{
    class Watcher
    {
        public void Watch()
        {
            Paths paths = new();
            string jsonString = "";

            using (var jsonReader = new StreamReader(@"./Configuration/config.json"))
            {
                jsonString = jsonReader.ReadToEnd();
            }
            
            paths = JsonSerializer.Deserialize<Paths>(jsonString)!;
            
            var watcher = new FileSystemWatcher(paths.InputPath);
        }
    }
}