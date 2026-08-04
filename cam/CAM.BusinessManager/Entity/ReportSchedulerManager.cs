using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ReportScheduler;
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
    public class ReportSchedulerManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;

        public ReportSchedulerManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<ReportSchedulerDto> FindWithCondition(ReportSchedulerQueryDto auditLogQueryDto)
        {
            var predicateResult = ApplyFilter(auditLogQueryDto);
            var rtn = new QueryResultDto<ReportSchedulerDto>(new GenerateRenderForGrid<ReportSchedulerDto>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.ReportSchedulerRepository.Count(predicateResult) : _repositoryWrapper.ReportSchedulerRepository.Count(),
            };
            var query = GetQuery(predicateResult, auditLogQueryDto.Deleted ?? false).ApplyOrdering(auditLogQueryDto, GetColumnsMap()).ApplyPaging(auditLogQueryDto);
            var data = query.ToList();
                     
            IEnumerable <ReportSchedulerDto> reportSchedulerDto;

            reportSchedulerDto = _mapper.Map<IEnumerable<ReportSchedulerDto>>(data);

            rtn.Items = reportSchedulerDto.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Reportscheduler> ApplyFilter(ReportSchedulerQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Reportscheduler>();
            var predicateInner = PredicateBuilder.New<Reportscheduler>();

            if (buildFilterDto.ReportSchedulerId != null && buildFilterDto.ReportSchedulerId.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.ReportSchedulerId)
                    predicateInner.Or(x => x.Reportschedulerid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReportName != null && buildFilterDto.ReportName.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.ReportName)
                    predicateInner.And(x => x.Reportname.Trim() == item.Trim());
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsScheduled != null && buildFilterDto.IsScheduled.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.IsScheduled)
                    predicateInner.Or(x => x.Isscheduled.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpcoId != null && buildFilterDto.OpcoId.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExportFilePath != null && buildFilterDto.ExportFilePath.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.ExportFilePath)
                    predicateInner.Or(x => x.Exportfilepath == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExportFileFormat != null && buildFilterDto.ExportFileFormat.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.ExportFileFormat)
                    predicateInner.Or(x => x.Exportfileformat == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ScheduledDate != null && buildFilterDto.ScheduledDate.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.ScheduledDate)
                    predicateInner.Or(x => x.Scheduleddate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ScheduledDayinWeek != null && buildFilterDto.ScheduledDayinWeek.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.ScheduledDayinWeek)
                    predicateInner.Or(x => x.Scheduleddayinweek == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Reportscheduler>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<ReportScheduler> GetQuery(ExpressionStarter<Reportscheduler> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.ReportSchedulerRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.ReportSchedulerRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => ReportSchedulerMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<ReportScheduler, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ReportScheduler, object>>[]>
            {
                ["reportSchedulerId"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ReportSchedulerId },
                ["reportName"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ReportName },
                ["isScheduled"] = new Expression<Func<ReportScheduler, object>>[] { p => p.IsScheduled },
                ["opcoId"] = new Expression<Func<ReportScheduler, object>>[] { p => p.OpcoId },
                ["reportVertical"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ReportVertical },
                ["exportFilePath"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ExportFilePath },
                ["exportFileFormat"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ExportFileFormat },
                ["scheduledDate"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ScheduledDate },
                ["scheduledDayinWeek"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ScheduledDayinWeek },
                ["lastModifiedValue"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<ReportScheduler, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<ReportScheduler, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<ReportScheduler, object>>[] { p => p.CreationDate },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, ReportSchedulerQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {
                "reportSchedulerId" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               { Text = p.ReportSchedulerId.ToString(), Value = p.ReportSchedulerId.ToString() }).Distinct().ToList()
               : query
                   .Where(x => x.ReportSchedulerId.ToString().Contains(propertyFilter)).Select(p =>
                       new FilterValueDto { Text = p.ReportSchedulerId.ToString(), Value = p.ReportSchedulerId.ToString() }).Distinct()
                   .ToList(),

                "reportName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.ReportName.ToString(), Value = p.ReportName.ToString() }).Distinct().ToList()
                    : query
                        .Where(x => x.ReportName.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.ReportName.ToString(), Value = p.ReportName.ToString() }).Distinct()
                        .ToList(),
                "isScheduled" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.IsScheduled == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.IsScheduled == true ? ConstantValueFilter.Yes : ConstantValueFilter.No }).Distinct().ToList()
                    : query
                        .Where(x => x.IsScheduled.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.IsScheduled == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = p.IsScheduled == true ? ConstantValueFilter.Yes : ConstantValueFilter.No }).Distinct()
                        .ToList(),

                "opcoId" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.OpcoId, Value = p.OpcoId}).Distinct().ToList()
                   : query
                       .Where(x => x.OpcoId.Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.OpcoId, Value = p.OpcoId }).Distinct()
                       .ToList(),

                "exportFilePath" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ExportFilePath, Value = p.ExportFilePath }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ExportFilePath.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ExportFilePath, Value = p.ExportFilePath }).Distinct().ToList(),

                "exportFileFormat" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.ExportFileFormat, Value = p.ExportFileFormat }).Distinct().ToList()
                    : query
                        .Where(x => x.ExportFileFormat.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.ExportFileFormat, Value = p.ExportFileFormat }).Distinct()
                        .ToList(),

                "scheduledDate" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Select(p => new FilterValueDto
                   { Text = p.ScheduledDate.ToString(), Value = p.ScheduledDate.ToString() }).Distinct().ToList()
                   : query
                       .Where(x => x.ScheduledDate.ToString().Contains(propertyFilter)).Select(p =>
                           new FilterValueDto { Text = p.ScheduledDate.ToString(), Value = p.ScheduledDate.ToString() }).Distinct()
                       .ToList(),

                "scheduledDayinWeek" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ScheduledDayinWeek, Value = p.ScheduledDayinWeek }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ScheduledDayinWeek.Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ScheduledDayinWeek, Value = p.ScheduledDayinWeek }).Distinct().ToList(),

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

        public async Task<ResultDto> Add(ReportSchedulerCreateOrUpdateDto dto)
        {
            var entityExists = await _repositoryWrapper.ReportSchedulerRepository.FindByCondition(
                x => x.Reportname.ToLower().Replace(" ", "") == dto.ReportName.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Reportschedulerid
                };
            }
            ReportScheduler entity = new ReportScheduler() 
            {
                ReportName = dto.ReportName,
                IsScheduled = dto.IsScheduled == ConstantValueFilter.Yes ? true : false,
                ExportFileFormat = dto.ExportFileFormat,
                ExportFilePath = dto.ExportFilePath,
                ScheduledDate = dto.ScheduledDate,
                ScheduledDayinWeek = dto.ScheduledDayInWeek,
                OpcoId = dto.OpcoId,
                ReportVertical = dto.ReportVertical,

            };
            _repositoryWrapper.ReportSchedulerRepository.Create(ReportSchedulerMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> AddOrUpdate(ReportSchedulerCreateOrUpdateDto dto)
        {
             var existingEntities = await _repositoryWrapper
                 .ReportSchedulerRepository
                 .FindByCondition(x =>
                     x.Reportname.ToLower().Replace(" ", "") == dto.ReportName.ToLower().Replace(" ", "") &&
                     x.Deleted == false)
                 .ToListAsync();

            #region  UPDATE 
            if (dto.ReportSchedulerId != 0)
            {
                // Duplicate check (excluding current record)
                if (existingEntities.Any(x => x.Reportschedulerid != dto.ReportSchedulerId))
                {
                    return new ResultDto
                    {
                        Warning = false,
                        Info = ResultMessages.EntryAlreadyExists,
                        Data = dto.ReportSchedulerId
                    };
                }

                var entityToUpdate = existingEntities
                    .FirstOrDefault(x => x.Reportschedulerid == dto.ReportSchedulerId);

                if (entityToUpdate == null)
                {
                    return new ResultDto { Info = ResultMessages.EntryNotFound };
                }
                entityToUpdate.Reportname = dto.ReportName;
                entityToUpdate.Isscheduled = dto.IsScheduled == ConstantValueFilter.Yes;
                entityToUpdate.Exportfileformat = dto.ExportFileFormat;
                entityToUpdate.Exportfilepath = dto.ExportFilePath;
                entityToUpdate.Scheduleddate = (dto.ScheduledDate==0 && dto.ScheduledDayInWeek !=null)?null: dto.ScheduledDate;
                entityToUpdate.Scheduleddayinweek = dto.ScheduledDayInWeek;
                entityToUpdate.Deleted = false;

                _repositoryWrapper.ReportSchedulerRepository.Update(entityToUpdate);
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
            }
            #endregion
            #region  ADD  
            if (existingEntities.Any())
            {
                return new ResultDto
                {
                    Warning = false,
                    Info = ResultMessages.EntryAlreadyExists
                };
            }

            var newEntity = new Reportscheduler
            {
                Reportname = dto.ReportName,
                Isscheduled = dto.IsScheduled == ConstantValueFilter.Yes,
                Exportfileformat = dto.ExportFileFormat,
                Exportfilepath = dto.ExportFilePath,
                Scheduleddate = (dto.ScheduledDate == 0 && dto.ScheduledDayInWeek != null) ? null : dto.ScheduledDate,
                Scheduleddayinweek = dto.ScheduledDayInWeek,
                Deleted = false
            };

            _repositoryWrapper.ReportSchedulerRepository.Create(newEntity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            #endregion
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ReportSchedulerRepository.FindByCondition(x => x.Reportschedulerid == id).SingleAsync();
            _repositoryWrapper.ReportSchedulerRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Reportschedulerid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ReportSchedulerRepository.FindByCondition(x => x.Reportschedulerid == id).SingleAsync();
            _repositoryWrapper.ReportSchedulerRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Reportschedulerid
            };
        }

    }
}
