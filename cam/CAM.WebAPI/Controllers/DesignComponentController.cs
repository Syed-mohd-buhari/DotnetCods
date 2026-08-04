using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using System.Linq;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.DesignComponent;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public partial class DesignComponentController : CamControllerBase
    {
        private readonly DesignComponentManager _designComponentManager;
        private readonly IExportService _exportService;
        private readonly ILogger<DesignComponentController> _loggerN;
        private readonly ILoggerManager _logger;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly List<string> _verticalList;
        private readonly ICurrentUserService _currentUserService;
        public readonly bool _adminRoleCheck=false;

        public DesignComponentController(DesignComponentManager designComponentManager,
            ILogger<DesignComponentController> loggerN,ILoggerManager logger, IHttpContextAccessor contextAccessor, 
            IExportService exportService, AuthorizedRoleManager authorizedRoleManager,
           ICurrentUserService currentUserService) : base(logger,contextAccessor)
        {
            _designComponentManager = designComponentManager;
            _exportService = exportService;
            _loggerN = loggerN;
            _authorizedRoleManager = authorizedRoleManager;
            _currentUserService = currentUserService;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToString(x)).Distinct().ToList() : null;

        }

        [HttpGet(template: "Create")]
        public DesignComponentDtoCreate GetCreateResourceDesignComponent()
        {
            try
            {
                var dcCreatModel = _designComponentManager.GetCreatePage().Result;
                return dcCreatModel;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public async Task<DesignComponentDtoUpdate> GetUpdateResourceDesignComponent(long id)
        {
            try
            {
                return await _designComponentManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost("Get")]
        public QueryResultDto<DesignComponentDtoGrid> GetDesignComponent(
           [FromBody] DesignComponentQueryDto designComponentFilterDto)
        {
            try
            {
                if ((designComponentFilterDto.SystemTypeIdBasedVerticalValue == null) || (designComponentFilterDto.SystemTypeIdBasedVerticalValue != null && designComponentFilterDto.SystemTypeIdBasedVerticalValue.Count == 0))
                    designComponentFilterDto.SystemTypeIdBasedVerticalValue = _verticalList;

                return _designComponentManager.FindWithCondition(designComponentFilterDto);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("GetProductNamesByVodfoneName/{vfNameId}")]
        public async Task<ResultDto> GetProductNamesByVodfoneName(int vfNameId)
        {
            try
            {
                return await _designComponentManager.GetProductNamesByVodfoneName(vfNameId);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] DesignComponentQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize= 0;
                if ((dto.SystemTypeIdBasedVerticalValue == null) || (dto.SystemTypeIdBasedVerticalValue != null && dto.SystemTypeIdBasedVerticalValue.Count == 0))
                   dto.SystemTypeIdBasedVerticalValue = _verticalList;

                var data = _designComponentManager.FindWithCondition(dto);
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()

                };

                //foreach (var nome in data.Items)
                //{
                //    nome.SystemType = nome.SystemType.Replace("<b class=\"text-lowercase\">", "");
                //    nome.SystemType = nome.SystemType.Replace("</b>", "");
                //}

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Design-Component" + DateTime.Now.ToShortDateString() + ".xlsx",data.GridRender);

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

        [HttpDelete(template:"Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _designComponentManager.Delete(id);
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
                return await _designComponentManager.DeleteDeep(id);
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
                return await  _designComponentManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] DesignComponentDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _designComponentManager.Add(dto, forced);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex); 
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(DesignComponentDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _designComponentManager.Update(dto, forced);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut(template: "Restore")]
        public async Task<ResultDto> Restore(long id)
        {
            try
            {
                return await _designComponentManager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }



        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] DesignComponentQueryDto designComponentFilterDto)
        {
            try
            {
                if ((designComponentFilterDto.SystemTypeIdBasedVerticalValue == null) || (designComponentFilterDto.SystemTypeIdBasedVerticalValue != null && designComponentFilterDto.SystemTypeIdBasedVerticalValue.Count == 0))
                  designComponentFilterDto.SystemTypeIdBasedVerticalValue = _verticalList;
                var data = _designComponentManager.GetFilter(propertyName, propertyFilter, designComponentFilterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPut(template: "ApplyDataRemediation")]
        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation([FromBody] DataRemediationDto data)
        {
            try
            {
                return await _designComponentManager.ApplyDataRemediation(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetSubNetworkBoundaries")]
        public List<SystemAndSubNetworkBoundary> GetSubNetworkBoundaries(long systemTypeId)
        {
            try
            {
                return  _designComponentManager.GetServicesBySWAppName(systemTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetSubNetworkBoundariesDestructured")]
        public List<SystemAndSubNetworkBoundary> GetSubNetworkBoundaries(int vodafoneNameId)
        {
            try
            {
                return _designComponentManager.GetServicesByVfName(vodafoneNameId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "ImpactedAreasOfEditDc")]
        public async Task<ResultDto> ImpactedAreasOfEditDc(long designComponentId, int newSystemTypeId, int subnetworkBoundryId)
        {
            try
            {
                return await _designComponentManager.ImpactedAreasOfEditDc(designComponentId, newSystemTypeId, subnetworkBoundryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet("IsDCHasNfxiBuildConstruction")]
        public async Task<ResultDto> IsDCHasNfxiBuildConstruction(int designComponentId)
        {
            try
            {
                return await _designComponentManager.IsDCHasNfxiBuildConstruction(designComponentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }
}
