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
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using AutoMapper;

namespace CAM.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MajorSoftwareBuildController : CamControllerBase
    {
        private readonly MajorSoftwareBuildManager _majorSoftwareBuildManager;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public MajorSoftwareBuildController(MajorSoftwareBuildManager majorSoftwareBuildManager,IMapper mapper, ILoggerManager logger, 
            IHttpContextAccessor contextAccessor, IExportService exportService, ICurrentUserService currentUserService) : base(logger, contextAccessor)
        {
            _majorSoftwareBuildManager = majorSoftwareBuildManager;
            _exportService = exportService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        [HttpGet(template: "Create")]
        public async Task<MajorSoftwareBuildDtoCreate> GetCreateResourceMajorSoftwareBuild()
        {
            try
            {
                return await _majorSoftwareBuildManager.GetCreatePage(_currentUserService.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public async Task<MajorSoftwareBuildDtoUpdate> GetUpdateResourceMajorSoftwareBuild(long id)
        {
            try
            {
                return await _majorSoftwareBuildManager.GetUpdatePage(id, _currentUserService.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Get")]
        public QueryResultDto<MajorSoftwareBuildDtoGrid> GetMajorSoftwareBuild(
           [FromBody] MajorSoftwareBuildQueryDto majorSoftwareBuildFilterDto)
        {
            try
            {
                var result = _majorSoftwareBuildManager.FindWithCondition(majorSoftwareBuildFilterDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost(template: "ExportReport")]
        public FileContentResult ExportReport([FromBody] MajorSoftwareBuildQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                var data = _majorSoftwareBuildManager.FindWithCondition(dto);
                //List<MajorSoftwareBuildDtoExportSheet> lst = new List<MajorSoftwareBuildDtoExportSheet>();
                
                //foreach (var item in data.Items)
                //{
                //    lst.Add(_mapper.Map<MajorSoftwareBuildDtoExportSheet>(item));
                //}
                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Major-Software-Build" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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
        public async Task<ResultDto> Create([FromBody] MajorSoftwareBuildDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                //if (dto.EndOfMaintenance == null || dto.EndOfMaintenance == DateTime.MinValue)
                //    throw new Exception();

                var result = await _majorSoftwareBuildManager.Add(dto, forced);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(MajorSoftwareBuildDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                //if (dto.EndOfMaintenance == null || dto.EndOfMaintenance == DateTime.MinValue)
                //    throw new Exception();

                var result = await _majorSoftwareBuildManager.Update(dto, forced);
                return result;
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
                return await _majorSoftwareBuildManager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template:"Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                var result = await _majorSoftwareBuildManager.Delete(id);
                return result;
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
                var result = await _majorSoftwareBuildManager.DeleteDeep(id);
                return result;
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
                return await _majorSoftwareBuildManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpDelete(template: "DeleteOrphans")]
        public async Task<ResultDto> DeleteOrphans()
        {
            try
            {
                return new ResultDto();
                //var result = await _majorSoftwareBuildManager.Delete();
                //return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] MajorSoftwareBuildQueryDto majorSoftwareBuildFilterDto)
        {
            try
            {
                var data = _majorSoftwareBuildManager.GetFilter(propertyName, propertyFilter, majorSoftwareBuildFilterDto);
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
                return await _majorSoftwareBuildManager.ApplyDataRemediation(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "GetMajorSoftwareBuild{id}")]
        public async Task<ResultDto<MajorSoftwareBuildToCloneDto>>  GetMajorSoftwareBuildToClone(long id)
        {
            try
            {
                return await _majorSoftwareBuildManager.GetMajorSoftwareBuildToClone(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<MajorSoftwareBuildToCloneDto>() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        //Ticket 744 - Add New SW build - the popup should be shown to display the libraries that is going to be created
        [HttpGet(template: "GetInfoMajorSoftwareBuildToClone{id}/{referenceSW}")]
        public async Task<ResultDto> GetInfoMajorSoftwareBuildToClone(long id,bool? referenceSW = false)
        {
            try
            {
                return await _majorSoftwareBuildManager.GetInfoMajorSoftwareBuildToClone(id, referenceSW);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "CloneMajorSoftwareBuild")]
        public async Task<ResultDto> CloneMajorSoftwareBuild([FromBody] CloneMajorSoftwareBuildDto dto)
        {
            try
            {
                return await _majorSoftwareBuildManager.CloneMajorSoftwareBuild(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }


        #region Ticket 642 - Add - Software Upgrade Utility to be enhanced to cover all required design components, System Types and Sub network boundary is created 
        [HttpPost("GetSystemTypeForAddMajorSW")]
        public async Task<ResultDto> GetSystemTypeForAddMajorSW([FromBody] MajorSoftwareBuildDtoCreate dto)
        {
            try
            {
              
                var result = await _majorSoftwareBuildManager.GetMSWListBasedOnProductNameAndOEM((short)dto.OriginalEquipmentManufacturerId, (short)dto.ProductNameId);
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        #endregion

        #region New portal changes
        [HttpPost(template: "GetProductBasedSofware")]
        public async Task<ResultDto> GetProductIdBasedLinkedRecords([FromBody] ProductBasedSwQueryDto majorSoftwareBuildFilterDto)
        {
            try
            {
                return await _majorSoftwareBuildManager.GetProductIdBasedLinkedRecords(majorSoftwareBuildFilterDto, _currentUserService.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }


        #endregion
    }
}
