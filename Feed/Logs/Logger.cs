using TEMS.Entity;

namespace TEMS.Logs
{

    public class Logger
    {
        public string separator;

        public Logger()
        {
            if (OperatingSystem.IsLinux())
            {
                separator = "/";
            }
            else
            {
                separator = "\\";
            }
        }
        public enum TEMLog
        {
            Info,
            Warn,
            Error,
            Debug,
            DBMappingHelp,
            DBTableHelp
        }
        public static void WriteLog(TEMLog type, string module_name, string message)
        {
            Logger _logger = new Logger();
            string SFTP_path = CommonFunction.getAppConfigValue("SFTP_path");
            string logFolder = CommonFunction.getAppConfigValue("logFolder");
            string logFileName = SFTP_path  + logFolder + _logger.separator + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string errorFileName = SFTP_path + logFolder + _logger.separator + DateTime.Now.ToString("yyyyMMdd") + ".error";
            string dbMapFileName = SFTP_path +logFolder + _logger.separator + DateTime.Now.ToString("yyyyMMdd") + ".dbmaphelp";
            string dbTableFileName = SFTP_path + logFolder +_logger.separator + DateTime.Now.ToString("yyyyMMdd") + ".dbtablehelp";
            string? DisplayDebug = CommonFunction.getAppConfigValue("DisplayDebug");
            if (type.Equals(TEMLog.Info) || type.Equals(TEMLog.Error))
            {
                Console.WriteLine($" {DateTime.Now} : [{type}] : [{module_name}] : {message}");
            }
            else if (DisplayDebug == "true" && TEMLog.Debug == type)
            {
                Console.WriteLine($" {DateTime.Now} : [{type}] : [{module_name}] : {message}");
            }
            if (DisplayDebug != "true" && TEMLog.Debug == type)
            {
                return;
            }
            try
            {
                if (!Directory.Exists(SFTP_path + _logger.separator + logFolder))
                {
                    Directory.CreateDirectory(SFTP_path + _logger.separator + logFolder);
                }
                if (type.Equals(TEMLog.Info))
                {
                    using (StreamWriter writer = new StreamWriter(logFileName, true))
                    {
                        writer.WriteLine($" {DateTime.Now} : [{type}] : [{module_name}] : {message}");
                    }
                }
                else if (type.Equals(TEMLog.Error) || type.Equals(TEMLog.Warn))
                {
                    using (StreamWriter writer = new StreamWriter(errorFileName, true))
                    {
                        writer.WriteLine($" {DateTime.Now} : [{type}] : [{module_name}] : {message}");
                    }
                }
                else if (type.Equals(TEMLog.DBMappingHelp))
                {
                    using (StreamWriter writer = new StreamWriter(dbMapFileName, true))
                    {
                        writer.WriteLine($"{message}");
                    }
                }
                else if (type.Equals(TEMLog.DBTableHelp))
                {
                    using (StreamWriter writer = new StreamWriter(dbTableFileName, true))
                    {
                        writer.WriteLine($"{message}");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
