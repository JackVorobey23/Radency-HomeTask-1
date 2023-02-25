using System.CommandLine;
using System.Text.Json;
namespace HomeTask1
{
    class Program
    {
        static async Task Main(string[] args)
        {

            Config config = new();
            config.GetPathes();
            MidnightWaiter waiter = new(config);


            int fullDayMsc = 24*60*60*1000;
            new Timer(
                (object obj) => waiter.WriteMetaData(),
                null, fullDayMsc - (int)DateTime.Now.TimeOfDay.TotalMilliseconds,
                fullDayMsc);

            
            var rootCommand = new RootCommand();
            var startCommand = new Command("read","read file");
            var stopCommand = new Command("stop","stop the program.");

            
            startCommand.SetHandler(()=> 
            {
                FileReader reader = new FileReader(waiter, config);
                reader.Start();
            });

            stopCommand.SetHandler(()=> 
            {
                System.Console.WriteLine("stopping...");
                Environment.Exit(0);
            });
            
            rootCommand.AddCommand(startCommand);
            rootCommand.AddCommand(stopCommand);

            await rootCommand.InvokeAsync(args);

            while(true)
                await rootCommand.InvokeAsync(Console.ReadLine());
        }
    }
}