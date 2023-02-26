namespace HomeTask1
{
    class WatcherConfig
    {
        private Config _config;
        private FileSystemWatcher _watcher;
        public WatcherConfig(Config config)
        {
            _config = config;
        }
        public FileSystemWatcher StartWatcher(FileSystemEventHandler handler)
        {
            _config.GetPathes();

            _watcher = new FileSystemWatcher(_config.paths.InputPath);

            _watcher.NotifyFilter = NotifyFilters.Attributes
                              | NotifyFilters.CreationTime
                              | NotifyFilters.DirectoryName
                              | NotifyFilters.FileName
                              | NotifyFilters.LastAccess
                              | NotifyFilters.LastWrite
                              | NotifyFilters.Security
                              | NotifyFilters.Size;

            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;

            _watcher.Changed += handler;
            _watcher.Created += handler;
            _watcher.Deleted += handler;

            _watcher.Error += (s, e) => PrintException(e.GetException())
            ;
            _watcher.Filters.Add("*.txt");
            _watcher.Filters.Add("*.csv");
            return _watcher;
        }
        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}