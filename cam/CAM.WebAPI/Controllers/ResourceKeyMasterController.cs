using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.Entity;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using CAM.DataTransferObjects.Entita.NetworkElement;
using CAM.DataTransferObjects.Entita.ResourceKeyMaster;

namespace CAM.WebAPI.Controllers
{
    [Route("api/ResourceKeyMaster")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class ResourceKeyMasterController : CamControllerBase
    {

        private readonly ResourceKeyMasterManager _manager;
        private readonly IExportService _exportService;

        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly string _adminRoleCheck;
        private readonly List<short> _opcoList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;

        public ResourceKeyMasterController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService,
            ResourceKeyMasterManager manager, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
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
        }
        [HttpPost("Get")]
        public QueryResultDto<ResourceKeyMasterDtoGrid> ResourceKeyMaster(
           [FromBody] ResourceKeyMasterQueryDto resourceKeyMasterQueryDto)
        {
            try
            {
                if (resourceKeyMasterQueryDto.OpCo != null && resourceKeyMasterQueryDto.OpCo.Count() == 0)
                    resourceKeyMasterQueryDto.OpCo = _opcoList;
                return _manager.FindWithCondition(resourceKeyMasterQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;

            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] ResourceKeyMasterQueryDto dto)
        {
            try
            {


                dto.Page = 0;
                dto.PageSize = 0;

                if (dto.OpCo != null && dto.OpCo.Count() == 0)
                    dto.OpCo = _opcoList;

                var data = _manager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });
                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                    "ResourceKeyMaster_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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


        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ResourceKeyMasterQueryDto resourceKeyMasterDto)
        {
            try
            {
                if (resourceKeyMasterDto.OpCo != null && resourceKeyMasterDto.OpCo.Count() == 0)
                    resourceKeyMasterDto.OpCo = _opcoList;
                var data = _manager.GetFilter(propertyName, propertyFilter, resourceKeyMasterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;

            }

        }
    }
}

