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
using System.Linq;
using System.Threading.Tasks;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.NetworkElementNodeCountManager;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.Infrastucture.QueryResult;
using CAM.Exports;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.Entita.Location;
using static CAM.Enum.ResourceTypeEnum;
using CAM.ResourcesKey;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NetworkElementAsPlannedController : CamControllerBase
    {

        private readonly NetworkElementsAsPlannedManager _manager;
        private readonly NetworkElementNodeCountManager _networkElementNodeCount;
        private readonly ResourceKeyMasterManager _resourceKeyMasterManager;
        private readonly IExportService _exportService;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly ICurrentUserService _currentUserService;
        private readonly List<short> _opcoList;
        private readonly List<int> _verticalList;
        private readonly bool _adminRoleCheck=false;
        public NetworkElementAsPlannedController(ILoggerManager logger, IHttpContextAccessor contextAccessor, IExportService exportService,
            NetworkElementsAsPlannedManager manager, NetworkElementNodeCountManager networkElementNodeCount, ResourceKeyMasterManager resourceKeyMasterManager, ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _networkElementNodeCount = networkElementNodeCount;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails : null;

        }

        [HttpGet(template: "Create")]
        public async Task<NetworkElementAsPlannedDtoCreate> GetCreateResourceLcmengineering( )
        {
            try
            {
                return await _manager.GetCreatePage(_opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "Update{id}")]
        public async Task<NetworkElementAsPlannedDtoUpdate> GetUpdateResourceNetworkElementAsplanned(long id)
        {
            try
            {
                return await _manager.GetUpdatePage(id, _opcoList, _verticalList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpPost("Get")]
        public async Task<QueryResultDto<NetworkElementAsPlannedDtoGrid>> GetNetworkElementAsplannedEntries(
           [FromBody] NetworkElementAsPlannedQueryDto designComponentFilterDto)
        {
            try
            {

                if (designComponentFilterDto.OpCo != null && designComponentFilterDto.OpCo.Count == 0)
                {
                    designComponentFilterDto.OpCo = _opcoList;
                }
                if ((designComponentFilterDto.VerticalName==null) || (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Count == 0))
                {
                    designComponentFilterDto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;
                }
                return await _manager.FindWithCondition(designComponentFilterDto);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet(template: "GetLocationRelatedDeploymentTypes{id}")]
        public async Task<ResultDto<LocationDeploymentTypeRelatedEntity>> GetLocationRelatedDeploymentTypes(int id)
        {
            try
            {
                return await Task.Run(() => _manager.GetLocationDeploymentTypeRelated(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto<LocationDeploymentTypeRelatedEntity>() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] NetworkElementAsPlannedQueryDto dto)
        {
            try
            {


                dto.Page = 0;
                dto.PageSize = 0;
                if (dto.OpCo != null && dto.OpCo.Count() == 0)
                    dto.OpCo = _opcoList;

                if ((dto.VerticalName == null) || (dto.VerticalName != null && dto.VerticalName.Count() ==0))
                    dto.VerticalName = _verticalList != null && _verticalList.Any() == true ? _verticalList.Select(x => x.ToString()).ToList() : null;

                var data = await _manager.FindWithCondition(dto);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = gcs.Key,
                        Data = data.Items.Cast<object>().ToList()
                    });
                foreach (var nome in data.Items)
                {
                    nome.DesignComponent = nome.DesignComponent.Replace("<b class=\"text-lowercase\">", "");
                    nome.DesignComponent = nome.DesignComponent.Replace("<b class=\"text-lowercase\" >", "");
                    nome.DesignComponent = nome.DesignComponent.Replace("</b>", "");
                }

                var reportSheets = tabs.ToList();
                var result = _exportService.GetExcelFrom(reportSheets,
                    "Assets" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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
        public async Task<ResultDto> DeleteDeep(long id, bool onlyPlannedActivities)
        {
            try
            {
                return await _manager.DeleteDeep(id, onlyPlannedActivities);
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
                return await _manager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost]
        public async Task<ResultDto> Create([FromBody] NetworkElementAsPlannedDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _manager.Add(dto, forced);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(NetworkElementAsPlannedDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _manager.Update(dto, forced);
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
                return await _manager.Restore(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "Filter")]
        public async Task<List<FilterValueDto>> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] NetworkElementAsPlannedQueryDto designComponentFilterDto)
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
                var data = await _manager.GetFilter(propertyName, propertyFilter, designComponentFilterDto,_adminRoleCheck);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }

        }

        [HttpGet(template: "GetDesignComponentList")]
        public async Task<ResultDto> GetDesignComponentList(short? oemId)
        {
            try
            {
                return await _manager.GetDesignComponentList(oemId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet(template: "GetNetworkElementassociated")]
        public async Task<IEnumerable<NetworkElementAssociated>> GetNetworkElementassociated(long designComponentId,
            short opcoId)
        {
            try
            {
                return await _networkElementNodeCount.GetNetworkElementassociated(designComponentId, opcoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return null;
            }
        }

        [HttpGet("GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource")]
        public async Task<ResultDto> GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource(short plannedActivityResourceId, short deliveryStatusId, bool isAddAsset)
        {
            try
            {
                return await _manager.GetAssetDeploymentStatusRelatedDeliveryStatusAndPAResource(plannedActivityResourceId, deliveryStatusId, isAddAsset);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpGet("GetAssetsByOpcoIdAndDcfId")]

        public ResultDto GetAssetsByOpcoIdAndDcfId(short opcoId, long dcfId)
        {
            try
            {
                return _manager.GetAssetsByOpcoIdAndDcfId(opcoId, dcfId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost(template: "GetLcmId")]
        public ResultDto GetLCMEngineeringID([FromBody] NetworkElementAsPlannedGetLCMIDQueryDto dto)
        {
            try
            {
                return _manager.GetLCMEngineeringID(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }
        //[HttpPost(template: "GenerateNewAssetRandomResourceKey")]
        //public async Task<(ResultDto, ResultDto)> GenerateNewAssetRandomResourceKey(int opcoId,string elementName)
        //{

        //    //var swResourceKey =  _resourceKeyMasterManager.GenerateResourceKeyForAsset((short)opcoId,  (int)ResourceTypesKey.SWAsset, elementName).Result.Data.ToString();

        //    //var hwResourceKey = _resourceKeyMasterManager.GenerateResourceKeyForAsset((short)opcoId,  (int)ResourceTypesKey.HWAsset, elementName).Result.Data.ToString(); 

        //    var result1 = new ResultDto
        //    {
        //        Info = "Software ResourceKey",
        //        Data = "",
        //    };
        //    var result2 = new ResultDto
        //    {
        //        Info = "Hardware ResourceKey ",
        //        Data = "",
        //    };

        //    return (result1, result2);
        //}


    }
}

