using CAM.Contracts;
using NLog;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CAM.LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Debug(Message(message, method, path));
        }
        public void LogError(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Error(Message(message, method, path));
        }

        public void LogError(Exception ex, [CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            var exceptionErrorMessage = $"Message: {ex.Message} - StackTrace: {ex.StackTrace}";
            logger.Error(Message(exceptionErrorMessage, method, path));
        }

        public void LogInfo(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Info(Message(message, method, path));
        }

        public void LogTrace(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Trace(Message(message, method, path));
        }

        public void LogTraceEnd([CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Trace(Message("END", method, path));
        }

        public void LogTraceStart([CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Trace(Message("START", method, path));
        }

        public void LogWarn(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Warn(Message(message, method, path));
        }

        public void LogFatal(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "")
        {
            logger.Fatal(Message(message, method, path));
        }

        private string Message(string message, string method, string path)
        {
            string className = System.IO.Path.GetFileNameWithoutExtension(path).Split('\\', StringSplitOptions.RemoveEmptyEntries).Last();
            return $"Class: {className} - Method: {method} - Message: {message}";
        }
    }
}
