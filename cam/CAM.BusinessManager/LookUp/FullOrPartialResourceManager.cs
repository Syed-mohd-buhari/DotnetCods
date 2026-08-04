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
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CAM.BusinessManager.LookUp
{
    public class FullOrPartialResourceManager : GridBaseAsync<FullOrPartialResource, TipologicaGridDto, TipologicaQueryDto, Fullorpartialresource>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public FullOrPartialResourceManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper,
            GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, 
            IRepositoryWrapper repositoryWrapper ) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Fullorpartialresource> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {

            var predicateResult = PredicateBuilder.New<Fullorpartialresource>();
            var predicateInner = PredicateBuilder.New<Fullorpartialresource>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Fullorpartialresource>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Fullorpartialresource>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Fullorpartialresource>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Fullorpartialresource>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }


            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Fullorpartialresource>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<FullOrPartialResource> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<FullOrPartialResource, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<FullOrPartialResource, object>>[]>
            {
                ["description"] = new Expression<Func<FullOrPartialResource, object>>[] { p => p.Description },
                ["id"] = new Expression<Func<FullOrPartialResource, object>>[] { p => p.Id },
                ["lastModifiedBy"] = new Expression<Func<FullOrPartialResource, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<FullOrPartialResource> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
              "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description)) : request.Where(x =>
                    x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                : request.Where(x =>
                    x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),
              "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                  : request
                      .Where(x =>
                          x.ModificationUserEntity.Email.Contains(
                              propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

            };
        }

        public override IQueryable<FullOrPartialResource> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<FullOrPartialResource> predicateResult , ExpressionStarter<Fullorpartialresource> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
              ? _repositoryWrapper.FullOrPartialResource.FindByCondition(oraclePredicateResult)
              : _repositoryWrapper.FullOrPartialResource.FindAll();

            return query
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => FullOrPartialResourceMapper.GetFullOrPartialResourceMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto FullOrPartialResourceDto)
        {
            var entityExists = await _repositoryWrapper.FullOrPartialResource.FindByCondition(
               x => x.Id == FullOrPartialResourceDto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            FullOrPartialResource entity = new FullOrPartialResource() { Id = FullOrPartialResourceDto.Id,Description=FullOrPartialResourceDto.Description };            
            _repositoryWrapper.FullOrPartialResource.Create(FullOrPartialResourceMapper.SetfullorpartialresourceMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.FullOrPartialResource.FindByCondition(
               x => x.Id != dto.Id && x.Description == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Id
                };
            }
            FullOrPartialResource entity = new FullOrPartialResource() { Id = dto.Id, Description = dto.Description };
            _repositoryWrapper.FullOrPartialResource.Update(FullOrPartialResourceMapper.SetfullorpartialresourceMapper(entity));           
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.FullOrPartialResource.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.FullOrPartialResource.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.FullOrPartialResource.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.FullOrPartialResource.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var entities = _repositoryWrapper.Lcmengineering
                .FindByCondition(x => x.Fullorpartialsupportid == id || x.Fullorpartialsupporthwid == id)
                .Include(x => x.Opco)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Select(x => x.toDescription(_repositoryWrapper)).ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "LCM Engineering", Values = entities });

            var entity = await _repositoryWrapper.FullOrPartialResource.FindByCondition(x => x.Id == id).SingleAsync();


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Full or Partial Support",
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
            var FullOrPartialResourceDto = new TipologicaGridDto();
            return FullOrPartialResourceDto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.FullOrPartialResource.FindByCondition(x => x.Id == id)
                .Include(x => x.ModificationuserNavigation).Single();

            var entity = FullOrPartialResourceMapper.GetFullOrPartialResourceMapper(model);
            var dto = new TipologicaGridDto() { Id = entity.Id, Description = entity.Description, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

      
    }
}
