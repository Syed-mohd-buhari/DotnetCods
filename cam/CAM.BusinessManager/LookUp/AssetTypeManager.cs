using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
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
using CAM.DataTransferObjects.LookUp.Asset;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class AssetTypeManager : GridBaseAsync<AssetType, AssetTypeDtoGrid, AssetTypeDtoQuery, Assettypes>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public AssetTypeManager(IEnumerable<IRepositoryWrapper> wrappers,
            GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

     
        public override ExpressionStarter<Assettypes> ApplyFilterForOracleModel(AssetTypeDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Assettypes>();
            var predicateInner = PredicateBuilder.New<Assettypes>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Assettypes>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Assettype == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Assettypes>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Assettypeid == item);
                predicateResult.And(predicateInner);
            }

            if (request.AssetCategoryId != null && request.AssetCategoryId.Any())
            {
                predicateInner = PredicateBuilder.New<Assettypes>();
                foreach (var item in request.AssetCategoryId)
                    predicateInner.Or(x => x.Assetcategoryid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Assettypes>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null )
            {
                predicateInner = PredicateBuilder.New<Assettypes>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Assettypes>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }

        public override List<AssetTypeDtoGrid> CastObjectToDto(IQueryable<AssetType> request)
        {
            
            var result = request.Select(dto => new AssetTypeDtoGrid()
            {
                Id = (short)dto.AssetTypeId,
                Description = dto.AssetTypeDescription,
                LastModified = dto.ModificationDate,
                IdAssetCategory = dto.AssetCategory != null ? dto.AssetCategory.AssetCategoryId : (int?)null,
                AssetCategoryId = dto.AssetCategory!= null ? dto.AssetCategory.AssetCategoryDescription :"",
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
           
            return result;
        }

        public override Dictionary<string, Expression<Func<AssetType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetType, object>>[]>
            {
                ["description"] = new Expression<Func<AssetType, object>>[] { p => p.AssetTypeDescription },
                ["id"] = new Expression<Func<AssetType, object>>[] { p => p.AssetTypeId },
                ["idAssetCategory"] = new Expression<Func<AssetType, object>>[] { p => p.AssetCategory.AssetCategoryId },
                ["assetCategoryId"] = new Expression<Func<AssetType, object>>[] { p => p.AssetCategory.AssetCategoryId },
                ["lastModifiedBy"] = new Expression<Func<AssetType, object>>[] { p => p.ModificationUserEntity.Email },



            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<AssetType> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.AssetTypeDescription))
                : request.Where(x => x.AssetTypeDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.AssetTypeDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.AssetTypeId.ToString()))
                : request.Where(x => x.AssetTypeId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.AssetTypeId.ToString())),
                "assetCategoryId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.AssetCategory.AssetCategoryId.ToString(), x.AssetCategory.AssetCategoryDescription))
                : request
                    .Where(x => x.AssetCategory.AssetCategoryDescription.ToUpper().Contains(propertyFilter.ToUpper()))
                    .Select(x => new FilterValueDto(x.AssetCategory.AssetCategoryId.ToString(), x.AssetCategory.AssetCategoryDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x => x.ModificationUserEntity.Email.Contains(propertyFilter))
                        .Select(p => new FilterValueDto(p.ModificationUserEntity.Email))
                        .Distinct().ToList(),
            };
        }

        public override IQueryable<AssetType> PrepareQuery(AssetTypeDtoQuery request, ExpressionStarter<AssetType> predicateResult , ExpressionStarter<Assettypes> oracleObject = null)
        {
            var query = oracleObject.IsStarted
               ? _repositoryWrapper.AssetType.FindByCondition(oracleObject).Include(x => x.Assetcategory)
               : _repositoryWrapper.AssetType.FindAll().Include(x => x.Assetcategory);
           
            
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => AssetTypeMapper.GetAssetTypeMapper(p)).AsQueryable();

          
        }

        public async Task<ResultDto> Add(AssetTypeDto dto)
        {
            var entityExists = await _repositoryWrapper.AssetType.FindByCondition(
               x => x.Assettype.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ","")).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Assettypeid
                };
            }
            AssetType entity = new AssetType() { AssetTypeId = dto.Id, AssetTypeDescription = dto.Description, AssetCategoryId = dto.IdAssetCategory };
            var savedObject = AssetTypeMapper.SetAssetTypeMapper(entity);

            _repositoryWrapper.AssetType.Create(savedObject);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(AssetTypeDto dto)
        {
            var entityExists = await _repositoryWrapper.AssetType.FindByCondition(
               x => x.Assettypeid != dto.Id 
               && x.Assettype.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Assettypeid
                };
            }
            AssetType entity = new AssetType() { AssetTypeId = dto.Id, AssetTypeDescription = dto.Description, AssetCategoryId = dto.IdAssetCategory };
            var savedObject = AssetTypeMapper.SetAssetTypeMapper(entity);
            _repositoryWrapper.AssetType.Update(savedObject);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.AssetType.FindByCondition(x => x.Assettypeid == id).SingleAsync();
            _repositoryWrapper.AssetType.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Assettypeid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.AssetType.FindByCondition(x => x.Assettypeid == id).SingleAsync();
            _repositoryWrapper.AssetType.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Assettypeid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var entities = _repositoryWrapper.SystemType.FindByCondition(x => x.Assettypeid == id).Select(x => x.toSystemTypeName(_repositoryWrapper)).ToArray();
            var entity = await _repositoryWrapper.AssetType.FindByCondition(x => x.Assettypeid == id).SingleAsync();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Type", Values = entities });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Asset Type",
                        RecordName = entity.Assettype,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public AssetTypeDto GetCreatePage()
        {
            var relaterResource = _repositoryWrapper.AssetCategory.FindAll();
            var dto = new AssetTypeDto();
            dto.AssetCategoryResource = relaterResource.ToDictionary(x => x.Assetcategoryid, x => x.Assetcategory);

            return dto;
        }

        public AssetTypeDto GetUpdatePage(short id)
        {
            var relaterResource = _repositoryWrapper.AssetCategory.FindAll();
            var model = _repositoryWrapper.AssetType
                .FindByCondition(x => x.Assettypeid == id)
                .Include(x => x.ModificationuserNavigation).Single();

            var entity = AssetTypeMapper.GetAssetTypeMapper(model);

            var dto = new AssetTypeDto()
            {
                Id = entity.AssetTypeId,
                Description = entity.AssetTypeDescription,
                LastModified = entity.ModificationDate,
                AssetCategoryId = entity.AssetCategoryId,
                IdAssetCategory = entity.AssetCategoryId,
                LastModifiedBy = entity.ModificationUserEntity.Email
            };
            dto.AssetCategoryResource = relaterResource.ToDictionary(x => x.Assetcategoryid, x => x.Assetcategory);
            //if (!dto.AssetCategoryResource.ContainsKey(dto.AssetCategoryId))
            //{
            //    var data = _repositoryWrapper.AssetCategory.FindByCondition(
            //        x => x.AssetCategoryId == dto.AssetCategoryId, true).SingleOrDefault();
            //    if (data != null)
            //    {
            //        dto.AssetCategoryResource.Add(data.AssetCategoryId, data.AssetCategoryDescription);
            //    }
            //}
            return dto;
        }

     
    }
}
