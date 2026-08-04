using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.GraphicalReport.Exodus_Graphical_Report;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.QueryDto.GraphicalReports;
using CAM.Exports;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.WebAPI.Controllers.Reports.GraphicalReports
{
    [Route("api/[controller]")]
    [ApiController]

    #if DEBUG
        [Authorize]
    #else
        [Authorize]
    #endif
    public class ExodusGraphicalAssetLevelReportController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly List<int> _verticalList;
        private readonly bool _adminRoleCheck =false;
        private readonly List<long> _opcoList;  
        private readonly CommonManager _commonManager;
        private readonly ExodusAssetLGraphicalevel1ReportManager _exodusAssetGraphicalLevel1ReportManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;

        public ExodusGraphicalAssetLevelReportController(SystemTypeManager systemTypeManager, ICurrentUserService currentUserService, IExportService exportService,
            ILoggerManager logger, IHttpContextAccessor contextAccessor, AuthorizedRoleManager authorizedRoleManager, CommonManager commonManager,
            DropdownDataServiceManager dropdownDataServiceManager, ExodusAssetLGraphicalevel1ReportManager exodusAssetGraphicalLevel1ReportManager) : base(logger, contextAccessor)
        {

            _currentUserService = currentUserService;
            _exportService = exportService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _exodusAssetGraphicalLevel1ReportManager = exodusAssetGraphicalLevel1ReportManager;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt64(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToInt32(x)).Distinct().ToList() : null;
        }


        [HttpPost("GetOpcoWisePercentage")]
        public async Task<ResultDto> GetOpcoWisePercentage([FromBody] ExodusGraphicalReportQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId==null ||(buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;

                if ((buildFilterDto.VerticalName == null) || (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count == 0))
                    buildFilterDto.VerticalName = _verticalList != null ? _verticalList.Select(x => Convert.ToString(x)).ToList() : null;
                return await _exodusAssetGraphicalLevel1ReportManager.FindWithCondition(buildFilterDto, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost("Get")]
        public async Task<ResultDto> Get([FromBody] ExodusGraphicalReportQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;

                if ((buildFilterDto.VerticalName == null) || (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count == 0))
                    buildFilterDto.VerticalName = _verticalList != null ? _verticalList.Select(x => Convert.ToString(x)).ToList() : null;
                return await _exodusAssetGraphicalLevel1ReportManager.FindWithCondition(buildFilterDto, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost("GetAllDropdownRecords")]
        public async Task<ResultDto> GetAllDropdownRecords([FromBody] ExodusGraphicalReportQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;

                if ((buildFilterDto.VerticalName == null) || (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count == 0))
                    buildFilterDto.VerticalName = _verticalList != null ? _verticalList.Select(x => Convert.ToString(x)).ToList() : null;

                return await _exodusAssetGraphicalLevel1ReportManager.GetAllDropdowns(buildFilterDto, isPageLoad);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }
    }
}
