using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using Serilog;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Http;
using LogLevel = NLog.LogLevel;

namespace CAM.WebAPI
{
    public class Program
    {
        static Logger logger;
        static IConfigurationRoot config;
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello to CAMS Application");
            logger = NLogBuilder.ConfigureNLog($"{Directory.GetCurrentDirectory()}/NLog.config").GetCurrentClassLogger();
            Console.WriteLine("CAMS Application Started");
            try
            {

                Console.WriteLine("CAMS Application InProgress");
                try
                {
                    CreateHostBuilder(args).Build().Run();
                }
                catch (Exception ex)
                {
                    logger.Fatal($"[FATAL][{DateTime.Now}][][][]" +
                   $"[][Request][]" +
                   $"[Kestrel services are down -  {ex.Message}]");
                    logger.Fatal($"[FATAL][{DateTime.Now}][][][] " +
                        $"[][Response][][Kestrel services are down -  {ex.Message}]");
                }
                config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();

            }
            catch (Exception e)
            {
                Console.WriteLine("CAMS Application Exception");
                Console.WriteLine($"{e.Message}");
                logger.Error(e, "stop to program because one exception");
                throw;
            }
            finally
            {
                Console.WriteLine("CAMS Application Shutdown");
                NLog.LogManager.Shutdown();
            }

        }

        protected Program() { }
        #region MyRegion
        //private static string GetCertificateName()
        //{
        //    var builder = new ConfigurationBuilder()
        //                        .SetBasePath(Directory.GetCurrentDirectory())
        //                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    string _certificateName = builder.Build().GetSection("CertificateName").Value;

        //    return _certificateName;
        //}

        //public static long ToInt(string addr)
        //{
        //    // careful of sign extension: convert to uint first;
        //    // unsigned NetworkToHostOrder ought to be provided.
        //    return (long)(uint)IPAddress.NetworkToHostOrder(
        //         (int)IPAddress.Parse(addr).Address);
        //}

        //public static string ToAddr(long address)
        //{
        //    return IPAddress.Parse(address.ToString()).ToString();
        //    // This also works:
        //    // return new IPAddress((uint) IPAddress.HostToNetworkOrder(
        //    //    (int) address)).ToString();
        //} 
        #endregion
        public static IHostBuilder CreateHostBuilder(string[] args) =>
                 Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 })
                 .ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                 });
    }
}