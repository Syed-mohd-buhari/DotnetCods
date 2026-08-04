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
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
   public class ProductImportanceManager : GridBaseAsync<ProductImportance, TipologicaGridDto, TipologicaQueryDto, Productimportances>
   {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public ProductImportanceManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
       
        public override ExpressionStarter<Productimportances> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Productimportances>();
            var predicateInner = PredicateBuilder.New<Productimportances>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Productimportances>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Productimportance == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Productimportances>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Productimportanceid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Productimportances>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Productimportances>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Productimportances>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<ProductImportance> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.ProductImportanceId,
                Description = dto.ProductImportanceDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<ProductImportance, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ProductImportance, object>>[]>
            {
                ["description"] = new Expression<Func<ProductImportance, object>>[] { p => p.ProductImportanceDescription },
                ["id"] = new Expression<Func<ProductImportance, object>>[] { p => p.ProductImportanceId },
                ["lastModifiedBy"] = new Expression<Func<ProductImportance, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<ProductImportance> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ProductImportanceDescription))
                : request.Where(x => x.ProductImportanceDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ProductImportanceDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.ProductImportanceId.ToString()))
                : request.Where(x => x.ProductImportanceId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.ProductImportanceId.ToString())),
            };
        }

        public override IQueryable<ProductImportance> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<ProductImportance> predicateResult, ExpressionStarter<Productimportances> oracleResult = null)
        {
            var query = oracleResult.IsStarted
               ? _repositoryWrapper.ProductImportance.FindByCondition(oracleResult)
               : _repositoryWrapper.ProductImportance.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => ProductImportanceMapper.GetProductImportanceMapper(p)).AsQueryable();
        }
        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.ProductImportance.FindByCondition(
               x =>  x.Productimportance.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Productimportanceid
                };
            }
            ProductImportance entity = new ProductImportance() { ProductImportanceId = dto.Id, ProductImportanceDescription = dto.Description };
            _repositoryWrapper.ProductImportance.Create(ProductImportanceMapper.SetProductImportanceMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.ProductImportance.FindByCondition(
               x => x.Productimportanceid != dto.Id 
               && x.Productimportance.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value)
               .FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Productimportanceid
                };
            }
            ProductImportance entity = new ProductImportance() { ProductImportanceId = dto.Id, ProductImportanceDescription = dto.Description };
            _repositoryWrapper.ProductImportance.Update(ProductImportanceMapper.SetProductImportanceMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportanceid == id).SingleAsync();
            _repositoryWrapper.ProductImportance.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Productimportanceid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportanceid == id).SingleAsync();
            _repositoryWrapper.ProductImportance.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Productimportanceid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var SystemTypes = _repositoryWrapper.SystemType
                              .FindByCondition(x => x.Productimportanceid == id)
                              .Include(k=>k.Majorsoftwarebuilds)
                              .Select(x => x.toSystemTypeName(_repositoryWrapper)).ToArray();

            var Lcmengineering = _repositoryWrapper.Lcmengineering
               .FindByCondition(x => x.Productimportanceid == id)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
               .Include(x => x.Opco)
               .Select(x => x.toDescription(_repositoryWrapper)).ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (Lcmengineering.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "LCM Engineering", Values = Lcmengineering });

            if (SystemTypes.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Types", Values = SystemTypes });

            var entity = await _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportanceid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Product Importance",
                        RecordName = entity.Productimportance,
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
            var entity = ProductImportanceMapper.GetProductImportanceMapper( _repositoryWrapper.ProductImportance
                .FindByCondition(x => x.Productimportanceid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = (short)entity.ProductImportanceId, LastModifiedBy = entity.ModificationUserEntity.Email, Description = entity.ProductImportanceDescription, LastModified = entity.ModificationDate };
            return dto;
        }
    }
}



 