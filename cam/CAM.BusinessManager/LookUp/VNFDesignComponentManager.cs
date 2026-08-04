using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.LookUp
{
    public class VNFDesignComponentManager : GridBaseAsync<VNFDesignComponent, TipologicaGridDto, TipologicaQueryDto , Vfndesigncomponents>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public VNFDesignComponentManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
   
        public override ExpressionStarter<Vfndesigncomponents> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Vfndesigncomponents>();
            var predicateInner = PredicateBuilder.New<Vfndesigncomponents>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Vfndesigncomponents>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Designcomponent == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Vfndesigncomponents>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Vnfdesigncomponentid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Vfndesigncomponents>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Vfndesigncomponents>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Vfndesigncomponents>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<VNFDesignComponent> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.VNFDesignComponentId,
                Description = dto.VNFDesignComponentDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<VNFDesignComponent, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VNFDesignComponent, object>>[]>
            {
                ["description"] = new Expression<Func<VNFDesignComponent, object>>[] { p => p.VNFDesignComponentDescription },
                ["lastModifiedBy"] = new Expression<Func<VNFDesignComponent, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<VNFDesignComponent, object>>[] { p => p.VNFDesignComponentId }

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<VNFDesignComponent> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.VNFDesignComponentDescription))
                : request.Where(x => x.VNFDesignComponentDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.VNFDesignComponentDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.VNFDesignComponentId.ToString()))
                : request.Where(x => x.VNFDesignComponentId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.VNFDesignComponentId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<VNFDesignComponent> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<VNFDesignComponent> predicateResult , ExpressionStarter<Vfndesigncomponents> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.VNFDesignComponent.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.VNFDesignComponent.FindAll();
            return  query.Include(m => m.CreationuserNavigation)
                .Include(m => m.ModificationuserNavigation)
                .AsEnumerable().Select(p => VNFDesignComponentMapper.GetVNFDesignComponentMapper(p)).AsQueryable() ;
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.VNFDesignComponent.FindByCondition(
               x => x.Designcomponent == dto.Description, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnfdesigncomponentid
                };
            }
            VNFDesignComponent entity = new VNFDesignComponent() { VNFDesignComponentId = dto.Id, VNFDesignComponentDescription = dto.Description };
            _repositoryWrapper.VNFDesignComponent.Create(VNFDesignComponentMapper.SetVNFDesignComponentMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.VNFDesignComponent.FindByCondition(
               x => x.Vnfdesigncomponentid != dto.Id && x.Designcomponent == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnfdesigncomponentid
                };
            }
            VNFDesignComponent entity = new VNFDesignComponent() { VNFDesignComponentId = dto.Id, VNFDesignComponentDescription = dto.Description };
            _repositoryWrapper.VNFDesignComponent.Update(VNFDesignComponentMapper.SetVNFDesignComponentMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.VNFDesignComponent.FindByCondition(x => x.Vnfdesigncomponentid == id).SingleAsync();
            _repositoryWrapper.VNFDesignComponent.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vnfdesigncomponentid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.VNFDesignComponent.FindByCondition(x => x.Vnfdesigncomponentid == id).SingleAsync();
            _repositoryWrapper.VNFDesignComponent.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vnfdesigncomponentid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var VNFTransition = _repositoryWrapper.VNFTransition.FindByCondition(x => x.Vnfdesigncomponentid == id).Select(x => x.Vnftype).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (VNFTransition.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "VNF Transition", Values = VNFTransition });

            var entity = await _repositoryWrapper.VNFDesignComponent.FindByCondition(x => x.Vnfdesigncomponentid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "VNF Design Component",
                        RecordName = entity.Designcomponent,
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
            var entity = VNFDesignComponentMapper.GetVNFDesignComponentMapper( _repositoryWrapper.VNFDesignComponent.FindByCondition(x => x.Vnfdesigncomponentid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.VNFDesignComponentId, Description = entity.VNFDesignComponentDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }
    }
}



    