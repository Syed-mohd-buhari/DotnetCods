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
    public class SharingTypesManager : GridBaseAsync<SharingType, TipologicaGridDto, TipologicaQueryDto, Sharingtype>
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;


        public SharingTypesManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager,contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
        }
        public override ExpressionStarter<Sharingtype> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Sharingtype>();
            var predicateInner = PredicateBuilder.New<Sharingtype>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Sharingtype>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Sharingtype>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Sharingtypeid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Sharingtype>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }


            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Sharingtype>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
       
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<SharingType> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.SharingTypeId,
                Description = dto.SharingTypeDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<SharingType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SharingType, object>>[]>
            {
                ["description"] = new Expression<Func<SharingType, object>>[] { p => p.SharingTypeDescription },
                ["id"] = new Expression<Func<SharingType, object>>[] { p => p.SharingTypeId },
                ["lastModifiedBy"] = new Expression<Func<SharingType, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<SharingType> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
              "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.SharingTypeDescription)) : request.Where(x =>
                    x.SharingTypeDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.SharingTypeDescription)),
              "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                  ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                  : request
                      .Where(x =>
                          x.ModificationUserEntity.Email.Contains(
                              propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.SharingTypeId.ToString()))
                : request.Where(x =>
                    x.SharingTypeId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.SharingTypeId.ToString())),

            };
        }

        public override IQueryable<SharingType> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<SharingType> predicateResult, ExpressionStarter<Sharingtype> oraclePredicateResult)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.SharingType.FindByCondition(oraclePredicateResult)

                : _repositoryWrapper.SharingType.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
              .Include(m => m.CreationuserNavigation)
              .AsEnumerable().Select(p => ShareTypeMapper.Get(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto opCoDto)
        {
            var entityExists = await _repositoryWrapper.SharingType.FindByCondition(
               x => x.Sharingtypeid == opCoDto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Sharingtypeid
                };
            }
            SharingType entity = new SharingType() { SharingTypeId = opCoDto.Id,SharingTypeDescription=opCoDto.Description };            
            _repositoryWrapper.SharingType.Create(ShareTypeMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.SharingType.FindByCondition(
               x => x.Sharingtypeid != dto.Id && x.Description == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Sharingtypeid
                };
            }
            SharingType entity = new SharingType() { SharingTypeId = dto.Id, SharingTypeDescription = dto.Description };
            _repositoryWrapper.SharingType.Update(ShareTypeMapper.Set(entity));           
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.SharingType.FindByCondition(x => x.Sharingtypeid == id).SingleAsync();
            _repositoryWrapper.SharingType.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Sharingtypeid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.SharingType.FindByCondition(x => x.Sharingtypeid == id).SingleAsync();
            _repositoryWrapper.SharingType.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Sharingtypeid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var data = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Sharingtypeid == id).Select(x => x.Description).ToArray();
          


            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (data.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Design component family", Values = data });
          

            var entity = await _repositoryWrapper.SharingType.FindByCondition(x => x.Sharingtypeid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "SharingType",
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
            var opCoDto = new TipologicaGridDto();
            return opCoDto;
        }

        public TipologicaGridDto GetUpdatePage(short id)
        {
            var entity = ShareTypeMapper.Get( _repositoryWrapper.SharingType.FindByCondition(x => x.Sharingtypeid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.SharingTypeId, Description = entity.SharingTypeDescription, LastModifiedBy = entity.ModificationUserEntity.Email, LastModified = entity.ModificationDate };
            return dto;
        }

    }
}
