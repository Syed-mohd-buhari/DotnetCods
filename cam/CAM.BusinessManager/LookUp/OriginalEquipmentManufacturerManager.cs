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
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs;
using CAM.BusinessManager.ExtensionMethod.MajorSoftwareBuild;
using CAM.BusinessManager.ExtensionMethod.MajorHardwareBuild;
using OracleModels.DBModels;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Mappers.Entity;
using CAM.Repository.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CAM.BusinessManager.LookUp
{
    public class OriginalEquipmentManufacturerManager : GridBaseAsync<OriginalEquipmentManufacturer, TipologicaGridDto, TipologicaQueryDto, Originalequipmentmanufacturers>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public OriginalEquipmentManufacturerManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }
       
        public override ExpressionStarter<Originalequipmentmanufacturers> ApplyFilterForOracleModel(TipologicaQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Originalequipmentmanufacturers>();
            var predicateInner = PredicateBuilder.New<Originalequipmentmanufacturers>();

            if (request.Description != null && request.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Originalequipmentmanufacturers>();
                foreach (var item in request.Description)
                    predicateInner.Or(x => x.Originalequipmentmanufacturer == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Originalequipmentmanufacturers>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Id != null && request.Id.Any())
            {
                predicateInner = PredicateBuilder.New<Originalequipmentmanufacturers>();
                foreach (var item in request.Id)
                    predicateInner.Or(x => x.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }



            if (request.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Originalequipmentmanufacturers>();
                if (request.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModified.StartDate);
                if (request.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Originalequipmentmanufacturers>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }
        public override List<TipologicaGridDto> CastObjectToDto(IQueryable<OriginalEquipmentManufacturer> request)
        {
            return  request.Select(dto => new TipologicaGridDto()
            {
                Id = dto.OriginalEquipmentManufacturerId,
                Description = dto.OriginalEquipmentManufacturerDescription,
                LastModified = dto.ModificationDate,
                LastModifiedBy = dto.ModificationUserEntity.Email
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<OriginalEquipmentManufacturer, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<OriginalEquipmentManufacturer, object>>[]>
            {
                ["description"] = new Expression<Func<OriginalEquipmentManufacturer, object>>[] { p => p.OriginalEquipmentManufacturerDescription },
                ["lastModifiedBy"] = new Expression<Func<OriginalEquipmentManufacturer, object>>[] { p => p.ModificationUserEntity.Email },
                ["id"] = new Expression<Func<OriginalEquipmentManufacturer, object>>[] { p => p.OriginalEquipmentManufacturerId }
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<OriginalEquipmentManufacturer> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "description" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.OriginalEquipmentManufacturerDescription))
                : request.Where(x => x.OriginalEquipmentManufacturerDescription.Contains(propertyFilter)).Select(x => new FilterValueDto(x.OriginalEquipmentManufacturerDescription)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "id" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.OriginalEquipmentManufacturerId.ToString()))
                : request.Where(x => x.OriginalEquipmentManufacturerId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.OriginalEquipmentManufacturerId.ToString())),
            };
        }

        public override IQueryable<OriginalEquipmentManufacturer> PrepareQuery(TipologicaQueryDto request, ExpressionStarter<OriginalEquipmentManufacturer> predicateResult , ExpressionStarter<Originalequipmentmanufacturers> oraclePredicateResult = null)
        {
            var query = oraclePredicateResult.IsStarted
                ? _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(oraclePredicateResult)
                : _repositoryWrapper.OriginalEquipmentManufacturer.FindAll();
          
            
            return query
                .Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .AsEnumerable().Select(p => OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(
               x => x.Originalequipmentmanufacturer.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ", ""), true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Orgeqpmanufacturerid
                };
            }
            OriginalEquipmentManufacturer entity = new OriginalEquipmentManufacturer() { OriginalEquipmentManufacturerId = dto.Id, OriginalEquipmentManufacturerDescription = dto.Description };
            _repositoryWrapper.OriginalEquipmentManufacturer.Create(OriginalEquipmentManufacturerMapper.SetOriginalEquipmentManufacturerMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<ResultDto> Update(TipologicaGridDto dto)
        {
            var entityExists = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(
               x => x.Orgeqpmanufacturerid != dto.Id && x.Originalequipmentmanufacturer.ToLower().Replace(" ","") == dto.Description.ToLower().Replace(" ", "") && !x.Deleted.Value).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Orgeqpmanufacturerid
                };
            }
            OriginalEquipmentManufacturer entity = new OriginalEquipmentManufacturer() { OriginalEquipmentManufacturerId = dto.Id, OriginalEquipmentManufacturerDescription = dto.Description };
            _repositoryWrapper.OriginalEquipmentManufacturer.Update(OriginalEquipmentManufacturerMapper.SetOriginalEquipmentManufacturerMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
        }

        public async Task<ResultDto> Delete(short id)
        {
            var entity = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == id).SingleAsync();
            _repositoryWrapper.OriginalEquipmentManufacturer.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Orgeqpmanufacturerid
            };
        }

        public async Task<ResultDto> DeleteDeep(short id)
        {
            var entity = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == id).SingleAsync();
            _repositoryWrapper.OriginalEquipmentManufacturer.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Orgeqpmanufacturerid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var NetworkElementAsIs = _repositoryWrapper.NetworkElementAsIs
                         .FindByCondition(x => x.Orgeqpmanufacturerid == id)
                         .Select(x => NetworkElementAsIsMapper.Get(x).toDescription())
                         .ToArray();

            var BundleUpgradeInitiative = _repositoryWrapper.BundleUpgradeInitiative
                         .FindByCondition(x => x.Orgeqpmanufacturerid == id)
                         .Select(x => x.Vnftype)
                         .ToArray();

            var MajorHardwareBuild = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Orgeqpmanufacturerid == id)
                .Include(x => x.Orgeqpmanufacturer)
                .Include(x => x.Platform)
                .Select(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + x.Platform.Platform + " - " + x.Hardwaretype).ToArray();
            var MajorSoftwareBuild = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Orgeqpmanufacturerid == id)
                .Include(x => x.Orgeqpmanufacturer)
                .Select(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + x.Productname != null ?x.Productname.Description:"" + " - " + x.Softwareversion).ToArray();
            
            
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (NetworkElementAsIs.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Is", Values = NetworkElementAsIs });
            if (BundleUpgradeInitiative.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Bundle Upgrade Initiative", Values = BundleUpgradeInitiative });
            if (MajorHardwareBuild.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Major Hardware Build", Values = MajorHardwareBuild });
            if (MajorSoftwareBuild.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Major Software Build", Values = MajorSoftwareBuild });

            var entity = await _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == id).SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Original Equipment Manufacturer",
                        RecordName = entity.Originalequipmentmanufacturer,
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
            var entity = OriginalEquipmentManufacturerMapper.GetOriginalEquipmentManufacturerMapper(_repositoryWrapper.OriginalEquipmentManufacturer
                .FindByCondition(x => x.Orgeqpmanufacturerid == id).Include(x => x.ModificationuserNavigation).Single());
            var dto = new TipologicaGridDto() { Id = (short)entity.OriginalEquipmentManufacturerId, LastModifiedBy = entity.ModificationUserEntity.Email, Description = entity.OriginalEquipmentManufacturerDescription, LastModified = entity.ModificationDate };
            return dto;
        }

     
    }
}

   
