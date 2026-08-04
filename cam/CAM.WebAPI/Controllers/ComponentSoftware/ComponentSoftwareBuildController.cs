using AutoMapper;
using CAM.BusinessManager.Entity.ComponentSoftware;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.ComponentSoftware;
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

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ComponentSoftwareBuildController : CamControllerBase
    {
        private readonly ComponentSoftwareBuildManager _componentSwBuildManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;

        public ComponentSoftwareBuildController(ComponentSoftwareBuildManager componentSwBuildManager, IMapper mapper, ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {
            _componentSwBuildManager = componentSwBuildManager;
            _exportService = exportService;
            _mapper = mapper;
        }

        [HttpPost("GetComponentSoftwareBuilds")]
        public async Task<QueryResultDto<ComponentSoftwareBuildDtoGrid>> GetComponentSoftwareBuilds([FromBody] ComponentSoftwareBuildQueryDto filterDto)

        {
            try
            {
                return await _componentSwBuildManager.FindWithCondition(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "CreateComponentSwBuild")]
        public async Task<ComponentSoftwareBuildDtoCreate> GetCreateComponentSwBuild()
        {
            try
            {
                return await _componentSwBuildManager.GetCreateSoftwareBuildPageDetailsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "EditComponentSwBuild{id}")]
        public async Task<ComponentSoftwareBuildDtoCreate> GetEditComponentSwBuild(long id)
        {
            try
            {
                return await _componentSwBuildManager.GetEditedSoftwareBuildDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        

        [HttpPost(template: "CreateNewComponentSwBuild")]
        public async Task<ResultDto> Create([FromBody] ComponentSoftwareBuildDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            { 

                var result = await _componentSwBuildManager.AddSoftwareBuildAsync(dto, forced);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut(template: "UpdateComponentSwBuild")]
        public async Task<ResultDto> Put(ComponentSoftwareBuildDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
               
                var result = await _componentSwBuildManager.UpdateSoftwareBuildAsync(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        [HttpPost(template: "FilterComponentSwBuild")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter, 
            [FromBody] ComponentSoftwareBuildQueryDto componetSoftwareBuildFilterDto)
        {
            try
            {
                var data = await _componentSwBuildManager.GetFilteredValuesAsync(propertyName, propertyFilter, componetSoftwareBuildFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetReferenceRecord{id}")]
        public async Task<ResultDto> GetReferenceRecord(long id)
        {
            try
            {
                return await _componentSwBuildManager.GetReferenceRecordAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetComponentSwBuildForClone{id}")]
        public async Task<ResultDto> GetCloneBuildBag(long id)
        {
            try
            {
                return await _componentSwBuildManager.CopyComponentSoftwareBuildToCloneAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "UpgradeClonedComponentSwBuild")]
        public async Task<ResultDto> UpgradeClonedComponentSwBuild([FromBody] CloneComponentSoftwareBuildDto dto)
        {
            try
            {
                return await _componentSwBuildManager.UpgradeClonedComponentSwBuild(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpDelete(template: "DeleteComponentSwBuild")]
        public async Task<ResultDto> DeleteDeep(long id)
        {
            try
            {
                var result = await _componentSwBuildManager.DeleteComponentSwBuild(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

 

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] ComponentSoftwareBuildQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                var data = await _componentSwBuildManager.FindWithCondition(dto);
               
                ExportSheet dataSheet = new ExportSheet()
                {
                  Data =   data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Component-Software-Build" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
  