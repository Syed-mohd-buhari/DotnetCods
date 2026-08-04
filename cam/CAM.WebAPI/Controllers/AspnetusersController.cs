using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AspNetUser;
using CAM.DataTransferObjects.Entita.AspNetUserRole;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.WebAPI.Identity;
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

    public class AspnetusersController : CamControllerBase
    {
        private readonly AspnetusersManager _manager;
        private readonly IExportService _exportService;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;
        private readonly List<int> _subdomainList;
        private readonly bool _adminRoleCheck = false;
        private readonly bool _managerRoleCheck = false;
        public AspnetusersController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, AspnetusersManager manager, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;

            _currentUserService = currentUserService;
            adminRoleId = _currentUserService.adminRoleId;
            sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;

            /*var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _managerRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsManager : _managerRoleCheck;
            _opcoList = (_adminRoleCheck == true ) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;
            _subdomainList = (_adminRoleCheck == true) ? null : _roleOpcoList.SubdomainDetails != null ? new List<int> {(int) _roleOpcoList.SubdomainDetails } : null;
               */
            
        }

        [HttpPost("Get")]

        public QueryResultDto<AspnetuserGridDto> GetAspnetuserrole([FromBody] AspnetuserroleQueryDto aspNetUserRoleDto)
        {
            try
            {
                if (aspNetUserRoleDto.OpcoId != null && aspNetUserRoleDto.OpcoId.Count == 0)
                {
                    aspNetUserRoleDto.OpcoId = _opcoList;
                }
                if ((aspNetUserRoleDto.VerticalResponsible == null) || (aspNetUserRoleDto.VerticalResponsible != null && aspNetUserRoleDto.VerticalResponsible.Count == 0))
                {
                    aspNetUserRoleDto.VerticalResponsible = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                if((aspNetUserRoleDto.SubdomainResponsible == null) || (aspNetUserRoleDto.SubdomainResponsible != null && aspNetUserRoleDto.SubdomainResponsible.Count == 0))
                {
                    aspNetUserRoleDto.SubdomainResponsible = _subdomainList; 
                }
                
                return _manager.FindWithCondition(aspNetUserRoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] AspnetuserroleQueryDto aspNetUserRoleDto)
        {
            try
            {
                if (aspNetUserRoleDto.OpcoId != null && aspNetUserRoleDto.OpcoId.Count == 0)
                {
                    aspNetUserRoleDto.OpcoId = _opcoList;
                }
                if ((aspNetUserRoleDto.VerticalResponsible == null) || (aspNetUserRoleDto.VerticalResponsible != null && aspNetUserRoleDto.VerticalResponsible.Count == 0))
                {
                    aspNetUserRoleDto.VerticalResponsible = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                if ((aspNetUserRoleDto.SubdomainResponsible == null) || (aspNetUserRoleDto.SubdomainResponsible != null && aspNetUserRoleDto.SubdomainResponsible.Count == 0))
                {
                    aspNetUserRoleDto.SubdomainResponsible = _subdomainList;
                }
                return _manager.GetFilter(propertyName, propertyFilter, aspNetUserRoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] AspnetuserroleQueryDto dto)
        {
            try
            {
                if (dto.OpcoId != null && dto.OpcoId.Count == 0)
                {
                    dto.OpcoId = _opcoList;
                }
                if ((dto.VerticalResponsible == null) || (dto.VerticalResponsible != null && dto.VerticalResponsible.Count == 0))
                {
                    dto.VerticalResponsible = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                if ((dto.SubdomainResponsible == null) || (dto.SubdomainResponsible != null && dto.SubdomainResponsible.Count == 0))
                {
                    dto.SubdomainResponsible = _subdomainList;
                }
                dto.Page = 0;
                dto.PageSize = 0;
                var data = _manager.GetUserRoles(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "Users_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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

        [HttpPut("Useractivation")]
        public async Task<ResultDto> Deactivate(AspnetuserroleGridDto dto)
        {
            try
            {
                return await _manager.Deactivate(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost("CreateUser")]
        public async Task<ResultDto> CreateUser([FromBody] List<AspNetUserRoleUpdateDto> aspNetUserRoleDto)
        {
            try
            {
                return await _manager.CreateUser(aspNetUserRoleDto);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
    }
}
