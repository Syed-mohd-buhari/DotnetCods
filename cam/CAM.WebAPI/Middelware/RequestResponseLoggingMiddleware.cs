using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Repository.Helpers;
using CAM.WebAPI.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using NLog.Web;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CAM.WebAPI.Middelware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NLog.Logger logger;
        private readonly ILoggerManager _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        private string _RequestPath = string.Empty;
        private string _Message = string.Empty;
        private string requestBodyAsText = string.Empty;
        private string _Method = string.Empty;
        private MemoryStream requestStream;
        private string _UserName = string.Empty;
        private string _UserRole = string.Empty;
        private string _SessionID = string.Empty;
        private string _IPAddress = string.Empty;
        public RequestResponseLoggingMiddleware(IHttpContextAccessor httpContextAccessor, RequestDelegate next, ILoggerManager logger_)
        {
            _next = next;
            logger = NLogBuilder.ConfigureNLog($"{Directory.GetCurrentDirectory()}/NLog.config").GetCurrentClassLogger();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _logger = logger_;
        }
        public async Task InvokeAsync(HttpContext context, IHttpContextAccessor httpContextAccessor, IServiceProvider _serviceProvider, ICurrentUserService currentUserService)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                CurrentLogLevelConfig._UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue("Email") ?? "";
                CurrentLogLevelConfig._UserRole = httpContextAccessor.HttpContext?.User?.FindFirstValue("Role") ?? "";
                CurrentLogLevelConfig._SessionID = httpContextAccessor.HttpContext.Session != null ? httpContextAccessor.HttpContext.Session.Id : "";

                var _IPRemoteAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null ? httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() : "";
                var ipForward = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
                CurrentLogLevelConfig._IPAddress = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString().Split(new
                char[] { ',' }).FirstOrDefault() + ", " + _IPRemoteAddress + ',' + ipForward;

                _RequestPath = context.Request.Path.Value != null ? (context.Request.Path.Value.Any(char.IsDigit) ? CustomizeHttpRequestPath.CustomizeRequestPath(context.Request.Path.Value) : context.Request.Path.Value) : "";


                _Method = context.Request.Method;

                var _UserLoggingLevelService = scope.ServiceProvider.GetRequiredService<IUserLoggingLevelManager>();
                try
                {
                    CurrentLogLevelConfig._CurrentLogLevel = await _UserLoggingLevelService.GetCurrentLogLevel();
                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && _RequestPath != "/" && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.LogFatal($"[FATAL][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                       $"[{CurrentLogLevelConfig._IPAddress}][ {_Method} Request][{_RequestPath}]" +
                       $"[Database server is down  - {ex.Message}]");
                        _logger.LogFatal($"[FATAL][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}] " +
                            $"[{CurrentLogLevelConfig._IPAddress}][ {_Method} Response][{_RequestPath}][Database server is down  -  {ex.Message}]");
                    }
                }
            }

            context.Request.EnableBuffering();
            using (var reader = new StreamReader(
                context.Request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024 * 100,
                leaveOpen: true))
            {
                _UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue("Email") ?? "";
                await using var requestStream = _recyclableMemoryStreamManager.GetStream();
                await context.Request.Body.CopyToAsync(requestStream);
                requestBodyAsText = ReadStreamInChunks(requestStream);
                context.Request.Body.Position = 0;  //rewinding the stream to 0
            }
            await _next.Invoke(context);

            _RequestPath = context.Request.Path.Value != null ? (context.Request.Path.Value.Any(char.IsDigit) ? CustomizeHttpRequestPath.CustomizeRequestPath(context.Request.Path.Value) : context.Request.Path.Value) : "";

            #region Lunk value changes xxx
            string[] value = _RequestPath.Split("/");
            if (value != null)
            {
                if (value.Contains("ImportReport"))
                {
                    requestBodyAsText = value[2] + " is imported";
                }
            }
            #endregion  
            if (!string.IsNullOrEmpty(CurrentLogLevelConfig._CurrentLogLevel) && _RequestPath != "/" && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
            {
                await LogRequest(context, httpContextAccessor, _serviceProvider, currentUserService, requestBodyAsText);
                await LogResponse(context, requestBodyAsText);
            }

        }
        private async Task LogRequest(HttpContext context, IHttpContextAccessor httpContextAccessor, IServiceProvider _serviceProvider,
            ICurrentUserService currentUserService, string requestBodyText)
        {
            try
            {
                context.Request.EnableBuffering();
                GlobalDbMode.CheckedDbMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";

                _RequestPath = context.Request.Path.Value != null ? (context.Request.Path.Value.Any(char.IsDigit) ? CustomizeHttpRequestPath.CustomizeRequestPath(context.Request.Path.Value) : context.Request.Path.Value) : "";

                _Method = context.Request.Method;
                _Message = context.Response.StatusCode == 200 ? "success" : "fail";
                requestStream = _recyclableMemoryStreamManager.GetStream();

                if (CurrentLogLevelConfig._CurrentLogLevel.Contains("Trace"))
                {

                    logger.Trace($"Enter {_RequestPath} [TRACE][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                        $"[{CurrentLogLevelConfig._IPAddress}][ {_Method} Request][{_RequestPath}]" +
                        $"[{_Message}]  Exit {_RequestPath}");
                    logger.Trace($"Enter {_RequestPath} [TRACE][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}] " +
                        $"[{CurrentLogLevelConfig._IPAddress}][ {_Method} Response][{_RequestPath}][{_Message}]  Exit {_RequestPath}");
                }

                if (CurrentLogLevelConfig._CurrentLogLevel.Contains("Debug") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Trace"))
                {
                    logger.Debug($"[DEBUG][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                        $"[{CurrentLogLevelConfig._IPAddress}][{_Method} Request][{_RequestPath}][{requestBodyText}]" +
                        $"[{_Message}]");
                    logger.Debug($"[DEBUG][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}] " +
                        $"[{CurrentLogLevelConfig._IPAddress}][{_Method} Response][{_RequestPath}][{_Message}]");
                }

                if (CurrentLogLevelConfig._CurrentLogLevel.Contains("Info") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Debug") || CurrentLogLevelConfig._CurrentLogLevel.Contains("Trace"))
                {
                    string _RequestPathNoParams = new String(_RequestPath.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());
                    logger.Info($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                    $"[{CurrentLogLevelConfig._IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                    $"[{_Message}]");
                    logger.Info($"[INFO][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                        $"[{CurrentLogLevelConfig._IPAddress}][{_Method} Response][{_RequestPath}][{_Message}]");
                }

                if (_Message == "fail")
                {
                    logger.Error($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                        $"[{CurrentLogLevelConfig._IPAddress}][ {_Method} Request][{_RequestPath}]" +
                        $"[{_Message}]");
                    logger.Error($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}] " +
                        $"[{CurrentLogLevelConfig._IPAddress}][ {_Method} Response][{_RequestPath}][{_Message}]");
                }

                context.Request.Body.Position = 0;
            }
            catch (Exception ex)
            {
                if (_RequestPath != "/" && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off") && !_RequestPath.Contains("RefreshToken", StringComparison.OrdinalIgnoreCase))
                {
                    string _RequestPathNoParams = new String(_RequestPath.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());

                    logger.Error($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                    $"[{CurrentLogLevelConfig._IPAddress}][{_Method} Request][{_RequestPathNoParams}]" +
                    $"[{ex.Message}]");
                    logger.Error($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{CurrentLogLevelConfig._UserName}][{CurrentLogLevelConfig._UserRole}][{CurrentLogLevelConfig._SessionID}]" +
                    $"[{CurrentLogLevelConfig._IPAddress}][{_Method} Response][{_RequestPathNoParams}]" +
                    $"[Failed]");

                }
            }
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);

                textWriter.Write(readChunk, 0, readChunkLength);

            } while (readChunkLength > 0);
            return textWriter.ToString();
        }
        private async Task LogResponse(HttpContext context, string requestBodyText)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            // await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
