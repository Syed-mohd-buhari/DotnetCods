using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.Entities.Models.Lookup;
using CAM.DataTransferObjects.QueryDto;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Environment = CAM.Entities.Models.Lookup.Environment;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CAM.BusinessManager.LookUp
{
    public class EnvironmentManager : GridBaseAsync<Environment, TipologicaGridDto, TipologicaQueryDto, Environments>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;

        public EnvironmentManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, 
            GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, 
            IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public override ExpressionStarter<Environments> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Environments>();
            var predicateInner = PredicateBuilder.New<Environments>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Environments>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Environment == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Environments>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Environmentid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Environments>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Environments>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Environments>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<Environment> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.EnvironmentId,
                Description = dto.EnvironmentDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<Environment, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Environment, object>>[]>
            {
                ["description"] = new Expression<Func<Environment, object>>[] { p => p.EnvironmentDescription },
                ["id"] = new Expression<Func<Environment, object>>[] { p => p.EnvironmentId },
                ["lastModifiedBy"] = new Expression<Func<Environment, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<Environment> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.EnvironmentDescription)) : request.Where(x =>
                    x.EnvironmentDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.EnvironmentDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(x => new FilterValueDto(x.EnvironmentId.ToString()))
                    : request.Where(x =>
                        x.EnvironmentId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.EnvironmentId.ToString())),

            };
        }

        public override IQueryable<Environment> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<Environment> predicateResult , ExpressionStarter<Environments> oraclePredicateResult = null)
        {

            var query = oraclePredicateResult.IsStarted
               ? _repositoryWrapper.Environment.FindByCondition(oraclePredicateResult)
               : _repositoryWrapper.Environment.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation).AsEnumerable()
                .Select(p => EnvironmentMapper.GetEnvironmentMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.Environment.FindByCondition(
               x => x.Environment.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Environmentid
                };
            }
            Environment entity = new Environment() { EnvironmentId = dto.Id, EnvironmentDescription = dto.Description };
            _repositoryWrapper.Environment.Create(EnvironmentMapper.SetEnvironmentMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.Environment.FindByCondition(
               x => x.Environmentid != dto.Id && x.Environment.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Environmentid
                };
            }
            Environment entity = new Environment() { EnvironmentId = dto.Id, EnvironmentDescription = dto.Description };
            _repositoryWrapper.Environment.Update(EnvironmentMapper.SetEnvironmentMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.Environment.FindByCondition(x => x.Environmentid == id).SingleAsync();
            _repositoryWrapper.Environment.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Environmentid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.Environment.FindByCondition(x => x.Environmentid == id).SingleAsync();
            _repositoryWrapper.Environment.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Environmentid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO: verificare descrizione da visualizzare
            var entities = _repositoryWrapper.NetworkElementAsPlanned
                .FindByCondition(x => x.Deploymenttypeid == id)
                .Select(x => NetworkElementAsPlannedMapper.Get(x,true).toDescription())
                .ToArray();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned", Values = entities });

            var entity = await _repositoryWrapper.Environment.FindByCondition(x => x.Environmentid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Environment",
                        RecordName = entity.Environment,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public TipologicaGridDto GetCreatePage()
        {
            var EnvironmentDto = new TipologicaGridDto();
            return EnvironmentDto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var model = _repositoryWrapper.Environment.FindByCondition(x => x.Environmentid == id).Include(x => x.ModificationuserNavigation).Single();
            var entity = EnvironmentMapper.GetEnvironmentMapper(model);
            var dto = new TipologicaGridDto() { Id = entity.EnvironmentId, Description = entity.EnvironmentDescription, LastModifiedBy = entity.ModificationUserEntity.Email, LastModified = entity.ModificationDate };
            return dto;
        }

      
    }
}
