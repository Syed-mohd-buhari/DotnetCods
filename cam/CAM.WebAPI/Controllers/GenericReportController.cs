using Microsoft.AspNetCore.Authorization;
using CAM.Contracts;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Exports;
using CAM.Infrastucture.QueryResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CAM.DataTransferObjects;
using OracleModels.DBContext;
using CAM.Infrastucture;
using CAM.BusinessManager.GenericReports;
using CAM.DataTransferObjects.GenericReportDto;
using CAM.DataTransferObjects.Entita.GenericReportDto;
using CAM.BusinessManager.Entity;
using System.Text;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

#if DEBUG
    [Authorize]
#else
    [Authorize]
#endif

    public class GenericReportController : CamControllerBase
    {
        private readonly GenericReportManager _manager;
        private readonly IExportService _exportService;
        private readonly ModelContext _context;
        private readonly GenericReportGenration _reportGenration;
        private readonly AuthorizedRoleManager _authorizedRoleManager;
        private readonly long sessionUserId;
        private readonly int adminRoleId;
        private readonly bool _adminRoleCheck=false;
        private readonly List<short> _opcoList;
        private readonly List<string> _verticalList;
        private readonly ICurrentUserService _currentUserService;
        private readonly CommonManager _commonManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;

        public GenericReportController(ILoggerManager logger, IHttpContextAccessor contextAccessor,
            ModelContext context, IExportService exportService, GenericReportManager manager,
            GenericReportGenration reportGenration, AuthorizedRoleManager authorizedRoleManager
            , ICurrentUserService currentUserService
            , CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager) : base(logger, contextAccessor)
        {
            _exportService = exportService;
            _manager = manager;
            _context = context;
            _reportGenration = reportGenration;
            _currentUserService = currentUserService;
            _authorizedRoleManager = authorizedRoleManager;
            this.adminRoleId = _currentUserService.adminRoleId;
            this.sessionUserId = _currentUserService.UserId;

            var _roleOpcoList = _authorizedRoleManager.GetUserRelevantOpcoVerticalDetails(this.sessionUserId);
            _adminRoleCheck = _roleOpcoList != null ? _roleOpcoList.IsAdmin : _adminRoleCheck;
            _opcoList = (_adminRoleCheck == true) ? null : _roleOpcoList.OpcoDetails != null && _roleOpcoList.OpcoDetails.Any() == true ?
                _roleOpcoList.OpcoDetails.Select(x => Convert.ToInt16(x)).Distinct().ToList() : null;
            _verticalList = (_adminRoleCheck == true) ? null : _roleOpcoList.VerticalDetails != null && _roleOpcoList.VerticalDetails.Any() == true ?
                _roleOpcoList.VerticalDetails.Select(x => x.ToString()).ToList() : null;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
        }


        //[HttpGet("Geaa")]
        //public Dictionary<string, List<GenericReportInfrastructureDto>> GetTableColumnsOld()
        //{
        //    List<System.Type> entityTypes = new List<System.Type>
        //    {
        //        typeof(LCMEngineering),
        //        typeof(Assets),
        //        typeof(PlannedActivities),
        //        typeof(SystemTypes),
        //        typeof(DataTransferObjects.Entita.GenericReportDto.AssetCategories),
        //        typeof(SubNetWorkBoundaries),
        //        typeof(DataTransferObjects.Entita.GenericReportDto.ProductImportances),
        //        typeof(MajorSoftWareBuilds),
        //        typeof(MajorHardWareBuilds),
        //        typeof(DataTransferObjects.Entita.GenericReportDto.Risk),
        //        typeof(DataTransferObjects.Entita.GenericReportDto.LcmAncillaryData),
        //        typeof(DataTransferObjects.Entita.GenericReportDto.DesignAspects),
        //        typeof(DataTransferObjects.Entita.GenericReportDto.DesignComponent),
        //        typeof(DataTransferObjects.Entita.GenericReportDto.DesignComponentFamily)
        //    };
        //    var excludedProperties = new List<string> { "Creationuser", "Creationdate", "Modificationuser", "Modificationdate", "Deleted", "Deletiondate" };
        //    Dictionary<long, string> fetchAllOpcosList = _commonManager.GetAllOpcos().Result;

        //    return _reportGenration.GetTableColumns(entityTypes, excludedProperties);

        //}

        [HttpGet("Get")]
        public ResultDto GetTableColumns()
        {
            try
            {
                List<System.Type> entityTypes = new List<System.Type>
            {
                typeof(LCMEngineering),
                typeof(Assets),
                typeof(PlannedActivities),
                typeof(SystemTypes),
                typeof(DataTransferObjects.Entita.GenericReportDto.AssetCategories),
                typeof(SubNetWorkBoundaries),
                typeof(DataTransferObjects.Entita.GenericReportDto.ProductImportances),
                typeof(MajorSoftWareBuilds),
                typeof(MajorHardWareBuilds),
                typeof(DataTransferObjects.Entita.GenericReportDto.Risk),
                typeof(DataTransferObjects.Entita.GenericReportDto.LcmAncillaryData),
                typeof(DataTransferObjects.Entita.GenericReportDto.DesignAspects),
                typeof(DataTransferObjects.Entita.GenericReportDto.DesignComponent),
                typeof(DataTransferObjects.Entita.GenericReportDto.DesignComponentFamily),
                typeof(SystemVerificationProblems)
            };
                var excludedProperties = new List<string> { "Creationuser", "Creationdate", "Modificationuser", "Modificationdate", "Deleted", "Deletiondate" };
                Dictionary<long, string> fetchAllOpcosList =  _dropdownDataServiceManager.GetAllOpcos(_opcoList).Result;

                var returnValue = _reportGenration.GetTableColumns(entityTypes, excludedProperties);

                return new ResultDto
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        userReportTablesAndProperties = returnValue,
                        getAllOpcos = fetchAllOpcosList
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }


        [HttpPost("GetData")]
        public GenericQueryResultInfrastructureDto<GenericReportDtoGrid> GetSelectedColumns([FromBody] GenericReportSave dynamicReporGrid)
        {
            try
            {
                if ((dynamicReporGrid.Opco == null || (dynamicReporGrid.Opco != null && dynamicReporGrid.Opco.Count() == 0))
              && (dynamicReporGrid.Opcoid == null || dynamicReporGrid.Opcoid.Count() == 0))
                    dynamicReporGrid.Opcoid = _opcoList;

                //if (dynamicReporGrid.Verticalresponsible == null   || (dynamicReporGrid.Verticalresponsible != null && dynamicReporGrid.Verticalresponsible.Count() == 0))
                //    dynamicReporGrid.VerticalNameId = _verticalList;

                return _reportGenration.GetReportSelectedColumns(dynamicReporGrid, false);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetUserDefinedReport")]
        public GenericQueryResultInfrastructureDto<GenericReportDtoGrid> GetUserDefinedReport([FromBody] GenericReportSave dynamicReporGrid)
        {
            try
            {
                if ((dynamicReporGrid.Opco == null) || (dynamicReporGrid.Opco != null && dynamicReporGrid.Opco.Count() == 0))
                    dynamicReporGrid.Opcoid = _opcoList;

                if (dynamicReporGrid.Verticalresponsible == null || (dynamicReporGrid.Verticalresponsible != null && dynamicReporGrid.Verticalresponsible.Count() == 0))
                    dynamicReporGrid.Verticalresponsible = _verticalList;

                return _reportGenration.GetSelectedColumns(dynamicReporGrid, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPost("Save")]
        public ResultDto Save([FromBody] GenericReportSave dynamicReporGrid)
        {
            try
            {
                var opcoIds = string.Empty;
                if(dynamicReporGrid != null)
                {
                    if (dynamicReporGrid.TsrOpcoId != null && dynamicReporGrid.TsrOpcoId.Count > 0 )
                    {
                          opcoIds  = string.Join (",", dynamicReporGrid.TsrOpcoId);
                    }
                }
                if (dynamicReporGrid.DynamicReportId.Count > 0)
                {
                    _manager.SaveReport(dynamicReporGrid.ReportName[0], dynamicReporGrid.DynamicReportId[0], dynamicReporGrid.Render, 
                        dynamicReporGrid.Published[0], opcoIds, dynamicReporGrid?.ExportFilePath,dynamicReporGrid?.ExportFileFormat,  dynamicReporGrid?.ExportType ,  dynamicReporGrid?.ScheduledDate, dynamicReporGrid.isExportReport, dynamicReporGrid?.ScheduledDayInWeek, dynamicReporGrid?.ScheduledType, dynamicReporGrid.IsTestNodeRequired);
                }
                else
                {
                    _manager.SaveReport(dynamicReporGrid.ReportName[0], dynamicReporGrid.Render,
                        dynamicReporGrid.Published[0], opcoIds, dynamicReporGrid?.ExportFilePath, dynamicReporGrid?.ExportFileFormat, dynamicReporGrid?.ExportType , dynamicReporGrid?.ScheduledDate, dynamicReporGrid.isExportReport, dynamicReporGrid?.ScheduledDayInWeek, dynamicReporGrid?.ScheduledType, dynamicReporGrid.IsTestNodeRequired);

                }
                return new ResultDto()
                {
                    Warning = false,
                    Info = ResultMessages.ConfigurationGridSuccess
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpDelete("Delete")]
        public ResultDto DeleteReport([FromBody] GenericReportSave dynamicReporGrid)
        {
            try
            {
                _manager.DeleteReport(dynamicReporGrid.DynamicReportId[0]);
                return new ResultDto()
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteSuccess
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }

        }

        [HttpPut("Update")]
        public ResultDto UpdateReport([FromBody] GenericReportSave dynamicReporGrid)
        {
            try
            {
                _manager.UpdateReport(dynamicReporGrid.DynamicReportId[0]);
                return new ResultDto()
                {
                    Warning = true,
                    Info = ResultMessages.ConfigurationGridSuccess
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("GetReport")]
        public QueryResultDto<GenericReportGridCreateDto> GetReport([FromBody] GenericReportQueryDto DynamicReportDto)
        {
            try
            {
                return _manager.FindWithCondition(DynamicReportDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("Filter")]
        public List<FilterValueDto> GetFilterResult([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] GenericReportQueryDto DynamicReportFilterDto)
        {
            try
            {
                var data = _manager.GetFilter(propertyName, propertyFilter, DynamicReportFilterDto);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("DynamicReportFilter")]
        public List<FilterValueDto> GetDynamicReporFilter([FromQuery] string propertyName, [FromQuery] string propertyFilter, [FromBody] GenericReportSave dto)
        {
            try
            {
                if ((dto.Opco == null || (dto.Opco != null && dto.Opco.Count() == 0))
                                && (dto.Opcoid == null || dto.Opcoid.Count() == 0))
                    dto.Opcoid = _opcoList;

                if (dto.Verticalresponsible == null || (dto.Verticalresponsible != null && dto.Verticalresponsible.Count() == 0))
                    dto.Verticalresponsible = _verticalList;

                return _reportGenration.GetDynamicReporFilter(propertyName, propertyFilter, dto,_adminRoleCheck);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("ExportReport")]
        public FileContentResult ExportReport([FromBody] GenericReportSave dynamicReporGrid)
        {
            try
            {
                if ((dynamicReporGrid.Opco == null || (dynamicReporGrid.Opco != null && dynamicReporGrid.Opco.Count() == 0))
                             && (dynamicReporGrid.Opcoid == null || dynamicReporGrid.Opcoid.Count() == 0))
                    dynamicReporGrid.Opcoid = _opcoList;

                if (dynamicReporGrid.Verticalresponsible == null || (dynamicReporGrid.Verticalresponsible != null && dynamicReporGrid.Verticalresponsible.Count() == 0))
                    dynamicReporGrid.Verticalresponsible = _verticalList;


                dynamicReporGrid.Page = 0;
                dynamicReporGrid.PageSize = 0;
                var data = _reportGenration.GetSelectedColumns(dynamicReporGrid, true);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
                    .Select(gcs => new ExportSheet()
                    {
                        TabName = data.GridRender.ReportName.ToString(),
                        Data = data.Items.Cast<object>().ToList()
                    });
                var reportSheets = tabs.ToList();
                //Oct 1st Client Request 
                string suffixFileName = dynamicReporGrid?.ViewMode.ToString();

                var result = _exportService.GetExcelFrom(reportSheets,
                   suffixFileName + "_" + data.GridRender.ReportName.ToString() + "_" + DateTime.Now.ToShortDateString() + ".xlsx", data.GridRender);
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

        #region Ticket 730 - TSR Report Download Automation
        [HttpGet("GetSchedulerReportDetails")]
        public List<GenericReportSchedulerDto> GetSchedulerReportDetails()
        {
            try
            {
                return _manager.GetSchedulerReportDetails();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }
        #endregion

        [HttpPost(template: "ExportDynamicReportCSV")]
        public FileContentResult ExportDynamicReportCSV([FromBody] GenericReportSave dynamicReporGrid)
        {
            try
            {
                 if (dynamicReporGrid.Opcoid != null && dynamicReporGrid.Opcoid.Count() == 0  )
                   dynamicReporGrid.Opcoid = _opcoList;

                if (dynamicReporGrid.Verticalresponsible == null   || (dynamicReporGrid.Verticalresponsible != null && dynamicReporGrid.Verticalresponsible.Count() == 0))
                    dynamicReporGrid.Verticalresponsible = _verticalList;

                dynamicReporGrid.Page = 0;
                var data = _reportGenration.GetSelectedColumns(dynamicReporGrid, true);
                var tabs = data.GridRender.Render.GroupBy(x => x.Tab)
               .Select(gcs => new ExportSheet()
               {
                   TabName = data.GridRender.ReportName.ToString(),
                   Data = data.Items.Cast<object>().ToList()
               });
                var reportSheets = tabs.ToList();

                //Oct 1st Client Request 
                string suffixFileName = dynamicReporGrid?.ViewMode.ToString();

                var fileName = suffixFileName + "_" + data.GridRender.ReportName.ToString() + "_" + DateTime.Now.ToShortDateString() + ".csv";
                var result = _exportService.GetExcelFromCSV(reportSheets, fileName, data.GridRender);

                var file = Encoding.ASCII.GetBytes(result.Stringbuilder.ToString());
                // return File(file, "text/csv", fileName);
                HttpContext.Response.ContentType = result.ContentType;
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                var fileContentResult = new FileContentResult(file, "text/csv")
                {
                    FileDownloadName = fileName
                };


                return fileContentResult;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw;
            }
        }

        [HttpPost("CloneRecord")]
        public ResultDto CloneRecord([FromBody] GenericReportGridCreateDto dynamicReporGrid)
        {
            try
            {
                var result = _manager.CloneRecord(dynamicReporGrid);
                if (result)
                {
                    return new ResultDto()
                    {
                        Warning = true,
                        Info = "Record is Cloned"
                    };
                }
                else
                {
                    return new ResultDto()
                    {
                        Warning = true,
                        Info = "Something Went Wrong"
                    };
                }
            }
            catch (Exception e)
            {
                return new ResultDto()
                {

                    Info = "Something Went Wrong"
                };
            }
        }

    }
}
