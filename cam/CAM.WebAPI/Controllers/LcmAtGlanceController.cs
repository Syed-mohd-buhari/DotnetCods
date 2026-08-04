
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.Report.GraphicalReport;
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

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class LcmAtGlanceController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly List<short> _verticalList;
        private readonly bool _adminRoleCheck=false;
        private readonly List<int> _userListBasedOnVerticalId;
        private readonly CommonManager _commonManager;
        private readonly LcmAtGlanceManager _lcmAtGlanceManager;
        private readonly LcmAtGlanceForEosManager _lcmAtGlanceForEosManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly List<short> _opcoList;

        public LcmAtGlanceController(SystemTypeManager systemTypeManager, ICurrentUserService currentUserService, IExportService exportService,
            ILoggerManager logger, IHttpContextAccessor contextAccessor, AuthorizedRoleManager authorizedRoleManager, CommonManager commonManager,
            LcmAtGlanceManager lcmAtGlanceManager, DropdownDataServiceManager dropdownDataServiceManager, LcmAtGlanceForEosManager lcmAtGlanceForEosManager) : base(logger, contextAccessor)
        {

            _currentUserService = currentUserService;
            _exportService = exportService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            _lcmAtGlanceManager = lcmAtGlanceManager;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails: null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;

            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _lcmAtGlanceForEosManager = lcmAtGlanceForEosManager;
        }

        [HttpPost("GetOverAllOpcoAndAssetWisePercentage")]
        public async Task<ResultDto> GetOverAllOpcoAndAssetWisePercentage()
        {
            try
            {
                return await _lcmAtGlanceManager.FindWithConditionForOverAllOpco();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("GetPlannedActivity")]
        public async Task<ResultDto> GetPlannedActivity([FromBody] LcmAtGlanceQueryDto buildFilterDto)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceManager.FindWithConditionForPALevelPercentage(buildFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("Get")]
        public async Task<ResultDto> GetLcmLevelPercentageReport([FromBody] LcmAtGlanceQueryDto buildFilterDto)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceManager.FindWithCondition(buildFilterDto, true, true, true, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("GetOpcoWisePercentage")]
        public async Task<ResultDto> GetOpcoWisePercentage([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceManager.FindWithCondition(buildFilterDto, true, false, false, false, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpPost("GetProductWisePercentage")]
        public async Task<ResultDto> GetProductWisePercentage([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceManager.FindWithCondition(buildFilterDto, false, true, false, false, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("GetSupportedServiceWisePercentage")]
        public async Task<ResultDto> GetSupportedServiceWisePercentage([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceManager.FindWithCondition(buildFilterDto, false, false, true, false, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("GetOverAllPercentageBasedOnFilters")]
        public async Task<ResultDto> GetOverAllPercentageBasedOnFilters([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceManager.FindWithCondition(buildFilterDto, false, false, false, true, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("GetPlannedActivityDropDown")]
        public async Task<ResultDto> GetPlannedActivityDropDown([FromBody] LcmAtGlanceQueryDto buildFilterDto)
        {
            try
            {
                return await _lcmAtGlanceManager.GetPADropdown(buildFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
 
        [HttpPost("GetAllDropdownRecords")]
        public async Task<ResultDto> GetAllDropdownRecords(bool isPageLoad = false)
        {
            try
            {
                var userVerticalList= _verticalList!=null && _verticalList.Count > 0? _verticalList.Select(x=>Convert.ToInt32(x)).ToList(): null;
                return await Task.Run(() => new ResultDto           
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        allOpcos = _dropdownDataServiceManager.GetOpcos(false, false, true, true,_opcoList).Result,
                        allProducts = _dropdownDataServiceManager.GetAllProducts(false, false, true).Result.Data,
                        allSubNetworkBoundary = _dropdownDataServiceManager.GetAllSupportedServices(false, false, true).Result.Data,
                        allVerticalResponse = _dropdownDataServiceManager.GetAllVerticalResponse(false, false, true,false,_adminRoleCheck, userVerticalList).Result.Data,
                        allProductImportances = _dropdownDataServiceManager.GetProductImportances().Result,
                        allPlannedActivityResources = _dropdownDataServiceManager.GetPlannedActivityType(0).Result.Data,
                        FilterVerticalRespone = _dropdownDataServiceManager.GetAllVerticalResponse(false, false, true, isPageLoad, _adminRoleCheck, userVerticalList).Result
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        [HttpPost("Create")]
        public ResultDto CreatePage([FromBody] LcmAtGlanceQueryDto buildFilterDto)
        {
            try
            {
                var allDropdownRecords = GetAllDropdownRecords();
                var lcmPercentagePercentage = _lcmAtGlanceManager.FindWithCondition(buildFilterDto);
                var lcmOverAllOpcoAndAssetPercentage = _lcmAtGlanceManager.FindWithConditionForOverAllOpco();

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        allDropdownRecords,
                        lcmPercentagePercentage,
                        lcmOverAllOpcoAndAssetPercentage
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        #region LCM AT GLANCE FOR EOFS
        [HttpPost("GetOverAllOpcoAndAssetWisePercentageForEofs")]
        public async Task<ResultDto> GetOverAllOpcoAndAssetWisePercentageForEofs()
        {
            try
            {
                return await _lcmAtGlanceForEosManager.FindWithConditionForOverAllOpco();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("GetPlannedActivityForEofs")]
        public async Task<ResultDto> GetPlannedActivityForEofs([FromBody] LcmAtGlanceQueryDto buildFilterDto)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceForEosManager.FindWithConditionForPALevelPercentage(buildFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("GetLcmLevelPercentageForEofs")]
        public async Task<ResultDto> GetLcmLevelPercentageReportForEofs([FromBody] LcmAtGlanceQueryDto buildFilterDto)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceForEosManager.FindWithCondition(buildFilterDto, true, true, true, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("GetOpcoWisePercentageForEofs")]
        public async Task<ResultDto> GetOpcoWisePercentageForEofs([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceForEosManager.FindWithCondition(buildFilterDto, true, false, false, false, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpPost("GetProductWisePercentageForEofs")]
        public async Task<ResultDto> GetProductWisePercentageForEofs([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceForEosManager.FindWithCondition(buildFilterDto, false, true, false, false, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("GetSupportedServiceWisePercentageForEofs")]
        public async Task<ResultDto> GetSupportedServiceWisePercentageForEofs([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceForEosManager.FindWithCondition(buildFilterDto, false, false, true, false, isPageLoad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("GetOverAllPercentageBasedOnFiltersForEofs")]
        public async Task<ResultDto> GetOverAllPercentageBasedOnFiltersForEofs([FromBody] LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            try
            {
                if (buildFilterDto.OpcoId == null || (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Count == 0))
                    buildFilterDto.OpcoId = _opcoList;
                if (buildFilterDto.VerticalResponsibleId == null || (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Count == 0))
                    buildFilterDto.VerticalResponsibleId = _verticalList;
                return await _lcmAtGlanceForEosManager.FindWithCondition(buildFilterDto, false, false, false, true, isPageLoad);
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
