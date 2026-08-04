using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using DocumentFormat.OpenXml.Spreadsheet;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static IdentityServer4.Models.IdentityResources;

namespace CAM.WebAPI.Middelware
{
    public class CAMErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private readonly IWebHostEnvironment _enviroment;
        private string _UserName = string.Empty;
        
        private string currentLogLevel = string.Empty;
        private string _UserRole = string.Empty;
        private string _SessionID = string.Empty;
        private string _IPAddress = string.Empty;
        private string _RequestPath = string.Empty;
        private string _Method = string.Empty;
        public CAMErrorHandlingMiddleware(RequestDelegate next, ILoggerManager logger, IWebHostEnvironment enviroment)
        {
            _next = next;
            _logger = logger;
            _enviroment = enviroment;
        }

        public async Task Invoke(HttpContext context, IHttpContextAccessor httpContextAccessor, IServiceProvider _serviceProvider)
        {
            try
            {
                await _next(context);
            }
            catch (AuthenticationException ex)
            {
                if (!CurrentLogLevelConfig._CurrentLogLevel.IsNullOrEmpty() && CurrentLogLevelConfig._CurrentLogLevel.Contains("Error") && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off"))
                {
                    _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{_UserName}][{_UserRole}][{_SessionID}]" +
                    $"[{_IPAddress}][{ex.Message}]");
                }
                await HandleException(context, ex, _enviroment);
            }
            catch (Exception ex)
            {
                if (!CurrentLogLevelConfig._CurrentLogLevel.IsNullOrEmpty() && CurrentLogLevelConfig._CurrentLogLevel.Contains("Error") && !CurrentLogLevelConfig._CurrentLogLevel.Contains("Off"))
                {
                    _logger.LogError($"[ERROR][{DateTime.Now}][{GlobalDbMode.CheckedDbMode}][{_UserName}][{_UserRole}][{_SessionID}]" +
                    $"[{_IPAddress}][{ex.Message}]");
                }
                await HandleException(context, ex, _enviroment);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex, IWebHostEnvironment env)
        {
            
            var message = ex.InnerException == null
                ? string.Join("{0}{1}", "Message = "+ ex.Message, "Stack Trace = " +  ex.StackTrace)
                : string.Join("{0}{1}{2}", "Message = " +  ex.Message , "Inner Exception= " + ex.InnerException.Message, "Stack Trace = " + ex.StackTrace);
            var resultDto = new ResultDto
            {
                Warning = true,
                Info = message,
            };
            if (env.IsDevelopment())
            {
                resultDto.Data = ex.GetBaseException().Message;
            }
            context.Response.ContentType = "application/json";
            if (ex is AuthenticationException)
            {
                resultDto.Info = ResultMessages.AuthError;
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }
            }
            else
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string result = JsonConvert.SerializeObject(resultDto, serializerSettings);
            return context.Response.WriteAsync(result);
        }
    }
}
