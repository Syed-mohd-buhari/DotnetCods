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
using CAM.DataTransferObjects.Entita.NetworkElementAsIs;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using CAM.Imports;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class NetworkElementAsIsController : CamControllerBase
    {

        private readonly NetworkElementAsIsManager _manager;
        public readonly NetworkElementAsIsImport _networkasisImport;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;

        public NetworkElementAsIsController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, IImportService importService, NetworkElementAsIsImport networkasisImport, NetworkElementAsIsManager manager
            , ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _networkasisImport = networkasisImport;
            _manager = manager;
            _importService = importService;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;
        }

        [HttpGet(template: "Create")]
        public NetworkElementAsIsDtoCreate GetCreateResourceNetworkElementAsIs()
        {
            try
            {
                return _manager.GetCreatePage(_opcoList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public NetworkElementAsIsDtoUpdate GetUpdateResourceNetworkElementAsIs(long id)
        {
            try
            {
                return _manager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Get")]
        public QueryResultDto<NetworkElementAsIsDtoGrid> GetNetworkElementAsIs(
           [FromBody] NetworkElementsAsIsQueryDto designComponentFilterDto)
        {
            try
            {
                if ((designComponentFilterDto.OpCo==null) || (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0))
                {
                    designComponentFilterDto.OpCo = _opcoList;
                }
                if ((designComponentFilterDto.VerticalName == null) || designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0)
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                return _manager.FindWithCondition(designComponentFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] NetworkElementsAsIsQueryDto dto)
        {
            try
            {

            
            dto.Page = 0;
                dto.PageSize = 0;
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                {
                    dto.OpCo = _opcoList;
                }
                if ((dto.VerticalName == null) || dto.VerticalName != null && dto.VerticalName.Count == 0)
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                var data = _manager.FindWithCondition(dto);
            var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                .Select(gcs => new ExportSheet()
                {
                    TabName = gcs.Key,
                    Data = data.Items.Cast<object>().ToList()
                });

            foreach (var nome in data.Items)
            {
                nome.HardwareSolution = nome.HardwareSolution?.Replace("<b class=\"text-lowercase\">", "");
                nome.HardwareSolution = nome.HardwareSolution?.Replace("<b class=\"text-lowercase\" >", "");
                nome.HardwareSolution = nome.HardwareSolution?.Replace("</b>", "");

                nome.HardwareType = nome.HardwareType?.Replace("<b class=\"text-lowercase\">", "");
                nome.HardwareType = nome.HardwareType?.Replace("<b class=\"text-lowercase\" >", "");
                nome.HardwareType = nome.HardwareType?.Replace("</b>", "");
            }

            var reportSheets = tabs.ToList();
            string IdColumn = _networkasisImport.IdColumn;
            List<string> EditableColumnList = _networkasisImport.EditableColumnList;

            var result = _exportService.GetExcelFrom(reportSheets,
                "Feedback-Loop" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender, IdColumn, EditableColumnList);
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
                return null;
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _manager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template:"DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                   return await _manager.DeleteDeep(id);
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
                return await _manager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] NetworkElementAsIsDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _manager.Add(dto, forced);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(NetworkElementAsIsDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _manager.Update(dto, forced);
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
                return await _manager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] NetworkElementsAsIsQueryDto designComponentFilterDto)
        {
            try
            {
                if ((designComponentFilterDto.OpCo == null) || (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0))
            {
                designComponentFilterDto.OpCo = _opcoList;
            }
                if ((designComponentFilterDto.VerticalName == null) || designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0)
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                var data = _manager.GetFilter(propertyName, propertyFilter, designComponentFilterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetSystemTypeList")]
        public async Task<ResultDto> GetSystemTypeList(short? oemId)
        {

            try
            {
                return await _manager.GetSystemTypeList(oemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpGet(template: "GetSystemTypeListFromAsPlanned")]
        public async Task<ResultDto> GetSystemTypeListFromAsPlanned(long asPlannedId)
        {
            try
            {
                return await _manager.GetSystemTypeListFromAsPlanned(asPlannedId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        [HttpGet(template: "GetSystemTypeInfo")]
        public async Task<ResultDto> GetSystemTypeInfo(long sysId)
        {
            try
            {
                return await _manager.GetSystemTypeInfo(sysId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        } 
        
        [HttpGet(template: "GetNetworkElementAsPlannedResource")]
        public async Task<ResultDto> GetNetworkElementAsPlannedResource(long sysId, int opcoId)
        {
            try
            {
                return await _manager.GetNetworkElementAsPlannedResource(sysId, opcoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }       
        
        [HttpGet(template: "GetAsPlannedLocation")]
        public async Task<ResultDto> GetAsPlannedLocation(long asPlannedId)
        {
            try
            {
                return await _manager.GetAsPlannedLocation(asPlannedId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost(template: "ImportReport")]
        public async Task<ResultDto> ImportReports()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                IFormFile File = httpRequest.Form.Files.FirstOrDefault();
                return await _importService.Processimportexcel(File, "networkelementasis");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}

