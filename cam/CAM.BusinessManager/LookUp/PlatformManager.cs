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
using CAM.BusinessManager.ExtensionMethod.MajorHardwareBuild;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
   public class PlatformManager : GridBaseAsync<Entities.Models.Lookup.Platform, TipologicaGridDto, TipologicaQueryDto, Platforms>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public PlatformManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

     
        public override ExpressionStarter<Platforms> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
               var predicateResult = PredicateBuilder.New<Platforms>();
            var predicateInner = PredicateBuilder.New<Platforms>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Platforms>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Platform == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Platforms>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Platforms>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Platformid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Platforms>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Platforms>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<Entities.Models.Lookup.Platform> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.PlatformId,
                Description = dto.PlatformDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<Entities.Models.Lookup.Platform, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Entities.Models.Lookup.Platform, object>>[]>
            {
                ["description"] = new Expression<Func<Entities.Models.Lookup.Platform, object>>[] { p => p.PlatformDescription },
                ["id"] = new Expression<Func<Entities.Models.Lookup.Platform, object>>[] { p => p.PlatformId },
                ["lastModifiedBy"] = new Expression<Func<Entities.Models.Lookup.Platform, object>>[] { p => p.ModificationUserEntity.Email }
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<Entities.Models.Lookup.Platform> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.PlatformDescription))
                : request.Where(x => x.PlatformDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.PlatformDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.PlatformId.ToString()))
                : request.Where(x => x.PlatformId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.PlatformId.ToString())),
            };
        }

       
        public override IQueryable<Entities.Models.Lookup.Platform> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<Entities.Models.Lookup.Platform> predicateResult, ExpressionStarter<Platforms> oracleObject = null)
        {
            var query = oracleObject.IsStarted
             ? _repositoryWrapper.Platform.FindByCondition(oracleObject)
             : _repositoryWrapper.Platform.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => PlatFormMapper.GetPlatFormMapper(p)).AsQueryable();
        }
        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.Platform.FindByCondition(
               x => x.Platform.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Platformid
                };
            }
            Entities.Models.Lookup.Platform entity = new Entities.Models.Lookup.Platform() { PlatformId = dto.Id, PlatformDescription = dto.Description };
            _repositoryWrapper.Platform.Create(PlatFormMapper.SetPlatFormMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.Platform.FindByCondition(
               x => x.Platformid != dto.Id 
               && x.Platform.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ", "")
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Platformid
                };
            }
            Entities.Models.Lookup.Platform entity = new Entities.Models.Lookup.Platform() { PlatformId = dto.Id, PlatformDescription = dto.Description };
            _repositoryWrapper.Platform.Update(PlatFormMapper.SetPlatFormMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.Platform.FindByCondition(x => x.Platformid == id).SingleAsync();
            _repositoryWrapper.Platform.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Platformid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.Platform.FindByCondition(x => x.Platformid == id).SingleAsync();
            _repositoryWrapper.Platform.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Platformid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var MajorHardwareBuild = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Platformid == id)
                 .Include(x => x.Orgeqpmanufacturer)
                 .Include(x => x.Platform)
                 .Select(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + x.Platform.Platform + " - " + x.Hardwaretype).ToArray();


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (MajorHardwareBuild.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Major Hardware Build", Values = MajorHardwareBuild });

            var entity = await _repositoryWrapper.Platform.FindByCondition(x => x.Platformid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Platform",
                        RecordName = entity.Platform,
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
            var entity = PlatFormMapper.GetPlatFormMapper(_repositoryWrapper.Platform.FindByCondition(x => x.Platformid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.PlatformId, LastModifiedBy = entity.ModificationUserEntity.Email, Description = entity.PlatformDescription, LastModified = entity.ModificationDate };
            return dto;
        }

      
    }
}

    
