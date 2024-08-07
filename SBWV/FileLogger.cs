namespace SBWV
{
    public class FileLogger: ILogger
    {
        string filePath;
        static object _lock = new object();
        static DateTime lastClearedTime = DateTime.Now;

        public FileLogger(string path)
        {
            filePath = path;
            ClearIfNeeded();
        }
        private void ClearIfNeeded() 
        {
            if (DateTime.Now.Date != lastClearedTime.Date)
            {
                File.WriteAllText(filePath, string.Empty);
                lastClearedTime = DateTime.Now;
            }
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Dispose() { }

        public bool IsEnabled(LogLevel logLevel)
        {
            
           

            return  logLevel==LogLevel.Debug || logLevel==LogLevel.Information ;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId,
               TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (_lock)
            {
                File.AppendAllText(filePath, formatter(state, exception) +DateTime.Now + Environment.NewLine);
               
            }
        }
    }
}
