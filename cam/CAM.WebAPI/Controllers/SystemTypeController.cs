using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Identity;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.Infrastucture.QueryResult;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public partial class SystemTypeController : CamControllerBase
    {

        private readonly SystemTypeManager _systemTypeManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly List<int> _verticalList;
        private readonly bool _adminRoleCheck=false;
        private readonly List<int> _userListBasedOnVerticalId;
        private readonly CommonManager _commonManager;
        public SystemTypeController(SystemTypeManager systemTypeManager, ICurrentUserService currentUserService, 
            IExportService exportService, IIdentityService identityService, ILoggerManager logger,
            IHttpContextAccessor contextAccessor, AuthorizedRoleManager authorizedRoleManager, CommonManager commonManager) : base(logger, contextAccessor)
        {
            _systemTypeManager = systemTypeManager;
            _currentUserService = currentUserService;
            _identityService = identityService;
            _exportService = exportService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToInt32(x)).Distinct().ToList() : null;

            _commonManager = commonManager;


        }

        [HttpGet(template: "Create")]
        public SystemTypeDtoCreate GetCreateResourceSystemType()
        {
            try
            {
                return _systemTypeManager.GetCreatePage(_verticalList);
                // return _systemTypeManager.GetCreatePage();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public SystemTypeDtoUpdate GetUpdateResourceSystemType(long id)
        {
            try
            {
                return _systemTypeManager.GetUpdatePage(id, _verticalList);
                // return _systemTypeManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "{id}")]
        public SystemTypeDto GetSingleSystemType(long id)
        {
            try
            {
                return _systemTypeManager.Get(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Get")]
        public QueryResultDto<SystemTypeDtoGrid> GetSystemType([FromBody] SystemTypeQueryDto SystemTypeFilterDto)
        {
            try
            {
                if(((SystemTypeFilterDto.HardWareVertical == null) || (SystemTypeFilterDto.HardWareVertical != null && SystemTypeFilterDto.HardWareVertical.Count == 0)) &&
                        ((SystemTypeFilterDto.SoftWareVertical == null) || (SystemTypeFilterDto.SoftWareVertical != null && SystemTypeFilterDto.SoftWareVertical.Count == 0)))
                {
                    if (SystemTypeFilterDto.VerticalResponsible != null && SystemTypeFilterDto.VerticalResponsible.Count() == 0)
                        SystemTypeFilterDto.VerticalResponsible = _verticalList;// _currentUserService.sessionVerticalList;
                }
                
                return _systemTypeManager.FindWithCondition(SystemTypeFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet("GetMajorSoftwareVodafoneName")]
        public IDictionary<int,string> GetMajorSoftwareVodafoneName(long id)
        {
            try
            {
                return _systemTypeManager.GetMajorSoftwareVodafoneName(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet("GetVfNameOfSwAppType")]
        public IDictionary<int, string> GetVfNameOfSwAppType(long swAppTypeId)
        {
            try
            {
                return _systemTypeManager.GetVfNameOfSwAppType(swAppTypeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] SystemTypeQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                // if (dto.VerticalResponsible != null && dto.VerticalResponsible.Count() == 0)
                //if (  dto.VerticalResponsible != null && dto.VerticalResponsible.Count() == 0) dto.VerticalResponsible = _verticalList ;

                if (((dto.HardWareVertical == null) || (dto.HardWareVertical != null && dto.HardWareVertical.Count == 0)) &&
                        ((dto.SoftWareVertical == null) || (dto.SoftWareVertical != null && dto.SoftWareVertical.Count == 0)))
                {
                    if (dto.VerticalResponsible != null && dto.VerticalResponsible.Count() == 0)
                        dto.VerticalResponsible = _verticalList;// _currentUserService.sessionVerticalList;
                }

                var data = _systemTypeManager.FindWithCondition(dto);
                foreach(var item in data.Items)
                {
                    item.EndOfMaintenance = item.EndOfMaintenanceValue;
                }
                var tabs = data.GridRender.Render.Where(x => x.Tab != "").GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });
                if (data.Items != null)
                {
                    foreach (var nome in data.Items)
                    {
                        nome.SystemSolution = nome.SystemSolution.Replace("<b class=\"text-lowercase\">", "");
                        nome.SystemSolution = nome.SystemSolution.Replace("</b>", "");
                        nome.AssetClass = nome.AssetClass.Replace("<b class=\"text-lowercase\">", "");
                        nome.AssetClass = nome.AssetClass.Replace("</b>", "");
                        //foreach (var mhb in nome.MajorHardwareBuild)
                        //{
                        //    mhb.Value = 
                        //}
                    }
                }


                List<ExportSheet> reportSheets = new List<ExportSheet>();
                foreach (var item in tabs)
                {
                    reportSheets.Add(item);
                }
                var result = _exportService.GetExcelFrom(reportSheets,
                    "System-Type" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);


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

        [HttpPost(template: "Export")]
        public FileContentResult GetSystemTypeExcel(
         [FromBody] SystemTypeQueryDto SystemTypeFilterDto)
        {
            try
            {


                // if (SystemTypeFilterDto.VerticalResponsible != null && SystemTypeFilterDto.VerticalResponsible.Count() == 0)
                //if (  SystemTypeFilterDto.VerticalResponsible != null && SystemTypeFilterDto.VerticalResponsible.Count() == 0) SystemTypeFilterDto.VerticalResponsible = _verticalList;


                ExportService exp = new ExportService();

                var res = _systemTypeManager.FindWithCondition(SystemTypeFilterDto);

                ExportSheet foglio1 = new ExportSheet()
                {
                    Data = res.Items.Cast<object>().ToList(),

                    TabName = "SystemType"
                };

                //ExportSheet foglio2 = new ExportSheet()
                //{
                //    Data = res.Items.Cast<object>().ToList(),

                //    TabName = "Software"
                //};

                List<ExportSheet> fogli = new List<ExportSheet>();
                fogli.Add(foglio1);
                //fogli.Add(foglio2);

                var result = exp.GetExcelFrom(fogli, "ExportSystemType" + DateTime.Now.ToShortDateString() + ".xlsx");

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
                return null;
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] SystemTypeDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.SystemTypeNameOem))
                {
                    _logger.LogError("SystemTypeNameOem must be a value");
                    return new ResultDto() { Info = "NameOem must be a value", Warning = true };
                }
                var result = await _systemTypeManager.Add(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(SystemTypeDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.SystemTypeNameOem))
                {
                    _logger.LogError("SystemTypeNameOem must be a value");
                    return new ResultDto() { Info = "Name Oem must be a value", Warning = true };
                }
                //if (dto.SubDomainSpocIds == null && !dto.SubDomainSpocIds.Any())
                //{
                //    _logger.LogError("SubDomainSpoc must be a value");
                //    return new ResultDto() { Info = "SubDomainSpoc must be a value", Warning = true };
                //}


                var result = await _systemTypeManager.Update(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
            finally
            {
               
            }
        }

        [HttpPut(template: "Restore")]
        public async Task<ResultDto> Restore(long id)
        {
            try
            {
                return await _systemTypeManager.Restore(id);
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
                var result = await _systemTypeManager.Delete(id);
                return result;
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
                var result = await _systemTypeManager.DeleteDeep(id);
                return result;
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
                return await _systemTypeManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] SystemTypeQueryDto SystemTypeFilterDto)
        {
            try
            {
                if (((SystemTypeFilterDto.HardWareVertical == null) || (SystemTypeFilterDto.HardWareVertical != null && SystemTypeFilterDto.HardWareVertical.Count == 0)) &&
                        ((SystemTypeFilterDto.SoftWareVertical == null) || (SystemTypeFilterDto.SoftWareVertical != null && SystemTypeFilterDto.SoftWareVertical.Count == 0)))
                {
                    if (SystemTypeFilterDto.VerticalResponsible != null && SystemTypeFilterDto.VerticalResponsible.Count() == 0)
                        SystemTypeFilterDto.VerticalResponsible = _verticalList;// _currentUserService.sessionVerticalList;
                }
                var data = _systemTypeManager.GetFilter(propertyName, propertyFilter, SystemTypeFilterDto,_adminRoleCheck,_verticalList);
                return data.OrderBy(x => x.Text).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "GetAllSubdomainAndVerticalResponsibles")]
        public Dictionary<string, string> GetAllSubdomainAndVerticalResponsibles(long majorHwId, long majorSwId)
        {
            try
            {
                Dictionary<string, string> SubdomainAndVerticalResponsible = new Dictionary<string, string>()
            {
                {"SubDomainResponsibles", _commonManager.GetSubdomainResponseForLibary(majorSwId,majorHwId) },
                {"VerticalResponsibles", _commonManager.GetVerticalResponseForLibary(majorSwId,majorHwId)},
                {"DesignContact", _commonManager.GetSubdomainSpocEmailForLibary(majorSwId,majorHwId)}

            };

                return SubdomainAndVerticalResponsible;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
    }

}


