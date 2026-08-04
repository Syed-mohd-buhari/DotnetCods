using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DaMigrationStatus;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Exports;
using CAM.Imports;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LcmEngineeringController : CamControllerBase
    {

        private readonly LcmEngineeringManager _lcmManager;
        public readonly LcmImport _lcmImport;
        private readonly ResourceKeyMasterManager _resourceKeyMasterManager;
        private readonly IExportService _exportService;
        private readonly IImportService _importService;

        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;
        private readonly bool _adminRoleCheck=false;
        private readonly CommonManager _commonManager;
        public LcmEngineeringController(LcmEngineeringManager designComponentManager, ILoggerManager logger, IHttpContextAccessor contextAccessor,
            IExportService exportService, IImportService importService, LcmImport lcmImport,
            ResourceKeyMasterManager resourceKeyMasterManager, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager, CommonManager commonManager) : base(logger, contextAccessor)
        {
            _lcmManager = designComponentManager;
            _lcmImport = lcmImport;
            _exportService = exportService;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _importService = importService;

            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            adminRoleId = _currentUserService.adminRoleId;
            sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList!=null?_roleOpcoList.IsAdmin:_adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;
            _commonManager = commonManager;
        }


        [HttpGet(template: "Create")]
        public async Task<LcmEngineeringDtoCreate> GetCreateResourceLcmengineering()
        {
            try
            {
                return await _lcmManager.GetCreatePage(_opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public async Task<LcmEngineeringDtoUpdate> GetUpdateResourceLcmengineering(long id)
        {
            try
            {
                return await _lcmManager.GetUpdatePage(id, _opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<LcmEngineeringDtoGrid>> GetLcmengineering(
           [FromBody] LcmEngineeringQueryDto designComponentFilterDto)
        {
            try
            {
                if (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0)
                {
                    designComponentFilterDto.OpCo = _opcoList;
                }
                if ((designComponentFilterDto.VerticalName == null) || (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0))
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }

                return await _lcmManager.FindWithCondition(designComponentFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }


        }

        [HttpGet("GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource")]
        public async Task<ResultDto> GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(short plannedActivityResourceId, short deliveryStatusId)
        {
            try
            {
                return await _lcmManager.GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(plannedActivityResourceId, deliveryStatusId);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("CreateOrUpdateAssetOnLCM")]
        public async Task<ResultDto> CreateOrUpdateAssetOnLCM([FromBody] LcmEngineeringDtoCreate dto)
        {
            try
            {
                return await _lcmManager.GenerateNetworkElementAssociated(dto, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] LcmEngineeringQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                if (dto.OpCo != null && dto.OpCo.Count == 0)
                {
                    dto.OpCo = _opcoList;
                }
                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count == 0))
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }

                QueryResultDto<LcmEngineeringDtoGrid> data = await _lcmManager.FindWithCondition(dto);
                IEnumerable<ExportSheet> tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                foreach (LcmEngineeringDtoGrid nome in data.Items)
                {
                    nome.DesignComponent = nome.DesignComponent.Replace("<b class=\"text-lowercase\">", "");
                    nome.DesignComponent = nome.DesignComponent.Replace("<b class=\"text-lowercase\" >", "");
                    nome.DesignComponent = nome.DesignComponent.Replace("</b>", "");


                }
                List<ExportSheet> reportSheets = tabs.ToList();
                string IdColumn = _lcmImport.IdColumn;
                List<string> EditableColumnList = _lcmImport.EditableColumnList;

                ExportResult result = _exportService.GetExcelFrom(reportSheets,
                    "Lcm-Engineering" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender, IdColumn, EditableColumnList);

                HttpContext.Response.ContentType = result.ContentType;
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                FileContentResult fileContentResult = new(result.FileInByteArray, result.ContentType)
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

        [HttpPost(template: "ExportArchivedLcmReport")]
        public async Task<FileContentResult> ExportArchivedLcmReport([FromBody] ArchivedLcmEngineeringQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;

                if (dto.OpCo != null && dto.OpCo.Count == 0)
                {
                    dto.OpCo = _opcoList;
                }
                if ((dto.VerticalName == null) || dto.VerticalName != null && dto.VerticalName.Count == 0)
                {
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }

                QueryResultDto<ArchivedLcmengineeringDtoGrid> data = await _lcmManager.ArchivedLcmFindWithConditionAsync(dto);
                IEnumerable<ExportSheet> tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });

                foreach (ArchivedLcmengineeringDtoGrid nome in data.Items)
                {
                    nome.DesignComponent = nome.DesignComponent.Replace("<b class=\"text-lowercase\">", "");
                    nome.DesignComponent = nome.DesignComponent.Replace("<b class=\"text-lowercase\" >", "");
                    nome.DesignComponent = nome.DesignComponent.Replace("</b>", "");
                }
                List<ExportSheet> reportSheets = tabs.ToList();
                ExportResult result = _exportService.GetExcelFrom(reportSheets,
                    "Archived-Lcm-Engineering" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);

                HttpContext.Response.ContentType = result.ContentType;
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                FileContentResult fileContentResult = new(result.FileInByteArray, result.ContentType)
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

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(long id)
        {
            try
            {
                return await _lcmManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(long id, bool onlyPlannedActivities)
        {
            try
            {
                return await _lcmManager.DeleteDeep(id, onlyPlannedActivities);
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
                return await _lcmManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] LcmEngineeringDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                if (dto.NumberOfNodes < 0 || dto.NumberOfNodesInLab < 0)
                {
                    return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
                }

                return dto.Warranty && !dto.SoftwareEndOfWarrantyDate.HasValue
                    ? new ResultDto() { Info = ResultMessages.SystemError, Warning = true }
                    : await _lcmManager.Add(dto, forced);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(LcmEngineeringDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                return dto.Warranty && !dto.SoftwareEndOfWarrantyDate.HasValue
               ? new ResultDto() { Info = ResultMessages.SystemError, Warning = true }
               : await _lcmManager.Update(dto, forced);
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
                return await _lcmManager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] LcmEngineeringQueryDto designComponentFilterDto)
        {
            try
            {
                if (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0)
                {
                    designComponentFilterDto.OpCo = _opcoList;
                }
                if ((designComponentFilterDto.VerticalName == null) || (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0))
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }

                List<FilterValueDto> data = await _lcmManager.GetFilter(propertyName, propertyFilter, designComponentFilterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpGet("MajorSoftwareBuildEoS")]
        public async Task<ResultDto> MajorSoftwareBuildEoS(int designComponentId)
        {

            try
            {
                return new ResultDto()
                {
                    Data = await _lcmManager.GetMajorSoftwareBuildEosMinorToDaydesignComponentId(designComponentId),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet("MajorHardwareBuildEoS")]
        public async Task<ResultDto> MajorHardwareBuildEoS(int designComponentId)
        {
            try
            {
                return new ResultDto()
                {
                    Data = await _lcmManager.GetMajorHarwareBuildEosMinorToDaydesignComponentId(designComponentId),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        [HttpPut(template: "ApplyDataRemediation")]
        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation([FromBody] DataRemediationDto data)
        {
            try
            {
                return await _lcmManager.ApplyDataRemediation(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ArchivedLcmFilter")]
        public async Task<List<FilterValueDto>> ArchivedLcmFilter([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] ArchivedLcmEngineeringQueryDto designComponentFilterDto)
        {
            try
            {

                if (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0)
                {
                    designComponentFilterDto.OpCo = _opcoList;
                }
                if ((designComponentFilterDto.VerticalName == null) || (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0))
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }

                List<FilterValueDto> data = await _lcmManager.ArchivedLcmFilter(propertyName, propertyFilter, designComponentFilterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetArchivedLCM")]
        public async Task<QueryResultDto<ArchivedLcmengineeringDtoGrid>> GetArchivedLCM(
           [FromBody] ArchivedLcmEngineeringQueryDto designComponentFilterDto)
        {
            try
            {
                if (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0)
                {
                    designComponentFilterDto.OpCo = _opcoList;
                }
                if ((designComponentFilterDto.VerticalName == null) || designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0)
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }

                return await _lcmManager.ArchivedLcmFindWithConditionAsync(designComponentFilterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }


        }

        [HttpPut(template: "PlannedActivityMigrations")]
        public async Task<ResultDto> PlannedActivityMigrations([FromBody] PlannedActivityMigrationsDto data)
        {
            try
            {
                return await _lcmManager.PlannedActivityMigrations(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }


        }

        [HttpPost(template: "GenerateNewLcmRandomResourceKey")]
        public string GenerateNewLcmRandomResourceKey(short opcoId, int designComponentId)
        {
            try
            {
                int lcmId = (int)ResourceTypesKey.Lcm;
                string Resourcekey = _resourceKeyMasterManager.GenerateResourceKey(opcoId, designComponentId, lcmId);
                return Resourcekey;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ImportReport")]
        public async Task<ResultDto> ImportReports()
        {
            try
            {
                HttpRequest httpRequest = HttpContext.Request;
                IFormFile File = httpRequest.Form.Files.FirstOrDefault();
                return await _importService.Processimportexcel(File, "lcm");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }


        [HttpPost("CreateUnkownDesignComponentBasedOnDCF")]
        public async Task<ResultDto> CreateUnkownDesignComponentBasedOnDCF(long dcfid)
        {
            try
            {
                return await _lcmManager.CreateUnkownDesignComponentBasedOnDCF(dcfid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("CreateUnkownDCPALevel")]
        public async Task<ResultDto> CreateUnkownDesignComponentPALevel(long dcid)
        {
            try
            {
                return await _lcmManager.CreateUnkownDesignComponentPALevel(dcid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetAllSubdomainAndVerticalResponsibles")]
        public Dictionary<string, string> GetAllSubdomainAndVerticalResponsibles(List<int?> eduSpocId)
        {
            try
            {
                Dictionary<string, string> SubdomainAndVerticalResponsible = new()
            {
                {"VerticalResponsibles", _commonManager.GetVerticaleNameRes(eduSpocId,0)}
            };

                return SubdomainAndVerticalResponsible;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }


        }

        #region Platformation Migration - Da Asset Migratio
        [HttpGet("GetUpdateDaMigrationRecords")]
        public async Task<ResultDto> GetUpdateDaMigrationRecords(long paId,  long plannedDcfId,long currentDcfId)

        {
            try
            {
                return await _lcmManager.CreateLcmAndLcmPaForPlatformationMigration(paId, plannedDcfId, currentDcfId, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        #endregion

    }
}

