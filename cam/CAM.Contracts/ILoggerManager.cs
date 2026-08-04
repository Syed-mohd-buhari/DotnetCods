using System;
using System.Runtime.CompilerServices;

namespace CAM.Contracts
{
    public interface ILoggerManager
    {
        void LogInfo(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogTraceStart([CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogTraceEnd([CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogTrace(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogWarn(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogFatal(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogDebug(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogError(string message, [CallerMemberName] string method = "", [CallerFilePath] string path = "");
        void LogError(Exception ex, [CallerMemberName] string method = "", [CallerFilePath] string path = "");

    }
}
