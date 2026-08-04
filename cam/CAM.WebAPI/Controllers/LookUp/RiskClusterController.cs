using CAM.BusinessManager.ILookUp;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.RiskCluster;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
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
    public class RiskClusterController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;
        private readonly RiskClusterManager _riskClusterManager;
        public RiskClusterController(ILoggerManager logger, IHttpContextAccessor contextAccessor, RiskClusterManager riskClusterManager , ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _riskClusterManager = riskClusterManager;
        }

        [HttpPost("Get")]
        public Task<QueryResultDto<RiskClusterDtoGrid>> GetProductNames([FromBody] RiskClusterQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return _riskClusterManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] RiskClusterQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _riskClusterManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "GetUpdatedPage")]
        public RiskClusterDtoGrid GetUpdatedPage(int id)
        {
            try
            {
                return _riskClusterManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public RiskClusterDtoGrid GetCreatePage()
        {
            try
            {
                return _riskClusterManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] RiskClusterCreateDto dto)
        {
            try
            {
                return await _riskClusterManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(RiskClusterUpdateDto dto)
        {
            try
            {
                return await _riskClusterManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(int id)
        {          
            try
            {
                return await _riskClusterManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(int id)
        {           

            try
            {
                return await _riskClusterManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
