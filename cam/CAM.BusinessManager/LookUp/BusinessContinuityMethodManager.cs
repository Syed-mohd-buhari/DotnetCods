using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
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
    public class BusinessContinuityMethodManager : GridBaseAsync<BusinessContinuityMethod, TipologicaGridDto, TipologicaQueryDto, Businesscontinuitymethod>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public BusinessContinuityMethodManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }


        public override ExpressionStarter<Businesscontinuitymethod> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Businesscontinuitymethod>();
            var predicateInner = PredicateBuilder.New<Businesscontinuitymethod>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Businesscontinuitymethod>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Businesscontinuitymethod>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Businesscontinuitymethod>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Businesscontinuitymethod>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Businesscontinuitymethod>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<BusinessContinuityMethod> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = (short)dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<BusinessContinuityMethod, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<BusinessContinuityMethod, object>>[]>
            {
                ["description"] = new Expression<Func<BusinessContinuityMethod, object>>[] { p => p.Description },
                ["id"] = new Expression<Func<BusinessContinuityMethod, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<BusinessContinuityMethod, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<BusinessContinuityMethod> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description))
                    : request.Where(x => x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                    : request.Where(x => x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<BusinessContinuityMethod> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<BusinessContinuityMethod> predicateResult, ExpressionStarter<Businesscontinuitymethod> oraclePredicateResult = null)
        {


            var query = oraclePredicateResult.IsStarted
              ? _repositoryWrapper.BusinessContinuityMethodRepository.FindByCondition(oraclePredicateResult)
              : _repositoryWrapper.BusinessContinuityMethodRepository.FindAll();


            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => BusinessContinuityMethodMapper.Get(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.BusinessContinuityMethodRepository.FindByCondition(
               x => x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            BusinessContinuityMethod entity = new BusinessContinuityMethod() { Id = dto.Id, Description = dto.Description };
            _repositoryWrapper.BusinessContinuityMethodRepository.Create(BusinessContinuityMethodMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.BusinessContinuityMethodRepository.FindByCondition(
               x => x.Id != dto.Id && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            BusinessContinuityMethod entity = new BusinessContinuityMethod() { Id = dto.Id, Description = dto.Description };

            _repositoryWrapper.BusinessContinuityMethodRepository.Update(BusinessContinuityMethodMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.BusinessContinuityMethodRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.BusinessContinuityMethodRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.BusinessContinuityMethodRepository.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.BusinessContinuityMethodRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var entities = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Businesscontinuitymethodid == id)
              .Include(p => p.Businesscontinuitymethod)
              .Include(p => p.Designcomponentfamily)
              .Include(p => p.Opco).Select(p => p.toDesignAspectName(_repositoryWrapper)).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Design Aspect", Values = entities });
            var entity = await _repositoryWrapper.BusinessContinuityMethodRepository.FindByCondition(x => x.Id == id)
                .SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Business Continuity Method",
                        RecordName = entity.Description,
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
            var model = _repositoryWrapper.BusinessContinuityMethodRepository.FindByCondition(x => x.Id == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = BusinessContinuityMethodMapper.Get(model);
            var dto = new TipologicaGridDto()
            {
                Id = (short)entity.Id,
                Description = entity.Description,
                LastModified = entity.ModificationDate,
                LastModifiedBy = entity.ModificationUserEntity.Email
            };
            return dto;
        }

    }
}
