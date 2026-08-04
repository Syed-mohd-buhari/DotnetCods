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
    public class BenefitManager : GridBaseAsync<Benefit, TipologicaGridDto, TipologicaQueryDto, Benefits>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public BenefitManager(IEnumerable<IRepositoryWrapper> wrappers,
            GridCustomColumnManager columnManager,IRepositoryWrapper repositoryWrapper,
            IHttpContextAccessor contextAccessor) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

      
        public override ExpressionStarter<Benefits> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Benefits>();
            var predicateInner = PredicateBuilder.New<Benefits>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Benefits>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Benefit == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Benefits>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Benefitid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Benefits>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Benefits>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Benefits>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<Benefit> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = (short)dto.BenefitId,
                Description = dto.BenefitDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<Benefit, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Benefit, object>>[]>
            {
                ["description"] = new Expression<Func<Benefit, object>>[] { p => p.BenefitDescription },
                ["id"] = new Expression<Func<Benefit, object>>[] { p => p.BenefitId },
                ["lastModifiedBy"] = new Expression<Func<Benefit, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<Benefit> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.BenefitDescription))
                    : request.Where(x => x.BenefitDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.BenefitDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.BenefitId.ToString()))
                    : request.Where(x => x.BenefitId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.BenefitId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<Benefit> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<Benefit> predicateResult, ExpressionStarter<Benefits> oraclePredicateResult = null)
        {
           

            var query = oraclePredicateResult.IsStarted
              ? _repositoryWrapper.Benefit.FindByCondition(oraclePredicateResult)
              : _repositoryWrapper.Benefit.FindAll();


            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => BenefitMapper.GetBenefitMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.Benefit.FindByCondition(
               x => x.Benefitid == dto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Benefitid
                };
            }
            Benefit entity = new Benefit() { BenefitId = dto.Id, BenefitDescription = dto.Description };
            _repositoryWrapper.Benefit.Create(BenefitMapper.SetBenefitMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.Benefit.FindByCondition(
               x => x.Benefitid != dto.Id && x.Benefit == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Benefitid
                };
            }
            Benefit entity = new Benefit() { BenefitId = dto.Id, BenefitDescription = dto.Description };

            _repositoryWrapper.Benefit.Update(BenefitMapper.SetBenefitMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.Benefit.FindByCondition(x => x.Benefitid == id).SingleAsync();
            _repositoryWrapper.Benefit.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Benefitid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.Benefit.FindByCondition(x => x.Benefitid == id).SingleAsync();
            _repositoryWrapper.Benefit.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Benefitid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - verificare che non esistono relazioni
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.Benefit.FindByCondition(x => x.Benefitid == id).SingleAsync();
            var plannedResources = _repositoryWrapper.PlannedActivityResourceBenefit
                .FindByCondition(x => x.Benefitid == entity.Benefitid)
                .Include(x => x.Plannedactivityresource)
                .Select(x => x.Plannedactivityresource.Plannedactivityresource)
                .ToArray();

            var plannedActivities = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Benefitid == entity.Benefitid)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                .ToArray();

            if (plannedActivities.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivities });
            }

            if (plannedResources.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Planned Activity Resource", Values = plannedResources });
            }

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Benefit",
                        RecordName = entity.Benefit,
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
            var model = _repositoryWrapper.Benefit.FindByCondition(x => x.Benefitid == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = BenefitMapper.GetBenefitMapper(model);
            var dto = new TipologicaGridDto() { 
                Id = (short)entity.BenefitId, 
                Description = entity.BenefitDescription, 
                LastModified = entity.ModificationDate, 
                LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

      
    }
}
