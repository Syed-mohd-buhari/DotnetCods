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
using CAM.BusinessManager.ExtensionMethod.MajorHardwareBuild;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class HardwareSolutionResourcesManager : GridBaseAsync<HardwareSolutionResource, TipologicaGridDto, TipologicaQueryDto, Hardwaresolutionresource>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public HardwareSolutionResourcesManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }

    

        public override ExpressionStarter<Hardwaresolutionresource> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Hardwaresolutionresource>();
            var predicateInner = PredicateBuilder.New<Hardwaresolutionresource>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwaresolutionresource>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Hardwaresolutionreource == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwaresolutionresource>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwaresolutionresource>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Hardwaresolutionresourceid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Hardwaresolutionresource>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Hardwaresolutionresource>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<HardwareSolutionResource> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.HardwareSolutionResourceId,
                Description = dto.HardwareSolutionResourceDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<HardwareSolutionResource, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<HardwareSolutionResource, object>>[]>
            {
                ["description"] = new Expression<Func<HardwareSolutionResource, object>>[] { p => p.HardwareSolutionResourceDescription },
                ["id"] = new Expression<Func<HardwareSolutionResource, object>>[] { p => p.HardwareSolutionResourceId },
                ["lastModifiedBy"] = new Expression<Func<HardwareSolutionResource, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<HardwareSolutionResource> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
              "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.HardwareSolutionResourceDescription)) : request.Where(x =>
                    x.HardwareSolutionResourceDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.HardwareSolutionResourceDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.HardwareSolutionResourceId.ToString()))
                : request.Where(x =>
                    x.HardwareSolutionResourceId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.HardwareSolutionResourceId.ToString())),
              "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                  : request
                      .Where(x =>
                          x.ModificationUserEntity.Email.Contains(
                              propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

            };
        }

        public override IQueryable<HardwareSolutionResource> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<HardwareSolutionResource> predicateResult , ExpressionStarter<Hardwaresolutionresource> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                   ? _repositoryWrapper.HardwareSolutionResource.FindByCondition(oraclePredicateResult)
                   : _repositoryWrapper.HardwareSolutionResource.FindAll();
                   
              return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => HardwareSolutionResourceMapper.GetHardwareSolutionResourceMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto HardwareSolutionResourceDto)
        {
            var entityExists = await _repositoryWrapper.HardwareSolutionResource.FindByCondition(
               x => x.Hardwaresolutionresourceid == HardwareSolutionResourceDto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Hardwaresolutionresourceid
                };
            }
            HardwareSolutionResource entity = new HardwareSolutionResource() { HardwareSolutionResourceId = HardwareSolutionResourceDto.Id,HardwareSolutionResourceDescription=HardwareSolutionResourceDto.Description };            
            _repositoryWrapper.HardwareSolutionResource.Create(HardwareSolutionResourceMapper.SetHardwareSolutionResourceMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.HardwareSolutionResource.FindByCondition(
               x => x.Hardwaresolutionresourceid != dto.Id && x.Hardwaresolutionreource == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Hardwaresolutionresourceid
                };
            }
            HardwareSolutionResource entity = new HardwareSolutionResource() { HardwareSolutionResourceId = dto.Id, HardwareSolutionResourceDescription = dto.Description };
            _repositoryWrapper.HardwareSolutionResource.Update(HardwareSolutionResourceMapper.SetHardwareSolutionResourceMapper(entity));           
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.HardwareSolutionResource.FindByCondition(x => x.Hardwaresolutionresourceid == id).SingleAsync();
            _repositoryWrapper.HardwareSolutionResource.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Hardwaresolutionresourceid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.HardwareSolutionResource.FindByCondition(x => x.Hardwaresolutionresourceid == id).SingleAsync();
            _repositoryWrapper.HardwareSolutionResource.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Hardwaresolutionresourceid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var entities = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Hardwaresolutionreourceid == id)
                .Include(x => x.Orgeqpmanufacturer)
                .Include(x => x.Platform)
                .Select(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + x.Platform.Platform + " - " + x.Hardwaretype).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Major Hardware Build", Values = entities });

            var entity = await _repositoryWrapper.HardwareSolutionResource.FindByCondition(x => x.Hardwaresolutionresourceid == id).SingleAsync();


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Hardware Solution",
                        RecordName = entity.Hardwaresolutionreource,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDto GetCreatePage()
        {
            var HardwareSolutionResourceDto = new TipologicaGridDto();
            return HardwareSolutionResourceDto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.HardwareSolutionResource.FindByCondition(x => x.Hardwaresolutionresourceid == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = HardwareSolutionResourceMapper.GetHardwareSolutionResourceMapper(model);
            var dto = new TipologicaGridDto() { Id = entity.HardwareSolutionResourceId, Description = entity.HardwareSolutionResourceDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

      
    }
}
