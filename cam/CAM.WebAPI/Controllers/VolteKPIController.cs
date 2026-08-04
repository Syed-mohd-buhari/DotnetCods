using CAM.BusinessManager.Entity;
using CAM.BusinessManager.ExtensionMethod.VolteKPI;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.VolteKPI;
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
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif 

    public class VolteKPIController : CamControllerBase
    {

        private readonly VolteKPIManager _manager;
        private readonly IExportService _exportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly bool _isKpiUser = false;

        public VolteKPIController(AuthorizedRoleManager authorizedRoleManager,ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, VolteKPIManager manager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _isKpiUser = _roleOpcoList != null ? _roleOpcoList.IsKpiUser : _isKpiUser;
        }

        [HttpPost(template: "KPIGet")]
        public VolteKPIDtoCreate GetKPI([FromBody] VolteKPIQueryDto volteKPIFilterDto)
        {
            try
            {
                //_currentUserService.UserInRole(true, "KPI Administrator", "Admin", "KPI Editor", "READONLY");
                if(_isKpiUser)
                return _manager.GetCreatePage(volteKPIFilterDto);
                else
                {
                    return new VolteKPIDtoCreate();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "KPI")]
        public async Task<ResultDto> SaveOrEdit([FromBody] VolteKPIDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                if (_isKpiUser)
                {
                    //_currentUserService.UserInRole(true, "KPI Administrator", "Admin", "KPI Editor");
                    Microsoft.Extensions.Primitives.StringValues strValues;
                    var url = "";
                    if (HttpContext.Request.Headers.TryGetValue("Origin", out strValues))
                    {
                        url = strValues[0];
                    }
                    else
                    {
                        url = HttpContext.Request.Host.Value;
                    }
                    return await _manager.Add(dto, url, forced);
                }
                else
                    return new ResultDto();
                    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        //[HttpGet(template: "Approvals")]
        //public QueryResultDto<VolteKPITargetApprovalDto> GetApprovals([FromQuery] VolteKPIQueryDto volteKPIFilterDto)
        //{
        //    _currentUserService.UserInRole(true, "KPI Administrator", "Admin");
        //    return _manager.GetApprovals(volteKPIFilterDto);
        //}
        //[HttpPut(template: "Approvals")]
        //public async Task<ResultDto> PutApproval(VolteKPITargetApprovalDto dto)
        //{
        //    _currentUserService.UserInRole(true, "KPI Administrator", "Admin");
        //    return await _manager.UpdateApproval(dto);
        //}

        [HttpPost(template: "GetDashboard")]
        public VolteKPIDashboardDto GetDashboard([FromBody] VolteKPIQueryDto volteKPIFilterDto)
        {
            try
            {
                //_currentUserService.UserInRole(true, "KPI Administrator", "Admin", "KPI Editor", "READONLY");
                if (_isKpiUser)
                    return _manager.GetDashboard(volteKPIFilterDto);
                else
                    return new VolteKPIDashboardDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "GetDashboardReports")]
        public VolteKPIReportDto GetDashboardReports([FromBody] VolteKPIQueryDto volteKPIFilterDto)
        {
            try
            {
                //_currentUserService.UserInRole(true, "KPI Administrator", "Admin", "KPI Editor");
                if (_isKpiUser)
                    return _manager.GetDashboardReports(volteKPIFilterDto);
                else return new VolteKPIReportDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromQuery] VolteKPIQueryDto dto)
        {
            try
            {
                if (_isKpiUser)
                {
                    //_currentUserService.UserInRole(true, "KPI Administrator", "Admin");
                    dto.Page = 0;
                    dto.PageSize = 0;

                    VolteKPIReportDto data = _manager.GetDashboardReports(dto);
                    List<ExportSheetCustom> tabs = new List<ExportSheetCustom>();
                    tabs.Add(new ExportSheetCustom
                    {
                        TabName = "PROVISIONED",
                        CustomHeaders = data.Provisioned.GetHeaders(data.Year),
                        Data = data.Provisioned.GetData()
                    });
                    tabs.Add(new ExportSheetCustom
                    {
                        TabName = "REGISTERED",
                        CustomHeaders = data.Registered.GetHeaders(data.Year),
                        Data = data.Registered.GetData()
                    });

                    var result = _exportService.GetExcelFrom(tabs, "VoLTE-KPIs-Dashboard" + DateTime.Now.ToShortDateString() + ".xlsx");
                    HttpContext.Response.ContentType = result.ContentType;
                    HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
                    var fileContentResult = new FileContentResult(result.FileInByteArray, result.ContentType)
                    {
                        FileDownloadName = result.FileName
                    };
                    return fileContentResult;
                }
                else
                    return null;
                    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(VolteKPIDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await SaveOrEdit(dto, forced);
                //return await _manager.Add(dto, forced);
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
                return await _manager.Delete(id);
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
                return await _manager.DeleteDeep(id);
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
                return await _manager.GetRelatedRecords(id);
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
                return await _manager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] VolteKPIQueryDto volteKPIFilterDto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, volteKPIFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpGet(template: "Update{id}")]
        public VolteKPIDtoUpdate GetUpdateResourceVolteKPI(long id)
        {
            try
            {
                return _manager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
        [HttpPost("GetVolteKPI")]
        public QueryResultDto<VolteKPIDtoGrid> GetVolteKPI([FromBody] VolteKPIQueryDto volteKPIFilterDto)
        {
            try
            {
                return _manager.FindWithCondition(volteKPIFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}

