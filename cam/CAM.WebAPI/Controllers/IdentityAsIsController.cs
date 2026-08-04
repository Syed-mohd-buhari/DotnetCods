using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using CAM.Infrastucture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.Identity;
using CAM.DataTransferObjects.Entita.IdentityAsIs;
using System.Linq;
using static CAM.Enum.ResourceTypeEnum;
using CAM.Imports;

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityAsIsController : CamControllerBase
    {
        private readonly IdentityAsIsManager _IdentityManager;
        private readonly ResourceKeyMasterManager _resourceKeyMasterManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;
        private readonly IdentityAsIsImport _identityAsIsImport;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly bool _adminRoleCheck=false;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;

        public IdentityAsIsController(IdentityAsIsManager IdentityManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ICurrentUserService currentUserService, IdentityAsIsImport identityAsIsImport, IExportService exportService,
            IImportService importService, ResourceKeyMasterManager resourceKeyMasterManager, AuthorizedRoleManager authorizedRoleManager
             ) : base(logger, contextAccessor)
        {
            _IdentityManager = IdentityManager;
            _currentUserService = currentUserService;
            _exportService = exportService;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _importService = importService;
            _identityAsIsImport = identityAsIsImport;


            _authorizedRoleManager = authorizedRoleManager;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;

        }

        [HttpPost("Get")]
        public QueryResultDto<IdentityAsIsDtoGrid> GetIdentityesAsync([FromBody] IdentityAsIsDtoQuery dto)
        {

            try
            {
                if (dto.VerticalName == null || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                    dto.VerticalName = _verticalList!=null && _verticalList.Count>0?_verticalList.Select(x=>x.ToString()).ToList():null;


                if (dto.OpCo == null || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCoId = _opcoList;


                return _IdentityManager.FindWithCondition(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost("Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromBody] IdentityAsIsDtoQuery dto)
        {
            try
            {
                if (dto.VerticalName == null || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                    dto.VerticalName = _verticalList != null && _verticalList.Count > 0?_verticalList.Select(x => x.ToString()).ToList():null;


                if (dto.OpCo == null || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCoId = _opcoList;

                var data = _IdentityManager.GetFilterResult(propertyName, propertyFilter, dto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }


        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] IdentityAsIsDtoQuery dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                if (dto.VerticalName == null || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                    dto.VerticalName = _verticalList != null && _verticalList.Count > 0 ? _verticalList.Select(x => x.ToString()).ToList():null;


                if (dto.OpCo == null || (dto.OpCo != null && dto.OpCo.Count == 0))
                    dto.OpCoId = _opcoList;

                var data = _IdentityManager.FindWithCondition(dto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                string IdColumn = _identityAsIsImport.IdColumn;
                List<string> EditableColumnList = _identityAsIsImport.EditableColumnList;

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Identities" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender, IdColumn, EditableColumnList);

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


        [HttpGet(template: "Update{id}")]
        public IdentityAsIsDtoUpdate GetUpdateResourceIdentity(short id)
        {
            try
            {
                return _IdentityManager.GetUpdatePage(id, _opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("Create")]
        public IdentityAsIsDtoCreate GetCreate()
        {
            try
            {
                return _IdentityManager.GetCreatePage(_opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet("GetRelatedRecords{id}")]
        public ResultDto GetRelatedRecords(short id)
        {
            try
            {
                return _IdentityManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] IdentityAsIsDtoCreate dto)
        {
            try
            {
                return await _IdentityManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(IdentityAsIsDtoUpdate IdentityDtoUpdate)
        {
            try
            {
                return await _IdentityManager.Update(IdentityDtoUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            try
            {
                return await _IdentityManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            try
            {
                return await _IdentityManager.DeleteDeep(id);
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
                return await _importService.Processimportexcel(File, "identityAsIs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
