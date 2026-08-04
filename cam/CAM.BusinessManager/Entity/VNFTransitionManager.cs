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
using CAM.DataTransferObjects.Entita.VNFTransition;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class VNFTransitionManager : GridBaseAsync<VNFTransition, VNFTransitionDtoGrid, VnfTransitionQueryDto, Vnftransitions>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _columnManager;


        public VNFTransitionManager(IEnumerable<IRepositoryWrapper> wrappers ,
            GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper) : base(columnManager, contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _columnManager = columnManager;
        }

        public override ExpressionStarter<Vnftransitions> ApplyFilterForOracleModel(VnfTransitionQueryDto request)
        {
            var predicateResult = PredicateBuilder.New<Vnftransitions>();
            var predicateInner = PredicateBuilder.New<Vnftransitions>();
            if (request.EquipmentStatus != null && request.EquipmentStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.EquipmentStatus)
                    predicateInner.Or(x => x.Equipmentstatusid == item);
                predicateResult.And(predicateInner);
            }
            if (request.OpCo != null && request.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.OpCo)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedBy != null && request.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (request.NfviBundleID != null && request.NfviBundleID.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.NfviBundleID)
                    predicateInner.Or(x => x.Nfvibundleidid == item);
                predicateResult.And(predicateInner);
            }
            if (request.VnfDesignComponent != null && request.VnfDesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.VnfDesignComponent)
                    predicateInner.Or(x => x.Vnfdesigncomponentid == item);
                predicateResult.And(predicateInner);
            }
            if (request.VnfTransitionId != null && request.VnfTransitionId.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.VnfTransitionId)
                    predicateInner.Or(x => x.Vnftransitionid == item);
                predicateResult.And(predicateInner);
            }
            if (request.CurrentRelease != null && request.CurrentRelease.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.CurrentRelease)
                    predicateInner.Or(x => x.Currentrelease == item);
                predicateResult.And(predicateInner);
            }
            if (request.ElementName != null && request.ElementName.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.ElementName)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (request.VnfType != null && request.VnfType.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.VnfType)
                    predicateInner.Or(x => x.Vnftype == item);
                predicateResult.And(predicateInner);
            }
            if (request.Location != null && request.Location.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.Location)
                    predicateInner.Or(x => x.Location == item);
                predicateResult.And(predicateInner);
            }
            if (request.NfviSiteDesignation != null && request.NfviSiteDesignation.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.NfviSiteDesignation)
                    predicateInner.Or(x => x.Nfvisitedesignation == item);
                predicateResult.And(predicateInner);
            }

            if (request.PlannedRelease != null && request.PlannedRelease.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.PlannedRelease)
                    predicateInner.Or(x => x.Plannedrelease == item);
                predicateResult.And(predicateInner);
            }
            if (request.Spare1Json != null && request.Spare1Json.Any())
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                foreach (var item in request.Spare1Json)
                    predicateInner.Or(x => x.Spare1json == item);
                predicateResult.And(predicateInner);
            }
            if (request.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                if (request.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= request.LastModifiedValue.StartDate);

                if (request.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= request.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (request.Deleted != null)
            {
                predicateInner = PredicateBuilder.New<Vnftransitions>();
                if (request.Deleted != null)
                    predicateInner.And(x => x.Deleted == request.Deleted);
                predicateResult.And(predicateInner);
            }



            return predicateResult;
        }
        public override List<VNFTransitionDtoGrid> CastObjectToDto(IQueryable<VNFTransition> request)
        {
            return  request.Select(dto => new VNFTransitionDtoGrid()
            {
                VNFTransitionId = dto.VNFTransitionId,
                CurrentRelease = dto.CurrentRelease,
                PlannedRelease = dto.PlannedRelease,
                Spare1Json = dto.Spare1Json,
                ElementName = dto.ElementName,
                NfviSiteDesignation = dto.NFVISiteDesignation,
                NfviBundleID = dto.NFVIBundleID != null ?dto.NFVIBundleID.NFVIBundleIdDescription : string.Empty,
                Location = dto.Location,
                LastModified = dto.ModificationDate,
                OpCo = dto.OpCo.OpCoDescription,
                EquipmentStatus = dto.EquipmentStatus != null ? dto.EquipmentStatus.EquipmentStatusDescription : string.Empty,
                vnfDesignComponent = dto.VNFDesignComponent != null ?  dto.VNFDesignComponent.VNFDesignComponentDescription : string.Empty,
                VnfType = dto.VNFType,
                Deleted = dto.Deleted,
                LastModifiedBy = dto.ModificationUserEntity.Email,
                LastModifiedValue = dto.ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
            }).ToList();
        }

        public override Dictionary<string, Expression<Func<VNFTransition, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<VNFTransition, object>>[]>
            {
                ["vnfTransitionId"] = new Expression<Func<VNFTransition, object>>[] { p => p.VNFTransitionId },
                ["vnfDesignComponent"] = new Expression<Func<VNFTransition, object>>[] { p => p.VNFDesignComponent.VNFDesignComponentId },
                ["opCoId"] = new Expression<Func<VNFTransition, object>>[] { p => p.OpCo.OpCoDescription },
                ["equipmentStatus"] = new Expression<Func<VNFTransition, object>>[] { p => p.EquipmentStatus.EquipmentStatusId },
                ["vnfType"] = new Expression<Func<VNFTransition, object>>[] { p => p.VNFType },
                ["currentRelease"] = new Expression<Func<VNFTransition, object>>[] { p => p.CurrentRelease },
                ["plannedRelease"] = new Expression<Func<VNFTransition, object>>[] { p => p.PlannedRelease },
                ["elementName"] = new Expression<Func<VNFTransition, object>>[] { p => p.ElementName },
                ["spare1Json"] = new Expression<Func<VNFTransition, object>>[] { p => p.Spare1Json },
                ["location"] = new Expression<Func<VNFTransition, object>>[] { p => p.Location },
                ["nfviBundleID"] = new Expression<Func<VNFTransition, object>>[] { p => p.NFVIBundleID.NFVIBundleIdDescription },
                ["nfviSiteDesignation"] = new Expression<Func<VNFTransition, object>>[] { p => p.NFVISiteDesignation },
                ["spareFieldsJson"] = new Expression<Func<VNFTransition, object>>[] { p => p.Spare1Json },
                ["lastModified"] = new Expression<Func<VNFTransition, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<VNFTransition, object>>[] { p => p.ModificationUserEntity.Email },
            };
        }

        public override async Task<IEnumerable<FilterValueDto>> GetFilterValueList(IQueryable<VNFTransition> request, string propertyName, string propertyFilter)
        {
            return propertyName switch
            {
                "vnfTransitionId" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(x => new FilterValueDto(x.VNFTransitionId.ToString()))
                : request.Where(x => x.VNFTransitionId.ToString() == propertyFilter).Select(x => new FilterValueDto(x.VNFTransitionId.ToString())),
                "vnfType" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.VNFType))
                : request.Where(x => x.VNFType.Contains(propertyFilter)).Select(x => new FilterValueDto(x.VNFType)),
                "currentRelease" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.CurrentRelease))
                : request.Where(x => x.CurrentRelease.Contains(propertyFilter)).Select(x => new FilterValueDto(x.CurrentRelease)),
                "plannedRelease" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.PlannedRelease))
                : request.Where(x => x.PlannedRelease.Contains(propertyFilter)).Select(x => new FilterValueDto(x.PlannedRelease)),
                "elementName" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.ElementName))
                : request.Where(x => x.ElementName.Contains(propertyFilter)).Select(x => new FilterValueDto(x.ElementName)),
                "location" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Location))
                : request.Where(x => x.Location.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Location)),
                "spare1Json" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.Spare1Json))
                : request.Where(x => x.Spare1Json.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Spare1Json)),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? request.Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList()
                    : request
                        .Where(x =>
                            x.ModificationUserEntity.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationUserEntity.Email)).Distinct().ToList(),
                "nfviSiteDesignation" => string.IsNullOrEmpty(propertyFilter) ? request.Select(x => new FilterValueDto(x.NFVISiteDesignation))
                : request.Where(x => x.NFVISiteDesignation.Contains(propertyFilter)).Select(x => new FilterValueDto(x.NFVISiteDesignation)),
                "opCo" => string.IsNullOrEmpty(propertyFilter)
                   ? request.Select(p => new FilterValueDto
                   {
                       Text = p.OpCo.OpCoDescription,
                       Value = p.OpCoId.ToString()
                   }).Distinct()
                   : request
                       .Where(x =>
                            x.OpCo.OpCoDescription.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.OpCo.OpCoDescription,
                                    Value = p.OpCoId.ToString()
                                }).Distinct(),
                "nfviBundleID" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(p => new FilterValueDto
                {
                    Text = p.NFVIBundleID.NFVIBundleIdDescription,
                    Value = p.NFVIBundleIDId.ToString()
                }).Distinct()
                               : request
                                   .Where(x =>
                                        x.NFVIBundleID.NFVIBundleIdDescription.Contains(
                                            propertyFilter)).Select(p => new FilterValueDto
                                            {
                                                Text = p.NFVIBundleID.NFVIBundleIdDescription,
                                                Value = p.NFVIBundleIDId.ToString()
                                            }).Distinct(),
                "vnfDesignComponent" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(p => new FilterValueDto
                {
                    Text = p.VNFDesignComponent.VNFDesignComponentDescription,
                    Value = p.VNFDesignComponentId.ToString()
                }).Distinct()
                : request
                    .Where(x =>
                         x.VNFDesignComponent.VNFDesignComponentDescription.Contains(
                             propertyFilter)).Select(p => new FilterValueDto
                             {
                                 Text = p.VNFDesignComponent.VNFDesignComponentDescription,
                                 Value = p.VNFDesignComponentId.ToString()
                             }).Distinct(),
                "equipmentStatus" => string.IsNullOrEmpty(propertyFilter)
                ? request.Select(p => new FilterValueDto
                {
                    Text = p.EquipmentStatus.EquipmentStatusDescription,
                    Value = p.EquipmentStatusId.ToString()
                }).Distinct()
                : request
                    .Where(x =>
                         x.EquipmentStatus.EquipmentStatusDescription.Contains(
                             propertyFilter)).Select(p => new FilterValueDto
                             {
                                 Text = p.EquipmentStatus.EquipmentStatusDescription,
                                 Value = p.EquipmentStatusId.ToString()
                             }).Distinct(),
            };
        }

        public override IQueryable<VNFTransition> PrepareQuery(VnfTransitionQueryDto request, ExpressionStarter<VNFTransition> predicateResult , ExpressionStarter<Vnftransitions> oracleObject = null)
        {
            var query = oracleObject.IsStarted
             ? _repositoryWrapper.VNFTransition.FindByCondition(oracleObject, request.Deleted ?? false)
                 .Include(x => x.Nfvibundleid)
             : _repositoryWrapper.VNFTransition.FindAll(request.Deleted ?? false)
                 .Include(x => x.Nfvibundleid);

            return query.Include(m => m.ModificationuserNavigation)
                .Include(m => m.CreationuserNavigation)
                .Include(m=>m.Vnfdesigncomponent)
                .Include(m => m.Nfvibundleid)
                .Include(m => m.Equipmentstatus)
                .Include(m => m.Opco)
                .AsEnumerable().Select(p=> VNFTransitionMapper.Get(p)).AsQueryable() ;
        }


        public async Task<ResultDto> Add(VnfTransitionDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.VNFTransition.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                && x.Vnftype == dto.VnfType
                && x.Elementname == dto.ElementName, true).FirstOrDefaultAsync();
            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                    Data = entityExists.Vnftransitionid
                };
            }

            dto.NfviBundleIDId = dto.NfviBundleIDId == 0 ? null : dto.NfviBundleIDId;
            dto.EquipmentStatusId = dto.EquipmentStatusId == 0 ? null : dto.EquipmentStatusId;
            dto.VnfDesignComponentId = dto.VnfDesignComponentId == 0 ? null : dto.VnfDesignComponentId;

            VNFTransition entity = new VNFTransition()
            {
                VNFType = dto.VnfType,
                VNFDesignComponentId = dto.VnfDesignComponentId,
                CurrentRelease = dto.CurrentRelease,
                ElementName = dto.ElementName,
                PlannedRelease = dto.PlannedRelease,
                OpCoId = dto.OpCoId,
                NFVIBundleIDId = dto.NfviBundleIDId,
                Location = dto.Location,
                EquipmentStatusId = dto.EquipmentStatusId,
                NFVISiteDesignation = dto.NfviSiteDesignation,
                Spare1Json = dto.Spare1Json,

            };
            _repositoryWrapper.VNFTransition.Create(VNFTransitionMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            await UpdateNetworkElementAsPlanned(entity);
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }
        public async Task<ResultDto> Restore(long id)
        {
            var entity = await _repositoryWrapper.VNFTransition.FindByCondition(x => x.Vnftransitionid == id, true).SingleAsync();
            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.VNFTransition.FindByCondition(
                         x => x.Opcoid == entity.Opcoid
                         && x.Vnftype == entity.Vnftype
                         && x.Elementname == entity.Elementname
                         && x.Vnftransitionid != entity.Vnftransitionid).FirstOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.Vnftransitionid
                };
            }
            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.VNFTransition.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Vnftransitionid
            };
        }
        public async Task<ResultDto> Update(VNFTransitionDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.VNFTransition.FindByCondition(
                x => x.Opcoid == dto.OpCoId
               && x.Vnftype == dto.VnfType
               && x.Elementname == dto.ElementName
                  && x.Vnftransitionid != dto.VnfTransitionId, true).FirstOrDefaultAsync();

            if (entityExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = entityExists.Deleted.Value ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                    Data = entityExists.Vnftransitionid
                };
            }


            dto.NfviBundleIDId = dto.NfviBundleIDId == 0 ? null : dto.NfviBundleIDId;
            dto.EquipmentStatusId = dto.EquipmentStatusId == 0 ? null : dto.EquipmentStatusId;
            dto.VnfDesignComponentId = dto.VnfDesignComponentId == 0 ? null : dto.VnfDesignComponentId;


            VNFTransition entity = new VNFTransition()
            {
                VNFTransitionId = dto.VnfTransitionId,
                VNFType = dto.VnfType,
                VNFDesignComponentId = dto.VnfDesignComponentId,
                CurrentRelease = dto.CurrentRelease,
                ElementName = dto.ElementName,
                PlannedRelease = dto.PlannedRelease,
                OpCoId = dto.OpCoId,
                NFVIBundleIDId = dto.NfviBundleIDId,
                Location = dto.Location,
                EquipmentStatusId = dto.EquipmentStatusId,
                NFVISiteDesignation = dto.NfviSiteDesignation,
                Spare1Json = dto.Spare1Json,
            };
            _repositoryWrapper.VNFTransition.Update(VNFTransitionMapper.Set(entity));
            await _repositoryWrapper.SaveAsync();
            await UpdateNetworkElementAsPlanned(entity);

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.VNFTransitionId
            };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.VNFTransition.FindByCondition(x => x.Vnftransitionid == id).SingleAsync();

            _repositoryWrapper.VNFTransition.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vnftransitionid
            };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.VNFTransition.FindByCondition(x => x.Vnftransitionid == id).SingleAsync();

            _repositoryWrapper.VNFTransition.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Vnftransitionid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(short id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            var entity = await _repositoryWrapper.VNFTransition.FindByCondition(x => x.Vnftransitionid == id)
              .Include(x => x.Opco)
              .Include(x => x.Vnfdesigncomponent).SingleAsync();


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto
                    {
                        EntityName = "System Type",
                        RecordName = entity.Opco.Opco + " - " + entity.Vnftype + " - " + entity.Elementname,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }

        public VnfTransitionDtoCreate GetCreatePage(List<short> _opcoList)
        {
            var opocResource = (_opcoList != null) ? _repositoryWrapper.OpCo
                .FindByCondition(x => _opcoList.Contains(x.Opcoid)) : _repositoryWrapper.OpCo.FindAll();
            var vnfDesignComponentResource = _repositoryWrapper.VNFDesignComponent.FindAll();
            var equipmentStatusResource = _repositoryWrapper.EquipmentStatus.FindAll();
            var vnfTypeResource = _repositoryWrapper.BundleUpgradeInitiative.FindAll().Select(x => x.Vnftype).Distinct().ToList();
            var nfviBundleIDResource = _repositoryWrapper.NFVIBundleID.FindAll();
            var nfviSiteDesignationResource = _repositoryWrapper.NFVITransitionRepository.FindAll().Select(x => x.Nfvisitedesignation).Distinct().ToList();
            var model = new VnfTransitionDtoCreate
            {
                OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco),
                VnfDesignComponentResource = vnfDesignComponentResource.ToDictionary(x => x.Vnfdesigncomponentid, x => x.Designcomponent),
                EquipmentStatusResource = equipmentStatusResource.ToDictionary(x => x.Equipmentstatusid, x => x.Equipmentstatus),
                VnfTypeResource = vnfTypeResource,
                NfviBundleIDResource = nfviBundleIDResource.ToDictionary(x => x.Nfvibundleidid, x => new RelatedResource()
                {
                    Id = x.Order.ToString(),
                    Value = x.Nfvibundleid,
                }),
                NfviSiteDesignationResource = nfviSiteDesignationResource,
            };
            return model;

        }

        public VNFTransitionDtoUpdate GetUpdatePage(long id, List<short> _opcoList)
        {
            var entity = VNFTransitionMapper.Get( _repositoryWrapper.VNFTransition.FindByCondition(x => x.Vnftransitionid == id, true)
                .Include(x => x.ModificationuserNavigation).Single());

            var dto = new VNFTransitionDtoUpdate
            {
                VnfTransitionId = entity.VNFTransitionId,
                VnfDesignComponentId = entity.VNFDesignComponentId,
                Spare1Json = entity.Spare1Json,
                VnfType = entity.VNFType,
                LastModified = entity.ModificationDate,
                Location = entity.Location,
                NfviSiteDesignation = entity.NFVISiteDesignation,
                ElementName = entity.ElementName,
                OpCoId = entity.OpCoId,
                EquipmentStatusId = entity.EquipmentStatusId,
                CurrentRelease = entity.CurrentRelease,
                PlannedRelease = entity.PlannedRelease,
                NfviBundleIDId = entity.NFVIBundleIDId,
                LastModifiedBy = entity.ModificationUserEntity.Email

            };

            #region lookUp
            var opocResource = (_opcoList != null) ? _repositoryWrapper.OpCo
                .FindByCondition(x => _opcoList.Contains(x.Opcoid)) : _repositoryWrapper.OpCo.FindAll();
            dto.OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco);
            if (!dto.OpCoResource.ContainsKey(dto.OpCoId))
            {
                var opco = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, includeDeleted: true).SingleOrDefault();
                if (opco != null)
                {
                    dto.OpCoResource.Add(opco.Opcoid, opco.Opco);
                }

            }

            var vnfDesignComponentResource = _repositoryWrapper.VNFDesignComponent.FindAll();
            dto.VnfDesignComponentResource = vnfDesignComponentResource.ToDictionary(x => x.Vnfdesigncomponentid, x => x.Designcomponent);
            if (dto.VnfDesignComponentId.HasValue && !dto.VnfDesignComponentResource.ContainsKey(dto.VnfDesignComponentId.Value))
            {
                var dc = _repositoryWrapper.VNFDesignComponent.FindByCondition(
                    x => x.Vnfdesigncomponentid == dto.VnfDesignComponentId, includeDeleted: true).SingleOrDefault();
                if (dc != null)
                {
                    dto.VnfDesignComponentResource.Add(dc.Vnfdesigncomponentid, dc.Designcomponent);
                }
            }

            var equipmentStatusResource = _repositoryWrapper.EquipmentStatus.FindAll();
            dto.EquipmentStatusResource = equipmentStatusResource.ToDictionary(x => x.Equipmentstatusid,
                    x => x.Equipmentstatus);
            if (dto.EquipmentStatusId.HasValue && !dto.EquipmentStatusResource.ContainsKey(dto.EquipmentStatusId.Value))
            {
                var es = _repositoryWrapper.EquipmentStatus.FindByCondition(
                    x => x.Equipmentstatusid == dto.EquipmentStatusId, includeDeleted: true).SingleOrDefault();
                if (es != null)
                {
                    dto.EquipmentStatusResource.Add(es.Equipmentstatusid, es.Equipmentstatus);
                }
            }

            var vnfTypeResource = _repositoryWrapper.BundleUpgradeInitiative.FindAll().Select(x => x.Vnftype).Distinct().ToList();
            dto.VnfTypeResource = vnfTypeResource;
            if (!dto.VnfTypeResource.Contains(dto.VnfType))
            {
                dto.VnfTypeResource.Add(dto.VnfType);
            }


            var nfviBundleIDResource = _repositoryWrapper.NFVIBundleID.FindAll();
            dto.NfviBundleIDResource = nfviBundleIDResource.ToDictionary(x => x.Nfvibundleidid, x => new RelatedResource()
            {
                Id = x.Order.ToString(),
                Value = x.Nfvibundleid,
            });
            if (dto.NfviBundleIDId.HasValue && !dto.NfviBundleIDResource.ContainsKey(dto.NfviBundleIDId.Value))
            {
                var es = _repositoryWrapper.NFVIBundleID.FindByCondition(
                    x => x.Nfvibundleidid == dto.NfviBundleIDId, includeDeleted: true).SingleOrDefault();
                if (es != null)
                {
                    dto.NfviBundleIDResource.Add(es.Nfvibundleidid, new RelatedResource()
                    {
                        Id = es.Order.ToString(),
                        Value = es.Nfvibundleid
                    });
                }
            }
            var nfviSiteDesignationResource = _repositoryWrapper.NFVITransitionRepository.FindAll().Select(x => x.Nfvisitedesignation).Distinct().ToList();
            dto.NfviSiteDesignationResource = nfviSiteDesignationResource;
            if (!dto.NfviSiteDesignationResource.Contains(dto.NfviSiteDesignation))
            {
                dto.NfviSiteDesignationResource.Add(dto.NfviSiteDesignation);
            }
            #endregion
            return dto;
        }

        private async Task UpdateNetworkElementAsPlanned(VNFTransition entity)
        {
            if (entity.ElementName != null && entity.ElementName != "" && entity.NFVIBundleIDId != null)
            {
                var napToUpdate = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == entity.OpCoId && x.Elementname == entity.ElementName).ToListAsync();
                if (napToUpdate.Count > 0)
                {
                    foreach (var nap in napToUpdate)
                    {
                        // Ogni volta che "Bundle Id" viene aggiunto o modificato in VNF Transition per un particolare elemento di rete
                        // (OPCO + Element Name), la sua voce corrispondente allo stesso elemento di rete in Network Element (As-Planned),
                        // se presente, deve essere aggiornata.
                        nap.Nfvibundleidid = entity.NFVIBundleIDId;
                        _repositoryWrapper.NetworkElementAsPlanned.Update(nap);
                    }
                    await _repositoryWrapper.SaveAsync();
                }
            }
        }

       
    }
}
