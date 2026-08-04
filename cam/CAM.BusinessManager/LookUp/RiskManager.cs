using AutoMapper;
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
    public class RiskManager : GridBaseAsync<RiskResource, OperationalRiskDto, OperationalRiskQueryDto, Risk>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public RiskManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }

   

        public override ExpressionStarter<Risk> ApplyFilterForOracleModel(OperationalRiskQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Risk>();
            ExpressionStarter<Risk> predicateInner;

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Risk>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Risk>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Riskid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Risk>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.Severity != null && request.Severity.Any())
            {
                predicateInner = PredicateBuilder.New<Risk>();
                foreach (var item in request.Severity)
                    predicateInner.Or(x => x.Severity == item);
                predicateResult.And(predicateInner);
            }



            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Risk>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Risk>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<OperationalRiskDto> CastObjectToDto(IQueryable<RiskResource> request)
        {
            return  request.Select(dto => new OperationalRiskDto()
            {
                Id = dto.RiskId,
                Description = dto.RiskDescription,
                LastModified = dto.ModificationDate,
                Severity = dto.Severity,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<RiskResource, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<RiskResource, object>>[]>
            {
                ["description"] = new Expression<Func<RiskResource, object>>[] { p => p.RiskDescription },
                ["id"] = new Expression<Func<RiskResource, object>>[] { p => p.RiskId },
                ["lastModifiedBy"] = new Expression<Func<RiskResource, object>>[] { p => p.ModificationUserEntity.Email },
                ["severity"] = new Expression<Func<RiskResource, object>>[] { p => p.Severity },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<RiskResource> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.RiskDescription)) : request.Where(x =>
                      x.RiskDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.RiskDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.RiskId.ToString()))
                : request.Where(x =>
                    x.RiskId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.RiskId.ToString())),
                "severity" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.Severity.ToString()))
                : request.Where(x =>
                    x.Severity.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Severity.ToString())),

            };
        }

        public override IQueryable<RiskResource> PrepareQuery(OperationalRiskQueryDto request, ExpressionStarter<RiskResource> predicateResult , ExpressionStarter<Risk> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.Risk.FindByCondition(oraclePredicateResult)

                : _repositoryWrapper.Risk.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => RiskMapper.GetRiskMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(OperationalRiskDto dto)
        {
            var entityExists = await _repositoryWrapper.Risk.FindByCondition(
               x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Riskid
                };
            }
            var allRisk = _repositoryWrapper.Risk.FindAll().OrderByDescending(x => x.Severity).FirstOrDefault();
            var severityMax = allRisk != null ? allRisk.Severity : 0;
            RiskResource entity = new RiskResource() { RiskId = dto.Id, RiskDescription = dto.Description, Severity = (severityMax + 1) };
            _repositoryWrapper.Risk.Create(RiskMapper.SetRiskMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(OperationalRiskDto dto)
        {
            var entityExists = await _repositoryWrapper.Risk.FindByCondition(
               x => x.Riskid != dto.Id && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Riskid
                };
            }
            RiskResource entity = new RiskResource() { RiskId = dto.Id, RiskDescription = dto.Description, Severity = dto.Severity };
            _repositoryWrapper.Risk.Update(RiskMapper.SetRiskMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }


        public async Task<ResultDto> ChangeGridOrderOperationalRisk(List<ChangeGridOrderDto> lista)
        {
            var allEntityExist = await _repositoryWrapper.Risk.FindAll().ToListAsync();            
            foreach(var item in lista)
            {
                RiskResource resource = RiskMapper.GetRiskMapper(allEntityExist.Single(x => x.Riskid == item.Id));
                resource.Severity = item.Order;
                _repositoryWrapper.Risk.Update(RiskMapper.SetRiskMapper(resource));

            }   
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.Risk.FindByCondition(x => x.Riskid == id).SingleAsync();
            _repositoryWrapper.Risk.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Riskid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.Risk.FindByCondition(x => x.Riskid == id).SingleAsync();
            _repositoryWrapper.Risk.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Riskid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var plannedActivity = _repositoryWrapper.PlannedActivity
                                    .FindByCondition(x => x.Operationalriskid == id)
                                    .Include(x => x.Plannedactivityresource)
                                    .Include(x => x.Activitystatus)
                                    .Include(x => x.Deliverystatus)
                                    .Select(x => PlannedActivityMapper.Get(x,true).toLinkedPlannedActivityName())
                                    .ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivity });

            var entity = await _repositoryWrapper.Risk.FindByCondition(x => x.Riskid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Risk",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public OperationalRiskDto GetCreatePage()
        {
            var opCoDto = new OperationalRiskDto();
            return opCoDto;
        }

        public OperationalRiskDto GetUpdatePage(short id)
        {
            var entity = RiskMapper.GetRiskMapper(_repositoryWrapper.Risk.FindByCondition(x => x.Riskid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new OperationalRiskDto() { Id = entity.RiskId, Description = entity.RiskDescription, LastModified = entity.ModificationDate, Severity = entity.Severity, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }
        

           
    }
}
