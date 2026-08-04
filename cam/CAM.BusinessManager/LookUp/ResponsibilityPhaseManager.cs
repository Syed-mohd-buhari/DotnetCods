using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.LookUp;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
  public  class ResponsibilityPhaseManager : GridBaseAsync<ResponsibilityPhase, TipologicaGridDtoRule, TipologicaQueryDtoRule, Responsibilityphases>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public ResponsibilityPhaseManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            
            _columnManager = columnManager;
        }
     

        public override ExpressionStarter<Responsibilityphases> ApplyFilterForOracleModel(TipologicaQueryDtoRule request)
        {
            var predicateResult = PredicateBuilder.New<Responsibilityphases>();
            var predicateInner = PredicateBuilder.New<Responsibilityphases>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Responsibilityphases>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Responsibilityphase == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Responsibilityphases>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Responsibilityphases>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Responsibilityphaseid == item);
                predicateResult.And(predicateInner);
            }
            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Responsibilityphases>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Responsibilityphases>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Responsibilityphases>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDtoRule> CastObjectToDto(IQueryable<ResponsibilityPhase> request)
        {
            return  request.Select(dto => new TipologicaGridDtoRule()
            {
                Id = dto.ResponsibilityPhaseId,
                Description = dto.ResponsibilityPhaseDescription,
                LastModified = dto.ModificationDate,
                Rule = dto.Rule,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<ResponsibilityPhase, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ResponsibilityPhase, object>>[]>
            {
                ["description"] = new Expression<Func<ResponsibilityPhase, object>>[] { p => p.ResponsibilityPhaseDescription },
                ["id"] = new Expression<Func<ResponsibilityPhase, object>>[] { p => p.ResponsibilityPhaseId },
                ["lastModifiedBy"] = new Expression<Func<ResponsibilityPhase, object>>[] { p => p.ModificationUserEntity.Email },
                ["rule"] = new Expression<Func<ResponsibilityPhase, object>>[] { p => p.Rule }
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ResponsibilityPhase> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ResponsibilityPhaseDescription))
                : request.Where(x => x.ResponsibilityPhaseDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ResponsibilityPhaseDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.ResponsibilityPhaseId.ToString()))
                : request.Where(x => x.ResponsibilityPhaseId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ResponsibilityPhaseId.ToString())),
                "rule" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Rule.ToString()))
                    : request.Where(x => x.Rule.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Rule.ToString())),
            };
        }

        public override IQueryable<ResponsibilityPhase> PrepareQuery(TipologicaQueryDtoRule request, ExpressionStarter<ResponsibilityPhase> predicateResult , ExpressionStarter<Responsibilityphases> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.ResponsibilityPhase.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.ResponsibilityPhase.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => ResponsibilityPhaseMapper.GetResponsibilityPhaseMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDtoRule dto)
        {
            var entityExists = await _repositoryWrapper.ResponsibilityPhase.FindByCondition(
               x => x.Responsibilityphase.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Responsibilityphaseid
                };
            }
            ResponsibilityPhase entity = new ResponsibilityPhase() { ResponsibilityPhaseId = dto.Id, ResponsibilityPhaseDescription = dto.Description, Rule = dto.Rule };
            _repositoryWrapper.ResponsibilityPhase.Create(ResponsibilityPhaseMapper.SetResponsibilityPhaseMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDtoRule dto)
        {
            var entityExists = await _repositoryWrapper.ResponsibilityPhase.FindByCondition(
               x => x.Responsibilityphaseid != dto.Id 
               && x.Responsibilityphase.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Responsibilityphaseid
                };
            }
            ResponsibilityPhase entity = new ResponsibilityPhase() { ResponsibilityPhaseId = dto.Id, ResponsibilityPhaseDescription = dto.Description, Rule = dto.Rule };
            _repositoryWrapper.ResponsibilityPhase.Update(ResponsibilityPhaseMapper.SetResponsibilityPhaseMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ResponsibilityPhase.FindByCondition(x => x.Responsibilityphaseid == id).SingleAsync();
            _repositoryWrapper.ResponsibilityPhase.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Responsibilityphaseid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ResponsibilityPhase.FindByCondition(x => x.Responsibilityphaseid == id).SingleAsync();
            _repositoryWrapper.ResponsibilityPhase.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Responsibilityphaseid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var plannedActivity = _repositoryWrapper.PlannedActivity
                                    .FindByCondition(x => x.Responsibilityphaseid == id)
                                    .Include(x => x.Plannedactivityresource)
                                    .Include(x => x.Activitystatus)
                                    .Include(x => x.Deliverystatus)
                                    .Select(x => PlannedActivityMapper.Get(x,true).toLinkedPlannedActivityName())
                                    .ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivity });

            var entity = await _repositoryWrapper.ResponsibilityPhase.FindByCondition(x => x.Responsibilityphaseid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Responsibility Phase",
                        RecordName = entity.Responsibilityphase,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDtoRule GetCreatePage()
        {
            var dto = new TipologicaGridDtoRule();
            return dto;
        }

        public TipologicaGridDtoRule GetUpdatePage(short id)
        {
            var entity = ResponsibilityPhaseMapper.GetResponsibilityPhaseMapper( _repositoryWrapper.ResponsibilityPhase
                .FindByCondition(x => x.Responsibilityphaseid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDtoRule() { Id = (short)entity.ResponsibilityPhaseId, LastModifiedBy = entity.ModificationUserEntity.Email, Description = entity.ResponsibilityPhaseDescription, LastModified = entity.ModificationDate, Rule = entity.Rule };
            return dto;
        }
    }
}


   