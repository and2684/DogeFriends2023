using DogeFriendsApi.Interfaces;
using NLog;
using ILogger = NLog.ILogger;

namespace DogeFriendsApi.Configuration
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message) => _logger.Debug(message);
        public void LogInfo(string message) => _logger.Info(message);
        public void LogWarn(string message) => _logger.Warn(message);
        public void LogError(Exception ex, string message) => _logger.Error(ex, message);
    }
}
