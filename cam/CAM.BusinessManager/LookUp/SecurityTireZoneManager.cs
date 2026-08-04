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
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class SecurityTireZoneManager : GridBaseAsync<SecurityTireZone, TipologicaGridDto, TipologicaQueryDto , Securitytirezone>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public SecurityTireZoneManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }
        public override ExpressionStarter<Securitytirezone> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Securitytirezone>();
            var predicateInner = PredicateBuilder.New<Securitytirezone>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Securitytirezone>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Securitytirezone>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Securitytirezoneid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Securitytirezone>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Securitytirezone>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
       
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<SecurityTireZone> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.SecurityTireZoneId,
                Description = dto.SecurityTireZoneDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SecurityTireZone, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SecurityTireZone, object>>[]>
            {
                ["description"] = new Expression<Func<SecurityTireZone, object>>[] { p => p.SecurityTireZoneDescription },
                ["id"] = new Expression<Func<SecurityTireZone, object>>[] { p => p.SecurityTireZoneId },
                ["lastModifiedBy"] = new Expression<Func<SecurityTireZone, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SecurityTireZone> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.SecurityTireZoneDescription)) : request.Where(x =>
                      x.SecurityTireZoneDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.SecurityTireZoneDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.SecurityTireZoneId.ToString()))
                : request.Where(x =>
                    x.SecurityTireZoneId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.SecurityTireZoneId.ToString())),

            };
        }

       
        public override IQueryable<SecurityTireZone> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<SecurityTireZone> predicateResult, ExpressionStarter<Securitytirezone> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.SecurityTireZone.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.SecurityTireZone.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => SecurityTireZoneMapper.Get(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto SecurityTireZoneDto)
        {
            var entityExists = await _repositoryWrapper.SecurityTireZone.FindByCondition(
               x => x.Description.ToLower().Replace(" ","") == SecurityTireZoneDto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Securitytirezoneid
                };
            }
            SecurityTireZone entity = new SecurityTireZone() { SecurityTireZoneId = SecurityTireZoneDto.Id, SecurityTireZoneDescription = SecurityTireZoneDto.Description };
            _repositoryWrapper.SecurityTireZone.Create(SecurityTireZoneMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SecurityTireZone.FindByCondition(
               x => x.Securitytirezoneid != dto.Id 
               && x.Description.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Securitytirezoneid
                };
            }
            SecurityTireZone entity = new SecurityTireZone() { SecurityTireZoneId = dto.Id, SecurityTireZoneDescription = dto.Description };
            _repositoryWrapper.SecurityTireZone.Update(SecurityTireZoneMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SecurityTireZone.FindByCondition(x => x.Securitytirezoneid == id).SingleAsync();
            _repositoryWrapper.SecurityTireZone.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Securitytirezoneid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SecurityTireZone.FindByCondition(x => x.Securitytirezoneid == id).SingleAsync();
            _repositoryWrapper.SecurityTireZone.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Securitytirezoneid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            string[] designComponentFamily = null;//_repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Securitytirezoneid == id).Select(x => x.Description).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (designComponentFamily.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "NFVI Transition", Values = designComponentFamily });

            var entity = await _repositoryWrapper.SecurityTireZone.FindByCondition(x => x.Securitytirezoneid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "SecurityTireZone",
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
            var SecurityTireZoneDto = new TipologicaGridDto();
            return SecurityTireZoneDto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var entity =SecurityTireZoneMapper.Get(_repositoryWrapper.SecurityTireZone.FindByCondition(x => x.Securitytirezoneid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.SecurityTireZoneId, Description = entity.SecurityTireZoneDescription, LastModifiedBy = entity.ModificationUserEntity.Email, LastModified = entity.ModificationDate };
            return dto;
        }

    }
}
