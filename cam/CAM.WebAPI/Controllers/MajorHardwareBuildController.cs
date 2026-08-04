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
using System.Threading.Tasks;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using System.Linq;
using CAM.BusinessManager.Entity;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using System.Globalization;
using AutoMapper;
using CAM.WebAPI.Helper;
using Microsoft.IO;
using System.IO;
using DocumentFormat.OpenXml.InkML;
using System.Text;
using NuGet.Protocol;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MajorHardwareBuildController : CamControllerBase
    {
        private readonly MajorHardwareBuildManager _majorHardwareBuildManager;
        
        private readonly IExportService _exportService;
        private readonly ILoggerManager _logger;

        public MajorHardwareBuildController(MajorHardwareBuildManager majorHardwareBuildManager, ILoggerManager logger, 
            IHttpContextAccessor contextAccessor, IExportService exportService) : base(logger, contextAccessor)
        {
            _majorHardwareBuildManager = majorHardwareBuildManager;
            _exportService = exportService;
        }

        [HttpGet(template: "Create")]
        public async Task<MajorHardwareBuildDtoCreate> GetCreateResourceMajorHardwareBuild()
        {
            try
            {
               
                return await _majorHardwareBuildManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public async Task<MajorHardwareBuildDtoUpdate> GetUpdateResourceMajorHardwareBuild(long id)
        {
            try
            {
                return await _majorHardwareBuildManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Get")]
        public QueryResultDto<MajorHardwareBuildDtoGrid> GetMajorHardwareBuild([FromBody] MajorHardwareBuildQueryDto majorHardwareBuildFilterDto)
        {
            try
            {
                var result = _majorHardwareBuildManager.FindWithCondition(majorHardwareBuildFilterDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] MajorHardwareBuildQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                var data = _majorHardwareBuildManager.FindWithCondition(dto);
              
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                foreach (var nome in data.Items)
                {
                    nome.HardwareType = nome.HardwareType.Replace("<b class=\"text-lowercase\">", "");
                    nome.HardwareType = nome.HardwareType.Replace("<b class=\"text-lowercase\" >", "");
                    nome.HardwareType = nome.HardwareType.Replace("</b>", "");
                }

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Major-Hardware-Build" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] MajorHardwareBuildDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                //if (dto.EndOfMaintenance == null || dto.EndOfMaintenance == DateTime.MinValue)
                //    throw new Exception();

                var result = await _majorHardwareBuildManager.Add(dto, forced);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut]
        public async Task<ResultDto> Put(MajorHardwareBuildDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _majorHardwareBuildManager.Update(dto, forced);
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
                return await _majorHardwareBuildManager.Restore(id);
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
                return await _majorHardwareBuildManager.Delete(id);
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
                return await _majorHardwareBuildManager.DeleteDeep(id);
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
                return await _majorHardwareBuildManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery]string propertyName, [FromQuery]string propertyFilter, [FromBody] MajorHardwareBuildQueryDto majorHardwareBuildFilterDto)
        {
            try
            {
                var data = _majorHardwareBuildManager.GetFilter(propertyName, propertyFilter, majorHardwareBuildFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpPut(template: "ApplyDataRemediation")]
        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation([FromBody] DataRemediationDto data)
        {
            try
            {
                
                return await _majorHardwareBuildManager.ApplyDataRemediation(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

       


    }
}
