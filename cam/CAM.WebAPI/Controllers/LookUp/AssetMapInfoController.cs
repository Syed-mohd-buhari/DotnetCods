using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.AssetMapInfo;
using CAM.DataTransferObjects.LookUp.MajorHardwareBuildAsIs;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.FNT;
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

namespace CAM.WebAPI.Controllers.LookUp
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetMapInfoController : CamControllerBase
    {
        private readonly AssetMapInfoManager _assetManager;
        private readonly IExportService _exportService;
        public AssetMapInfoController(AssetMapInfoManager assetManager, IExportService exportService,ILoggerManager logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
        {
            _assetManager=assetManager;
            _exportService = exportService;
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<AssetMapInfoDtoGrid>> GetAssetMapInfo(AssetMapInfoQueryDto majorHardwareBuildAsIsQueryDto)
        {
            try
            {
                return await _assetManager.FindWithCondition(majorHardwareBuildAsIsQueryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching AssetMapInfo data - \n Error Message :{ex}");
                return null;
            }

        }
        [HttpGet(template: "GetCreatePage")]
        public AsseMapInfoCreateDto GetCreatePage()
        {
            try
            {
                return _assetManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching create page AssetMapInfo data - \n Error Message :{ex}");
                return null;
            }

        }
        [HttpGet(template: "GetUpdatePage")]
        public Task<AsseMapInfoCreateDto> GetUpdatePage(long id)
        {
            try
            {
                return _assetManager.GetUpdatedPage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while fetching update page AssetMapInfo data - \n Error Message :{ex}");
                return null;
            }


        }
        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult(string propertyName, string propertyFilter, [FromQuery] AssetMapInfoQueryDto dto)
        {
            try
            {
                var data = _assetManager.GetFilter(propertyName, propertyFilter, dto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while filtering AssetMapInfo data - \n Error Message :{ex}");
                return null;
            }

        }
        [HttpPost(template: "Create")]
        public async Task<ResultDto> Add(AsseMapInfoCreateDto dto)
        {
            try
            {
                var data = _assetManager.Add(dto);
                return await data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while creating AssetMapInfo data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        [HttpPut(template: "Update")]
        public async Task<ResultDto> Update(AsseMapInfoCreateDto dto)
        {
            try
            {
                var data = _assetManager.Update(dto);
                return await data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while updating AssetMapInfo data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            try
            {
                return await _assetManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue happen while deleting AssetMapInfo data - \n Error Message :{ex}");
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] AssetMapInfoQueryDto dto)
        {
            try 
            {
                dto.Page = 0;
                dto.PageSize = 0;
                var data = await _assetManager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                     "AssetMappingInfo_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
                _logger.LogError($"Issue happen while exporting AssetMapInfo data - \n Error Message :{ex}");
                return null;
            }

        }
    }
}
