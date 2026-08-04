using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.VNFCluster;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
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
    public class VnfClusterNameController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly VnfClusterNameManager _vnfClusterNameManager;
        private readonly ILoggerManager _loggerManager;
        public VnfClusterNameController(ILoggerManager logger, IHttpContextAccessor contextAccessor, VnfClusterNameManager vnfClusterNameManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _vnfClusterNameManager = vnfClusterNameManager;
            _loggerManager = logger;
        }

        [HttpPost("Get")]
        public async Task<ResultDto> GetProductNames([FromBody] VnfClusterNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                return await _vnfClusterNameManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] VnfClusterNameQueryDto dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _vnfClusterNameManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetUpdatedPage")]
        public async Task<VnfCluserNameDtoGrid> GetUpdatedPage(int id)
        {
            try
            {
                return await _vnfClusterNameManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetCreatepage")]
        public VnfCluserNameDtoGrid GetCreatePage()
        {
            try
            {
                return _vnfClusterNameManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<ResultDto> Create([FromBody] VnfClusterNameCreateDto dto)
        {
            try
            {
                return await _vnfClusterNameManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut("Update")]
        public async Task<ResultDto> Put(VnfCluserNameUpdateDto dto)
        {
            try
            {
                return await _vnfClusterNameManager.Update(dto);
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
                return await _vnfClusterNameManager.Delete(id);
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
                return await _vnfClusterNameManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
