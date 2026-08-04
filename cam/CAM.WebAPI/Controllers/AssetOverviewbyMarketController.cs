using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
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
    public class AssetOverviewbyMarketController : CamControllerBase
    { 
 
        private readonly AssetOverviewbyMarketManager _assetOverviewbyMarketManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<short> _opcoList;
        private readonly List<short> _verticalList;
        private readonly bool _adminRoleCheck = false;
        public AssetOverviewbyMarketController(ICurrentUserService currentUserService,AuthorizedRoleManager authorizedRoleManager,  ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService
            , AssetOverviewbyMarketManager assetOverviewbyMarketManager, DropdownDataServiceManager dropdownDataServiceManager) : base(logger, contextAccessor)
        {
            _assetOverviewbyMarketManager = assetOverviewbyMarketManager;           
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _exportService = exportService;
            _authorizedRoleManager = authorizedRoleManager;
            _currentUserService= currentUserService;
            sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToInt16(x)).Distinct().ToList() : null;
        }


        [HttpPost("GetAssetOverviewbyMarket")]
        public async Task<ResultDto> GetNetworkElementGraph(
           [FromBody] AssetOverviewbyMarketQueryDto networkElementGraphFilterDto)
        {
            try
            {

                if ((networkElementGraphFilterDto.OpcoId == null) || networkElementGraphFilterDto.OpcoId != null && networkElementGraphFilterDto.OpcoId.Count == 0)
                {
                    networkElementGraphFilterDto.OpcoId = _opcoList;
                }
                if ((networkElementGraphFilterDto.VerticalId == null) || (networkElementGraphFilterDto.VerticalId != null && networkElementGraphFilterDto.VerticalId.Count == 0))
                {
                    networkElementGraphFilterDto.VerticalId = _verticalList;
                }
                var userVertical= _verticalList!=null && _verticalList.Count > 0 ? _verticalList.Select(x=>Convert.ToInt32(x)).ToList():null;
                return await _assetOverviewbyMarketManager.FindWithConditionQueryCheck(networkElementGraphFilterDto, userVertical,_adminRoleCheck);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        } 
        [HttpPost("GetAllOpcos")]
        public async Task<Dictionary<long, string>> GetAllOpcos()
        {
            try
            {
                return await _dropdownDataServiceManager.GetAllOpcos();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost("GetAllResources")]
        public async Task<ResultDto> GetAllResources()
        {
            try
            {
                return await _assetOverviewbyMarketManager.GetAllResourcesDropDown();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

       [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] AssetOverviewbyMarketQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if ((dto.OpcoId == null) || dto.OpcoId != null && dto.OpcoId.Count == 0)
                {
                    dto.OpcoId = _opcoList;
                }
                if ((dto.VerticalId == null) || (dto.VerticalId != null && dto.VerticalId.Count == 0))
                {
                    dto.VerticalId = _verticalList;
                }
                var data = await Task.Run(() => _assetOverviewbyMarketManager.FindWithConditionForExport(dto));
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "AssetOverviewbyMarket_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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


    }
}

