using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.Entita.Organisation;
using CAM.DataTransferObjects.Entita.UserDefinedReportLog;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class UserDefinedReportLogManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public UserDefinedReportLogManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<UserDefinedReportLogDto> FindWithCondition(UserDefinedReportLogQueryDto userDefinedReportLogQueryDto)
        {
            var predicateResult = ApplyFilter(userDefinedReportLogQueryDto);
            var rtn = new QueryResultDto<UserDefinedReportLogDto>(new GenerateRenderForGrid<UserDefinedReportLogDto>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.UserDefinedReportsLogsRepository.Count(predicateResult) :
                _repositoryWrapper.UserDefinedReportsLogsRepository.Count(),
            };
            var query = GetQuery(predicateResult).ApplyOrdering(userDefinedReportLogQueryDto, GetColumnsMap()).ApplyPaging(userDefinedReportLogQueryDto);
            var data = query.ToList();

            
            IEnumerable <UserDefinedReportLogDto> returnUserDefinedReportLogDto;
            returnUserDefinedReportLogDto = _mapper.Map<IEnumerable<UserDefinedReportLogDto>>(data);

            rtn.Items = returnUserDefinedReportLogDto.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Userdefinedreportslogs> ApplyFilter(UserDefinedReportLogQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Userdefinedreportslogs>();
            var predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();

            if (buildFilterDto.UserDefinedReportsLogId != null && buildFilterDto.UserDefinedReportsLogId.Any())
            {
                predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();
                foreach (var item in buildFilterDto.UserDefinedReportsLogId)
                    predicateInner.Or(x => x.Userdefinedreportslogid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReportName != null && buildFilterDto.ReportName.Any())
            {
                predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();
                foreach (var item in buildFilterDto.ReportName)
                    predicateInner.Or(x => x.Reportname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReportFormat != null && buildFilterDto.ReportFormat.Any())
            {
                predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();
                foreach (var item in buildFilterDto.ReportFormat)
                    predicateInner.Or(x => x.Reportformat == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReportDownloadedPath != null && buildFilterDto.ReportDownloadedPath.Any())
            {
                predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();
                foreach (var item in buildFilterDto.ReportDownloadedPath)
                    predicateInner.Or(x => x.Reportdownloadedpath == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReportStatus != null && buildFilterDto.ReportStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();
                foreach (var item in buildFilterDto.ReportStatus)
                    predicateInner.Or(x => x.Reportstatus == item);
                predicateResult.And(predicateInner);
            }
         
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Userdefinedreportslogs>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<UserDefinedReportsLogs> GetQuery(ExpressionStarter<Userdefinedreportslogs> predicateResult )
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.UserDefinedReportsLogsRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.UserDefinedReportsLogsRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);           
            return (IQueryable<UserDefinedReportsLogs>)query.AsEnumerable().Select(x => UserDefinedReportLogMapper.GetUserDefinedReportsLogsMapper(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<UserDefinedReportsLogs, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<UserDefinedReportsLogs, object>>[]>
            {
                ["userDefinedReportsLogId"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.UserDefinedReportsLogId },
                ["reportName"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.ReportName },
                ["reportFormat"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.ReportFormat },
                ["reportDownloadedPath"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.ReportDownloadedPath },
                ["reportStatus"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.ReportStatus },
                ["lastModifiedValue"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<UserDefinedReportsLogs, object>>[] { p => p.CreationDate },
                 
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, UserDefinedReportLogQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult);

            var rtn = propertyName switch
            {
                "userDefinedReportsLogId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.UserDefinedReportsLogId.ToString(), Value = p.UserDefinedReportsLogId.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.UserDefinedReportsLogId.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.UserDefinedReportsLogId.ToString(), Value = p.UserDefinedReportsLogId.ToString() }).Distinct()
                   .ToList(),

                "reportName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.ReportName.ToString(), Value = p.ReportName.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.ReportName.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.ReportName.ToString(), Value = p.ReportName.ToString() }).Distinct()
                        .ToList(),
                "reportStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.ReportStatus.ToString(), Value = p.ReportStatus.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.ReportStatus.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.ReportStatus.ToString(), Value = p.ReportStatus.ToString() }).Distinct()
                        .ToList(),

                "reportDownloadedPath" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.ReportDownloadedPath, Value = p.ReportDownloadedPath }).Distinct().ToList()
                   : query
                       .Where(x => x.ReportDownloadedPath.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.ReportDownloadedPath, Value = p.ReportDownloadedPath }).Distinct()
                       .ToList(),

                "reportFormat" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ReportFormat, Value = p.ReportFormat }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ReportFormat.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ReportFormat, Value = p.ReportFormat }).Distinct().ToList(),
 
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


                "lastModified" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.ModificationDate.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),


                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion        

        public async Task<ResultDto> Add(UserDefinedReportLogCreateDto dto)
        {
            var userDefinedReportLogEntity = new UserDefinedReportsLogs();

            userDefinedReportLogEntity.ReportStatus = dto.ReportStatus;
            userDefinedReportLogEntity.ReportDownloadedPath = dto.ReportDownloadedPath;
            userDefinedReportLogEntity.ReportFormat = dto.ReportFormat  ;
            userDefinedReportLogEntity.ReportName = dto.ReportName;             
            _repositoryWrapper.UserDefinedReportsLogsRepository.Create(UserDefinedReportLogMapper.SetUserDefinedReportsLogs(userDefinedReportLogEntity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

    }
}
