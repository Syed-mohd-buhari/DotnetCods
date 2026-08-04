using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.GraphicalReport.Exodus_Graphical_Report;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.QueryDto;
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
    public class ExodusGraphicalLevel3ReportController : CamControllerBase
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
        private readonly ExodusGraphicalLevel3ReportManager _exodusGraphicalLevel3ReportManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;

        public ExodusGraphicalLevel3ReportController(SystemTypeManager systemTypeManager, ICurrentUserService currentUserService, IExportService exportService,
            ILoggerManager logger, IHttpContextAccessor contextAccessor, AuthorizedRoleManager authorizedRoleManager, CommonManager commonManager,
            DropdownDataServiceManager dropdownDataServiceManager, ExodusGraphicalLevel3ReportManager exodusGraphicalLevel3ReportManager) : base(logger, contextAccessor)
        {

            _currentUserService = currentUserService;
            _exportService = exportService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _exodusGraphicalLevel3ReportManager = exodusGraphicalLevel3ReportManager;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt64(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToInt32(x)).Distinct().ToList() : null;
        }


        [HttpPost("GetAssetLevlReportForExodus")]
        public async Task<ResultDto> GetOpcoWisePercentage([FromBody] ExodusQueryLevel3ReportQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;

                if ((buildFilterDto.VerticalName == null) || (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count == 0))
                    buildFilterDto.VerticalName = _verticalList != null ? _verticalList?.Select(x => Convert.ToString(x)).ToList() : null;

                return await _exodusGraphicalLevel3ReportManager.FindWithConditionAsyncNew(buildFilterDto, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpPost("GetAssetLevlReportForExodusAPI")]
        public async Task<ResultDto> GetOpcoWisePercentageAPI([FromBody] ExodusQueryLevel3ReportQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;

                if ((buildFilterDto.VerticalName == null) || (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count == 0))
                    buildFilterDto.VerticalName = _verticalList != null ? _verticalList?.Select(x => Convert.ToString(x)).ToList() : null;

                return await _exodusGraphicalLevel3ReportManager.FindWithConditionAsyncNew(buildFilterDto, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpPost("GetOpcoAndPlannnedDcfDropdowns")]
        public async Task<ResultDto> GetOpcoAndPlannnedDcfDropdowns([FromBody] ExodusQueryLevel3ReportQueryDto buildFilterDto)
        {
             
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;

                return await _exodusGraphicalLevel3ReportManager.GetOpcoAndDcfDropdown(buildFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        

    }
}
