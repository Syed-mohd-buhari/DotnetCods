using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport;
using CAM.BusinessManager.ExtensionMethod.Report;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
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
    public class AssetPivotReportController : CamControllerBase
    {

        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<short> _opcoList;
        private readonly bool _adminRoleCheck=false;
        private readonly List<int> _verticalList;

        private readonly CommonManager _commonManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly AssetPivotReportManager _networkElementPivotReportManager;
        public AssetPivotReportController(ILoggerManager logger, IHttpContextAccessor contextAccessor,
            IExportService exportService, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager
                , CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager
            , AssetPivotReportManager networkElementPivotReportManager) : base(logger, contextAccessor)
        {            
            _exportService = exportService;
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;           
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _networkElementPivotReportManager = networkElementPivotReportManager;

            this.sessionUserId = _currentUserService.UserId;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
               _roleOpcoList.VerticalDetails : null;
        }
         

        #region //----------------- NetworkElement Pivot Graph
        [HttpPost("GetAssetPivot")]
        public async Task<QueryResultDto<NetworkElementAsPlannedPivotDtoGrid>> GetAssetPivot(
           [FromBody] NetworkElementAsPlannedQueryDto designComponentFilterDto)
        {
            try
            {
                if (designComponentFilterDto.OpCo == null || (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0))
                    designComponentFilterDto.OpCo = _opcoList;
                if (designComponentFilterDto.VerticalName == null || (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0))
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                return await _networkElementPivotReportManager.FindWithCondition(designComponentFilterDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex);

                throw;
            }

        }

        [HttpGet("GetPivotFilterAllDropDown")]
        public async Task<ResultDto> GetAssetPivot()
        {
            try
            {
                var result = _networkElementPivotReportManager.GetFilterDropdown(_opcoList)?.Result.Data;
                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);

                throw;
            }
        }

        
       [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] NetworkElementAsPlannedQueryDto designComponentFilterDto)
        {
            try
            {
                if (designComponentFilterDto.OpCo == null || (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0))
                    designComponentFilterDto.OpCo = _opcoList;
                if (designComponentFilterDto.VerticalName == null || (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0))
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                var data = await _networkElementPivotReportManager.GetFilterForGridLevel(propertyName, propertyFilter, designComponentFilterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);

                throw;
            }
        }

        [HttpPost(template: "ExportAssetPivotReport")]
        public FileContentResult ExportAssetPivotReport([FromQuery] NetworkElementAsPlannedQueryDto designComponentFilterDto)
        {
            try
            {
                if (designComponentFilterDto.OpCo==null || (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0))
                    designComponentFilterDto.OpCo = _opcoList;
                if (designComponentFilterDto.VerticalName == null || (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0))
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                var data = _networkElementPivotReportManager.FindWithCondition(designComponentFilterDto).Result;

                List<ExportSheetCustom> tabs = new List<ExportSheetCustom>();

                List<NetworkElementAsPlannedPivotDtoGrid> list = data.Items.ToList();
                if (list != null && list.Count != 0)
                {
                    tabs.Add(new ExportSheetCustom
                    {
                        TabName = "AssetPivotByLocation",
                        CustomHeaders = list.GetHeaders(),
                        Data = list.GetData()
                    });
                }
                else
                {
                    tabs.Add(new ExportSheetCustom
                    {
                        TabName = "AssetPivotByLocation",
                        CustomHeaders = new List<List<ExportSheetCustomHeader>>(),
                        Data = new List<List<ExportSheetCustomCell>>()
                    });
                }

                var result = _exportService.GetExcelFrom(tabs, "AssetPivotByLocation_" + DateTime.Now.ToShortDateString() + ".xlsx");
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
        #endregion

    }
}

