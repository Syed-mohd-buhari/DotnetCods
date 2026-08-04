using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using CAM.BusinessManager.Grid;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.BundleUpgradeInitiative;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class BundleUpgradeInitiativeManager : GridBaseAsync<BundleUpgradeInitiative, BundleUpgradeInitiativeDtoGrid, BundleUpgradeInitiativeQueryDto, Bundleupgradeinitiatives>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;

        public BundleUpgradeInitiativeManager(IEnumerable<IRepositoryWrapper> wrappers, GridCustomColumnManager columnManager, 
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(columnManager,contextAccessor,wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

       
        public override ExpressionStarter<Bundleupgradeinitiatives> ApplyFilterForOracleModel(BundleUpgradeInitiativeQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Bundleupgradeinitiatives>();
            var predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
            if (request.OriginalEquipmentManufacturer != null && request.OriginalEquipmentManufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.OriginalEquipmentManufacturer)
                    predicateInner.Or(x => x.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }
            if (request.BundleUpgradeInitiativeId != null && request.BundleUpgradeInitiativeId.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.BundleUpgradeInitiativeId)
                    predicateInner.Or(x => x.Bundleupgradeinitiativeid == item);
                predicateResult.And(predicateInner);
            }
            if (request.OemCertifiedRelease != null && request.OemCertifiedRelease.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.OemCertifiedRelease)
                    predicateInner.Or(x => x.Oemcertifiedrelease == item);
                predicateResult.And(predicateInner);
            }
          
            if (request.VnfType != null && request.VnfType.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.VnfType)
                    predicateInner.Or(x => x.Vnftype == item);
                predicateResult.And(predicateInner);
            }
            if (request.VerticalOwner != null && request.VerticalOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.VerticalOwner)
                    predicateInner.Or(x => x.Verticalowner == item);
                predicateResult.And(predicateInner);
            }
            if (request.Remarks != null && request.Remarks.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.Remarks)
                    predicateInner.Or(x => x.Remarks == item);
                predicateResult.And(predicateInner);
            }
            if (request.Spare1Json != null && request.Spare1Json.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.Spare1Json)
                    predicateInner.Or(x => x.Spare1json == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                if (request.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModifiedValue.StartDate);

                if (request.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Bundleupgradeinitiatives>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        public override List<BundleUpgradeInitiativeDtoGrid> CastObjectToDto(IQueryable<BundleUpgradeInitiative> request)
        {
            return request.Select(dto => new BundleUpgradeInitiativeDtoGrid()
            {
                VnfType = dto.VNFType,
                VerticalOwner = dto.VerticalOwner,
                Spare1Json = dto.Spare1Json,
                OemCertifiedRelease = dto.OEMCertifiedRelease,
                OriginalEquipmentManufacturer = dto.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription,
                Remarks = dto.Remarks,
                LastModified = dto.ModificationDate,
                BundleUpgradeInitiativeId = dto.BundleUpgradeInitiativeId,
                Deleted = dto.Deleted,
                LastModifiedBy =  dto.ModificationUserEntity.Email,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<BundleUpgradeInitiative, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<BundleUpgradeInitiative, object>>[]>
            {
                ["verticalOwner"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.VerticalOwner },
                ["originalEquipmentManufacturer"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription },
                ["oemCertifiedRelease"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.OEMCertifiedRelease },
                ["vnfType"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.VNFType },
                ["remarks"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.Remarks },
                ["spare1Json"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.Spare1Json },
                ["lastModifiedValue"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<BundleUpgradeInitiative, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<BundleUpgradeInitiative> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "bundleUpgradeInitiativeId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.BundleUpgradeInitiativeId.ToString()))
                : request.Where(x => x.BundleUpgradeInitiativeId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.BundleUpgradeInitiativeId.ToString())),
                "vnfType" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.VNFType))
                : request.Where(x => x.VNFType.Contains(propertyFilter)).Select(x => new FilterValueDto(x.VNFType)),
                "verticalOwner" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.VerticalOwner))
                : request.Where(x => x.VerticalOwner.Contains(propertyFilter)).Select(x => new FilterValueDto(x.VerticalOwner)),
                "oemCertifiedRelease" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.OEMCertifiedRelease))
                : request.Where(x => x.OEMCertifiedRelease.Contains(propertyFilter)).Select(x => new FilterValueDto(x.OEMCertifiedRelease)),
                "remarks" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Remarks))
                : request.Where(x => x.Remarks.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Remarks)),
                "spare1Json" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Spare1Json))
                : request.Where(x => x.Spare1Json.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Spare1Json)),
                "originalEquipmentManufacturer" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto
                    {
                        Text = p.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription,
                        Value = p.OriginalEquipmentManufacturerId.ToString()
                    }).Distinct()
                    : request
                        .Where(x =>
                            x.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription,
                                    Value = p.OriginalEquipmentManufacturerId.ToString()
                                }).Distinct(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
            };
        }

        private static IQueryable<FilterValueDto> Select(IQueryable<BundleUpgradeInitiative> request, string propertyFilter)
        {
            return string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.BundleUpgradeInitiativeId.ToString()))
                : request.Where(x => x.BundleUpgradeInitiativeId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.BundleUpgradeInitiativeId.ToString()));
        }
        public override IQueryable<BundleUpgradeInitiative> PrepareQuery(BundleUpgradeInitiativeQueryDto request, ExpressionStarter<BundleUpgradeInitiative> predicateResult , ExpressionStarter<Bundleupgradeinitiatives> oracleObject = null)
        {
            var query = oracleObject.IsStarted
               ? _repositoryWrapper.BundleUpgradeInitiative.FindByCondition(oracleObject, request.Deleted ?? false)
               : _repositoryWrapper.BundleUpgradeInitiative.FindAll(request.Deleted ?? false);


            return query
           .Include(m=>m.Orgeqpmanufacturer)
           .Include(m => m.ModificationuserNavigation)
           .Include(m => m.CreationuserNavigation)
           .AsEnumerable().Select(p => BundleUpgradeInitiativeMapper.GetBundleUpgradeInitiativeMapper(p)).AsQueryable();
        }

        public async Task<ResultDto> Add(BundleUpgradeInitiativeDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.BundleUpgradeInitiative.FindByCondition(
                x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId
                && x.Vnftype == dto.VnfType, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Bundleupgradeinitiativeid
                };
            }
            BundleUpgradeInitiative entity = new BundleUpgradeInitiative()
            {
                VNFType = dto.VnfType,
                VerticalOwner = dto.VerticalOwner,
                Spare1Json = dto.Spare1Json,
                OEMCertifiedRelease = dto.OemCertifiedRelease,
                OriginalEquipmentManufacturerId = dto.OriginalEquipmentManufacturerId,
                Remarks = dto.Remarks

            };
            _repositoryWrapper.BundleUpgradeInitiative.Create(BundleUpgradeInitiativeMapper.SetBundleUpgradeInitiativeMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }
        public async Task<ResultDto> Restore(long id)
        {
            var entity = await _repositoryWrapper.BundleUpgradeInitiative.FindByCondition(x => x.Bundleupgradeinitiativeid == id, true).SingleAsync();

            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.BundleUpgradeInitiative.FindByCondition(
               x => x.Orgeqpmanufacturerid == entity.Orgeqpmanufacturerid
                  && x.Vnftype == entity.Vnftype
                  && x.Bundleupgradeinitiativeid != entity.Bundleupgradeinitiativeid).FirstOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.Bundleupgradeinitiativeid
                };
            }
            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.BundleUpgradeInitiative.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Bundleupgradeinitiativeid
            };
        }
        public async Task<ResultDto> Update(BundleUpgradeInitiativeDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.BundleUpgradeInitiative.FindByCondition(
                x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId
                   && x.Vnftype == dto.VnfType
                   && x.Bundleupgradeinitiativeid != dto.BundleUpgradeInitiativeId, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                    Data = entityExists.Bundleupgradeinitiativeid
                };
            }

            BundleUpgradeInitiative entity = new BundleUpgradeInitiative()
            {
                BundleUpgradeInitiativeId = dto.BundleUpgradeInitiativeId,
                VNFType = dto.VnfType,
                VerticalOwner = dto.VerticalOwner,
                Spare1Json = dto.Spare1Json,
                OEMCertifiedRelease = dto.OemCertifiedRelease,
                OriginalEquipmentManufacturerId = dto.OriginalEquipmentManufacturerId,
                Remarks = dto.Remarks
            };
            _repositoryWrapper.BundleUpgradeInitiative.Update(BundleUpgradeInitiativeMapper.SetBundleUpgradeInitiativeMapper(entity));
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.BundleUpgradeInitiativeId
            };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.BundleUpgradeInitiative.FindByCondition(x => x.Bundleupgradeinitiativeid == id).SingleAsync();

            _repositoryWrapper.BundleUpgradeInitiative.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Bundleupgradeinitiativeid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {

            var entity = await _repositoryWrapper.BundleUpgradeInitiative.FindByCondition(x => x.Bundleupgradeinitiativeid == id).SingleAsync();
            _repositoryWrapper.BundleUpgradeInitiative.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Bundleupgradeinitiativeid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            var entities = new string[] { };
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            var entity = await _repositoryWrapper.BundleUpgradeInitiative
              .FindByCondition(x => x.Bundleupgradeinitiativeid == id)
              .Include(x => x.Orgeqpmanufacturer)
              .SingleAsync();
            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Bundle Upgrade Initiative",
                        RecordName = entity.Verticalowner + " - " + entity.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + entity.Vnftype,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }


        public BundleUpgradeInitiativeDtoCreate GetCreatePage()
        {
            var resource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll();

            var model = new BundleUpgradeInitiativeDtoCreate
            {
                OriginalEquipmentManufacturerResource = resource.ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer)

            };
            return model;


        }

        public BundleUpgradeInitiativeDtoUpdate GetUpdatePage(long id)
        {
            var entity = BundleUpgradeInitiativeMapper.GetBundleUpgradeInitiativeMapper(_repositoryWrapper.BundleUpgradeInitiative.FindByCondition(x => x.Bundleupgradeinitiativeid == id, true)
                .Include(x => x.ModificationuserNavigation).Single());
            var dto = new BundleUpgradeInitiativeDtoUpdate
            {
                //OriginalEquipmentManufacturerResource = resource.ToDictionary(x => x.OriginalEquipmentManufacturerId,
                //   x => x.OriginalEquipmentManufacturerDescription),
                BundleUpgradeInitiativeId = entity.BundleUpgradeInitiativeId,
                OemCertifiedRelease = entity.OEMCertifiedRelease,
                Remarks = entity.Remarks,
                Spare1Json = entity.Spare1Json,
                VerticalOwner = entity.VerticalOwner,
                VnfType = entity.VNFType,
                OriginalEquipmentManufacturerId = entity.OriginalEquipmentManufacturerId,
                LastModified = entity.ModificationDate,
                 LastModifiedBy = entity.ModificationUserEntity.Email
            };
            #region lookUp
            var resource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll();

            dto.OriginalEquipmentManufacturerResource = resource.ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer);

            if (!dto.OriginalEquipmentManufacturerResource.ContainsKey(dto.OriginalEquipmentManufacturerId))
            {
                var oem = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId,
                    includeDeleted: true).SingleOrDefault();
                if (oem != null)
                {
                    dto.OriginalEquipmentManufacturerResource.Add(oem.Orgeqpmanufacturerid, oem.Originalequipmentmanufacturer);
                }

            }

            #endregion



            return dto;
        }

       
    }
}
