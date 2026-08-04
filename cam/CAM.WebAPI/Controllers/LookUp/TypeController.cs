using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.SoftwareApplicationTypes;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CAM.DataTransferObjects.LookUp.VodafoneName;
using System.Linq;
using CAM.BusinessManager.LookUp;
using CAM.DataTransferObjects.LookUp.SubNetworkBoundary;
using CAM.DataTransferObjects.LookUp.Type;
using CAM.BusinessManager.Entity;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : CamControllerBase
    {
        private readonly TypeManager _typeManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;

        public TypeController(TypeManager typeManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService, IExportService exportService) : base(logger, contextAccessor)
        {
            _typeManager = typeManager;
            _currentUserService = currentUserService;
            _exportService = exportService; 
        }

        [HttpGet]
        public QueryResultDto<TypeDtoGrid> GetTypesAsync([FromQuery] TypeDtoQuery dto)
        {
            try
            {
                return _typeManager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] TypeDtoQuery dto)
        {
            try
            {
                /*_currentUserService.UserInRole("Admin");*/
                var data = _typeManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public TypeDtoUpdate GetUpdateResourceType(short id)
        {
            try
            {
                return _typeManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("Create")]
        public TypeDtoCreate GetCreate()
        {
            try
            {
                return _typeManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }


        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _typeManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] TypeDtoCreate dto)
        {
            try
            {
                return await _typeManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        
        [HttpPut]
        public async Task<ResultDto> Put(TypeDtoUpdate TypeDtoUpdate)
        {
            try
            {
                return await _typeManager.Update(TypeDtoUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            /*_currentUserService.UserInRole("Admin");*/
            try
            {
                return await _typeManager.Delete(id);
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
            /*_currentUserService.UserInRole("Admin");*/

            try
            {
                return await _typeManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet("GetTypesByClassId")]
        public ResultDto GetTypesByClassId(int classId)
        {
            try
            {
                return _typeManager.GetTypesByClassId(classId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
