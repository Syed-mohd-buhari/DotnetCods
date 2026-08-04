using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.LookUp
{
    public class PlanningRiskManager : GridBaseAsync<PlanningRisk, TipologicaGridDto, TipologicaQueryDto, Planningrisks>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public PlanningRiskManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }



        public override ExpressionStarter<Planningrisks> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Planningrisks>();
            var predicateInner = PredicateBuilder.New<Planningrisks>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Planningrisks>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Planningrisk == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Planningrisks>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Planningriskid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Planningrisks>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Planningrisks>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Planningrisks>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<PlanningRisk> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = (short)dto.PlanningRiskId,
                Description = dto.PlanningRiskDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<PlanningRisk, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<PlanningRisk, object>>[]>
            {
                ["description"] = new Expression<Func<PlanningRisk, object>>[] { p => p.PlanningRiskDescription },
                ["id"] = new Expression<Func<PlanningRisk, object>>[] { p => p.PlanningRiskId },
                ["lastModifiedBy"] = new Expression<Func<PlanningRisk, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<PlanningRisk> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.PlanningRiskDescription))
                    : request.Where(x => x.PlanningRiskDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.PlanningRiskDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.PlanningRiskId.ToString()))
                    : request.Where(x => x.PlanningRiskId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.PlanningRiskId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<PlanningRisk> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<PlanningRisk> predicateResult, ExpressionStarter<Planningrisks> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.PlanningRisk.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.PlanningRisk.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => PlanningRiskMapper.GetPlanningRiskMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.PlanningRisk.FindByCondition(
               x => x.Planningrisk == dto.Description, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Planningriskid
                };
            }
            PlanningRisk entity = new PlanningRisk() { PlanningRiskId = dto.Id, PlanningRiskDescription = dto.Description };
            _repositoryWrapper.PlanningRisk.Create(PlanningRiskMapper.SetPlanningRiskMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.PlanningRisk.FindByCondition(
               x => x.Planningriskid != dto.Id && x.Planningrisk == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Planningriskid
                };
            }
            PlanningRisk entity = new PlanningRisk() { PlanningRiskId = dto.Id, PlanningRiskDescription = dto.Description };
            _repositoryWrapper.PlanningRisk.Update(PlanningRiskMapper.SetPlanningRiskMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.PlanningRisk.FindByCondition(x => x.Planningriskid == id).SingleAsync();
            _repositoryWrapper.PlanningRisk.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Planningriskid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.PlanningRisk.FindByCondition(x => x.Planningriskid == id).SingleAsync();
            _repositoryWrapper.PlanningRisk.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Planningriskid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.PlanningRisk
                .FindByCondition(x => x.Planningriskid == id)
                .Include(x => x.Plannedactivityresourceplanningrisk).ThenInclude(x => x.Plannedactivityresource)
                .SingleAsync();
            var plannedActivities = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Planningriskid == entity.Planningriskid)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                .ToArray();

            if (entity.Plannedactivityresourceplanningrisk != null && entity.Plannedactivityresourceplanningrisk.Count > 0)
            {
                rm.Add(new ResultMessageDto()
                {
                    Table = "Planned Activity Resource",
                    Values = entity.Plannedactivityresourceplanningrisk.Select(x => x.Plannedactivityresource?.Plannedactivityresource).ToArray()
                });
            }

            if (plannedActivities.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivities });
            }

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Planning Risk",
                        RecordName = entity.Planningrisk,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDto GetCreatePage()
        {
            var dto = new TipologicaGridDto();
            return dto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var entity = PlanningRiskMapper.GetPlanningRiskMapper(_repositoryWrapper.PlanningRisk.FindByCondition(x => x.Planningriskid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = (short)entity.PlanningRiskId, Description = entity.PlanningRiskDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

        //public override IQueryable<PlanningRisk> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<PlanningRisk> predicateResult, ExpressionStarter<PlanningRisk> oracleObject = null)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
