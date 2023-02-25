using System.Text.Json;
namespace HomeTask1
{
    class Config
    {
        public Paths paths;
        public void CreateOutputDirectory(string day)
        {
            using (StreamWriter configWriter = new StreamWriter(@"./Configuration/config.json"))
            {
                string dir = paths.OutputPath.Substring(0, paths.OutputPath.Length - 10);
                
                paths.OutputPath = dir + day;

                Directory.CreateDirectory(paths.OutputPath);
                
                configWriter.Write(JsonSerializer.Serialize<Paths>(paths));
            }
        }
        public void GetPathes()
        {
            string jsonString = "";

            using (var jsonReader = new StreamReader(@"./Configuration/config.json"))
            {
                jsonString = jsonReader.ReadToEnd();
            }
            try
            {
                paths = JsonSerializer.Deserialize<Paths>(jsonString);
            } 
            catch (JsonException jEx) 
            {
                Console.WriteLine(jEx.Message);
                Console.WriteLine("Write a files to source folder or change file directory in config.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}