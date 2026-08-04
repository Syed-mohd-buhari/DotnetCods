using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]

    public class DeploymentStatusController : CamControllerBase
    {
        private readonly DeploymentStatusManager _deploymentStatusManager;
        private readonly ICurrentUserService _currentUserService;

        public DeploymentStatusController(DeploymentStatusManager deploymentStatusManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _deploymentStatusManager = deploymentStatusManager;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public Task<QueryResultDto<DeploymentStatusDtoGrid>> GetDeploymentStatus([FromQuery] DeploymentStatusQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _deploymentStatusManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] DeploymentStatusQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                var data = _deploymentStatusManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] DeploymentStatusDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return await _deploymentStatusManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(DeploymentStatusDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return await _deploymentStatusManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpDelete(template:"Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _deploymentStatusManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _deploymentStatusManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _deploymentStatusManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public DeploymentStatusDto GetCreateResourceDeploymentStatus()
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _deploymentStatusManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public DeploymentStatusDto GetUpdateResourceDeploymentStatus(short id)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/

                return _deploymentStatusManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
