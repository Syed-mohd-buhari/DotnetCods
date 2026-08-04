using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.WebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    public abstract class CamControllerBase : ControllerBase
    {
        protected readonly ILoggerManager _logger;
        protected readonly IHttpContextAccessor _contextAccessor;
        protected readonly IUserLoggingLevelManager _userLoggingLevelManager;
        private string currentLogLevel = "" ;
        protected CultureInfo BrowserCulture { get; }

        protected CamControllerBase(ILoggerManager logger, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            
            try
            {
                BrowserCulture = _contextAccessor.GetUserCulture();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
            finally
            {
                //_logger.LogTraceEnd();
            }
        }


    }
}
