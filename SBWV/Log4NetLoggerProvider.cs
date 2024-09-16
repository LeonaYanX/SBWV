using Microsoft.Extensions.Logging;
using log4net;
using log4net.Core;



namespace SBWV
{
    public class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly ILog _logger;

        public Log4NetLogger(string categoryName)
        {
            _logger = LogManager.GetLogger(categoryName);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.Logger.IsEnabledFor(ConvertLogLevel(logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var message = formatter(state, exception);
                // Вызываем метод Log, передавая все необходимые параметры
                _logger.Logger.Log( callerStackBoundaryDeclaringType: null, ConvertLogLevel(logLevel), message, exception);
            }
        }

        private Level ConvertLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => Level.Trace,
                LogLevel.Debug => Level.Debug,
                LogLevel.Information => Level.Info,
                LogLevel.Warning => Level.Warn,
                LogLevel.Error => Level.Error,
                LogLevel.Critical => Level.Fatal,
                _ => Level.Info,
            };
        }
    }
}
