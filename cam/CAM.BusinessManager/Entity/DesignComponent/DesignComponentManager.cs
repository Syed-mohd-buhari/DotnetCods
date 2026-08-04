using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity.DesigComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsPlanned;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.LookUp;
using CAM.BusinessManager.Rules;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.Entita.DesignComponentFamily;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class DesignComponentManager : BaseManager
    {
        private readonly DesignComponentFamilyManager _designComponentFamilyManager;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly LcmEngineeringManager _lcmEngineeringManager;
        private readonly GridCustomColumnManager manager;
        private readonly SubNetworkBoundaryManager _subNetworkBoundaryManager;
        private readonly CommonManager _commonManager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        public DesignComponentManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper,
            GridCustomColumnManager manager, LcmEngineeringManager lcmEngineeringManager,
            DesignComponentFamilyManager designComponentFamilyManager, SubNetworkBoundaryManager subNetworkBoundaryManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            this.manager = manager;
            _lcmEngineeringManager = lcmEngineeringManager;
            _designComponentFamilyManager = designComponentFamilyManager;
            _subNetworkBoundaryManager = subNetworkBoundaryManager;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
        }

        public async Task<ResultDto> GetProductNamesByVodfoneName(int VfNameId)
        {
            List<string> productNames = await _repositoryWrapper.ProductNameRepository
                .FindByCondition(x => x.Vodafonenamesid == VfNameId && VfNameId != 0)
                .Select(x => x.Description)
                .ToListAsync();
            if (productNames != null && productNames.Count > 0)
            {
                return new ResultDto
                {
                    Warning = false,
                    Info = ResultMessages.GetInfoSuccess,
                    Data = productNames
                };
            }
            else
            {
                return new ResultDto();
            }


        }

        public bool HasSupportedAllServices(int systemTypeId)
        {
            var hasSupportedAllSrv = true;

            var systemTypeWithDC = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Systemtypeid == systemTypeId)
               .Include(x => x.Subnetworkboundary);
            if (systemTypeWithDC != null && systemTypeWithDC.Count() > 0)
            {
                hasSupportedAllSrv = systemTypeWithDC.Any(x => x.Subnetworkboundary.Default == true);
            }
            else
            {
                hasSupportedAllSrv = false;
            }

            return hasSupportedAllSrv;
        }
        private async Task<IEnumerable<Designcomponents>> GetAllWithRelations()
        {
            return await _repositoryWrapper.DesignComponent.GetAllWithRelations();
        }

        public async Task<IEnumerable<Designcomponents>> FindAll()
        {
            var data = _repositoryWrapper.DesignComponent.FindAll();
            return data.AsEnumerable();
        }

        public async Task<ResultDto> Add(DesignComponentDtoCreate dto, bool? forced = false)
        {
            if ((dto.SubNetworkBoundaryIds != null && dto.SubNetworkBoundaryIds.Any()) || dto.SupportedAllServices)
            {

                if (dto.SupportedAllServices)
                {
                    if (dto.SubNetworkBoundaryIds == null)
                    {
                        dto.SubNetworkBoundaryIds = new List<long>();
                    }

                    dto.SubNetworkBoundaryIds.AddRange(AddDCDefaultSubnetworkBoundry(dto.SystemTypeId));
                }

                foreach (var item in dto.SubNetworkBoundaryIds)
                {
                    dto.DesignComponentFamilyId = await GetDesigComponentFamilyId(dto.SystemTypeId, item);
                    var model = _mapper.Map<DesignComponent>(dto);
                    model.SubNetworkBoundaryId = item;
                    var entityExists = await EntityExists(dto.SystemTypeId, dto.DesignComponentFamilyId, item);
                    if (entityExists != null)
                    {
                        var relations = _repositoryWrapper.Lcmengineering
                            .FindByCondition(x => x.Designcomponentid == entityExists.DesignComponentId, true).FirstOrDefault();
                        if (relations == null && entityExists.Deleted)
                        {
                            if (forced == true)
                            {
                                var entityForced = DesignComponentMapper.SetDesignComponentMapper(model);
                                _repositoryWrapper.DesignComponent.Create(entityForced);
                                await _repositoryWrapper.SaveAsync();
                                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
                            }

                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExists,
                                Data = new { id = entityExists.DesignComponentId, orphanDeleted = true }
                            };
                        }

                        return new ResultDto
                        {
                            Warning = true,
                            Info = entityExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entityExists.DesignComponentId
                        };
                    }
                    var result = await DesignComponentFamilyHandlerForDesignComponent(dto, item, 0);
                    dto.DesignComponentFamilyId = result.DCFId;
                    model.DesignComponentFamilyId = dto.DesignComponentFamilyId;
                    var entity = DesignComponentMapper.SetDesignComponentMapper(model);
                    _repositoryWrapper.DesignComponent.Create(entity);
                    await _repositoryWrapper.SaveAsync();
                    if (result.IsNewDCF)
                    {
                        var currentDCF = _repositoryWrapper.DesignComponentFamily
                            .FindByCondition(p => p.Designcomponentfamilyid == dto.DesignComponentFamilyId).SingleOrDefault();
                        currentDCF.Systemisshared = _designComponentFamilyManager.IsSystemShared(currentDCF.Designcomponentfamilyid);
                        await _repositoryWrapper.ClearTracker();
                        _repositoryWrapper.DesignComponentFamily.Update(currentDCF);
                        await _repositoryWrapper.SaveAsync();

                    }
                }
                return new ResultDto { Info = ResultMessages.EntryAddSuccess };
            }
            return new ResultDto
            {
                Warning = true,
                Info = ResultMessages.SubNetworkBoundariesEmptyLst,
            };
        }

        private async Task<long?> GetDesigComponentFamilyId(long systemTypeId, long subnetworkBoundaryId)
        {
            var systemType = await _repositoryWrapper.SystemType
                .FindByCondition(x => x.Systemtypeid == systemTypeId)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .SingleAsync();
            var designComponentFamily =
                await GetDesignComponentFamilyFromSystemType(systemType, subnetworkBoundaryId);

            return designComponentFamily?.DesignComponentFamilyId;
        }
        private async Task<DCFCreationModel> DesignComponentFamilyHandlerForDesignComponent(DesignComponentDtoCreate dto, long subnetworkboundryId, long designComponentId)
        {
            var systemType = await _repositoryWrapper.SystemType
                .FindByCondition(x => x.Systemtypeid == dto.SystemTypeId)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Include(x => x.Majorsoftwarebuilds).AsNoTracking()
                .SingleAsync();

            var designComponentFamily =
                await GetDesignComponentFamilyFromSystemType(systemType, subnetworkboundryId);

            if (designComponentFamily == null)
            {
                var result = await GenerateDesignComponentFamily(dto, subnetworkboundryId);
                return new DCFCreationModel() { DCFId = result, IsNewDCF = true };
                //if (designComponentId != 0)
                //{
                //    var designComponent = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == designComponentId).FirstOrDefault();
                //    if (designComponent.Subnetworkboundaryid != subnetworkboundryId || designComponent.Systemtypeid != dto.SystemTypeId)
                //    {
                //        var result = await UpdateDesignComponantFamily(designComponentId, subnetworkboundryId, dto.SystemTypeId);
                //        await UpdateDesignAspectSupportedServices(designComponentId, subnetworkboundryId);
                //        if (designComponent.Subnetworkboundaryid != subnetworkboundryId)
                //        {
                //            await UpdateOtherDesignComponants((long)result.Data, subnetworkboundryId);
                //        }
                //        return new DCFCreationModel() { DCFId = (long)result.Data, IsNewDCF = false };
                //    }

                //}
                //else
                //{
                //    var result = await GenerateDesignComponentFamily(dto, subnetworkboundryId);
                //    return new DCFCreationModel() { DCFId = result, IsNewDCF = true };

                //}
            }

            await _repositoryWrapper.ClearTracker();

            //designComponentFamily.GdprRelevant = dto.GdprRelevant;
            var mjh = systemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain);

            var majorHW = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == mjh.Majorhardwareid, false)
              .Include(x => x.Buildconstruction).SingleOrDefault();

            var plid = majorHW?.Platformid;

            var platform = (await _repositoryWrapper.Platform.FindByCondition(x =>
                x.Platformid == plid, true, false).SingleOrDefaultAsync()).Platform;


            var hardwareSolution = majorHW.Hardwaresolution;
            var buildConstructionRule = majorHW.Buildconstruction.Rule;
            var productName = systemType.Majorsoftwarebuilds.Productname != null ? systemType.Majorsoftwarebuilds.Productname.Description : "";
            designComponentFamily.SystemTypeIdentityName = await Utils.STIM(_repositoryWrapper, designComponentFamily.MajorHardwareOemId, designComponentFamily.MajorSoftwareOemId, productName, platform, hardwareSolution, buildConstructionRule);

            await _repositoryWrapper.ClearTracker();
            await _designComponentFamilyManager.Update(_mapper.Map<DesignComponentFamilyDtoUpdate>(designComponentFamily), true);
            await _repositoryWrapper.SaveAsync();
            var dcfId = designComponentFamily.DesignComponentFamilyId;

            return new DCFCreationModel() { DCFId = dcfId, IsNewDCF = false };
        }

        private async Task<DesignComponentFamily> GetDesignComponentFamilyFromSystemType(Systemtypes systemType,
            long subnetworkBoundryId)
        {
            var majorHardware = systemType.Systemtypesmajorhardwarebuilds
                .FirstOrDefault(s => s.Ismain)?
                .Majorhardware;
            var productNameId = systemType.Majorsoftwarebuilds != null ? systemType.Majorsoftwarebuilds.Productnameid : (decimal?)null;
            var vfName = systemType.Vodafonename;
            var originalEquipmentManufacturerId = systemType.Majorsoftwarebuilds.Orgeqpmanufacturerid;
            //var transaction = await _repositoryWrapper.BeginTransactionAsync();
            var designComponentFamily = await GetDesignComponentFamilyUniqueQuery(
                majorHardware?.Orgeqpmanufacturerid ?? 0, productNameId, vfName, originalEquipmentManufacturerId,
                subnetworkBoundryId, majorHardware?.Platformid ?? 0);
            return designComponentFamily;
        }

        public async Task<DesignComponentFamily> GetDesignComponentFamilyUniqueQuery(
            long majorHardwareOriginalEquipmentManufacturerId, decimal? productNameId, int? vfNAmeId,
            long originalEquipmentManufacturerId, long subnetworkBoundaryId, long platformId, bool supportedAllService = false)
        {
            var subnetwork_AllSupportedService = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(p => p.Default != null && p.Default.Value == true && p.Vodafonenameid == vfNAmeId).FirstOrDefault();
            subnetworkBoundaryId = supportedAllService ? (subnetwork_AllSupportedService == null ? AddDCDefaultSubnetworkBoundry(vfNAmeId).FirstOrDefault() : subnetwork_AllSupportedService.Id) : subnetworkBoundaryId;

            return DesignComponentFamilyMapper.Get(
                await
                _repositoryWrapper.DesignComponentFamily
                    .FindByCondition(x => x.Productname != null &&
                    x.Majorhardwareoemid == majorHardwareOriginalEquipmentManufacturerId
                    && x.Productnameid == productNameId
                    && x.Majorsoftwareoemid == originalEquipmentManufacturerId
                    && x.Subnetworkboundaryid == subnetworkBoundaryId
                    && x.Platformid == platformId
                ).Include(x => x.Productname).Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .AsNoTracking()
                .FirstOrDefaultAsync());

        }




        private async Task<long> GenerateDesignComponentFamily(DesignComponentDtoCreate dto, long subnetworkBoundryId)
        {
            var systemType = await _repositoryWrapper.SystemType
                .FindByCondition(x => x.Systemtypeid == dto.SystemTypeId)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Include(x => x.Majorsoftwarebuilds)
                .AsNoTracking().SingleAsync();

            var hardware = systemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
               ?.Majorhardware;

            var plid = hardware?.Platformid ?? 0;

            var platform = (await _repositoryWrapper.Platform.FindByCondition(x =>
                x.Platformid == plid, true, false).SingleOrDefaultAsync())?.Platform ?? "";

            var majorHW = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == hardware.Majorhardwareid, false)
               .Include(x => x.Buildconstruction).SingleOrDefault();

            var hardwareSolution = majorHW.Hardwaresolution;
            var buildConstructionRule = majorHW?.Buildconstruction?.Rule;

            var hwOemId = hardware?.Orgeqpmanufacturerid;
            var ProductName = systemType.Majorsoftwarebuilds.Productname != null ? systemType.Majorsoftwarebuilds.Productname.Description : "";

            var subnetworkBoundaryAlias = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(x => x.Id == subnetworkBoundryId).Select(y => y.Alias ?? y.Description).FirstOrDefault();
            var result = await _designComponentFamilyManager.Add(new DesignComponentFamilyDtoCreate
            {
                SubNetworkBoundaryId = subnetworkBoundryId,
                Description = subnetworkBoundaryAlias,
                //GdprRelevant = dto.GdprRelevant,
                SystemTypeIdentityName = await Utils.STIM(_repositoryWrapper, hwOemId,
                systemType.Majorsoftwarebuilds.Orgeqpmanufacturerid,
                ProductName, platform, hardwareSolution, buildConstructionRule),
                MajorHardwareOemId = hwOemId,
                MajorSoftwareOemId = systemType.Majorsoftwarebuilds.Orgeqpmanufacturerid,
                ProductName = systemType.Majorsoftwarebuilds.Productname != null ?
                systemType.Majorsoftwarebuilds.Productname.Description : "",
                PlatformId = systemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                    ?.Majorhardware.Platformid,
                Implementation = false,
                ProductNameId = systemType.Majorsoftwarebuilds.Productname != null ? systemType.Majorsoftwarebuilds.Productnameid : null
            });
            return result.Warning ? throw new Exception() : result.Data;
        }

        public async Task<DesignComponent> EntityExists(long systemTypeId, long? designComponentFamilyId, long subNetworkBoundaryid)
        {
            var entityExists = DesignComponentMapper.GetDesignComponentMapper(await _repositoryWrapper.DesignComponent.FindByCondition(
                    x => x.Systemtypeid == systemTypeId &&
                         x.Subnetworkboundaryid == subNetworkBoundaryid
                   
                ).OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync());
            return entityExists;
        }

        public async Task<DesignComponent> EntityExists(long systemTypeId, long? designComponentFamilyId, long subNetworkBoundaryid, long designComponentId)
        {
            var entityExists = DesignComponentMapper.GetDesignComponentMapper(await _repositoryWrapper.DesignComponent.FindByCondition(
                    x => x.Systemtypeid == systemTypeId
                         && x.Subnetworkboundaryid == subNetworkBoundaryid
                         && x.Designcomponentid != designComponentId
                    ).OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync());
            return entityExists;
        }

        public async Task<ResultDto> Update(DesignComponentDtoUpdate dto, bool? forced = false)
        {
            dto.SubNetworkBoundaryIds = dto.SupportedAllServices ? AddDCDefaultSubnetworkBoundry(dto.SystemTypeId) : dto.SubNetworkBoundaryIds;

            dto.DesignComponentFamilyId = await GetDesigComponentFamilyId(dto.SystemTypeId, dto.SubNetworkBoundaryIds.FirstOrDefault());
            var anotherEntityWithSameNaturalKeyExists = await EntityExists(dto.SystemTypeId, dto.DesignComponentFamilyId, dto.SubNetworkBoundaryIds.FirstOrDefault(), dto.DesignComponentId);
            var originalEntityWithSameNaturalKey = await _repositoryWrapper.DesignComponent.FindByCondition(
                x => x.Systemtypeid == dto.SystemTypeId
                     && x.Designcomponentfamilyid == dto.DesignComponentFamilyId
                     && x.Designcomponentid == dto.DesignComponentId, true).SingleOrDefaultAsync();
            if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
            {
                var relations = _repositoryWrapper.Lcmengineering
                    .FindByCondition(
                        x => x.Designcomponentid == anotherEntityWithSameNaturalKeyExists.DesignComponentId, true)
                    .FirstOrDefault();
                if (relations == null && anotherEntityWithSameNaturalKeyExists.Deleted)
                {
                    if (forced == true)
                    {
                        var dcfCreated = await DesignComponentFamilyHandlerForDesignComponent(dto, dto.SubNetworkBoundaryIds.FirstOrDefault(), dto.DesignComponentId);
                        dto.DesignComponentFamilyId = dcfCreated.DCFId;
                        var entityForced = _mapper.Map<DesignComponent>(dto);
                        entityForced.Deleted = false;
                        entityForced.DeletionDate = null;
                        _repositoryWrapper.DesignComponent.Update(DesignComponentMapper.SetDesignComponentMapper(entityForced));
                        await _repositoryWrapper.SaveAsync();
                        await CalculateLcmValueFromDesignComponentId(entityForced.DesignComponentId);
                        if (dcfCreated.IsNewDCF)
                        {
                            var currentDCF = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == dto.DesignComponentFamilyId).SingleOrDefault();
                            currentDCF.Systemisshared = _designComponentFamilyManager.IsSystemShared(currentDCF.Designcomponentfamilyid);
                            _repositoryWrapper.DesignComponentFamily.Update(currentDCF);
                            await _repositoryWrapper.SaveAsync();

                        }
                        return new ResultDto { Info = ResultMessages.EntryUpdateSuccess };
                    }

                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryUpdateExists,
                        Data = new { id = anotherEntityWithSameNaturalKeyExists.DesignComponentId, orphanDeleted = true }
                    };
                }

                return new ResultDto
                {
                    Warning = true,
                    Info = anotherEntityWithSameNaturalKeyExists.Deleted
                        ? ResultMessages.EntryUpdateExistsDeleted
                        : ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.DesignComponentId
                };
            }
            var result = await DesignComponentFamilyHandlerForDesignComponent(dto, dto.SubNetworkBoundaryIds.FirstOrDefault(), dto.DesignComponentId);
            dto.DesignComponentFamilyId = result.DCFId;
            var entity = _mapper.Map<DesignComponent>(dto);
            if (forced == true)
            {
                entity.Deleted = false;
                entity.DeletionDate = null;
            }

            var model = DesignComponentMapper.SetDesignComponentMapper(entity);
            await _repositoryWrapper.ClearTracker();

            model.Subnetworkboundaryid = dto.SubNetworkBoundaryIds.FirstOrDefault();


            _repositoryWrapper.DesignComponent.Update(model);

            await _repositoryWrapper.SaveAsync();
            await _repositoryWrapper.ClearTracker();

            await CalculateLcmValueFromDesignComponentId(entity.DesignComponentId);

            if (result.IsNewDCF)
            {
                var currentDCF = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == dto.DesignComponentFamilyId).SingleOrDefault();
                currentDCF.Systemisshared = _designComponentFamilyManager.IsSystemShared(currentDCF.Designcomponentfamilyid);
                _repositoryWrapper.DesignComponentFamily.Update(currentDCF);
                await _repositoryWrapper.SaveAsync();

            }
            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess
            };
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id)
                .SingleAsync();
            _repositoryWrapper.DesignComponent.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Designcomponentid
            };
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            #region  //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
            var LcmengineeringRecord = _repositoryWrapper.Lcmengineering.FindByConditionWithDelete(x => x.Designcomponentid == id).FirstOrDefault();

            var plannedActivityRecord = _repositoryWrapper.PlannedActivity.FindByConditionWithDelete(x => x.Designcomponentid == id).FirstOrDefault();

            short assetDeploymentRemovedStatusId = (short)LCMEngineeringRulesExtension.GetAssetDeploymentStatusBasedOnParameter(
                    "removed", _repositoryWrapper)?
                    .FirstOrDefault().Key;

            var NetworkElementAsPlanned = _repositoryWrapper.NetworkElementAsPlanned
                .FindByConditionWithDelete(x => x.Designcomponentid == id && x.Deploymentstatusid == assetDeploymentRemovedStatusId)
                 .FirstOrDefault();

            var dcfEntry = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindByConditionWithDelete(x => x.Dcid == id).FirstOrDefault();
            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries

            //if (LcmengineeringRecord != null || plannedActivityRecord != null || NetworkElementAsPlanned != null || dcfEntry != null)             
            //     await Delete(id);
            // else
            //{
            var entity = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id, true)
               .FirstOrDefaultAsync();
            if (entity != null)
            {
                var dcWithSameDfc = await _repositoryWrapper.DesignComponent
                                    .FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid && x.Designcomponentid != entity.Designcomponentid).ToListAsync();

                var dcf = await _repositoryWrapper.DesignComponentFamily
                    .FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid)
                    .FirstOrDefaultAsync();

                _repositoryWrapper.DesignComponent.Delete(entity);
                await _repositoryWrapper.SaveAsync();

                if (dcWithSameDfc == null || !dcWithSameDfc.Any())
                {
                    var designAspects = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Designcomponentfamilyid == dcf.Designcomponentfamilyid);
                    foreach (var item in designAspects)
                    {
                        _repositoryWrapper.DesignAspectRepository.Delete(item);

                    }
                    _repositoryWrapper.DesignComponentFamily.Delete(dcf);
                    await _repositoryWrapper.SaveAsync();
                }


            }

            //}            

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = id
            };
            #endregion
        }
        public async Task<ResultDto> DeleteDeep_Existing(long id)
        {

            var entity = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id)
                .SingleAsync();
            var dcWithSameDfc = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid && x.Designcomponentid != entity.Designcomponentid).ToListAsync();

            var dcf = await _repositoryWrapper.DesignComponentFamily
                .FindByCondition(x => x.Designcomponentfamilyid == entity.Designcomponentfamilyid)
                .SingleOrDefaultAsync();

            _repositoryWrapper.DesignComponent.DeleteDeep(entity);
            await _repositoryWrapper.SaveAsync();

            if (dcWithSameDfc == null || !dcWithSameDfc.Any())
            {
                var designAspects = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Designcomponentfamilyid == dcf.Designcomponentfamilyid);
                foreach (var item in designAspects)
                {
                    _repositoryWrapper.DesignAspectRepository.DeleteDeep(item);

                }
                _repositoryWrapper.DesignComponentFamily.DeleteDeep(dcf);
                await _repositoryWrapper.SaveAsync();

            }
            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Designcomponentid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            var rm = new List<ResultMessageDto>(); var Lcmengineering = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == id && x.Archived == false)
                .Include(x => x.Opco)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .Select(x => x.toDescription(_repositoryWrapper)).ToArray();

            var plannedActivity = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Designcomponentid == id && x.Archived == false)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Activitystatus)
                .Include(x => x.Deliverystatus)
                .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                .ToArray();

            short assetDeploymentRemovedStatusId = (short)LCMEngineeringRulesExtension.GetAssetDeploymentStatusBasedOnParameter(
                    "removed", _repositoryWrapper)?
                    .FirstOrDefault().Key;
            var NetworkElementAsPlanned = _repositoryWrapper.NetworkElementAsPlanned
                .FindByCondition(x => x.Designcomponentid == id && x.Deploymentstatusid != assetDeploymentRemovedStatusId)
                .Select(x => NetworkElementAsPlannedMapper.Get(x, true).toDescription())
                .ToArray();



            if (Lcmengineering.Length > 0)
                rm.Add(new ResultMessageDto { Table = "LCM Engineering", Values = Lcmengineering });
            if (plannedActivity.Length > 0)
                rm.Add(new ResultMessageDto { Table = "Planned Activity", Values = plannedActivity });
            if (NetworkElementAsPlanned.Length > 0)
                rm.Add(new ResultMessageDto { Table = "Network Element As Planned", Values = NetworkElementAsPlanned });

            var entity = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == id)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                 .Include(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Select(x => x.toDesignComponentNameLcm(_repositoryWrapper)).SingleAsync();

            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            List<string> removeDuplicatesAndLinkedTableCheck = new List<string>()
            {
                "Lcmengineering","Plannedactivitytypes","Networkelementsasplanned","Plannedactivities"
            };
            var referenceTableRecord = _commonManager.GetForeignKeyRefernceTable("Designcomponents", id, removeDuplicatesAndLinkedTableCheck);

            var daEntity = Task.Run(() => _repositoryWrapper.DesignAspectRepository
                                        .FindByCondition(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Designcomponentid == id) && x.Archived == false)
                                        .Select(x => x.Id.ToString()).ToArray());

            if (daEntity != null && daEntity.Result.Count() > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Design Aspects", Values = daEntity.Result});
            }

            if (referenceTableRecord.Result != null && referenceTableRecord.Result.Count() > 0)
                rm.Add(new ResultMessageDto() { Table = _commonManager.popupTabName, Values = referenceTableRecord.Result.ToArray() });


            if (rm.Count > 0)
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto
                    {
                        EntityName = "Design Component",
                        RecordName = entity,
                        DataRelatedList = rm
                    }
                };
            return new ResultDto();
        }

        public async Task<DesignComponentDtoCreate> GetCreatePage()
        {
            return await _dropdownDataServiceManager.GetSystemType() ;
           
        }

        public async Task<DesignComponentDtoUpdate> GetUpdatePage(long id)
        {
            var entity = _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == id, true)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Subnetworkboundary)
                .Include(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Single();
            var dto = _mapper.Map<DesignComponentDtoUpdate>(DesignComponentMapper.GetDesignComponentMapper(entity));
            dto.GdprRelevant = entity.Designcomponentfamily != null ? entity.Subnetworkboundary.Gdprrelevant : null;
            dto.SupportedAllServices = (entity.Subnetworkboundary.Default.Value == true) ? true : false;
            dto.SystemTypeResource =
                (await SystemTypesFromFamily(entity.Systemtypeid))
                .Select(x => new DictionaryList { Key = x.Systemtypeid, Value = x.toSystemTypeName(_repositoryWrapper) }).ToList();
            //.ToDictionary(x => x.Systemtypeid,
            //    x => x.toSystemTypeName(_repositoryWrapper));
            dto.SubNetworkBoundaryIds = new List<long>() { entity.Subnetworkboundaryid };

            //    dto.SubNetworkBoundaryResource = GetServicesBySWAppName(entity.Systemtypeid);
            var supportedServiceResources = _repositoryWrapper.SubnetworkSupportedServiceRepository.FindByCondition(p => p.Subnetworkid == entity.Subnetworkboundaryid).Include(p => p.Service).Select(p => p.Service).ToList();

            dto.SubNetworkSupportedServices = supportedServiceResources.ToDictionary(x => x.Id, y => y.Description);


            return dto;
        }

        public QueryResultDto<DesignComponentDtoGrid> FindWithCondition(
            DesignComponentQueryDto designComponentFilterDto)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            //Remove Release Details Unknown records
            predicateResult = predicateResult.And(x => x.Systemtype.Majorsoftwarebuilds.Softwareversion.ToLower() != ConstantValueFilter.Unknown);
            if (designComponentFilterDto.Deleted == true) predicateResult = predicateResult.And(x => x.Deleted.Value);
            if (designComponentFilterDto.Orphan == true)
                predicateResult = predicateResult.And(x => !x.Lcmengineering.Any() && !x.Plannedactivities.Any() && !x.Networkelementsasplanned.Any());

            var data = PrepareQuery(predicateResult, designComponentFilterDto.Deleted ?? false)
                      .ApplyOrdering(designComponentFilterDto, GetColumnsMap(), "Modificationdate")
                      .ApplyPaging(designComponentFilterDto).ToList();

            var rtn = new QueryResultDto<DesignComponentDtoGrid>(
             new GenerateRenderForGrid<DesignComponentDtoGrid>(manager))
            {
                TotalItems = _repositoryWrapper.DesignComponent.Count(predicateResult)
                //TotalItems = data.Count()
            };

            if (designComponentFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.Designcomponentid == designComponentFilterDto.PrincipalId);
                if (!exist)
                {

                    var addedResource = _repositoryWrapper.DesignComponent.FindAll(true)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                          //.Include(x => x.Systemtype).ThenInclude(x => x.Verticalresponsible)
                        .Include(x => x.Subnetworkboundary)
                        .Include(x => x.Lcmengineering)
                        .Include(x => x.ModificationuserNavigation)
                        .Single(x => x.Designcomponentid == designComponentFilterDto.PrincipalId);

                    if (designComponentFilterDto.SystemTypeIdBasedVerticalId != null
                        && designComponentFilterDto.SystemTypeIdBasedVerticalId.Count > 0 && addedResource != null)

                    {
                        //if (designComponentFilterDto.SystemTypeIdBasedVerticalId.
                        //    Contains((int)addedResource.Systemtype.Verticalresponsibleid))
                            data.Add(addedResource);

                    }
                }
            }

            var designComponentResult = _mapper.Map<IEnumerable<DesignComponentDtoGrid>>(data);
            rtn.Items = designComponentResult.ToArray();
            return rtn;
        }

        private IQueryable<Designcomponents> PrepareQuery(ExpressionStarter<Designcomponents> predicateResult,
            bool includeDeleted)
        {
            var result = _repositoryWrapper.DesignComponent.FindByCondition(predicateResult, includeDeleted)
                        .Include(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                         //.Include(x => x.Systemtype).ThenInclude(x => x.Verticalresponsible)
                        .Include(x => x.Subnetworkboundary)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Productname)
                        .Include(x => x.ModificationuserNavigation)
                        .Include(x => x.Lcmengineering)
                        .Include(x => x.Networkelementsasplanned)
                        .Include(x => x.Plannedactivities)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary);
            //Ticket 603 - #503 :  Analysis - Software Upgrade Utility


            return result.OrderByDescending(p => p.Modificationdate).AsQueryable();
        }

        public async Task<ResultDto> Restore(long id)
        {
            var entity = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == id, true)
                .SingleAsync();
            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.DesignComponent
                .FindByCondition(
                    x =>
                        x.Systemtypeid == entity.Systemtypeid &&
                        x.Designcomponentfamilyid == entity.Designcomponentfamilyid &&
                        x.Designcomponentid != entity.Designcomponentid
                ).FirstOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null)
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.Designcomponentid
                };
            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.DesignComponent.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Designcomponentid
            };
        }

        private static ExpressionStarter<Designcomponents> ApplyFilter(DesignComponentQueryDto buildFilterDto)
        {

            var predicateResult = PredicateBuilder.New<Designcomponents>(true);

            var predicateInner = PredicateBuilder.New<Designcomponents>(true);

            #region //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
            predicateInner = PredicateBuilder.New<Designcomponents>();
            predicateInner.Or(x => x.Visibleflag == true);
            predicateResult.And(predicateInner);
            #endregion

            if (buildFilterDto.DesignComponentFamily != null && buildFilterDto.DesignComponentFamily.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.DesignComponentFamily)
                    predicateInner.Or(x => x.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SubNetworkBoundary != null && buildFilterDto.SubNetworkBoundary.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.SubNetworkBoundary)
                    predicateInner.Or(x => (string.IsNullOrEmpty(x.Subnetworkboundary.Alias) ? x.Subnetworkboundary.Description : x.Subnetworkboundary.Alias) == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignComponentFamilyId != null && buildFilterDto.DesignComponentFamilyId.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.DesignComponentFamilyId)
                    predicateInner.Or(x => x.Designcomponentfamilyid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VodafoneName != null && buildFilterDto.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.VodafoneName)
                    predicateInner.Or(x => x.Systemtype.VodafonenameNavigation != null
                    && x.Systemtype.VodafonenameNavigation.Id == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SystemTypeId != null && buildFilterDto.SystemTypeId.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.SystemTypeId)
                    predicateInner.Or(x => x.Systemtypeid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignComponentId != null && buildFilterDto.DesignComponentId.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.DesignComponentId)
                    predicateInner.Or(x => x.Designcomponentid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.EquipmentManufacturer != null && buildFilterDto.EquipmentManufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.EquipmentManufacturer)
                    predicateInner.Or(x =>
                        x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer
                            .Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ProductName != null && buildFilterDto.ProductName.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.ProductName)
                    predicateInner.Or(x => x.Systemtype.Majorsoftwarebuilds.Productnameid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SoftwareVersion != null && buildFilterDto.SoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.SoftwareVersion)
                    predicateInner.Or(x => x.Systemtype.Majorsoftwarebuilds.Softwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponent != null && buildFilterDto.DesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.DesignComponent)
                    predicateInner.Or(x => x.Designcomponentid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwarePlatform != null && buildFilterDto.HardwarePlatform.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.HardwarePlatform)
                    predicateInner.Or(x =>
                        x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault().Majorhardware.Platformid == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.HardwareSolution != null && buildFilterDto.HardwareSolution.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.HardwareSolution)
                    predicateInner.Or(x =>
                        x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault().Majorhardware.Hardwaresolution ==
                        item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareType != null && buildFilterDto.HardwareType.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.HardwareType)
                    predicateInner.Or(x =>
                        x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault().Majorhardware.Hardwaretype ==
                        item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignContact != null && buildFilterDto.DesignContact.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.DesignContact)
                    if(item == "yes")
                    {
                        predicateInner.Or(x => !x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any(x => x.Designcontactid.ToString() == item));
                    }
                    
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemTypeIdBasedVerticalValue != null && buildFilterDto.SystemTypeIdBasedVerticalValue.Any())
            {
                predicateInner = PredicateBuilder.New<Designcomponents>();
                foreach (var item in buildFilterDto.SystemTypeIdBasedVerticalValue)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => !x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any(r => r.Designcontact.AspnetuserverticalsUser.Any(e => e.Organisation.Vertical.Verticalresponsibleid.ToString() == item)));
                    }
               
                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        private Dictionary<string, Expression<Func<Designcomponents, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Designcomponents, object>>[]>
            {
                ["designComponentId"] = new Expression<Func<Designcomponents, object>>[] { p => p.Designcomponentid },
                ["designComponent"] = new Expression<Func<Designcomponents, object>>[] { p => p.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["systemTypeId"] = new Expression<Func<Designcomponents, object>>[] { p => p.Systemtypeid },
                ["designComponentFamily"] = new Expression<Func<Designcomponents, object>>[]
                    {p => p.Designcomponentfamily.Systemtypeidentityname},
                ["designComponentFamilyId"] = new Expression<Func<Designcomponents, object>>[]
                    {p => p.Designcomponentfamilyid},
                ["equipmentManufacturer"] = new Expression<Func<Designcomponents, object>>[]
                {
                    p => p.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer
                        .Originalequipmentmanufacturer
                },
                ["productName"] = new Expression<Func<Designcomponents, object>>[]
                    {p => p.Systemtype.Majorsoftwarebuilds.Productname != null ? p.Systemtype.Majorsoftwarebuilds.Productname.Description:""},
                ["vodafoneName"] = new Expression<Func<Designcomponents, object>>[] { p => p.Systemtype.Vodafonename },
                ["softwareVersion"] = new Expression<Func<Designcomponents, object>>[]
                    {p => p.Systemtype.Majorsoftwarebuilds.Softwareversion},
                ["hardwarePlatform"] = new Expression<Func<Designcomponents, object>>[]
                {
                    p => p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware
                        .Platform.Platform
                },
                ["hardwareType"] = new Expression<Func<Designcomponents, object>>[]
                {
                    p => p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware
                        .Hardwaretype
                },
                ["hardwareSolution"] = new Expression<Func<Designcomponents, object>>[]
                {
                    p => p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware
                        .Hardwaresolution
                },
                ["systemType"] = new Expression<Func<Designcomponents, object>>[]
                {
                    p => p.Systemtype.Systemtypenameoem,
                    p => p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware
                        .Platform.Platform,
                    p => p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware
                },
                ["subNetworkBoundary"] = new Expression<Func<Designcomponents, object>>[] { p => !string.IsNullOrEmpty(p.Subnetworkboundary.Alias) ? p.Subnetworkboundary.Alias : p.Subnetworkboundary.Description },
                ["lastModifiedValue"] = new Expression<Func<Designcomponents, object>>[] { p => p.Modificationdate },
                ["lastModifiedBy"] = new Expression<Func<Designcomponents, object>>[]
                    {p => p.ModificationuserNavigation.Email},
                ["systemTypeId"] = new Expression<Func<Designcomponents, object>>[] { p => p.Systemtypeid },
                //["systemTypeIdBasedVerticalId"] = new Expression<Func<Designcomponents, object>>[]
                //{ p => p.Systemtype.Verticalresponsible != null ?
                //p.Systemtype.Verticalresponsible.Verticalresponsibleid : default },

                //["systemTypeIdBasedVerticalValue"] = new Expression<Func<Designcomponents, object>>[]
                // { p => p.Systemtype.Verticalresponsible != null &&
                //p.Systemtype.Verticalresponsible.Verticalresponsible != null ?
                //p.Systemtype.Verticalresponsible.Verticalresponsible : default }
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter,
            DesignComponentQueryDto designComponentFilterDto,bool isAdmin)
        {
            var predicateResult = ApplyFilter(designComponentFilterDto);
            //Remove Release Details Unknown records
            predicateResult = predicateResult.And(x => x.Systemtype.Majorsoftwarebuilds.Softwareversion.ToLower() != ConstantValueFilter.Unknown);

            var query = PrepareQuery(predicateResult, false);//.AsEnumerable().Select(p => DesignComponentMapper.GetDesignComponentMapper(p)).AsQueryable();
            ;

            var rtn = propertyName switch
            {
                "designComponentId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Designcomponentid.ToString(), Value = p.Designcomponentid.ToString() }).Distinct()
                        .ToList()
                    : query
                        .Where(x => x.Designcomponentid.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto
                            { Text = p.Designcomponentid.ToString(), Value = p.Designcomponentid.ToString() })
                        .Distinct()
                        .ToList(),
                "systemTypeId" => string.IsNullOrEmpty(propertyFilter)
                              ? query.Select(p => new FilterValueDto
                              { Text = p.Systemtypeid.ToString(), Value = p.Systemtypeid.ToString() }).Distinct()
                                  .ToList()
                              : query
                                  .Where(x => x.Systemtypeid.ToString().Contains(propertyFilter)).Select(p =>
                                      new FilterValueDto
                                      { Text = p.Systemtypeid.ToString(), Value = p.Systemtypeid.ToString() })
                                  .Distinct()
                                  .ToList(),
                "designComponent" => /*string.IsNullOrEmpty(propertyFilter)
                    ?*/ query.ToList().Select(p => new FilterValueDto
                        {
                            Text = p.toDesignComponentNameLcm(_repositoryWrapper),
                            Value = p.Designcomponentid.ToString()
                        }).Distinct().ToList(),

                //"designComponent" => string.IsNullOrEmpty(propertyFilter)
                //    ? query.ToList().Select(p => new FilterValueDto
                //    {
                //        Text = p.toDesignComponentNameLcm(_repositoryWrapper),
                //        Value = p.Designcomponentid.ToString()
                //    }).Distinct().ToList()
                //    : query.ToList()
                //        .Where(x =>
                //            x.toDesignComponentNameLcm(_repositoryWrapper).ToUpper().Contains(
                //                propertyFilter.ToUpper())).Select(p => new FilterValueDto
                //                {
                //                    Text = p.toDesignComponentNameLcm(_repositoryWrapper),
                //                    Value = p.Designcomponentid.ToString()
                //                }).Distinct().ToList(),

                "subNetworkBoundary" => string.IsNullOrEmpty(propertyFilter) ? query.Select(x => new FilterValueDto(string.IsNullOrEmpty(x.Subnetworkboundary.Alias) ? x.Subnetworkboundary.Description : x.Subnetworkboundary.Alias)).Distinct().ToList()
                   : query.Where(x => string.IsNullOrEmpty(x.Subnetworkboundary.Alias) ? x.Subnetworkboundary.Description.Contains(propertyFilter) : x.Subnetworkboundary.Alias.Contains(propertyFilter))
                   .Select(x => new FilterValueDto(string.IsNullOrEmpty(x.Subnetworkboundary.Alias) ? x.Subnetworkboundary.Description : x.Subnetworkboundary.Alias)).Distinct().ToList(),


                "vodafoneName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Systemtype.VodafonenameNavigation != null).Select(p => new FilterValueDto(p.Systemtype.VodafonenameNavigation.Id, p.Systemtype.VodafonenameNavigation.Description))
                    .Distinct()
                        .ToList()
                    : query.Where(x => x.Systemtype.VodafonenameNavigation != null && x.Systemtype.VodafonenameNavigation.Description.Contains(propertyFilter)).Select(p =>
                         new FilterValueDto(p.Systemtype.VodafonenameNavigation.Id, p.Systemtype.VodafonenameNavigation.Description))
                        .Distinct()
                        .ToList(),
                "equipmentManufacturer" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer
                            .Originalequipmentmanufacturer.ToString(),
                        Value = p.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer
                            .Originalequipmentmanufacturer.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto
                            {
                                Text = p.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer
                                    .Originalequipmentmanufacturer.ToString(),
                                Value = p.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid.ToString()
                            }).Distinct()
                        .ToList(),
                "productName" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                    .Where(x => x.Systemtype.Majorsoftwarebuilds.Productname != null).Select(p => new FilterValueDto
                    {
                        Text = p.Systemtype.Majorsoftwarebuilds.Productname != null ? p.Systemtype.Majorsoftwarebuilds.Productname.Description : "",
                        Value = p.Systemtype.Majorsoftwarebuilds.Productname != null ? p.Systemtype.Majorsoftwarebuilds.Productname.Productnameid.ToString() : ""
                    }).Distinct().ToList()
                    : query
                        .Where(x => x.Systemtype.Majorsoftwarebuilds.Productname != null
                        && x.Systemtype.Majorsoftwarebuilds.Productname.Productnameid.ToString().Contains(propertyFilter))
                        .Select(p =>
                            new FilterValueDto
                            {
                                Text = p.Systemtype.Majorsoftwarebuilds.Productname != null ? p.Systemtype.Majorsoftwarebuilds.Productname.Description : "",
                                Value = p.Systemtype.Majorsoftwarebuilds.Productname != null ? p.Systemtype.Majorsoftwarebuilds.Productname.Productnameid.ToString() : ""
                            }).Distinct()
                        .ToList(),
                "softwareVersion" => query.Select(p => new FilterValueDto
                {
                    Text = p.Systemtype.Majorsoftwarebuilds.Softwareversion,
                    Value = p.Systemtype.Majorsoftwarebuilds.Softwareversion
                }).Distinct().ToList(),
                "hardwarePlatform" => string.IsNullOrEmpty(propertyFilter)
                    ? query.AsEnumerable().Select(p => new FilterValueDto
                    {
                        Text = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)?.Majorhardware
                            ?.Platform?.Platform.ToString(),
                        Value = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)?.Majorhardware
                            ?.Platformid.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x => x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                            .Majorhardware.Platform.Platform.Contains(propertyFilter)).AsEnumerable().Select(p =>
                            new FilterValueDto
                            {
                                Text = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                                    ?.Majorhardware?.Platform?.Platform.ToString(),
                                Value = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                                    ?.Majorhardware?.Platformid.ToString()
                            }).Distinct().ToList()
                        .ToList(),
                "hardwareSolution" => query.Select(p => new FilterValueDto(p.Systemtype.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain).Select(x => x.Majorhardware.Hardwaresolution).FirstOrDefault())).Distinct().ToList(),
                //"hardwareSolution" => string.IsNullOrEmpty(propertyFilter)
                //  ? query.AsEnumerable().Select(p => new FilterValueDto
                //  {
                //      Text = p.Systemtype.Systemtypesmajorhardwarebuilds.Where(x => x.Ismain).Select(x => x.Majorhardware.Hardwaresolution).FirstOrDefault(),
                //      Value = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)?.Majorhardware
                //          ?.Hardwaresolution?.ToString(),
                //  }).Distinct().ToList()
                //  : query
                //      .Where(x => x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                //          .Majorhardware.Hardwaresolution.Contains(propertyFilter)).AsEnumerable().Select(p =>
                //          new FilterValueDto
                //          {
                //              Text = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                //                  ?.Majorhardware?.Hardwaresolution?.ToString(),
                //              Value = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                //                  ?.Majorhardware?.Hardwaresolution?.ToString(),
                //          }).Distinct().ToList()
                //      .ToList(),
                "hardwareType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.AsEnumerable().Select(p => new FilterValueDto
                    {
                        Text = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)?.Majorhardware
                            ?.Hardwaretype?.ToString(),
                        Value = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)?.Majorhardware
                            ?.Hardwaretype?.ToString(),
                    }).Distinct().ToList()
                    : query
                        .Where(x => x.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                            .Majorhardware.Hardwaretype.Contains(propertyFilter)).AsEnumerable().Select(p =>
                            new FilterValueDto
                            {
                                Text = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                                    ?.Majorhardware?.Hardwaretype?.ToString(),
                                Value = p.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                                    ?.Majorhardware?.Hardwaretype?.ToString(),
                            }).Distinct().ToList()
                        .ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationuserNavigation.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationuserNavigation.Email))
                        .Distinct().ToList(),

                "designComponentFamily" =>
                    string.IsNullOrEmpty(propertyFilter)
                        ? query
                            .Select(p => new FilterValueDto
                            {
                                Text = p.Designcomponentfamily.Systemtypeidentityname,
                                Value = p.Designcomponentfamilyid.ToString()
                            })
                            .Distinct().ToList()
                        : query
                            .Where(x => x.Designcomponentfamily.Systemtypeidentityname.ToUpper()
                                .Contains(propertyFilter.ToUpper()))
                            .Select(p => new FilterValueDto
                            {
                                Text = p.Designcomponentfamily.Systemtypeidentityname,
                                Value = p.Designcomponentfamilyid.ToString()
                            })
                            .Distinct().ToList(),
                "designComponentFamilyId" =>
                    string.IsNullOrEmpty(propertyFilter)
                        ? query
                            .Select(p => new FilterValueDto
                            {
                                Text = p.Designcomponentfamilyid.ToString(),
                                Value = p.Designcomponentfamilyid.ToString()
                            })
                            .Distinct().ToList()
                        : query
                            .Where(x => x.Designcomponentfamilyid.ToString().ToUpper()
                                .Contains(propertyFilter.ToUpper()))
                            .Select(p => new FilterValueDto
                            {
                                Text = p.Designcomponentfamilyid.ToString(),
                                Value = p.Designcomponentfamilyid.ToString()
                            })
                            .Distinct().ToList(),

                "systemType" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer).ToList()
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Systemtype.toSystemTypeName(_repositoryWrapper),
                            Value = p.Systemtypeid.ToString()
                        }).Distinct().ToList()
                    : query
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware)
                        .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer).ToList()
                        .Where(x => x.Systemtype.toSystemTypeName(_repositoryWrapper).ToUpper()
                            .Contains(propertyFilter.ToUpper()))
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Systemtype.toSystemTypeName(_repositoryWrapper),
                            Value = p.Systemtypeid.ToString()
                        }).Distinct().ToList(),

                "designContact" =>
                    query.SelectMany(x => x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Select(x=>x.Designcontact)).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Email,
                        Value = p.Id.ToString()
                    }).Distinct().ToList()
                    .Concat(query.Where(x => x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),
                "systemTypeIdBasedVerticalValue" => 
                    query.SelectMany(x=>x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.SelectMany(x=>x.Designcontact.AspnetuserverticalsUser.Select(x=>x.Organisation.Vertical))).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Verticalresponsible,
                        Value = p.Verticalresponsibleid.ToString()
                    }).Distinct().ToList()
                    .Concat(query.Where(x => x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && x.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),
                _ => new List<FilterValueDto>()
            };
            if (!isAdmin && (designComponentFilterDto.SystemTypeIdBasedVerticalValue != null && designComponentFilterDto.SystemTypeIdBasedVerticalValue.Count > 0) && propertyName == "systemTypeIdBasedVerticalValue")
            {
                rtn = rtn.Where(x => designComponentFilterDto.SystemTypeIdBasedVerticalValue.Contains(x.Value.ToString())).ToList();
            }
            return rtn;
        }

        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation(DataRemediationDto data)
        {
            var lcmIDs = new List<long>();
            var existsRelationWithLcm = _repositoryWrapper.Lcmengineering
                .FindByCondition(x => x.Designcomponentid == data.CorrectId, true).ToList();

            foreach (var item in data.DuplicatesId)
            {
                //Get delle relazioni con lcm
                var lcmWithDuplicates = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Designcomponentid == item, true, false).ToList();
                lcmIDs.AddRange(lcmWithDuplicates.Select(x => x.Lcmengineeringid));
                foreach (var st in lcmWithDuplicates)
                {
                    st.Designcomponentid = data.CorrectId;
                    var entityExists = existsRelationWithLcm
                        .SingleOrDefault(str => str.Lcmengineeringid == st.Lcmengineeringid);
                    if (entityExists == null)
                    {
                        _repositoryWrapper.Lcmengineering.Update(st);
                        _repositoryWrapper.Save();
                    }
                }


                var plannedActivityWithDuplicates = _repositoryWrapper.PlannedActivity
                    .FindByCondition(x => x.Designcomponentid == item, true, false).ToList();
                foreach (var st in plannedActivityWithDuplicates)
                {
                    st.Designcomponentid = data.CorrectId;
                    _repositoryWrapper.PlannedActivity.Update(st);
                    _repositoryWrapper.Save();
                }

                //cancello i duplicati
                var dc = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == item, true)
                    .Single();
                _repositoryWrapper.DesignComponent.DeleteDeep(dc);
                _repositoryWrapper.Save();
            }

            await SetLcmValue(lcmIDs);
            return new ResultDto<ResultDataRemediationDto>
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = new ResultDataRemediationDto { Id = lcmIDs }
            };
        }

        private async Task CalculateLcmValueFromDesignComponentId(long designComponentId)
        {
            await SetLcmValue(await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == designComponentId).SelectMany(x => x.Lcmengineering)
                .Select(x => x.Lcmengineeringid).ToListAsync());
        }

        private async Task SetLcmValue(List<long> lcmIDs)
        {
            foreach (var id in lcmIDs)
            {
                var lcmList = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Lcmengineeringid == id, true, false)
                    .Include(x => x.PlannedactivitiesLcmengineering)
                    .ToList();
                foreach (var lcm in lcmList)
                {
                    await _repositoryWrapper.ClearTracker();
                    var updated = await _lcmEngineeringManager.SetLcmValue(lcm);
                    await _repositoryWrapper.ClearTracker();

                    #region // # 181 last modified issue
                    var originalLcm = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == updated.Lcmengineeringid &&
                    x.Outputtolcmsoftware == updated.Outputtolcmsoftware && x.Hardwaresupporttype == updated.Hardwaresupporttype &&
                    x.Softwareendofwarrantydate == updated.Softwareendofwarrantydate && x.Outputtolcmhardware == updated.Outputtolcmhardware &&
                    x.Softwaresupporttype == updated.Softwaresupporttype && x.Hardwaresupportprovider == updated.Hardwaresupportprovider &&
                    x.Softwaresupportprovider == updated.Softwaresupportprovider && x.Vendorendmntdatehw == updated.Vendorendmntdatehw &&
                    x.Lcmstatusopssoftware == updated.Lcmstatusopssoftware && x.Lcmstatusopshardware == updated.Lcmstatusopshardware &&
                    x.Lcmstatusengsoftware == updated.Lcmstatusengsoftware && x.Lcmstatusenghardware == updated.Lcmstatusenghardware &&
                    x.Lcmstatussoftware == updated.Lcmstatussoftware && x.Lcmstatushardware == updated.Lcmstatushardware &&
                    x.Vendorendmntedatesw == updated.Vendorendmntedatesw && x.Hardwareendofsupportcontract == updated.Hardwareendofsupportcontract
                     ).FirstOrDefault();
                    if (originalLcm == null)
                    {
                        #region //should be removed
                        //var propertiesToIgnore = new HashSet<string> { "CreateDate", "CreationUser", "ModificationDate", "ModificationUser", "Lcmancillarydata", "Lcmengineeringeduspoc", "Lcmengineeringsubdomainspoc",
                        //"Lcmoperationalcontracts","Networkelementsasplanned","PlannedactivitiesLcmengineering","PlannedactivitiesOriginallcmengineering","Reasoncheckboxresourcelcmengineeringhardware","Reasoncheckboxresourcelcmengineeringsoftware"}; 
                        //var areEqual = true; 
                        //foreach (var property in typeof(Lcmengineering).GetProperties()) 
                        //{ 
                        //    if (!propertiesToIgnore.Contains(property.Name)) 
                        //    { 
                        //        var value1 = property.GetValue(updated); 
                        //        var value2 = property.GetValue(originalLcm); 
                        //        if(value1 == null)
                        //        {
                        //            value1 = "";
                        //        }
                        //        if(value2 == null)
                        //        {
                        //            value2 = "";
                        //        }
                        //        if (!value1.Equals(value2)) 
                        //        { 
                        //            areEqual = false; break;
                        //        } 
                        //    } 
                        //} 
                        //if (!areEqual) 
                        //{
                        // } 
                        #endregion
                        _repositoryWrapper.Lcmengineering.Update(updated);
                        _repositoryWrapper.Save();

                    }
                    #endregion
                    //_repositoryWrapper.Lcmengineering.Update(updated);
                    //_repositoryWrapper.Save();
                }
            }
        }

        public async Task<ResultDto<SupportedAllServicesModel>> GetSystemAndSubNetworkBoundary(short msOem,
            int? vfNameId,
            short mhOem)
        {
            if (vfNameId != null || vfNameId == 0)
            {
                //var usedSubnetworks = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Subnetworkboundary.Vodafonenameid == vfNameId && x.Subnetworkboundary.Default != true)
                //.Include(x => x.Subnetworkboundary).Select(x => x.Subnetworkboundary);

                var unUsedsubnetworks = _repositoryWrapper.SubNetworkBoundaries.FindByCondition(p => p.Vodafonenameid == vfNameId && p.Default != true).ToList();

                var unUsedSubnetworksList = unUsedsubnetworks != null ? unUsedsubnetworks?.Select(x => new SystemAndSubNetworkBoundary
                {
                    SubNetworkBoundaryId = x.Id,
                    SubNetworkBoundaryName = !string.IsNullOrEmpty(x.Alias) ? x.Alias : x.Description,
                    SubNetworkBoundaryAlias = x.Alias,// will be deleted after migration with FE should use SubNetworkBoundaryName only
                }
                ).Distinct().ToList() : new List<SystemAndSubNetworkBoundary>();

                var hasSupportedAllServices = false;
                var vodafoneName = _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == vfNameId).FirstOrDefault().Description;

                return new ResultDto<SupportedAllServicesModel>()
                {
                    Data = new SupportedAllServicesModel(new List<SystemAndSubNetworkBoundary>(), unUsedSubnetworksList, hasSupportedAllServices, vfNameId, vodafoneName),
                };
            }

            return new ResultDto<SupportedAllServicesModel>()
            {
                Data = null
            };
        }

        public async Task<ResultDto<SupportedAllServicesModel>> GetSystemAndSubNetworkBoundary(int systemTypeId)
        {

            var systemType = _repositoryWrapper.SystemType.FindByCondition(p => p.Systemtypeid == systemTypeId)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware)
                .Include(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname).FirstOrDefault();
            if (systemType.Majorsoftwarebuilds.Productname != null)
            {
                //var productNameId = systemType.Majorsoftwarebuilds.Productname != null? systemType.Majorsoftwarebuilds.Productname.Productnameid;
                var vfNameId = systemType.Vodafonename;
                var usedSubnetworks = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Subnetworkboundary.Vodafonenameid == vfNameId && x.Systemtypeid == systemTypeId)
                    .Include(x => x.Subnetworkboundary).Select(x => x.Subnetworkboundary);

                var unUsedsubnetworks = _repositoryWrapper.SubNetworkBoundaries
                    .FindByCondition(p => p.Vodafonenameid == vfNameId && p.Default != true
                && !usedSubnetworks.Select(u => u.Id).Contains(p.Id)).ToList();

                var usedSubnetworksList = usedSubnetworks != null ? usedSubnetworks?.Select(x => new SystemAndSubNetworkBoundary
                {
                    SubNetworkBoundaryId = x.Id,
                    SubNetworkBoundaryName = !string.IsNullOrEmpty(x.Alias) ? x.Alias : x.Description,
                    SubNetworkBoundaryAlias = x.Alias,// will be deleted after migration with FE should use SubNetworkBoundaryName only
                    SystemSolutionName = systemType.toSystemTypeName(_repositoryWrapper),
                    SystemTypeId = systemType.Systemtypeid,
                }
                ).Distinct().ToList() : new List<SystemAndSubNetworkBoundary>();

                var unUsedSubnetworksList = unUsedsubnetworks != null ? unUsedsubnetworks?.Select(x => new SystemAndSubNetworkBoundary
                {
                    SubNetworkBoundaryId = x.Id,
                    SubNetworkBoundaryName = !string.IsNullOrEmpty(x.Alias) ? x.Alias : x.Description,
                    SubNetworkBoundaryAlias = x.Alias,// will be deleted after migration with FE should use SubNetworkBoundaryName only
                    SystemSolutionName = systemType.toSystemTypeName(_repositoryWrapper),
                    SystemTypeId = systemType.Systemtypeid,
                }
                ).Distinct().ToList() : new List<SystemAndSubNetworkBoundary>();

                var hasSupportedAllServices = HasSupportedAllServices(systemTypeId);
                var vodafoneName = _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == vfNameId)?.FirstOrDefault()?.Description;

                return new ResultDto<SupportedAllServicesModel>()
                {
                    Data = new SupportedAllServicesModel(usedSubnetworksList, unUsedSubnetworksList, hasSupportedAllServices, vfNameId, vodafoneName),

                };
            }

            return new ResultDto<SupportedAllServicesModel>()
            {
                Data = null

            };
        }

        private async Task<List<Systemtypes>> SystemTypesFromFamily(long systemTypeId)
        {
            var systemType = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemTypeId)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponents).ThenInclude(p => p.Subnetworkboundary)
                .Include(x => x.Designcomponents).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .SingleAsync();

            var msOem = systemType.Majorsoftwarebuilds.Orgeqpmanufacturerid;
            var mhOem = systemType.Systemtypesmajorhardwarebuilds.SingleOrDefault(s => s.Ismain).Majorhardware
                .Orgeqpmanufacturerid;
            var msst = systemType.Majorsoftwarebuilds.Productname != null ? systemType.Majorsoftwarebuilds.Productname.Description : "";

            var systemTypesFromFamily = await GetSystemTypesFromFamily(msOem, msst, mhOem);
            return systemTypesFromFamily;
        }

        private async Task<List<Systemtypes>> GetSystemTypesFromFamily(short msOem, string msst, short mhOem)
        {
            var systemTypesFromFamily = await _repositoryWrapper.SystemType.FindByCondition(x =>
                    x.Designcomponents.Any()
                    && x.Designcomponents.Any(s => s.Designcomponentfamily != null)
                    && x.Majorsoftwarebuilds.Orgeqpmanufacturerid == msOem
                    && x.Majorsoftwarebuilds.Productname != null
                    && x.Majorsoftwarebuilds.Productname.Description == msst
                    && x.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(s => s.Ismain).Majorhardware.Orgeqpmanufacturerid
                    == mhOem
                )
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponents).ThenInclude(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Subnetworkboundary).ToListAsync();
            return systemTypesFromFamily;
        }


        public async Task<ImpactSubNetworkBoundaryChanged> GetImpact(long subNetworkBoundaryId, long systemTypeId)
        {
            var systemType = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemTypeId)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .SingleAsync();
            var majorHardwareOriginalEquipmentManufacturerId = systemType.Systemtypesmajorhardwarebuilds
                .First(s => s.Ismain)
                .Majorhardware.Orgeqpmanufacturerid;
            var productName = systemType.Majorsoftwarebuilds.Productname != null ? systemType.Majorsoftwarebuilds.Productname.Description : "";
            var originalEquipmentManufacturerId = systemType.Majorsoftwarebuilds.Orgeqpmanufacturerid;


            var lcm = await _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponent.Designcomponentfamily.Productname != null &&
                    x.Designcomponent.Designcomponentfamily.Subnetworkboundaryid == subNetworkBoundaryId
                    && x.Designcomponent.Designcomponentfamily.Majorhardwareoemid == majorHardwareOriginalEquipmentManufacturerId
                    && x.Designcomponent.Designcomponentfamily.Productname.Description == productName
                    && x.Designcomponent.Designcomponentfamily.Majorsoftwareoemid == originalEquipmentManufacturerId)
                 .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Productname)
                .Include(x => x.PlannedactivitiesLcmengineering)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Responsibilityphase)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Opco)
                .Include(x => x.Productimportance)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Reasoncheckboxresourcelcmengineeringhardware).ThenInclude(x => x.Reasoncheckboxresource)
                .Include(x => x.Reasoncheckboxresourcelcmengineeringsoftware).ThenInclude(x => x.Reasoncheckboxresource)
                .ToListAsync();

            var planned = await _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Designcomponent != null && x.Designcomponent.Designcomponentfamily.Productname != null &&
                    x.Designcomponent.Designcomponentfamily.Subnetworkboundaryid == subNetworkBoundaryId
                    && x.Designcomponent.Designcomponentfamily.Majorhardwareoemid == majorHardwareOriginalEquipmentManufacturerId
                    && x.Designcomponent.Designcomponentfamily.Productname != null
                    && x.Designcomponent.Designcomponentfamily.Productname.Description == productName
                    && x.Designcomponent.Designcomponentfamily.Majorsoftwareoemid == originalEquipmentManufacturerId)
                .Include(x => x.Designcomponent)
                .ThenInclude(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Productname)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Networkelementasplanned).ThenInclude(x => x.Opco)
                .Include(x => x.Activitystatus)
                .Include(x => x.Planningactivitystatus)
                .Include(x => x.Deliverystatus)
                //.Include(x => x.RelatesTo)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Responsibilityphase)
                .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                .Include(x => x.Opco)
                .Include(x => x.Plannedactivityresource)
                .Include(x => x.Driver)
                .Include(x => x.Benefit)
                .Include(x => x.PlanningriskNavigation)
                .Include(x => x.Budgetavailability).ToListAsync();

            var networkElement = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Designcomponent.Designcomponentfamily.Productname != null &&
                    x.Designcomponent.Designcomponentfamily.Subnetworkboundaryid == subNetworkBoundaryId
                    && x.Designcomponent.Designcomponentfamily.Majorhardwareoemid == majorHardwareOriginalEquipmentManufacturerId
                    && x.Designcomponent.Designcomponentfamily.Productname != null
                    && x.Designcomponent.Designcomponentfamily.Productname.Description == productName
                    && x.Designcomponent.Designcomponentfamily.Majorsoftwareoemid == originalEquipmentManufacturerId)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Productname)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Activitystatus)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Deliverystatus)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Designcomponent)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Planningactivitystatus)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Deliverystatus)
                .Include(x => x.Plannedactivities).ThenInclude(x => x.Responsibilityphase)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .ThenInclude(x => x.Platform)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Environment)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.Deploymenttype)
                .Include(x => x.Location)
                .Include(x => x.Nfvibundleid)
                .Include(x => x.Opco)
                .Include(x => x.ModificationuserNavigation).ToListAsync();

            return new ImpactSubNetworkBoundaryChanged
            {
                EngineeringDtoGrids = _mapper.Map<List<LcmEngineeringDtoGrid>>(lcm.Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p))),
                PlannedActivityDtoGrids = _mapper.Map<List<PlannedActivityDtoGrid>>(planned.Select(p => PlannedActivityMapper.Get(p))),
                NetworkElementAsPlannedDtoGrids = _mapper.Map<List<NetworkElementAsPlannedDtoGrid>>(networkElement.Select(p => NetworkElementAsPlannedMapper.Get(p)))
            };
        }


        public async Task<bool> GetDesignComponentFamilyExist(long systemTypeId, int? subnetworkBoundryId, bool supportedAllService = false)
        {
            var systemType = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemTypeId)
                   .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                   .ThenInclude(x => x.Platform)
                   .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                   .FirstAsync();

            var subNetId = supportedAllService ? AddDCDefaultSubnetworkBoundry(systemTypeId).FirstOrDefault() : (long)subnetworkBoundryId;
            return await GetDesignComponentFamilyFromSystemType(systemType, subNetId) != null;

        }

        public async Task Rebuild()
        {
            var desigs = await _repositoryWrapper.DesignComponent.FindAll().AsNoTracking()
                .ToListAsync();
            foreach (var design in desigs.Where(x => x.Designcomponentfamilyid != null))
            {
                var dcf = await _repositoryWrapper.DesignComponentFamily
                    .FindByCondition(x => x.Designcomponentfamilyid == design.Designcomponentfamilyid)
                    .AsNoTracking().SingleOrDefaultAsync();
                await _repositoryWrapper.ClearTracker();

                var result = await DesignComponentFamilyHandlerForDesignComponent(new DesignComponentDtoUpdate()
                {
                    DesignComponentId = design.Designcomponentid,
                    SubNetworkBoundaryIds = new List<long>() { dcf.Subnetworkboundaryid },
                    GdprRelevant = dcf.Subnetworkboundary.Gdprrelevant,
                    SystemTypeId = design.Systemtypeid,
                    DesignComponentFamilyId = design.Designcomponentfamilyid,

                }, dcf.Subnetworkboundaryid, design.Designcomponentid);
                design.Designcomponentfamilyid = result.DCFId;
                await _repositoryWrapper.ClearTracker();
                _repositoryWrapper.DesignComponent.Update(design);
                if (result.IsNewDCF)
                {
                    var currentDCF = _repositoryWrapper.DesignComponentFamily.FindByCondition(p => p.Designcomponentfamilyid == result.DCFId).SingleOrDefault();
                    currentDCF.Systemisshared = _designComponentFamilyManager.IsSystemShared(currentDCF.Designcomponentfamilyid);
                    _repositoryWrapper.DesignComponentFamily.Update(currentDCF);
                    await _repositoryWrapper.SaveAsync();

                }
            }
            await _repositoryWrapper.SaveAsync();
        }

        public List<SystemAndSubNetworkBoundary> GetServicesBySWAppName(long systemTypeId)
        {
            var vfNameId = _repositoryWrapper.SystemType.FindByCondition(p => p.Systemtypeid == systemTypeId).FirstOrDefault().Vodafonename;
            if (vfNameId != null || vfNameId != 0)
            {
                var usedSubnetworks = _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Subnetworkboundary.Vodafonenameid == vfNameId && x.Systemtypeid == systemTypeId)
                .Include(x => x.Subnetworkboundary).Select(x => x.Subnetworkboundary);

                var unUsedsubnetworks = _repositoryWrapper.SubNetworkBoundaries
                  .FindByCondition(p => p.Vodafonenameid == vfNameId && p.Default != true
                && !usedSubnetworks.Select(u => u.Id).Contains(p.Id)).ToList();

                var usedSubnetworksList = usedSubnetworks != null ? usedSubnetworks?.Select(x => new SystemAndSubNetworkBoundary
                {
                    SubNetworkBoundaryId = x.Id,
                    SubNetworkBoundaryName = !string.IsNullOrEmpty(x.Alias) ? x.Alias : x.Description,
                    SubNetworkBoundaryAlias = x.Alias,// will be deleted after migration with FE should use SubNetworkBoundaryName only
                }
            ).Distinct().ToList() : new List<SystemAndSubNetworkBoundary>();

                var unUsedSubnetworksList = unUsedsubnetworks != null ? unUsedsubnetworks?.Select(x => new SystemAndSubNetworkBoundary
                {
                    SubNetworkBoundaryId = x.Id,
                    SubNetworkBoundaryName = !string.IsNullOrEmpty(x.Alias) ? x.Alias : x.Description,
                    SubNetworkBoundaryAlias = x.Alias,// will be deleted after migration with FE should use SubNetworkBoundaryName only
                }
           ).Distinct().ToList() : new List<SystemAndSubNetworkBoundary>();

                return unUsedSubnetworksList;//_repositoryWrapper.SubNetworkBoundaries.FindByCondition(p => p.Swapplicationname == majorSWAppName && (p.Default == false || p.Default == null)).ToDictionary(x => x.Id, y => string.IsNullOrEmpty(y.Alias) ? y.Description : y.Alias);
            }

            return new List<SystemAndSubNetworkBoundary>();
        }

        private new List<long> AddDCDefaultSubnetworkBoundry(long systemTypeId)
        {
            var vfNAmeId = _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemTypeId).FirstOrDefault().Vodafonename;
            var supportedAllServices = _subNetworkBoundaryManager.AddDefaultSubnetwork(vfNAmeId);

            return new List<long> { supportedAllServices.Id };
        }
        private new List<long> AddDCDefaultSubnetworkBoundry(int? VodafoneNameId)
        {
            var supportedAllServices = _subNetworkBoundaryManager.AddDefaultSubnetwork(VodafoneNameId);

            return new List<long> { supportedAllServices.Id };
        }

        public new List<SystemAndSubNetworkBoundary> GetServicesByVfName(int? vodafoneNameId)
        {
            //var usedSubnetworks = _repositoryWrapper.DesignComponent
            //    .FindByCondition(x => x.Subnetworkboundary.Vodafonenameid == vodafoneNameId && x.Subnetworkboundary.Default != true)
            //    .Include(x => x.Subnetworkboundary).Select(x => x.Subnetworkboundary);

            var unUsedsubnetworks = _repositoryWrapper.SubNetworkBoundaries
                .FindByCondition(p => p.Vodafonenameid == vodafoneNameId && p.Default != true).ToList();

            var unUsedSubnetworksList = unUsedsubnetworks != null ? unUsedsubnetworks?.Select(x => new SystemAndSubNetworkBoundary
            {
                SubNetworkBoundaryId = x.Id,
                SubNetworkBoundaryName = !string.IsNullOrEmpty(x.Alias) ? x.Alias : x.Description,
                SubNetworkBoundaryAlias = x.Alias,// will be deleted after migration with FE should use SubNetworkBoundaryName only
            }
           ).Distinct().ToList() : new List<SystemAndSubNetworkBoundary>();

            return unUsedSubnetworksList;//_repositoryWrapper.SubNetworkBoundaries.FindByCondition(p => p.Swapplicationname == majorSWAppName && (p.Default == false || p.Default == null)).ToDictionary(x => x.Id, y => string.IsNullOrEmpty(y.Alias) ? y.Description : y.Alias);
        }


        public async Task<ResultDto> ImpactedAreasOfEditDc(long designComponentId, int newSystemTypeId, int subnetworkBoundryId)
        {
            var dc = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId)
                .Include(x => x.Designcomponentfamily)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .ThenInclude(x => x.Orgeqpmanufacturer)
                .FirstOrDefault();

            var updateDCF = false;

            if (subnetworkBoundryId != dc.Subnetworkboundaryid)
            {
                updateDCF = true;
            }
            else
            {
                if (newSystemTypeId != dc.Systemtypeid)
                {
                    var newSystemType = _repositoryWrapper.SystemType
                .FindByCondition(k => k.Systemtypeid == newSystemTypeId)
                .Include(p => p.Majorsoftwarebuilds)
                .ThenInclude(p => p.Orgeqpmanufacturer)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Platform)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                .FirstOrDefault();
                    var newmhb = newSystemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;

                    var oldSystemType = _repositoryWrapper.SystemType
                .FindByCondition(k => k.Systemtypeid == dc.Systemtypeid)
                .Include(p => p.Majorsoftwarebuilds)
                .ThenInclude(p => p.Orgeqpmanufacturer)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Platform)
                .Include(p => p.Systemtypesmajorhardwarebuilds).ThenInclude(p => p.Majorhardware).ThenInclude(p => p.Buildconstruction)
                .FirstOrDefault();
                    var oldmhb = oldSystemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain && x.Deleted == false)?.Majorhardware;

                    if (oldSystemType.Majorsoftwarebuilds.Productname != null && newSystemType.Majorsoftwarebuilds.Productname != null)
                    {
                        if (oldSystemType.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer
                                        != newSystemType.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer
                                        || oldSystemType.Majorsoftwarebuilds.Productname
                                        != newSystemType.Majorsoftwarebuilds.Productname
                                        //|| oldSystemType.Majorsoftwarebuilds.Softwareapplication != newSystemType.Majorsoftwarebuilds.Softwareapplication
                                        || oldmhb?.Hardwaresolution != newmhb?.Hardwaresolution
                                        || oldmhb?.Platform.Platform != newmhb?.Platform.Platform)
                        {
                            updateDCF = true;
                        }
                    }
                }
            }

            // var dc = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId).Include(x => x.Designcomponentfamily).FirstOrDefault();

            var rm = new List<ResultMessageDto>();

            if (updateDCF)
            {
                var designComponentFamily = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == dc.Designcomponentfamilyid)
              .Select(x => x.DCFName(_repositoryWrapper)).ToArray();

                var designComponents = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == dc.Designcomponentfamilyid)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Select(x => x.toDesignComponentNameLcm(_repositoryWrapper)).ToArray();

                var designAspects = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Designcomponentfamilyid == dc.Designcomponentfamilyid)
                           .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                           .Select(x => x.toDesignAspectName(_repositoryWrapper)).ToArray();

                if (designComponentFamily.Length > 0)
                    rm.Add(new ResultMessageDto { Table = "Design Component Family", Values = designComponentFamily });

                if (designAspects.Length > 0)
                    rm.Add(new ResultMessageDto { Table = "Design Aspects", Values = designAspects });

                if (designComponents.Length > 1)
                    rm.Add(new ResultMessageDto { Table = "Design Component", Values = designComponents });

            }

            //var lcmEngineering = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == designComponentId)
            //               .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
            //               .Select(x => x.toDescription(_repositoryWrapper)).ToArray();

            var Lcmengineering = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == designComponentId)
              .Include(x => x.Opco)
              .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype)
              .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
              .ThenInclude(x => x.Platform)
              .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
              .ThenInclude(x => x.Orgeqpmanufacturer)
              .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
              .ThenInclude(x => x.Subnetworkboundary)

              .Select(x => x.toDescription(_repositoryWrapper)).ToArray();

            //var assets = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Designcomponentid == designComponentId)
            //               .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
            //               .Select(x => x.to(_repositoryWrapper)).ToArray();

            var NetworkElementAsPlanned = _repositoryWrapper.NetworkElementAsPlanned
               .FindByCondition(x => x.Designcomponentid == designComponentId)
               .Include(p => p.Opco)
               .Select(x => x.toFullDescription(_repositoryWrapper))
               .ToArray();

            if (Lcmengineering.Length > 0)
                rm.Add(new ResultMessageDto { Table = "LCM", Values = Lcmengineering });

            if (NetworkElementAsPlanned.Length > 0)
                rm.Add(new ResultMessageDto { Table = "Assets", Values = NetworkElementAsPlanned });

            if (rm.Count > 0)
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = new RelatedRecordsResultDto
                    {
                        EntityName = "Design Component",
                        RecordName = dc.toDesignComponentNameLcm(_repositoryWrapper),
                        DataRelatedList = rm
                    }
                };
            return new ResultDto();
        }

        public async Task<ResultDto> UpdateDesignComponantFamily(long designComponentId, long subnetworkId, long systemTypeId)
        {
            var dcf = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId).Include(x => x.Designcomponentfamily).FirstOrDefault().Designcomponentfamily;

            var systemType = await _repositoryWrapper.SystemType
                .FindByCondition(x => x.Systemtypeid == systemTypeId)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Majorsoftwarebuilds).AsNoTracking()
                .SingleAsync();

            var majorHardware = systemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware;

            var plid = majorHardware?.Platformid ?? 0;

            var platform = (await _repositoryWrapper.Platform.FindByCondition(x =>
                x.Platformid == plid, true, false).SingleOrDefaultAsync())?.Platform ?? "";

            var majorHW = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == majorHardware.Majorhardwareid, false)
               .Include(x => x.Buildconstruction).SingleOrDefault();

            var hardwareSolution = majorHW.Hardwaresolution;
            var buildConstructionRule = majorHW.Buildconstruction.Rule;

            var hwOemId = majorHardware?.Orgeqpmanufacturerid;

            var productName = systemType.Majorsoftwarebuilds.Productname != null ?
                systemType.Majorsoftwarebuilds.Productname.Description : "";
            var originalEquipmentManufacturerId = systemType.Majorsoftwarebuilds.Orgeqpmanufacturerid;

            dcf.Systemtypeidentityname = dcf.Systemtypeidentityname = await Utils.STIM(_repositoryWrapper, hwOemId, originalEquipmentManufacturerId, productName, platform, hardwareSolution, buildConstructionRule);

            dcf.Subnetworkboundaryid = subnetworkId;

            _repositoryWrapper.DesignComponentFamily.Update(dcf);

            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Warning = true,
                Info = ResultMessages.EntryUpdateSuccess,
                Data = dcf.Designcomponentfamilyid
            };
        }

        public async Task<ResultDto> UpdateDesignAspectSupportedServices(long designComponentId, long subnetworkId)
        {
            var dcf = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId).Include(x => x.Designcomponentfamily).FirstOrDefault().Designcomponentfamily;
            var designAspects = _repositoryWrapper.DesignAspectRepository.FindByCondition(x => x.Designcomponentfamilyid == dcf.Designcomponentfamilyid);
            var newServicesIds = _repositoryWrapper.SubnetworkSupportedServiceRepository.FindByCondition(x => x.Subnetworkid == subnetworkId).Select(x => x.Serviceid).Distinct().ToList();
            foreach (var item in designAspects)
            {
                var services = _repositoryWrapper.DesignAspectSupportedServiceRepository.FindByCondition(x => x.Id == item.Id);
                foreach (var srv in services)
                {
                    _repositoryWrapper.DesignAspectSupportedServiceRepository.DeleteDeep(srv);
                }

                await _repositoryWrapper.SaveAsync();

                foreach (var srvId in newServicesIds)
                {
                    _repositoryWrapper.DesignAspectSupportedServiceRepository.Create(new Designaspectssupportedsvr() { Designaspectid = item.Id, Serviceid = srvId });
                }

                await _repositoryWrapper.SaveAsync();
            }

            return new ResultDto
            {
                Warning = true,
                Info = ResultMessages.EntryUpdateSuccess,
                Data = new { id = designComponentId }
            };
        }

        public async Task UpdateOtherDesignComponants(long dcfId, long subnetworkId)
        {
            var designComponents = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentfamilyid == dcfId).ToList();
            if (designComponents.Count > 1)
            {
                foreach (var item in designComponents)
                {
                    item.Subnetworkboundaryid = subnetworkId;
                    _repositoryWrapper.DesignComponent.Update(item);
                }
                await _repositoryWrapper.SaveAsync();
            }
        }

        public async Task<ResultDto> IsDCHasNfxiBuildConstruction(int designComponentId)
        {
            var isDCVirtualized = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId)
                                                            .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                                                            .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                                            .AnyAsync(x => x.Systemtype.Systemtypesmajorhardwarebuilds
                                                            .FirstOrDefault(h => h.Ismain).Majorhardware.Buildconstruction.Rule == (int)BuildconstructionRuleEnum.VirtualHW);

            return new ResultDto
            {
                Data = isDCVirtualized,
                Info = "",
                Warning = false
            };
        }

    }
}