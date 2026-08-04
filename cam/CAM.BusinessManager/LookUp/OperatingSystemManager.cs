using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OperatingSystem = CAM.Entities.Models.Lookup.OperatingSystem;

namespace CAM.BusinessManager.LookUp
{
    public class OperatingSystemManager : GridBaseAsync<OperatingSystem, OperatingSystemDto, TipologicaQueryDto, Operatingsystems>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public OperatingSystemManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
    
        public override ExpressionStarter<Operatingsystems> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Operatingsystems>();
            var predicateInner = PredicateBuilder.New<Operatingsystems>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Operatingsystems>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Operatingsystemname == item);
                predicateResult.And(predicateInner);
            }

            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Operatingsystems>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Operatingsystemid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Operatingsystems>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Operatingsystems>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Operatingsystems>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }
        public override List<OperatingSystemDto> CastObjectToDto(IQueryable<OperatingSystem> request)
        {
            return request.Select(dto => new OperatingSystemDto()
            {
                Id = dto.OperatingSystemId,
                Description = dto.OperatingSystemName,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                OperatingSystemVersion = dto.OperatingSystemVersion,
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<OperatingSystem, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<OperatingSystem, object>>[]>
            {
                ["description"] = new Expression<Func<OperatingSystem, object>>[] { p => p.OperatingSystemName },
                ["lastModifiedBy"] = new Expression<Func<OperatingSystem, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<OperatingSystem, object>>[] { p => p.OperatingSystemId },
                ["operatingSystemVersion"] = new Expression<Func<OperatingSystem, object>>[] { p => p.OperatingSystemVersion }

            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<OperatingSystem> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.OperatingSystemName)) 
                : request.Where(x => x.OperatingSystemName.Contains(propertyFilter)).Select(x => new FilterValueDto(x.OperatingSystemName)),
                "operatingSystemVersion" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.OperatingSystemVersion))
                : request.Where(x => x.OperatingSystemVersion.Contains(propertyFilter)).Select(x => new FilterValueDto(x.OperatingSystemVersion)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.OperatingSystemId.ToString()))
                : request.Where(x => x.OperatingSystemId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.OperatingSystemId.ToString())),
            };
        }

        public override IQueryable<OperatingSystem> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<OperatingSystem> predicateResult , ExpressionStarter<Operatingsystems> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
              ? _repositoryWrapper.OperatingSystem.FindByCondition(oraclePredicateResult)
              : _repositoryWrapper.OperatingSystem.FindAll();

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => OperatingSystemMapper.GetOperatingSystemMapper(p)).AsQueryable();

        }

        public async Task<ResultDto> Add(OperatingSystemDto dto)
        {
            var entityExists = await _repositoryWrapper.OperatingSystem.FindByCondition(
               x => x.Operatingsystemname.ToLower().Replace(" ", "") == dto.Description.ToLower().Replace(" ", "") 
               && x.Operatingsystemversion.ToLower().Replace(" ", "") == dto.OperatingSystemVersion.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Operatingsystemid
                };
            }
            OperatingSystem entity = new OperatingSystem() { OperatingSystemId = dto.Id, OperatingSystemName = dto.Description , OperatingSystemVersion = dto.OperatingSystemVersion};
            _repositoryWrapper.OperatingSystem.Create(OperatingSystemMapper.SetOperatingSystemMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(OperatingSystemDto dto)
        {
            var entityExists = await _repositoryWrapper.OperatingSystem.FindByCondition(
               x => x.Operatingsystemid == dto.Id && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                OperatingSystem entity = new OperatingSystem() { OperatingSystemId = dto.Id, OperatingSystemName = dto.Description, OperatingSystemVersion = dto.OperatingSystemVersion };
                _repositoryWrapper.OperatingSystem.Update(OperatingSystemMapper.SetOperatingSystemMapper(entity));
                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };

            }
            else
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Operatingsystemid
                };
            }

        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.OperatingSystem.FindByCondition(x => x.Operatingsystemid == id).SingleAsync();
            _repositoryWrapper.OperatingSystem.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Operatingsystemid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.OperatingSystem.FindByCondition(x => x.Operatingsystemid == id).SingleAsync();
            _repositoryWrapper.OperatingSystem.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Operatingsystemid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            //TODO - da verificare descrizione

            var MajorSoftwareBuild = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Operatingsystemid == id)
                .Include(x => x.Orgeqpmanufacturer)
                .Select(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + x.Productname != null ? x.Productname.Description:"" + " - " + x.Softwareversion).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (MajorSoftwareBuild.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Major Software Build", Values = MajorSoftwareBuild });

            var entity = await _repositoryWrapper.OperatingSystem.FindByCondition(x => x.Operatingsystemid == id).SingleAsync();

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Operating System",
                        RecordName = entity.Operatingsystemname,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }
        public OperatingSystemDto GetCreatePage()
        {
            var dto = new OperatingSystemDto();
            return dto;
        }

        public OperatingSystemDto GetUpdatePage(short id)
        {
            var entity = OperatingSystemMapper.GetOperatingSystemMapper( _repositoryWrapper.OperatingSystem.FindByCondition(x => x.Operatingsystemid == id).Include(x => x.ModificationuserNavigation).Single());


            var dto = new OperatingSystemDto() { Id = entity.OperatingSystemId, Description = entity.OperatingSystemName,LastModified = entity.ModificationDate, LastModifiedBy = entity.ModificationUserEntity.Email,OperatingSystemVersion = entity.OperatingSystemVersion };
            return dto;
        }

      
    }
}
