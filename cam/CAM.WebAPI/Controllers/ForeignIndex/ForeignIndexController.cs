using CAM.BusinessManager.ForeignIndex;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.ForeignIndex;
using CAM.Exports;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.ForeignIndex
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class ForeignIndexController : CamControllerBase
    {
        private readonly ForeignIndexManager _foreignIndexManager;
        private readonly IExportService _exportService;

        public ForeignIndexController(ForeignIndexManager foreignIndexManager, ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {
            _foreignIndexManager = foreignIndexManager;
            _exportService = exportService;
        }

        [HttpGet(template: "GetOrphanDesignComponents")]
        public async Task<ResultDto<ForeignIndexDto>> GetOrphanDesignComponents([FromQuery] long? id)
        {
            try
            {
                return await _foreignIndexManager.GetOrphanDesignComponents(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<ForeignIndexDto>() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "GetOrphanSystemTypes")]
        public async Task<ResultDto<ForeignIndexDto>> GetOrphanSystemTypes([FromBody] ForeignIndexDto foreignIndexDto)
        {
            try
            {
                return await _foreignIndexManager.GetOrphanSystemTypes(foreignIndexDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<ForeignIndexDto>() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "GetGroups")]
        public async Task<ResultDto<ForeignIndexDto>> GetGroups([FromBody] ForeignIndexDto foreignIndexDto)
        {
            try
            {
                return await _foreignIndexManager.GetGroups(foreignIndexDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<ForeignIndexDto>() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpPost(template: "ImpactCheck")]
        public async Task<ResultDto<ForeignIndexDto>> ImpactCheck([FromBody] ForeignIndexDto foreignIndexDto)
        {
            try
            {
                return await _foreignIndexManager.ImpactCheck(foreignIndexDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<ForeignIndexDto>() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Apply")]
        public async Task<ResultDto<ForeignIndexDto>> Apply([FromBody] ForeignIndexDto foreignIndexDto)
        {
            try
            {
                return await _foreignIndexManager.Apply(foreignIndexDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<ForeignIndexDto>() { Info = ResultMessages.SystemError, Warning = true };
            }
        }



        [HttpPost(template: "sessionTest")]
        public async Task sessionModel([FromBody] FI_SessionDto foreignIndexDto)
        {
            
        }

        [HttpPost(template: "CancelForeignIndex")]
        public async Task<ResultDto> CancelForeignIndex([FromBody] ForeignIndexDto foreignIndexDto)
        {
            try
            {
                return await _foreignIndexManager.CancelForeignIndex(foreignIndexDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ResetForeignIndex")]
        public async Task<ResultDto> ResetForeignIndex()
        {
            try
            {
                return await _foreignIndexManager.ResetForeignIndex();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

    }
}
