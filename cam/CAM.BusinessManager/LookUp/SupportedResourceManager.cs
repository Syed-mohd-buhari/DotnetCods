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
using CAM.Entities.Models.Cross;
using CAM.Entities.Models.Lookup;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class SupportedResourceManager : GridBaseAsync<SupportedResource, TipologicaGridDtoRule, TipologicaQueryDtoRule, Supportedresource>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public SupportedResourceManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager,contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }

      
        public override ExpressionStarter<Supportedresource> ApplyFilterForOracleModel(TipologicaQueryDtoRule request)
        {
            var predicateResult = PredicateBuilder.New<Supportedresource>();
            var predicateInner = PredicateBuilder.New<Supportedresource>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Supportedresource>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Supportedresource>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Id == item);
                predicateResult.And(predicateInner);
            }

            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Supportedresource>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Supportedresource>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Supportedresource>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Supportedresource>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDtoRule> CastObjectToDto(IQueryable<SupportedResource> request)
        {
            return  request.Select(dto => new TipologicaGridDtoRule()
            {
                Id = dto.Id,
                Description = dto.Description,
                LastModified = dto.ModificationDate,
                Rule = dto.Rule,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SupportedResource, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SupportedResource, object>>[]>
            {
                ["description"] = new Expression<Func<SupportedResource, object>>[] { p => p.Description },
                ["id"] = new Expression<Func<SupportedResource, object>>[] { p => p.Id },
                ["rule"] = new Expression<Func<SupportedResource, object>>[] { p => p.Rule },
                ["lastModifiedBy"] = new Expression<Func<SupportedResource, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SupportedResource> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
              "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Description)) : request.Where(x =>
                    x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.Id.ToString()))
                : request.Where(x =>
                    x.Id.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Id.ToString())),
              "rule" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.Rule.ToString()))
                : request.Where(x =>
                    x.Rule.ToString() == propertyFilter).Select(x => new FilterValueDto(x.Rule.ToString())),
              "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                  : request
                      .Where(x =>
                          x.ModificationUserEntity.Email.Contains(
                              propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),

            };
        }

        public override IQueryable<SupportedResource> PrepareQuery(TipologicaQueryDtoRule request, ExpressionStarter<SupportedResource> predicateResult , ExpressionStarter<Supportedresource> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                 ? _repositoryWrapper.SupportedResource.FindByCondition(oraclePredicateResult)
                 : _repositoryWrapper.SupportedResource.FindAll();

            var result = query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => SupportedResourceMapper.GetSupportedResourceMapper(p)).AsQueryable();

            return result;
        }

        public async Task<ResultDto> Add(TipologicaGridDtoRule dto)
        {
            var entityExists = await _repositoryWrapper.SupportedResource.FindByCondition(
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

            SupportedResource entity = new SupportedResource() { Id = dto.Id,Description = dto.Description , Rule = dto.Rule };            
            _repositoryWrapper.SupportedResource.Create(SupportedResourceMapper.SetSupportedResourceMapper(entity));
            await _repositoryWrapper.SaveAsync();

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

       

        public async Task<ResultDto> Update(TipologicaGridDtoRule dto)
        {
            var entityExists = await _repositoryWrapper.SupportedResource.FindByCondition(
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
            SupportedResource entity = new SupportedResource() { Id = dto.Id, Description = dto.Description,Rule =  dto.Rule};
            _repositoryWrapper.SupportedResource.Update(SupportedResourceMapper.SetSupportedResourceMapper(entity));           
            await _repositoryWrapper.SaveAsync();
            


            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.SupportedResource.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == id).SingleAsync();
            _repositoryWrapper.SupportedResource.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Id
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var Lcmengineering = _repositoryWrapper.Lcmengineering
               .FindByCondition(x => x.Softwaresupportedid == id)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
               .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x=>x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
               .Include(x => x.Opco)
               .Select(x => x.toDescription(_repositoryWrapper)).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (Lcmengineering.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "LCM Engineering", Values = Lcmengineering });


            var entity = await _repositoryWrapper.SupportedResource.FindByCondition(x => x.Id == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Supported Resource",
                        RecordName = entity.Description,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDtoRule GetCreatePage()
        {
            var opCoDto = new TipologicaGridDtoRule();
            return opCoDto;
        }

        public TipologicaGridDtoRule GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.SupportedResource
                .FindByCondition(x => x.Id == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = SupportedResourceMapper.GetSupportedResourceMapper(model);
            var dto = new TipologicaGridDtoRule() 
                { Id = entity.Id, Description = entity.Description, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email, Rule = entity.Rule};
            return dto;
        }

    }
}


