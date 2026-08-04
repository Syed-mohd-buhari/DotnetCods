using CAM.BusinessManager;
using CAM.Contracts;
using CAM.DataTransferObjects;
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
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.NFVITransition;
using CAM.WebAPI.Identity;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif
    public class NFVITransitionController : CamControllerBase
    {
        private readonly NFVITransitionManager _nvfiTransitionManager;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly string _adminRoleCheck;
        private readonly List<short> _opcoList;

        public NFVITransitionController(NFVITransitionManager nvfiTransitionManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            IExportService exportService, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _nvfiTransitionManager = nvfiTransitionManager;
            _exportService = exportService;
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRoleOpcoList(this.sessionUserId);

            _adminRoleCheck = _roleOpcoList.Where(x => x.Role == this.adminRoleId.ToString()).Select(x => x.Role).FirstOrDefault();

            _opcoList = (_adminRoleCheck ==null) ? _roleOpcoList.Select(x => Convert.ToInt16(x.OpCo)).Distinct().ToList() : null;
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<NFVITransitionDtoGrid>> GetVNFTransition([FromBody] NFVITransitionQueryDto filterDto)
        {
            try
            {
                if (filterDto.OpCo == null || (filterDto.OpCo != null && filterDto.OpCo.Count() == 0)) filterDto.OpCo = _opcoList;
                return await _nvfiTransitionManager.GetEnityGrid(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] NFVITransitionQueryDto filterDto)
        {
            try
            {
                if (filterDto.OpCo == null || (filterDto.OpCo != null && filterDto.OpCo.Count() == 0)) filterDto.OpCo = _opcoList;
                var data = _nvfiTransitionManager.GetFilter(propertyName, propertyFilter, filterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost]
        public async Task<ResultDto> Create([FromBody] NFVITransitionDtoCreate dto)
        {
            try
            {
                return await _nvfiTransitionManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut(template: "Restore")]
        public async Task<ResultDto> Restore(long id)
        {
            try
            {
                return await _nvfiTransitionManager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(NFVITransitionDtoUpdate dto)
        {
            try
            {
                return await _nvfiTransitionManager.Update(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] NFVITransitionQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.OpCo != null && dto.OpCo.Count() == 0)  dto.OpCo = _opcoList;
                var data = await _nvfiTransitionManager.GetEnityGrid(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                foreach (var item in tabs)
                {
                    reportSheets.Add(item);
                }


                var result = _exportService.GetExcelFrom(reportSheets,
                    "NFVI-Transition" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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


        [HttpDelete]
        public async Task<ResultDto> Delete(short id)
        {
            try
            {
                return await _nvfiTransitionManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template:"DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            try
            {
                return await _nvfiTransitionManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            try
            {
                return await _nvfiTransitionManager.GetRelatedRecords(id);
            }
           
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "Create")]
        public NFVITransitionDtoCreate GetCreateResourceVNFTransition()
        {
            try
            {
                return _nvfiTransitionManager.GetCreatePage(_opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public NFVITransitionDtoUpdate GetUpdateResourceVNFTransition(short id)
        {
            try
            {
                return _nvfiTransitionManager.GetUpdatePage(id, _opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }
    }
}
