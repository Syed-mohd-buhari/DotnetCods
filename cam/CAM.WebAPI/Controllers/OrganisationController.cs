using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Organisation;
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
    [Route("api/Organisation")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class OrganisationController : CamControllerBase
    {
        private readonly OrganisationManager _manager;
        private readonly IExportService _exportService;
        private readonly CommonManager _commonManager;
        public OrganisationController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService, OrganisationManager manager, CommonManager commonManger) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _commonManager = commonManger;
        }

        [HttpPost("Get")]
        public QueryResultDto<OrganisatioDtoGrid> GetOrganisation([FromBody] OrganisationQueryDto OrganisationQueryDto)
        {
            try
            {
                return _manager.FindWithCondition(OrganisationQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] OrganisationQueryDto OrganisationQueryDto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, OrganisationQueryDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] OrganisationQueryDto dto)
        {
            try
            {


                dto.Page = 0;
                dto.PageSize = 0;
                var data = _manager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "Organisation_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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

        [HttpGet(template: "Create")]
        public async Task<OrganisationCreateDto> GetCreatePage()
        {
            try
            {
                return await _manager.GetCreatepage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetAllOpCosAndVerticalResponsibles")]
        public Dictionary<string, string> GetAllOpCosAndVerticalResponsibles(long id)
        {
            try
            {
                Dictionary<string, string> OpCosAndVerticalResponsible = new Dictionary<string, string>()
            {
                { "OpCos", _commonManager.GetOpcoForOrganisation(id) },
                {"VerticalResponsibles", _commonManager.GetVerticalResponsiblesForOrganisation(id)}
            };

                return OpCosAndVerticalResponsible;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpPost(template: "GetOpCosAndVerticalAndSubDomainResponsibles")]
        public Dictionary<string, string> GetOpCosAndVerticalAndSubDomainResponsibles([FromBody] List<int?> id)
        {
            try
            {
                Dictionary<string, string> OpCosAndVerticalResponsible = new Dictionary<string, string>()
            {
                { "OpCos", _commonManager.GetEduAndSubDomainSpocUsersOpcos(id,false,false) },
                { "VerticalResponsibles", _commonManager.GetVerticaleNameRes(id,(long) 0)},
                { "SubDomainResponsibles", _commonManager.GetSubDomainResponsibleDescription(id,false,false)}
            };

                return OpCosAndVerticalResponsible;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public OrganisationUpdateDto GetUpdatePage(short id)
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

        [HttpPost(template: "Add")]
        public async Task<ResultDto> Create([FromBody] OrganisationCreateDto dto)
        {
            try
            {
                return await _manager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut("Update")]
        public async Task<ResultDto> UpdateOrganisationEnity([FromBody] OrganisationUpdateDto dto)
        {
            try
            {
                return await _manager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(long id, bool IsEduSpoc, bool IsSubDomainSpoc)
        {
            try
            {
                return await _manager.GetRelatedRecords(id, IsEduSpoc, IsSubDomainSpoc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
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

        [HttpDelete(template: "DeleteDeep")]
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




    }
}
