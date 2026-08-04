using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.PlannedActivities;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignAspects;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.DaPlannedActivityDcf;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignAspectPlannedActivityController : CamControllerBase
    {
        private readonly DesignAspectPlannedActivityManger _designAspectPlannedActivityManager;


        public DesignAspectPlannedActivityController(DesignAspectPlannedActivityManger designAspectPlannedActivityManager, ILoggerManager logger,
            IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _designAspectPlannedActivityManager = designAspectPlannedActivityManager;
        }


        [HttpPost(template: "GetDcfAssociatedEntities")]
        public async Task<ResultDto> GetDcfAssociatedEntities([FromBody]DesignAspectPlannedActivityDto dto)
        {
            try
            {
                return await _designAspectPlannedActivityManager.GetDcfAssociatedEntities(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "GetMultipleDcfEntities")]
        public async Task<ResultDto> GetMultipleDcfEntities([FromBody] PlannedActivityDtoUpdate dto)
        {
            try
            {
                return await _designAspectPlannedActivityManager.GetMultipleDcfEntities(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

    }
}
