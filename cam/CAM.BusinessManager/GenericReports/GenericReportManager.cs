using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.GenericReportDto;
using CAM.DataTransferObjects.Entita.GenericReportDto;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Net.Sockets;
using DocumentFormat.OpenXml.InkML;
using CAM.DataTransferObjects;

namespace CAM.BusinessManager.GenericReports
{
    public class GenericReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;



        public GenericReportManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper, GridCustomColumnManager manager, ICurrentUserService currentUserService) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _manager = manager;

        }

        #region UIMemberFunctions

        public QueryResultDto<GenericReportGridCreateDto> FindWithCondition(GenericReportQueryDto dynamicReportQueryDto)
        {
            var predicateResult = ApplyFilter(dynamicReportQueryDto);
            if (dynamicReportQueryDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            var rtn = new QueryResultDto<GenericReportGridCreateDto>(new GenerateRenderForGrid<GenericReportGridCreateDto>(_manager))
            {
                //TotalItems = predicateResult.IsStarted ? _repositoryWrapper.GenericReportRepository.Count(predicateResult) : _repositoryWrapper.DynamicReportRepository.Count(),
            };
            var query = GetQuery(predicateResult, dynamicReportQueryDto.Deleted ?? false).ApplyOrdering(dynamicReportQueryDto, GetColumnsMap());
            rtn.TotalItems = query.Count();
            query = query.ApplyPaging(dynamicReportQueryDto);
            var data = query.ToList();

            IEnumerable<GenericReportGridCreateDto> genericReportGridResult;
            genericReportGridResult = _mapper.Map<IEnumerable<GenericReportGridCreateDto>>(data);
            rtn.Items = genericReportGridResult.ToArray();
            return rtn;
        }
        private static ExpressionStarter<Dynamicreports> ApplyFilter(GenericReportQueryDto DynamicReportQueryFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Dynamicreports>();
            var predicateInner = PredicateBuilder.New<Dynamicreports>();

            if (DynamicReportQueryFilterDto.DynamicReportsId != null && DynamicReportQueryFilterDto.DynamicReportsId.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.DynamicReportsId)
                    predicateInner.Or(x => x.Dynamicreportsid == item);
                predicateResult.And(predicateInner);
            }

            if (DynamicReportQueryFilterDto.UserId != null && DynamicReportQueryFilterDto.UserId.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.UserId)
                    predicateInner.Or(x => x.Userid == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ReportName != null && DynamicReportQueryFilterDto.ReportName.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ReportName)
                    predicateInner.Or(x => x.Reportname == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.Published != null && DynamicReportQueryFilterDto.Published.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.Published)
                    predicateInner.Or(x => x.Published.ToString().ToLower().Trim() == (item == "Published" ? "true" : "false"));
                predicateResult.And(predicateInner);
            }

            if (DynamicReportQueryFilterDto.IsScheduled != null && DynamicReportQueryFilterDto.IsScheduled.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.IsScheduled)
                    predicateInner.Or(x => x.Isscheduled ==  item );
                predicateResult.And(predicateInner);
            }

            if (DynamicReportQueryFilterDto.CreationUser != null && DynamicReportQueryFilterDto.CreationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.CreationUser)
                    predicateInner.Or(x => x.CreationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (DynamicReportQueryFilterDto.ModificationUser != null && DynamicReportQueryFilterDto.ModificationUser.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ModificationUser)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.CreationDate != null)
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                if (DynamicReportQueryFilterDto.CreationDate.StartDate != null)
                    predicateInner.And(x => x.Creationdate >= DynamicReportQueryFilterDto.CreationDate.StartDate);
                if (DynamicReportQueryFilterDto.CreationDate.EndDate != null)
                    predicateInner.And(x => x.Creationdate <= DynamicReportQueryFilterDto.CreationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ModificationDate != null)
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                if (DynamicReportQueryFilterDto.ModificationDate.StartDate != null)
                    predicateInner.And(x => x.Modificationdate >= DynamicReportQueryFilterDto.ModificationDate.StartDate);
                if (DynamicReportQueryFilterDto.ModificationDate.EndDate != null)
                    predicateInner.And(x => x.Modificationdate <= DynamicReportQueryFilterDto.ModificationDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ExportType != null && DynamicReportQueryFilterDto.ExportType.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ExportType)
                    predicateInner.Or(x => x.Exporttype == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ExportFileFormat != null && DynamicReportQueryFilterDto.ExportFileFormat.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ExportFileFormat)
                    predicateInner.Or(x => x.Exportfileformat == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ExportFilePath != null && DynamicReportQueryFilterDto.ExportFilePath.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ExportFilePath)
                    predicateInner.Or(x => x.Exportfilepath == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ScheduledDate != null && DynamicReportQueryFilterDto.ScheduledDate.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ScheduledDate)
                    predicateInner.Or(x => x.Scheduleddate == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ScheduledDayInWeek != null && DynamicReportQueryFilterDto.ScheduledDayInWeek.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ScheduledDayInWeek)
                    predicateInner.Or(x => x.Scheduleddayinweek == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.ScheduledType != null && DynamicReportQueryFilterDto.ScheduledType.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.ScheduledType)
                    predicateInner.Or(x => x.Scheduledtype == item);
                predicateResult.And(predicateInner);
            }
            if (DynamicReportQueryFilterDto.IsTestNodeRequired != null && DynamicReportQueryFilterDto.IsTestNodeRequired.Any())
            {
                predicateInner = PredicateBuilder.New<Dynamicreports>();
                foreach (var item in DynamicReportQueryFilterDto.IsTestNodeRequired) 
                    predicateInner.Or(x => x.Istestnoderequired == (item == ConstantValueFilter.Yes ? true : false));
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<CAM.Entities.Models.DynamicReports> GetQuery(ExpressionStarter<Dynamicreports> predicateResult, bool includeDeleted)
        {
            IQueryable<OracleModels.DBModels.Dynamicreports> query;
            if (_currentUserService.Rule[0] == "Admin")
            {
                 query = predicateResult.IsStarted
                    ? _repositoryWrapper.GenericReportRepository.FindByCondition(predicateResult, includeDeleted)
                    .Include(x => x.CreationuserNavigation)
                    .Include(x => x.ModificationuserNavigation)
                    : _repositoryWrapper.GenericReportRepository.FindAll()
                    .Include(x => x.CreationuserNavigation)
                    .Include(x => x.ModificationuserNavigation);
            }
            else
            {
                query = predicateResult.IsStarted
                   ? _repositoryWrapper.GenericReportRepository.FindByCondition(predicateResult, includeDeleted)
                   .Include(x => x.CreationuserNavigation)
                   .Include(x => x.ModificationuserNavigation).Where(x => x.Published == true)
                   : _repositoryWrapper.GenericReportRepository.FindAll()
                   .Include(x => x.CreationuserNavigation)
                   .Include(x => x.ModificationuserNavigation).Where(x => x.Published == true);
            }
            return query.AsEnumerable().Select(x => DynamicReportMapper.Get(x)).AsQueryable();
        }
        
        private Dictionary<string, Expression<Func<CAM.Entities.Models.DynamicReports, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<CAM.Entities.Models.DynamicReports, object>>[]>
            {
                ["dynamicReportsId"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.DynamicReportsId },
                ["userId"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.UserId },
                ["reportName"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.ReportName },
               // ["jsongridCustomizationData"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.JsongridCustomizationData },
                ["published"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.Published },
                ["isscheduled"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.Isscheduled },
                ["modificationDate"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.ModificationDate },
                ["modificationUser"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationDate"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.CreationDate },
                ["creationUser"] = new Expression<Func<CAM.Entities.Models.DynamicReports, object>>[] { p => p.CreationUserEntity.Email },
            };
        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, GenericReportQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "dynamicReportsId" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.DynamicReportsId.ToString(), Value = p.DynamicReportsId.ToString() }).Distinct().ToList()
                : query
                    .Where(x => x.DynamicReportsId.ToString().Contains(propertyFilter)).Select(p =>
                        new FilterValueDto { Text = p.DynamicReportsId.ToString(), Value = p.DynamicReportsId.ToString() }).Distinct()
                    .ToList(),

                "userId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.UserId.ToString(), Value = p.UserId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.UserId.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.UserId.ToString(), Value = p.UserId.ToString() }).Distinct().ToList(),

                "reportName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.ReportName != null)
                           .Select(p => new FilterValueDto { Text = p.ReportName, Value = p.ReportName.ToString() })
                           .Distinct()
                           .ToList()
                    : query
                        .Where(x => x.ReportName != null && x.ReportName != null && x.ReportName.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.ReportName, Value = p.ReportName.ToString() })
                        .Distinct()
                        .ToList(),
                "isScheduled" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.Isscheduled.ToString(), Value = p.Isscheduled.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Isscheduled.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Isscheduled.ToString(), Value = p.Isscheduled.ToString() }).Distinct().ToList(),

                "published" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto { Text = p.Published == true ? "Published" : p.Published == null ? null : "Not Published".ToString(), Value = p.Published == true ? "Published" : p.Published == null ? null : "Not Published".ToString() }).Distinct().ToList()
                     : query
                     .Where(x => x.Published != null)
                     .Select(p => new FilterValueDto { Text = p.Published == true ? "Published" : p.Published == null ? null : "Not Published".ToString(), Value = p.Published == true ? "Published" : p.Published == null ? null : "Not Published".ToString() }).Distinct().ToList(),


                "creationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.CreationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.CreationUserEntity.Email)).Distinct().ToList(),

                "creationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.CreationDate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.CreationDate.ToString(), Value = p.CreationDate.ToString() }).Distinct().ToList(),


                "modificationUser" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


                "modificationDate" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationDate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),
                "exportFileFormat" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto(p.ExportFileFormat)).Distinct().ToList()
                     : query
                         .Where(x =>  x.ExportFileFormat.Contains( propertyFilter)).Select(p => new FilterValueDto(p.ExportFileFormat)).Distinct().ToList(),
                "exportFilePath" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto(p.ExportFilePath)).Distinct().ToList()
                     : query
                         .Where(x => x.ExportFilePath.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.ExportFilePath))
                         .Distinct().ToList(),
                "exportType" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(src => new FilterValueDto
                     {
                         Text = (string.IsNullOrEmpty(Convert.ToString(src.ExportType))) ? null :
                   ((src.ExportType == 1) ? "Aggregate"
                    : (src.ExportType == 2) ? "DisAggregate" : null),
                         Value = src.ExportType.ToString()
                     }).Distinct().ToList()
                     : query.Where(x => x.ExportType.ToString().Contains(propertyFilter)).Select(src => new FilterValueDto
                     {
                         Text = (string.IsNullOrEmpty(Convert.ToString(src.ExportType))) ? null :
                   ((src.ExportType == 1) ? "Aggregate"
                    : (src.ExportType == 2) ? "DisAggregate" : null),
                         Value = src.ExportType.ToString()
                     }).Distinct().ToList(),
                "scheduledDate" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto(p.ScheduledDate)).Distinct().ToList()
                     : query
                         .Where(x => x.ScheduledDate.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.ScheduledDate)).Distinct().ToList(),
                "scheduledDayInWeek" => string.IsNullOrEmpty(propertyFilter)
                      ? query.Select(p => new FilterValueDto(p.ScheduledDayInWeek)).Distinct().ToList()
                      : query
                          .Where(x => x.ScheduledDayInWeek.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto(p.ScheduledDayInWeek)).Distinct().ToList(),
                "scheduledType" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(src => new FilterValueDto
                   {
                       Text = (string.IsNullOrEmpty(Convert.ToString(src.ScheduledType))) ? null :
                 ((src.ScheduledType == 1) ? "Monthly"
                  : (src.ScheduledType == 2) ? "Weekly" : null),
                       Value = src.ScheduledType.ToString()
                   }).Distinct().ToList()
                   : query.Where(x => x.ScheduledType.ToString().Contains(propertyFilter)).Select(src => new FilterValueDto
                   {
                       Text = (string.IsNullOrEmpty(Convert.ToString(src.ScheduledType))) ? null :
                 ((src.ScheduledType == 1) ? "Monthly"
                  : (src.ScheduledType == 2) ? "Weekly" : null),
                       Value = src.ScheduledType.ToString()
                   }).Distinct().ToList(),

                "isTestNodeRequired" => query.Select(p => new FilterValueDto { Text=p.IsTestNodeRequired==true?ConstantValueFilter.Yes:ConstantValueFilter.No, Value = p.IsTestNodeRequired == true ? ConstantValueFilter.Yes : ConstantValueFilter.No }).Distinct().ToList(),

                _ => new List<FilterValueDto>()
            };

            return rtn;
        }
        #endregion

        #region CRUD Operations
        public List<GenericReportInfrastructureDto> GetMapping(long id)
        {
            var mapping = _repositoryWrapper.GenericReportRepository.FindByCondition(x =>
                x.Dynamicreportsid == id).FirstOrDefault();
            if (mapping == null)
            {
                return new List<GenericReportInfrastructureDto>();
            }
            return JsonSerializer.Deserialize<List<GenericReportInfrastructureDto>>(mapping.Jsongridcustomizationdata);
        }

        /// <summary>
        /// SaveReport function is used to Save the report 
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="dynamicReports"></param>
        /// <param name="publish"></param>
        public void SaveReport(string reportName, long dynamicReportsId, List<GenericReportInfrastructureDto> dynamicReports, bool publish, string opcoIds, string  ExportFilePath, string  ExportFileFormat, string ExportType, string ScheduledDate,bool isExportReport, string ScheduledDayInWeek, string ScheduledType,bool isTestNodeRequired=false)
        {
            var jsonData = JsonSerializer.Serialize(dynamicReports);
           
            if (dynamicReportsId > 0)
            {
                var model = _repositoryWrapper.GenericReportRepository.FindByCondition(x => x.Dynamicreportsid == dynamicReportsId && !x.Deleted.Value).OrderByDescending(p => p.Creationdate).SingleOrDefault();                
                var mapping = DynamicReportMapper.Get(model);
                if (mapping != null)
                {
                    mapping.JsongridCustomizationData = jsonData;
                    mapping.ReportName = reportName;
                    mapping.Published = publish;
                    mapping.OpcoId = !string.IsNullOrEmpty(opcoIds) ? opcoIds.ToString().TrimEnd(',') : "";
                    mapping.ScheduledDate = (!string.IsNullOrEmpty(ScheduledDate)) ? Convert.ToInt16(ScheduledDate) : null;
                    mapping.ExportFileFormat =  ExportFileFormat;
                    mapping.ExportFilePath = ExportFilePath;
                    mapping.ExportType = !string.IsNullOrEmpty(ExportType) ? Convert.ToInt16(ExportType) : null;
                    mapping.Isscheduled = isExportReport;
                    mapping.ScheduledDayInWeek = (!string.IsNullOrEmpty(ScheduledDayInWeek)) ? ScheduledDayInWeek : null;
                    mapping.ScheduledType = !string.IsNullOrEmpty(ScheduledType) ? Convert.ToInt16(ScheduledType) : null;
                    mapping.IsTestNodeRequired = isTestNodeRequired;
                    _repositoryWrapper.GenericReportRepository.Update(DynamicReportMapper.Set(mapping));
                }
            }
            else
            {
                _repositoryWrapper.GenericReportRepository.Create(DynamicReportMapper.Set(new Entities.Models.DynamicReports()
                {
                    JsongridCustomizationData = jsonData,
                    ReportName = reportName,
                    Published = publish,
                    OpcoId = !string.IsNullOrEmpty(opcoIds) ? opcoIds.ToString().TrimEnd(',') : "",
                    UserId = _currentUserService.UserId,
                    ExportType = ((isExportReport == true) &&!string.IsNullOrEmpty(ExportType)) ? Convert.ToInt16(ExportType) : null,
                    ExportFilePath = (isExportReport == true) ? ExportFilePath : null,
                    ExportFileFormat = (isExportReport == true) ? ExportFileFormat : null,
                    ScheduledDate = ((isExportReport == true) && !string.IsNullOrEmpty(ScheduledDate)) ? Convert.ToInt16(ScheduledDate) : null,
                    ScheduledDayInWeek = ((isExportReport == true) && !string.IsNullOrEmpty(ScheduledDayInWeek)) ? ScheduledDayInWeek : null,
                    ScheduledType = ((isExportReport == true) && !string.IsNullOrEmpty(ScheduledType)) ? Convert.ToInt16(ScheduledType) : null,
                    Isscheduled = isExportReport,
                    IsTestNodeRequired = isTestNodeRequired,
                }));
                
            }
            _repositoryWrapper.Save();
        }

        public void SaveReport(string reportName, List<GenericReportInfrastructureDto> dynamicReports, bool publish,string opcoIds, string ExportFilePath, string ExportFileFormat, string ExportType, string ScheduledDate, bool isExportReport, string ScheduledDayInWeek, string ScheduledType, bool isTestNodeRequired=false)
        {
            var jsonData = JsonSerializer.Serialize(dynamicReports);
            
            _repositoryWrapper.GenericReportRepository.Create(DynamicReportMapper.Set(new Entities.Models.DynamicReports()
            {
                JsongridCustomizationData = jsonData,
                ReportName = reportName,
                Published = publish,
                OpcoId = !string.IsNullOrEmpty(opcoIds) ? opcoIds.ToString().TrimEnd(',') : "",
                UserId = _currentUserService.UserId,
               
                ExportFileFormat = (isExportReport == true) ? ExportFileFormat : null ,
                ExportFilePath = (isExportReport == true) ? ExportFilePath : null,
                ExportType = !string.IsNullOrEmpty(ExportType) ? Convert.ToInt16(ExportType) : null,
                ScheduledDate = !string.IsNullOrEmpty(ScheduledDate) ? Convert.ToInt16(ScheduledDate) : null,
                ScheduledDayInWeek = !string.IsNullOrEmpty(ScheduledDayInWeek) ? ScheduledDayInWeek : null,
                ScheduledType = !string.IsNullOrEmpty(ScheduledType) ? Convert.ToInt16(ScheduledType) : null,
                Isscheduled = isExportReport,
                IsTestNodeRequired=isTestNodeRequired,

        }));
            _repositoryWrapper.Save();

        }

        /// <summary>
        /// This function is whenever we publish the report the publish column to change false to true.
        /// </summary>
        /// <param name="reportid"></param>
        public bool UpdateReport(long reportid)
        {
            bool result = false;
            try
            {
                var entity = _repositoryWrapper.GenericReportRepository.FindByCondition(x => x.Dynamicreportsid == reportid && !x.Deleted.Value).SingleOrDefault();
                if (entity != null)
                {
                    entity.Published = true;
                    _repositoryWrapper.GenericReportRepository.Update(entity);
                    _repositoryWrapper.Save();
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }



        public bool DeleteReport(long reportid)
        {
            bool result = false;
            try
            {
                var entity = _repositoryWrapper.GenericReportRepository.FindByCondition(x =>
                       x.Dynamicreportsid == reportid).SingleOrDefault();
                if (entity != null)
                {
                    _repositoryWrapper.GenericReportRepository.Delete(entity);
                    _repositoryWrapper.Save();
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch
            {
                return false;
            }

        }
        #endregion

        /// <summary>
        /// This function is get the list of column name for Particular table
        /// </summary>
        /// <param name="entityTypes"></param>
        /// <returns>List of Columns</returns>
        public Dictionary<string, List<string>> GetTableColumns(List<System.Type> entityTypes, List<string> excludedProperties)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            try
        {
                foreach (System.Type entityType in entityTypes)
                {
                    var properties = entityType.GetProperties()
                                     .Where(x => !excludedProperties.Contains(x.Name))
                                     .Select(p => p.Name)
                                     .ToList();
                    string entityName = entityType.Name;
                    result[entityName] = properties;

                }
            }    
            catch (Exception ex)
            {

            }
            return result;
        }


        public QueryResultDto<GenericReportDtoGrid> GetTableColumns(List<System.Type> entityTypes)
        {
            var rtn = new QueryResultDto<GenericReportDtoGrid>(new GenerateRenderForGrid<GenericReportDtoGrid>(_manager))
            {

            };

            return rtn;
        }

        #region //Clone Record
        public bool CloneRecord(GenericReportGridCreateDto dynamicReporGrid)
        {
            bool result = false;
            try
            {
                var entity = _repositoryWrapper.GenericReportRepository.FindByCondition(x => x.Dynamicreportsid == dynamicReporGrid.DynamicReportsId).SingleOrDefault();
                if (entity != null)
                {
                    CAM.Entities.Models.DynamicReports model = new CAM.Entities.Models.DynamicReports()
                    {
                        JsongridCustomizationData = entity.Jsongridcustomizationdata,
                        ReportName = dynamicReporGrid.ReportName,
                        Published = false,
                        UserId = _currentUserService.UserId,
                        ExportFileFormat = entity.Exportfileformat,
                        ExportFilePath = entity.Exportfilepath,
                        ExportType = entity.Exporttype,
                        ScheduledDate= entity.Scheduleddate,
                        ScheduledDayInWeek = entity.Scheduleddayinweek,
                        ScheduledType = entity.Scheduledtype
                    };
                    var dynamicentity = DynamicReportMapper.Set(model);
                    _repositoryWrapper.GenericReportRepository.Create(dynamicentity);
                    _repositoryWrapper.Save();
                    result = true;
                }
                else
                {
                    result =false;
                }
                return result;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Ticket 730 - TSR Report Download Automation

        public List<GenericReportSchedulerDto> GetSchedulerReportDetails()
        {

            string currentDayOfWeek = DateTime.Now.DayOfWeek.ToString();

            var query = _repositoryWrapper.GenericReportRepository
                .FindByCondition(x => x.Scheduleddate == DateTime.Now.Day || x.Scheduleddate == 0 || x.Scheduleddayinweek == currentDayOfWeek);// && x.Isscheduled == true);



            var dynamicReport = query.ToList().Select(
                x => new GenericReportSchedulerDto
                {
                    DynamicReportId = (long)x.Dynamicreportsid,
                    ReportName = x.Reportname,
                    Isscheduled = x.Isscheduled , //!= null ? (DateTime)x.Isscheduled : null,
                    ExportFileFormat = !string.IsNullOrEmpty( x.Exportfileformat) ? x.Exportfileformat .Replace('.',' ') : ".csv",
                    ExportFilePath = x.Exportfilepath,
                    ScheduledDate = Convert.ToInt16(  x.Scheduleddate),
                    ScheduledDayInWeek = x.Scheduleddayinweek,
                    ScheduledType = (short)(x.Scheduledtype ?? 0),
                    TsrOpcoId = x.Opcoid,
                    ExportType = (short)( x.Exporttype ?? 0)
                });
        
            return dynamicReport.ToList();
        }
        #endregion
    }
}
