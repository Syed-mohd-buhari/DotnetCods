using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
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
    public class MajorHardwareBuildAsIsController : CamControllerBase
    {
        private readonly MajorHardwareBuildAsIsManager _majorHwManager;
        public MajorHardwareBuildAsIsController(MajorHardwareBuildAsIsManager majorHwManager, ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _majorHwManager = majorHwManager;
        }
        [HttpPost("Get")]
        public async Task<ResultDto> GetMajorHardwareBuildAsis(MajorHardwareBuildAsIsQueryDto majorHardwareBuildAsIsQueryDto)
        {
            try
            {
                return await _majorHwManager.FindWithCondition(majorHardwareBuildAsIsQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpGet(template: "GetCreatePage")]
        public MajorHardwareBuildAsIsCreateDto GetCreatePage()
        {
            try
            {
                return _majorHwManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "GetUpdatePage")]
        public Task<MajorHardwareBuildAsIsCreateDto> GetUpdatePage(long id)
        {
            try
            {
                return _majorHwManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] MajorHardwareBuildAsIsQueryDto dto)
        {
            try
            {
                var data = _majorHwManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpPost(template: "Create")]
        public async  Task<ResultDto> Add(MajorHardwareBuildAsIsCreateDto dto)
        {
            try
            {
                var data = _majorHwManager.Add(dto);
                return await data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpPut(template: "Update")]
        public async Task<ResultDto> Update(MajorHardwareBuildAsIsCreateDto dto)
        {
            try
            {
                var data = _majorHwManager.Update(dto);
                return await data;
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
                return await _majorHwManager.GetRelatedRecords(id);
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
                return await _majorHwManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
