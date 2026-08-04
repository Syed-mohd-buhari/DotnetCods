using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.BusinessManager.ExtensionMethod.MajorHardwareBuild;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.LookUp
{
   public class BuildConstructionManager : GridBaseAsync<BuildConstruction, BuildConstructionRule, TipologicaQueryDtoRule, Buildconstructions>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public BuildConstructionManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

    
        public override ExpressionStarter<Buildconstructions> ApplyFilterForOracleModel(TipologicaQueryDtoRule request)
        {
            var predicateResult = PredicateBuilder.New<Buildconstructions>();
            var predicateInner = PredicateBuilder.New<Buildconstructions>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Buildconstruction == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Buildconstructionid == item);
                predicateResult.And(predicateInner);
            }

            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            if (request.CloudTypeBuild != null && request.CloudTypeBuild.Any())
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                foreach (var item in request.CloudTypeBuild)
                    predicateInner.Or(x => x.Cloudtype == item);
                predicateResult.And(predicateInner);
            }
            if (request.IsCloudHostedAsset != null && request.IsCloudHostedAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Buildconstructions>();
                foreach (var item in request.IsCloudHostedAsset)
                    if (item.ToLower() == "yes")
                    {
                        predicateInner.Or(x => x.Iscloudasset == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Iscloudasset == false);
                    }

                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<BuildConstructionRule> CastObjectToDto(IQueryable<BuildConstruction> request)
        {
            return request.Select(dto => new BuildConstructionRule()
            {
                Id = dto.BuildConstructionId,
                Description = dto.BuildConstructionDescription,
                LastModified = dto.ModificationDate,
                Rule = dto.Rule,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                CloudTypeBuild = dto.CloudType,
                IsCloudHostedAsset = dto.IsCluodHostedAsset == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                RuleDescription= ConstantValueFilter.buildConstructionRule.FirstOrDefault(z => z.Key == dto.Rule).Text,
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<BuildConstruction, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<BuildConstruction, object>>[]>
            {
                ["description"] = new Expression<Func<BuildConstruction, object>>[] { p => p.BuildConstructionDescription },
                ["id"] = new Expression<Func<BuildConstruction, object>>[] { p => p.BuildConstructionId },
                ["rule"] = new Expression<Func<BuildConstruction, object>>[] { p => p.Rule },
                ["cloudTypeBuild"] = new Expression<Func<BuildConstruction, object>>[] { p => p.CloudType },
                ["lastModifiedBy"] = new Expression<Func<BuildConstruction, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<BuildConstruction> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.BuildConstructionDescription))
                : request.Where(x => x.BuildConstructionDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.BuildConstructionDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.BuildConstructionId.ToString()))
                : request.Where(x => x.BuildConstructionId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.BuildConstructionId.ToString())),
                //"rule" => string.IsNullOrEmpty(propertyFilter)
                //? request.Select(x => new FilterValueDto(x.Rule.ToString()))
                //: request.Where(x => propertyFilter.Contains(x.Rule.ToString())).Select(x => new FilterValueDto(x.Rule.ToString())),

                "rule" => request.Where(x=>x.Rule != null )
                .Select(y=>new FilterValueDto {
                    Text= ConstantValueFilter.buildConstructionRule.FirstOrDefault(z=>z.Key==y.Rule).Text,
                    Value = Convert.ToString(y.Rule),
                }),

                "cloudTypeBuild" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.CloudType))
                : request.Where(x => propertyFilter.Contains(x.CloudType)).Select(x => new FilterValueDto(x.CloudType)),
                "isCloudHostedAsset" => string.IsNullOrEmpty(propertyFilter)
                ?request.Select(p => new FilterValueDto
                {
                    Text = p.IsCluodHostedAsset == false ?ConstantValueFilter.No: ConstantValueFilter.Yes,
                    Value = p.IsCluodHostedAsset == false ? ConstantValueFilter.No : ConstantValueFilter.Yes,
                })
                : request.Where(x => propertyFilter.Contains(x.IsCluodHostedAsset.ToString() == "true" ? ConstantValueFilter.Yes:ConstantValueFilter.No)).Select(x => new FilterValueDto(x.IsCluodHostedAsset.ToString() == "true" ? ConstantValueFilter.Yes : ConstantValueFilter.No)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<BuildConstruction> PrepareQuery(TipologicaQueryDtoRule request, ExpressionStarter<BuildConstruction> predicateResult , ExpressionStarter<Buildconstructions> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
             ? _repositoryWrapper.BuildConstruction.FindByCondition(oraclePredicateResult)
             : _repositoryWrapper.BuildConstruction.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => BuildConstructionMapper.GetBuildConstructionMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(BuildConstructionRule dto)
        {
            var entityExists = await _repositoryWrapper.BuildConstruction.FindByCondition(
               x => x.Buildconstruction.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "")
               , true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Buildconstructionid,
                    
                };
            }
            BuildConstruction entity = new BuildConstruction() 
            { 
                BuildConstructionId = dto.Id, 
                BuildConstructionDescription = dto.Description,
                Rule = dto.Rule,
                CloudType = dto.CloudTypeBuild,
                IsCluodHostedAsset = dto.IsCloudHostedAssetBool,
            };
            _repositoryWrapper.BuildConstruction.Create(BuildConstructionMapper.SetBuildConstructionMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(BuildConstructionRule dto)
        {
            var entityExists = await _repositoryWrapper.BuildConstruction.FindByCondition(
               x => x.Buildconstructionid != dto.Id 
               && x.Buildconstruction.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Buildconstructionid
                };
            }
            BuildConstruction entity = new BuildConstruction()
            {
                BuildConstructionId = dto.Id,
                BuildConstructionDescription = dto.Description,
                Rule = dto.Rule,
                CloudType = dto.CloudTypeBuild,
                IsCluodHostedAsset = dto.IsCloudHostedAssetBool,
            };
            _repositoryWrapper.BuildConstruction.Update(BuildConstructionMapper.SetBuildConstructionMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.BuildConstruction.FindByCondition(x => x.Buildconstructionid == id).SingleAsync();
            _repositoryWrapper.BuildConstruction.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Buildconstructionid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.BuildConstruction.FindByCondition(x => x.Buildconstructionid == id).SingleAsync();
            _repositoryWrapper.BuildConstruction.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Buildconstructionid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare

            var entities = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Buildconstructionid == id)
                .Include(x => x.Orgeqpmanufacturer)
                .Include(x => x.Platform)
                .Select(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + x.Platform.Platform + " - " + x.Hardwaretype).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Major Hardware Build", Values = entities });
            var entity = await _repositoryWrapper.BuildConstruction.FindByCondition(x => x.Buildconstructionid == id)
                .SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Build Construction",
                        RecordName = entity.Buildconstruction,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }



        public BuildConstructionRule GetCreatePage()
        {
            var dto = new BuildConstructionRule();
            return dto;
        }

        public BuildConstructionRule GetUpdatePage(short id)
        {
            var entity = BuildConstructionMapper.GetBuildConstructionMapper(_repositoryWrapper.BuildConstruction.FindByCondition(x => x.Buildconstructionid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new BuildConstructionRule() { Id = entity.BuildConstructionId, Description = entity.BuildConstructionDescription, LastModified = entity.ModificationDate,Rule = entity.Rule, LastModifiedBy = entity.ModificationUserEntity.Email, CloudTypeBuild = entity.CloudType, IsCloudHostedAssetBool = (bool)entity.IsCluodHostedAsset };
            return dto;
        }

        public async Task<int> GetRuleFromBuildCostruction(int id)
        {
            return (await _repositoryWrapper.BuildConstruction.FindByCondition(x => x.Buildconstructionid == id).SingleOrDefaultAsync())?.Rule ?? 0;
        }

     
    }
}
