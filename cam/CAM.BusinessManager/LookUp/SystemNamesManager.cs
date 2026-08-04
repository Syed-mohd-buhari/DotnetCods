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
using CAM.DataTransferObjects.LookUp.SystemNames;
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
   public class SystemNamesManager : GridBaseAsync<SystemNames, SystemNamesDtoGrid, SystemNamesQueryDto, Systemnames>
   {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public SystemNamesManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

    
        public override ExpressionStarter<Systemnames> ApplyFilterForOracleModel(SystemNamesQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Systemnames>();
            var predicateInner = PredicateBuilder.New<Systemnames>();

            if (request.SystemNameId != null && request.SystemNameId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemnames>();
                foreach (var item in request.SystemNameId)
                    predicateInner.Or(x => x.Systemnameid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Systemnames>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.SystemNameDescription != null && request.SystemNameDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Systemnames>();
                foreach (var item in request.SystemNameDescription)
                    predicateInner.Or(x => x.Systemnamedescription == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Systemnames>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Systemnames>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<SystemNamesDtoGrid> CastObjectToDto(IQueryable<SystemNames> request)
        {
            return request.Select(dto => new SystemNamesDtoGrid()
            {
                SystemNameId = dto.SystemNameId,
                SystemNameDescription = dto.SystemNameDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SystemNames, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SystemNames, object>>[]>
            {
                ["systemNameId"] = new Expression<Func<SystemNames, object>>[] { p => p.SystemNameId },
                ["systemNameDescription"] = new Expression<Func<SystemNames, object>>[] { p => p.SystemNameDescription },
                ["lastModifiedBy"] = new Expression<Func<SystemNames, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SystemNames> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "systemNameDescription" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.SystemNameDescription))
                : request.Where(x => x.SystemNameDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.SystemNameDescription)),
                "systemNameId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.SystemNameId.ToString()))
                : request.Where(x => x.SystemNameId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.SystemNameId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<SystemNames> PrepareQuery(SystemNamesQueryDto request, ExpressionStarter<SystemNames> predicateResult , ExpressionStarter<Systemnames> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
             ? _repositoryWrapper.SystemNamesRepository.FindByCondition(oraclePredicateResult)
             : _repositoryWrapper.SystemNamesRepository.FindAll();
            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => SystemNameMapper.GetSystemNameMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(SystemNamesDtoGrid dto)
        {
            
            SystemNames entity = new SystemNames() 
            { 
                SystemNameId = dto.SystemNameId, 
                SystemNameDescription = dto.SystemNameDescription,
            };
            _repositoryWrapper.SystemNamesRepository.Create(SystemNameMapper.SetSystemNamesMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(SystemNamesDtoGrid dto)
        {
            var entityExists = await _repositoryWrapper.SystemNamesRepository.FindByCondition(
               x => x.Systemnameid != dto.SystemNameId 
               && x.Systemnamedescription.ToLower().Replace(" ", "") == dto.SystemNameDescription.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Systemnameid
                };
            }
            SystemNames entity = new SystemNames()
            {
                SystemNameId = dto.SystemNameId,
                SystemNameDescription = dto.SystemNameDescription,
            };
            _repositoryWrapper.SystemNamesRepository.Update(SystemNameMapper.SetSystemNamesMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SystemNamesRepository.FindByCondition(x => x.Systemnameid == id).SingleAsync();
            _repositoryWrapper.SystemNamesRepository.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Systemnameid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SystemNamesRepository.FindByCondition(x => x.Systemnameid == id).SingleAsync();
            _repositoryWrapper.SystemNamesRepository.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Systemnameid
            };
        }


        public SystemNamesCreateDto GetCreatePage()
        {
            var dto = new SystemNamesCreateDto();
            return dto;
        }

        public SystemNamesUpdateDto GetUpdatePage(short id)
        {
            var entity = SystemNameMapper.GetSystemNameMapper(_repositoryWrapper.SystemNamesRepository.FindByCondition(x => x.Systemnameid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new SystemNamesUpdateDto() { SystemNameId = entity.SystemNameId, SystemNameDescription = entity.SystemNameDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email};
            return dto;
        }

        //public async Task<int> GetRuleFromBuildCostruction(int id)
        //{
        //    return (await _repositoryWrapper.SystemNames.FindByCondition(x => x.Buildconstructionid == id).SingleOrDefaultAsync())?.Rule ?? 0;
        //}

     
    }
}
