using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Reconsiliation;
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

    public class ReconciliationController : CamControllerBase
    {
        private readonly ReconciliationManager _manager;
        private readonly IExportService _exportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck=false;
        private readonly List<string> _opcoDescriptionList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        public ReconciliationController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, ReconciliationManager manager,
             ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoDescriptionList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDescription != null && _roleOpcoList.OpcoDescription.Any() == true ?
                _roleOpcoList.OpcoDescription : null;
        }

        [HttpPost("Get")]

        public async Task<QueryResultDto<ReconciliationGridDto>> GetReconciliationRecords([FromBody] ReconciliationQueryDto dto)
        {
            try
            {
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoDescriptionList;
                return await _manager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ReconciliationQueryDto dto)
        {
            try
            {
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCo = _opcoDescriptionList;
                return _manager.GetFilter(propertyName, propertyFilter, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost("Update")]
        public async Task<ResultDto> UpdateReconciliation(long id)
        {
            try
            {
                return await _manager.Update(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        #region //commond
        //[HttpPost("ExportReport")]
        //public FileContentResult ExportReport([FromBody] AspnetuserroleQueryDto dto)
        //{
        //    dto.Page = 0;
        //    dto.PageSize = 0;
        //    var data = _manager.FindWithCondition(dto);
        //    var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
        //        .Select(gcs => new ExportSheet()
        //        {
        //            TabName = gcs.Key,
        //            Data = data.Items.Cast<object>().ToList()
        //        });

        //    var reportSheets = tabs.ToList();
        //    var result = _exportService.GetExcelFrom(reportSheets,
        //         "Users_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
        //    HttpContext.Response.ContentType = result.ContentType;
        //    HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
        //    var fileContentResult = new FileContentResult(result.FileInByteArray, result.ContentType)
        //    {
        //        FileDownloadName = result.FileName
        //    };
        //    return fileContentResult;
        //}
        #endregion
    }
}
