using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.LcmEngineering;
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
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class NFVICompatibilityAtGlanceController : CamControllerBase
    {
        private readonly ICurrentUserService _currentUserService;        
        private readonly NFVICompatibilityAtGlanceManager _nfviAtGlanceManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly IExportService _exportService;
        private readonly bool _adminRoleCheck=false;
        private readonly List<short> _opcoList;
        private readonly List<long> _verticalList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;

        public NFVICompatibilityAtGlanceController(ICurrentUserService currentUserService,
            ILoggerManager logger, IHttpContextAccessor contextAccessor,IExportService exportService,
            NFVICompatibilityAtGlanceManager nfviAtGlanceManager, DropdownDataServiceManager dropdownDataServiceManager,AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {

            _currentUserService = currentUserService;            
            _nfviAtGlanceManager = nfviAtGlanceManager;
            _exportService = exportService;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToInt64(x)).ToList() : null;
        }

        [HttpPost("Get")]

        public async Task<QueryResultDto<NFVICompatibilityAtGlanceDtoGrid>> GetNFVICompatibilityRecords([FromBody] NFVICompatibilityAtGlanceQueryDto nfvidto, long vmVarId)
        {
            try
            {
                if ((nfvidto.Market ==null) || (nfvidto.Market != null && nfvidto.Market.Count == 0))
                {
                    nfvidto.Market = _opcoList;
                }
                if ((nfvidto.Domain == null) || nfvidto.Domain != null && nfvidto.Domain.Count == 0)
                {
                    nfvidto.Domain = _verticalList ;
                }
                return await _nfviAtGlanceManager.FindWithCondition(nfvidto, vmVarId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] NFVICompatibilityAtGlanceQueryDto nfvidto)
        {
            try
            {
                if ((nfvidto.Market == null) || (nfvidto.Market != null && nfvidto.Market.Count == 0))
                {
                    nfvidto.Market = _opcoList;
                }
                if ((nfvidto.Domain == null) || nfvidto.Domain != null && nfvidto.Domain.Count == 0)
                {
                    nfvidto.Domain = _verticalList;
                }
                var data = await _nfviAtGlanceManager.GetFilteredValuesAsync(propertyName, propertyFilter, nfvidto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        [HttpPost("ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] NFVICompatibilityAtGlanceQueryDto dto, long vmVarId)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if ((dto.Market == null) || (dto.Market != null && dto.Market.Count == 0))
                {
                    dto.Market = _opcoList;
                }
                if ((dto.Domain == null) || dto.Domain != null && dto.Domain.Count == 0)
                {
                    dto.Domain = _verticalList;
                }
                var data = await _nfviAtGlanceManager.FindWithCondition(dto,vmVarId,true);

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                var result = _exportService.GetExcelFrom(reportSheets,
                    "NfviCompatibilityAtGlance_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

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


        [HttpPost("GetAllDropDown")]

        public async Task<ResultDto> GetAllVmwareFromMajorSwBuild()
        {
            try
            {
                var vmwareMswPlaftform = await _dropdownDataServiceManager.GetAllVmwareFromNfvi();
                //var opcoDropDown = await _dropdownDataServiceManager.GetAllOpcos();
                //var verticalDropDown = await _dropdownDataServiceManager.GetVerticalFromLCMDto();
                //var oemDropDown = await _dropdownDataServiceManager.GetOemFromMajorSwBuildDto();
                //var vfDropDown = await _dropdownDataServiceManager.GetVFFromSystemTypeDto();
                //var plannedComplentionDropDown = await _dropdownDataServiceManager.GetPlannedCompletionFromLcmDto();
                return new ResultDto
                {
                    Data = new
                    {
                        vmwareMswPlaftform,
                        //opcoDropDown,
                        //verticalDropDown,
                        //oemDropDown,
                        //vfDropDown,
                        //plannedComplentionDropDown
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPost("GetGraphicalReport")]

        public async Task<ResultDto> GetGraphicalReport([FromBody] NFVICompatibilityAtGlanceQueryDto dto, long vmVarId)
        {
            try
            {
                if ((dto.Market == null) || (dto.Market != null && dto.Market.Count == 0))
                {
                    dto.Market = _opcoList;
                }
                if ((dto.Domain == null) || dto.Domain != null && dto.Domain.Count == 0)
                {
                    dto.Domain = _verticalList;
                }
                var reports = await _nfviAtGlanceManager.FindWithConditionForGraphicalReport(dto, vmVarId);
                return new ResultDto
                {
                    Data = reports,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }       

    }
}
