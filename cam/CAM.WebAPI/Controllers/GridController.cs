using Microsoft.AspNetCore.Mvc;
using System;
using CAM.BusinessManager.Grid;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Grid;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class GridController : CamControllerBase
    {
        private GridCustomColumnManager _manager;
        public GridController(ILoggerManager logger, IHttpContextAccessor contextAccessor, GridCustomColumnManager manager) : base(logger, contextAccessor)
        {
            this._manager = manager;
        }
        [HttpPost]
        public ResultDto Save([FromBody]SaveGrid saveGrid)
        {
            try
            {
                _manager.SaveMapping(saveGrid.ClassName, saveGrid.Render,saveGrid.UserPrefrenceDetails,saveGrid.WhatsGoingOnDate, saveGrid.MessagingDate );
                return new ResultDto()
                {
                    Warning = false,
                    Info = ResultMessages.ConfigurationGridSuccess
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpDelete]
        public ResultDto Delete(string className)
        {
            try
            {
                _manager.DeleteMapping(className);
                return new ResultDto()
                {
                    Warning = false,
                    Info = ResultMessages.ConfigurationGridSuccess
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
            
        }

    }
}
