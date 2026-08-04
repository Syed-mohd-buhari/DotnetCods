using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp.DeliveryTracking;
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

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DeliveryTrackingController : CamControllerBase
    {
        private readonly DeliveryTrackingManager _DeliveryTrackingManager;
        
        private readonly IExportService _exportService;
        private new readonly ILoggerManager _logger;
        private readonly IImportService _importService;
        private readonly DeliveryTrackingsImport _deliveryTrackingsImport;
        private readonly ProjectsPlanManager _projectPlanManager;
        private CustomGridRender<ExportService> render;
        private GridCustomColumnManager _manager;
        private readonly ICurrentUserService _currentUserService;
        private readonly bool _adminRoleCheck;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;


        public DeliveryTrackingController(DeliveryTrackingManager DeliveryTrackingManager, ILoggerManager logger, ProjectsPlanManager projectPlanManager, GridCustomColumnManager manager,
            IHttpContextAccessor contextAccessor, DeliveryTrackingsImport deliveryTrackingsImport, IImportService importService,IExportService exportService
            , ICurrentUserService currentUserService, AuthorizedRoleManager authorizedRoleManager) : base(logger, contextAccessor)
        {
            _DeliveryTrackingManager = DeliveryTrackingManager;
            _exportService = exportService;
            _importService = importService;
            _deliveryTrackingsImport = deliveryTrackingsImport;
            _projectPlanManager = projectPlanManager;
            _manager = manager;
            _logger = logger;
            _currentUserService = currentUserService;
            this.sessionUserId = _currentUserService.UserId;
            _authorizedRoleManager = authorizedRoleManager;
            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x=>Convert.ToString(x)).Distinct().ToList():null;
        }

        [HttpGet(template: "Create")]
        public DeliveryTrackingDtoCreate GetCreate()
        {
            try
            {
                return _DeliveryTrackingManager.GetCreatePage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpGet(template: "Update{id}")]
        public DeliveryTrackingDtoUpdate GetUpdate(short id)
        {
            try
            {
                return _DeliveryTrackingManager.GetUpdatePage(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Get")]
        public Task<QueryResultDto<DeliveryTrackingDtoGrid>> GetEntityGrid([FromBody] DeliveryTrackingQueryDto dto)
        {
            try
            {
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                {
                    dto.OpCo = _opcoList;
                }
                if ((dto.VerticalName == null) || dto.VerticalName != null && dto.VerticalName.Count == 0)
                {
                    dto.VerticalName = _verticalList ;
                }
                return _DeliveryTrackingManager.GetEnityGrid(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ExportReport")]
        public async Task<FileContentResult> ExportReport([FromBody] DeliveryTrackingQueryDto dto)
        {
            try
            {
                dto.Page = 0;
                dto.PageSize = 0;
                if ((dto.OpCo == null) || (dto.OpCo != null && dto.OpCo.Count == 0))
                {
                    dto.OpCo = _opcoList;
                }
                if ((dto.VerticalName == null) || dto.VerticalName != null && dto.VerticalName.Count == 0)
                {
                    dto.VerticalName = _verticalList ;
                }
                var data = await _DeliveryTrackingManager.GetEnityGrid(dto);
                render = new CustomGridRender<ExportService>()
                {
                    Render = new List<RenderDetail>()
                };

                ExportSheet dataSheet = new ExportSheet()
                {
                    Data = data.Items.Cast<object>().ToList()
                };
                render.Render.AddRange(data.GridRender.Render);

                List<ExportSheet> reportSheets = new List<ExportSheet>();
                reportSheets.Add(dataSheet);

                List<long> paId = new List<long>();
                if(data.Items.Count > 0)
                {
                    paId = data.Items.Where(x=>x.PlannedActivityId != null).Select(x=>(long)x.PlannedActivityId).ToList();    
                }

                var projectPlansEntity = await _projectPlanManager.FindWithConditionForExport(paId);

                var projectPlansSheet = new ExportSheet()
                {
                    Data = projectPlansEntity.Items.Cast<object>().ToList(),
                    TabName = "ProjectPlan"
                };

                render.Render.AddRange(projectPlansEntity.GridRender.Render);
                reportSheets.Add(projectPlansSheet);

                string IdColumn = _deliveryTrackingsImport.IdColumn;
                List<string> EditableColumnList = _deliveryTrackingsImport.EditableColumnList;

                var result = _exportService.GetExcelFrom(reportSheets,
                    "Delivery-Tracking" + DateTime.Now.ToShortDateString() + ".xlsx", render, IdColumn, EditableColumnList);

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
        public async Task<ResultDto> Create([FromBody] DeliveryTrackingDtoCreate dto, [FromQuery] bool? forced)
        {
            try
            {
                return await _DeliveryTrackingManager.Add(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPut]
        public async Task<ResultDto> Put(DeliveryTrackingDtoUpdate dto, [FromQuery] bool? forced)
        {
            try
            {

                return await _DeliveryTrackingManager.Update(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpDelete(template: "Delete")]
        public async Task<ResultDto> Delete(short id)
        {
            try
            {
                return await _DeliveryTrackingManager.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpDelete(template: "DeleteDeep")]
        public async Task<ResultDto> DeleteDeep(short id)
        {
            try
            {
                return await _DeliveryTrackingManager.DeleteDeep(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpGet(template: "GetRelatedRecords{id}")]
        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            try
            {
                return await _DeliveryTrackingManager.GetRelatedRecords(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }


        [HttpPost(template: "Filter")]
        public Task<List<FilterValueDto>> GetFilterResult([FromQuery]string propertyName, [FromQuery]string propertyFilter, [FromBody] DeliveryTrackingQueryDto deliveryTrackingFilterDto)
        {
            try
            {
                if ((deliveryTrackingFilterDto.OpCo == null) || (deliveryTrackingFilterDto.OpCo != null && deliveryTrackingFilterDto.OpCo.Count == 0))
                {
                    deliveryTrackingFilterDto.OpCo = _opcoList;
                }
                if ((deliveryTrackingFilterDto.VerticalName == null) || deliveryTrackingFilterDto.VerticalName != null && deliveryTrackingFilterDto.VerticalName.Count == 0)
                {
                    deliveryTrackingFilterDto.VerticalName = _verticalList ;
                }
                return _DeliveryTrackingManager.GetFilter(propertyName, propertyFilter, deliveryTrackingFilterDto,_adminRoleCheck);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost(template: "ImportReport")]
        public async  Task<ResultDto> ImportReports()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                IFormFile File = httpRequest.Form.Files.FirstOrDefault();
                return await _importService.Processimportexcel(File, "deliverytrackings");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

        [HttpPost("BulkUpdate")]
        public async Task<ResultDto> BulkUpdate()
        {
            try
            {
                return await _DeliveryTrackingManager.DeliveryTrackingDataRefresh();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }
        }

    }
}
