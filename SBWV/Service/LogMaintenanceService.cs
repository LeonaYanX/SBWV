
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;


namespace SBWV.Service
{
    public class LogMaintenanceService : BackgroundService
    {
        private readonly ILogger<LogMaintenanceService> _logger;
        private readonly string _logFilePath = "logger.txt";
        private readonly TimeSpan _interval = TimeSpan.FromDays(1);

        public LogMaintenanceService(ILogger<LogMaintenanceService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                PerformLogMaintenance();
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private void PerformLogMaintenance()
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    // Добавляем текущую дату в файл лога
                    File.AppendAllText(_logFilePath, $"--- Log Maintenance on {DateTime.Now} ---{Environment.NewLine}");

                    // Очистка файла (опционально, если требуется)
                    // File.WriteAllText(_logFilePath, string.Empty);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during log maintenance.");
            }
        }
    }

}
