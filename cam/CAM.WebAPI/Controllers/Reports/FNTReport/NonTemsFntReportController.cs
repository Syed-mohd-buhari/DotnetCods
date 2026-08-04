using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.PassThroughData;
using CAM.BusinessManager.Entity.Report.FNT_Report;
using CAM.Contracts;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.BPT;
using CAM.DataTransferObjects.QueryDto.FNT;
using CAM.Enum;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture.QueryResult;
using CAM.WebAPI.Identity;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Reports.FNTReport
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NonTemsFntReportController : CamControllerBase
    {
        private readonly AssetPassThroughManager _manager;
        private readonly NonTemsFntReportManger _nonTemsFntManger;
        private readonly IExportService _exportService;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<string> _opcoDescriptionList;
        private readonly bool _adminRoleCheck = false;
        private readonly List<long> _verticalList;
        public NonTemsFntReportController(ILoggerManager logger, IHttpContextAccessor contextAccessor, NonTemsFntReportManger nonTemsFntManger
            , IExportService exportService, AssetPassThroughManager manager, DropdownDataServiceManager dropdownDataServiceManager
            , AuthorizedRoleManager authorizedRoleManager, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _nonTemsFntManger = nonTemsFntManger;
            _exportService = exportService;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoDescriptionList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDescription != null && _roleOpcoList.OpcoDescription.Any() == true ?
                _roleOpcoList.OpcoDescription : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToInt64(x)).Distinct().ToList() : null;
        }
        [HttpPost("Get")]
        public async Task<QueryResultDto<NonTemsFntReportDtoGrid>> GetNonTemsFntReport([FromBody] NonTemsFntReportQueryDto fntQueryDto)
        {
            try
            {
                if (((fntQueryDto.LocationOfHardwareAsset == null) || (fntQueryDto.LocationOfHardwareAsset != null && fntQueryDto.LocationOfHardwareAsset.Count == 0))
                   && ((fntQueryDto.LocalMarket == null) || (fntQueryDto.LocalMarket != null && fntQueryDto.LocalMarket.Count == 0))
                   && ((fntQueryDto.Market == null) || (fntQueryDto.Market != null && fntQueryDto.Market.Count == 0)))
                {
                    fntQueryDto.LocationOfHardwareAsset = _opcoDescriptionList;
                    fntQueryDto.LocalMarket = _opcoDescriptionList;
                    fntQueryDto.Market = _opcoDescriptionList;
                }

                if ((fntQueryDto.NonTemsVertical == null) || (fntQueryDto.NonTemsVertical != null && fntQueryDto.NonTemsVertical.Count == 0))
                    fntQueryDto.NonTemsVertical = _verticalList;
                return await _nonTemsFntManger.FindWithCondition(fntQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetNonTemsFntFilter([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] NonTemsFntReportQueryDto fntQueryDto)
        {
            try
            {
                if (((fntQueryDto.LocationOfHardwareAsset == null) || (fntQueryDto.LocationOfHardwareAsset != null && fntQueryDto.LocationOfHardwareAsset.Count == 0))
                   && ((fntQueryDto.LocalMarket == null) || (fntQueryDto.LocalMarket != null && fntQueryDto.LocalMarket.Count == 0))
                   && ((fntQueryDto.Market == null) || (fntQueryDto.Market != null && fntQueryDto.Market.Count == 0)))
                {
                    fntQueryDto.LocationOfHardwareAsset = _opcoDescriptionList;
                    fntQueryDto.LocalMarket = _opcoDescriptionList;
                    fntQueryDto.Market = _opcoDescriptionList;
                }

                if ((fntQueryDto.NonTemsVertical == null) || (fntQueryDto.NonTemsVertical != null && fntQueryDto.NonTemsVertical.Count == 0))
                    fntQueryDto.NonTemsVertical = _verticalList;
                var data = await _nonTemsFntManger.GetFilteredValues(propertyName, propertyFilter, fntQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
           
        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] NonTemsFntReportQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                string domainName = string.Empty;
                if (((dto.LocationOfHardwareAsset == null) || (dto.LocationOfHardwareAsset != null && dto.LocationOfHardwareAsset.Count == 0))
                    && ((dto.LocalMarket == null) || (dto.LocalMarket != null && dto.LocalMarket.Count == 0))
                    && ((dto.Market == null) || (dto.Market != null && dto.Market.Count == 0)))
                {
                    dto.LocationOfHardwareAsset = _opcoDescriptionList;
                    dto.LocalMarket = _opcoDescriptionList;
                    dto.Market = _opcoDescriptionList;
                }

                if ((dto.NonTemsVertical == null) || (dto.NonTemsVertical != null && dto.NonTemsVertical.Count == 0))
                    dto.NonTemsVertical = _verticalList;
                if (dto.NonTemsVertical != null && dto.NonTemsVertical.Any())
                {
                    foreach (var item in dto.NonTemsVertical)
                        domainName = _dropdownDataServiceManager.GetVerticalResponseName(item).Result;
                }
                var data = await _nonTemsFntManger.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "FNT_Report_" + domainName + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
        [HttpGet("GetDomainNames")]
        public async Task<List<KeyValuePairDto>> GetDomainNames()
        {
            try
            {
                var verticalList = _verticalList != null && _verticalList.Count > 0 ? _verticalList.Select(x => Convert.ToInt32(x)).ToList() : null;
                return await _dropdownDataServiceManager.GetAllVerticalResponsible(verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

    }
}
