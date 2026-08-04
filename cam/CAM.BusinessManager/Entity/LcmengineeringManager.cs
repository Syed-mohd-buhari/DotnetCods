using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.LookUp;
using CAM.BusinessManager.Rules;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.DesignAspects;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.Entita.ResourceKeyMaster;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.LookUp;
using CAM.DataTransferObjects.LookUp.ReasonCheckbox;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Identity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBContext;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;
using static CAM.Enum.ResourceTypeEnum;

namespace CAM.BusinessManager.Entity
{
    public class LcmEngineeringManager : BaseManager
    {
        private readonly ModelContext _modelContext;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _manager;
        private readonly PlannedActivityManager _plannedActivityManager;
        private readonly DeliveryTrackingManager _deliveryTrackingManager;
        private readonly DesignAspectManager _designAspectManager;
        private readonly NetworkElementsAsPlannedManager _networkElementsAsPlannedManager;
        private readonly ResourceKeyMasterManager _resourceKeyMasterManager;
        private readonly DesignComponentFamilyLifeCycleManager _designComponentFamilyLifeCycleManager;
        private readonly ILoggerManager _logger;
        private readonly NetworkElementNodeCountManager.NetworkElementNodeCountManager _networkElementNodeCountManager;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private bool isNoPa = false;
        private readonly DesignComponentFamilyManager _designComponentFamilyManager;
        private readonly LcmAncillaryDataManager _lcmAncillaryDataManager;
        public LcmEngineeringManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, IRepositoryWrapper repositoryWrapper,
            GridCustomColumnManager manager, PlannedActivityManager plannedActivityManager,
            NetworkElementsAsPlannedManager networkElementsAsPlannedManager, DesignAspectManager designAspectManager,
            IHttpContextAccessor contextAccessor, ILoggerManager logger, NetworkElementNodeCountManager.NetworkElementNodeCountManager networkElementNodeCountManager
            , DeliveryTrackingManager deliveryTrackingManager, ResourceKeyMasterManager resourceKeyMasterManager,
            DesignComponentFamilyLifeCycleManager designComponentLIfecycleManager,
            CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager, DesignComponentFamilyManager designComponentFamilyManager,
            LcmAncillaryDataManager lcmAncillaryDataManager
            ) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _plannedActivityManager = plannedActivityManager;
            _networkElementsAsPlannedManager = networkElementsAsPlannedManager;
            _designAspectManager = designAspectManager;
            _logger = logger;
            _networkElementNodeCountManager = networkElementNodeCountManager;
            _deliveryTrackingManager = deliveryTrackingManager;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _resourceKeyMasterManager = resourceKeyMasterManager;
            _designComponentFamilyLifeCycleManager = designComponentLIfecycleManager;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _designComponentFamilyManager = designComponentFamilyManager;
            _lcmAncillaryDataManager = lcmAncillaryDataManager;
        }

        public async Task<ResultDto> Add(LcmEngineeringDtoCreate dto, bool? forced = false)
        {
            Lcmengineering model = await _repositoryWrapper.Lcmengineering.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                && x.Designcomponentid == dto.DesignComponentId && x.Archived != true, true)
                .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();
            LcmEngineering entityExists = LCMEngineeringMapper.GetLcmEngineeringMapper(model);

            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _repositoryWrapper.BeginTransaction();
            try
            {

                if (entityExists != null)
                {
                    if (entityExists.NumberOfNodes == 0 && entityExists.Deleted == true)
                    {
                        if (forced == true)
                        {
                            ResultDto addBaseRes = await AddBase(dto);
                            trans.Commit();
                            return addBaseRes;

                        }
                        else
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExists,
                                Data = new { id = entityExists.LcmengineeringId, orphanDeleted = true }
                            };
                        }
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = entityExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entityExists.LcmengineeringId
                        };
                    }
                }
                if (dto?.NetworkElementAssociateds != null)
                {
                    ResultDto result = await GenerateNetworkElementAssociated(dto, false);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                    if (result.Warning)
                    {
                        trans.Rollback();
                        return result;
                    }
                    ;
                }


                ResultDto res = await AddBase(dto);
                trans.Commit();
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                trans.Rollback();
                return new ResultDto
                {
                    Warning = true,
                    Info = "Server error occured",
                    Data = null
                };
            }

        }
        public async Task<ResultDto> GenerateNetworkElementAssociated(LcmEngineeringDtoCreate dto, bool isUpdate)
        {
            short? majorSoftwareOemId;
            if (dto.IsReleaseDetailUnKnown)
            {
                majorSoftwareOemId = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == dto.DesignComponentFamilyid)
                    .FirstOrDefault().Majorsoftwareoemid;
            }
            else
            {
                majorSoftwareOemId = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentid == dto.DesignComponentId)
                    .Include(x => x.Designcomponentfamily).FirstOrDefault().Designcomponentfamily.Majorsoftwareoemid;
            }

            if (majorSoftwareOemId != null)
            {
                short oemId = majorSoftwareOemId.Value;
                if (dto?.NetworkElementAssociateds != null)
                {

                    IEnumerable<string> names = dto.NetworkElementAssociateds?.Where(x => x.Id == 0).Select(x => x.ElementName);

                    List<Networkelementsasplanned> entityExists = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(
                            x => x.Opcoid == dto.OpCoId
                                 && x.Orgeqpmanufacturerid == oemId
                                 && x.Designcomponentid == dto.DesignComponentId
                                 && names.Contains(x.Elementname), true)
                        .OrderByDescending(x => x.Creationdate).ToListAsync();
                    //Ticket 658 Dev - #622 - Release details unknown - Duplicate PA's
                    if (((entityExists != null && entityExists.Count > 0 && !isUpdate) || (entityExists != null && entityExists.Count > 1)) &&
                        (dto.IsReleaseDetailUnKnown == false))
                    {
                        return new ResultDto()
                        {
                            Warning = true,
                            Info = $"Element Name duplicate {entityExists.FirstOrDefault().Elementname}",
                            Data = entityExists
                        };
                    }
                    foreach (NetworkElementAssociated x in dto.NetworkElementAssociateds)
                    {
                        if (x.Id != 0) //Ticket 797 
                        {
                            Networkelementsasplanned asset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(y => y.Networkelementasplannedid == x.Id).FirstOrDefault();
                            if (asset != null)
                            {
                                asset.Isfinalasset = x.IsFinalAsset;
                                _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                                _repositoryWrapper.Save();
                                await _repositoryWrapper.ClearTracker();
                            }
                        }
                        else if (x.Id == 0)
                        {

                            ResultDto data = await _networkElementsAsPlannedManager.Add(new NetworkElementAsPlannedDtoCreate()
                            {
                                DesignComponentId = dto.DesignComponentId,
                                OriginalEquipmentManufacturerId = oemId,
                                OpCoId = dto.OpCoId,
                                EduSpocIds = dto.EduSpocIds,
                                SubDomainSpocIds = dto.SubDomainSpocIds,
                                ElementName = x.ElementName,
                                EnvironmentId = x.EnviromentId,
                                LocationId = x.LocationId,
                                DeploymentStatusId = x.AssetsStatusId,
                                // add node selection code
                                IsFinalAsset = x.IsFinalAsset,
                                BuildBagId = dto.BuildBagId,
                                AssetLiveStatusDate = x?.AssetLiveStatusDate == null ? null : x.AssetLiveStatusDate,
                                AssetRfoDate = x?.AssetRfoDate == null ? null : x.AssetRfoDate,
                                AssetRfsDate = x?.AssetRfsDate == null ? null : x.AssetRfsDate,

                                BomSubmittedDate = x?.BomSubmittedDate == null ? null : x.BomSubmittedDate,
                                RfaDate = x?.RfaDate == null ? null : x.RfaDate,
                                HwPoRaisedDate = x?.HwPoRaisedDate == null ? null : x.HwPoRaisedDate,
                                HwPoArrivedDate = x?.HwPoArrivedDate == null ? null : x.HwPoArrivedDate,

                            });

                            if (data.Warning)
                            {
                                return data.Info.Contains("exist")
                                    ? new ResultDto()
                                    {
                                        Warning = true,
                                        Info = $"Element Name duplicate",
                                        Data = null
                                    }
                                    : data;
                            }
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                        }
                    }
                }
            }

            return new ResultDto()
            {
                Warning = false,
                Info = ResultMessages.EntryUpdateSuccess
            };
        }
        public async Task<ResultDto> AddBase(LcmEngineeringDtoCreate dto)
        {
            try
            {
                if (dto.CheckboxResourceResource == null || !dto.CheckboxResourceResource.Any())
                {
                    IQueryable<Reasoncheckboxresources> checkBox = _repositoryWrapper.ReasonCheckboxResource.FindAll(true);
                    dto.CheckboxResourceResource =
                        checkBox.ToDictionary(x => (int)x.Id, x => _mapper.Map<ReasonCheckboxDto>(x));
                }

                LcmEngineering entity = _mapper.Map<LcmEngineering>(dto);

                Lcmengineering model = LCMEngineeringMapper.SetLcmEngineeringMapper(entity);

                if (!string.IsNullOrEmpty(dto.ReasonForNoPlan) && !string.IsNullOrEmpty(dto.CommentOnProjectStatus))
                {
                    Lcmancillarydata lcmancillarydata = new()
                    {
                        Reasonfornoplan = dto.ReasonForNoPlan,
                        Commentonprojectstatus = dto.CommentOnProjectStatus
                    };
                    model.Lcmancillarydata.Add(lcmancillarydata);
                }
                return await AddLcmEntity(model, null, false, true, false, dto.PlannedActivityDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        ///create a new dc based on dcf
        public async Task<ResultDto> CreateUnkownDesignComponentBasedOnDCF(long dcfid)
        {
            Designcomponentfamilies dcfEntity = await _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == dcfid).FirstOrDefaultAsync();
            long majorhardwareid = 0;
            long majorsoftwareid = 0;
            long systemtypeid = 0;
            long DCid = 0;
            IDictionary<long, string> newDcResouce = new Dictionary<long, string>();
            if (dcfEntity != null)
            {
                //Get Existing majorhardwarebuild
                var majorhardwareEntity = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Orgeqpmanufacturerid == dcfEntity.Majorhardwareoemid
                && x.Platformid == dcfEntity.Platformid).OrderByDescending(x => x.Modificationdate).Include(x => x.Majorhwbuildsdesigncontacts).FirstOrDefault();



                if (majorhardwareEntity != null)
                {
                    ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'
                    if (majorhardwareEntity.Hardwaretype.ToLower() == ConstantValueFilter.Unknown.ToLower())
                    {
                        var mHwWithoutUnknownData = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Orgeqpmanufacturerid == dcfEntity.Majorhardwareoemid
                           && x.Platformid == dcfEntity.Platformid && x.Hardwaretype.ToLower() != ConstantValueFilter.Unknown.ToLower()).OrderByDescending(x => x.Modificationdate).Include(x => x.Majorhwbuildsdesigncontacts).FirstOrDefault();

                        majorhardwareid = majorhardwareEntity.Majorhardwareid;
                    }
                    else
                    {
                        ///create majorhardwareEntity
                        MajorHardwareBuild majorhardwarenewEntity = new()
                        {
                            OriginalEquipmentManufacturerId = dcfEntity.Majorhardwareoemid.Value,
                            PlatformId = dcfEntity.Platformid.Value,
                            ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'
                            HardwareType = ConstantValueFilter.unKnownValue,
                            HardwareSolution = majorhardwareEntity.Hardwaresolution,
                            EOMStatus = (EOMEnum)majorhardwareEntity.Eomstatus,
                            BuildConstructionId = majorhardwareEntity.Buildconstructionid,
                            EndOfMaintenance = majorhardwareEntity.Endofmaintenance,
                            EndOfsupport = majorhardwareEntity.Endofsupport,
                        };
                        Majorhardwarebuilds mappingMajorHWEntity = MajorHardwareBuildMapper.SetMajorHardwareBuildMapper(majorhardwarenewEntity);
                        if (mappingMajorHWEntity != null)
                        {
                            _repositoryWrapper.MajorHardwareBuild.Create(mappingMajorHWEntity);
                            _repositoryWrapper.Save();
                            await _repositoryWrapper.ClearTracker();
                        }

                        //create HwDesignContact while creating unknown HW -- march 20 25
                        _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.Create(new Majorhwbuildsdesigncontacts
                        {
                            Majorhardwarebuildsid = mappingMajorHWEntity.Majorhardwareid,
                            Designcontactid = majorhardwareEntity.Majorhwbuildsdesigncontacts.Select(x => x.Designcontactid).FirstOrDefault(),
                        });

                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();

                        //Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'  -- July 3 -24
                        majorhardwareid = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Orgeqpmanufacturerid == dcfEntity.Majorhardwareoemid
                        && x.Platformid == dcfEntity.Platformid && x.Hardwaretype.ToLower() == ConstantValueFilter.Unknown).OrderByDescending(x => x.Modificationdate).FirstOrDefault().Majorhardwareid;

                    }

                }
                //Get Existing majorsoftwarebuild
                // Ticket 711 Unable to select  Some Current DesignCompnenet Family while creating new  Unknown Resource Key LCM
                var majorsoftwareEntity = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Orgeqpmanufacturerid == dcfEntity.Majorsoftwareoemid &&
               x.Productnameid == dcfEntity.Productnameid && x.Systemtypes != null && x.Systemtypes.Count() > 0).OrderByDescending(x => x.Modificationdate).Include(x => x.Systemtypes).Include(x => x.Majorswbuildsdesigncontacts).FirstOrDefault();

                if (majorsoftwareEntity != null)
                {
                    ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'
                    if (majorsoftwareEntity.Softwareversion.ToLower() == ConstantValueFilter.Unknown)
                    {
                        majorsoftwareid = majorsoftwareEntity.Majorsoftwarebuildsid;
                    }
                    else
                    {
                        ///create majorsoftwareEntity
                        MajorSoftwareBuild majorsoftwarNewEntity = new()
                        {
                            OriginalEquipmentManufacturerId = dcfEntity.Majorsoftwareoemid.Value,
                            //Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'
                            SoftwareVersion = ConstantValueFilter.unKnownValue,
                            EndOfMaintenance = majorsoftwareEntity.Endofmaintenance,
                            EndOfsupport = majorsoftwareEntity.Endofsupport,
                            GeneraAvailableDate = majorsoftwareEntity.Generaavailabledate,
                            DeliveryMethod = majorsoftwareEntity.Deliverymethod,
                            EOMStatus = (EOMEnum)majorsoftwareEntity.Eomstatus,
                            ProductNameId = dcfEntity.Productnameid
                        };
                        Majorsoftwarebuilds mappingMajorSWEntity = MajorSoftwareBuildMapper.SetMajorSoftwareBuildMapper(majorsoftwarNewEntity);
                        if (mappingMajorSWEntity != null)
                        {
                            _repositoryWrapper.MajorSoftwareBuild.Create(mappingMajorSWEntity);
                            _repositoryWrapper.Save();
                            await _repositoryWrapper.ClearTracker();
                        }

                        _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.Create(new Majorswbuildsdesigncontacts
                        {
                            Majorsoftwarebuildsid = mappingMajorSWEntity.Majorsoftwarebuildsid,
                            Designcontactid = majorsoftwareEntity.Majorswbuildsdesigncontacts.Select(x => x.Designcontactid).FirstOrDefault(),
                        });
                        _repositoryWrapper.Save();
                        await _repositoryWrapper.ClearTracker();

                        //Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'
                        majorsoftwareid = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Orgeqpmanufacturerid == dcfEntity.Majorsoftwareoemid
                        && x.Productnameid == dcfEntity.Productnameid && x.Softwareversion.ToLower() == ConstantValueFilter.Unknown).OrderByDescending(x => x.Modificationdate).FirstOrDefault().Majorsoftwarebuildsid;
                    }
                }
                //Get product name
                Productname productnameEntity = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == dcfEntity.Productnameid).FirstOrDefault();
                //Get Existing systemtypes
                Systemtypes systemTypesEntity = _repositoryWrapper.SystemType.FindByCondition(x => x.Vodafonename == productnameEntity.Vodafonenamesid
                && x.Majorsoftwarebuildsid == majorsoftwareEntity.Majorsoftwarebuildsid && x.Systemtypenameoem.ToLower() == productnameEntity.Description.ToLower())
                    .OrderByDescending(x => x.Modificationuser).FirstOrDefault();

                if (systemTypesEntity != null)
                {
                    if (systemTypesEntity.Majorsoftwarebuildsid == majorsoftwareid)
                    {
                        systemtypeid = systemTypesEntity.Systemtypeid;
                    }
                    else
                    {
                        SystemType systemTypeNewEntity = new()
                        {
                            SystemTypeNameOem = systemTypesEntity.Systemtypenameoem,
                            MajorSoftwareBuildsId = majorsoftwareid,
                            EndOfMaintenance = systemTypesEntity.Endofmaintenance.ToString(),
                            AssetCategoryId = systemTypesEntity.Assetcategoryid,
                            ProductImportanceId = systemTypesEntity.Productimportanceid,
                            AssetClassId = systemTypesEntity.Assetclassid,
                            VodafoneNameId = systemTypesEntity.Vodafonename
                        };
                        Systemtypes mappingSystemTypeEntity = SystemTypeMapper.SetSystemTypeMapper(systemTypeNewEntity);
                        if (mappingSystemTypeEntity != null)
                        {
                            _repositoryWrapper.SystemType.Create(mappingSystemTypeEntity);
                            _repositoryWrapper.Save();
                        }

                        systemtypeid = _repositoryWrapper.SystemType.FindByCondition(x => x.Vodafonename == productnameEntity.Vodafonenamesid
                        && x.Majorsoftwarebuildsid == majorsoftwareid &&
                        x.Systemtypenameoem.ToLower() == productnameEntity.Description.ToLower()).OrderByDescending(x => x.Modificationdate).FirstOrDefault().Systemtypeid;


                        ///get or create systemtypemajorhardwarebuild

                        Systemtypesmajorhardwarebuilds systemtypemajorHardwareBuildEntity = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Systemtypeid == systemtypeid).FirstOrDefault();
                        if (systemtypemajorHardwareBuildEntity != null)
                        {
                            systemtypemajorHardwareBuildEntity.Majorhardwareid = majorhardwareid;
                            _repositoryWrapper.SystemTypesMajorHardwareBuild.Update(systemtypemajorHardwareBuildEntity);
                            _repositoryWrapper.Save();
                        }
                        else
                        {
                            Systemtypesmajorhardwarebuilds smhb = new()
                            {
                                Systemtypeid = systemtypeid,
                                Majorhardwareid = majorhardwareid,
                                Ismain = true
                            };
                            _repositoryWrapper.SystemTypesMajorHardwareBuild.Create(smhb);
                            _repositoryWrapper.Save();
                        }

                    }
                }
                //Create DesignComponent 
                //Check the designcomponent is existing are not

                Designcomponents existDcomponent = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Subnetworkboundaryid == dcfEntity.Subnetworkboundaryid
                && x.Systemtypeid == systemtypeid && x.Designcomponentfamilyid == dcfEntity.Designcomponentfamilyid).FirstOrDefault();

                #region  // Ticket 711 Unable to select  Some Current DesignCompnenet Family while creating new  Unknown Resource Key LCM
                if (existDcomponent == null && systemtypeid != 0)
                {
                    Designcomponents createDesignComponent = new()
                    {
                        Systemtypeid = systemtypeid,
                        Subnetworkboundaryid = dcfEntity.Subnetworkboundaryid,
                        Designcomponentfamilyid = dcfEntity.Designcomponentfamilyid
                    };
                    _repositoryWrapper.DesignComponent.Create(createDesignComponent);
                    _repositoryWrapper.Save();

                    DCid = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Subnetworkboundaryid == dcfEntity.Subnetworkboundaryid
                           && x.Systemtypeid == systemtypeid && x.Designcomponentfamilyid == dcfEntity.Designcomponentfamilyid, true).FirstOrDefault().Designcomponentid;

                    List<Designcomponents> newDesignResource = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == DCid)
                                           .Include(p => p.Systemtype)
                                           .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                           .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction).ToList();
                    newDcResouce = newDesignResource.toDesignComponentResource(_repositoryWrapper);
                }
                else if (existDcomponent != null)
                {
                    DCid = existDcomponent.Designcomponentid;
                    List<Designcomponents> newDesignResource = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == DCid, true)
                                            .Include(p => p.Systemtype)
                                            .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                            .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction).ToList();
                    newDcResouce = newDesignResource.toDesignComponentResource(_repositoryWrapper);
                }
                #endregion
            }

            return new ResultDto
            {
                // Ticket 711 Unable to select  Some Current DesignCompnenet Family while creating new  Unknown Resource Key LCM
                Info = newDcResouce.Count() != 0 ? ResultMessages.EntryAddSuccess :
                (systemtypeid == 0 ? ResultMessages.NoSystemType : ResultMessages.NoDesignComponent),
                Data = newDcResouce
            };
        }

        public async Task<ResultDto> CreateUnkownDesignComponentPALevel(long dcId)
        {
            long? dcfId = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == dcId).FirstOrDefault()?.Designcomponentfamilyid;

            return new ResultDto
            {
                Data = CreateUnkownDesignComponentBasedOnDCF(dcfId.Value).Result.Data,
            };
        }

        public Plannedactivities PlannedActivityExists(long plannedActivityId, long? plannedDC, short? OpcoId, long dc, short? plannedActivityResourceID, bool? isUnknownLcm = false)
        {
            //Ticket 658 Dev - #622 - Release details unknown - Duplicate PA's         
            var query = _repositoryWrapper.PlannedActivity.FindByCondition(x =>
    x.Plannedactivityresourceid == plannedActivityResourceID &&
    x.Opcoid == OpcoId &&
    x.Lcmengineering.Designcomponentid == dc &&
    x.Designcomponentid == plannedDC);

            if (plannedActivityId == 0)
            {
                query = query.Where(x => x.Archived != true);
            }
            else if (isUnknownLcm == false)
            {
                query = query.Where(x => x.Plannedactivityid != plannedActivityId && x.Archived != true);
            }
            else
            {
                query = query.Where(x => x.Plannedactivityid != plannedActivityId);
            }

            var result = query.Include(x => x.Lcmengineering).FirstOrDefault();

            return result;

        }
        public async Task<ResultDto> CreateDesignAspectWithOPCOAndDCF(short? opcoId, long? dcf)
        {
            Designaspects designAspect = _repositoryWrapper.DesignAspectRepository
           .FindByCondition(x => x.Opcoid == opcoId && x.Designcomponentfamilyid == dcf && x.Archived != true)
           .FirstOrDefault();
            try
            {
                if (designAspect == null && opcoId != null && dcf != null)
                {
                    DesignAspectDtoModel designAspectObject = new()
                    {
                        OpCoId = opcoId,
                        DesignComponentFamilyId = dcf.Value,
                        UsedNetworkFunctionsIds = new List<int>(),
                        SupportedServicesIds = new List<int>(),
                        NominalCapacityLimit = string.Empty,
                        DesignedCapacityLimit = string.Empty,
                        MaxAllowedLoading = string.Empty
                    };
                    _ = await _designAspectManager.Add(designAspectObject);

                    return new ResultDto
                    {
                        Info = ResultMessages.EntryAddSuccess,
                        Data = designAspect
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Issue happen when try to Adding Design Aspect  : " + ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }
        public async Task<ResultDto> AddLcmEntity(Lcmengineering entity, Lcmengineering oldLcmEng = null, bool? isNotAllowedToCreateAssetForUnKnownResource = false, bool isNewLcmCreateViaUi = false, bool isCreateLCMAndRealtedTableRecordsForResourceUnknowLCM = false, List<PlannedActivityDtoUpdate> PlannedActivityDto = null)
        {
            try
            {
                entity = await SetLcmValue(entity);

                entity.Buildbagid = entity.Buildbagid;

                // Start of ResourceKeyMaster table (LCM R8 Part 3 requirements)
                string lcmResourceKeys = await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforLCM(entity.Resourcekey, entity.Designcomponentid, entity.Opcoid, (long)entity.Buildbagid);
                entity.Resourcekey = lcmResourceKeys;
                if (!(entity.Isreleasedetailunknown == true))
                {
                    entity.Designcomponentfamilyid = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == entity.Designcomponentid).Select(x => x.Designcomponentfamilyid).FirstOrDefault();
                }
                // End of ResourceKeyMaster table (LCM R8 Part 3 requirements)

                short? FetchNopAResourcesId = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x =>
                               x.Plannedactivityresourceid == entity.PlannedactivitiesLcmengineering.Select(p => p.Plannedactivityresourceid).FirstOrDefault()
                              && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity).FirstOrDefault()?.Plannedactivityresourceid;
                ICollection<Plannedactivities> nOPaplannedActivities = entity.PlannedactivitiesLcmengineering;
                if (FetchNopAResourcesId is not null and not 0)
                {
                    isNoPa = true;
                }

                if (isNoPa)
                {

                    entity.PlannedactivitiesLcmengineering = null;
                }

                _repositoryWrapper.Lcmengineering.Create(entity);
                await _repositoryWrapper.SaveAsync();



                #region  //Ticket 1810 -Design Components Not Getting Listed in PROD
                var transitDcEntry = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == entity.Designcomponentid && p.Visibleflag == false).FirstOrDefault();
                if (transitDcEntry != null)
                {
                    transitDcEntry.Visibleflag = true;
                    _repositoryWrapper.DesignComponent.Update(transitDcEntry);
                    await _repositoryWrapper.SaveAsync();

                }
                #endregion

                List<Networkelementsasplanned> assets = _repositoryWrapper.NetworkElementAsPlanned
                                    .FindByCondition(x => x.Opcoid == entity.Opcoid && x.Designcomponentid == entity.Designcomponentid)
                                    .ToList();

                #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow and  #412 Decommission Flow Implementation Decommission Flow April 17 2024
                string assetDeploymentStatusId = string.Empty;

                if (entity.PlannedactivitiesLcmengineering != null && entity.PlannedactivitiesLcmengineering.Count() > 0 && isNoPa == false)
                {
                    Plannedactivities plannedActivityDetail = entity.PlannedactivitiesLcmengineering.FirstOrDefault();
                    assetDeploymentStatusId = FetchAssetDeploymentStatusFromSettingPA(plannedActivityDetail.Plannedactivityresourceid, plannedActivityDetail.Deliverystatusid);

                }
                #endregion
                #region Ticket 722 Create New LCM - Using Existing Unknown Resource LCM When User Specify the DC and LCM Have More than one PA 
                if (isNotAllowedToCreateAssetForUnKnownResource == false)
                {
                    foreach (Networkelementsasplanned asset in assets)
                    {
                        asset.Buildbagid = entity.Buildbagid;
                        if ((asset.Swresourcekey == null) || (asset.Hwresourcekey == null))
                        {
                            //(LCM R8 Part 3 requirements)
                            string swResourceKey = string.Empty;
                            string hwResourceKey = string.Empty;

                            //Passing the new LCM Entity design component will validate against the existing entry
                            //and there will not be an entry, if the DCF changes so a new resourcekey will be assigned.
                            IDictionary<int, string> AssetKeys = await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforAssets(asset.Designcomponentid, asset.Opcoid, asset.Elementname, asset.Buildbagid);
                            if (AssetKeys != null)
                            {
                                swResourceKey = AssetKeys[(int)ResourceTypesKey.SWAsset];
                                hwResourceKey = AssetKeys[(int)ResourceTypesKey.HWAsset];
                                asset.Swresourcekey = swResourceKey;
                                asset.Hwresourcekey = hwResourceKey;
                            }
                            //End of (LCM R8 Part 3 requirements)
                        }
                        #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow and  #412 Decommission Flow Implementation Decommission Flow April 17 2024
                        if (!string.IsNullOrEmpty(assetDeploymentStatusId))
                        {
                            asset.Deploymentstatusid = Convert.ToInt16(assetDeploymentStatusId);
                        }
                        #endregion
                        asset.Lcmengineeringid = entity.Lcmengineeringid;

                        _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                    }

                    _repositoryWrapper.Save();
                }
                #endregion

                int prodNodesCount = entity.CountNetworkElementReleated(false, _repositoryWrapper);
                if (prodNodesCount > 0)
                {
                    _ = await CreateDesignAspectWithOPCOAndDCF(entity.Opcoid,
                        _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                   .Select(x => x.Designcomponentfamilyid).FirstOrDefault());
                }
                entity.Numberofnodes = prodNodesCount;
                entity.Numberofnodesinlab = entity.CountNetworkElementReleated(true, _repositoryWrapper);
                _repositoryWrapper.Lcmengineering.Update(entity);
                await _repositoryWrapper.SaveAsync();

                if (isNoPa)
                {
                    entity.PlannedactivitiesLcmengineering = nOPaplannedActivities;
                }

                foreach (Plannedactivities plannedActivity in entity.PlannedactivitiesLcmengineering)
                {
                    if (isNoPa)
                    {
                        Dictionary<short, string> activitystatusid = _dropdownDataServiceManager.GetActivityStatusDic(false, ConstantValueFilter.paActivityStatus).Result;
                        Dictionary<short, string> planningactivitystatusid = _dropdownDataServiceManager.GetPlanningActivityStatusDic(false, ConstantValueFilter.paPlanningActivityStatus).Result;
                        plannedActivity.Activitystatusid = activitystatusid.Select(x => x.Key).FirstOrDefault();
                        plannedActivity.Planningactivitystatusid = planningactivitystatusid.Select(x => x.Key).FirstOrDefault();
                        plannedActivity.Projectstatus = Outputs.NotRequested;
                    }
                    plannedActivity.Designcomponentid =
                        plannedActivity.Designcomponentid == 0 ? null : plannedActivity.Designcomponentid;
                    plannedActivity.Deliverystatusid =
                        plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatusid;
                    plannedActivity.Responsibilityphaseid =
                        plannedActivity.Responsibilityphaseid == 0 ? null : plannedActivity.Responsibilityphaseid;
                    plannedActivity.Deliverystatusid =
                        plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatusid;
                    plannedActivity.Lcmengineeringid = entity.Lcmengineeringid;
                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Planningriskid =
                        plannedActivity.Planningriskid == 0 ? null : plannedActivity.Planningriskid;
                    plannedActivity.Engineeringriskid =
                        plannedActivity.Engineeringriskid == 0 ? null : plannedActivity.Engineeringriskid;
                    plannedActivity.Operationalriskid =
                                plannedActivity.Operationalriskid == 0 ? null : plannedActivity.Operationalriskid;
                    plannedActivity.Plannedactivitycategoryid =
                                plannedActivity.Plannedactivitycategoryid == 0 ? null : plannedActivity.Plannedactivitycategoryid;
                    plannedActivity.Programid =
                         plannedActivity.Programid == 0 ? null : plannedActivity.Programid;

                    #region  //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                    if (plannedActivity.Designcomponentid != 0)
                    {

                        // var dcEntry = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == plannedActivity.Designcomponentid).Select(p => p.Designcomponentfamilyid).SingleOrDefault();
                        var dcEntry = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == plannedActivity.Designcomponentid && p.Visibleflag == false).FirstOrDefault();
                        if (dcEntry != null)
                        {
                            dcEntry.Visibleflag = true;
                            _repositoryWrapper.DesignComponent.Update(dcEntry);
                        }
                    }

                }
                _repositoryWrapper.Save();
                await _repositoryWrapper.ClearTracker();
                #endregion

                bool archivedPA = false;
                foreach (Plannedactivities item in entity.PlannedactivitiesLcmengineering)
                {
                    if (item.Plannedactivityid == 0)
                    {
                        Plannedactivities plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, item.Designcomponentid, entity.Opcoid, entity.Designcomponentid, item.Plannedactivityresourceid);
                        if (plannedActivityExist != null && entity.Isreleasedetailunknown == false && item.Ispareleasedetailunknown == false)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExists,
                                Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                            };
                        }
                        else
                        {
                            _repositoryWrapper.PlannedActivity.Create(item);
                            await _repositoryWrapper.SaveAsync();
                            if (item.Deliveryplanavailable && isNoPa == false)
                            {
                                var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                if (mileStoneStatus.isSuccess)
                                {
                                    // We should pass inservice and planned asset count only 
                                    var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                    var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                    int assetCount = assets.Where(x => x.Deploymentstatusid == getAssetInserviceID && x.Environmentid == getProductionEnvrionmentId).Count();

                                    var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, assetCount);
                                    await _deliveryTrackingManager.Add(deliveryTracking);
                                }

                            }
                            var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                            foreach (var plan in addProjectPlanEntity)
                            {
                                _repositoryWrapper.ProjectPlanRepository.Create(plan);
                                await _repositoryWrapper.SaveAsync();

                                await _commonManager.CreateOrUpdateProjectPlanAudit(plan.Projectsplanid, null, item.Plannedcompletion.Value.ToShortDateString(), 1, _repositoryWrapper);
                            }
                            _repositoryWrapper.Save();

                            #region ClusterLevelPa                       
                            var clusterDto = PlannedActivityDto?.Where(x => x.DesignComponentId == item.Designcomponentid)?.FirstOrDefault()?.infraClusterClusterUpgradeUpsertDto;
                            if (clusterDto != null)
                            {
                                var rulelLinkedDc = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid).Include(x => x.Plannedactivityresource).FirstOrDefault().Plannedactivityresource?.Rulelinkeddc;
                                await _plannedActivityManager.AddOrUpdateClusterLevelData(clusterDto, item.Plannedactivityid, rulelLinkedDc);
                            }
                            #endregion

                            await _repositoryWrapper.ClearTracker();


                        }

                    }
                    else
                    {
                        Plannedactivities plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, item.Designcomponentid, entity.Opcoid, entity.Designcomponentid, item.Plannedactivityresourceid);
                        if (plannedActivityExist != null && entity.Isreleasedetailunknown == false && item.Ispareleasedetailunknown == false)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryUpdateExists,
                                Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                            };
                        }
                        else
                        {
                            _repositoryWrapper.PlannedActivity.Update(item);
                            if (item.Deliveryplanavailable && isNoPa == false)
                            {
                                var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                if (mileStoneStatus.isSuccess)
                                {
                                    // We should pass inservice and planned asset count only 
                                    var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                    var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                    int assetCount = assets.Where(x => x.Deploymentstatusid == getAssetInserviceID && x.Environmentid == getProductionEnvrionmentId).Count();

                                    var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, assetCount);
                                    await _deliveryTrackingManager.Add(deliveryTracking);
                                }

                            }
                            var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                            foreach (var plan in addProjectPlanEntity)
                            {
                                _repositoryWrapper.ProjectPlanRepository.Create(plan);
                            }
                            _repositoryWrapper.Save();
                            await _repositoryWrapper.ClearTracker();
                        }
                    }

                    await _repositoryWrapper.SaveAsync();

                    int? settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering && x.Plannedactivityresourceid == item.Plannedactivityresourceid && x.Deliverystatusid == item.Deliverystatusid)
                            .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Ruleelementcount;

                    if (settingRuleElementCount == (int)RuleElementCountEnum.RolloutComplete && (entity.CountNetworkElementReleated(false, _repositoryWrapper) > 0 || entity.CountNetworkElementReleated(true, _repositoryWrapper) > 0))
                    {
                        archivedPA = true;
                        item.Archived = true;
                        _repositoryWrapper.PlannedActivity.Update(item);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();

                        _ = await RolloutPlannedActivityFromDDl(item);
                        Plannedactivities PAEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid, true, false)
                            .Include(x => x.Plannedactivityresource)
                            .FirstOrDefault();
                        if (PAEntity.Plannedactivityresource.Rulelinkeddc is not ((int)PlannedActivityResourceEnum.New_NFxI_Solution)
                         and not ((int)PlannedActivityResourceEnum.New_System_HW_SW_Solution))
                        {
                            _ = await ArchiveDesginAspectCasePARolledout(entity, item);
                        }

                    }
                    else if (isNoPa == false)
                    {
                        Lcmengineering plannedDc = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == entity.Opcoid && x.Designcomponentid == item.Designcomponentid && x.Archived != true).FirstOrDefault();
                        if (plannedDc == null)
                        {
                            Plannedactivities PAEntinty = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid, true, false)
                           .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringeduspoc)
                           .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                           .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmoperationalcontracts)
                           .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringhardware)
                           .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringsoftware)
                           .Include(x => x.Plannedactivityresource)
                            .FirstOrDefault();

                            #region //Ticket 687 EDU/Sub domain SPOC details are not copied to new LCM while doinf SW upgrade/modernize flow

                            if (entity.Lcmengineeringsubdomainspoc != null
                                && entity.Lcmengineeringsubdomainspoc.Count() != 0)
                            {
                                PAEntinty.Lcmengineering.Lcmengineeringsubdomainspoc = entity.Lcmengineeringsubdomainspoc;
                            }

                            if (entity.Lcmengineeringeduspoc != null
                              && entity.Lcmengineeringeduspoc.Count() != 0)
                            {
                                PAEntinty.Lcmengineering.Lcmengineeringeduspoc = entity.Lcmengineeringeduspoc;
                            }

                            #endregion

                            await CreateNewLcmAndPlannedActivity(PAEntinty);
                        }
                    }


                    await _repositoryWrapper.SaveAsync();

                    await _commonManager.CreateOrUpdateBptreport(item.Plannedactivityid, entity.Designcomponentid, (long)item.Designcomponentid);

                }
                if (isNoPa && !isCreateLCMAndRealtedTableRecordsForResourceUnknowLCM)
                {
                    //await InsertUpdateAncillaryData(entity.Lcmengineeringid, 0, false, entity?.Lcmancillarydata?.FirstOrDefault());
                    await InsertUpdateAncillaryDatas(entity.Lcmengineeringid, 0, false, entity?.Lcmancillarydata?.FirstOrDefault(), isNoPa);
                    return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = entity };
                }
                if (archivedPA == true)
                {
                    await _repositoryWrapper.ClearTracker();
                    Plannedactivities firstArchivedPA = _repositoryWrapper.PlannedActivity.
                        FindByCondition(x => x.Lcmengineeringid == entity.Lcmengineeringid && x.Archived == true)
                        .Include(x => x.Plannedactivityresource)
                        .FirstOrDefault();
                    if (firstArchivedPA != null)
                    {
                        _ = await MoveActivePAs(entity.Lcmengineeringid, firstArchivedPA.Designcomponentid.Value);

                        if (firstArchivedPA.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Decommission_Service_Node)
                        {
                            _ = await _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforNewSolution(entity,
                                                          firstArchivedPA, true);
                            //await InsertUpdateAncillaryData((long)entity.Lcmengineeringid, 0);

                            #region// Change DCF Asset record activityDescription 
                            string dcfAssetActivitydescription = "Asset Decommissioned";
                            #endregion
                            foreach (Networkelementsasplanned asset in assets)
                            {
                                _ = _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(asset, dcfAssetActivitydescription);
                            }
                        }


                    }

                }
                else if (!isCreateLCMAndRealtedTableRecordsForResourceUnknowLCM)
                {
                    //await InsertUpdateAncillaryData((long)entity.Lcmengineeringid, 0,false,entity.Lcmancillarydata.FirstOrDefault(),false,entity.Lcmdeploymentstatusid);
                    if (isNewLcmCreateViaUi)
                    {
                        await InsertUpdateAncillaryDatas(entity.Lcmengineeringid, 0, false, null, false, entity.Lcmdeploymentstatusid.Value, false);
                    }
                    else
                    {
                        await InsertUpdateAncillaryDatas(oldLcmEng.Lcmengineeringid, entity.Lcmengineeringid, false, null, false, entity.Lcmdeploymentstatusid.Value, false);
                    }
                }
                #region //Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.
                _ = await _commonManager.SetImplementationFlagInDCF(entity.Designcomponentid);
                #endregion


                await _repositoryWrapper.SaveAsync();
                return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = entity };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        public async Task<ResultDto> Update(LcmEngineeringDtoUpdate dto, bool? forced = false)
        {
            Lcmengineering anotherEntityWithSameNaturalKeyExistsModel = await _repositoryWrapper.Lcmengineering.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                && x.Designcomponentid == dto.DesignComponentId && x.Archived != true
                && x.Lcmengineeringid != dto.LcmEngineeringId, true)
                .OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync();
            Lcmengineering originalEntityWithSameNaturalKeyModel = await _repositoryWrapper.Lcmengineering.FindByCondition(
                x => x.Opcoid == dto.OpCoId
                && x.Designcomponentid == dto.DesignComponentId
                && x.Lcmengineeringid == dto.LcmEngineeringId, true)
                .FirstOrDefaultAsync();

            LcmEngineering anotherEntityWithSameNaturalKeyExists = LCMEngineeringMapper.GetLcmEngineeringMapper(anotherEntityWithSameNaturalKeyExistsModel);

            LcmEngineering originalEntityWithSameNaturalKey = LCMEngineeringMapper.GetLcmEngineeringMapper(originalEntityWithSameNaturalKeyModel);

            if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
            {
                return anotherEntityWithSameNaturalKeyExists.NumberOfNodes == 0 && anotherEntityWithSameNaturalKeyExists.Deleted == true
                    ? forced == true
                        ? await UpdateBase(dto, forced)
                        : new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryUpdateExists,
                            Data = new { id = anotherEntityWithSameNaturalKeyExists.LcmengineeringId, orphanDeleted = true }
                        }
                    : new ResultDto
                    {
                        Warning = true,
                        Info = anotherEntityWithSameNaturalKeyExists.Deleted ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                        Data = anotherEntityWithSameNaturalKeyExists.DesignComponentId
                    };
            }
            if (dto.NetworkElementAssociateds != null && dto.NetworkElementAssociateds.Count() > 0)
            {
                ResultDto result = await GenerateNetworkElementAssociated(dto, true);
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();
                if (result.Warning)
                {
                    return result;
                }
            }

            return await UpdateBase(dto, forced);
        }

        public async Task RecalculateSetLcmValueFromMajorSoftwareUpdate(long majorSoftwareId)
        {
            List<Lcmengineering> lcms = _repositoryWrapper.Lcmengineering.FindAll().Include(x => x.Designcomponent)
                .ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                .Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Majorsoftwarebuildsid == majorSoftwareId).ToList();
            if (lcms.Any())
            {
                foreach (Lcmengineering lcm in lcms)
                {
                    LcmEngineeringDtoUpdate entity = _mapper.Map<LcmEngineeringDtoUpdate>(lcm);

                    entity.EduSpocIds = _repositoryWrapper.LcmEngineeringEduSpoc
                        .FindByCondition(x => x.Lcmengineeringid == entity.LcmEngineeringId && x.Eduspocid != null).Select(x => x.Eduspocid)?.ToList();

                    entity.SubDomainSpocIds = _repositoryWrapper.LcmEngineeringSubDomainSpoc
                        .FindByCondition(x => x.Lcmengineeringid == entity.LcmEngineeringId && x.Subdomainspocid != null).Select(x => x.Subdomainspocid)?.ToList();

                    _ = await Update(entity, false);

                }
            }
        }
        public async Task<PlannedActivityProps> GetModernizeAndReplacePAProperties(Plannedactivities item)
        {
            return new PlannedActivityProps
            {
                plannedLcmDeploymentStatus = _repositoryWrapper.LcmDeploymentStatusRepository
                   .FindByCondition(x => x.Description.ToLower().Replace(" ", "") == "Planned".ToLower().Replace(" ", ""))
                   .Select(x => x.Id).FirstOrDefault(),
                budgetPlanningDeliveryStatusId = _repositoryWrapper.DeliveryStatus.FindByCondition(x => x.Deliverystatus == "Budget Planning")
                            .Select(x => x.Deliverystatusid).FirstOrDefault(),
                newSolutionPATypeId = _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x =>
                x.Rulelinkeddc == (int)PlannedActivityResourceEnum.New_Solution)
                .Select(x => x.Plannedactivityresourceid).FirstOrDefault(),
                noBudgetAvailabilityId = _repositoryWrapper.BudgetAvailability
                .FindByCondition(x => x.Description.ToLower().Replace(" ", "") == "NO".ToLower().Replace(" ", "")).Select(x => x.Budgetavailabilityid)
                .FirstOrDefault(),
                planningActivityStatusId = _repositoryWrapper.PlanningActivityStatus
                .FindByCondition(x => x.Planningactivitystatus.ToLower().Replace(" ", "") == "PROPOSED".ToLower().Replace(" ", ""))
                .Select(x => x.Planningactivitystatusid).FirstOrDefault(),
                activityStatusId = _repositoryWrapper.ActivityStatus
                .FindByCondition(x => x.Activitystatus.ToLower().Replace(" ", "") == "In Planning".ToLower().Replace(" ", ""))
                .Select(x => x.Activitystatusid).FirstOrDefault(),
                responsibilityPhaseId = _repositoryWrapper.ResponsibilityPhase
                .FindByCondition(x => x.Responsibilityphase.ToLower().Replace(" ", "") == "Engineering".ToLower().Replace(" ", ""))
                .Select(x => x.Responsibilityphaseid).FirstOrDefault(),
                lcmExists = _repositoryWrapper.Lcmengineering
            .FindByCondition(x => x.Designcomponentid == item.Designcomponentid && x.Opcoid == item.Opcoid).FirstOrDefault(),
            };
        }
        public async Task<Plannedactivities> SetPlannedActivityWithStaticProps(Lcmengineering lcmengineering, PlannedActivityProps props, short? successorPAResourceId)
        {
            int nextYear = DateTime.Now.Year + 1;
            short riskId = _repositoryWrapper.Risk
                        .FindByCondition(r => r.Description.ToLower().Replace(" ", "") == "Low - Enables Future Ready - Capability, Best Network, Secure".ToLower().Replace(" ", ""))
                        .Select(r => r.Riskid)
                        .FirstOrDefault();

            return new Plannedactivities
            {
                Lcmengineeringid = lcmengineering.Lcmengineeringid,
                Deliverystatusid = props.budgetPlanningDeliveryStatusId,
                Plannedactivityresourceid = successorPAResourceId,
                Localapproval = "NO",
                Budgetavailabilityid = props.noBudgetAvailabilityId,
                Planningactivitystatusid = props.planningActivityStatusId,
                Activitystatusid = props.activityStatusId,
                Plannedimplementationyear = DateTime.Now.Month <= 3 ? short.Parse(DateTime.Now.Year.ToString()) : short.Parse(nextYear.ToString()),
                Plannedcompletion = DateTime.Now.Month <= 3 ? new DateTime(DateTime.Now.Year, 3, 31) : new DateTime(nextYear, 3, 31),
                Responsibilityphaseid = props.responsibilityPhaseId,
                Riskengineeringnotes = "Low",
                Riskoperationalnotes = "Low",
                Engineeringriskid = riskId != null && riskId != 0 ? riskId : null,
                Operationalriskid = riskId != null && riskId != 0 ? riskId : null,
                Isnewservicearchitecture = false,
                Isreplacementexistingsolution = false,
            };
        }

        private async Task CreateNewLcmAndPlannedActivity(Plannedactivities item)
        {
            Lcmengineering originalLCM = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == item.Lcmengineeringid)?.FirstOrDefault();

            short modernizedPAResourceId = _repositoryWrapper.PlannedActivityResourceRepository
                 .FindByCondition(x => x.Rulelinkeddc ==
                 (int)PlannedActivityResourceEnum.Modernize_System_incl_virtualization)
                 .Select(x => x.Plannedactivityresourceid).FirstOrDefault();
            short replacePAResourceId = _repositoryWrapper.PlannedActivityResourceRepository
                .FindByCondition(x => x.Rulelinkeddc ==
                (int)PlannedActivityResourceEnum.Replace_Solution_Change_Equipment_Manufacturer)
                .Select(x => x.Plannedactivityresourceid).FirstOrDefault();
            if (item.Plannedactivityresourceid == modernizedPAResourceId || item.Plannedactivityresourceid == replacePAResourceId)
            {
                PlannedActivityProps pa = GetModernizeAndReplacePAProperties(item).Result;
                if (pa.lcmExists == null)
                {
                    Task<Lcmengineering> newlyCreatedLcm = CreateLcmAndPA(0, 0, item);
                    if (newlyCreatedLcm != null && newlyCreatedLcm.Result != null)
                    {
                        newlyCreatedLcm.Result.Lcmdeploymentstatusid = pa.plannedLcmDeploymentStatus;
                    }

                }
                else
                {
                    pa.lcmExists.Lcmdeploymentstatusid = originalLCM.Lcmdeploymentstatusid;
                    _repositoryWrapper.Lcmengineering.Update(pa.lcmExists);
                    await _repositoryWrapper.SaveAsync();
                    AddDesignAspectAndPAForLCM(pa.lcmExists, item);

                }
            }
            else
            {
                _ = await CreateLcmAndPA(0, 0, item);
            }
        }
        private async Task<ResultDto> UpdateBase(LcmEngineeringDtoUpdate dto, bool? forced)
        {
            try
            {
                ////Start of LCM R8 - Part 3 requirements to update overridden resource keys  ////Updating the ResourceKey in Resourcekey Master table
                Lcmengineering existingLCMRecord = _repositoryWrapper.Lcmengineering.FindByCondition(
                                        x => x.Lcmengineeringid == dto.LcmEngineeringId).FirstOrDefault();

                if (existingLCMRecord.Buildbagid != dto.BuildBagId)
                {
                    await _designComponentFamilyLifeCycleManager.setResourceKeyNotInUseAndAddDcfLifeCycleEntry
                    (dto.OpCoId, dto.DesignComponentId, dto.DesignComponentFamilyid, 1, existingLCMRecord.Buildbagid);

                    string dcfElementNAme = ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 1).Text;
                    await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforComponent(dto.OpCoId, dto.DesignComponentId, dto.BuildBagId);



                }

                #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
                int modernizeRuleLinkedDcId = 0;
                short assetDeploymentInserviceStatusId = 0;
                int modernizeSettingPaRule = 0; ///This is used to Split the Asset without Status Change
                if (dto.PlannedActivityDto != null)
                {
                    Plannedactivityresources getPASettingPlan = _repositoryWrapper.PlannedActivityResourceRepository
                        .FindByCondition(x => x.Rulelinkeddc == (int)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network && dto.PlannedActivityDto.Select(x => x.PlannedActivityResourceId)
                        .Contains(x.Plannedactivityresourceid))
                        .Include(x => x.SettingsupdateplannedactivityPlannedactivityresource
                        .Where(x => dto.PlannedActivityDto.Select(y => y.DeliveryStatusId)
                        .Contains(x.Deliverystatusid)))
                        .FirstOrDefault();
                    if (getPASettingPlan != null)
                    {
                        modernizeRuleLinkedDcId = Convert.ToInt16(getPASettingPlan?.Rulelinkeddc);
                        modernizeSettingPaRule = Convert.ToInt16(getPASettingPlan?.SettingsupdateplannedactivityPlannedactivityresource.FirstOrDefault()?.Rule);

                    }
                }

                #endregion

                // Check if the ResourceKey got updated and call to update the DCFLifecycle.
                if (existingLCMRecord != null)
                {
                    if (existingLCMRecord.Resourcekey != dto.ResourceKey)
                    {
                        dto.PreviousResourceKey = existingLCMRecord.Resourcekey;

                        string[] LcmResourcKey = dto.ResourceKey.Split("_");
                        if (LcmResourcKey.Length > 1)
                        {
                            Designcomponents dc = _repositoryWrapper.DesignComponent
                           .FindByCondition(x => x.Designcomponentid == dto.DesignComponentId)
                           .FirstOrDefault();

                            string dcName = dc.ToDesignComponentName(_repositoryWrapper);

                            ResourceKeyMasterDto dtoresourceKeyMaster = new()
                            {
                                DcfId = dc.Designcomponentfamilyid,
                                OpCoId = dto.OpCoId,
                                ResourceTypesId = (int)ResourceTypesKey.Lcm,
                                ResourceKey = LcmResourcKey[0].ToString()
                            };
                            _ = await _resourceKeyMasterManager.CreateOrUpdateResourceKey(dtoresourceKeyMaster);
                        }

                        _ = _designComponentFamilyLifeCycleManager.UpdateDCFLifecycleForLCMResourceKey(existingLCMRecord.Resourcekey, dto.ResourceKey);
                    }
                }
                ////End of LCM R8 - Part 3 requirements to update overridden resource keys

                LcmEngineering model = _mapper.Map<LcmEngineering>(dto);
                Lcmengineering entity = LCMEngineeringMapper.SetLcmEngineeringMapper(model);

                if (!string.IsNullOrEmpty(dto.ReasonForNoPlan) && !string.IsNullOrEmpty(dto.CommentOnProjectStatus))
                {
                    Lcmancillarydata lcmancillarydata = new()
                    {
                        Reasonfornoplan = dto.ReasonForNoPlan,
                        Commentonprojectstatus = dto.CommentOnProjectStatus
                    };
                    entity.Lcmancillarydata.Add(lcmancillarydata);
                }

                entity = await SetLcmValue(entity);
                short? FetchNopAResourcesId = _repositoryWrapper.PlannedActivityResourceRepository.FindByCondition(x =>
                               x.Plannedactivityresourceid == entity.PlannedactivitiesLcmengineering.Select(p => p.Plannedactivityresourceid).FirstOrDefault()
                              && x.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity).FirstOrDefault()?.Plannedactivityresourceid;

                if (FetchNopAResourcesId is not null and not 0)
                {
                    isNoPa = true;
                }

                ICollection<Plannedactivities> nOPaplannedActivities = entity.PlannedactivitiesLcmengineering;

                if (isNoPa)
                {
                    entity.PlannedactivitiesLcmengineering = null;
                }

                List<Networkelementsasplanned> assets = _repositoryWrapper.NetworkElementAsPlanned
                                  .FindByCondition(x => x.Opcoid == dto.OpCoId && x.Designcomponentid == dto.DesignComponentId)
                                  .ToList();
                #region // Asset status #412 Decommission Flow Implementation  - Decommission Flow April 17 2024  
                //Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
                assetDeploymentInserviceStatusId = (short)LCMEngineeringRulesExtension.GetAssetDeploymentStatusBasedOnParameter(
                   ConstantValueFilter.InService, _repositoryWrapper)?
                    .FirstOrDefault().Key;

                var assetDeploymentStatus = await _dropdownDataServiceManager.GetAssetDeploymentStatus();
                var inservice_Decommission_AssetDeploymentId = assetDeploymentStatus
                    .Where(x => ConstantValueFilter.assetDeploymentStatusForArchivePa.Any(y => y == x.Value.Replace(" ", "").ToLower()))
                    .Select(t => t.Key).ToList();

                string settingsPaAssetDeploymentStatusId = string.Empty;
                if (entity.PlannedactivitiesLcmengineering != null && entity.PlannedactivitiesLcmengineering.Count() > 0 && isNoPa == false)
                {
                    Plannedactivities plannedActivityDetail = entity.PlannedactivitiesLcmengineering.FirstOrDefault();
                    settingsPaAssetDeploymentStatusId = FetchAssetDeploymentStatusFromSettingPA(plannedActivityDetail.Plannedactivityresourceid, plannedActivityDetail.Deliverystatusid);

                }
                IEnumerable<NetworkElementAssociated> dtoAsset = dto?.NetworkElementAssociateds;
                foreach (Networkelementsasplanned asset in assets)
                {
                    var existingAssetDeploymentStatusId = asset.Deploymentstatusid;
                    if (!string.IsNullOrEmpty(settingsPaAssetDeploymentStatusId))
                    {
                        if (modernizeRuleLinkedDcId == (int)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network)
                        {
                            if (asset.Deploymentstatusid != assetDeploymentInserviceStatusId && modernizeSettingPaRule == 1)
                            {
                                asset.Deploymentstatusid = asset.Deploymentstatusid;
                            }
                            else if (modernizeSettingPaRule == 0)
                            {
                                asset.Deploymentstatusid = Convert.ToInt16(settingsPaAssetDeploymentStatusId);
                            }
                        }
                        else
                        {
                            asset.Deploymentstatusid = Convert.ToInt16(settingsPaAssetDeploymentStatusId);
                        }
                    }
                    asset.Buildbagid = entity.Buildbagid;
                    asset.Lcmengineeringid = entity.Lcmengineeringid;
                    if (dtoAsset != null && dtoAsset.Count() > 0)
                    {
                        NetworkElementAssociated getAssetIsFinalAssetInDto = dtoAsset.Where(x => x.Id == asset.Networkelementasplannedid).FirstOrDefault();
                        asset.Isfinalasset = (getAssetIsFinalAssetInDto != null) ? getAssetIsFinalAssetInDto.IsFinalAsset : asset.Isfinalasset;

                    }
                    _repositoryWrapper.NetworkElementAsPlanned.Update(asset);

                    if (existingAssetDeploymentStatusId != asset.Deploymentstatusid &&
                        inservice_Decommission_AssetDeploymentId.Any(x => x == asset.Deploymentstatusid))
                        await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(asset.Networkelementasplannedid);



                }

                _repositoryWrapper.Save();
                await _repositoryWrapper.ClearTracker();
                #endregion

                #region 1062 InsertOrUpdateAncillary

                if (isNoPa)
                {
                    //await InsertUpdateAncillaryData(entity.Lcmengineeringid, 0, false, entity?.Lcmancillarydata?.FirstOrDefault());
                    await InsertUpdateAncillaryDatas(entity.Lcmengineeringid, 0, false, entity?.Lcmancillarydata?.FirstOrDefault(), isNoPa);

                }


                #endregion

                if (forced == true)
                {
                    entity.Deleted = false;
                    entity.Deletiondate = null;
                }

                if (isNoPa)
                {
                    entity.PlannedactivitiesLcmengineering = nOPaplannedActivities;
                }


                #region  //Ticket 1810 -Design Components Not Getting Listed in PROD
                var transitDcEntry = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == entity.Designcomponentid && p.Visibleflag == false).FirstOrDefault();
                if (transitDcEntry != null)
                {
                    transitDcEntry.Visibleflag = true;
                    _repositoryWrapper.DesignComponent.Update(transitDcEntry);
                    await _repositoryWrapper.SaveAsync();

                }
                #endregion

                foreach (Plannedactivities plannedActivity in entity.PlannedactivitiesLcmengineering)
                {

                    if (isNoPa)
                    {
                        Dictionary<short, string> activitystatusid = _dropdownDataServiceManager.GetActivityStatusDic(false, ConstantValueFilter.paActivityStatus).Result;
                        Dictionary<short, string> planningactivitystatusid = _dropdownDataServiceManager.GetPlanningActivityStatusDic(false, ConstantValueFilter.paPlanningActivityStatus).Result;
                        plannedActivity.Activitystatusid = activitystatusid.Select(x => x.Key).FirstOrDefault();
                        plannedActivity.Planningactivitystatusid = planningactivitystatusid.Select(x => x.Key).FirstOrDefault();
                        plannedActivity.Projectstatus = Outputs.NotRequested;
                    }
                    plannedActivity.Archived = false;
                    plannedActivity.Designcomponentid =
                        plannedActivity.Designcomponentid == 0 ? null : plannedActivity.Designcomponentid;
                    plannedActivity.Deliverystatusid =
                        plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatusid;
                    plannedActivity.Responsibilityphaseid =
                        plannedActivity.Responsibilityphaseid == 0 ? null : plannedActivity.Responsibilityphaseid;
                    plannedActivity.Deliverystatus = plannedActivity.Deliverystatusid == 0 ? null : plannedActivity.Deliverystatus;
                    plannedActivity.Lcmengineeringid = entity.Lcmengineeringid;
                    plannedActivity.Archived = plannedActivity.Archived == null ? false : plannedActivity.Archived;
                    plannedActivity.Budgetavailabilityid =
                                plannedActivity.Budgetavailabilityid == 0 ? null : plannedActivity.Budgetavailabilityid;
                    plannedActivity.Planningriskid =
                                plannedActivity.Planningriskid == 0 ? null : plannedActivity.Planningriskid;
                    plannedActivity.Engineeringriskid =
                                plannedActivity.Engineeringriskid == 0 ? null : plannedActivity.Engineeringriskid;
                    plannedActivity.Operationalriskid =
                                plannedActivity.Operationalriskid == 0 ? null : plannedActivity.Operationalriskid;
                    plannedActivity.Plannedactivitycategoryid =
                                plannedActivity.Plannedactivitycategoryid == 0 ? null : plannedActivity.Plannedactivitycategoryid;
                    plannedActivity.Programid =
                                 plannedActivity.Programid == 0 ? null : plannedActivity.Programid;

                    plannedActivity.Buildbagid = plannedActivity.Buildbagid;
                    if (plannedActivity.Designcomponentid != 0)
                    {
                        #region  //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                        var dcEntry = _repositoryWrapper.DesignComponent.FindByCondition(p => p.Designcomponentid == plannedActivity.Designcomponentid).FirstOrDefault();
                        plannedActivity.Designcomponentfamilyid = dcEntry?.Designcomponentfamilyid;
                        if (dcEntry?.Visibleflag == false)
                        {
                            dcEntry.Visibleflag = true;
                            _repositoryWrapper.DesignComponent.Update(dcEntry);
                        }
                    }

                }
                _repositoryWrapper.Save();
                await _repositoryWrapper.ClearTracker();
                #endregion

                entity.Reasoncheckboxresourcelcmengineeringhardware = null;
                entity.Reasoncheckboxresourcelcmengineeringsoftware = null;

                entity.Buildbagid = entity.Buildbagid;
                _repositoryWrapper.Lcmengineering.Update(entity);

                IEnumerable<long> ids = entity.PlannedactivitiesLcmengineering.Select(s => s.Plannedactivityid);

                await _repositoryWrapper.SaveAsync();
                List<Plannedactivities> plannedActivitiestoDelete = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Archived != true &&
                        x.Lcmengineeringid == dto.LcmEngineeringId && !ids.Contains(x.Plannedactivityid)
                    , false, false).Include(x => x.Budgetprojecttrackers).Include(x => x.Projectsplan).ThenInclude(x => x.Projectplanaudit).ToList();

                foreach (Plannedactivities plannedActivity in plannedActivitiestoDelete)
                {
                    Deliverytrackings deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == plannedActivity.Plannedactivityid).FirstOrDefault();
                    if (deliveryTracking != null)
                    {
                        _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);
                    }
                    foreach (var deletepp in plannedActivity.Projectsplan)
                    {
                        foreach (var deleteppa in deletepp.Projectplanaudit)
                        {
                            _repositoryWrapper.ProjectPlanAuditRepository.DeleteDeep(deleteppa);
                        }
                        _repositoryWrapper.ProjectPlanRepository.DeleteDeep(deletepp);
                    }
                    // add delete funtion for projectplan 

                    //Ticket 897 
                    _ = await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(plannedActivity);

                    if (plannedActivity.Budgetprojecttrackers != null && plannedActivity.Budgetprojecttrackers.Count > 0)
                    {
                        if (plannedActivity.Budgetprojecttrackers.FirstOrDefault() != null)
                        {
                            _repositoryWrapper.BudgetProjectTrackersRepository.DeleteDeep(plannedActivity.Budgetprojecttrackers.FirstOrDefault());
                        }

                    }
                    _repositoryWrapper.PlannedActivity.DeleteDeep(plannedActivity);

                    if (isNoPa)
                    {
                        Lcmancillarydata lcmancillarydata = new()
                        {
                            Reasonfornoplan = null,
                            Lcmengineeringid = entity.Lcmengineeringid,
                            Commentonprojectstatus = null
                        };

                        //await InsertUpdateAncillaryData(entity.Lcmengineeringid, 0, false, lcmancillarydata, true);
                        await InsertUpdateAncillaryDatas(entity.Lcmengineeringid, 0, false, lcmancillarydata, isNoPa);

                    }

                }
                await _repositoryWrapper.SaveAsync();

                int prodNodesCount = entity.CountNetworkElementReleated(false, _repositoryWrapper);
                if (prodNodesCount > 0)
                {
                    DesignAspectDtoModel designAspectObject = new()
                    {
                        OpCoId = entity.Opcoid,
                        DesignComponentFamilyId = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                   .Select(x => x.Designcomponentfamilyid.Value).FirstOrDefault(),
                        UsedNetworkFunctionsIds = new List<int>(),
                        SupportedServicesIds = new List<int>()
                    };
                    _ = await _designAspectManager.Add(designAspectObject);
                }
                bool archivedPA = false;

                foreach (Plannedactivities item in entity.PlannedactivitiesLcmengineering)
                {

                    if (item.Plannedactivityid == 0)
                    {
                        Plannedactivities plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, item.Designcomponentid, entity.Opcoid, entity.Designcomponentid, item.Plannedactivityresourceid);
                        //Ticket 658 Dev - #622 - Release details unknown - Duplicate PA's
                        if (plannedActivityExist != null && dto.IsReleaseDetailUnKnown == false && item.Ispareleasedetailunknown == false)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExists,
                                Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                            };
                        }
                        else
                        {
                            if (isNoPa)
                            {
                                item.Archived = false;

                                _repositoryWrapper.PlannedActivity.Create(item);
                                await _repositoryWrapper.SaveAsync();
                            }
                            else
                            {
                                bool getDeliveryplanavailable = item.Deliveryplanavailable;
                                item.Archived = false;

                                _repositoryWrapper.PlannedActivity.Create(item);
                                await _repositoryWrapper.SaveAsync();

                                // Face issue - Need this code
                                item.Deliveryplanavailable = getDeliveryplanavailable;
                                _repositoryWrapper.PlannedActivity.Update(item);
                                await _repositoryWrapper.SaveAsync();

                                if (getDeliveryplanavailable)
                                {
                                    var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                    if (mileStoneStatus.isSuccess)
                                    {
                                        // We should pass inservice and planned asset count only 
                                        var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                        var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                        int assetCount = assets.Where(x => x.Deploymentstatusid == getAssetInserviceID && x.Environmentid == getProductionEnvrionmentId).Count();

                                        var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, assetCount);
                                        await _deliveryTrackingManager.Add(deliveryTracking);
                                    }

                                }
                                var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                                foreach (var plan in addProjectPlanEntity)
                                {
                                    _repositoryWrapper.ProjectPlanRepository.Create(plan);
                                    await _repositoryWrapper.SaveAsync();

                                    await _commonManager.CreateOrUpdateProjectPlanAudit(plan.Projectsplanid, null, item.Plannedcompletion.Value.ToShortDateString(), 1, _repositoryWrapper);

                                }

                                await _repositoryWrapper.ClearTracker();
                            }

                        }

                    }
                    else
                    {
                        Plannedactivities plannedActivityExist = PlannedActivityExists(item.Plannedactivityid, item.Designcomponentid, entity.Opcoid, entity.Designcomponentid, item.Plannedactivityresourceid);
                        //Ticket 658 Dev - #622 - Release details unknown - Duplicate PA's
                        if (plannedActivityExist != null && dto.IsReleaseDetailUnKnown == false && item.Ispareleasedetailunknown == false)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryUpdateExists,
                                Data = new { id = plannedActivityExist.Plannedactivityid, orphanDeleted = true }
                            };
                        }
                        else
                        {
                            item.Archived = false;
                            //need to call function here
                            var pacompletiondate = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid).Select(x => x.Plannedcompletion).FirstOrDefault();
                            if (pacompletiondate != item.Plannedcompletion)
                                await _commonManager.UpdateProjectPlanDateForLastDeliveryStatusOfMS(item, item.Plannedcompletion, (int)MilestoneStatusEnum.MS4, _repositoryWrapper, true);

                            _repositoryWrapper.PlannedActivity.Update(item);

                            if (item.Deliveryplanavailable && isNoPa == false)
                            {
                                var mileStoneStatus = await _commonManager.CalculateMSDuration(item, _repositoryWrapper);
                                if (mileStoneStatus.isSuccess)
                                {
                                    // We should pass inservice and planned asset count only 
                                    var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                                    var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                                    int assetCount = assets.Where(x => x.Deploymentstatusid == getAssetInserviceID && x.Environmentid == getProductionEnvrionmentId).Count();

                                    var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, item, assetCount);
                                    await _deliveryTrackingManager.Add(deliveryTracking);
                                }

                            }
                            var addProjectPlanEntity = await _commonManager.AddProjectPlan(item, _repositoryWrapper);

                            foreach (var plan in addProjectPlanEntity)
                            {
                                _repositoryWrapper.ProjectPlanRepository.Create(plan);

                                var ppcAudit = plan;
                            }
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                        }
                    }
                    await _repositoryWrapper.SaveAsync();
                    int? settingRuleElementCount = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering && x.Plannedactivityresourceid == item.Plannedactivityresourceid && x.Deliverystatusid == item.Deliverystatusid)
                        .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Ruleelementcount;


                    if (settingRuleElementCount == (int)RuleElementCountEnum.RolloutComplete && (entity.CountNetworkElementReleated(false, _repositoryWrapper) > 0 || entity.CountNetworkElementReleated(true, _repositoryWrapper) > 0))
                    {
                        archivedPA = true;
                        item.Archived = true;
                        _repositoryWrapper.PlannedActivity.Update(item);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                        entity.Archived = true;  // var resourceKey = entity.Resourcekey.Split("_");
                        entity.Resourcekey = entity.Resourcekey; // April 17 2024 -  resourceKey[0] + "_FF";

                        _ = await RolloutPlannedActivityFromDDl(item);
                        #region //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries                         
                        #endregion

                        Plannedactivities PAEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid, true, false)
                       .Include(x => x.Plannedactivityresource)
                       .FirstOrDefault();
                        if (PAEntity.Plannedactivityresource.Rulelinkeddc is not ((int)PlannedActivityResourceEnum.New_NFxI_Solution)
                         and not ((int)PlannedActivityResourceEnum.New_System_HW_SW_Solution))
                        {
                            _ = await ArchiveDesginAspectCasePARolledout(entity, item);
                        }
                        #region // Asset status #412 Decommission Flow Implementation  - Decommission Flow April 17 2024
                        //Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
                        if (PAEntity.Plannedactivityresource.Rulelinkeddc is ((int)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network)
                            or ((int)PlannedActivityResourceEnum.Decommission_Service_Node))
                        {
                            await _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforNewSolution(entity, PAEntity, true);
                            //await InsertUpdateAncillaryData((long)item.Lcmengineeringid, 0);

                            #region// Change DCF Asset record activityDescription 
                            string dcfAssetActivitydescription = "Asset Removed Through Modernized";
                            if (PAEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Decommission_Service_Node)
                            {
                                dcfAssetActivitydescription = "Asset Decommissioned";
                            }
                            #endregion

                            foreach (Networkelementsasplanned asset in assets)
                            {
                                _ = _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(asset, dcfAssetActivitydescription);
                            }
                        }
                        #endregion

                    }
                    else if (isNoPa == false)
                    {
                        Plannedactivities PAEntinty = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid, true, false)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringeduspoc)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmoperationalcontracts)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringhardware)
                       .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringsoftware)
                        .FirstOrDefault();

                        #region //Ticket 687 EDU/Sub domain SPOC details are not copied to new LCM while doinf SW upgrade/modernize flow

                        if (entity.Lcmengineeringsubdomainspoc != null
                            && entity.Lcmengineeringsubdomainspoc.Count() != 0)
                        {
                            PAEntinty.Lcmengineering.Lcmengineeringsubdomainspoc = entity.Lcmengineeringsubdomainspoc;
                        }

                        if (entity.Lcmengineeringeduspoc != null
                          && entity.Lcmengineeringeduspoc.Count() != 0)
                        {
                            PAEntinty.Lcmengineering.Lcmengineeringeduspoc = entity.Lcmengineeringeduspoc;
                        }

                        #endregion

                        await CreateNewLcmAndPlannedActivity(PAEntinty);

                    }
                    await _repositoryWrapper.SaveAsync();

                    #region ClusterLevelPa

                    var clusterDto = dto?.PlannedActivityDto?.Where(x => x.DesignComponentId == item.Designcomponentid)?.FirstOrDefault()?.infraClusterClusterUpgradeUpsertDto;
                    if (clusterDto != null)
                    {
                        var rulelLinkedDc = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == item.Plannedactivityid).Include(x => x.Plannedactivityresource).FirstOrDefault().Plannedactivityresource?.Rulelinkeddc;
                        await _plannedActivityManager.AddOrUpdateClusterLevelData(clusterDto, item.Plannedactivityid, rulelLinkedDc);
                    }
                    #endregion
                    await _commonManager.CreateOrUpdateBptreport(item.Plannedactivityid, entity.Designcomponentid, (long)item.Designcomponentid);

                }


                if (archivedPA == true && isNoPa == false)
                {
                    await _repositoryWrapper.ClearTracker();
                    Plannedactivities firstArchivedPA = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == entity.Lcmengineeringid && x.Archived == true).FirstOrDefault();
                    if (firstArchivedPA != null)
                    {
                        _ = await MoveActivePAs(entity.Lcmengineeringid, firstArchivedPA.Designcomponentid.Value);

                    }

                }
                List<Lcmengineeringsubdomainspoc> LcmengineeringSubDomainSpocsRelations =
                 _repositoryWrapper.LcmEngineeringSubDomainSpoc.
                     FindByCondition(x => x.Lcmengineeringid == dto.LcmEngineeringId).ToList();

                foreach (Lcmengineeringsubdomainspoc toDelete in LcmengineeringSubDomainSpocsRelations)
                {
                    _repositoryWrapper.LcmEngineeringSubDomainSpoc.DeleteDeep(toDelete);
                }

                List<Lcmengineeringeduspoc> LcmengineeringEduSpocRelations =
                    _repositoryWrapper.LcmEngineeringEduSpoc.FindByCondition(x => x.Lcmengineeringid == dto.LcmEngineeringId).ToList();

                foreach (Lcmengineeringeduspoc toDelete in LcmengineeringEduSpocRelations)
                {
                    _repositoryWrapper.LcmEngineeringEduSpoc.DeleteDeep(toDelete);
                }

                List<Lcmoperationalcontracts> LcmengineeringOpertionalContractRelations =
                  _repositoryWrapper.LCMOperationalContracts.
                      FindByCondition(x => x.Lcmid == dto.LcmEngineeringId).ToList();


                foreach (Lcmoperationalcontracts toDelete in LcmengineeringOpertionalContractRelations)
                {
                    _repositoryWrapper.LCMOperationalContracts.DeleteDeep(toDelete);
                }

                dto.EduSpocIds = dto.EduSpocIds?.Distinct().ToList();
                dto.SubDomainSpocIds = dto.SubDomainSpocIds?.Distinct().ToList();
                dto.OperationContractsIds = dto.OperationContractsIds?.Distinct().ToList();
                foreach (Lcmengineeringeduspoc contact in entity.Lcmengineeringeduspoc)
                {
                    if (contact.Lcmengineeringeduspocid == 0)
                    {
                        _repositoryWrapper.LcmEngineeringEduSpoc.Create(contact);
                    }
                    else
                    {
                        _repositoryWrapper.LcmEngineeringEduSpoc.Update(contact);
                    }
                }


                foreach (Lcmengineeringsubdomainspoc contact in entity.Lcmengineeringsubdomainspoc)
                {
                    if (contact.Lcmengineeringsubdomainspocid == 0)
                    {
                        _repositoryWrapper.LcmEngineeringSubDomainSpoc.Create(contact);
                    }
                    else
                    {
                        _repositoryWrapper.LcmEngineeringSubDomainSpoc.Update(contact);
                    }
                }


                foreach (Lcmoperationalcontracts contact in entity.Lcmoperationalcontracts)
                {
                    if (contact.Id == 0)
                    {
                        _repositoryWrapper.LCMOperationalContracts.Create(contact);
                    }
                    else
                    {
                        _repositoryWrapper.LCMOperationalContracts.Update(contact);
                    }
                }

                await _repositoryWrapper.SaveAsync();
                List<Reasoncheckboxresourcelcmengineeringhardware> removeChechbox =
                    _repositoryWrapper.CheckboxResourceLcmEngineeringHardware.FindByCondition(c =>
                        c.Lcmengineeringid == dto.LcmEngineeringId).ToList();
                if (removeChechbox != null)
                {
                    foreach (Reasoncheckboxresourcelcmengineeringhardware hardware in removeChechbox)
                    {
                        _repositoryWrapper.CheckboxResourceLcmEngineeringHardware.DeleteDeep(hardware);
                    }
                }

                if (dto.CheckboxResourceLcmEngineeringHardwares != null)
                {
                    foreach (int x in dto.CheckboxResourceLcmEngineeringHardwares)
                    {
                        _repositoryWrapper.CheckboxResourceLcmEngineeringHardware.Create(
                            ReasonCheckboxResourceLcmEngineeringHardwareMapper.Set(

                            new ReasonCheckboxResourceLcmEngineeringHardware()
                            {
                                LcmEngineeringId = dto.LcmEngineeringId,
                                ReasonCheckboxResourceId = (short)x,
                                //Id = Guid.NewGuid(),
                            }));
                    }
                }

                await _repositoryWrapper.SaveAsync();


                List<Reasoncheckboxresourcelcmengineeringsoftware> removeChechboxSoftwares =
                     _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware.FindByCondition(c =>
                         c.Lcmengineeringid == dto.LcmEngineeringId).ToList();
                if (removeChechboxSoftwares != null)
                {
                    foreach (Reasoncheckboxresourcelcmengineeringsoftware hardware in removeChechboxSoftwares)
                    {
                        _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware.DeleteDeep(hardware);
                    }
                }

                if (dto.CheckboxResourceLcmEngineeringSoftwares != null)
                {
                    foreach (int x in dto.CheckboxResourceLcmEngineeringSoftwares)
                    {
                        _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware.Create(

                            ReasonCheckboxResourceLcmEngineeringSoftwareMapper.Set(
                            new ReasonCheckboxResourceLcmEngineeringSoftware()
                            {
                                LcmEngineeringId = dto.LcmEngineeringId,
                                ReasonCheckboxResourceId = (short)x,
                                // Id = Guid.NewGuid(),
                            }));
                    }
                }

                await _repositoryWrapper.SaveAsync();

                //// Check if the ResourceKey got updated and call to update the DCFLifecycle.
                if (existingLCMRecord != null)
                {
                    if (existingLCMRecord.Resourcekey != dto.ResourceKey)
                    {
                        _designComponentFamilyLifeCycleManager.setResourceKeyStatus(existingLCMRecord.Resourcekey, (int)ResourceTypesKey.Lcm);
                    }
                }
                ////End of LCM R8 - Part 3 requirements to update overridden resource keys
                //Ticket 1292 - Insert/Update/Delete actions in the Asset screen are not updating the LCM Number of Nodes Count and DCF Implementation Value.
                _ = await _commonManager.SetImplementationFlagInDCF(entity.Designcomponentid);

                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = entity.Lcmengineeringid
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Line 666 LCM update main method issue happen: " + e);
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.ErrorValidation,
                };
            }
        }

        public async Task<Lcmengineering> CreateLcmAndPA(int numberOfNodes, int numberOfNodesInLab, Plannedactivities plannedActivity)
        {
            Lcmengineering originalLCM = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == plannedActivity.Lcmengineeringid)?.FirstOrDefault();



            Lcmengineering lcm = await _repositoryWrapper.Lcmengineering.FindByCondition(x =>
               x.Opcoid == plannedActivity.Opcoid && x.Designcomponentid == plannedActivity.Designcomponentid && x.Archived != true, false, false).FirstOrDefaultAsync();
            Lcmengineering lcmEngineer = new();

            Settingsupdateplannedactivity settingUpdatePA = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering
          && x.Plannedactivityresourceid == plannedActivity.Plannedactivityresourceid && x.Deliverystatusid == plannedActivity.Deliverystatusid).Include(x => x.Plannedactivityresource)
          .FirstOrDefault();

            // set default value of lcm deployment status for new lcm                  
            short? InServiceLcmDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
                .FindByCondition(x => x.Description == "In-Service").Select(x => x.Id).FirstOrDefault();
            string settingsPaAssetDeploymentStatusId = string.Empty;

            #region #1658 - Archive LCM - Move Asset PA to Archive
            var assetDeploymentStatus = await _dropdownDataServiceManager.GetAssetDeploymentStatus();
            var inservice_Decommission_AssetDeploymentId = assetDeploymentStatus
                .Where(x => ConstantValueFilter.assetDeploymentStatusForArchivePa.Any(y => y == x.Value.Replace(" ", "").ToLower()))
                .Select(t => t.Key).ToList();
            #endregion

            if (lcm == null)
            {
                if (settingUpdatePA?.Rule is null or 0)
                {
                    return null;
                }
                int CopyRule = settingUpdatePA.Rule;
                Lcmengineering newLcmEntity = new()
                {
                    Opcoid = plannedActivity.Lcmengineering.Opcoid,
                    Designcomponentid = plannedActivity.Designcomponentid ?? 0,
                    Buildbagid = plannedActivity.Buildbagid   // Whether need to pass old LCM bag id or PA BagID?
                };

                //Lcm-R8 Requirements
                if (originalLCM != null && newLcmEntity != null)
                {
                    newLcmEntity.Resourcekey = _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforLCMTransition(originalLCM, newLcmEntity, plannedActivity).Result.Data.ToString();
                }
                if (newLcmEntity.Resourcekey != originalLCM.Resourcekey)
                {
                    _ = await _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforLCMCreated(newLcmEntity);
                }
                string PADeliveryStatus = _repositoryWrapper.DeliveryStatus
                    .FindByCondition(x => x.Deliverystatusid == plannedActivity.Deliverystatusid)
                    .Select(x => x.Deliverystatus)
                    .FirstOrDefault();

                newLcmEntity.Lcmdeploymentstatusid = originalLCM?.Lcmdeploymentstatusid != null ? originalLCM?.Lcmdeploymentstatusid : InServiceLcmDeploymentStatusId;

                newLcmEntity.Productimportanceid = plannedActivity.Lcmengineering.Productimportanceid;

                var result = await AddLcmEntity(newLcmEntity, originalLCM);
                lcmEngineer = (Lcmengineering)result.Data;

                #region Ticket 687 EDU/Sub domain SPOC details are not copied to new LCM while doinf SW upgrade/modernize flow
                if (plannedActivity.Lcmengineering.Lcmengineeringeduspoc != null)
                {
                    foreach (Lcmengineeringeduspoc it in plannedActivity.Lcmengineering.Lcmengineeringeduspoc)
                    {
                        _repositoryWrapper.LcmEngineeringEduSpoc.Create(new Lcmengineeringeduspoc
                        {
                            Lcmengineeringid = lcmEngineer.Lcmengineeringid,
                            Eduspocid = it.Eduspocid,
                            Eduspoc = it.Eduspoc,
                        });
                    }
                    await _repositoryWrapper.SaveAsync();
                }
                if (plannedActivity.Lcmengineering.Lcmengineeringsubdomainspoc != null)
                {
                    foreach (Lcmengineeringsubdomainspoc it in plannedActivity.Lcmengineering.Lcmengineeringsubdomainspoc)
                    {
                        _repositoryWrapper.LcmEngineeringSubDomainSpoc.Create(new Lcmengineeringsubdomainspoc
                        {
                            Lcmengineeringid = lcmEngineer.Lcmengineeringid,
                            Subdomainspocid = it.Subdomainspocid,
                            Subdomainspoc = it.Subdomainspoc
                        });
                    }
                    await _repositoryWrapper.SaveAsync();
                }
                #endregion



                #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
                if (settingUpdatePA.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network)
                {
                    ///Get In-service Asset Deployment Status Id
                    short? inServiceAssetDeploymentStatusId = (short)LCMEngineeringRulesExtension.GetAssetDeploymentStatusBasedOnParameter(
                        "in-service", _repositoryWrapper)?.FirstOrDefault().Key;

                    ///Filter Planned/In commission Assets from Existing LCM Asset and Assigned to New LCM Asset
                    List<Networkelementsasplanned> getNotInserviceAssetsForNewLCM = _repositoryWrapper.NetworkElementAsPlanned.
                        FindByCondition(x => x.Lcmengineeringid == plannedActivity.Lcmengineeringid &&
                     x.Deploymentstatusid != inServiceAssetDeploymentStatusId).ToList();


                    settingsPaAssetDeploymentStatusId = FetchAssetDeploymentStatusFromSettingPA(settingUpdatePA.Plannedactivityresourceid, settingUpdatePA.Deliverystatusid);


                    foreach (Networkelementsasplanned item in getNotInserviceAssetsForNewLCM)
                    {
                        var existingAssetDeploymentStatusId = item.Deploymentstatusid;

                        item.Lcmengineeringid = lcmEngineer.Lcmengineeringid;
                        item.Designcomponentid = lcmEngineer.Designcomponentid;
                        item.Opcoid = (short)lcmEngineer.Opcoid;
                        item.Deploymentstatusid = (short)((!string.IsNullOrEmpty(settingsPaAssetDeploymentStatusId)) ?
                              Convert.ToInt16(settingsPaAssetDeploymentStatusId) :
                            inServiceAssetDeploymentStatusId);

                        _repositoryWrapper.NetworkElementAsPlanned.Update(item);
                        _ = _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(item, "Assest Migrated");

                        if (existingAssetDeploymentStatusId != item.Deploymentstatusid &&
                        inservice_Decommission_AssetDeploymentId.Any(x => x == item.Deploymentstatusid))
                            await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(item.Networkelementasplannedid);
                    }
                    _repositoryWrapper.Save();


                    await _repositoryWrapper.ClearTracker();


                    lcmEngineer.Elementcount = plannedActivity.Lcmengineering.Elementcount;

                    lcmEngineer.Numberofnodes = lcmEngineer.CountNetworkElementReleated(false, _repositoryWrapper); ;
                    lcmEngineer.Numberofnodesinlab = lcmEngineer.CountNetworkElementReleated(true, _repositoryWrapper);

                    //After Filter Planned/In commission Assets from Existing LCM Asset update Node counts Details

                    originalLCM.Numberofnodes = originalLCM.CountNetworkElementReleated(false, _repositoryWrapper);
                    originalLCM.Numberofnodesinlab = originalLCM.CountNetworkElementReleated(true, _repositoryWrapper);

                    _repositoryWrapper.Lcmengineering.Update(originalLCM);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                #endregion
                else
                {

                    lcmEngineer.Elementcount = plannedActivity.Lcmengineering.Elementcount;

                    lcmEngineer.Numberofnodes = numberOfNodes;
                    lcmEngineer.Numberofnodesinlab = numberOfNodesInLab;
                }
                if (CopyRule != (int)CreateRule.CreateAndDoNotCopyOperationalData)
                {
                    if (plannedActivity.Lcmengineering.Lcmoperationalcontracts != null)
                    {

                        foreach (Lcmoperationalcontracts it in plannedActivity.Lcmengineering.Lcmoperationalcontracts)
                        {
                            Lcmoperationalcontracts lcmOprContracts = new() { Lcmid = lcmEngineer.Lcmengineeringid, Operationalcontractid = it.Operationalcontractid };
                            _repositoryWrapper.LCMOperationalContracts.Create(lcmOprContracts);
                        }


                    }

                    lcmEngineer.Warranty = plannedActivity.Lcmengineering.Warranty;
                    lcmEngineer.Softwareendofwarrantydate = plannedActivity.Lcmengineering.Softwareendofwarrantydate;
                    lcmEngineer.Softwaresupportprovider = plannedActivity.Lcmengineering.Softwaresupportprovider;
                    lcmEngineer.Hardwaresupportprovider = plannedActivity.Lcmengineering.Hardwaresupportprovider;
                    lcmEngineer.Softwaresupporttype = plannedActivity.Lcmengineering.Softwaresupporttype;
                    lcmEngineer.Hardwaresupporttype = plannedActivity.Lcmengineering.Hardwaresupporttype;
                    lcmEngineer.Softwareendofsupportcontract = plannedActivity.Lcmengineering.Softwareendofsupportcontract;
                    lcmEngineer.Hardwareendofsupportcontract = plannedActivity.Lcmengineering.Hardwareendofsupportcontract;
                    lcmEngineer.Softwaresupportedid = plannedActivity.Lcmengineering.Softwaresupportedid;
                    lcmEngineer.Hardwaresupportedid = plannedActivity.Lcmengineering.Hardwaresupportedid;

                    lcmEngineer.Reasoncheckboxresourcelcmengineeringhardware = plannedActivity.Lcmengineering
                        .Reasoncheckboxresourcelcmengineeringhardware.Select(x => new Reasoncheckboxresourcelcmengineeringhardware
                        { Lcmengineeringid = newLcmEntity.Lcmengineeringid, Reasoncheckboxresourceid = x.Reasoncheckboxresourceid })
                        .ToList();
                    lcmEngineer.Reasoncheckboxresourcelcmengineeringsoftware = plannedActivity.Lcmengineering
                        .Reasoncheckboxresourcelcmengineeringsoftware.Select(x => new Reasoncheckboxresourcelcmengineeringsoftware()
                        { Lcmengineeringid = newLcmEntity.Lcmengineeringid, Reasoncheckboxresourceid = x.Reasoncheckboxresourceid })
                        .ToList();
                    lcmEngineer.Sparesprovisioned = plannedActivity.Lcmengineering.Sparesprovisioned;
                    lcmEngineer.Renewalinprogress = plannedActivity.Lcmengineering.Renewalinprogress;
                    lcmEngineer.Fullorpartialsupportid = plannedActivity.Lcmengineering.Fullorpartialsupportid;
                    lcmEngineer.Fullorpartialsupporthwid = plannedActivity.Lcmengineering.Fullorpartialsupporthwid;

                }

                _repositoryWrapper.Lcmengineering.Update(lcmEngineer);
                await _repositoryWrapper.SaveAsync();

            }
            else
            {
                if (lcm.Lcmdeploymentstatusid == null)
                {
                    string PADeliveryStatus = _repositoryWrapper.DeliveryStatus
                                   .FindByCondition(x => x.Deliverystatusid == plannedActivity.Deliverystatusid)
                                   .Select(x => x.Deliverystatus)
                                   .FirstOrDefault();

                    lcm.Lcmdeploymentstatusid = originalLCM?.Lcmdeploymentstatusid != null ? originalLCM?.Lcmdeploymentstatusid : InServiceLcmDeploymentStatusId;

                    lcmEngineer = lcm;

                    _repositoryWrapper.Lcmengineering.Update(lcmEngineer);
                    await _repositoryWrapper.SaveAsync();
                }
                else
                {

                    #region Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
                    if (settingUpdatePA.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Modernize_Solution_Successor_Network)
                    {

                        settingsPaAssetDeploymentStatusId = FetchAssetDeploymentStatusFromSettingPA(settingUpdatePA.Plannedactivityresourceid, settingUpdatePA.Deliverystatusid);
                        List<Networkelementsasplanned> asset = _repositoryWrapper.NetworkElementAsPlanned.
                        FindByCondition(x => x.Lcmengineeringid == plannedActivity.Lcmengineeringid).ToList();

                        if (!string.IsNullOrEmpty(settingsPaAssetDeploymentStatusId))
                        {
                            foreach (Networkelementsasplanned item in asset)
                            {
                                var existingAssetDeploymentStatusId = item.Deploymentstatusid;
                                item.Deploymentstatusid =
                                      Convert.ToInt16(settingsPaAssetDeploymentStatusId);
                                _repositoryWrapper.NetworkElementAsPlanned.Update(item);

                                if (existingAssetDeploymentStatusId != item.Deploymentstatusid &&
                 inservice_Decommission_AssetDeploymentId.Any(x => x == item.Deploymentstatusid))
                                    await _networkElementsAsPlannedManager.ArchiveAssetAddNodeAndDecommissionPA(item.Networkelementasplannedid);
                            }
                            _repositoryWrapper.Save();
                        }
                        await _repositoryWrapper.ClearTracker();

                        lcm.Elementcount = plannedActivity.Lcmengineering.Elementcount;

                        lcm.Numberofnodes = lcm.CountNetworkElementReleated(false, _repositoryWrapper); ;
                        lcm.Numberofnodesinlab = lcm.CountNetworkElementReleated(true, _repositoryWrapper);
                        _repositoryWrapper.Lcmengineering.Update(lcm);
                        await _repositoryWrapper.SaveAsync();


                    }
                    #endregion
                    lcmEngineer = lcm;
                }

            }

            AddDesignAspectAndPAForLCM(lcmEngineer, plannedActivity);

            return lcmEngineer;
        }
        #region Ticket 722 Create New LCM - Using Existing Unknown Resource LCM When User Specify the DC and LCM Have More than one PA 
        public async Task<Lcmengineering> CreateLCMAndRealtedTableRecordsForResourceUnknowLCM(Plannedactivities plannedActivity)
        {
            List<Plannedactivities> movetoNewLCMPlannedActivityList = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == plannedActivity.Lcmengineeringid &&
             x.Plannedactivityid != plannedActivity.Plannedactivityid).ToList();

            Lcmengineering lcmEngineer = new();

            if (movetoNewLCMPlannedActivityList != null && movetoNewLCMPlannedActivityList.Count() > 0)
            {
                Lcmengineering originalAssignedLCM = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == plannedActivity.Lcmengineeringid)?.FirstOrDefault();

                Lcmengineering originalLCM = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == plannedActivity.Lcmengineeringid)
                   .Include(x => x.Lcmengineeringeduspoc).Include(x => x.Lcmengineeringsubdomainspoc).Include(x => x.Lcmoperationalcontracts)
                   .Include(x => x.Networkelementsasplanned)
                   .FirstOrDefault();

                short? plannedLcmDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower() == "planned").Select(x => x.Id).FirstOrDefault();

                Lcmengineering newLcmEntity = new();
                newLcmEntity = originalAssignedLCM;
                newLcmEntity.Lcmengineeringid = 0;

                if (originalLCM != null && newLcmEntity != null)
                {
                    newLcmEntity.Resourcekey = _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforLCMTransition(originalLCM, newLcmEntity, plannedActivity).Result.Data.ToString();
                }
                if (newLcmEntity.Resourcekey != originalLCM.Resourcekey)
                {
                    _ = await _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforLCMCreated(newLcmEntity);
                }

                newLcmEntity.Lcmdeploymentstatusid = plannedLcmDeploymentStatusId;
                newLcmEntity.Numberofnodesinlab = 0;
                newLcmEntity.Numberofnodes = 0;
                newLcmEntity.Elementcount = false;

                var result = await AddLcmEntity(newLcmEntity, null, true, true);
                lcmEngineer = (Lcmengineering)result.Data;

                #region Insert  EDU/Sub domain SPOC details 
                if (originalLCM.Lcmengineeringeduspoc != null)
                {
                    foreach (Lcmengineeringeduspoc it in originalLCM.Lcmengineeringeduspoc)
                    {
                        _repositoryWrapper.LcmEngineeringEduSpoc.Create(new Lcmengineeringeduspoc
                        {
                            Lcmengineeringid = lcmEngineer.Lcmengineeringid,
                            Eduspocid = it.Eduspocid,
                            Eduspoc = it.Eduspoc
                        });
                    }
                    await _repositoryWrapper.SaveAsync();
                }
                if (originalLCM.Lcmengineeringsubdomainspoc != null)
                {
                    foreach (Lcmengineeringsubdomainspoc it in originalLCM.Lcmengineeringsubdomainspoc)
                    {
                        _repositoryWrapper.LcmEngineeringSubDomainSpoc.Create(new Lcmengineeringsubdomainspoc
                        {
                            Lcmengineeringid = lcmEngineer.Lcmengineeringid,
                            Subdomainspocid = it.Subdomainspocid,
                            Subdomainspoc = it.Subdomainspoc
                        });
                    }
                    await _repositoryWrapper.SaveAsync();
                }
                #endregion
                await _repositoryWrapper.ClearTracker();

                #region Copy Lcmoperationalcontracts ,Reasoncheckboxresourcelcmengineeringhardware ,Reasoncheckboxresourcelcmengineeringsoftware Table

                if (originalLCM.Lcmoperationalcontracts != null)
                {
                    foreach (Lcmoperationalcontracts item in originalLCM.Lcmoperationalcontracts)
                    {
                        Lcmoperationalcontracts lcmOprContracts = new() { Lcmid = lcmEngineer.Lcmengineeringid, Operationalcontractid = item.Operationalcontractid };
                        _repositoryWrapper.LCMOperationalContracts.Create(lcmOprContracts);
                    }
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                if (plannedActivity.Lcmengineering.Reasoncheckboxresourcelcmengineeringhardware != null)
                {
                    foreach (Reasoncheckboxresourcelcmengineeringhardware item in plannedActivity.Lcmengineering.Reasoncheckboxresourcelcmengineeringhardware)
                    {
                        Reasoncheckboxresourcelcmengineeringhardware reasonResourceLCMHardware = new() { Lcmengineeringid = newLcmEntity.Lcmengineeringid, Reasoncheckboxresourceid = item.Reasoncheckboxresourceid };
                        _repositoryWrapper.CheckboxResourceLcmEngineeringHardware.Create(reasonResourceLCMHardware);
                    }
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                if (plannedActivity.Lcmengineering.Reasoncheckboxresourcelcmengineeringsoftware != null)
                {
                    foreach (Reasoncheckboxresourcelcmengineeringsoftware item in plannedActivity.Lcmengineering.Reasoncheckboxresourcelcmengineeringsoftware)
                    {
                        Reasoncheckboxresourcelcmengineeringsoftware reasonResourceLCMSoftware = new() { Lcmengineeringid = newLcmEntity.Lcmengineeringid, Reasoncheckboxresourceid = item.Reasoncheckboxresourceid };
                        _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware.Create(reasonResourceLCMSoftware);
                    }
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                _repositoryWrapper.Lcmengineering.Update(lcmEngineer);
                await _repositoryWrapper.SaveAsync();

                #endregion

                #region  PlannedActivity           

                foreach (Plannedactivities paItem in movetoNewLCMPlannedActivityList)
                {
                    paItem.Lcmengineeringid = lcmEngineer.Lcmengineeringid;
                    _repositoryWrapper.PlannedActivity.Update(paItem);
                }

                _repositoryWrapper.Save();
                await _repositoryWrapper.ClearTracker();

                #endregion

                #region NetworkElementAsPlanned Table
                List<Networkelementsasplanned> unSelectedAsset = originalLCM.Networkelementsasplanned.Where(x => x.Isfinalasset == false).ToList();

                foreach (Networkelementsasplanned asset in unSelectedAsset)
                {
                    newLcmEntity.Elementcount = true;
                    asset.Lcmengineeringid = lcmEngineer.Lcmengineeringid;
                    asset.Buildbagid = lcmEngineer.Buildbagid;
                    _repositoryWrapper.NetworkElementAsPlanned.Update(asset);

                    _ = _designComponentFamilyLifeCycleManager.GenerateAssetLifeCycleEntryInDCF(asset, "Assest Migrated");
                }
                await _repositoryWrapper.SaveAsync();

                // Update Nodes counts after move the Asset 
                newLcmEntity.Numberofnodes = newLcmEntity.CountNetworkElementReleated(false, _repositoryWrapper);
                newLcmEntity.Numberofnodesinlab = newLcmEntity.CountNetworkElementReleated(true, _repositoryWrapper);
                if (newLcmEntity.Numberofnodes == 0 && newLcmEntity.Numberofnodesinlab == 0)
                {
                    newLcmEntity.Elementcount = false;
                }

                _repositoryWrapper.Lcmengineering.Update(newLcmEntity);
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();


                #endregion
            }

            return lcmEngineer;
        }

        #endregion


        #region  -- 483 Insert/Update ENGUPDATETRACKER = 'Completed', ASSETOUTOFSCOPE='Historical back-up (remediation action completed) column in Ancillary Table when PA is Archived
        // Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow

        public async Task InsertUpdateAncillaryDatas(long oldLcmEngId = 0, long newLcmEngId = 0, bool isModernizeSolutionFlow = false, Lcmancillarydata userAncillaryData = null,
            bool isNoPa = false, short deploystatusId = 0, bool isArchiveLcm = false)
        {
            try
            {
                var exitAncillaryEntity = await _repositoryWrapper.LcmAncillaryData.FindByConditionWithDelete(x => x.Lcmengineeringid == oldLcmEngId).SingleOrDefaultAsync();
                var newLcmAncillaryEntity = await _repositoryWrapper.LcmAncillaryData.FindByConditionWithDelete(x => x.Lcmengineeringid == newLcmEngId).SingleOrDefaultAsync();
                var lcmdeploymentStatus = deploystatusId > 0 ? _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Planned).FirstOrDefault() : null;
                var AssetOutOfScope = (lcmdeploymentStatus != null) ? lcmdeploymentStatus.Id == deploystatusId ? ConstantValueFilter.assetPlannedTobeInserted : ConstantValueFilter.inScope : ConstantValueFilter.inScope;

                if (exitAncillaryEntity != null)
                {
                    if (isArchiveLcm)
                    {
                        // need to update the existing 
                        await _lcmAncillaryDataManager.CreateAndUpdateAncillaryViaLcm(newLcmAncillaryEntity, exitAncillaryEntity, isNoPa, isModernizeSolutionFlow, isArchiveLcm, newLcmEngId, AssetOutOfScope);

                    }
                    else if (exitAncillaryEntity != null && newLcmAncillaryEntity == null && !isNoPa && userAncillaryData == null)
                    {
                        // need to clone ancillary for new Lcm from old lcm 
                        await _lcmAncillaryDataManager.CreateAndUpdateAncillaryViaLcm(newLcmAncillaryEntity, exitAncillaryEntity, isNoPa, isModernizeSolutionFlow, isArchiveLcm, newLcmEngId, AssetOutOfScope);

                    }
                    else if (exitAncillaryEntity != null && newLcmAncillaryEntity != null && userAncillaryData == null)
                    {
                        // in the newLcm para we should pass the exist lcmAncillary data not current ancillary
                        // if already new lcm exist 
                        await _lcmAncillaryDataManager.CreateAndUpdateAncillaryViaLcm(newLcmAncillaryEntity, exitAncillaryEntity, isNoPa, isModernizeSolutionFlow, isArchiveLcm, newLcmEngId, AssetOutOfScope);

                    }
                    else if (isNoPa && exitAncillaryEntity != null && userAncillaryData != null)
                    {
                        // if we have a lcm without PA, when we create a NO pa we need to update two fields from user input
                        await _lcmAncillaryDataManager.CreateAndUpdateAncillaryViaLcm(userAncillaryData, exitAncillaryEntity, isNoPa, isModernizeSolutionFlow, isArchiveLcm, newLcmEngId, AssetOutOfScope);
                    }

                }
                else if (exitAncillaryEntity == null)
                {
                    if (isNoPa == true && userAncillaryData != null)
                    {
                        // need to create noPa ancillary 
                        await _lcmAncillaryDataManager.CreateAndUpdateAncillaryViaLcm(newLcmAncillaryEntity, userAncillaryData, isNoPa, isModernizeSolutionFlow, isArchiveLcm, oldLcmEngId, AssetOutOfScope);

                    }
                    else if (isModernizeSolutionFlow == true)
                    {
                        // need to create modernize ancillary
                    }
                    else if (newLcmAncillaryEntity == null && exitAncillaryEntity == null && userAncillaryData == null)
                    {
                        //need to create new ancillary for new lcm from UI lcm screen
                        await _lcmAncillaryDataManager.CreateAndUpdateAncillaryViaLcm(newLcmAncillaryEntity, exitAncillaryEntity, isNoPa, isModernizeSolutionFlow, isArchiveLcm, oldLcmEngId, AssetOutOfScope);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }

        }

        #endregion
        /// <summary>
        /// Create DA and PA for LCM
        /// </summary>
        /// <param name="lcmEngineer"></param>
        /// <param name="plannedActivity"></param>
        public async void AddDesignAspectAndPAForLCM(Lcmengineering lcmEngineer, Plannedactivities plannedActivity)
        {

            _ = await CreateDesignAspectWithOPCOAndDCF(lcmEngineer.Opcoid, plannedActivity.Designcomponentfamilyid);

            Settingsupdateplannedactivity relatedSettingUpdate = _repositoryWrapper.SettingsUpdatePlannedActivity
                           .FindByCondition(x => x.Plannedactivityresourceid == plannedActivity.Plannedactivityresourceid && x.Deliverystatusid == plannedActivity.Deliverystatusid && x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering)
                           .FirstOrDefault();

            if (relatedSettingUpdate != null && relatedSettingUpdate.Ruleforsuccessorplannedactivitycreation != null && relatedSettingUpdate.Ruleforsuccessorplannedactivitycreation.Value == true)
            {
                PlannedActivityProps pa = GetModernizeAndReplacePAProperties(plannedActivity).Result;
                short? relatedSettingUpdateSuccessorPAResourceId = relatedSettingUpdate.Successorplannedactivityresourceid;
                bool isPaExist = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == lcmEngineer.Lcmengineeringid && x.Plannedactivityresourceid == relatedSettingUpdateSuccessorPAResourceId).Any();
                if (!isPaExist)
                {
                    Plannedactivities newPlannedActivity = SetPlannedActivityWithStaticProps(lcmEngineer, pa, relatedSettingUpdateSuccessorPAResourceId).Result;
                    newPlannedActivity.Opcoid = lcmEngineer.Opcoid;
                    newPlannedActivity.Lcmengineeringid = lcmEngineer.Lcmengineeringid;
                    newPlannedActivity.Designcomponentfamilyid = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == plannedActivity.Designcomponentid)?.FirstOrDefault()?.Designcomponentfamilyid;

                    _repositoryWrapper.PlannedActivity.Create(newPlannedActivity);
                    await _repositoryWrapper.SaveAsync();

                    if (newPlannedActivity.Deliveryplanavailable)
                    {
                        var mileStoneStatus = await _commonManager.CalculateMSDuration(newPlannedActivity, _repositoryWrapper);
                        if (mileStoneStatus.isSuccess)
                        {
                            var assets = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == lcmEngineer.Opcoid && x.Designcomponentid == lcmEngineer.Designcomponentid).ToListAsync();
                            // We should pass inservice and planned asset count only 
                            var getProductionEnvrionmentId = _repositoryWrapper.Environment.FindByCondition(x => x.Environment.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.production).FirstOrDefault().Environmentid;
                            var getAssetInserviceID = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.Trim().ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault().Deploymentstatusid;
                            int assetCount = assets.Where(x => x.Deploymentstatusid == getAssetInserviceID && x.Environmentid == getProductionEnvrionmentId).Count();

                            var deliveryTracking = _commonManager.MappingDeliveryTracing(mileStoneStatus, newPlannedActivity, assetCount);
                            await _deliveryTrackingManager.Add(deliveryTracking);
                        }

                    }
                    var addProjectPlanEntity = await _commonManager.AddProjectPlan(newPlannedActivity, _repositoryWrapper);

                    foreach (var plan in addProjectPlanEntity)
                    {
                        _repositoryWrapper.ProjectPlanRepository.Create(plan);
                    }
                    _repositoryWrapper.Save();
                    await _repositoryWrapper.ClearTracker();

                    await _repositoryWrapper.SaveAsync();
                    await _commonManager.CreateOrUpdateBptreport(newPlannedActivity.Plannedactivityid,
                        lcmEngineer.Designcomponentid, (long)newPlannedActivity.Designcomponentid);
                }

            }


        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == id).SingleOrDefaultAsync();

            Lcmancillarydata ancillaryEntity = await _repositoryWrapper.LcmAncillaryData
               .FindByConditionWithDelete(x => x.Lcmengineeringid == id).SingleOrDefaultAsync();
            if (ancillaryEntity != null)
            {
                _repositoryWrapper.LcmAncillaryData.Delete(ancillaryEntity);
                _repositoryWrapper.Save();
            }

            List<Networkelementsasplanned> relatedAssets = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Lcmengineeringid == id).ToList();
            foreach (Networkelementsasplanned item in relatedAssets)
            {
                item.Lcmengineeringid = null;
                _repositoryWrapper.NetworkElementAsPlanned.Update(item);
            }
            _repositoryWrapper.Save();

            foreach (Plannedactivities toDelete in entity.PlannedactivitiesLcmengineering)
            {
                _repositoryWrapper.PlannedActivity.Delete(toDelete);
            }

            /// LCM Part3 Requirements. if the key is not used i any of the existing entities, set the status of the key to not in use.  /// the below code block is for Software ResourceKey
            if (entity.Resourcekey != null)
            {
                _designComponentFamilyLifeCycleManager.setResourceKeyStatus(entity.Resourcekey, (int)ResourceTypesKey.Lcm);
                _ = await _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforLCMDeletion(entity);
            }
            await _repositoryWrapper.SaveAsync();

            _repositoryWrapper.Lcmengineering.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Lcmengineeringid
            };
        }
        public async Task<ResultDto> DeleteDeep(long id, bool onlyPlannedActivities)
        {
            Lcmengineering entity = new();
            entity = await _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == id)
                .Include(x => x.Lcmdeploymentstatus)
                .Include(x => x.Lcmengineeringeduspoc)
                .Include(x => x.Lcmengineeringsubdomainspoc)
                .Include(x => x.Lcmoperationalcontracts)
                .Include(x => x.Reasoncheckboxresourcelcmengineeringhardware)
                .Include(x => x.Reasoncheckboxresourcelcmengineeringsoftware)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Budgetprojecttrackers)
                 .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Daassetmigration)
                  .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Damigrationstatus)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Projectsplan).ThenInclude(x => x.Projectplanaudit)
                .FirstOrDefaultAsync();


            if (entity.PlannedactivitiesLcmengineering != null && entity.PlannedactivitiesLcmengineering.Count > 0)
            {
                List<Plannedactivities> plannedActivityToDelete = entity.PlannedactivitiesLcmengineering.ToList();
                foreach (Plannedactivities toDelete in plannedActivityToDelete)
                {
                    //Ticket 897 
                    _ = await _commonManager.GenerateAuditLogEntryForPAHardDeleteEntity(toDelete);
                    Deliverytrackings deliveryTracking = _repositoryWrapper.DeliveryTrackingRepository.FindByCondition(x => x.Plannedactivityid == toDelete.Plannedactivityid).FirstOrDefault();
                    if (deliveryTracking != null)
                    {
                        _repositoryWrapper.DeliveryTrackingRepository.DeleteDeep(deliveryTracking);
                    }
                    foreach (var deletepp in toDelete.Projectsplan)
                    {
                        foreach (var deletePpa in deletepp.Projectplanaudit)
                        {
                            _repositoryWrapper.ProjectPlanAuditRepository.DeleteDeep(deletePpa);
                        }
                        _repositoryWrapper.ProjectPlanRepository.DeleteDeep(deletepp);

                    }
                    if (toDelete.Budgetprojecttrackers != null && toDelete.Budgetprojecttrackers.Count > 0)
                    {
                        if (toDelete.Budgetprojecttrackers.FirstOrDefault() != null)
                        {
                            _repositoryWrapper.BudgetProjectTrackersRepository.DeleteDeep(toDelete.Budgetprojecttrackers.FirstOrDefault());
                        }

                    }

                    if (toDelete.Daassetmigration != null && toDelete.Daassetmigration.Count > 0)
                        await DeletePltformMigrationPaNewAsset(toDelete.Plannedactivityid);

                    foreach (var item in toDelete.Daassetmigration)
                    {
                        _repositoryWrapper.DaAssetMigrationRepository.DeleteDeep(item);
                    }
                    foreach (var item in toDelete.Damigrationstatus)
                    {
                        _repositoryWrapper.DaMigrationStatusRepository.DeleteDeep(item);
                    }

                    _repositoryWrapper.PlannedActivity.DeleteDeep(toDelete);

                }
                //Ticket 897 
                await _repositoryWrapper.SaveAsync();
                entity.PlannedactivitiesLcmengineering = null;
            }



            if (!onlyPlannedActivities)
            {
                var ancillaryEntity = await _repositoryWrapper.LcmAncillaryData
             .FindByConditionWithDelete(x => x.Lcmengineeringid == id).FirstOrDefaultAsync();
                if (ancillaryEntity != null)
                {
                    _repositoryWrapper.LcmAncillaryData.DeleteDeep(ancillaryEntity);
                    _repositoryWrapper.Save();
                }

                List<Networkelementsasplanned> relatedAssets = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Lcmengineeringid == id).ToList();
                foreach (Networkelementsasplanned item in relatedAssets)
                {
                    item.Lcmengineeringid = null;
                    _repositoryWrapper.NetworkElementAsPlanned.Update(item);
                }
                if (entity.Lcmengineeringeduspoc != null && entity.Lcmengineeringeduspoc.Count > 0)
                {
                    List<Lcmengineeringeduspoc> eduSpocList = entity.Lcmengineeringeduspoc.ToList();

                    foreach (Lcmengineeringeduspoc toDelete in eduSpocList)
                    {
                        _repositoryWrapper.LcmEngineeringEduSpoc.DeleteDeep(toDelete);
                    }
                }
                if (entity.Lcmengineeringsubdomainspoc != null && entity.Lcmengineeringsubdomainspoc.Count > 0)
                {
                    List<Lcmengineeringsubdomainspoc> subDomainSpocList = entity.Lcmengineeringsubdomainspoc.ToList();

                    foreach (Lcmengineeringsubdomainspoc toDelete in subDomainSpocList)
                    {
                        _repositoryWrapper.LcmEngineeringSubDomainSpoc.DeleteDeep(toDelete);
                    }
                }
                if (entity.Lcmoperationalcontracts != null && entity.Lcmoperationalcontracts.Count > 0)
                {
                    List<Lcmoperationalcontracts> opertionalContractsList = entity.Lcmoperationalcontracts.ToList();

                    foreach (Lcmoperationalcontracts toDelete in opertionalContractsList)
                    {
                        _repositoryWrapper.LCMOperationalContracts.DeleteDeep(toDelete);
                    }
                }
                if (entity.Reasoncheckboxresourcelcmengineeringhardware != null && entity.Reasoncheckboxresourcelcmengineeringhardware.Count > 0)
                {
                    List<Reasoncheckboxresourcelcmengineeringhardware> checkboxResourceLcmEngineeringHardwareList = entity.Reasoncheckboxresourcelcmengineeringhardware.ToList();
                    foreach (Reasoncheckboxresourcelcmengineeringhardware toDelete in checkboxResourceLcmEngineeringHardwareList)
                    {
                        _repositoryWrapper.CheckboxResourceLcmEngineeringHardware.DeleteDeep(toDelete);
                    }
                }
                if (entity.Reasoncheckboxresourcelcmengineeringsoftware != null && entity.Reasoncheckboxresourcelcmengineeringsoftware.Count > 0)
                {
                    List<Reasoncheckboxresourcelcmengineeringsoftware> checkboxResourceLcmEngineeringSoftwareList = entity.Reasoncheckboxresourcelcmengineeringsoftware.ToList();
                    foreach (Reasoncheckboxresourcelcmengineeringsoftware toDelete in checkboxResourceLcmEngineeringSoftwareList)
                    {
                        _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware.DeleteDeep(toDelete);
                    }
                }
                _repositoryWrapper.Lcmengineering.DeleteDeep(entity);
            }

            await _repositoryWrapper.SaveAsync();

            /// LCM Part3 Requirements. if the key is not used i any of the existing entities, set the status of the key to not in use.     /// the below code block is for Software ResourceKey
            if (entity.Resourcekey != null)
            {
                _designComponentFamilyLifeCycleManager.setResourceKeyStatus(entity.Resourcekey, (int)ResourceTypesKey.Lcm);
                _ = await _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforLCMDeletion(entity);
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Lcmengineeringid
            };
        }
        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            string[] plannedActivityLinked = new string[0];
            string[] plannedActivity = new string[0];

            IQueryable<long?> plannedActivityLinkedIds = _repositoryWrapper.PlannedActivity
                        .FindByCondition(x => x.Lcmengineeringid == id && x.Linkedtoplannedactivityid != null)
                        .Select(x => x.Linkedtoplannedactivityid);

            if (plannedActivityLinkedIds.Count() > 0)
            {
                plannedActivityLinked = _repositoryWrapper.PlannedActivity
                        .FindByCondition(x => plannedActivityLinkedIds.Contains(x.Plannedactivityid))
                        .Include(x => x.Plannedactivityresource)
                        .Include(x => x.Activitystatus)
                        .Include(x => x.Deliverystatus)
                        .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                        .ToArray();
            }
            plannedActivity = _repositoryWrapper.PlannedActivity
                        .FindByCondition(x => x.Lcmengineeringid == id && x.Linkedtoplannedactivityid == null)
                        .Include(x => x.Plannedactivityresource)
                        .Include(x => x.Activitystatus)
                        .Include(x => x.Deliverystatus)
                        .Select(x => PlannedActivityMapper.Get(x, true).toLinkedPlannedActivityName())
                        .ToArray();


            List<ResultMessageDto> rm = new();
            if (plannedActivity.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Planned Activity", Values = plannedActivity });
            }

            if (plannedActivityLinked.Length > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "Linked Planned Activity", Values = plannedActivityLinked });
            }


            var entity = await _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == id)
             .Include(x => x.Opco).FirstOrDefaultAsync();
            var designComponent = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Designcomponentid == entity.Designcomponentid)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction)
                .ThenInclude(x => x.Systemfunction)
                .Include(x => x.Systemtype)
                .FirstOrDefaultAsync();

            string dcName = designComponent.toDesignComponentNameLcm(_repositoryWrapper);

            return plannedActivityLinked.Length > 0
                ? new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Lcm Engineering",
                        RecordName = entity.Opco.Opco + " - " + dcName,
                        DataRelatedList = rm
                    }
                }
                : plannedActivity.Length > 0
                    ? new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryDeleteHasPlannedActivities,
                        Data = new RelatedRecordsResultDto()
                        {
                            EntityName = "Lcm Engineering",
                            RecordName = entity.Opco.Opco + " - " + dcName,
                            DataRelatedList = rm
                        }
                    }
                    : new ResultDto();

        }
        public async Task<ResultDto> Restore(long id)
        {

            Lcmengineering entity = await _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == id, true).SingleAsync();

            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.Lcmengineering.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Lcmengineeringid
            };
        }
        public LcmEngineeringDto Get(long id)
        {
            var entity = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == id).FirstOrDefault();
            return _mapper.Map<LcmEngineeringDto>(entity);
        }
        public async Task<LcmEngineeringDtoCreate> GetCreatePage(List<short> _opcoList, List<int> _verticalList)
        {
            IQueryable<Reasoncheckboxresources> checkBox = _repositoryWrapper.ReasonCheckboxResource.FindAll();
            IQueryable<Supportedresource> supportResource = _repositoryWrapper.SupportedResource.FindAll();
            IQueryable<Fullorpartialresource> fullOrPartialResources = _repositoryWrapper.FullOrPartialResource.FindAll();

            #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc.,

            #region //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
            var allDesigncomponents = DesignComponentTypeExtensionMethod.verticalBasedDesignComponentRecord(_verticalList, _repositoryWrapper, ConstantValueFilter.All)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
               .Include(x => x.Designcomponentfamily).
               ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction)
               .ThenInclude(x => x.Systemfunction).ToList();
            List<Designcomponents> designComponentResource = allDesigncomponents;//.Where(x => x.Visibleflag == true).ToList();
            List<Designcomponents> transientDsignComponentResource = allDesigncomponents.Where(x => x.Visibleflag == false).ToList();
            #endregion

            #region Ticket 711 Unable to select  Some Current DesignCompnenet Family while creating new  Unknown Resource Key LCM  

            Dictionary<long, string> designComponentFamilyResource = _repositoryWrapper.DesignComponentFamily.FindAll()
               .ToDictionary(x => x.Designcomponentfamilyid, x => x.DCFName(_repositoryWrapper)).Where(f => !f.Value.IsNullOrEmpty()).ToDictionary(k => k.Key,
               v => v.Value);
            #endregion

            #endregion
            IQueryable<Lcmdeploymentstatus> LcmDeploymentResource = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description != "Removed" && x.Description != "Powered Off");

            var opocResource = (_opcoList != null && _opcoList.Any() == true) ?
                                            _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid)).ToDictionary(x => x.Opcoid, x => x.Opco)
                                            : _repositoryWrapper.OpCo.FindAll().ToDictionary(x => x.Opcoid, x => x.Opco);

            IQueryable<Productimportances> productImportanceResource = _repositoryWrapper.ProductImportance.FindAll();

            var aspNetUsersIdList = (_opcoList != null && _opcoList.Any() == true && _verticalList != null && _verticalList.Any() == true) ? _repositoryWrapper.UserRepository.FindAll()
                                                                                    .Include(y => y.AspnetuseropcosUser)
                                                                                    .Include(y => y.AspnetuserverticalsUser).ThenInclude(y => y.Organisation)
                                                                                    .Where(x =>
                                                                                           x.AspnetuseropcosUser.Any(y => _opcoList.Contains((short)y.Opcoid)) &&
                                                                                           x.AspnetuserverticalsUser.Any(x1 => _verticalList.Contains((int)x1.Organisation.Verticalid))
                                                                                        ).ToList()
                                                                                    : _repositoryWrapper.UserRepository.FindAll().ToList();

            var subSpoc = aspNetUsersIdList.Where(x => x.Issubdomainspoc == true && x.Active == true).ToList();
            var eduSpoc = aspNetUsersIdList.Where(x => x.Iseduspoc == true && x.Active == true).ToList();

            IQueryable<Operationalcontracts> opertionalContracts = _repositoryWrapper.OperationalContract.FindAll();

            List<ViewBagandComponenetDto> buildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(0, true);

            ///Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'
            IDictionary<long, string> iDictionaryDesignComponentResource =
                              designComponentResource.toDesignComponentResource(_repositoryWrapper)
                              .Where(
                    x => !x.Value.ToLower().Contains("unknown")).ToDictionary(x => x.Key, x => x.Value);

            IDictionary<long, string> iDictionaryTransientDesignComponentResource =
                             transientDsignComponentResource.toDesignComponentResource(_repositoryWrapper)
                             .Where(
                   x => !x.Value.ToLower().Contains("unknown") && !x.Value.IsNullOrEmpty()).ToDictionary(x => x.Key, x => x.Value);

            Dictionary<int, string> verticalResponsibles = _repositoryWrapper.VerticalResponsible.FindAll().ToDictionary(x => x.Verticalresponsibleid, x => x.Verticalresponsible);
            LcmEngineeringDtoCreate model = new()
            {
                DesignComponentResource = iDictionaryDesignComponentResource,
                DesignComponentFamilyResource = designComponentFamilyResource,
                OpCoResource = opocResource,
                ProductImportanceResource = productImportanceResource.ToDictionary(x => x.Productimportanceid, x => x.Productimportance),
                SubDomainSpocResource = subSpoc.DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email),
                EduSpocResource = eduSpoc.DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email),
                OperationalContractResource = opertionalContracts.ToDictionary(x => (int)x.Id, x => x.Description),
                LCMDeploymentStatusResource = LcmDeploymentResource.ToDictionary(x => x.Id, x => x.Description),
                CheckboxResourceResource = checkBox.ToDictionary(x => (int)x.Id, x => _mapper.Map<ReasonCheckboxDto>(ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(x))),
                SupportedResource = supportResource.ToDictionary(x => x.Id, x => _mapper.Map<TipologicaGridDtoRule>(SupportedResourceMapper.GetSupportedResourceMapper(x))),
                FullorPartialSupportResource = fullOrPartialResources.ToDictionary(x => x.Id, x => x.Description),
                FullorPartialSupportHWResource = fullOrPartialResources.ToDictionary(x => x.Id, x => x.Description),
                ElementCount = true,
                VerticalResponsibles = verticalResponsibles,
                BuildBagResources = buildBagResources,
                TransientDesignComponentResource = iDictionaryTransientDesignComponentResource
            };
            return model;
        }
        public async Task<LcmEngineeringDtoUpdate> GetUpdatePage(long id, List<short> _opcoList, List<int> _verticalList)
        {
            Lcmengineering entity = await Task.Run(() => _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == id, true)
                .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent)
                .Include(x => x.Designcomponentfamily)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Reasoncheckboxresourcelcmengineeringhardware)
                .Include(x => x.Reasoncheckboxresourcelcmengineeringsoftware)
                .Include(x => x.Designcomponent)
                .FirstOrDefault());

            entity.Buildbagid = entity.Buildbagid;

            IEnumerable<Plannedactivities> plannedActivites = entity.PlannedactivitiesLcmengineering.Where(x => x.Archived != true);

            LcmEngineering model = await Task.Run(() => LCMEngineeringMapper.GetLcmEngineeringMapper(entity));
            if (plannedActivites != null)
            {
                foreach (Plannedactivities item in plannedActivites)
                {
                    model.PlannedActivities.Add(PlannedActivityMapper.Get(item));

                }

            }

            LcmEngineeringDtoUpdate dto = _mapper.Map<LcmEngineeringDtoUpdate>(model);
            if (model.PlannedActivities != null && model.PlannedActivities.Count > 0)
            {
                foreach (PlannedActivity item in model.PlannedActivities.Where(x => x.Deleted == false))
                {
                    if (dto.PlannedActivityDto == null || dto.PlannedActivityDto.Count == 0)
                    {
                        dto.PlannedActivityDto.Add(_mapper.Map<PlannedActivityDtoUpdate>(item));
                    }
                }
                if (model.PlannedActivities.Count == 1)
                {
                    PlannedActivity firstPlannedActivity = model.PlannedActivities.FirstOrDefault();

                    dto.LCMDeploymentStatusResource = _repositoryWrapper.SettingsUpdatePlannedActivity
                        .FindByCondition(x => x.Deliverystatusid == firstPlannedActivity.DeliveryStatusId
                        && x.Plannedactivityresourceid == firstPlannedActivity.PlannedActivityResourceId && x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering)
                        .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus).ThenInclude(x => x.Lcmdeploymentstatus)
                        .SelectMany(x => x.Settingupdateplannedactivitylcmdeploymentstatus)
                        .ToDictionary(x => x.Lcmdeploymentstatusid, x => x.Lcmdeploymentstatus.Description);
                }
                if (model.PlannedActivities.Count > 1)
                {
                    DateTime? minPlannedCompletion = model.PlannedActivities.Select(x => x.PlannedCompletion).Min();
                    PlannedActivity plannedActivityWithMinPlannedComp = model.PlannedActivities.Where(x => x.PlannedCompletion == minPlannedCompletion).FirstOrDefault();

                    dto.LCMDeploymentStatusResource = _repositoryWrapper.SettingsUpdatePlannedActivity
                        .FindByCondition(x => x.Deliverystatusid == plannedActivityWithMinPlannedComp.DeliveryStatusId && x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering
                        && x.Plannedactivityresourceid == plannedActivityWithMinPlannedComp.PlannedActivityResourceId)
                        .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus).ThenInclude(x => x.Lcmdeploymentstatus)
                        .SelectMany(x => x.Settingupdateplannedactivitylcmdeploymentstatus)
                        .ToDictionary(x => x.Lcmdeploymentstatusid, x => x.Lcmdeploymentstatus.Description);
                }

                if (dto.LCMDeploymentStatusId.HasValue &&
               !dto.LCMDeploymentStatusResource.ContainsKey(dto.LCMDeploymentStatusId.Value))
                {
                    var data = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(
                        x => x.Id == dto.LCMDeploymentStatusId, true).FirstOrDefault();
                    if (data != null)
                    {
                        dto.LCMDeploymentStatusResource.Add(data.Id, data.Description);
                    }
                }
            }

            else
            {
                dto.LCMDeploymentStatusResource = _repositoryWrapper.LcmDeploymentStatusRepository
                   .FindByCondition(x => x.Description != "Removed" && x.Description != "Powered Off")
                   .ToDictionary(x => x.Id, x => x.Description);

                if (dto.LCMDeploymentStatusId.HasValue &&
               !dto.LCMDeploymentStatusResource.ContainsKey(dto.LCMDeploymentStatusId.Value))
                {
                    var data = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(
                        x => x.Id == dto.LCMDeploymentStatusId, true).FirstOrDefault();
                    if (data != null)
                    {
                        dto.LCMDeploymentStatusResource.Add(data.Id, data.Description);
                    }
                }
            }

            dto.EduSpocIds = _repositoryWrapper.LcmEngineeringEduSpoc
                .FindByCondition(x => x.Lcmengineeringid == dto.LcmEngineeringId && x.Eduspocid != null).Select(x => x.Eduspocid).ToList();

            dto.SubDomainSpocIds = _repositoryWrapper.LcmEngineeringSubDomainSpoc
                .FindByCondition(x => x.Lcmengineeringid == dto.LcmEngineeringId && x.Subdomainspocid != null).Select(x => x.Subdomainspocid).ToList();

            dto.OperationContractsIds = _repositoryWrapper.LCMOperationalContracts
              .FindByCondition(x => x.Lcmid == dto.LcmEngineeringId).Select(x => x.Operationalcontractid).ToList();

            Lcmancillarydata LcmAncillaryData = _repositoryWrapper.LcmAncillaryData.FindByCondition(x => x.Lcmengineeringid == id).FirstOrDefault();

            if (LcmAncillaryData != null)
            {
                dto.CommentOnProjectStatus = LcmAncillaryData.Commentonprojectstatus;
                dto.ReasonForNoPlan = LcmAncillaryData.Reasonfornoplan;
            }

            #region lookUp

            IQueryable<Reasoncheckboxresources> checkBox = _repositoryWrapper.ReasonCheckboxResource.FindAll();
            dto.CheckboxResourceResource = checkBox.ToDictionary(x => (int)x.Id, x => _mapper.Map<ReasonCheckboxDto>(ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(x)));
            foreach (int s in dto.CheckboxResourceLcmEngineeringHardwares)
            {
                if (!dto.CheckboxResourceResource.ContainsKey(s))
                {
                    var data = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(
                        x => x.Id == s, true).FirstOrDefault();
                    if (data != null)
                    {
                        dto.CheckboxResourceResource.Add(data.Id, _mapper.Map<ReasonCheckboxDto>(ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(data)));
                    }
                }
            }
            foreach (int s in dto.CheckboxResourceLcmEngineeringSoftwares)
            {
                if (!dto.CheckboxResourceResource.ContainsKey(s))
                {
                    var data = _repositoryWrapper.ReasonCheckboxResource.FindByCondition(
                        x => x.Id == s, true).FirstOrDefault();
                    if (data != null)
                    {
                        dto.CheckboxResourceResource.Add(data.Id, _mapper.Map<ReasonCheckboxDto>(ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(data)));
                    }
                }
            }

            IQueryable<Supportedresource> supportResource = _repositoryWrapper.SupportedResource.FindAll();
            dto.SupportedResource = supportResource.ToDictionary(x => x.Id, x => _mapper.Map<TipologicaGridDtoRule>(SupportedResourceMapper.GetSupportedResourceMapper(x)));
            if (dto.SoftwareSupportedId != null && !dto.SupportedResource.ContainsKey(dto.SoftwareSupportedId.Value))
            {
                var data = _repositoryWrapper.SupportedResource.FindByCondition(
                    x => x.Id == dto.SoftwareSupportedId, true).FirstOrDefault();
                if (data != null)
                {
                    dto.SupportedResource.Add(data.Id, _mapper.Map<TipologicaGridDtoRule>(SupportedResourceMapper.GetSupportedResourceMapper(data)));
                }
            }
            if (dto.HardwareSupportedId != null && !dto.SupportedResource.ContainsKey(dto.HardwareSupportedId.Value))
            {
                var data = _repositoryWrapper.SupportedResource.FindByCondition(
                    x => x.Id == dto.HardwareSupportedId, true).FirstOrDefault();
                if (data != null)
                {
                    dto.SupportedResource.Add(data.Id, _mapper.Map<TipologicaGridDtoRule>(SupportedResourceMapper.GetSupportedResourceMapper(data)));
                }
            }
            IQueryable<Fullorpartialresource> fullOrPartialResources = _repositoryWrapper.FullOrPartialResource.FindAll();
            dto.FullorPartialSupportResource = fullOrPartialResources.ToDictionary(x => x.Id, x => x.Description);
            dto.FullorPartialSupportHWResource = fullOrPartialResources.ToDictionary(x => x.Id, x => x.Description);

            if (dto.FullorPartialSupportId != null && !dto.FullorPartialSupportResource.ContainsKey(dto.FullorPartialSupportId.Value))
            {
                var data = _repositoryWrapper.FullOrPartialResource.FindByCondition(
                    x => x.Id == dto.FullorPartialSupportId, true).FirstOrDefault();
                if (data != null)
                {
                    dto.FullorPartialSupportResource.Add(data.Id, data.Description);
                }
            }
            if (dto.FullorPartialSupportHWId != null && !dto.FullorPartialSupportHWResource.ContainsKey(dto.FullorPartialSupportHWId.Value))
            {
                var data = _repositoryWrapper.FullOrPartialResource.FindByCondition(
                    x => x.Id == dto.FullorPartialSupportHWId, true).FirstOrDefault();
                if (data != null)
                {
                    dto.FullorPartialSupportHWResource.Add(data.Id, data.Description);
                }
            }




            #region    //Ticket 595 -#590 - Vertical Filter to be applied on design component dropdown's in LC, PA, DA etc., 

            var allDesigncomponents = DesignComponentTypeExtensionMethod.verticalBasedDesignComponentRecord(_verticalList, _repositoryWrapper, ConstantValueFilter.All)
               .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
              .Include(x => x.Designcomponentfamily).
              ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction)
              .ThenInclude(x => x.Systemfunction).ToList();
            List<Designcomponents> designComponentResource = allDesigncomponents.Where(x => x.Visibleflag == true).ToList();
            List<Designcomponents> transientDsignComponentResource = allDesigncomponents.Where(x => x.Visibleflag == false).ToList();

            IDictionary<long, string> designComponenentResource = designComponentResource.toDesignComponentResource(_repositoryWrapper);
            IDictionary<long, string> transientDesignComponenentResource = transientDsignComponentResource.toDesignComponentResource(_repositoryWrapper);

            #region Ticket 711 Unable to select  Some Current DesignCompnenet Family while creating new  Unknown Resource Key LCM 
            dto.DesignComponentFamilyResource = _repositoryWrapper.DesignComponentFamily.FindAll()
                .ToDictionary(x => x.Designcomponentfamilyid, x => x.DCFName(_repositoryWrapper));

            #endregion

            #endregion

            dto.DesignComponentResource = designComponenentResource;
            dto.TransientDesignComponentResource = transientDesignComponenentResource;

            dto.VerticalResponsibles = _repositoryWrapper.VerticalResponsible.FindAll().ToDictionary(x => x.Verticalresponsibleid, x => x.Verticalresponsible);

            if (!dto.DesignComponentResource.ContainsKey(dto.DesignComponentId))
            {
                Designcomponents data = _repositoryWrapper.DesignComponent.FindByCondition(
                    x => x.Designcomponentid == dto.DesignComponentId,
                    includeDeleted: true)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                    .FirstOrDefault();
                if (data != null)
                {
                    dto.DesignComponentResource.Add(data.Designcomponentid, data.toDesignComponentNameLcm(_repositoryWrapper
                        ));
                }
            }

            #region  Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
            if (!dto.DesignComponentFamilyResource.ContainsKey(dto.DesignComponentFamilyid))
            {
                Designcomponentfamilies data = _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == dto.DesignComponentFamilyid, true)
                    .Include(x => x.Designcomponents).FirstOrDefault();
                if (data != null)
                {
                    dto.DesignComponentFamilyResource.Add(data.Designcomponentfamilyid, data.DCFName(_repositoryWrapper
                        ));
                }
            }
            dto.DesignComponentFamilyResource.Where(f => !f.Value.IsNullOrEmpty()).ToDictionary(k => k.Key, v => v.Value);
            #endregion            
            var opocResource = (_opcoList != null && _opcoList.Any() == true) ?
                                 _repositoryWrapper.OpCo.FindByCondition(x => _opcoList.Contains(x.Opcoid))
                                 : _repositoryWrapper.OpCo.FindAll();
            dto.OpCoResource = opocResource.ToDictionary(x => x.Opcoid, x => x.Opco);
            if (!dto.OpCoResource.ContainsKey(dto.OpCoId))
            {
                var data = _repositoryWrapper.OpCo.FindByCondition(
                    x => x.Opcoid == dto.OpCoId, true).FirstOrDefault();
                if (data != null)
                {
                    dto.OpCoResource.Add(data.Opcoid, data.Opco);
                }
            }

            IQueryable<Productimportances> productImportanceResource = _repositoryWrapper.ProductImportance.FindAll();
            dto.ProductImportanceResource =
                productImportanceResource.ToDictionary(x => x.Productimportanceid, x => x.Productimportance);

            if (dto.ProductImportanceId.HasValue &&
                !dto.ProductImportanceResource.ContainsKey(dto.ProductImportanceId.Value))
            {
                var data = _repositoryWrapper.ProductImportance.FindByCondition(
                    x => x.Productimportanceid == dto.ProductImportanceId, true).FirstOrDefault();
                if (data != null)
                {
                    dto.ProductImportanceResource.Add(data.Productimportanceid, data.Productimportance);
                }
            }



            IEnumerable<long> ids = dto.PlannedActivityDto.Select(x => x.PlannedActivityId);
            dto.PlannedActivityDto = new List<PlannedActivityDtoUpdate>();
            foreach (long planned in ids)
            {
                dto.PlannedActivityDto.Add(await _plannedActivityManager.GetUpdatePage(planned, _opcoList, _verticalList, 0, (long)dto?.DesignComponentFamilyid));
            }

            var aspNetUserIdList = (_opcoList != null && _opcoList.Any() == true) ? _repositoryWrapper.UserRepository.FindAllWithDelete(true)
                                                                                    .Include(y => y.AspnetuseropcosUser)
                                                                                    .Include(y => y.AspnetuserverticalsUser).ThenInclude(y => y.Organisation)
                                                                                    .Where(x =>
                                                                                           x.AspnetuseropcosUser.Any(y => _opcoList.Contains((short)y.Opcoid)) &&
                                                                                           x.AspnetuserverticalsUser.Any(x1 => _verticalList.Contains((int)x1.Organisation.Verticalid))
                                                                                        ).ToList()
                                                                                    : _repositoryWrapper.UserRepository.FindAllWithDelete(true).ToList();

            dto.EduSpocResource =
                           aspNetUserIdList.Where(x => x.Iseduspoc == true && x.Deleted == false).DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email);
            if (dto.EduSpocIds != null && dto.EduSpocIds.Any())
            {
                foreach (int item in dto.EduSpocIds)
                {
                    if (!dto.EduSpocResource.ContainsKey(item))
                    {
                        Aspnetusers data = aspNetUserIdList.Where(x => x.Id == item).FirstOrDefault();
                        if (data != null)
                        {
                            dto.EduSpocResource.Add(data.Id,
                                data.Email);
                        }
                    }
                }
            }
            dto.SubDomainSpocResource =
                          aspNetUserIdList.Where(x => x.Issubdomainspoc == true && x.Deleted == false).DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email);
            if (dto.SubDomainSpocIds != null && dto.SubDomainSpocIds.Any())
            {
                foreach (int item in dto.SubDomainSpocIds)
                {
                    if (!dto.SubDomainSpocResource.ContainsKey(item))
                    {
                        Aspnetusers data = aspNetUserIdList.Where(x => x.Id == item).FirstOrDefault();
                        if (data != null)
                        {
                            dto.SubDomainSpocResource.Add(data.Id,
                                data.Email);
                        }
                    }
                }
            }
            IQueryable<Operationalcontracts> opertionalOontracts = _repositoryWrapper.OperationalContract.FindAll();
            dto.OperationalContractResource =
                opertionalOontracts.ToDictionary(x => (int)x.Id, x => x.Description);
            if (dto.OperationContractsIds != null && dto.OperationContractsIds.Any())
            {
                foreach (int idsds in dto.OperationContractsIds)
                {
                    if (!dto.OperationalContractResource.ContainsKey(idsds))
                    {
                        var data = _repositoryWrapper.OperationalContract.FindByCondition(
                            x => x.Id == (short)idsds, true).FirstOrDefault();
                        if (data != null)
                        {
                            dto.OperationalContractResource.Add(data.Id,
                                data.Description);
                        }
                    }
                }
            }
            if (dto.OperationContractsIds != null && dto.OperationContractsIds.Any())
            {
                foreach (int idsds in dto.EduSpocIds)
                {
                    if (!dto.OperationalContractResource.ContainsKey(idsds))
                    {
                        var data = _repositoryWrapper.OperationalContract.FindByCondition(
                            x => x.Id == (short)idsds, true).FirstOrDefault();
                        if (data != null)
                        {
                            dto.OperationalContractResource.Add(data.Id,
                                data.Description);
                        }
                    }
                }
            }

            #endregion

            #region System Of System

            dto.BuildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(0, false, true, (long)model.DesignComponentId, (short)model.OpCoId, 0);
            if (dto.BuildBagResources.Any(x => x.BuildBagId == model.BuildBagId) == false)
            {
                dto.BuildBagResources = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(model.BuildBagId, false);
            }
            //_dropdownDataServiceManager.GetBuildBagDetails();

            #endregion
            return dto;
        }
        public async Task<ResultDto> GetLcmDeploymentStatusRelatedDeliveryStatusAndPAResource(short plannedActivityResourceId, short deliveryStatusId)
        {
            IDictionary<short, string> LcmdeploymentstatusDict = new Dictionary<short, string>();

            IDictionary<short, string> AssetdeploymentstatusDict = new Dictionary<short, string>();

            LcmdeploymentstatusDict = await Task.Run(() => _repositoryWrapper.SettingsUpdatePlannedActivity
               .FindByCondition(x => x.Plannedactivityresourceid == plannedActivityResourceId
               && x.Deliverystatusid == deliveryStatusId && x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering)
               .Include(x => x.Settingupdateplannedactivitylcmdeploymentstatus)
               .ThenInclude(x => x.Lcmdeploymentstatus)?
               .Select(x => x.Settingupdateplannedactivitylcmdeploymentstatus)
               .FirstOrDefault()?
               .Where(x => x.Lcmdeploymentstatus != null)
               .Select(x => x.Lcmdeploymentstatus)
               .ToDictionary(x => x.Id, x => x.Description));

            return LcmdeploymentstatusDict != null && LcmdeploymentstatusDict.Count > 0
                ? new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = LcmdeploymentstatusDict
                }
                : new ResultDto
                {
                    Info = ResultMessages.NoDeploymentStatusExistForThePlannedActivity,
                    Data = null,
                    Warning = true
                };

        }
        private static ExpressionStarter<Lcmengineering> ApplyFilter(LcmEngineeringQueryDto buildFilterDto)
        {


            ExpressionStarter<Lcmengineering> predicateResult = PredicateBuilder.New<Lcmengineering>(true);

            ExpressionStarter<Lcmengineering> predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            if (buildFilterDto.LcmEngineeringId != null && buildFilterDto.LcmEngineeringId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.LcmEngineeringId)
                {
                    _ = predicateInner.Or(x => x.Lcmengineeringid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VodafoneName != null && buildFilterDto.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (int item in buildFilterDto.VodafoneName)
                {
                    _ = predicateInner.Or(x => x.Designcomponent.Subnetworkboundary != null && x.Designcomponent.Subnetworkboundary.Vodafonenameid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductImportanceId != null && buildFilterDto.ProductImportanceId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.ProductImportanceId)
                {
                    _ = predicateInner.Or(x => x.Productimportance.Productimportance == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (short item in buildFilterDto.OpCo)
                {
                    _ = predicateInner.Or(x => x.Opcoid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.LastModifiedBy)
                {
                    _ = predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                }

                _ = predicateResult.And(predicateInner);

            }

            if (buildFilterDto.DesignComponent != null && buildFilterDto.DesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.DesignComponent)
                {
                    _ = predicateInner.Or(x => x.Designcomponentid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmDeploymentStatus != null && buildFilterDto.LcmDeploymentStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (short item in buildFilterDto.LcmDeploymentStatus)
                {
                    _ = predicateInner.Or(x => x.Lcmdeploymentstatusid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponentFamilyId != null && buildFilterDto.DesignComponentFamilyId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.DesignComponentFamilyId)
                {
                    _ = predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OperationalContact != null && buildFilterDto.OperationalContact.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.OperationalContact)
                {
                    _ = predicateInner.Or(x => x.Lcmoperationalcontracts.Any(d => d.Operationalcontract.Description == item));
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignComponentId != null && buildFilterDto.DesignComponentId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.DesignComponentId)
                {
                    _ = predicateInner.Or(x => x.Designcomponentid == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SoftwareEndOfSupportContract != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.SoftwareEndOfSupportContract.StartDate != null)
                {
                    _ = predicateInner.And(x => x.Softwareendofsupportcontract >= buildFilterDto.SoftwareEndOfSupportContract.StartDate);
                }

                if (buildFilterDto.SoftwareEndOfSupportContract.EndDate != null)
                {
                    _ = predicateInner.And(x => x.Softwareendofsupportcontract <= buildFilterDto.SoftwareEndOfSupportContract.EndDate);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareEndOfSupportContract != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.HardwareEndOfSupportContract.StartDate != null)
                {
                    _ = predicateInner.And(x => x.Hardwareendofsupportcontract >= buildFilterDto.HardwareEndOfSupportContract.StartDate);
                }

                if (buildFilterDto.HardwareEndOfSupportContract.EndDate != null)
                {
                    _ = predicateInner.And(x => x.Hardwareendofsupportcontract <= buildFilterDto.HardwareEndOfSupportContract.EndDate);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SoftwareEndOfWarrantyDate != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.SoftwareEndOfWarrantyDate.StartDate != null)
                {
                    _ = predicateInner.And(x => x.Softwareendofwarrantydate >= buildFilterDto.SoftwareEndOfWarrantyDate.StartDate);
                }

                if (buildFilterDto.SoftwareEndOfWarrantyDate.EndDate != null)
                {
                    _ = predicateInner.And(x => x.Softwareendofwarrantydate <= buildFilterDto.SoftwareEndOfWarrantyDate.EndDate);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                {
                    _ = predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                }

                if (buildFilterDto.LastModifiedValue.EndDate != null)
                {
                    _ = predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareSupportProvider != null && buildFilterDto.HardwareSupportProvider.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.HardwareSupportProvider)
                {
                    _ = predicateInner.Or(x => x.Hardwaresupportprovider == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareSupportType != null && buildFilterDto.HardwareSupportType.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.HardwareSupportType)
                {
                    _ = predicateInner.Or(x => x.Hardwaresupporttype == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareSupportProvider != null && buildFilterDto.SoftwareSupportProvider.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.SoftwareSupportProvider)
                {
                    _ = predicateInner.Or(x => x.Softwaresupportprovider == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareSupportType != null && buildFilterDto.SoftwareSupportType.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.SoftwareSupportType)
                {
                    _ = predicateInner.Or(x => x.Softwaresupporttype == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedActivity != null && buildFilterDto.PlannedActivity.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                //foreach (long item in buildFilterDto.PlannedActivity)
                //{

                //    _ = predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(s => s.Plannedactivityid == item));
                //}

                //_ = predicateResult.And(predicateInner);

                predicateInner = PredicateBuilder.New<Lcmengineering>();
                int count = 0;

                foreach (var item in buildFilterDto?.PlannedActivity)
                    if (item.ToString() == "0")
                    {
                        count++;

                        predicateInner.Or(x => x.PlannedactivitiesLcmengineering != null && !x.PlannedactivitiesLcmengineering.Any());
                    }
                    else
                    {
                        count++;

                        _ = predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(s => s.Plannedactivityid == item));
                    }

                if (count > 0) predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CommentOnProjectStatus != null && buildFilterDto.CommentOnProjectStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.CommentOnProjectStatus)
                {
                    _ = predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(s => s.Plannedactivityid == item));
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ReasonForNoPlan != null && buildFilterDto.ReasonForNoPlan.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.ReasonForNoPlan)
                {
                    _ = predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(s => s.Plannedactivityid == item));
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OriginalLcm != null && buildFilterDto.OriginalLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.OriginalLcm)
                {
                    _ = predicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any(s => s.Originallcmengineeringid == item));
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Warranty != null && buildFilterDto.Warranty.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (bool item in buildFilterDto.Warranty)
                {
                    predicateResult = predicateResult.And(x => x.Warranty == item);
                }
            }
            if (buildFilterDto.ResourceKey != null && buildFilterDto.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.ResourceKey)
                {
                    _ = predicateInner.Or(x => x.Resourcekey == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PreviousResourceKey != null && buildFilterDto.PreviousResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.PreviousResourceKey)
                {
                    _ = predicateInner.Or(x => x.Previousresourcekey == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.IsExtendedSupportOfferedByVendor != null && buildFilterDto.IsExtendedSupportOfferedByVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.IsExtendedSupportOfferedByVendor)
                {
                    _ = item.ToLower() == "yes"
                        ? predicateInner.Or(x => x.Isextendedsupportofferedbyvendor == true)
                        : predicateInner.Or(x => x.Isextendedsupportofferedbyvendor == false || !x.Isextendedsupportofferedbyvendor.HasValue);
                }
                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IsLcmAncillaryData != null && buildFilterDto.IsLcmAncillaryData.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.IsLcmAncillaryData)
                {
                    if (item.ToLower() == "yes")
                    {
                        _ = predicateInner.Or(x => x.Lcmancillarydata.Any(data => data.Lcmengineeringid == x.Lcmengineeringid && data.Deleted == false)); ;
                    }
                    else
                    {
                        _ = predicateInner.Or(x => x.Lcmancillarydata.All(data => data.Lcmengineeringid != x.Lcmengineeringid && data.Deleted == false));
                    }
                }


                _ = predicateResult.And(predicateInner);
            }

            #region Org Based Edu, Subdomain ,Vertical filter

            if (buildFilterDto.SubDomainSpoc != null && buildFilterDto.SubDomainSpoc.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.SubDomainSpoc)
                {
                    _ = item == "yes"
                        ? predicateInner.Or(x => x.Lcmengineeringsubdomainspoc != null && !x.Lcmengineeringsubdomainspoc.Any())
                        : predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspocid.ToString() == item));
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Eduspoc != null && buildFilterDto.Eduspoc.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.Eduspoc)
                {
                    _ = item == "yes"
                        ? predicateInner.Or(x => x.Lcmengineeringeduspoc != null && !x.Lcmengineeringeduspoc.Any())
                        : predicateInner.Or(x => x.Lcmengineeringeduspoc.Any(d => d.Eduspocid.ToString() == item));
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                int count = 0;
                foreach (string item in buildFilterDto.VerticalName)
                {
                    if (item == "yes")
                    {
                        count++;
                        _ = predicateInner.Or(x => x.Lcmengineeringsubdomainspoc != null && !x.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;
                        _ = predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                                           .Any(m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false
                                           //&& x.Lcmengineeringsubdomainspoc.Any(a => a.Subdomainspoc.AspnetuseropcosUser.Any(o => o.Opco.Opcoid == x.Opcoid))
                                           )));
                    }
                }

                if (count > 0)
                {
                    _ = predicateResult.And(predicateInner);
                }
            }

            if (buildFilterDto.BuildBagDescription?.Any() == true)
            {
                ExpressionStarter<Lcmengineering> descriptionPredicate = PredicateBuilder.New<Lcmengineering>();
                foreach (long description in buildFilterDto.BuildBagDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Buildbag.Buildbagid == description);
                }

                _ = predicateResult.And(descriptionPredicate);
            }

            #endregion
            return predicateResult;

        }
        public async Task<QueryResultDto<LcmEngineeringDtoGrid>> FindWithCondition(LcmEngineeringQueryDto designComponentFilterDto)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ApplyFilter(designComponentFilterDto);

            if (designComponentFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }

            if (designComponentFilterDto.Orphan == true)
            {
                predicateResult = predicateResult.And(x => x.Numberofnodes == 0);
            }

            IQueryable<Lcmengineering> items = await Task.Run(() => GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false).Result.AsQueryable()
                .Where(x => x.Archived == !ConstantValueFilter.isTrue || x.Archived == null));

            QueryResultDto<LcmEngineeringDtoGrid> rtn = new(new GenerateRenderForGrid<LcmEngineeringDtoGrid>(_manager))
            {
            };
            List<LcmEngineering> mappedItems = new();
            IQueryable<LcmEngineering> query;
            if ((designComponentFilterDto.NumberOfNodes == null || !designComponentFilterDto.NumberOfNodes.Any()) && (designComponentFilterDto.NumberOfNodesInLab == null || !designComponentFilterDto.NumberOfNodesInLab.Any()))
            {
                rtn.TotalItems = items.Count();
                if (designComponentFilterDto.PageSize == 0)
                {
                    designComponentFilterDto.PageSize = rtn.TotalItems;
                    designComponentFilterDto.Page = 1;
                }
                items = items.OrderByDescending(x => x.Modificationdate).Skip((designComponentFilterDto.Page - 1) * designComponentFilterDto.PageSize).Take(designComponentFilterDto.PageSize);
                query = items.AsEnumerable().Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p)).AsQueryable().ApplyOrdering(designComponentFilterDto, GetColumnsMap());//.ApplyPaging(designComponentFilterDto);
            }
            else
            {
                mappedItems = items.AsEnumerable().Select(p => LCMEngineeringMapper.GetLcmEngineeringMapper(p)).ToList();
                if (designComponentFilterDto.NumberOfNodes != null && designComponentFilterDto.NumberOfNodes.Any())
                {
                    mappedItems = mappedItems.Where(x => designComponentFilterDto.NumberOfNodes.Contains(x.CountNetworkElementReleated(false, _repositoryWrapper))).ToList();
                    rtn.TotalItems = mappedItems.Count();
                }
                if (designComponentFilterDto.NumberOfNodesInLab != null && designComponentFilterDto.NumberOfNodesInLab.Any())
                {
                    mappedItems = mappedItems.Where(x => designComponentFilterDto.NumberOfNodesInLab.Contains(x.CountNetworkElementReleated(true, _repositoryWrapper))).ToList();
                    rtn.TotalItems = mappedItems.Count();
                }
                query = mappedItems.AsQueryable().ApplyOrdering(designComponentFilterDto, GetColumnsMap()).ApplyPaging(designComponentFilterDto);
            }
            List<LcmEngineering> data = query.ToList();


            IQueryable<Subdomainspocs> subDomainSpocResource = _repositoryWrapper.SubDomainSpoc.FindAll();
            IQueryable<Aspnetusers> organisation = _repositoryWrapper.UserRepository.FindAll();


            #region filter for Org based edu,subdomain and vertical

            ExpressionStarter<Lcmengineeringeduspoc> eduPredicateResult = PredicateBuilder.New<Lcmengineeringeduspoc>(true);
            ExpressionStarter<Lcmengineeringeduspoc> eduPredicateInner = PredicateBuilder.New<Lcmengineeringeduspoc>(true);

            ExpressionStarter<Lcmengineeringsubdomainspoc> subDomainPredicateResult = PredicateBuilder.New<Lcmengineeringsubdomainspoc>(true);
            ExpressionStarter<Lcmengineeringsubdomainspoc> subDomainPredicateInner = PredicateBuilder.New<Lcmengineeringsubdomainspoc>(true);

            if (designComponentFilterDto.Eduspoc != null && designComponentFilterDto.Eduspoc.Any())
            {
                eduPredicateInner = PredicateBuilder.New<Lcmengineeringeduspoc>();
                foreach (string item in designComponentFilterDto.Eduspoc)
                {
                    _ = eduPredicateInner.Or(x => x.Eduspocid.ToString() == item);
                }

                _ = eduPredicateResult.And(eduPredicateInner);
            }
            if (designComponentFilterDto.SubDomainSpoc != null && designComponentFilterDto.SubDomainSpoc.Any())
            {
                subDomainPredicateInner = PredicateBuilder.New<Lcmengineeringsubdomainspoc>();
                foreach (string item in designComponentFilterDto.SubDomainSpoc)
                {
                    _ = subDomainPredicateInner.Or(x => x.Subdomainspocid.ToString() == item);
                }

                _ = subDomainPredicateResult.And(subDomainPredicateInner);
            }
            if (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Any())
            {
                subDomainPredicateInner = PredicateBuilder.New<Lcmengineeringsubdomainspoc>();
                foreach (string item in designComponentFilterDto.VerticalName)
                {
                    _ = subDomainPredicateInner.Or(x => x.Subdomainspoc.AspnetuserverticalsUser.Any(m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false /*&& m.Opcoid == x.Lcmengineering.Opcoid*/));
                }

                _ = eduPredicateResult.And(eduPredicateInner);
            }

            #endregion

            foreach (LcmEngineering lcm in data)
            {
                lcm.PlannedActivities = items.FirstOrDefault(p => p.Lcmengineeringid == lcm.LcmengineeringId).PlannedactivitiesLcmengineering.Where(x => x.Archived != ConstantValueFilter.isTrue).Select(p => PlannedActivityMapper.Get(p)).ToList();

                IQueryable<Lcmengineeringsubdomainspoc> lcmEngineeringSubDomainSpoc = _repositoryWrapper.LcmEngineeringSubDomainSpoc.FindByCondition(subDomainPredicateResult)
                    .Where(x => x.Lcmengineeringid == lcm.LcmengineeringId);
                IQueryable<Lcmengineeringeduspoc> lcmEngineeringEduSpoc = _repositoryWrapper.LcmEngineeringEduSpoc.FindByCondition(eduPredicateResult)
                    .Where(x => x.Lcmengineeringid == lcm.LcmengineeringId);

                lcm.LcmEngineeringEduSpoc = lcmEngineeringEduSpoc.Where(x => x.Eduspocid != null).ToList().Select(p => LcmEngineeringEduSpocMapper.Get(p)).ToList();
                foreach (LcmEngineeringEduSpoc eduSpoc in lcm.LcmEngineeringEduSpoc)
                {
                    var orgEntity = organisation.FirstOrDefault(x => x.Id == eduSpoc.Eduspocid);
                    if (orgEntity != null)
                        eduSpoc.Eduspoc = ApplicationUserMapper.GetApplicationUserMapper(orgEntity);
                }

                lcm.LcmEngineeringSubDomainSpoc = lcmEngineeringSubDomainSpoc.Where(x => x.Subdomainspocid != null).ToList().Select(p => LcmEngineeringSubDomainSpocMapper.Get(p)).ToList();
                foreach (LcmEngineeringSubDomainSpoc subSpoc in lcm.LcmEngineeringSubDomainSpoc)
                {
                    var orgEntity = organisation.FirstOrDefault(x => x.Id == subSpoc.Subdomainspocid);
                    if (orgEntity != null)
                        subSpoc.Subdomainspoc = ApplicationUserMapper.GetApplicationUserMapper(orgEntity);
                }

                IQueryable<Lcmoperationalcontracts> lcmEngineeringOperationalContract = _repositoryWrapper.LCMOperationalContracts
                   .FindByCondition(x => x.Lcmid == lcm.LcmengineeringId);
                lcm.LCMOperationContracts = lcmEngineeringOperationalContract.ToList().Select(p => LCMOperationalContractsMapper.Get(p)).ToList();
                IQueryable<Operationalcontracts> opertionalContractResource = _repositoryWrapper.OperationalContract.FindAll();
                foreach (LcmOperationalContracts contract in lcm.LCMOperationContracts)
                {
                    var operationalEntity = opertionalContractResource.FirstOrDefault(x => x.Id == contract.OperationalContractId);
                    if (operationalEntity != null)
                        contract.OperationalContract = OperationalContractMapper.GetOperationalContractMapper(operationalEntity);
                }
                IQueryable<Lcmancillarydata> lcmAuditAttributes = _repositoryWrapper.LcmAncillaryData.FindByCondition(x => x.Lcmengineeringid == lcm.LcmengineeringId);
                lcm.LcmAncillaryData = lcmAuditAttributes.ToList().Select(x => LcmAncillaryDataMapper.Get(x)).ToList();

            }

            IEnumerable<LcmEngineeringDtoGrid> lcmEngineeringdResult;

            lcmEngineeringdResult = _mapper.Map<IEnumerable<LcmEngineeringDtoGrid>>(data);

            rtn.Items = lcmEngineeringdResult.ToArray();

            return rtn;
        }
        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, LcmEngineeringQueryDto buildFilterDto,bool isAdmin)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ApplyFilter(buildFilterDto);

            IQueryable<Lcmengineering> query = await Task.Run(() => GetQuery(predicateResult, false).Result.AsQueryable()
                .Where(x => x.Archived == !ConstantValueFilter.isTrue || x.Archived == null));


            List<FilterValueDto> rtn = propertyName switch
            {
                "warranty" =>
                    string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Warranty == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.Warranty.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.Warranty ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Warranty == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.Warranty.ToString() }).Distinct()
                        .ToList(),

                "archived" =>
           string.IsNullOrEmpty(propertyFilter)
           ? query.Where(x => x.Archived != null)
               .Select(p => new FilterValueDto { Text = p.Archived == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.Archived.ToString() }).Distinct().ToList()
           : query
               .Where(p => p.Archived != null && (p.Archived.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
               .Select(p => new FilterValueDto { Text = p.Archived == false ? ConstantValueFilter.No.ToUpper() : ConstantValueFilter.Yes.ToUpper(), Value = p.Archived.ToString() }).Distinct()
               .ToList(),
                "isLcmAncillaryData"
           => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes.ToUpper()), new FilterValueDto(ConstantValueFilter.No.ToUpper()) },

                "hardwareSupportProvider" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               {
                   Text = p.Hardwaresupportprovider,
                   Value = p.Hardwaresupportprovider
               }).Distinct().ToList()
                : query
                    .Where(x => x.Hardwaresupportprovider == propertyFilter).Select(p => new FilterValueDto
                    {
                        Text = p.Hardwaresupportprovider,
                        Value = p.Hardwaresupportprovider
                    }).Distinct().ToList(),
                "hardwareSupportType" => string.IsNullOrEmpty(propertyFilter)
                ? query
                .Select(p => new FilterValueDto { Text = p.Hardwaresupporttype, Value = p.Hardwaresupporttype }).Distinct()
                .ToList()
                : query
                .Where(x => x.Hardwaresupporttype.Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.Hardwaresupporttype, Value = p.Hardwaresupporttype }).Distinct()
                .ToList(),
                "softwareSupportProvider" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                {
                    Text = p.Softwaresupportprovider,
                    Value = p.Softwaresupportprovider
                }).Distinct().ToList()
                : query
                    .Where(x =>
                        x.Softwaresupportprovider == propertyFilter).Select(p => new FilterValueDto
                        {
                            Text = p.Softwaresupportprovider,
                            Value = p.Softwaresupportprovider
                        }).Distinct().ToList(),
                "softwareSupportType" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto
                { Text = p.Softwaresupporttype, Value = p.Softwaresupporttype }).Distinct().ToList()
                : query
                .Where(x => x.Softwaresupporttype.Contains(propertyFilter)).Select(p =>
                new FilterValueDto
                { Text = p.Softwaresupporttype, Value = p.Softwaresupporttype }).Distinct()
                .ToList(),
                "productImportanceId" => string.IsNullOrEmpty(propertyFilter)
                    ? _repositoryWrapper.ProductImportance.FindAll().Select(x => new FilterValueDto(x.Productimportance)).ToList() :
                      _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportance.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Productimportance)).ToList(),



                "operationalContact" => string.IsNullOrEmpty(propertyFilter)
                    ? query.SelectMany(x => x.Lcmoperationalcontracts).Select(p => new FilterValueDto(p.Operationalcontract.Description)).Distinct().ToList()
                    : query.Where(x => x.Lcmoperationalcontracts.Any(s => s.Operationalcontract.Description.Contains(propertyFilter)))
                        .SelectMany(x => x.Lcmoperationalcontracts)
                        .Select(p =>
                            new FilterValueDto(p.Operationalcontract.Description)).Distinct()
                        .ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationuserNavigation.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList(),


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


                "designComponentFamilyId" => string.IsNullOrEmpty(propertyFilter)
                      ? query.Select(p => new FilterValueDto
                      { Text = p.Designcomponent.Designcomponentfamilyid.ToString(), Value = p.Designcomponent.Designcomponentfamilyid.ToString() }).Distinct()
                          .ToList()
                      : query
                          .Where(x => x.Designcomponent.Designcomponentfamilyid.ToString().Contains(propertyFilter)).Select(p =>
                              new FilterValueDto
                              { Text = p.Designcomponent.Designcomponentfamilyid.ToString(), Value = p.Designcomponent.Designcomponentfamilyid.ToString() })
                          .Distinct()
                          .ToList(),
                "lcmEngineeringId" => query.Select(p => new FilterValueDto(p.Lcmengineeringid)).Distinct().ToList(),
                "lcmDeploymentStatus" => string.IsNullOrEmpty(propertyFilter)
                                  ? query.Select(p => new FilterValueDto
                                  { Text = p.Lcmdeploymentstatus.Description, Value = p.Lcmdeploymentstatusid.ToString() }).Distinct()
                                  .ToList()
                                  : query
                                  .Where(x => x.Lcmdeploymentstatusid.ToString().Contains(propertyFilter)).Select(p =>
                                      new FilterValueDto
                                      { Text = p.Lcmdeploymentstatus.Description, Value = p.Lcmdeploymentstatusid.ToString() })
                                  .Distinct()
                                  .ToList(),


                "designComponent" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto
                    {
                        Text = p.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper),
                        Value = p.Designcomponentid.ToString()
                    }).Distinct().ToList()
                    : query.ToList()
                        .Where(x =>
                            x.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper).ToUpper().Contains(
                                propertyFilter.ToUpper())).Select(p => new FilterValueDto
                                {
                                    Text = p.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper),
                                    Value = p.Designcomponentid.ToString()
                                }).Distinct().ToList(),
                "vodafoneName" => string.IsNullOrEmpty(propertyFilter)
                            ? query.Select(p => new FilterValueDto
                            {
                                Text = p.Designcomponent.Subnetworkboundary.Vodafonename != null ? p.Designcomponent.Subnetworkboundary.Vodafonename.Description : "",
                                Value = p.Designcomponent.Subnetworkboundary.Vodafonename != null ? p.Designcomponent.Subnetworkboundary.Vodafonenameid.ToString() : ""
                            }).Distinct().ToList()
                            : query
                            .Where(x =>
                            x.Designcomponent.Subnetworkboundary.Vodafonename.Description.Contains(
                            propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.Designcomponent.Subnetworkboundary.Vodafonename != null ? p.Designcomponent.Subnetworkboundary.Vodafonename.Description : "",
                                Value = p.Designcomponent.Subnetworkboundary.Vodafonename != null ? p.Designcomponent.Subnetworkboundary.Vodafonenameid.ToString() : ""
                            }).Distinct().ToList(),

                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Opco.Opco,
                        Value = p.Opcoid.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Opco.Opco.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Opco.Opco,
                                    Value = p.Opcoid.ToString()
                                }).Distinct().ToList(),
                "numberOfNodes" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               {
                   Text = p.Numberofnodes.ToString(),
                   Value = p.Numberofnodes.ToString()
               }).Distinct().ToList()
               : query
                   .Where(x =>
                       x.Numberofnodes.ToString() == propertyFilter).Select(p => new FilterValueDto
                       {
                           Text = p.Numberofnodes.ToString(),
                           Value = p.Numberofnodes.ToString()
                       }).Distinct().ToList(),
                "numberOfNodesInLab" => query.Where(x => x.Networkelementsasplanned.FirstOrDefault() != null && x.Lcmengineeringid == x.Networkelementsasplanned.FirstOrDefault().Lcmengineeringid).Select(p => new FilterValueDto(p.Numberofnodesinlab.ToString())).Distinct().ToList(),

                "systemTypeId" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)

                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer)
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Designcomponent.Systemtype.toSystemTypeName(_repositoryWrapper),
                            Value = p.Designcomponent.Systemtypeid.ToString()
                        }).Distinct().ToList()
                    : query
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer).Where(x => x.Designcomponent.Systemtype.toSystemTypeName(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper()))
                        .Select(p => new FilterValueDto
                        {
                            Text = p.Designcomponent.Systemtype.toSystemTypeName(_repositoryWrapper),
                            Value = p.Designcomponent.Systemtypeid.ToString()
                        }).Distinct().ToList(),
                "plannedActivity" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().SelectMany(q => q.PlannedactivitiesLcmengineering.Select(x => new FilterValueDto
                    {
                        Value = x.Plannedactivityid.ToString(),
                        Text = x.GetPlannedAction(_repositoryWrapper)
                    }).ToList()).Distinct().Concat(query.Where(x => x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "0",
                                      }
                                   )).Distinct().ToList()
                    : query.ToList()
                        .Where(x =>
                            x.PlannedactivitiesLcmengineering.Any(s => s.Plannedimplementationyear.ToString().Contains(propertyFilter) || s.Activitystatus.Activitystatus.Contains(propertyFilter) || s.Planningactivitystatus.Planningactivitystatus.Contains(propertyFilter))).ToList()
                        .SelectMany(q => q.PlannedactivitiesLcmengineering.Select(x => new FilterValueDto
                        {
                            Value = x.Plannedactivityid.ToString(),
                            Text = x.GetPlannedAction(_repositoryWrapper)
                        }).ToList()).Distinct().Concat(query.Where(x => x.PlannedactivitiesLcmengineering != null && x.PlannedactivitiesLcmengineering.Count() <= 0)
                                   .Select(x =>

                                      new FilterValueDto
                                      {
                                          Text = "---",
                                          Value = "0",
                                      }
                                   )).Distinct().ToList(),

                "originalLcm" => string.IsNullOrEmpty(propertyFilter)
            ? query.ToList().SelectMany(q => q.PlannedactivitiesLcmengineering.Where(x => x.Originallcmengineeringid.HasValue && x.Originallcmengineeringid != 0)
            .Select(x =>
                      new
                      {
                          x.Originallcmengineeringid,
                          DC = x.Originallcmengineering.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper)
                      }).Distinct().Select(x => new FilterValueDto
                      {
                          Value = x.Originallcmengineeringid.ToString(),
                          Text = x.DC
                      }).ToList()).Distinct().ToList()
            : query.ToList().SelectMany(q => q.PlannedactivitiesLcmengineering.Where(x => x.Originallcmengineeringid.HasValue && x.Originallcmengineeringid != 0)
            .Select(x =>
                      new
                      {
                          x.Originallcmengineeringid,
                          DC = x.Originallcmengineering.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper)
                      }).Distinct().Where(x => x.DC.Contains(propertyFilter)).Select(x => new FilterValueDto
                      {
                          Value = x.Originallcmengineeringid.ToString(),
                          Text = x.DC
                      }).ToList()).Distinct().ToList(),

                "resourceKey" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Resourcekey != null).Select(p => new FilterValueDto
                    {
                        Text = p.Resourcekey,
                        Value = p.Resourcekey
                    }).Distinct().ToList()
                   : query
                     .Where(x => x.Resourcekey != null && x.Resourcekey.Contains(propertyFilter)).Select(p => new FilterValueDto
                     {
                         Text = p.Resourcekey,
                         Value = p.Resourcekey
                     }).ToList().Distinct().ToList(),
                "previousResourceKey" => string.IsNullOrEmpty(propertyFilter)
               ? query.Where(x => x.Previousresourcekey != null).Select(p => new FilterValueDto
               {
                   Text = p.Previousresourcekey,
                   Value = p.Previousresourcekey
               }).Distinct().ToList()
              : query
                .Where(x => x.Previousresourcekey != null && x.Previousresourcekey.Contains(propertyFilter)).Select(p => new FilterValueDto
                {
                    Text = p.Previousresourcekey,
                    Value = p.Previousresourcekey
                }).Distinct().ToList(),
                "isExtendedSupportOfferedByVendor" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes.ToUpper()), new FilterValueDto(ConstantValueFilter.No.ToUpper()) },
                "hwIsExtendedSupportOfferedByVendor" => new List<FilterValueDto> { new FilterValueDto(ConstantValueFilter.Yes.ToUpper()), new FilterValueDto(ConstantValueFilter.No.ToUpper()) },


                #region Org based edu , sub domain and vertical
                "eduspoc" => (string.IsNullOrEmpty(propertyFilter)
                    ? query.SelectMany(x => x.Lcmengineeringeduspoc).Where(x => x.Deleted == false).Select(p => new FilterValueDto
                    {
                        Text = p.Eduspoc.Email,
                        Value = p.Eduspocid.ToString()
                    }).Distinct().ToList()
                    : query.Where(x => x.Lcmengineeringeduspoc.Any(s => s.Eduspocid.ToString().Contains(propertyFilter)))
                        .SelectMany(x => x.Lcmengineeringeduspoc).Where(x => x.Deleted == false)
                        .Select(p =>
                            new FilterValueDto
                            {
                                Text = p.Eduspoc.Email,
                                Value = p.Eduspocid.ToString()
                            }).Distinct()
                        .ToList())?.Distinct()?.ToList(),
                "subDomainSpoc" =>(string.IsNullOrEmpty(propertyFilter)
                    ? query.SelectMany(x => x.Lcmengineeringsubdomainspoc).Select(p => new FilterValueDto
                    {
                        Text = p.Subdomainspoc.Email,
                        Value = p.Subdomainspocid.ToString()
                    }).Distinct().ToList()
                    : query.Where(x => x.Lcmengineeringsubdomainspoc.Any(s => s.Subdomainspocid.ToString().Contains(propertyFilter)))
                        .SelectMany(x => x.Lcmengineeringsubdomainspoc).Where(x => x.Deleted == false)
                        .Select(p =>
                            new FilterValueDto
                            {
                                Text = p.Subdomainspoc.Email,
                                Value = p.Subdomainspocid.ToString()
                            }).Distinct()
                        .ToList())?.Distinct()?.ToList(),

                "verticalName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuserverticalsUser.Any(r => r.Deleted == false  /*r.Opcoid == x.Opcoid*/) && t.Deleted == false
                    ).SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Verticalresponsible,
                        Value = p.Verticalresponsibleid.ToString()
                    }).Distinct().ToList()
                     .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList()
                    : query.SelectMany(x => x.Lcmengineeringsubdomainspoc.Where(t => t.Subdomainspoc.AspnetuserverticalsUser.Any(r => r.Deleted == false /* r.Organisation.Vertical.Ve == x.Opcoid*/) && t.Deleted == false)
                    .SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Verticalresponsible,
                        Value = p.Verticalresponsibleid.ToString()
                    }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                    .Concat(query.Where(x => x.Lcmengineeringeduspoc != null && x.Lcmengineeringeduspoc.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),

                #endregion

                "commentOnProjectStatus" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.ToList().SelectMany(q => q.PlannedactivitiesLcmengineering.Select(x => new FilterValueDto
                                   {
                                       Value = x.Plannedactivityid.ToString(),
                                       Text = x.GetAncillaryDataForNoPa(_repositoryWrapper, false)
                                   }).ToList()).Distinct().ToList()
                                   : query.ToList()
                                       .Where(x =>
                                           x.PlannedactivitiesLcmengineering.Any(s => s.Plannedimplementationyear.ToString().Contains(propertyFilter) || s.Activitystatus.Activitystatus.Contains(propertyFilter) || s.Planningactivitystatus.Planningactivitystatus.Contains(propertyFilter))).ToList()
                                       .SelectMany(q => q.PlannedactivitiesLcmengineering.Select(x => new FilterValueDto
                                       {
                                           Value = x.Plannedactivityid.ToString(),
                                           Text = x.GetAncillaryDataForNoPa(_repositoryWrapper, false)
                                       }).ToList()).Distinct().ToList(),

                "reasonForNoPlan" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.ToList().SelectMany(q => q.PlannedactivitiesLcmengineering.Select(x => new FilterValueDto
                                   {
                                       Value = x.Plannedactivityid.ToString(),
                                       Text = x.GetAncillaryDataForNoPa(_repositoryWrapper, true)
                                   }).ToList()).Distinct().ToList()
                                   : query.ToList()
                                       .Where(x =>
                                           x.PlannedactivitiesLcmengineering.Any(s => s.Plannedimplementationyear.ToString().Contains(propertyFilter) || s.Activitystatus.Activitystatus.Contains(propertyFilter) || s.Planningactivitystatus.Planningactivitystatus.Contains(propertyFilter))).ToList()
                                       .SelectMany(q => q.PlannedactivitiesLcmengineering.Select(x => new FilterValueDto
                                       {
                                           Value = x.Plannedactivityid.ToString(),
                                           Text = x.GetAncillaryDataForNoPa(_repositoryWrapper, true)
                                       }).ToList()).Distinct().ToList(),
                "buildBagDescription" => await query
                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Buildbag.Bagdescription.Contains(propertyFilter))
                   .Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescription(p.Buildbag), Value = p.Buildbagid.ToString() })
                   .Distinct()
                   .ToListAsync(),

                _ => new List<FilterValueDto>()
            };
            if (!isAdmin && (buildFilterDto.VerticalName!=null && buildFilterDto.VerticalName.Count > 0) && propertyName=="verticalName")
            {
                rtn=rtn.Where(x=>buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();    
            }

            return rtn;
        }
        private async Task<IQueryable<Lcmengineering>> GetQuery(ExpressionStarter<Lcmengineering> predicateResult, bool includeDeleted)
        {

            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Lcmengineering, Buildbags> query = await Task.Run(() => _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult, includeDeleted)
                    .Include(x => x.PlannedactivitiesLcmengineering)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Activitystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Responsibilityphase)
                    .Include(x => x.Lcmdeploymentstatus)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Vodafonename)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)

                    .Include(x => x.Opco)
                    .Include(x => x.Productimportance)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Reasoncheckboxresourcelcmengineeringhardware).ThenInclude(x => x.Reasoncheckboxresource)
                    .Include(x => x.Reasoncheckboxresourcelcmengineeringsoftware).ThenInclude(x => x.Reasoncheckboxresource)
                    .Include(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Originallcmengineering).ThenInclude(x => x.Designcomponent)
                    .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Vodafonename)
                     .Include(x => x.Buildbag)
                        );

            return query;

        }
        private Dictionary<string, Expression<Func<LcmEngineering, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<LcmEngineering, object>>[]>
            {
                ["lcmengineeringId"] = new Expression<Func<LcmEngineering, object>>[] { p => p.LcmengineeringId },
                ["designComponent"] = new Expression<Func<LcmEngineering, object>>[] { p => string.IsNullOrEmpty(p.DesignComponent.DesignComponentFamily.SubNetworkBoundary.Alias) ? p.DesignComponent.DesignComponentFamily.SubNetworkBoundary.Description : p.DesignComponent.DesignComponentFamily.SubNetworkBoundary.Alias },
                ["opCoId"] = new Expression<Func<LcmEngineering, object>>[] { p => p.OpCo.OpCoDescription },

                ["operationalContact"] = new Expression<Func<LcmEngineering, object>>[] { p => p.LCMOperationContracts.FirstOrDefault().OperationalContract.Description },
                ["hardwareSupportProvider"] = new Expression<Func<LcmEngineering, object>>[] { p => p.HardwareSupportProvider },
                ["hardwareSupportType"] = new Expression<Func<LcmEngineering, object>>[] { p => p.HardwareSupportType },
                ["softwareSupportProvider"] = new Expression<Func<LcmEngineering, object>>[] { p => p.SoftwareSupportProvider },
                ["softwareEndOfWarrantyDate"] = new Expression<Func<LcmEngineering, object>>[] { p => p.SoftwareEndOfWarrantyDate },
                ["softwareSupportType"] = new Expression<Func<LcmEngineering, object>>[] { p => p.SoftwareSupportType },

                ["numberOfNodes"] = new Expression<Func<LcmEngineering, object>>[] { p => p.NumberOfNodes },
                ["numberOfNodesInLab"] = new Expression<Func<LcmEngineering, object>>[] { p => p.NumberOfNodesInLab },

                ["plannedActivity"] = new Expression<Func<LcmEngineering, object>>[] { x => x.PlannedActivities.Select(x => x.PlannedActivityId).FirstOrDefault() },
                ["warranty"] = new Expression<Func<LcmEngineering, object>>[] { p => p.Warranty },
                ["lastModified"] = new Expression<Func<LcmEngineering, object>>[] { p => p.ModificationDate },
                ["hardwareEndOfSupportContract"] = new Expression<Func<LcmEngineering, object>>[] { p => p.HardwareEndOfSupportContract },
                ["softwareEndOfSupportContract"] = new Expression<Func<LcmEngineering, object>>[] { p => p.SoftwareEndOfSupportContract },

                ["lastModifiedBy"] = new Expression<Func<LcmEngineering, object>>[] { p => p.ModificationUserEntity.Email },
                ["designComponentId"] = new Expression<Func<LcmEngineering, object>>[] { p => p.DesignComponentId },
                ["lcmDeploymentStatusId"] = new Expression<Func<LcmEngineering, object>>[] { p => p.LCMDeploymentStatusId },
                ["lCMOperationContracts"] = new Expression<Func<LcmEngineering, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }
        public async Task<DateTime?> GetMajorSoftwareBuildEosMinorToDaydesignComponentId(int designComponentId)
        {
            var designContactEntity = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId).Include(x => x.Systemtype)
                .ThenInclude(x => x.Majorsoftwarebuilds).FirstOrDefaultAsync();
            return designContactEntity?.Systemtype?.Majorsoftwarebuilds?.Endofsupport;
        }
        public async Task<DateTime?> GetMajorHarwareBuildEosMinorToDaydesignComponentId(int designComponentId)
        {
            var test = await _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == designComponentId).Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).FirstOrDefaultAsync();

            var hardWareEOS = test.Systemtype?.Systemtypesmajorhardwarebuilds
                       ?.FirstOrDefault(m =>
                           m.Ismain &&
                           m.Deleted == false)?.Majorhardware.Endofsupport;


            return hardWareEOS;
        }
        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation(DataRemediationDto data)
        {
            List<long> lcmIDs = new();

            foreach (long item in data.DuplicatesId)
            {
                Lcmengineering lcm = await _repositoryWrapper.Lcmengineering
                    .FindByCondition(x => x.Lcmengineeringid == item, true)
                    .FirstOrDefaultAsync();

                var lcmEduSpocs = await _repositoryWrapper.LcmEngineeringEduSpoc
                    .FindByCondition(x => x.Lcmengineeringid == lcm.Lcmengineeringid, true)
                    .ToListAsync();

                foreach (Lcmengineeringeduspoc lcmEduSpoc in lcmEduSpocs)
                {
                    _repositoryWrapper.LcmEngineeringEduSpoc
                        .DeleteDeep(lcmEduSpoc);
                    _repositoryWrapper.Save();
                }

                List<Lcmengineeringsubdomainspoc> lcmSubDomaniSpocs = _repositoryWrapper.LcmEngineeringSubDomainSpoc
                    .FindByCondition(x => x.Lcmengineeringid == lcm.Lcmengineeringid, true)
                    .ToList();
                foreach (Lcmengineeringsubdomainspoc lcmSubDomaniSpoc in lcmSubDomaniSpocs)
                {
                    _repositoryWrapper.LcmEngineeringSubDomainSpoc
                        .DeleteDeep(lcmSubDomaniSpoc);
                    _repositoryWrapper.Save();
                }

                List<Lcmoperationalcontracts> lcmOpertionalContracts = _repositoryWrapper.LCMOperationalContracts
                    .FindByCondition(x => x.Lcmid == lcm.Lcmengineeringid, true).ToList();
                foreach (Lcmoperationalcontracts contract in lcmOpertionalContracts)
                {
                    _repositoryWrapper.LCMOperationalContracts
                        .DeleteDeep(contract);
                    _repositoryWrapper.Save();
                }

                List<Reasoncheckboxresourcelcmengineeringhardware> reasonsCheckboxResourceLcmEngineeringHardware = _repositoryWrapper.CheckboxResourceLcmEngineeringHardware
                    .FindByCondition(x => x.Lcmengineeringid == lcm.Lcmengineeringid)
                    .ToList();
                foreach (Reasoncheckboxresourcelcmengineeringhardware reasonHw in reasonsCheckboxResourceLcmEngineeringHardware)
                {
                    _repositoryWrapper.CheckboxResourceLcmEngineeringHardware
                        .DeleteDeep(reasonHw);
                    _repositoryWrapper.Save();
                }

                List<Reasoncheckboxresourcelcmengineeringsoftware> reasonCheckboxResourceLcmEngineeringSoftware = _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware
                    .FindByCondition(x => x.Lcmengineeringid == lcm.Lcmengineeringid)
                    .ToList();
                foreach (Reasoncheckboxresourcelcmengineeringsoftware reasonSw in reasonCheckboxResourceLcmEngineeringSoftware)
                {
                    _repositoryWrapper.CheckboxResourceLcmEngineeringSoftware
                        .DeleteDeep(reasonSw);
                    _repositoryWrapper.Save();
                }

                List<Plannedactivities> plannedActivitiesLcm = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == lcm.Lcmengineeringid && x.Archived != true, true).ToList();
                foreach (Plannedactivities planned in plannedActivitiesLcm)
                {
                    _repositoryWrapper.PlannedActivity
                        .DeleteDeep(planned);
                    _repositoryWrapper.Save();
                }
                _repositoryWrapper.Lcmengineering
                    .DeleteDeep(lcm);

            }
            return new ResultDto<ResultDataRemediationDto>()
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
            };
        }
        public async Task<Lcmengineering> SetLcmValue(Lcmengineering entity)
        {
            try
            {
                entity = await SetLcmSoftware(entity);

                entity = await SetLcmHardware(entity);

                #region PlannedActivity
                List<Plannedactivities> PlannedActivtiesLst = new();
                foreach (Plannedactivities plannedEntity in entity.PlannedactivitiesLcmengineering)
                {
                    Plannedactivities item = plannedEntity;
                    item = _plannedActivityManager.SetPlannedActivityValue(item);
                    item.Opcoid = entity.Opcoid;
                    item.Buildbagid = item.Buildbagid;
                    item.Plannedactivitycategoryid = plannedEntity.Plannedactivitycategoryid == 0 ? null : plannedEntity.Plannedactivitycategoryid;
                    item.Programid = plannedEntity.Programid == 0 ? null : plannedEntity.Programid;
                    PlannedActivtiesLst.Add(item);
                }

                entity.PlannedactivitiesLcmengineering = PlannedActivtiesLst;
                #endregion
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        private async Task<Lcmengineering> SetLcmHardware(Lcmengineering entity)
        {
            entity.Hardwaresupporttype =
               await entity.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareSupportType);
            entity.Outputtolcmhardware =
                await entity.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareOutput);
            entity.Hardwaresupportprovider =
                await entity.GetRuleHardware<string>(_repositoryWrapper,
                    LCMEngineeringRulesHardware.LCMHardwareSupportProvider);
            entity.Hardwareendofsupportcontract =
                await entity.GetRuleHardware<DateTime?>(_repositoryWrapper,
                    LCMEngineeringRulesHardware.LCMHardwareEndOfSupportContractDate);
            entity.Vendorendmntdatehw =
                await entity.GetRuleHardware<DateTime?>(_repositoryWrapper,
                    LCMEngineeringRulesHardware.LCMHardwareEndOfSupportContractOutputLcm);
            entity.Lcmstatusopshardware =
                await entity.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusOps);

            entity.Lcmstatusenghardware =
                await entity.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatusEng);

            entity.Lcmstatushardware =
                await entity.GetRuleHardware<string>(_repositoryWrapper, LCMEngineeringRulesHardware.LCMHardwareLcmStatus);
            return entity;
        }
        private async Task<Lcmengineering> SetLcmSoftware(Lcmengineering entity)
        {
            try
            {
                entity.Outputtolcmsoftware = await entity.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareOutput);
                ///Task 447 - Non deve essere cambiato il valore di warranty

                entity.Softwareendofwarrantydate =
                    await entity.GetRuleSoftware<DateTime?>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareEndofWarrantyDate);
                entity.Softwaresupporttype =
                    await entity.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareSupportType);
                entity.Softwaresupportprovider =
                    await entity.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareSupportProvider);
                entity.Lcmstatusopssoftware =
                    await entity.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusOps);
                entity.Lcmstatusengsoftware =
                    await entity.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatusEng);
                entity.Lcmstatussoftware =
                    await entity.GetRuleSoftware<string>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareLcmStatus);
                entity.Vendorendmntedatesw =
                    await entity.GetRuleSoftware<DateTime?>(_repositoryWrapper, LCMEngineeringRulesSoftware.LCMSoftwareEndOfSupportContractOutputLcm);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
        public async Task<ResultDto> ArchiveDesginAspectCasePARolledout(Lcmengineering lcmEntity, Plannedactivities rolledoutPlannedActivity)
        {
            try
            {
                Designaspects equivalentDesignAspect = null;
                long? equivalentLCM_DC_DCF = _repositoryWrapper.DesignComponentFamily
                    .FindByCondition(x => x.Designcomponents.Any(y => y.Designcomponentid == lcmEntity.Designcomponentid))
                    .FirstOrDefault()?.Designcomponentfamilyid;
                var dcList = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentfamilyid == equivalentLCM_DC_DCF).Select(x => x.Designcomponentid).ToList();
                var dcfAccosiatedLcmList = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == lcmEntity.Opcoid && dcList.Contains(x.Designcomponentid) && x.Archived != true).ToList();

                if (dcfAccosiatedLcmList.Count == 0)
                {
                    equivalentDesignAspect = _repositoryWrapper.DesignAspectRepository
                        .FindByCondition(x => x.Designcomponentfamilyid == equivalentLCM_DC_DCF
                        && x.Opcoid == lcmEntity.Opcoid && x.Archived != true).FirstOrDefault();


                    if (equivalentDesignAspect != null)
                    {


                        var designAspectPlannedActivities = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Designaspectid != null
                            && x.Designaspectid.Value == equivalentDesignAspect.Id && x.Opcoid == lcmEntity.Opcoid && x.Archived != true).Include(x => x.Plannedactivityresource).ToList();
                        if (designAspectPlannedActivities.Where(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Project_Plan).Any() == true)
                        { }
                        else
                        {
                            short? deliveryStatusCompleteId = _repositoryWrapper.SettingsUpdatePlannedActivity.FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.DesignAspect && x.Plannedactivityresourceid == rolledoutPlannedActivity.Plannedactivityresourceid && x.Deliverystatusid == rolledoutPlannedActivity.Deliverystatusid
                                                                && x.Ruleelementcount == (int)RuleElementCountEnum.RolloutComplete)
                                                                   .Include(x => x.Deliverystatus)?.FirstOrDefault()?.Deliverystatusid;
                            Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();

                            foreach (Plannedactivities pa in designAspectPlannedActivities)
                            {
                                pa.Archived = true;
                                pa.Deliverystatusid = deliveryStatusCompleteId;
                                pa.Activitystatusid = completedActivityStatus.Activitystatusid;
                                _repositoryWrapper.PlannedActivity.Update(pa);
                                await _repositoryWrapper.SaveAsync();
                            }
                            equivalentDesignAspect.Archived = true;
                            _repositoryWrapper.DesignAspectRepository.Update(equivalentDesignAspect);
                            await _repositoryWrapper.SaveAsync();
                        }


                    }
                    else if (equivalentDesignAspect == null)
                    {
                        _ = await CreateDesignAspectWithOPCOAndDCF(lcmEntity.Opcoid, equivalentLCM_DC_DCF);
                        Designaspects designAspect = _repositoryWrapper.DesignAspectRepository
                       .FindByCondition(x => x.Designcomponentfamilyid == equivalentLCM_DC_DCF
                       && x.Opcoid == lcmEntity.Opcoid).FirstOrDefault();
                        designAspect.Archived = true;
                        _repositoryWrapper.DesignAspectRepository.Update(designAspect);
                        await _repositoryWrapper.SaveAsync();
                    }
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = equivalentDesignAspect
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Issue happen when try to Archive Design Aspect  : " + ex);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }



        public async Task<ResultDto> MoveActivePAs(long Lcmengineeringid, long plannedActivityDC)
        {
            List<Plannedactivities> activePlannedActivites = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == Lcmengineeringid && (x.Archived == false || x.Archived == null)).ToList();
            foreach (Plannedactivities pa in activePlannedActivites)
            {
                Lcmengineering plannedLCM = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == pa.Opcoid && x.Designcomponentid == plannedActivityDC && x.Archived != true).FirstOrDefault();
                if (plannedLCM == null)
                {
                    return null;
                }
                // if new lcm has PA similare to the moved PA archived the moved PA 
                Plannedactivities PAExist = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Designcomponentid == pa.Designcomponentid && x.Opcoid == pa.Opcoid && x.Plannedactivityresourceid == pa.Plannedactivityresourceid && x.Lcmengineeringid == plannedLCM.Lcmengineeringid && x.Archived != true).FirstOrDefault();
                if (PAExist != null && PAExist.Ispareleasedetailunknown == false)
                {
                    pa.Archived = true;
                    _repositoryWrapper.PlannedActivity.Update(pa);
                    await _repositoryWrapper.SaveAsync();
                }
                else
                {
                    pa.Originallcmengineeringid = Lcmengineeringid;

                    pa.Lcmengineeringid = plannedLCM?.Lcmengineeringid;

                    _repositoryWrapper.PlannedActivity.Update(pa);
                    await _repositoryWrapper.SaveAsync();
                }

            }

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
            };

        }
        public async Task<ResultDto> RolloutPlannedActivityFromDDl(Plannedactivities data)
        {
            try
            {
                Plannedactivities plannedActivityEntity = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == data.Plannedactivityid, true, false)
               .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringeduspoc)
               .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
               .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc)
               .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmoperationalcontracts)
               .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringhardware)
               .Include(x => x.Lcmengineering).ThenInclude(x => x.Reasoncheckboxresourcelcmengineeringsoftware)
               .Include(x => x.Plannedactivityresource)
               .FirstOrDefault();

                Lcmengineering originalLcm = plannedActivityEntity.Lcmengineering;

                short? inServiceStatus = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.InService).FirstOrDefault()?.Id;

                if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc is ((int)PlannedActivityResourceEnum.New_NFxI_Solution)
                  or ((int)PlannedActivityResourceEnum.New_System_HW_SW_Solution))
                {
                    // LCM Part 3 Requirements
                    if (originalLcm != null)
                    {
                        originalLcm.Resourcekey = _designComponentFamilyLifeCycleManager.CreateDCFLifecycleforNewSolution(originalLcm, plannedActivityEntity).Result.Data.ToString();
                    }
                    originalLcm.Lcmdeploymentstatusid = inServiceStatus;
                    _repositoryWrapper.Lcmengineering.Update(originalLcm);
                    await _repositoryWrapper.SaveAsync();

                }
                else
                {
                    short RemovedDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
                    .FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Removed).Select(x => x.Id).FirstOrDefault();



                    originalLcm.Archived = true;

                    originalLcm.Resourcekey = originalLcm.Resourcekey; ///resourceKey[0] + "_FF"; April 17 2024 suggested by Sathish


                    Lcmengineering lcmExists = await _repositoryWrapper.Lcmengineering
                      .FindByCondition(x => x.Archived != true && x.Designcomponentid == plannedActivityEntity.Designcomponentid
                                            && x.Opcoid == plannedActivityEntity.Lcmengineering.Opcoid, false, false)
                      .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();


                    int numberOfNodesOutput = originalLcm.CountNetworkElementReleated(false, _repositoryWrapper);
                    int numberOfNodesInLabOutput = originalLcm.CountNetworkElementReleated(true, _repositoryWrapper);

                    if (lcmExists == null)
                    {
                        #region //Ticket 687 EDU/Sub domain SPOC details are not copied to new LCM while doinf SW upgrade/modernize flow

                        if (data.Lcmengineering.Lcmengineeringsubdomainspoc != null
                            && data.Lcmengineering.Lcmengineeringsubdomainspoc.Count() != 0)
                        {
                            plannedActivityEntity.Lcmengineering.Lcmengineeringsubdomainspoc = data.Lcmengineering.Lcmengineeringsubdomainspoc;
                        }

                        if (data.Lcmengineering.Lcmengineeringeduspoc != null
                          && data.Lcmengineering.Lcmengineeringeduspoc.Count() != 0)
                        {
                            plannedActivityEntity.Lcmengineering.Lcmengineeringeduspoc = data.Lcmengineering.Lcmengineeringeduspoc;
                        }

                        #endregion

                        Lcmengineering newLCM = await CreateLcmAndPA(numberOfNodesOutput, numberOfNodesInLabOutput, plannedActivityEntity);

                        if (originalLcm.Elementcount)
                        {
                            await CompletePlannedActivity(originalLcm, newLCM, plannedActivityEntity);
                        }
                        originalLcm.Numberofnodesinlab -= numberOfNodesInLabOutput;
                        originalLcm.Numberofnodes -= numberOfNodesOutput;
                        await _repositoryWrapper.ClearTracker();
                        originalLcm.Lcmdeploymentstatusid = RemovedDeploymentStatusId;
                        _repositoryWrapper.Lcmengineering.Update(originalLcm);
                        await _repositoryWrapper.SaveAsync();

                    }
                    else
                    {
                        string PADeliveryStatus = _repositoryWrapper.DeliveryStatus
                            .FindByCondition(x => x.Deliverystatusid == plannedActivityEntity.Deliverystatusid)
                            .Select(x => x.Deliverystatus)
                            .FirstOrDefault();

                        short? InServiceLcmDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
                             .FindByCondition(x => x.Description.ToLower() == ConstantValueFilter.InService)
                             .Select(x => x.Id)
                             .FirstOrDefault();

                        lcmExists.Lcmdeploymentstatusid ??= originalLcm?.Lcmdeploymentstatusid != null ? originalLcm?.Lcmdeploymentstatusid : InServiceLcmDeploymentStatusId;

                        lcmExists.Numberofnodes += numberOfNodesOutput;
                        lcmExists.Numberofnodesinlab += numberOfNodesInLabOutput;

                        _repositoryWrapper.Lcmengineering.Update(lcmExists);
                        await _repositoryWrapper.SaveAsync();
                        await _repositoryWrapper.ClearTracker();
                        if (lcmExists.Elementcount)
                            await CompletePlannedActivity(originalLcm, lcmExists, plannedActivityEntity);


                        originalLcm.Numberofnodesinlab -= numberOfNodesInLabOutput;
                        originalLcm.Numberofnodes -= numberOfNodesOutput;

                        await _repositoryWrapper.ClearTracker();
                        originalLcm.Lcmdeploymentstatusid = RemovedDeploymentStatusId;

                        _repositoryWrapper.Lcmengineering.Update(originalLcm);
                        await _repositoryWrapper.SaveAsync();

                    }

                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = plannedActivityEntity
                };
            }
            catch (Exception e)
            {
                _logger.LogError("Issue happen when try to update PA status : " + e);
                return new ResultDto() { Info = ResultMessages.SystemError, Warning = true };
            }

        }

        public async Task CompletePlannedActivity(Lcmengineering lcmengineering, Lcmengineering newLCM, Plannedactivities plannedActivityEntity)
        {
            if (newLCM != null)
            {
                List<Networkelementsasplanned> nodes = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(p => p.Designcomponentid == lcmengineering.Designcomponentid && p.Opcoid == lcmengineering.Opcoid && p.Deploymentstatus.Deploymentstatus.ToLower().Replace(" ", "") == "In-Service".ToLower().Replace(" ", ""))
                    .Include(x => x.Deploymentstatus).ToList();

                long? currentDCF = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == lcmengineering.Designcomponentid).Select(x => x.Designcomponentfamilyid).FirstOrDefault();
                long? plannedDCF = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == newLCM.Designcomponentid).Select(x => x.Designcomponentfamilyid).FirstOrDefault();

                foreach (Networkelementsasplanned networkElementAssociated in nodes)
                {
                    networkElementAssociated.Buildbagid = newLCM.Buildbagid;
                    ///(LCM R8 Part 3 requirements)                    //Check for refactor use cases
                    if (plannedActivityEntity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Refactor)
                    {
                        _ = await _resourceKeyMasterManager.UpdateResourcekeyMasterForRefactor(networkElementAssociated.Swresourcekey, plannedDCF, newLCM.Opcoid);
                        _ = await _resourceKeyMasterManager.UpdateResourcekeyMasterForRefactor(networkElementAssociated.Hwresourcekey, plannedDCF, newLCM.Opcoid);
                    }
                    else if (currentDCF != plannedDCF)
                    {
                        string swResourceKey = string.Empty;
                        string hwResourceKey = string.Empty;

                        ///Passing the new LCM Entity design component will validate against the existing entry
                        ///and there will not be an entry, if the DCF changes so a new resourcekey will be assigned.
                        IDictionary<int, string> AssetKeys = await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforAssets(newLCM.Designcomponentid, networkElementAssociated.Opcoid, networkElementAssociated.Elementname, networkElementAssociated.Buildbagid);
                        if (AssetKeys != null)
                        {
                            swResourceKey = AssetKeys[(int)ResourceTypesKey.SWAsset];
                            hwResourceKey = AssetKeys[(int)ResourceTypesKey.HWAsset];
                            networkElementAssociated.Swresourcekey = swResourceKey;
                            networkElementAssociated.Hwresourcekey = hwResourceKey;
                        }
                    }
                    ///End of (LCM R8 Part 3 requirements)
                    networkElementAssociated.Designcomponentid = newLCM.Designcomponentid;
                    networkElementAssociated.Lcmengineeringid = newLCM?.Lcmengineeringid;

                    _repositoryWrapper.NetworkElementAsPlanned.Update(networkElementAssociated);
                }
            }
            await _repositoryWrapper.SaveAsync();
        }

        #region
        private static ExpressionStarter<Lcmengineering> ArchivedLcmApplyFilter(ArchivedLcmEngineeringQueryDto buildFilterDto)
        {
            ExpressionStarter<Lcmengineering> predicateResult = PredicateBuilder.New<Lcmengineering>(true);

            ExpressionStarter<Lcmengineering> predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            if (buildFilterDto.LcmEngineeringId != null && buildFilterDto.LcmEngineeringId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.LcmEngineeringId)
                {
                    _ = predicateInner.Or(x => x.Lcmengineeringid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OpCo != null && buildFilterDto.OpCo.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (short item in buildFilterDto.OpCo)
                {
                    _ = predicateInner.Or(x => x.Opcoid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LcmDeploymentStatus != null && buildFilterDto.LcmDeploymentStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (short item in buildFilterDto.LcmDeploymentStatus)
                {
                    _ = predicateInner.Or(x => x.Lcmdeploymentstatusid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignComponentFamily != null && buildFilterDto.DesignComponentFamily.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.DesignComponentFamily)
                {
                    _ = predicateInner.Or(x => x.Designcomponent.Designcomponentfamilyid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.LastModifiedBy)
                {
                    _ = predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignComponent != null && buildFilterDto.DesignComponent.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.DesignComponent)
                {
                    _ = predicateInner.Or(x => x.Designcomponentid == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DesignComponentId != null && buildFilterDto.DesignComponentId.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.DesignComponentId)
                {
                    _ = predicateInner.Or(x => x.Designcomponentid == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VodafoneName != null && buildFilterDto.VodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.VodafoneName)
                {
                    _ = predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonenameid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SubnetworkBoundary != null && buildFilterDto.SubnetworkBoundary.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (long item in buildFilterDto.SubnetworkBoundary)
                {
                    _ = predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundaryid == item);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                {
                    _ = predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                }

                if (buildFilterDto.LastModifiedValue.EndDate != null)
                {
                    _ = predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.StartDate != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.StartDate.StartDate != null)
                {
                    _ = predicateInner.And(x => x.PlannedactivitiesLcmengineering.Any(p => p.Startdate != null && p.Startdate.Value.Date >= buildFilterDto.StartDate.StartDate));
                }

                if (buildFilterDto.StartDate.EndDate != null)
                {
                    _ = predicateInner.And(x => x.PlannedactivitiesLcmengineering.Any(p => p.Startdate != null && p.Startdate.Value.Date <= buildFilterDto.StartDate.EndDate));
                }

                _ = predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EndDate != null)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                if (buildFilterDto.EndDate.StartDate != null)
                {
                    _ = predicateInner.And(x => x.PlannedactivitiesLcmengineering.Any(p => p.Plannedcompletion != null && p.Plannedcompletion.Value.Date >= buildFilterDto.EndDate.StartDate));
                }

                if (buildFilterDto.EndDate.EndDate != null)
                {
                    _ = predicateInner.And(x => x.PlannedactivitiesLcmengineering.Any(p => p.Plannedcompletion != null && p.Plannedcompletion.Value.Date <= buildFilterDto.EndDate.EndDate));
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ResourceKey != null && buildFilterDto.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (string item in buildFilterDto.ResourceKey)
                {
                    _ = predicateInner.Or(x => x.Resourcekey == item);
                }

                _ = predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Any())
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                int count = 0;

                foreach (string item in buildFilterDto.VerticalName)
                {
                    if (item == "yes")
                    {
                        count++;

                        _ = predicateInner.Or(x => !x.Lcmengineeringsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        _ = predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser.Any(m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false /*&& m.Opcoid == x.Opcoid*/)));

                    }
                }

                if (count > 0)
                {
                    _ = predicateResult.And(predicateInner);
                }
            }
            if (buildFilterDto.BuildBagDescription?.Any() == true)
            {
                ExpressionStarter<Lcmengineering> descriptionPredicate = PredicateBuilder.New<Lcmengineering>();
                foreach (long description in buildFilterDto.BuildBagDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Buildbag.Buildbagid == description);
                }

                _ = predicateResult.And(descriptionPredicate);
            }


            return predicateResult;
        }
        public async Task<QueryResultDto<ArchivedLcmengineeringDtoGrid>> ArchivedLcmFindWithConditionAsync(ArchivedLcmEngineeringQueryDto designComponentFilterDto)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ArchivedLcmApplyFilter(designComponentFilterDto);

            IQueryable<Lcmengineering> items = await Task.Run(() => GetQuery(predicateResult, designComponentFilterDto.Deleted ?? false).Result.AsQueryable()
                .Where(x => x.Archived == ConstantValueFilter.isTrue));

            QueryResultDto<ArchivedLcmengineeringDtoGrid> rtn = new(new GenerateRenderForGrid<ArchivedLcmengineeringDtoGrid>(_manager))
            {
            };
            List<LcmEngineering> mappedItems = new();

            rtn.TotalItems = items.Count();
            if (designComponentFilterDto.PageSize == 0)
            {
                designComponentFilterDto.PageSize = rtn.TotalItems;
                designComponentFilterDto.Page = 1;
            }

            items = items.OrderByDescending(x => x.Modificationdate).Skip((designComponentFilterDto.Page - 1) * designComponentFilterDto.PageSize).Take(designComponentFilterDto.PageSize);

            IEnumerable<Lcmengineering> data = items.ToList();
            IEnumerable<ArchivedLcmengineeringDtoGrid> lcmEngineeringdResult;

            #region filter for Org based edu,subdomain and vertical

            ExpressionStarter<Lcmengineeringsubdomainspoc> eduPredicateResult = PredicateBuilder.New<Lcmengineeringsubdomainspoc>(true);
            ExpressionStarter<Lcmengineeringsubdomainspoc> eduPredicateInner = PredicateBuilder.New<Lcmengineeringsubdomainspoc>(true);

            if (designComponentFilterDto.VerticalName != null && designComponentFilterDto.VerticalName.Any())
            {
                eduPredicateInner = PredicateBuilder.New<Lcmengineeringsubdomainspoc>();
                foreach (string item in designComponentFilterDto.VerticalName)
                {
                    _ = eduPredicateInner.Or(x => x.Subdomainspoc.AspnetuserverticalsUser.Any(m => m.Organisation.Vertical.Verticalresponsibleid.ToString() == item && m.Deleted == false /*&& m.Opcoid == x.Lcmengineering.Opcoid*/));
                }

                _ = eduPredicateResult.And(eduPredicateInner);
            }

            #endregion

            foreach (Lcmengineering lcm in data)
            {

                List<Lcmengineeringsubdomainspoc> lcmEngineeringSubdomainSpoc = _repositoryWrapper.LcmEngineeringSubDomainSpoc.FindByCondition(eduPredicateInner)
                    .Where(x => x.Lcmengineeringid == lcm.Lcmengineeringid).Include(x => x.Subdomainspoc).ToList();

                lcm.Lcmengineeringsubdomainspoc = lcmEngineeringSubdomainSpoc;

            }

            lcmEngineeringdResult = await Task.Run(() => data.Select(x =>
        new ArchivedLcmengineeringDtoGrid
        {
            OpCo = x.Opco.Opco,
            DesignComponent = x.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper),
            DesignComponentId = x.Designcomponentid,
            LcmEngineeringId = x.Lcmengineeringid,
            DesignComponentFamily = x.Designcomponent.toDesignComponentFamily(_repositoryWrapper),
            VodafoneName = x.Designcomponent.Designcomponentfamily.Subnetworkboundary?.Vodafonename?.Description,
            SubnetworkBoundary = !string.IsNullOrEmpty(x.Designcomponent.Designcomponentfamily.Subnetworkboundary?.Alias) ? x.Designcomponent.Designcomponentfamily.Subnetworkboundary?.Alias : x.Designcomponent.Designcomponentfamily.Subnetworkboundary?.Description,
            StartDateValue = x.PlannedactivitiesLcmengineering.FirstOrDefault()?.Startdate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDateValue = x.PlannedactivitiesLcmengineering.FirstOrDefault()?.Plannedcompletion?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            Archived = x.Archived == null || x.Archived.Value,
            LastModifiedValue = x.Modificationdate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            LastModifiedBy = x.ModificationuserNavigation.Email,
            LcmDeploymentStatus = x.Lcmdeploymentstatus != null ? x.Lcmdeploymentstatus.Description : "",
            VerticalName = _commonManager.GetVerticaleNameRes(x.Lcmengineeringsubdomainspoc.Select(x => x?.Subdomainspocid).ToList(), x.Opcoid),
            NumberOfNodes = x.Numberofnodes,
            NumberOfNodesInLab = x.Numberofnodesinlab,
            ResourceKey = x.Resourcekey,
            BuildBagDescription = _commonManager.GetBuildBagDescription(x.Buildbag),

        }
        ));

            rtn.Items = lcmEngineeringdResult.ToArray();

            return rtn;
        }
        public async Task<List<FilterValueDto>> ArchivedLcmFilter(string propertyName, string propertyFilter, ArchivedLcmEngineeringQueryDto buildFilterDto,bool isAdmin)
        {
            ExpressionStarter<Lcmengineering> predicateResult = ArchivedLcmApplyFilter(buildFilterDto);

            IQueryable<Lcmengineering> query = await Task.Run(() => GetQuery(predicateResult, buildFilterDto.Deleted ?? false).Result.AsQueryable()
                .Where(x => x.Archived == ConstantValueFilter.isTrue));

            List<FilterValueDto> rtn = propertyName switch
            {
                "lcmEngineeringId" => query.Select(x => new FilterValueDto { Text = x.Lcmengineeringid.ToString(), Value = x.Lcmengineeringid.ToString() }).Distinct().ToList(),
                "archived" =>
           string.IsNullOrEmpty(propertyFilter)
                      ? query.Where(p => p.Archived != null)
                          .Select(p => new FilterValueDto { Text = p.Archived.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(), Value = p.Archived.ToString() }).Distinct().ToList()
                      : query
                          .Where(p => (p.Archived != null && p.Archived.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper()).Contains(propertyFilter))
                          .Select(p => new FilterValueDto { Text = p.Archived.Value ? ConstantValueFilter.Yes.ToUpper() : ConstantValueFilter.No.ToUpper(), Value = p.Archived.ToString() }).Distinct().ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationuserNavigation.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList(),

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


                "designComponent" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto
                    {
                        Text = p.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper),
                        Value = p.Designcomponentid.ToString()
                    }).Distinct().ToList()
                    : query.ToList()
                        .Where(x =>
                            x.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper).ToUpper().Contains(
                                propertyFilter.ToUpper())).Select(p => new FilterValueDto
                                {
                                    Text = p.Designcomponent.toDesignComponentNameLcm(_repositoryWrapper),
                                    Value = p.Designcomponentid.ToString()
                                }).Distinct().ToList(),
                "designComponentFamily" => string.IsNullOrEmpty(propertyFilter)
                ? query.ToList().Select(p => new FilterValueDto
                {
                    Text = p.Designcomponent.toDesignComponentFamily(_repositoryWrapper),
                    Value = p.Designcomponent.Designcomponentfamilyid.ToString()
                }).Distinct().ToList()
                : query.ToList()
                    .Where(x =>
                        x.Designcomponent.toDesignComponentFamily(_repositoryWrapper).ToUpper().Contains(
                            propertyFilter.ToUpper())).Select(p => new FilterValueDto
                            {
                                Text = p.Designcomponent.toDesignComponentFamily(_repositoryWrapper),
                                Value = p.Designcomponent.Designcomponentfamilyid.ToString()
                            }).Distinct().ToList(),


                "opCo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Opco.Opco,
                        Value = p.Opcoid.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Opco.Opco.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Opco.Opco,
                                    Value = p.Opcoid.ToString()
                                }).Distinct().ToList(),
                "vodafoneName" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.Where(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonename != null).Select(p => new FilterValueDto
                                   {
                                       Text = p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonename.Description,
                                       Value = p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonename.Id.ToString()
                                   }).Distinct().ToList()
                                   : query
                                       .Where(x => x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonename != null &&
                                           x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonename.Description.Contains(
                                               propertyFilter)).Select(p => new FilterValueDto
                                               {
                                                   Text = p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonename.Description,
                                                   Value = p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Vodafonename.Id.ToString()
                                               }).Distinct().ToList(),
                "subnetworkBoundary" => string.IsNullOrEmpty(propertyFilter)
                                                 ? query.Select(p => new FilterValueDto
                                                 {
                                                     Text = !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias) ? p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias : p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description,
                                                     Value = p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Id.ToString()
                                                 }).Distinct().ToList()
                                                 : query
                                                     .Where(x => !string.IsNullOrEmpty(x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias) ?
                                                         x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias.Contains(propertyFilter) : x.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description.Contains(propertyFilter))
                                                     .Select(p => new FilterValueDto
                                                     {
                                                         Text = !string.IsNullOrEmpty(p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias) ? p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Alias : p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Description,
                                                         Value = p.Designcomponent.Designcomponentfamily.Subnetworkboundary.Id.ToString()
                                                     }).Distinct().ToList(),
                "lcmDeploymentStatus" => string.IsNullOrEmpty(propertyFilter)
                                                ? query.Select(p => new FilterValueDto
                                                { Text = p.Lcmdeploymentstatus.Description, Value = p.Lcmdeploymentstatusid.ToString() }).Distinct()
                                                .ToList()
                                                : query
                                                .Where(x => x.Lcmdeploymentstatusid.ToString().Contains(propertyFilter)).Select(p =>
                                                new FilterValueDto
                                                { Text = p.Lcmdeploymentstatus.Description, Value = p.Lcmdeploymentstatusid.ToString() })
                                                .Distinct()
                                                .ToList(),

                "numberOfNodes" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               {
                   Text = p.Numberofnodes.ToString(),
                   Value = p.Numberofnodes.ToString()
               }).Distinct().ToList()
               : query
                   .Where(x =>
                       x.Numberofnodes.ToString() == propertyFilter).Select(p => new FilterValueDto
                       {
                           Text = p.Numberofnodes.ToString(),
                           Value = p.Numberofnodes.ToString()
                       }).Distinct().ToList(),
                "numberOfNodesInLab" => string.IsNullOrEmpty(propertyFilter)
               ? query.Select(p => new FilterValueDto
               {
                   Text = p.Numberofnodesinlab.ToString(),
                   Value = p.Numberofnodesinlab.ToString()
               }).Distinct().ToList()
               : query
                   .Where(x =>
                       x.Numberofnodesinlab.ToString() == propertyFilter).Select(p => new FilterValueDto
                       {
                           Text = p.Numberofnodesinlab.ToString(),
                           Value = p.Numberofnodesinlab.ToString()
                       }).Distinct().ToList(),
                "resourceKey" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Resourcekey != null).Select(p => new FilterValueDto
                    {
                        Text = p.Resourcekey,
                        Value = p.Resourcekey
                    }).Distinct().ToList()
                   : query
                     .Where(x => x.Resourcekey != null && x.Resourcekey.Contains(propertyFilter)).Select(p => new FilterValueDto
                     {
                         Text = p.Resourcekey,
                         Value = p.Resourcekey
                     }).ToList().Distinct().ToList(),
                "verticalName" => string.IsNullOrEmpty(propertyFilter)
                                   ? query.SelectMany(x => x.Lcmengineeringsubdomainspoc.SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                   .Select(p => new FilterValueDto
                                   {
                                       Text = p.Verticalresponsible,
                                       Value = p.Verticalresponsibleid.ToString()
                                   }).Distinct().ToList()
                                   .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                                        .Select(x =>

                                           new FilterValueDto
                                           {
                                               Text = "---",
                                               Value = "yes",
                                           }
                                        )).Distinct().ToList()
                                   : query.SelectMany(x => x.Lcmengineeringsubdomainspoc.SelectMany(y => y.Subdomainspoc.AspnetuserverticalsUser.Select(i => i.Organisation.Vertical))).ToList()
                                   .Select(p => new FilterValueDto
                                   {
                                       Text = p.Verticalresponsible,
                                       Value = p.Verticalresponsibleid.ToString()
                                   }).Where(x => x.Text.Contains(propertyFilter)).Distinct().ToList()
                                   .Concat(query.Where(x => x.Lcmengineeringsubdomainspoc != null && x.Lcmengineeringsubdomainspoc.Count() <= 0)
                                        .Select(x =>

                                           new FilterValueDto
                                           {
                                               Text = "---",
                                               Value = "yes",
                                           }
                                        )).Distinct().ToList(),

                "buildBagDescription" => await query
                       .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Buildbag.Bagdescription.Contains(propertyFilter))
                       .Select(p => new FilterValueDto { Text = _commonManager.GetBuildBagDescription(p.Buildbag), Value = p.Buildbagid.ToString() })
                       .Distinct()
                       .ToListAsync(),
            };

            if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                rtn = rtn.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }

            return rtn;
        }
        #endregion
        public async Task<ResultDto> PlannedActivityMigrations(PlannedActivityMigrationsDto data)
        {
            Plannedactivities plannedActivitiEntity = _repositoryWrapper.PlannedActivity
                .FindByCondition(x => x.Plannedactivityid == data.PlannedActivityId, false, false)
                .Include(x => x.Lcmengineering)
                .FirstOrDefault();
            var lcmEnd = await _repositoryWrapper.Lcmengineering.FindByCondition(x =>
               x.Opcoid == plannedActivitiEntity.Opcoid && x.Designcomponentid == plannedActivitiEntity.Designcomponentid && x.Archived != true, false, false)
                .FirstOrDefaultAsync();

            var lcmStart = await _repositoryWrapper.Lcmengineering.FindByCondition(x =>
                 x.Opcoid == lcmEnd.Opcoid && x.Designcomponentid == data.DesignComponentIdStart && x.Archived != true, false, false).FirstOrDefaultAsync();

            lcmEnd.Elementcount = lcmStart.Elementcount;

            #region #1631 -LCM and PA got Archived when doing the Node migration in Manage Migration Screen
            if (lcmStart.Elementcount)
            {
                await _networkElementNodeCountManager.MigrateNetworkElement(data, PlannedActivityMapper.Get(plannedActivitiEntity));
            }

            if (lcmStart != null && lcmEnd != null)
            {
                if (lcmStart.Elementcount == true)
                {
                    lcmStart.Numberofnodes = lcmStart.CountNetworkElementReleated(false, _repositoryWrapper);
                    lcmStart.Numberofnodesinlab = lcmStart.CountNetworkElementReleated(true, _repositoryWrapper);

                    lcmEnd.Numberofnodes = lcmEnd.CountNetworkElementReleated(false, _repositoryWrapper);
                    lcmEnd.Numberofnodesinlab = lcmEnd.CountNetworkElementReleated(true, _repositoryWrapper);
                }
                else
                {

                    lcmEnd.Numberofnodes += data.NumberOfNodes;
                    lcmStart.Numberofnodes -= data.NumberOfNodes;

                    if (lcmStart.Numberofnodes < 0)
                    {
                        lcmStart.Numberofnodes = 0;
                    }

                    lcmEnd.Numberofnodesinlab += data.NumberOfLabNodes;
                    lcmStart.Numberofnodesinlab -= data.NumberOfLabNodes;

                    if (lcmStart.Numberofnodesinlab < 0)
                    {
                        lcmStart.Numberofnodesinlab = 0;
                    }
                }



            }
            _repositoryWrapper.Lcmengineering.Update(lcmStart);
            _repositoryWrapper.Lcmengineering.Update(lcmEnd);
            await _repositoryWrapper.SaveAsync();



            await _repositoryWrapper.ClearTracker();

            #endregion


            if ((lcmStart.Numberofnodes == 0 && lcmStart.Numberofnodesinlab == 0) || (data.NetworkElementStart?.Count() == 0 && data.NetworkElementEnd?.Count() > 0))
            {

                #region #1631 -LCM and PA got Archived when doing the Node migration in Manage Migration Screen
                var roleOutDeliveryStatusId = _repositoryWrapper.SettingsUpdatePlannedActivity
                                   .FindByCondition(x => x.Plannedactivitytypefor == (short)PlannedActivityTypeForEnum.LcmEngineering &&
                                    x.Plannedactivityresourceid == plannedActivitiEntity.Plannedactivityresourceid).OrderBy(x => x.Order).LastOrDefault()?.Deliverystatusid;
                if (roleOutDeliveryStatusId != null)
                    plannedActivitiEntity.Deliverystatusid = roleOutDeliveryStatusId;

                #endregion
                Activitystatuses completedActivityStatus = _repositoryWrapper.ActivityStatus.FindByCondition(p => p.Activitystatus == ConstantValueFilter.completedActivityStatus).FirstOrDefault();
                if (completedActivityStatus != null) plannedActivitiEntity.Activitystatusid = completedActivityStatus.Activitystatusid;

                plannedActivitiEntity = _plannedActivityManager.SetPlannedActivityValue(plannedActivitiEntity);
                plannedActivitiEntity.Archived = true;
                plannedActivitiEntity.Archived = plannedActivitiEntity.Archived == null ? false : plannedActivitiEntity.Archived;


                Lcmengineering originalLcnm = plannedActivitiEntity.Lcmengineering;
                originalLcnm.Archived = true;

                #region  July 7th : Need to update LCM status  #1631 -LCM and PA got Archived when doing the Node migration in Manage Migration Screen
                var RemovedDeploymentStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
               .FindByCondition(x => x.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Removed).Select(x => x.Id).FirstOrDefault();

                if (RemovedDeploymentStatusId != 0)
                    originalLcnm.Lcmdeploymentstatusid = RemovedDeploymentStatusId;

                #endregion
                _repositoryWrapper.Lcmengineering.Update(originalLcnm);
                _repositoryWrapper.PlannedActivity.Update(plannedActivitiEntity);
                await _repositoryWrapper.SaveAsync();

                _ = await ArchiveDesginAspectCasePARolledout(originalLcnm, plannedActivitiEntity);

                _ = await MoveActivePAs(originalLcnm.Lcmengineeringid, plannedActivitiEntity.Designcomponentid.Value);
            }
            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
            };
        }


        #region // Asset status #412 Decommission Flow Implementation  - Decommission Flow April 17 2024
        ///Ticket #3 Req #3022 modernizesolutionsuccessornetwork Flow
        public string FetchAssetDeploymentStatusFromSettingPA(short? settingPAResourceId, short? settingPADeliveryStatusId)
        {
            string settingPAAssetDeploymentID = Convert.ToString(_repositoryWrapper.SettingsUpdatePlannedActivity
                                          .FindByCondition(x => x.Plannedactivityresourceid ==
                                          settingPAResourceId
                                          && x.Deliverystatusid == settingPADeliveryStatusId
                                          && x.Plannedactivitytypefor ==
                                          (short)PlannedActivityTypeForEnum.LcmEngineering)
                                          .Include(x => x.Settingupdateplannedactivityassetdeploymentstatus)
                                          .Select(x => x.Settingupdateplannedactivityassetdeploymentstatus.Where(x => x.Deleted
                                          == false))
                                          .FirstOrDefault()?.Select
                                          (x => x.Assetdeploymentstatusid.ToString()).FirstOrDefault());

            return settingPAAssetDeploymentID;
        }


        #endregion

        #region LcmAncillaryEntry
        public async Task<ResultDto> GenerateLcmAncillaryEntry(long lcmId, short? deploymentStatusID, bool isArchived)
        {
            Lcmancillarydata lcmancillarydataEntity = new Lcmancillarydata();
            var IsLcmAncillaryExits = _repositoryWrapper.LcmAncillaryData.FindByCondition(x => x.Lcmengineeringid == lcmId).FirstOrDefault();
            var LcmDeployementStatus = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Id == deploymentStatusID).FirstOrDefault();
            try
            {

                if (IsLcmAncillaryExits == null && isArchived == false)
                {
                    if (LcmDeployementStatus.Description.ToLower().Replace(" ", "") == ConstantValueFilter.InService)
                    {
                        lcmancillarydataEntity.Assetoutofscope = "In scope";
                        lcmancillarydataEntity.Engupdatetracker = ConstantValueFilter._engUpdateTracker;
                        lcmancillarydataEntity.Opsupdatetracker = ConstantValueFilter._engUpdateTracker;
                        lcmancillarydataEntity.Lcmengineeringid = lcmId;
                    }
                    else if (LcmDeployementStatus.Description.ToLower().Replace(" ", "") == ConstantValueFilter.Planned)
                    {
                        lcmancillarydataEntity.Assetoutofscope = "Asset planned to be inserted in the network";
                        lcmancillarydataEntity.Engupdatetracker = ConstantValueFilter._engUpdateTracker;
                        lcmancillarydataEntity.Opsupdatetracker = ConstantValueFilter._engUpdateTracker;
                        lcmancillarydataEntity.Lcmengineeringid = lcmId;
                    }
                    _repositoryWrapper.LcmAncillaryData.Create(lcmancillarydataEntity);

                }
                else
                {
                    IsLcmAncillaryExits.Assetoutofscope = "Historical back-up (remediation action completed)";
                    IsLcmAncillaryExits.Engupdatetracker = "Completed";
                    IsLcmAncillaryExits.Opsupdatetracker = "Completed";
                    _repositoryWrapper.LcmAncillaryData.Update(IsLcmAncillaryExits);
                }
                await _repositoryWrapper.SaveAsync();
                return new ResultDto()
                {
                    Data = lcmancillarydataEntity.Lcmancillarydataid,
                    Info = ResultMessages.EntryAddSuccess
                };
            }
            catch (Exception)
            {
                return new ResultDto()
                {
                    Data = IsLcmAncillaryExits.Lcmancillarydataid,
                    Info = ResultMessages.EntryAddExists
                };
            }

        }
        #endregion


        #region Create LCM and LCM PA
        public async Task<ResultDto> CreateLcmForDaAssetMigrate(long paId, short opCoId, long targetDcId, long plannedDcfId, long currentDcfId, long? oldDcId, bool isReleaseDetailUnknown = false)
        {
            try
            {
                var daAssetMigrationEntity = _repositoryWrapper.DaAssetMigrationRepository.FindByCondition(x => x.Plannedactivityid == paId && x.Targetdesigncomponenetid != null
                && x.Newelementname != null).ToList();
                if (daAssetMigrationEntity != null && daAssetMigrationEntity.Count == 0)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.NoDaAssetMigration,
                        Data = null

                    };
                }


                var lcmDeploymentStatus = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower() == ConstantValueFilter.Planned
                || x.Description.ToLower() == ConstantValueFilter.inService).ToList();
                //Unknow Dc create 
                if (isReleaseDetailUnknown)
                {

                    var unknownDcId = await CreateUnkownDesignComponentBasedOnDCF(plannedDcfId);
                    if (unknownDcId.Data != null)
                    {
                        var unKnownDcResult = (IDictionary<long, string>)unknownDcId.Data;
                        var OpcoId = 0;
                        var designComponentID = unKnownDcResult.FirstOrDefault().Key; //  unknownDcId.Data;
                        if (designComponentID != 0)
                        {
                            foreach (var item in daAssetMigrationEntity)
                            {
                                OpcoId = item.Opcoid;
                                item.Targetdesigncomponenetid = designComponentID;
                                _repositoryWrapper.DaAssetMigrationRepository.Update(item);
                            }
                            await _repositoryWrapper.SaveAsync();


                            #region LCM  //Create new Planned LCM
                            LcmEngineeringDtoCreate _lcmEngineeringDtoCreate = new LcmEngineeringDtoCreate();
                            var lcmPlannedStatusId = _repositoryWrapper.LcmDeploymentStatusRepository.FindByCondition(x => x.Description.ToLower()
                            == ConstantValueFilter.Planned).FirstOrDefault().Id;

                            // Get Empty Bag ID
                            var dummyBag = _commonManager.GetDummyBag();

                            if (dummyBag != null)
                            {
                                _lcmEngineeringDtoCreate.BuildBagId = dummyBag.Buildbagid;
                            }
                            //Product important 
                            var productImportantId = _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportance.ToLower() ==
                            ConstantValueFilter.notStrategic).FirstOrDefault().Productimportanceid;

                            // Get Edu andSubdomainc spoc from currentdcf 
                            var currentDcfLCMList = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponent.Designcomponentfamilyid == currentDcfId
                            && x.Opcoid == OpcoId)
                                .Include(x => x.Lcmengineeringeduspoc).Include(x => x.Lcmengineeringsubdomainspoc)
                                .Include(x => x.Lcmoperationalcontracts).ToList();

                            _lcmEngineeringDtoCreate.LCMDeploymentStatusId = lcmPlannedStatusId;
                            _lcmEngineeringDtoCreate.OpCoId = (short)OpcoId;
                            _lcmEngineeringDtoCreate.DesignComponentId = designComponentID;
                            _lcmEngineeringDtoCreate.ElementCount = false;
                            //_lcmEngineeringDtoCreate.ProductImportanceId = productImportantId;
                            _lcmEngineeringDtoCreate.IsReleaseDetailUnKnown = true;
                            _lcmEngineeringDtoCreate.DesignComponentFamilyid = plannedDcfId;

                            var existingProductImportance = currentDcfLCMList.Select(x => x.Productimportanceid).FirstOrDefault();
                            var existingOperationContact = currentDcfLCMList.SelectMany(x => x.Lcmoperationalcontracts.Select(y => y.Operationalcontractid)).ToList();
                            _lcmEngineeringDtoCreate.ProductImportanceId = existingProductImportance != null ? existingProductImportance : productImportantId;
                            if (existingOperationContact.Any())
                            {
                                _lcmEngineeringDtoCreate.OperationContractsIds = existingOperationContact.Distinct().ToList();
                            }


                            IQueryable<Reasoncheckboxresources> checkBox = _repositoryWrapper.ReasonCheckboxResource.FindAll(true);
                            _lcmEngineeringDtoCreate.CheckboxResourceResource = checkBox.ToDictionary(x => (int)x.Id, x => _mapper.Map<ReasonCheckboxDto>(ReasonCheckboxResourceMapper
                                .GetReasonCheckboxResourceMapper(x)));

                            if (currentDcfLCMList != null && currentDcfLCMList.Count > 0)
                            {
                                var eduspocList = currentDcfLCMList.SelectMany(x => x.Lcmengineeringeduspoc.Select(y => y.Eduspocid)).ToList();
                                if (eduspocList.Any())
                                {
                                    _lcmEngineeringDtoCreate.EduSpocIds = eduspocList.Distinct().ToList();
                                }
                                var subDomainList = currentDcfLCMList.SelectMany(x => x.Lcmengineeringsubdomainspoc.Select(y => y.Subdomainspocid)).ToList();
                                if (subDomainList.Any())
                                {
                                    _lcmEngineeringDtoCreate.SubDomainSpocIds = subDomainList.Distinct().ToList();
                                }
                            }
                            #endregion
                            #region Pa Create
                            var getDaPa = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == paId).FirstOrDefault();
                            if (getDaPa != null)
                            {
                                PlannedActivityDtoUpdate _paDto = new PlannedActivityDtoUpdate();

                                // Get deployNewSolutionId
                                var paResources = _repositoryWrapper.PlannedActivityResourceRepository
                                    .FindByCondition(x => x.Rulelinkeddc == (int)PlannedActivityResourceEnum.New_NFxI_Solution).FirstOrDefault();

                                if (paResources != null)
                                    _paDto.PlannedActivityResourceId = paResources.Plannedactivityresourceid;

                                //Dilvery Status Id
                                var settingUpdatePA = _repositoryWrapper.SettingsUpdatePlannedActivity
            .FindByCondition(x => x.Plannedactivityresourceid == paResources.Plannedactivityresourceid && x.Plannedactivitytypefor == 0
            && x.Settingsupdateplnactdes.ToLower() == ConstantValueFilter.TypeAcceptance).FirstOrDefault();

                                _paDto.DeliveryStatusId = settingUpdatePA.Deliverystatusid;

                                _paDto.OpCoId = (short?)OpcoId;
                                _paDto.DesignComponentId = designComponentID;
                                _paDto.BuildBagId = dummyBag.Buildbagid;

                                _paDto.ActivityDetails = paResources.Activitydetailslcm;
                                _paDto.PlannedImplementationYear = getDaPa.Plannedimplementationyear;
                                _paDto.PlannedCompletion = getDaPa.Plannedcompletion;
                                _paDto.DeliveryPlanAvailable = true;
                                _paDto.PlanningActivityStatusId = 1;
                                _paDto.BudgetAvailabilityId = 1;

                                // Responsibility Phase*
                                _paDto.ResponsibilityPhaseId = getDaPa.Responsibilityphaseid;

                                //Operations Risk Evaluation
                                _paDto.RiskOpeId = getDaPa.Operationalriskid;


                                //Engineering Risk Evaluation
                                _paDto.RiskEngId = getDaPa.Engineeringriskid;


                                _paDto.ActivityStatusId = getDaPa.Activitystatusid;

                                _paDto.LocalApproval = "NO";

                                // _paDto.ResponsibilityPhaseId = getDaPa.Responsibilityphaseid;

                                //Planning Risk 
                                var planningRisk = _repositoryWrapper.PlannedActivityResourcePlanningRisk
                                    .FindByCondition(x => x.Plannedactivityresourceid == paResources.Plannedactivityresourceid && x.Forlcm == true).
                                    OrderBy(x => x.Plnactresourceplanningriskid).FirstOrDefault();


                                if (planningRisk != null)
                                {

                                    _paDto.PlanningRiskId = planningRisk.Planningriskid;
                                }

                                var riskOperation = _repositoryWrapper.Risk
                                  .FindAll().
                                  OrderBy(x => x.Riskid).FirstOrDefault();

                                if (riskOperation != null)
                                {
                                    _paDto.RiskOpeId = riskOperation.Riskid;
                                }


                                if (_lcmEngineeringDtoCreate.PlannedActivityDto == null)
                                    _lcmEngineeringDtoCreate.PlannedActivityDto = new List<PlannedActivityDtoUpdate>();

                                _lcmEngineeringDtoCreate.PlannedActivityDto.Add(_paDto);

                            }
                            #endregion
                            var newLcmEntity = await Add(_lcmEngineeringDtoCreate);
                            return new ResultDto
                            {
                                Info = ResultMessages.EntryUpdateSuccess,
                                Data = newLcmEntity

                            };
                        }
                    }

                }
                else
                {

                    var designComponentID = targetDcId;  //  unknownDcId.Data;
                    if (designComponentID != 0)
                    {

                        #region LCM  //Create new Planned LCM
                        LcmEngineeringDtoCreate _lcmEngineeringDtoCreate = new LcmEngineeringDtoCreate();
                        var lcmInserviceStatusId = lcmDeploymentStatus.Where(x => x.Description.ToLower()
                        == ConstantValueFilter.inService).FirstOrDefault().Id;

                        var lcmPlannedStatusId = lcmDeploymentStatus.Where(x => x.Description.ToLower()
                        == ConstantValueFilter.Planned).FirstOrDefault().Id;
                        // Get Empty Bag ID
                        var dummyBag = _commonManager.GetDummyBag();

                        if (dummyBag != null)
                        {
                            _lcmEngineeringDtoCreate.BuildBagId = dummyBag.Buildbagid;
                        }
                        //Product important 
                        var productImportantId = _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportance.ToLower() ==
                        ConstantValueFilter.notStrategic).FirstOrDefault().Productimportanceid;

                        // Get Edu andSubdomainc spoc from currentdcf 
                        var getExisingLcmEntities = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == oldDcId
                        && x.Opcoid == opCoId)
                            .Include(x => x.Lcmengineeringeduspoc).Include(x => x.Lcmengineeringsubdomainspoc).Include(x => x.Lcmoperationalcontracts).ToList();

                        if (getExisingLcmEntities != null && getExisingLcmEntities.Count == 0)
                        {
                            getExisingLcmEntities = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponent.Designcomponentfamilyid == currentDcfId
                        && x.Opcoid == opCoId)
                            .Include(x => x.Lcmengineeringeduspoc).Include(x => x.Lcmengineeringsubdomainspoc).Include(x => x.Lcmoperationalcontracts).ToList();
                        }

                        _lcmEngineeringDtoCreate.LCMDeploymentStatusId = lcmInserviceStatusId;
                        _lcmEngineeringDtoCreate.OpCoId = opCoId;
                        _lcmEngineeringDtoCreate.DesignComponentId = designComponentID;
                        _lcmEngineeringDtoCreate.ElementCount = false;
                        //_lcmEngineeringDtoCreate.ProductImportanceId = productImportantId;
                        _lcmEngineeringDtoCreate.DesignComponentFamilyid = plannedDcfId;

                        var existingProductImportance = getExisingLcmEntities.Select(x => x.Productimportanceid).FirstOrDefault();
                        var existingOperationContact = getExisingLcmEntities.SelectMany(x => x.Lcmoperationalcontracts.Select(y => y.Operationalcontractid)).ToList();
                        _lcmEngineeringDtoCreate.ProductImportanceId = existingProductImportance != null ? existingProductImportance : productImportantId;
                        if (existingOperationContact.Any())
                        {
                            _lcmEngineeringDtoCreate.OperationContractsIds = existingOperationContact.Distinct().ToList();
                        }

                        IQueryable<Reasoncheckboxresources> checkBox = _repositoryWrapper.ReasonCheckboxResource.FindAll(true);
                        _lcmEngineeringDtoCreate.CheckboxResourceResource = checkBox.ToDictionary(x => (int)x.Id, x => _mapper.Map<ReasonCheckboxDto>(ReasonCheckboxResourceMapper
                            .GetReasonCheckboxResourceMapper(x)));

                        if (getExisingLcmEntities != null && getExisingLcmEntities.Count > 0)
                        {
                            var eduspocList = getExisingLcmEntities.SelectMany(x => x.Lcmengineeringeduspoc.Select(y => y.Eduspocid)).ToList();
                            if (eduspocList.Any())
                            {
                                _lcmEngineeringDtoCreate.EduSpocIds = eduspocList.Distinct().ToList();
                            }
                            var subDomainList = getExisingLcmEntities.SelectMany(x => x.Lcmengineeringsubdomainspoc.Select(y => y.Subdomainspocid)).ToList();
                            if (subDomainList.Any())
                            {
                                _lcmEngineeringDtoCreate.SubDomainSpocIds = subDomainList.Distinct().ToList();
                            }
                        }
                        #endregion

                        var newLcmEntity = await Add(_lcmEngineeringDtoCreate);
                        return new ResultDto
                        {
                            Info = ResultMessages.EntryUpdateSuccess,
                            Data = newLcmEntity
                        };
                    }
                }
                return new ResultDto
                {
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = null

                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue occurred while  updating  Unknown Dcid in updateUnknownDcIdToDaAssetMigrate() for Da Asset Migration Platform Migration: {ex.Message}");
                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed,

                };

            }

        }

        #region  // Asset Delete for DaMigration  - We can't call Asset manager in PlaftformMigratonManager 
        public async Task<ResultDto> AssetRecordDelete(long assetId)
        {
            var entity = await _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Networkelementasplannedid == assetId)
              .Include(x => x.Plannedactivities)
              .FirstOrDefaultAsync();
            var isPaDelete = false;
            if (entity != null)
            {
                if (entity.Plannedactivities != null && entity.Plannedactivities.Count > 0)
                    isPaDelete = true;
            }

            await _networkElementsAsPlannedManager.DeleteDeep(assetId, isPaDelete);

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = null

            };
        }
        #endregion
        public async Task<ResultDto> CreateLcmAndLcmPaForPlatformationMigration(long paId, long plannedDcfId, long currenDcfId, List<Daassetmigration> oldDaAssetMigrationEntity)
        {
            if (currenDcfId == 0)
            {
                var daPlatformMigrationPaList = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Plannedactivityid == paId)
                    .Include(x => x.Designaspect).FirstOrDefault();
                if (daPlatformMigrationPaList != null)
                    currenDcfId = daPlatformMigrationPaList.Designaspect.Designcomponentfamilyid;
                else
                {
                    _logger.LogDebug($"{currenDcfId} - {paId} - {plannedDcfId}");
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryAddUpdateFailed,
                        Data = "Current DCF Id is empty"
                    };
                }
            }

            #region AssetName update and Delete - Jan 23
            if (oldDaAssetMigrationEntity != null && oldDaAssetMigrationEntity.Count == 0)
            {
                oldDaAssetMigrationEntity = _repositoryWrapper.DaAssetMigrationRepository.FindByCondition(x => x.Plannedactivityid == paId).ToList();
            }

            #endregion
            try
            {

                //  Get Deployment Statuses for Asset
                var assetDeploymentStatus = _repositoryWrapper.DeploymentStatus
                    .FindByCondition(x =>
                        x.Deploymentstatus.ToLower() == ConstantValueFilter.Planned ||
                        x.Deploymentstatus.ToLower() == ConstantValueFilter.inService ||
                        x.Deploymentstatus.ToLower() == ConstantValueFilter.Removed ||
                        x.Deploymentstatus.ToLower() == ConstantValueFilter.inCommisioning)
                    .ToList();

                var assetPlannedId = assetDeploymentStatus
                    .FirstOrDefault(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.Planned)?
                    .Deploymentstatusid;

                var assetInServiceId = assetDeploymentStatus
                    .FirstOrDefault(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.inService)?
                    .Deploymentstatusid;

                var assetRemovedId = assetDeploymentStatus
                   .FirstOrDefault(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.Removed)?
                   .Deploymentstatusid;

                var assetCommisioningId = assetDeploymentStatus
                   .FirstOrDefault(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.inCommisioning)?
                   .Deploymentstatusid;

                //  Get DA Assets

                var assetMigrationsForPlannedActivity = _repositoryWrapper.DaAssetMigrationRepository
              .FindByCondition(x => x.Plannedactivityid == paId)
              .Include(x => x.Networkelementasplanned)
              .Include(x => x.Targetdesigncomponenet).OrderBy(x => x.Deploymentstatusid)
              .ToList();


                var daAssetMigration = assetMigrationsForPlannedActivity
                    .Where(x => x.Targetdesigncomponenetid != null && x.Newelementname != null).ToList(); 

          
                #region//  Get Deployment Statuses for LCM
                var lcmDeploymentStatus = _repositoryWrapper.LcmDeploymentStatusRepository
                    .FindByCondition(x =>
                        x.Description.ToLower() == ConstantValueFilter.InService ||
                        x.Description.ToLower() == ConstantValueFilter.Planned ||
                        x.Description.ToLower() == ConstantValueFilter.LcmIn_Commissioning)
                    .ToList();

                var lcmPlannedId = lcmDeploymentStatus
                    .FirstOrDefault(x => x.Description.ToLower() == ConstantValueFilter.Planned)?
                    .Id;

                var lcmInServiceId = lcmDeploymentStatus
                    .FirstOrDefault(x => x.Description.ToLower() == ConstantValueFilter.inService)?
                    .Id;

                var lcmInCommisionId = lcmDeploymentStatus
                    .FirstOrDefault(x => x.Description.ToLower() == ConstantValueFilter.LcmIn_Commissioning)?
                    .Id;

                #endregion
                var errorDetails = new List<(string Key, string Value)>();

                //  Group DA Assets by Target Design Component
                var daAssetGroups = daAssetMigration
                    .GroupBy(x => x.Targetdesigncomponenetid)
                    .ToList();

                if (daAssetGroups != null && daAssetGroups.Count > 0)
                {
                    var opCoId = daAssetGroups.FirstOrDefault()?.Select(x => x.Opcoid).FirstOrDefault();

                    var targetDcList = daAssetGroups
                        .SelectMany(x => x.Select(y => y.Targetdesigncomponenetid))
                        .ToList();

                    //  Get existing LCMs for Design Components
                    var targetDcBasedLcmEntities = _repositoryWrapper.Lcmengineering
                        .FindByCondition(x =>
                            x.Opcoid == opCoId &&
                            targetDcList.Contains(x.Designcomponentid) &&
                            x.Archived != true)
                        .Include(x => x.Lcmengineeringeduspoc)
                        .Include(x => x.Lcmengineeringsubdomainspoc)
                        .ToList();
                    
                    //  Process each grouped DA Asset
                    var createdNetworkElements = new List<NetworkElementAsPlannedDtoCreate>();


                    foreach (var groupedDaItem in daAssetGroups)
                    {
                        var firstItem = groupedDaItem.FirstOrDefault();
                        if (firstItem == null) continue;


                        var lcmEntity = targetDcBasedLcmEntities
                            .FirstOrDefault(l => l.Designcomponentid == (long)firstItem.Targetdesigncomponenetid);

                        var networkElementAssociateds = new List<NetworkElementAssociated>();

                        var isExistsAssetAvailable = false;

                        #region LCM is Planned  - But Asset is Inservice - Check and update 


                        #endregion
                        //  Process each Asset in the group
                        foreach (var asset in groupedDaItem)
                        {
                            var opCoid = opCoId;
                            var targetDcId = (long)asset?.Targetdesigncomponenetid;
                            var assetDeploymentId = (short)asset?.Deploymentstatusid;
                            var isNewAsset = true;
                            long? oldDcId = asset.Networkelementasplanned != null ? (long)asset?.Networkelementasplanned?.Designcomponentid : null;
                            var assetRemovedStatus = assetDeploymentStatus.Where(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.Removed)?.FirstOrDefault()?.Deploymentstatusid;


                            if (lcmEntity == null)
                            {

                                var newLcmEntity = await CreateLcmForDaAssetMigrate(paId, (short)opCoId, targetDcId, plannedDcfId, currenDcfId, oldDcId, false);

                                lcmEntity = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == firstItem.Targetdesigncomponenetid && x.Opcoid == opCoId).FirstOrDefault();

                                #region march 11 2026  #1887 Enhancement – DA Platform Migration: Set Newly Created LCM (LcmAncillary – AssetOutOfScope) as “Asset Planned to be Inserted in the Network”
                                if (lcmEntity != null)
                                {
                                    _logger.LogDebug($"LCM Has been Created for the following OpCoId and DcId{opCoId} - {firstItem.Targetdesigncomponenetid} - LcmID {lcmEntity.Lcmengineeringid}");
                                    var lcmAncillary = _repositoryWrapper.LcmAncillaryData.FindByCondition(x => x.Lcmengineeringid == lcmEntity.Lcmengineeringid)?.FirstOrDefault();
                                    if (lcmAncillary != null)
                                    {
                                        lcmAncillary.Assetoutofscope = ConstantValueFilter.assetPlannedTobeInserted;
                                        _repositoryWrapper.LcmAncillaryData.Update(lcmAncillary);
                                        await _repositoryWrapper.SaveAsync();
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                _logger.LogDebug($"LCM Already Exists for the following OpCoId and DcId{opCoId} - {firstItem.Targetdesigncomponenetid} - LcmID {lcmEntity.Lcmengineeringid}");

                                var assetNotPlannedStatus = groupedDaItem.Any(x => (x.Deploymentstatusid == assetInServiceId)
                                || (x.Deploymentstatusid == assetCommisioningId));

                                var lcmIsPlannedSatus = lcmEntity.Lcmdeploymentstatusid == lcmPlannedId;

                                var lcmIsCommisionStatus = lcmEntity.Lcmdeploymentstatusid == lcmInCommisionId;

                                if ((lcmIsPlannedSatus || lcmIsCommisionStatus) && assetNotPlannedStatus && (assetRemovedStatus != asset.Deploymentstatusid))
                                {
                                    var assetLiveDateFromDaAsset = groupedDaItem.Where(a => a.Migrationcompletiondate.HasValue)?.Max(a => a.Migrationcompletiondate) ?? default(DateTime);

                                    var lcmPa = _repositoryWrapper.PlannedActivity.FindByCondition(x => x.Lcmengineeringid == lcmEntity.Lcmengineeringid && x.Archived == false)
                                        .FirstOrDefault();
                                    var getUpdateManageScreenRecord = await _plannedActivityManager.GetUpdatePlannedActivityStatusForExodus(lcmPa.Plannedactivityid,
                                        (short)PlannedActivityTypeForEnum.LcmEngineering, asset.Migrationcompletiondate ?? System.DateTime.Now, true
                                        );

                                    lcmEntity = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Designcomponentid == firstItem.Targetdesigncomponenetid && x.Opcoid == opCoId).FirstOrDefault();
                                    _logger.LogDebug($"LCM Already Exists for the following OpCoId and DcId{opCoId} - {firstItem.Targetdesigncomponenetid} - LcmID {lcmEntity.Lcmengineeringid}");

                                }

                            }

                            //Update already generated Asset 
                            var existsAsset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == opCoid
                            && x.Designcomponentid == targetDcId && x.Elementname.ToLower() == Convert.ToString(asset.Newelementname).ToLower()).FirstOrDefault();

                            if (existsAsset != null)
                            {

                                isExistsAssetAvailable = true;
                                if (asset.Environmentid != null && asset.Environmentid != 0) existsAsset.Environmentid = (short)asset.Environmentid;
                                if (asset.Deploymentstatusid != null && asset.Deploymentstatusid != 0) existsAsset.Deploymentstatusid = (short)asset.Deploymentstatusid;
                                if (asset.Locationid != null && asset.Locationid != 0) existsAsset.Locationid = (short)asset.Locationid;
                                if (asset.Targetdesigncomponenetid != null && asset.Targetdesigncomponenetid != 0) existsAsset.Designcomponentid = (long)asset.Targetdesigncomponenetid;
                                if (asset.Migrationcompletiondate != null) existsAsset.Assetlivestatusdate = asset.Migrationcompletiondate;

                                if (asset.Rfodate != null) existsAsset.Assetrfodate = asset.Rfodate;
                                if (asset.Rfsdate != null) existsAsset.Assetrfsdate = asset.Rfsdate;

                                if (asset.Rfadate != null) existsAsset.Rfadate = asset.Rfadate;
                                if (asset.Hwpoarriveddate != null) existsAsset.Hwpoarriveddate = asset.Hwpoarriveddate;
                                if (asset.Hwporaiseddate != null) existsAsset.Hwporaiseddate = asset.Hwporaiseddate;
                                if (asset.Bomsubmitteddate != null) existsAsset.Bomsubmitteddate = asset.Bomsubmitteddate;

                                if (lcmEntity != null) existsAsset.Lcmengineeringid = lcmEntity.Lcmengineeringid;

                                _repositoryWrapper.NetworkElementAsPlanned.Update(existsAsset);
                                await _repositoryWrapper.SaveAsync();
                                isNewAsset = false;
                                _logger.LogDebug($"Asset Already Exists for the following OpCoId and DcId{opCoId} - {firstItem.Targetdesigncomponenetid} - AssetId {existsAsset.Networkelementasplannedid}");

                            }


                            if (lcmEntity != null && isNewAsset && (assetRemovedId != assetDeploymentId))
                            {
                                networkElementAssociateds.Add(new NetworkElementAssociated
                                {
                                    ElementName = asset.Newelementname,
                                    LocationId = (short)asset.Locationid,
                                    EnviromentId = (short)asset.Environmentid,
                                    AssetsStatusId = (short)(assetDeploymentId == 0 ? assetPlannedId : assetDeploymentId),
                                    AssetLiveStatusDate = asset.Migrationcompletiondate,
                                    IsFinalAsset = false,
                                    AssetRfoDate = asset.Rfodate,
                                    AssetRfsDate = asset.Rfsdate,
                                    BomSubmittedDate = asset.Bomsubmitteddate,
                                    RfaDate= asset.Rfadate,
                                    HwPoArrivedDate=asset.Hwpoarriveddate,
                                    HwPoRaisedDate= asset.Hwporaiseddate
                                    
                                });
                            }
                            else if (lcmEntity == null)
                            {
                                // LCM doesn’t exist – error
                                errorDetails.Add((ResultMessages.EntryLcmIdNotExists, asset.Newelementname));
                                _logger.LogDebug($"LCM Not Exists for the following OpCoId and DcId{opCoId} - {firstItem.Targetdesigncomponenetid} - New AssetName {asset.Newelementname}");

                                // errorDetails.Add(ResultMessages.EntryLcmIdNotExists , asset.Newelementname);
                            }
                            else if ((assetRemovedId == assetDeploymentId))
                            {
                                errorDetails.Add((ResultMessages.DaAssetIsRemovedStatus, asset.Newelementname));
                                _logger.LogDebug($"Asset is in the removed status for the following OpCoId and DcId{opCoId} - {firstItem.Targetdesigncomponenetid} -  New AssetName {asset.Newelementname}");

                            }
                        }

                        if (isExistsAssetAvailable && lcmEntity != null)
                        {
                            lcmEntity.Elementcount = true;
                            int prodNodesCount = lcmEntity.CountNetworkElementReleated(false, _repositoryWrapper);
                            if (prodNodesCount > 0)
                            {
                                _ = await CreateDesignAspectWithOPCOAndDCF(lcmEntity.Opcoid,
                                    _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == lcmEntity.Designcomponentid)
                               .Select(x => x.Designcomponentfamilyid).FirstOrDefault());
                            }
                            lcmEntity.Numberofnodes = prodNodesCount;
                            lcmEntity.Numberofnodesinlab = lcmEntity.CountNetworkElementReleated(true, _repositoryWrapper);

                            _repositoryWrapper.Lcmengineering.Update(lcmEntity);
                            await _repositoryWrapper.SaveAsync();
                        }

                        if (networkElementAssociateds.Any() && networkElementAssociateds.Count > 0 && lcmEntity != null)
                        {
                            //  Create LCM Engineering DTO
                            var createdLcm = new LcmEngineeringDtoCreate
                            {
                                DesignComponentFamilyid = (long)firstItem.Targetdesigncomponenet.Designcomponentfamilyid,
                                DesignComponentId = (long)firstItem.Targetdesigncomponenetid,
                                OpCoId = (short)opCoId,
                                EduSpocIds = lcmEntity?.Lcmengineeringeduspoc.Select(x => x.Eduspocid).ToList(),
                                SubDomainSpocIds = lcmEntity?.Lcmengineeringsubdomainspoc.Select(x => x.Subdomainspocid).ToList(),
                                NetworkElementAssociateds = networkElementAssociateds
                            };

                            //  Save to DB
                            var createdElements = await GenerateNetworkElementAssociated(createdLcm, false);
                            List<Networkelementsasplanned> assets = _repositoryWrapper.NetworkElementAsPlanned
                        .FindByCondition(x => x.Opcoid == lcmEntity.Opcoid && x.Designcomponentid == lcmEntity.Designcomponentid)
                        .ToList();

                            foreach (Networkelementsasplanned asset in assets)
                            {
                                asset.Buildbagid = lcmEntity.Buildbagid;
                                if ((asset.Swresourcekey == null) || (asset.Hwresourcekey == null))
                                {
                                    //(LCM R8 Part 3 requirements)
                                    string swResourceKey = string.Empty;
                                    string hwResourceKey = string.Empty;

                                    //Passing the new LCM Entity design component will validate against the existing entry
                                    //and there will not be an entry, if the DCF changes so a new resourcekey will be assigned.
                                    IDictionary<int, string> AssetKeys = await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforAssets(asset.Designcomponentid, asset.Opcoid, asset.Elementname, asset.Buildbagid);
                                    if (AssetKeys != null)
                                    {
                                        swResourceKey = AssetKeys[(int)ResourceTypesKey.SWAsset];
                                        hwResourceKey = AssetKeys[(int)ResourceTypesKey.HWAsset];
                                        asset.Swresourcekey = swResourceKey;
                                        asset.Hwresourcekey = hwResourceKey;
                                    }
                                    //End of (LCM R8 Part 3 requirements)
                                }
                                asset.Lcmengineeringid = lcmEntity.Lcmengineeringid;

                                _repositoryWrapper.NetworkElementAsPlanned.Update(asset);
                            }
                            await _repositoryWrapper.SaveAsync();
                            lcmEntity.Elementcount = true;
                            int prodNodesCount = lcmEntity.CountNetworkElementReleated(false, _repositoryWrapper);
                            if (prodNodesCount > 0)
                            {
                                _ = await CreateDesignAspectWithOPCOAndDCF(lcmEntity.Opcoid,
                                    _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == lcmEntity.Designcomponentid)
                               .Select(x => x.Designcomponentfamilyid).FirstOrDefault());
                            }
                            lcmEntity.Numberofnodes = prodNodesCount;
                            lcmEntity.Numberofnodesinlab = lcmEntity.CountNetworkElementReleated(true, _repositoryWrapper);
                            _repositoryWrapper.Lcmengineering.Update(lcmEntity);
                            await _repositoryWrapper.SaveAsync();
                        }
                    }

                    // Optionally process `createdNetworkElements` here


                    
                    }
                #region update date column to asset table
                if (assetMigrationsForPlannedActivity != null && assetMigrationsForPlannedActivity.Count > 0)
                {
                   
                    var groupByOldAsset = assetMigrationsForPlannedActivity
                    .GroupBy(x => x.Networkelementasplannedid)
                    .ToList();
                    var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);
                    var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);
                    foreach (var item in groupByOldAsset)
                    {
                        predicateInner.Or(x => x.Networkelementasplannedid == item.Key);

                    }
                    predicateResult.And(predicateInner);
                    var assetEntity = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).ToList();
                    foreach (var item in groupByOldAsset)
                    {
                        var existingAsset = assetEntity.Where(x => x.Networkelementasplannedid == item.Key).FirstOrDefault();
                        if (existingAsset != null)
                        {
                            var isNewElementCreated = false;
                            var newElementAssigned = item.Where(x => x.Newelementname != null && x.Targetdesigncomponenetid != null).FirstOrDefault()  ;

                            #region As per July 3rd 2026 discussion  - Based on this point hide this logic remove add new asset and get only HW OEM - Broadcom with Pltform Vmware/Other NFVI NFCI , CASS
                            //    if(newElementAssigned != null)
                            //    {
                            //        isNewElementCreated = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == newElementAssigned.Opcoid
                            //&& x.Designcomponentid == newElementAssigned.Targetdesigncomponenetid && x.Elementname.ToLower() == Convert.ToString(newElementAssigned.Newelementname).ToLower())
                            //            .FirstOrDefault() != null ? true : false;

                            //    }
                            #endregion

                            var assetRfoDate = (!isNewElementCreated)? item.OrderByDescending(o => o.Rfodate).FirstOrDefault()?.Rfodate : null;
                            var assetRfsDate = (!isNewElementCreated) ?  item.OrderByDescending(o => o.Rfsdate).FirstOrDefault()?.Rfsdate : null;
                            var hwPoRaisedDate = (!isNewElementCreated) ?  item.OrderByDescending(o => o.Hwporaiseddate).FirstOrDefault()?.Hwporaiseddate : null;
                            var hwPoArrivedDate = (!isNewElementCreated) ?  item.OrderByDescending(o => o.Hwpoarriveddate).FirstOrDefault()?.Hwporaiseddate : null;
                            var bomSubmittedDate = (!isNewElementCreated) ?  item.OrderByDescending(o => o.Bomsubmitteddate).FirstOrDefault()?.Bomsubmitteddate : null;
                            var rfaDate = (!isNewElementCreated) ? item.OrderByDescending(o => o.Rfadate).FirstOrDefault()?.Rfadate : null;

                            existingAsset.Assetrfodate = assetRfoDate;
                            existingAsset.Assetrfsdate = assetRfsDate;
                            existingAsset.Hwporaiseddate = hwPoRaisedDate;
                            existingAsset.Hwpoarriveddate = hwPoArrivedDate;
                            existingAsset.Bomsubmitteddate = bomSubmittedDate;
                            existingAsset.Rfadate = rfaDate;

                            _repositoryWrapper.NetworkElementAsPlanned.Update(existingAsset);
                            _repositoryWrapper.Save();

                        }

                    }

                  
                }
                #endregion
                // await transaction.CommitAsync();
                return new ResultDto
                {
                    Warning = errorDetails.Any() ? true : false,
                    Info = errorDetails.Any() ? ResultMessages.EntryAddUpdateFailed : ResultMessages.EntryAddSuccess,
                    Data = errorDetails.Any() ? errorDetails.GroupBy(x => x.Key) : errorDetails
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue occurred while adding/updating LCM and LCM PA entity for Da Asset Migration Platform Migration: {ex.Message}");
                //  await transaction.RollbackAsync();

                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed
                };
            }


        }

        #region DeletePltformMigrationPaNewAsset   -- this method used in LCMEnigneering , PA , PlatformMigration Manager  - depencies injection issue  - 
        public async Task<ResultDto> DeletePltformMigrationPaNewAsset(long paId)
        {
            try
            {


                // 2. Get existing migration records for this Planned Activity
                var deleteNewMigratedAssets = _repositoryWrapper.DaAssetMigrationRepository
                    .FindByCondition(x => x.Plannedactivityid == paId && x.Targetdesigncomponenetid != null && x.Newelementname != null)
                    .ToList();

                #region Update Asset while edit NewElement

                foreach (var deleteItem in deleteNewMigratedAssets)
                {
                    var existsAsset = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(x => x.Opcoid == deleteItem.Opcoid
                              && x.Designcomponentid == deleteItem.Targetdesigncomponenetid && x.Elementname.ToLower()
                              == Convert.ToString(deleteItem.Newelementname).ToLower()).FirstOrDefault();
                    if (existsAsset != null)
                    {
                        await AssetRecordDelete(existsAsset.Networkelementasplannedid);

                    }

                }


                #endregion

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteSuccess,
                    Data = 1// This value is used for Archive method in DAPAManager
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Issue occurred while delete NewAsset in Asset Table for DaAssetMigration entity: {ex.Message}");

                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotDeleted,
                    Data = 0// This value is used for Archive method in DAPAManager
                };
            }
        }
        #endregion



        #endregion


        #region Create LCM for DA - Infra Ready PA
        public async Task<ResultDto> CreateLcmForInfraReadyPA(long paId)
        {
            var errorDetails = new List<(string Key, string Value)>();

            try
            {
                // 1. Get Planned Activity
                var paEntity = _repositoryWrapper.PlannedActivity
                    .FindByCondition(x =>
                        x.Plannedactivityid == paId &&
                        x.Damigrationstatus.Any(y => y.Statusid == 3))
                    .Include(x => x.Designaspect)
                    .FirstOrDefault();

                if (paEntity == null)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryNotFound
                    };
                }

                var opCoId = paEntity.Opcoid;
                var dcfId = paEntity.Designcomponentfamilyid;
                var currentDcfId = paEntity.Designaspect.Designcomponentfamilyid;

                // 2. Get Design Components for PA
                var dcIds = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentfamilyid == dcfId)
                    .Select(x => x.Designcomponentid)
                    .ToList();

                if (!dcIds.Any())
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryNotFound
                    };
                }

                // 3. Existing LCMs for those DCs
                var existingLcMs = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x =>
                        x.Opcoid == opCoId &&
                        dcIds.Contains(x.Designcomponentid) &&
                        x.Archived != true)
                    .ToList();

                // 4. Reference data  
                var inServiceStatusId = _repositoryWrapper.LcmDeploymentStatusRepository
                    .FindByCondition(x => x.Description.ToLower() == ConstantValueFilter.InService)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                var productImportanceId = _repositoryWrapper.ProductImportance
                    .FindByCondition(x => x.Productimportance.ToLower() == ConstantValueFilter.notStrategic)
                    .Select(x => x.Productimportanceid)
                    .FirstOrDefault();

                var dummyBagId = _commonManager.GetDummyBag()?.Buildbagid;

                var checkBoxDict = _repositoryWrapper.ReasonCheckboxResource
                    .FindAll(true)
                    .ToList()
                    .ToDictionary(
                        x => (int)x.Id,
                        x => _mapper.Map<ReasonCheckboxDto>(
                            ReasonCheckboxResourceMapper.GetReasonCheckboxResourceMapper(x))
                    );

                if (inServiceStatusId == 0 || productImportanceId == 0 || dummyBagId == 0 || checkBoxDict == null)
                {
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryNotAdd
                    };
                }

                // 5. Get EDU & Subdomain SPOCs from current DCF
                var spocSource = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x =>
                        x.Opcoid == opCoId &&
                        x.Designcomponent.Designcomponentfamilyid == currentDcfId &&
                        x.Archived != true)
                    .Include(x => x.Lcmengineeringeduspoc)
                    .Include(x => x.Lcmengineeringsubdomainspoc)
                    .FirstOrDefault();

                var eduSpocIds = spocSource?.Lcmengineeringeduspoc?
                    .Select(x => x.Eduspocid)
                    .Distinct()
                    .ToList();

                var subDomainSpocIds = spocSource?.Lcmengineeringsubdomainspoc?
                    .Select(x => x.Subdomainspocid)
                    .Distinct()
                    .ToList();

                // 6. Create missing LCMs
                foreach (var dcId in dcIds)
                {
                    if (existingLcMs.Any(x => x.Designcomponentid == dcId))
                    {
                        errorDetails.Add((dcId.ToString(), ResultMessages.EntryLcmIdExists));
                        continue;
                    }

                    var lcmCreateDto = new LcmEngineeringDtoCreate
                    {
                        BuildBagId = (long)dummyBagId,
                        LCMDeploymentStatusId = inServiceStatusId,
                        OpCoId = (short)opCoId,
                        DesignComponentId = dcId,
                        ElementCount = false,
                        ProductImportanceId = productImportanceId,
                        DesignComponentFamilyid = (long)dcfId,
                        CheckboxResourceResource = checkBoxDict,
                        EduSpocIds = eduSpocIds,
                        SubDomainSpocIds = subDomainSpocIds
                    };

                    var result = await Add(lcmCreateDto);

                    errorDetails.Add((
                        dcId.ToString(),
                        result.Info == ResultMessages.EntryAddSuccess
                            ? ResultMessages.EntryAddSuccess
                            : result.Info
                    ));
                }

                return new ResultDto
                {
                    Warning = errorDetails.Any(x => x.Value != ResultMessages.EntryAddSuccess),
                    Info = ResultMessages.EntryUpdateSuccess,
                    Data = errorDetails
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Issue occurred while creating LCM entity for DA Infra Ready. PAId: {paId}");

                return new ResultDto
                {
                    Info = ResultMessages.EntryAddUpdateFailed
                };
            }
        }

        #endregion

        public async Task<ResultDto>  DeleteDeepAssetRecordForPaRefactor(long id)
        {
            return await _networkElementsAsPlannedManager.DeleteDeep(id, false);
        }
    }
}

