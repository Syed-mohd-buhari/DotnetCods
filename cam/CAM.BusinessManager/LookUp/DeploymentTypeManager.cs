using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.LookUp
{
   public class DeploymentTypeManager : GridBaseAsync<DeploymentType, TipologicaGridDtoRule, TipologicaQueryDtoRule, Deploymenttypes>
    {

       private readonly IRepositoryWrapper _repositoryWrapper;
       private readonly IMapper _mapper;
       private readonly GridCustomColumnManager _columnManager;

       public DeploymentTypeManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager, 
           IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
       {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
           _columnManager = columnManager;
       }

    

        public override ExpressionStarter<Deploymenttypes> ApplyFilterForOracleModel(TipologicaQueryDtoRule request)
        {
            var predicateResult = PredicateBuilder.New<Deploymenttypes>();
            var predicateInner = PredicateBuilder.New<Deploymenttypes>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymenttypes>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Deploymenttype == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymenttypes>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Deploymenttypeid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymenttypes>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Rule != null && request.Rule.Any())
            {
                predicateInner = PredicateBuilder.New<Deploymenttypes>();
                foreach (var item in request.Rule)
                    predicateInner.Or(x => x.Rule == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Deploymenttypes>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Deploymenttypes>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public override List<TipologicaGridDtoRule> CastObjectToDto(IQueryable<DeploymentType> request)
        {
            return  request.Select(dto => new TipologicaGridDtoRule()
            {
                Id = dto.DeploymentTypeId,
                Description = dto.DeploymentTypeDescription,
                LastModified = dto.ModificationDate,
                Rule = dto.Rule,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<DeploymentType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<DeploymentType, object>>[]>
            {
                ["description"] = new Expression<Func<DeploymentType, object>>[] { p => p.DeploymentTypeDescription },
                ["id"] = new Expression<Func<DeploymentType, object>>[] { p => p.DeploymentTypeId },
                ["rule"] = new Expression<Func<DeploymentType, object>>[] { p => p.Rule },
                ["lastModifiedBy"] = new Expression<Func<DeploymentType, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<DeploymentType> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.DeploymentTypeDescription)) : request.Where(x =>
                    x.DeploymentTypeDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.DeploymentTypeDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "rule" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.Rule.ToString()))
                    : request.Where(x => propertyFilter.Contains(x.Rule.ToString())).Select(x => new FilterValueDto(x.Rule.ToString())),

                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.DeploymentTypeId.ToString()))
                    : request.Where(x =>
                        x.DeploymentTypeId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.DeploymentTypeId.ToString())),

            };
        }

        public override IQueryable<DeploymentType> PrepareQuery(TipologicaQueryDtoRule request, ExpressionStarter<DeploymentType> predicateResult , ExpressionStarter<Deploymenttypes> oraclepPredicateResult = null)
        {
            var query = oraclepPredicateResult.IsStarted
                ? _repositoryWrapper.DeploymentType.FindByCondition(oraclepPredicateResult)
                : _repositoryWrapper.DeploymentType.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => DeploymentTypeMapper.GetDeploymentTypeMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDtoRule dto)
        {
            var entityExists = await _repositoryWrapper.DeploymentType.FindByCondition(
               x => x.Deploymenttype.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Deploymenttypeid
                };
            }
            DeploymentType entity = new DeploymentType() { DeploymentTypeId = dto.Id, DeploymentTypeDescription = dto.Description, Rule = dto.Rule};
            _repositoryWrapper.DeploymentType.Create(DeploymentTypeMapper.SetDeploymentTypeMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDtoRule dto)
        {
            var entityExists = await _repositoryWrapper.DeploymentType.FindByCondition(
               x => x.Deploymenttypeid != dto.Id 
               && x.Deploymenttype.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Deploymenttypeid
                };
            }
            DeploymentType entity = new DeploymentType() { DeploymentTypeId = dto.Id, DeploymentTypeDescription = dto.Description, Rule = dto.Rule };
            _repositoryWrapper.DeploymentType.Update(DeploymentTypeMapper.SetDeploymentTypeMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.DeploymentType.FindByCondition(x => x.Deploymenttypeid == id).SingleAsync();
            _repositoryWrapper.DeploymentType.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Deploymenttypeid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.DeploymentType.FindByCondition(x => x.Deploymenttypeid == id).SingleAsync();
            _repositoryWrapper.DeploymentType.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Deploymenttypeid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var entities = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Deploymenttypeid == id)
                .Select(x => NetworkElementAsPlannedMapper.Get(x,true).toDescription()).ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned", Values = entities });


            var entity = await _repositoryWrapper.DeploymentType.FindByCondition(x => x.Deploymenttypeid == id)
             .SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Deployment Type",
                        RecordName = entity.Deploymenttype,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDtoRule GetCreatePage()
        {
            var DeploymentTypeDto = new TipologicaGridDtoRule();
            return DeploymentTypeDto;
        }

        public TipologicaGridDtoRule GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.DeploymentType.FindByCondition(x => x.Deploymenttypeid == id).Include(x => x.ModificationuserNavigation).Single();

            var entity = DeploymentTypeMapper.GetDeploymentTypeMapper(model);
            var dto = new TipologicaGridDtoRule() { Id = entity.DeploymentTypeId, Description = entity.DeploymentTypeDescription, LastModifiedBy = entity.ModificationUserEntity.Email, LastModified = entity.ModificationDate, Rule = entity.Rule};
            return dto;
        }

       
    }
}
