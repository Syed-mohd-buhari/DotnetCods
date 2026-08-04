using Azure;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace CAM.WebAPI.Helper
{
    public static class Javascript
    {
        static string scriptTag = "<script type=\"\" language=\"\">{0}</script>";
            
        public static void ConsoleLog(string message, HttpContext context)
        {
            string function = "console.log('{0}');";
            string log = string.Format(GenerateCodeFromFunction(function), message);
        
            context.Response.WriteHtmlAsync(log);
        }

        public static void Alert(string message, HttpContext context)
        {
            string function = "alert('{0}');";
            string log = string.Format(GenerateCodeFromFunction(function), message);
            context.Response.WriteHtmlAsync(log);
        }

        static string GenerateCodeFromFunction(string function)
        {
            return string.Format(scriptTag, function);
        }
    }
}
