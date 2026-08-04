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

    public class VolteKPIWorklogController : CamControllerBase
    {

        private readonly VolteKPIManager _manager;
        private readonly IExportService _exportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<int> _opcoList;
        private readonly List<int> _verticalList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly bool _adminRoleCheck=false;
        private readonly bool _isKpiAdmin = false;
        public VolteKPIWorklogController(ILoggerManager logger, IHttpContextAccessor contextAccessor,
            IExportService exportService, VolteKPIManager manager, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _currentUserService = currentUserService;
            
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt32(x)).Distinct().ToList() : null;
            _isKpiAdmin = _roleOpcoList != null ? _roleOpcoList.IsKpiAdmin : _isKpiAdmin;


        }


        [HttpPost("Get")]
        public QueryResultDto<VolteKPIWorklogDto> Get([FromBody] VolteKPIWorklogQueryDto dto)
        {
            try
            {
                if (_isKpiAdmin)
                {
                    //_currentUserService.UserInRole(true, "KPI Administrator", "Admin");
                    if ((dto.OpCo == null) || dto.OpCo != null &&
           dto.OpCo.Count() == 0)
                        dto.OpCo = _opcoList;

                    return _manager.FindWithCondition(dto);
                }
                else return new QueryResultDto<VolteKPIWorklogDto>();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(VolteKPIWorklogDto dto, [FromQuery] bool? forced)
        {
            try
            {
                _currentUserService.UserInRole(true, "KPI Administrator", "Admin");
                return await _manager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] VolteKPIWorklogQueryDto dto)
        {
            try
            {
                if (_isKpiAdmin)
                {
                    if ((dto.OpCo == null) || dto.OpCo != null &&
                  dto.OpCo.Count() == 0)
                        dto.OpCo = _opcoList;
                    var data = _manager.GetFilter(propertyName, propertyFilter, dto);
                    return data;
                }
                else
                    return new List<FilterValueDto>();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetDuplicates")]
        public async Task<ResultDto> GetDuplicates(long VolteKPIId, int VolteKPIType, long VolteKPIWorklogId)
        {
            try
            {
                return await _manager.GetDuplicates(VolteKPIId, VolteKPIType, VolteKPIWorklogId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}

