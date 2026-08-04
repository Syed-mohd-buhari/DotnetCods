using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace CAM.WebAPI.Middelware
{
    public class UserLoginSessionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] allowedURLS = new string [3]{ "/api/User", "/swagger", "/api/AuthCheck" };
        public UserLoginSessionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var exist = false;
            foreach (string item in allowedURLS)
            {
                if (httpContext.Request.Path.ToString().Contains(item))
                {
                    exist = true;
                    break;
                }
            }
             if (!exist && httpContext.Request.Path.ToString() !="/")
            {
                if(httpContext.Session != null && httpContext.Session.GetString("AdventureWorks.Session.AppSession") == null)
                {
                    throw new AuthenticationException();
                }
            }
            await _next(httpContext);
        }
    }
}
