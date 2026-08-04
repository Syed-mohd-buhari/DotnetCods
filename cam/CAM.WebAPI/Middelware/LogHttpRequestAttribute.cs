using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Middelware
{
    public class LogHttpRequestAttribute : ActionFilterAttribute
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logger.Debug("action executing");

            base.OnActionExecuting(filterContext);
        }
    }
}
