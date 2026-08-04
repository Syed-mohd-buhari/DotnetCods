using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponentFamily;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignComponentFamilyController : CamControllerBase
    {
        private readonly DesignComponentFamilyManager _designComponentFamilyManager;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly List<string> _verticalList;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck=false;
        private readonly List<long> _opcoList;
        public DesignComponentFamilyController(DesignComponentFamilyManager designComponentFamilyManager, 
            ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService
              ,AuthorizedRoleManager authorizedRoleManager
            , ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _designComponentFamilyManager = designComponentFamilyManager;
            _exportService = exportService;
            _authorizedRoleManager = authorizedRoleManager;
            _currentUserService = currentUserService;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt64(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToString(x)).Distinct().ToList() : null;

        }

        [HttpGet(template: "Create")]
        public DesignComponentFamilyDtoCreate GetCreateResourceDesignComponentFamily()
        {
            try
            {
                return _designComponentFamilyManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public DesignComponentFamilyDtoUpdate GetUpdateResourceDesignComponentFamily(long id)
        {
            try
            {
                return _designComponentFamilyManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Get")]
        public QueryResultDto<DesignComponentFamilyDtoGrid> GetDesignComponentFamily(
           [FromBody] DesignComponentFamilyQueryDto designComponentFamilyFilterDto)
        {
            try
            {
                if (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue == null ||
                               (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue != null
                             && designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Count == 0))
                    designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue = _verticalList;

                return _designComponentFamilyManager.FindWithCondition(designComponentFamilyFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] DesignComponentFamilyQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.SystemTypeIdBasedVerticalValue == null ||
      (dto.SystemTypeIdBasedVerticalValue != null
    && dto.SystemTypeIdBasedVerticalValue.Count == 0))

                    dto.SystemTypeIdBasedVerticalValue = _verticalList;

                var data = _designComponentFamilyManager.FindWithCondition(dto);
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()

                };

                foreach (var nome in data.Items)
                {
                    nome.SystemTypeIdentityName = nome.SystemTypeIdentityName?.Replace("<b class=\"text-lowercase\">", "");
                    nome.SystemTypeIdentityName = nome.SystemTypeIdentityName?.Replace("<b class=\"text-lowercase\" >", "");
                    nome.SystemTypeIdentityName = nome.SystemTypeIdentityName?.Replace("</b>", "");

                }

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets, "Design-Component-Family" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

                HttpContext.Response.ContentType = result.ContentType;
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                var fileContentResult = new FileContentResult(result.FileInByteArray, result.ContentType)
                {
                    FileDownloadName = result.FileName
                };


                return fileContentResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _designComponentFamilyManager.Delete(id);
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
                return await _designComponentFamilyManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            try
            {
                return await _designComponentFamilyManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetOpcoImplementation")]
        public ResultDto GetOpcoImplementation(int designComponentFamilyId)
        {
            try
            {
                return new ResultDto()
                {
                    Data = _designComponentFamilyManager.GetOpcosImplementationsFlags(designComponentFamilyId, _opcoList),
                    Warning = false,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpGet(template: "GetOpcoImplementationDetails")]
        public ResultDto GetOpcoImplementationDetails(int designComponentFamilyId)
        {
            try
            {
                return new ResultDto()
                {
                    Data = _designComponentFamilyManager.GetOpcosImplementationDetails(designComponentFamilyId),
                    Warning = false,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpGet(template: "GetDCS")]
        public ResultDto GetDCS(int designComponentFamilyId)
        {
            try
            {
                return new ResultDto()
                {
                    Data = _designComponentFamilyManager.GetDCSForDCF(designComponentFamilyId),
                    Warning = false,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto<long>> Create([FromBody] DesignComponentFamilyDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _designComponentFamilyManager.Add(dto, forced);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<long>() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(DesignComponentFamilyDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _designComponentFamilyManager.Update(dto, forced);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery]string propertyName, [FromQuery] string propertyFilter, [FromBody] DesignComponentFamilyQueryDto designComponentFamilyFilterDto)
        {
            try
            {
                if (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue == null ||
     (designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue != null
   && designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue.Count == 0))
                    designComponentFamilyFilterDto.SystemTypeIdBasedVerticalValue = _verticalList;

                var data = _designComponentFamilyManager.GetFilter(propertyName, propertyFilter, designComponentFamilyFilterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "LinkSubnetworkWithSupportedService")]
        public async Task<ResultDto> LinkSubnetworkWithSupportedService(int designComponentFamilyId,int? supportedServiceId = null)
        {
            try
            {
                return await _designComponentFamilyManager.LinkSubnetworkWithSupportedService(designComponentFamilyId, supportedServiceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "LinkDCFWithNetworkFunction")]
        public async Task<ResultDto> LinkDCFWithNetworkFunction(int designComponentFamilyId, int? networkFunctionId = null)
        {
            try
            {
                return await _designComponentFamilyManager.LinkDCFWithNetworkFunction(designComponentFamilyId, networkFunctionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

    }
}
