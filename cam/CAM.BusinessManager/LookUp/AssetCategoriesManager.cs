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
using CAM.DataTransferObjects.LookUp.Asset;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CAM.BusinessManager.LookUp
{
    public class AssetCategoriesManager : GridBaseAsync<AssetCategory, AssetCategoryDtoGrid, AssetCategoryDtoQuery, Assetcategories>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public AssetCategoriesManager(IEnumerable<IRepositoryWrapper> wrappers
            , IRepositoryWrapper repositoryWrapper,GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Assetcategories> ApplyFilterForOracleModel(AssetCategoryDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Assetcategories>();
            var predicateInner = PredicateBuilder.New<Assetcategories>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Assetcategories>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Assetcategory == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Assetcategories>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Assetcategoryid == item);
                predicateResult.And(predicateInner);
            }

            if (request.AssetClassId != null && request.AssetClassId.Any())
            {
                predicateInner = PredicateBuilder.New<Assetcategories>();
                foreach (var item in request.AssetClassId)
                    predicateInner.Or(x => x.Assetclassid == item);
                predicateResult.And(predicateInner);
            }

            if (request.TakeFromAssetTypeTable != null && request.TakeFromAssetTypeTable.Any())
            {
                predicateInner = PredicateBuilder.New<Assetcategories>();
                foreach (var item in request.TakeFromAssetTypeTable)
                {
                    if (item == true)
                    {
                        predicateResult = predicateResult.And(x => x.Takefromassettypetable == true);
                    }
                    else
                    {
                        predicateResult = predicateResult.And(x => x.Takefromassettypetable == false);
                    }
                }
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Assetcategories>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Assetcategories>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Assetcategories>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }


        public override List<AssetCategoryDtoGrid> CastObjectToDto(IQueryable<AssetCategory> request)
        {
            return request.Select(dto => new AssetCategoryDtoGrid()
            {
                Id = (short)dto.AssetCategoryId,
                Description = dto.AssetCategoryDescription,
                //AssetClass = dto.AssetClass.AssetClassDescription,
                IdAssetClass = dto.AssetClass.AssetClassId,
                AssetClassId = dto.AssetClass.AssetClassDescription,
                TakeFromAssetTypeTable = dto.TakeFromAssetTypeTable,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<AssetCategory, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetCategory, object>>[]>
            {
                ["description"] = new Expression<Func<AssetCategory, object>>[] { p => p.AssetCategoryDescription },
                ["id"] = new Expression<Func<AssetCategory, object>>[] { p => p.AssetCategoryId },
                ["IdAssetClass"] = new Expression<Func<AssetCategory, object>>[] { p => p.AssetClass.AssetClassId },
                ["assetClassId"] = new Expression<Func<AssetCategory, object>>[] { p => p.AssetClass.AssetClassDescription },
                ["takeFromAssetTypeTable"] = new Expression<Func<AssetCategory, object>>[] { p => p.TakeFromAssetTypeTable },
                ["lastModifiedBy"] = new Expression<Func<AssetCategory, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<AssetCategory> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.AssetCategoryDescription))
                : request.Where(x => x.AssetCategoryDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.AssetCategoryDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.AssetCategoryId.ToString()))
                : request.Where(x => x.AssetCategoryId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.AssetCategoryId.ToString())),
                "assetClassId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.AssetClass.AssetClassId.ToString(), x.AssetClass.AssetClassDescription))
                : request
                    .Where(x => x.AssetClass.AssetClassDescription.ToUpper().Contains(propertyFilter.ToUpper()))
                    .Select(x => new FilterValueDto(x.AssetClass.AssetClassId.ToString(), x.AssetClass.AssetClassDescription)),
                "takeFromAssetTypeTable" => string.IsNullOrEmpty(propertyFilter)
                    ? request
                        .Select(p => new FilterValueDto { Text = p.TakeFromAssetTypeTable ? "YES" : "NO", Value = p.TakeFromAssetTypeTable.ToString() }).Distinct()
                    : request
                        .Where(x => x.TakeFromAssetTypeTable == false)
                        .Select(p => new FilterValueDto { Text = p.TakeFromAssetTypeTable ? "YES" : "NO", Value = p.TakeFromAssetTypeTable.ToString() }).Distinct(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x => x.ModificationUserEntity.Email.Contains(propertyFilter))
                        .Select(p => new FilterValueDto(p.ModificationUserEntity.Email))
                        .Distinct().ToList(),
            };
        }

  
        public override IQueryable<AssetCategory> PrepareQuery(AssetCategoryDtoQuery request, ExpressionStarter<AssetCategory> predicateResult, ExpressionStarter<Assetcategories> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.AssetCategory.FindByCondition(oraclePredicateResult).Include(x => x.Assetclass)
                : _repositoryWrapper.AssetCategory.FindAll().Include(x => x.Assetclass);

            var result = query.Include(m=>m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable()
                .Select(p => AssetCategoryMapper.GetAssetCategoryMapper(p)).AsQueryable();

            return result;
        }
        public async Task<ResultDto> Add(AssetCategoryDto dto)
        {
            var entityExists = await _repositoryWrapper.AssetCategory.FindByCondition(
               x => x.Assetcategory.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ","")
               && x.Assetclassid == dto.IdAssetClass, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Assetcategoryid
                };
            }
            AssetCategory entity = new AssetCategory()
            {
                AssetCategoryId = dto.Id,
                AssetCategoryDescription = dto.Description,
                AssetClassId = dto.IdAssetClass,
                TakeFromAssetTypeTable = dto.TakeFromAssetTypeTable
            };

           
            _repositoryWrapper.AssetCategory.Create(AssetCategoryMapper.SetAssetCategoryMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(AssetCategoryDto dto)
        {
            var entityExists = await _repositoryWrapper.AssetCategory.FindByCondition(
               x => x.Assetcategoryid != dto.Id 
               && x.Assetcategory == dto.Description
               && x.Assetclassid == dto.IdAssetClass
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Assetcategoryid
                };
            }
            AssetCategory entity = new AssetCategory()
            {
                AssetCategoryId = dto.Id,
                AssetCategoryDescription = dto.Description,
                AssetClassId = dto.IdAssetClass,
                TakeFromAssetTypeTable = dto.TakeFromAssetTypeTable
            };
            _repositoryWrapper.AssetCategory.Update(AssetCategoryMapper.SetAssetCategoryMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.AssetCategory.FindByCondition(x => x.Assetcategoryid == id).SingleAsync();
            _repositoryWrapper.AssetCategory.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Assetcategoryid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.AssetCategory.FindByCondition(x => x.Assetcategoryid == id).SingleAsync();
            _repositoryWrapper.AssetCategory.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Assetcategoryid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var assetType = _repositoryWrapper.AssetType.FindByCondition(x => x.Assetcategoryid == id).Select(x => x.Assettype).ToArray();
            var systemType = _repositoryWrapper.SystemType.FindByCondition(x => x.Assetcategoryid == id).Select(x => x.toSystemTypeName(_repositoryWrapper)).ToArray();
            var entity = await _repositoryWrapper.AssetCategory.FindByCondition(x => x.Assetcategoryid == id).SingleAsync();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (assetType.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Asset Class", Values = assetType });
            if (systemType.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Type", Values = systemType });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Asset Category",
                        RecordName = entity.Assetcategory,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();

        }

        public AssetCategoryDto GetCreatePage()
        {
            var dto = new AssetCategoryDto
            {
                AssetClassResource = _repositoryWrapper.AssetClass.FindAll().ToDictionary(x => x.Assetclassid, x => x.Assetclass),
                TakeFromAssetTypeTable = true
            };
            return dto;
        }

        public AssetCategoryDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.AssetCategory.FindByCondition(x => x.Assetcategoryid == id).Include(x => x.ModificationuserNavigation).Include(x => x.Assetclass).Single();

            var entity = AssetCategoryMapper.GetAssetCategoryMapper(model);
            var dto = new AssetCategoryDto() 
            { 
                Id = (short)entity.AssetCategoryId, 
                Description = entity.AssetCategoryDescription,
                AssetClassId = entity.AssetClass.AssetClassDescription,
                IdAssetClass = entity.AssetClassId,
                TakeFromAssetTypeTable = entity.TakeFromAssetTypeTable,
                LastModified = entity.ModificationDate, 
                LastModifiedBy = entity.ModificationUserEntity.Email,

                AssetClassResource = _repositoryWrapper.AssetClass.FindAll().ToDictionary(x => x.Assetclassid, x => x.Assetclass),
            };
            return dto;
        }

    
    }
}
