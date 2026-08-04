using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AuditLog;
using CAM.DataTransferObjects.Entita.TsrLog;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Office.CustomUI;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CAM.BusinessManager.Entity
{
    public class TsrLogManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public TsrLogManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<TsrLogDtoGrid> FindWithCondition(TsrLogQueryDto auditLogQueryDto)
        {
            var predicateResult = ApplyFilter(auditLogQueryDto);
            var rtn = new QueryResultDto<TsrLogDtoGrid>(new GenerateRenderForGrid<TsrLogDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.TsrLogRepository.Count(predicateResult) : _repositoryWrapper.TsrLogRepository.Count(),
            };
            var query = GetQuery(predicateResult, auditLogQueryDto.Deleted ?? false).ApplyOrdering(auditLogQueryDto, GetColumnsMap()).ApplyPaging(auditLogQueryDto);
            var data = query.ToList();
                     
            IEnumerable <TsrLogDtoGrid> TsrLogDtoGrid;

            TsrLogDtoGrid = _mapper.Map<IEnumerable<TsrLogDtoGrid>>(data);

            rtn.Items = TsrLogDtoGrid.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Tsrlogs> ApplyFilter(TsrLogQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Tsrlogs>();
            var predicateInner = PredicateBuilder.New<Tsrlogs>();

            if (buildFilterDto.TsrLogId != null && buildFilterDto.TsrLogId.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.TsrLogId)
                    predicateInner.Or(x => x.Tsrlogid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TypeOfOperation != null && buildFilterDto.TypeOfOperation.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.TypeOfOperation)
                    predicateInner.Or(x => x.Typeofoperation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FileName != null && buildFilterDto.FileName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.FileName)
                    predicateInner.Or(x => x.Filename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TotalRecord != null && buildFilterDto.TotalRecord.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.TotalRecord)
                    predicateInner.Or(x => x.Totalrecord == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProcessedRecord != null && buildFilterDto.ProcessedRecord.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.ProcessedRecord)
                    predicateInner.Or(x => x.Processedrecord == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Status != null && buildFilterDto.Status.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.Status)
                    predicateInner.Or(x => x.Status == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BatchIdentifier != null && buildFilterDto.BatchIdentifier.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.BatchIdentifier)
                    predicateInner.Or(x => x.Batchidentifier == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Domain != null && buildFilterDto.Domain.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.Domain)
                    predicateInner.Or(x => x.Domain == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EndTime != null)
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                if (buildFilterDto.EndTime.StartDate != null)
                    predicateInner.And(x => x.Endtime.Value.Date >= buildFilterDto.EndTime.StartDate);
                if (buildFilterDto.EndTime.EndDate != null)
                    predicateInner.And(x => x.Endtime.Value.Date <= buildFilterDto.EndTime.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.StartTime != null)
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                if (buildFilterDto.StartTime.StartDate != null)
                    predicateInner.And(x => x.Starttime.Value.Date >= buildFilterDto.StartTime.StartDate);
                if (buildFilterDto.StartTime.EndDate != null)
                    predicateInner.And(x => x.Starttime.Value.Date <= buildFilterDto.StartTime.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Tsrlogs>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<TsrLog> GetQuery(ExpressionStarter<Tsrlogs> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.TsrLogRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.TsrLogRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => TsrLogMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<TsrLog, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<TsrLog, object>>[]>
            {
                ["tsrLogId"] = new Expression<Func<TsrLog, object>>[] { p => p.TsrLogId },
                ["typeOfOperation"] = new Expression<Func<TsrLog, object>>[] { p => p.TypeOfOperation },
                ["fileName"] = new Expression<Func<TsrLog, object>>[] { p => p.FileName },
                ["totalRecord"] = new Expression<Func<TsrLog, object>>[] { p => p.TotalRecord },
                ["processedRecord"] = new Expression<Func<TsrLog, object>>[] { p => p.ProcessedRecord },
                ["startTime"] = new Expression<Func<TsrLog, object>>[] { p => p.StartTime },
                ["endTime"] = new Expression<Func<TsrLog, object>>[] { p => p.EndTime },
                ["status"] = new Expression<Func<TsrLog, object>>[] { p => p.Status },
                ["domain"] = new Expression<Func<TsrLog, object>>[] { p => p.Domain },
                ["batchIdentifier"] = new Expression<Func<TsrLog, object>>[] { p => p.BatchIdentifier },
                ["lastModifiedValue"] = new Expression<Func<TsrLog, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<TsrLog, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<TsrLog, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<TsrLog, object>>[] { p => p.CreationDate },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, TsrLogQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "tsrLogId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.TsrLogId.ToString(), Value = p.TsrLogId.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.TsrLogId.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.TsrLogId.ToString(), Value = p.TsrLogId.ToString() }).Distinct()
                   .ToList(),

                "typeOfOperation" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.TypeOfOperation.ToString(), Value = p.TypeOfOperation.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.TypeOfOperation.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.TypeOfOperation.ToString(), Value = p.TypeOfOperation.ToString() }).Distinct()
                        .ToList(),
                "fileName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.FileName, Value = p.FileName }).Distinct().ToList()
                    : query
                        .Where(x => x.FileName.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.FileName, Value = p.FileName }).Distinct()
                        .ToList(),

                "totalRecord" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.TotalRecord.ToString(), Value = p.TotalRecord.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.TotalRecord.ToString().Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.TotalRecord.ToString(), Value = p.TotalRecord.ToString() }).Distinct()
                       .ToList(),

                "processedRecord" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ProcessedRecord.ToString(), Value = p.ProcessedRecord.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ProcessedRecord.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ProcessedRecord.ToString(), Value = p.ProcessedRecord.ToString() }).Distinct().ToList(),

                "status" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Status, Value = p.Status }).Distinct().ToList()
                    : query
                        .Where(x => x.Status.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.Status, Value = p.Status }).Distinct()
                        .ToList(),

                "domain" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.Domain, Value = p.Domain }).Distinct().ToList()
                   : query
                       .Where(x => x.Domain.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.Domain, Value = p.Domain }).Distinct()
                       .ToList(),

                "batchIdentifier" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.BatchIdentifier, Value = p.BatchIdentifier }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.BatchIdentifier.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.BatchIdentifier, Value = p.BatchIdentifier }).Distinct().ToList(),

                "endTime" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto { Text = p.EndTime.ToString(), Value = p.EndTime.ToString() }).Distinct().ToList()
                   : query
                       .Where(x =>
                           x.EndTime.ToString().Contains(
                               propertyFilter)).Select(p => new FilterValueDto { Text = p.EndTime.ToString(), Value = p.EndTime.ToString() }).Distinct().ToList(),



                "startTime" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.StartTime.ToString(), Value = p.StartTime.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.StartTime.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.StartTime.ToString(), Value = p.StartTime.ToString() }).Distinct().ToList(),


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


        public TsrLogDtoGrid GetRefreshStatus(int filterId ,string opcoId)
        {

            var latestRefreshStatus = new Tsrlogs();
            var entity = _repositoryWrapper.OpCo.FindAll().ToList();
            var opCoList = entity.Distinct().ToDictionary(x => x.Opcoid, y => y.Opco);
            Dictionary<short,DateTime> opcoWiseLastModifiedDate = new Dictionary<short,DateTime>();

            if (filterId == (int)TsrAndFntLogsEnum.TSR)
            {
                latestRefreshStatus = _repositoryWrapper.TsrLogRepository.FindByCondition(x => x.Typeofoperation == "Data Refresh" && x.Batchidentifier.ToLower().Trim() != "tems-fnt").OrderByDescending(x => x.Modificationdate).FirstOrDefault();
            }
            else if(filterId == (int)TsrAndFntLogsEnum.FNT)
            {
                if (!string.IsNullOrEmpty(opcoId))
                {
                    var opcoName = entity.Where(x => x.Opcoid.ToString() == opcoId).Select(x => x.Opco).FirstOrDefault();
                    if (opcoName != null)
                        latestRefreshStatus = _repositoryWrapper.TsrLogRepository.FindByCondition(x => x.Typeofoperation == "Data Refresh"
                        && x.Batchidentifier.ToLower().Trim() == "tems-fnt" && x.Domain == opcoName).
                        OrderByDescending(x => x.Modificationdate).FirstOrDefault();
                }
                else
                {
                    latestRefreshStatus = _repositoryWrapper.TsrLogRepository.FindByCondition(x => x.Typeofoperation == "Data Refresh"
                        && x.Batchidentifier.ToLower().Trim() == "tems-fnt").
                        OrderByDescending(x => x.Modificationdate).FirstOrDefault();
                }

                var opcoWiseRefreshStatus = _repositoryWrapper.TsrLogRepository.FindByCondition(x => x.Typeofoperation == "Data Refresh"
                                                    && x.Batchidentifier.ToLower().Trim() == "tems-fnt" && x.Domain != null)
                                                    .GroupBy(x => x.Domain)
                                                    .Select(x => x.OrderByDescending(x => x.Modificationdate).FirstOrDefault())
                                                    .ToList();
                if (opcoWiseRefreshStatus != null)
                {
                    opcoWiseLastModifiedDate = opcoWiseRefreshStatus.ToList().Select(c => new
                    {
                        OpcoId = opCoList.Where(x => x.Value == c.Domain).Select(x => x.Key).FirstOrDefault(),
                        LastModifiedDate = (DateTime)c.Endtime,
                    }).ToDictionary(x => x.OpcoId, x => x.LastModifiedDate);

                }

            }
            
            bool IsRefreshCompleted = false;
            if (latestRefreshStatus != null)
            {
                if (latestRefreshStatus.Status.ToLower() == ConstantValueFilter.Completed)
            {
                    IsRefreshCompleted = true;
            }
                return new TsrLogDtoGrid()
            {
                    Status = latestRefreshStatus.Status,
                    EndTime = latestRefreshStatus.Endtime,
                    IsRefreshCompleted = IsRefreshCompleted,
                    TypeOfOperation = latestRefreshStatus.Typeofoperation,
                    opCoList = opCoList,
                    opcoWiseLastModifiedDate=opcoWiseLastModifiedDate,
                };
            }
            else
            {
                return new TsrLogDtoGrid()
                {
                        EndTime = DateTime.Now,
                        IsRefreshCompleted = true,
                        opCoList = opCoList
                };
            }

        }
        public QueryResultDto<TsrLogDtoGrid> GetAllOpcoWiseLatestRefreshStatus(TsrLogQueryDto tsrLogQueryDto)
        {
           
            var OpcoWiseRefreshStatus = _repositoryWrapper.TsrLogRepository.FindByCondition(x => x.Typeofoperation == "Data Refresh"
                                                        && x.Batchidentifier.ToLower().Trim() == "tems-fnt" && x.Domain != null)
                                                        .GroupBy(x => x.Domain)
                                                        .Select(x=>x.OrderByDescending(x => x.Modificationdate).FirstOrDefault())
                                                        .OrderBy(x=>x.Domain)
                                                        .ToList();

            var rtn = new QueryResultDto<TsrLogDtoGrid>(new GenerateRenderForGrid<TsrLogDtoGrid>(_manager))
            {
                TotalItems = OpcoWiseRefreshStatus.Count(),
            };
            if (OpcoWiseRefreshStatus != null && OpcoWiseRefreshStatus.Count > 0)
            {
                var allOpcoWiseRefreshedStatus = OpcoWiseRefreshStatus
                                                .Select(allOpco => new TsrLogDtoGrid
                                                {
                                                     Status = allOpco.Status,
                                                     EndTime = allOpco.Endtime,
                                                     IsRefreshCompleted = allOpco.Status.ToLower() == ConstantValueFilter.Completed ? true : false,
                                                     TypeOfOperation = allOpco.Typeofoperation,
                                                     Domain = allOpco.Domain,
                                                 }).ToList();
                var refreshData = allOpcoWiseRefreshedStatus.ApplyPaginationList(tsrLogQueryDto);
                rtn.Items = refreshData.ToArray();
            }

            return rtn;
        }

    }
}
