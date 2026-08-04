using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models.Lookup;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CAM.BusinessManager.LookUp
{
    public class SystemFunctionManager : GridBaseAsync<SystemFunction, TipologicaGridDto, TipologicaQueryDto , Systemfunctions>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public SystemFunctionManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, 
            IRepositoryWrapper repositoryWrapper) : base(columnManager,contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Systemfunctions> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Systemfunctions>();
            var predicateInner = PredicateBuilder.New<Systemfunctions>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Systemfunctions>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Systemfunction == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Systemfunctions>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Systemfunctions>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Systemfunctionid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Systemfunctions>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
       

        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<SystemFunction> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.SystemFunctionId,
                Description = dto.SystemFunctionDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SystemFunction, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SystemFunction, object>>[]>
            {
                ["description"] = new Expression<Func<SystemFunction, object>>[] { p => p.SystemFunctionDescription },
                ["id"] = new Expression<Func<SystemFunction, object>>[] { p => p.SystemFunctionId },
                ["lastModifiedBy"] = new Expression<Func<SystemFunction, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SystemFunction> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.SystemFunctionDescription))
                : request.Where(x => x.SystemFunctionDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.SystemFunctionDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.SystemFunctionId.ToString()))
                : request.Where(x => x.SystemFunctionId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.SystemFunctionId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<SystemFunction> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<SystemFunction> predicateResult, ExpressionStarter<Systemfunctions> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
              ? _repositoryWrapper.SystemFunction.FindByCondition(oraclePredicateResult)
              : _repositoryWrapper.SystemFunction.FindAll();
        
            
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => SystemFunctionMapper.Get(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SystemFunction.FindByCondition(
               x => x.Systemfunction.ToLower().Replace(" ","")  == dto.Description.ToLower().Replace(" ",""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Systemfunctionid
                };
            }
            SystemFunction entity = new SystemFunction() { SystemFunctionId = dto.Id, SystemFunctionDescription = dto.Description };
            _repositoryWrapper.SystemFunction.Create(SystemFunctionMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SystemFunction.FindByCondition(
               x => x.Systemfunctionid != dto.Id && x.Systemfunction == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Systemfunctionid
                };
            }
            SystemFunction entity = new SystemFunction() { SystemFunctionId = dto.Id, SystemFunctionDescription = dto.Description };
            _repositoryWrapper.SystemFunction.Update(SystemFunctionMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SystemFunction.FindByCondition(x => x.Systemfunctionid == id).SingleAsync();
            _repositoryWrapper.SystemFunction.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Systemfunctionid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SystemFunction.FindByCondition(x => x.Systemfunctionid == id).SingleAsync();
            _repositoryWrapper.SystemFunction.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Systemfunctionid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - Verificare che non esistano relazioni
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.SystemFunction.FindByCondition(x => x.Systemfunctionid == id).SingleAsync();


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "System Function",
                        RecordName = entity.Systemfunction,
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
            var entity = SystemFunctionMapper.Get( _repositoryWrapper.SystemFunction.FindByCondition(x => x.Systemfunctionid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.SystemFunctionId, Description = entity.SystemFunctionDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }
    }
}
