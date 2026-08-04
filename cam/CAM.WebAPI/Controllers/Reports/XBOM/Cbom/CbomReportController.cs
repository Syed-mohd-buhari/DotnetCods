using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport;
using CAM.BusinessManager.Entity.Report.XBom;
using CAM.BusinessManager.ExtensionMethod.Report;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.XBom.CBom;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
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

namespace CAM.WebAPI.Controllers.Reports.XBOM.Vbom
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CbomReportController : CamControllerBase
    {
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<short> _opcoList;
        private readonly bool _adminRoleCheck;

        private readonly CommonManager _commonManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly CBomReportManager _cbomReportManager;
        public CbomReportController(ILoggerManager logger, IHttpContextAccessor contextAccessor, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager
                , CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager
            , CBomReportManager cbomReportManager) : base(logger, contextAccessor)
        {
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _cbomReportManager = cbomReportManager;

            sessionUserId = _currentUserService.UserId;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
        }



        [HttpPost("GetAggregatedAnddissagregatedReport")]
        public async Task<QueryResultDto<CbomReportDto>> GetAggregatedAnddissagregatedReport([FromBody] CbomReportQueryDto vbomReportQueryDto)
        {
            try
            {
                if (vbomReportQueryDto.OpcoName == null || vbomReportQueryDto.OpcoName != null && vbomReportQueryDto.OpcoName.Count == 0)
                {
                    vbomReportQueryDto.OpcoName = _opcoList != null && _opcoList.Count > 0 ? _opcoList.Select(x => x.ToString()).ToList() : null;
                }
                return await _cbomReportManager.GetCbomAggregatedAndDisaggregatedReport(vbomReportQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        #region //----------------- Filters

        [HttpPost("GetAllResources")]
        public async Task<ResultDto> GetAllResources()
        {
            try
            {
                return await _cbomReportManager.GetAllResourcesDropDown(_opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        #endregion

    }
}

