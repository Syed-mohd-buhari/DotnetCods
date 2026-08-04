using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
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
  public  class HardwareTypeManager : GridBaseAsync<HardwareType, TipologicaGridDto, TipologicaQueryDto, Hardwaretypes>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public HardwareTypeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Hardwaretypes> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Hardwaretypes>();
            var predicateInner = PredicateBuilder.New<Hardwaretypes>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwaretypes>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Hardwaretype == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwaretypes>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Hardwaretypes>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Hardwaretypeid == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Hardwaretypes>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Hardwaretypes>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override  List<TipologicaGridDto> CastObjectToDto(IQueryable<HardwareType> request)
        {
            return request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.HardwareTypeId,
                Description = dto.HardwareTypeDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<HardwareType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<HardwareType, object>>[]>
            {
                ["description"] = new Expression<Func<HardwareType, object>>[] { p => p.HardwareTypeDescription },
                ["id"] = new Expression<Func<HardwareType, object>>[] { p => p.HardwareTypeId },
                ["lastModifiedBy"] = new Expression<Func<HardwareType, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<HardwareType> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.HardwareTypeDescription))
                : request.Where(x => x.HardwareTypeDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.HardwareTypeDescription)),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.HardwareTypeId.ToString()))
                : request.Where(x => x.HardwareTypeId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.HardwareTypeId.ToString())),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        public override IQueryable<HardwareType> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<HardwareType> predicateResult, ExpressionStarter<Hardwaretypes> oraclePredicateResult = null)
        {

            var query = oraclePredicateResult.IsStarted
                 ? _repositoryWrapper.HardwareType.FindByCondition(oraclePredicateResult)
                 : _repositoryWrapper.HardwareType.FindAll();

           return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => HardwareTypeMapper.GetHardwareTypeMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.HardwareType.FindByCondition(
               x => x.Hardwaretypeid == dto.Id, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Hardwaretypeid
                };
            }
            HardwareType entity = new HardwareType() { HardwareTypeId = dto.Id, HardwareTypeDescription = dto.Description };
            _repositoryWrapper.HardwareType.Create(HardwareTypeMapper.SetHardwareTypeMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.HardwareType.FindByCondition(
               x => x.Hardwaretypeid != dto.Id && x.Hardwaretype == dto.Description && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Hardwaretypeid
                };
            }
            HardwareType entity = new HardwareType() { HardwareTypeId = dto.Id, HardwareTypeDescription = dto.Description };
            _repositoryWrapper.HardwareType.Update(HardwareTypeMapper.SetHardwareTypeMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.HardwareType.FindByCondition(x => x.Hardwaretypeid == id).SingleAsync();
            _repositoryWrapper.HardwareType.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Hardwaretypeid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.HardwareType.FindByCondition(x => x.Hardwaretypeid == id).SingleAsync();
            _repositoryWrapper.HardwareType.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Hardwaretypeid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - Verificare che non esistano relazioni
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            var entity = await _repositoryWrapper.HardwareType.FindByCondition(x => x.Hardwaretypeid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Hardware Type",
                        RecordName = entity.Hardwaretype,
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
            var entity = HardwareTypeMapper.GetHardwareTypeMapper(_repositoryWrapper.HardwareType.FindByCondition(x => x.Hardwaretypeid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = entity.HardwareTypeId, Description = entity.HardwareTypeDescription, LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email };
            return dto;
        }

      
    }
}
