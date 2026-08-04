using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponentFamilyLifeCycle;
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
    [Route("api/DesignComponentFamilyLifeCycle")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class DesignComponentFamilyLifeCycleController : CamControllerBase
    {
        private readonly DesignComponentFamilyLifeCycleManager _manager;
        private readonly IExportService _exportService;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly string _adminRoleCheck;
        private readonly List<short> _opcoList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        public DesignComponentFamilyLifeCycleController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService,
            DesignComponentFamilyLifeCycleManager manager,
            ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager, DropdownDataServiceManager dropdownDataServiceManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;

            _currentUserService = currentUserService;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRoleOpcoList(this.sessionUserId);

            _adminRoleCheck = _roleOpcoList.Where(x => x.Role == this.adminRoleId.ToString()).Select(x => x.Role).FirstOrDefault();

            _opcoList = (_adminRoleCheck != null) ? null : _roleOpcoList.Select(x => Convert.ToInt16(x.OpCo)).Distinct().ToList();
            _dropdownDataServiceManager = dropdownDataServiceManager;
        }


        [HttpPost("Get")]
        public   QueryResultDto<DesignComponentFamilyLifeCycleDtoGrid> GetIdentity([FromBody] DesignComponentFamilyLifeCycleQueryDto DcfLifeCycleDto)
        {
            try
            {
                if (DcfLifeCycleDto.OpCo != null && DcfLifeCycleDto.OpCo.Count() == 0) DcfLifeCycleDto.OpCo = _opcoList;
                return _manager.FindWithCondition(DcfLifeCycleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] DesignComponentFamilyLifeCycleQueryDto DcfLifeCycleFilterDto)
        {
            try
            {
                if (DcfLifeCycleFilterDto.OpCo != null && DcfLifeCycleFilterDto.OpCo.Count() == 0) DcfLifeCycleFilterDto.OpCo = _opcoList;
                var data = _manager.GetFilter(propertyName, propertyFilter, DcfLifeCycleFilterDto);


                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        #region // Drop down
        [HttpPost("GetPageFilterDropdown")]
        public async Task <ResultDto> GetAllDropdownAsync()
        {
            try
            {
                return await Task.Run(() => new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        allOpcos = _dropdownDataServiceManager.GetOpcos(false, false, true).Result.Data,
                        designComponentFammiyNames = _dropdownDataServiceManager.GetAllDesignComponentFamilyName(null)
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        #endregion

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] DesignComponentFamilyLifeCycleQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.OpCo != null && dto.OpCo.Count() == 0) dto.OpCo = _opcoList;

                var data = _manager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });
                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "DesignComponentFamilyLifeCylce_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
    }
}