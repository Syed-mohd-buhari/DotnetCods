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
    public class AssetClassManager : GridBaseAsync<AssetClass, AssetClassDtoGrid, AssetClassDtoQuery, Assetclasses>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public AssetClassManager(IEnumerable<IRepositoryWrapper> wrappers, IRepositoryWrapper repositoryWrapper,
            GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor) : base(columnManager,contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

   

        public override ExpressionStarter<Assetclasses> ApplyFilterForOracleModel(AssetClassDtoQuery request)
        {
            var predicateResult = PredicateBuilder.New<Assetclasses>();
            var predicateInner = PredicateBuilder.New<Assetclasses>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Assetclasses>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Assetclass == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Assetclasses>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Assetclassid == item);
                predicateResult.And(predicateInner);
            }

            //if (request.AssetCategoryId != null && request.AssetCategoryId.Any())
            //{
            //    predicateInner = PredicateBuilder.New<AssetClass>();
            //    foreach (var item in request.AssetCategoryId)
            //        predicateInner.Or(x => x.AssetCategoryId == item);
            //    predicateResult.And(predicateInner);
            //}

            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Assetclasses>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Assetclasses>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Assetclasses>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }
        public override List<AssetClassDtoGrid> CastObjectToDto(IQueryable<AssetClass> request)
        {
            return  request.Select(dto => new AssetClassDtoGrid()
            {
                Id = (short)dto.AssetClassId,
                Description = dto.AssetClassDescription,
                LastModified = dto.ModificationDate,
                //IdAssetCategory = dto.AssetCategoryId,
                //AssetCategoryId = dto.AssetCategory.AssetCategoryDescription,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<AssetClass, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetClass, object>>[]>
            {
                ["description"] = new Expression<Func<AssetClass, object>>[] { p => p.AssetClassDescription },
                ["id"] = new Expression<Func<AssetClass, object>>[] { p => p.AssetClassId },
                //["assetCategoryId"] = new Expression<Func<AssetClass, object>>[] { p => p.AssetCategory.AssetCategoryDescription },
                ["lastModifiedBy"] = new Expression<Func<AssetClass, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<AssetClass> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.AssetClassDescription))
                : request.Where(x => x.AssetClassDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.AssetClassDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.AssetClassId.ToString()))
                : request.Where(x => x.AssetClassId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.AssetClassId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                //"assetCategoryId" => string.IsNullOrEmpty(propertyFilter)
                //? request.Select(x => new FilterValueDto(x.AssetCategoryId.ToString(), x.AssetCategory.AssetCategoryDescription))
                //: request.Where(x => x.AssetCategory.AssetCategoryDescription.ToString() == propertyFilter).Select(x => new FilterValueDto(x.AssetCategoryId.ToString(), x.AssetCategory.AssetCategoryDescription)),
            };
        }

        public override IQueryable<AssetClass> PrepareQuery(AssetClassDtoQuery request, ExpressionStarter<AssetClass> predicateResult , ExpressionStarter<Assetclasses> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.AssetClass.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.AssetClass.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => AssetClassMapper.GetAssetClassMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(AssetClassDto dto)
        {
           
            AssetClass entity = new AssetClass() { AssetClassId = dto.Id, AssetClassDescription = dto.Description /*, AssetCategoryId = dto.AssetCategoryId*/ };

            var model = AssetClassMapper.SetAssetClassMapper(entity);
            _repositoryWrapper.AssetClass.Create(model);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(AssetClassDto dto)
        {
           
            AssetClass entity = new AssetClass() { AssetClassId = dto.Id, AssetClassDescription = dto.Description/*, AssetCategoryId = dto.AssetCategoryId*/ };
            var model = AssetClassMapper.SetAssetClassMapper(entity);
            _repositoryWrapper.AssetClass.Update(model);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.AssetClass.FindByCondition(x => x.Assetclassid == id).SingleAsync();
            _repositoryWrapper.AssetClass.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Assetclassid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.AssetClass.FindByCondition(x => x.Assetclassid == id).SingleAsync();
            _repositoryWrapper.AssetClass.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Assetclassid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var AssetCategory = _repositoryWrapper.AssetCategory.FindByCondition(x => x.Assetclassid == id).Select(x => x.Assetcategory).ToArray();
            var SystemTypes = _repositoryWrapper.SystemType.FindByCondition(x => x.Assetclassid == id).Select(x => x.toSystemTypeName(_repositoryWrapper)).ToArray();
            var entity = await _repositoryWrapper.AssetClass.FindByCondition(x => x.Assetclassid == id).SingleAsync();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (AssetCategory.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Asset Category", Values = AssetCategory });
            }
            if (SystemTypes.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "System Types", Values = SystemTypes });
            }

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Asset Class",
                        RecordName = entity.Assetclass,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public AssetClassDto GetCreatePage()
        {
            var relaterResource = _repositoryWrapper.AssetCategory.FindAll();
            var dto = new AssetClassDto();
            //dto.AssetCategoryResource = relaterResource.ToDictionary(x => x.AssetCategoryId, x => x.AssetCategoryDescription);

            return dto;
        }

        public AssetClassDto GetUpdatePage(short id)
        {
            //var relaterResource = _repositoryWrapper.AssetCategory.FindAll();
            var model = _repositoryWrapper.AssetClass.FindByCondition(x => x.Assetclassid == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = AssetClassMapper.GetAssetClassMapper(model);
            var dto = new AssetClassDto()
            {
                Id = entity.AssetClassId,
                Description = entity.AssetClassDescription,
                LastModifiedBy = entity.ModificationUserEntity.Email,
                LastModified = entity.ModificationDate
               
            };
      
            return dto;
        }

      
    }


}
