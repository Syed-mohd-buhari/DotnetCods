using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditHistory;
using CAM.DataTransferObjects.Entita.NetworkElement;
using CAM.DataTransferObjects.Entita.ProjectPlan;
using CAM.DataTransferObjects.FunctionalityDto;
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
    [Route("api/ProjectPlan")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class ProjectPlanController : CamControllerBase
    {
        private readonly ProjectsPlanManager _manager;
        private readonly IExportService _exportService;
        public ProjectPlanController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, ProjectsPlanManager manager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProjectPlan([FromQuery] long paId)
        {
            try
            {
                return await _manager.FindWithCondition(paId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Update([FromBody] ProjectPlanDto projectPlanDto)
        {
            try
            {
                return await _manager.UpdateProjectPlan(projectPlanDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("BulkUpdate")]
        public async Task<ResultDto> BulkUpdate()
        {
            try
            {
                return await _manager.BulkCreation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }




    }
}
