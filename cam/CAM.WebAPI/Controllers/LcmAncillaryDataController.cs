using Microsoft.AspNetCore.Authorization;
using CAM.Contracts;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using CAM.DataTransferObjects;
using System.Threading.Tasks;
using CAM.Infrastucture;
using CAM.DataTransferObjects.Entita.LcmAncillaryData;
using CAM.Entities.Models;
using CAM.BusinessManager.Entity;

namespace CAM.WebAPI.Controllers
{
    [Route("api/LcmAncillaryData")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class LcmAncillaryDataController : CamControllerBase 
    {
        private readonly LcmAncillaryDataManager _lcmAncillaryDataManager;
        private readonly IExportService _exportService;
        public LcmAncillaryDataController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, LcmAncillaryDataManager lcmAncillaryDataManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _lcmAncillaryDataManager = lcmAncillaryDataManager;
        }

        [HttpPost("Get")]

        public QueryResultDto<LcmAncillaryDataDtoGrid> GetLcmAncillaryData([FromBody] LcmAncillaryDataQueryDto lcmAncillaryDataDto)
        {
            try
            {
                return _lcmAncillaryDataManager.FindWithCondition(lcmAncillaryDataDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        #region //CURD
        [HttpPost("Create")]
        public async Task<ResultDto> CreateLcmAncillaryData([FromBody] LcmAncillaryDataCRUDDto dto)
        {
            try
            {
                return await _lcmAncillaryDataManager.CreateLcmAncillaryData(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut("Update")]
        public async Task<ResultDto> UpdateLcmAuditAttribute([FromBody] LcmAncillaryDataCRUDDto dto)
        {
            try
            {
                return await _lcmAncillaryDataManager.UpdateLcmAncillaryData(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _lcmAncillaryDataManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                return await _lcmAncillaryDataManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        #endregion
    }
}
