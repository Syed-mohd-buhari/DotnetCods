using AutoMapper;
using CAM.BusinessManager.Entity.ComponentSoftware;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.ComponentSoftware.MappedComponentBuildBag;
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
    public class ComponentSwBuildBagMappingController : CamControllerBase
    {
        private readonly ComponentSoftwareBuildBagMappingManager _componetSwBuildBagManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;

        public ComponentSwBuildBagMappingController(ComponentSoftwareBuildBagMappingManager componetSwBuildBagManager, IMapper mapper, ILoggerManager logger, 
            IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {           
            _exportService = exportService;
            _mapper = mapper;
            _componetSwBuildBagManager = componetSwBuildBagManager;
        }

        [HttpPost("GetMappingComponentSwBuildBag")]
        public async Task<QueryResultDto<ComponentMappingSWBuildBagGridDto>> GetMappingComponentSwBuildBag([FromBody] ComponentMappingSWBuildBagQueryDto filterDto)

        {
            try
            {
                return await _componetSwBuildBagManager.FindWithCondition(filterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost(template: "FilterMappingComponentSwBuildBag")]
        public async Task<List<FilterValueDto>> GetFilteredValuesAsync([FromQuery] string propertyName, [FromQuery] string propertyFilter,
            [FromBody] ComponentMappingSWBuildBagQueryDto mappingComponetSwBuildFilterDto)
        {
            try
            {
                var data = await _componetSwBuildBagManager.GetFilteredValuesAsync(propertyName, propertyFilter, mappingComponetSwBuildFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "CreateMappingComponentSwBuild")]
        public async Task<ComponentBuilBagCreatePageDto> CreateSwBuildMapBag()
        {
            try
            {
                return await _componetSwBuildBagManager.GetCreateMappingPageDetailsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "EditMappingComponentSwBuild{id}")]
        public async Task<ComponentBuildBagEditPageDto> EditSwBuildMapBag(long id)
        {
            try
            {
                return await _componetSwBuildBagManager.GetEditAndUpgradeMappingPageDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpGet(template: "GetMappingComponentSwBuildForClone{id}")]
        public async Task<ComponentBuildBagEditPageDto> GetComponentSwBuildForClone(long id)
        {
            try
            {
                return await _componetSwBuildBagManager.GetEditAndUpgradeMappingPageDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "UpgradeClonedMapping")]
        public async Task<ResultDto> UpgradeComponentSwBuildBag([FromBody] ComponentBuilBagClonedDto dto)
        {
            try
            { 

                var result = await _componetSwBuildBagManager.UpgradeComponenetSoftwareBuildBagAsync(dto);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] ComponentMappingSWBuildBagQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                var data = await _componetSwBuildBagManager.FindWithCondition(dto);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Componet Software Build Bag" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
  