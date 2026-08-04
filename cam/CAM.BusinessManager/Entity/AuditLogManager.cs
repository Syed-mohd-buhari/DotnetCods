using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AuditLog;
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

namespace CAM.BusinessManager.Entity
{
    public class AuditLogManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public AuditLogManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<AuditLogDto> FindWithCondition(AuditLogQueryDto auditLogQueryDto)
        {
            var predicateResult = ApplyFilter(auditLogQueryDto);
            var rtn = new QueryResultDto<AuditLogDto>(new GenerateRenderForGrid<AuditLogDto>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.AuditLogRepository.Count(predicateResult) : _repositoryWrapper.AuditLogRepository.Count(),
            };
            var query = GetQuery(predicateResult, auditLogQueryDto.Deleted ?? false).ApplyOrdering(auditLogQueryDto, GetColumnsMap()).ApplyPaging(auditLogQueryDto);
            var data = query.ToList();
                     
            IEnumerable <AuditLogDto> AuditLogDto;

            AuditLogDto = _mapper.Map<IEnumerable<AuditLogDto>>(data);

            rtn.Items = AuditLogDto.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Auditlogs> ApplyFilter(AuditLogQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Auditlogs>();
            var predicateInner = PredicateBuilder.New<Auditlogs>();

            if (buildFilterDto.AuditLogId != null && buildFilterDto.AuditLogId.Any())
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                foreach (var item in buildFilterDto.AuditLogId)
                    predicateInner.Or(x => x.Auditlogid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EntityName != null && buildFilterDto.EntityName.Any())
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                foreach (var item in buildFilterDto.EntityName)
                    predicateInner.Or(x => x.Entityname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EntityId != null && buildFilterDto.EntityId.Any())
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                foreach (var item in buildFilterDto.EntityId)
                    predicateInner.Or(x => x.Entityid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EntityField != null && buildFilterDto.EntityField.Any())
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                foreach (var item in buildFilterDto.EntityField)
                    predicateInner.Or(x => x.Entityfield == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OldValue != null && buildFilterDto.OldValue.Any())
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                foreach (var item in buildFilterDto.OldValue)
                    predicateInner.Or(x => x.Oldvalue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NewValue != null && buildFilterDto.NewValue.Any())
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                foreach (var item in buildFilterDto.NewValue)
                    predicateInner.Or(x => x.Newvalue == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Auditlogs>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<AuditLogs> GetQuery(ExpressionStarter<Auditlogs> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.AuditLogRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.AuditLogRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => AuditLogMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<AuditLogs, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AuditLogs, object>>[]>
            {
                ["auditLogId"] = new Expression<Func<AuditLogs, object>>[] { p => p.AuditLogId },
                ["entityName"] = new Expression<Func<AuditLogs, object>>[] { p => p.EntityName },
                ["entityField"] = new Expression<Func<AuditLogs, object>>[] { p => p.EntityField },
                ["oldValue"] = new Expression<Func<AuditLogs, object>>[] { p => p.OldValue },
                ["newValue"] = new Expression<Func<AuditLogs, object>>[] { p => p.NewValue },
                ["lastModifiedValue"] = new Expression<Func<AuditLogs, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<AuditLogs, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<AuditLogs, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<AuditLogs, object>>[] { p => p.CreationDate },
                ["entityId"] = new Expression<Func<AuditLogs, object>>[] { p => p.EntityId },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, AuditLogQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "auditLogId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.AuditLogId.ToString(), Value = p.AuditLogId.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.AuditLogId.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.AuditLogId.ToString(), Value = p.AuditLogId.ToString() }).Distinct()
                   .ToList(),

                "entityName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.EntityName.ToString(), Value = p.EntityName.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.EntityName.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.EntityName.ToString(), Value = p.EntityName.ToString() }).Distinct()
                        .ToList(),
                "entityId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.EntityId.ToString(), Value = p.EntityId.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.EntityId.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.EntityId.ToString(), Value = p.EntityId.ToString() }).Distinct()
                        .ToList(),

                "entityField" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.EntityField, Value = p.EntityField }).Distinct().ToList()
                   : query
                       .Where(x => x.EntityField.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.EntityField, Value = p.EntityField }).Distinct()
                       .ToList(),

                "oldValue" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.OldValue, Value = p.OldValue }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.OldValue.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.OldValue, Value = p.OldValue }).Distinct().ToList(),

                "newValue" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.NewValue, Value = p.NewValue }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.NewValue.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.NewValue, Value = p.NewValue }).Distinct().ToList(),


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

       

    }
}
