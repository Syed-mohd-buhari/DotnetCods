using AutoMapper;
using CAM.BusinessManager.Business.PlannedActivity;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.Entita.MajorSoftwareBuild;
using CAM.DataTransferObjects.Entita.SoftwareBuildCompatibility;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Mappers.Lookup;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.Repository;
using CAM.Repository.Helpers;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class MajorSoftwareBuildManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        private PlannedActivityCommon _plannedActivityCommon;
        private SystemTypeManager _systemTypeManager;
        private LcmEngineeringManager _lcmEngineeringManager;
        private readonly DesignComponentManager _designComponentManager;
        private readonly IProductNameManager _productNameManager;
        protected readonly ILoggerManager _logger;
        private readonly NetworkFunctionManager _networkFunctionManager;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        AuthorizedRoleManager _authorizedRoleManager;
        //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
        private string constSubnetworkAllSupportedServie = ConstantValueFilter.subnetworkAllSupportedServie;
        private readonly MajorSoftwareDapperQueryManager _majorSoftwareDapperQueryManager;
        private readonly CommonDapperRepository _commonDapperRepository;
        private readonly SoftwareBuildCompatibilityManager _softwareBuildCompatibilityManager;
        private readonly string dapperDatabaseMode = "normal";
        public MajorSoftwareBuildManager(IEnumerable<IRepositoryWrapper> wrappers,
            DesignComponentManager designComponentManager,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            PlannedActivityCommon plannedActivityCommon,
            SystemTypeManager systemTypeManager,
            LcmEngineeringManager lcmEngineeringManager,
            IProductNameManager productNameManager,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            NetworkFunctionManager networkFunctionManager
             , SoftwareBuildCompatibilityManager softwareBuildCompatibilityManager,
           CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager, AuthorizedRoleManager authorizedRoleManager
            , MajorSoftwareDapperQueryManager majorSoftwareDapperQueryManager
            , CommonDapperRepository commonDapperRepository) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _plannedActivityCommon = plannedActivityCommon;
            _systemTypeManager = systemTypeManager;
            _lcmEngineeringManager = lcmEngineeringManager;
            _designComponentManager = designComponentManager;
            _productNameManager = productNameManager;
            _logger = logger;
            _networkFunctionManager = networkFunctionManager;
            _softwareBuildCompatibilityManager = softwareBuildCompatibilityManager;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _authorizedRoleManager = authorizedRoleManager;
            _majorSoftwareDapperQueryManager = majorSoftwareDapperQueryManager;
            _commonDapperRepository = commonDapperRepository;
            dapperDatabaseMode = GlobalDbMode.DbMode.ContainsKey(CurrentLogLevelConfig._UserName) ? GlobalDbMode.DbMode[CurrentLogLevelConfig._UserName] : "normal";
        }
        public async Task<ResultDto> Add(MajorSoftwareBuildDtoCreate dto, bool? forced = false)
        {
            var entityExists = await EntityExists(dto);

            if (entityExists != null)
            {
                try
                {
                    var relations = _repositoryWrapper.SystemType.FindByCondition(x => x.Majorsoftwarebuildsid == entityExists.MajorSoftwareBuildsId, true).FirstOrDefault();
                    if (relations == null && entityExists.Deleted == true)
                    {
                        if (forced == true)
                        {
                            return (dto.ExistSystemTypeId != null && dto.ExistSystemTypeId != 0) ? await AddClonedMajorSW(dto) : await AddBase(dto);
                        }
                        else
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExists,
                                Data = new { id = entityExists.MajorSoftwareBuildsId, orphanDeleted = true }
                            };
                        }
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = entityExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entityExists.MajorSoftwareBuildsId
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ex.Message
                    };
                }


            }
            return (dto.ExistSystemTypeId != null && dto.ExistSystemTypeId != 0) ? await AddClonedMajorSW(dto) : await AddBase(dto);
        }
        public async Task<MajorSoftwareBuild> EntityExists(MajorSoftwareBuildDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId
                         && x.Productnameid == dto.ProductNameId
                         && x.Softwareversion.ToLower().Replace(" ", "").Replace(".0", "") == dto.SoftwareVersion.ToLower().Replace(" ", "").Replace(".0", ""))
                .Include(x => x.Systemtypes).OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();
            return MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(entityExists);
        }
        public async Task<MajorSoftwareBuild> EntityExists(MajorSoftwareBuildDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId
                         && x.Productnameid == dto.ProductNameId
                         && x.Softwareversion.ToLower().Replace(" ", "").Replace(".0", "") == dto.SoftwareVersion.ToLower().Replace(" ", "").Replace(".0", "")
                         && x.Majorsoftwarebuildsid != dto.MajorSoftwareBuildsId
                    )
                .Include(x => x.Systemtypes).OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();
            return MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(entityExists);
        }
        public async Task<ResultDto> AddBase(MajorSoftwareBuildDtoCreate dto)
        {
            Majorsoftwarebuilds result;
            try
            {
                var entityForced = _mapper.Map<MajorSoftwareBuild>(dto);


                entityForced.DeliveryMethod = string.IsNullOrEmpty(dto.DeliveryMethod) ? "Traditional" : dto.DeliveryMethod;

                entityForced.OperatingSystemId = entityForced.OperatingSystemId == 0 ? null : entityForced.OperatingSystemId;

                var productName = _repositoryWrapper.ProductNameRepository
                    .FindByCondition(x => x.Productnameid == dto.ProductNameId).FirstOrDefault();


                dto.NetworkFunctionsIds = dto.NetworkFunctionsIds.Distinct().ToList();

                result = MajorSoftwareBuildMapper.SetMajorSoftwareBuildMapper(entityForced);

                _repositoryWrapper.MajorSoftwareBuild.Create(result);
                await _repositoryWrapper.SaveAsync();

                await AddOrUpdateDesignContact(dto.DesignContactIds, result.Majorsoftwarebuildsid);

                foreach (var item in dto.NetworkFunctionsIds)
                {
                    _repositoryWrapper.MajorSoftwareBuildNetworkFunction.Create(new Majorsoftwarebuildnetworkfunction() { Majorsoftwarebuildid = result.Majorsoftwarebuildsid, Networkfunctionid = item });
                }

                await _repositoryWrapper.SaveAsync();


                #region Ticket 743 NFCI Bundle

                SoftwareBuildCompatibilityListDtoCreateUpdate swCompatibilityObj = new SoftwareBuildCompatibilityListDtoCreateUpdate()
                {
                    TCPSoftwareCompatibilityId = (dto.TCPSoftwareCompatibilityIdList != null && dto.TCPSoftwareCompatibilityIdList.Count > 0) ?
                    dto.TCPSoftwareCompatibilityIdList : null,
                    TCISoftwareCompatibilityId = (dto.TCISoftwareCompatibilityIdList != null && dto.TCISoftwareCompatibilityIdList.Count > 0) ?
                    dto.TCISoftwareCompatibilityIdList : null,
                    MajorSoftwareBuildId = result.Majorsoftwarebuildsid
                };

                await _softwareBuildCompatibilityManager.Add(swCompatibilityObj);


                #endregion
            }
            catch (Exception ex)
            {
                return new ResultDto { Info = ex.Message };
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = result.Majorsoftwarebuildsid };
        }
        public async Task<ResultDto> Update(MajorSoftwareBuildDtoUpdate dto, bool? forced)
        {
            var anotherEntityWithSameNaturalKeyExists = await EntityExists(dto);

            var originalEntityWithSameNaturalKey = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(
                x => x.Majorsoftwarebuildsid == dto.MajorSoftwareBuildsId
                    && x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId

                    && x.Productnameid == dto.ProductNameId
                    && x.Softwareversion == dto.SoftwareVersion, true).FirstOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
            {
                var relations = _repositoryWrapper.SystemType
                    .FindByCondition(x => x.Majorsoftwarebuildsid == anotherEntityWithSameNaturalKeyExists.MajorSoftwareBuildsId, true)
                    .FirstOrDefault();
                if (relations == null && anotherEntityWithSameNaturalKeyExists.Deleted == true)
                {
                    if (forced == true)
                    {
                        return await UpdateBase(dto, forced);
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryUpdateExists,
                            Data = new { id = anotherEntityWithSameNaturalKeyExists.MajorSoftwareBuildsId, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = anotherEntityWithSameNaturalKeyExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                        Data = anotherEntityWithSameNaturalKeyExists.MajorSoftwareBuildsId
                    };
                }
            }
            return await UpdateBase(dto, forced);
        }
        public async Task<ResultDto> UpdateBase(MajorSoftwareBuildDtoUpdate dto, bool? forced)
        {
            var entity = await UpdateMajorSoftwareBuildEntity(dto, forced);

            var systemTypes = _repositoryWrapper.SystemType.FindByCondition(x => x.Majorsoftwarebuildsid == entity.MajorSoftwareBuildsId)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                 .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Criticalassettype)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Designcomponents).ThenInclude(x => x.Designcomponentfamily)

                .ThenInclude(x => x.Subnetworkboundary).AsNoTracking().ToList();
            var networkfunctionRelations = _repositoryWrapper.MajorSoftwareBuildNetworkFunction.FindByCondition(x => x.Majorsoftwarebuildid == dto.MajorSoftwareBuildsId).ToList();

            foreach (var toDelete in networkfunctionRelations)
                _repositoryWrapper.MajorSoftwareBuildNetworkFunction.DeleteDeep(toDelete);

            if (dto.NetworkFunctionsIds != null && dto.NetworkFunctionsIds.Count() > 0)
            {
                dto.NetworkFunctionsIds = dto.NetworkFunctionsIds?.Distinct().ToList();

                foreach (var item in dto.NetworkFunctionsIds)
                {
                    _repositoryWrapper.MajorSoftwareBuildNetworkFunction.Create(new Majorsoftwarebuildnetworkfunction() { Majorsoftwarebuildid = entity.MajorSoftwareBuildsId, Networkfunctionid = item });
                }
            }

            foreach (var systemType in systemTypes)
            {
                var updated = _systemTypeManager.SetSystemTypeValue(systemType);
                _repositoryWrapper.SystemType.Update(updated);
            }

            await _plannedActivityCommon.ReloadPlannedActivity(systemTypes.Select(x => x.Systemtypeid).ToList(), true);
            await _repositoryWrapper.ClearTracker();

            foreach (var systemType in systemTypes)
            {
                //Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
                var activeDcRecords = systemType.Designcomponents.Where(x => x.Deleted == false).ToList();
                foreach (var t in activeDcRecords)
                {
                    await _repositoryWrapper.ClearTracker();

                    await _designComponentManager.Update(new DesignComponentDtoUpdate()
                    {
                        PlatformId = t.Designcomponentfamily.Platformid,
                        SubNetworkBoundaryIds = new List<long>() { t.Designcomponentfamily.Subnetworkboundaryid },
                        DesignComponentId = t.Designcomponentid,
                        GdprRelevant = t.Designcomponentfamily.Subnetworkboundary.Gdprrelevant,
                        SystemTypeId = t.Systemtypeid,
                        DesignComponentFamilyId = t.Designcomponentfamilyid,
                        VisibleFlag = (t.Visibleflag == null) ? true : t.Visibleflag,

                    });
                }
            }
            await _repositoryWrapper.SaveAsync();

            #region Ticket 743 NFCI Bundle  
            #region - Ticket 880 - Dev - Need to make TCI/TCP columns as non-mandatory fields

            var bundleRecord = _repositoryWrapper.SoftwareBuildCompatibility.FindByCondition(x => x.Majorsoftwarebuildid
                == dto.MajorSoftwareBuildsId).ToList();

            if (dto.TCPSoftwareCompatibilityIdList == null)
            {
                var tcpBundleRecord = bundleRecord.Where(x => x.Bundletype == (int)SoftwareCompatibilityEnum.TCPBundle).ToList();
                foreach (var tcpItem in tcpBundleRecord)
                {
                    _repositoryWrapper.SoftwareBuildCompatibility.DeleteDeep(tcpItem);
                }
                await _repositoryWrapper.SaveAsync();

            }
            if (dto.TCISoftwareCompatibilityIdList == null)
            {
                var tciBundleRecord = bundleRecord.Where(x => x.Bundletype == (int)SoftwareCompatibilityEnum.TCIBundle).ToList();
                foreach (var tciItem in tciBundleRecord)
                {
                    _repositoryWrapper.SoftwareBuildCompatibility.DeleteDeep(tciItem);
                }
                await _repositoryWrapper.SaveAsync();

            }
            await _repositoryWrapper.ClearTracker();
            #endregion
            SoftwareBuildCompatibilityListDtoCreateUpdate swCompatibilityObj = new SoftwareBuildCompatibilityListDtoCreateUpdate()
            {
                TCPSoftwareCompatibilityId = (dto.TCPSoftwareCompatibilityIdList != null && dto.TCPSoftwareCompatibilityIdList.Count > 0) ?
                dto.TCPSoftwareCompatibilityIdList : null,
                TCISoftwareCompatibilityId = (dto.TCISoftwareCompatibilityIdList != null && dto.TCISoftwareCompatibilityIdList.Count > 0) ?
                dto.TCISoftwareCompatibilityIdList : null,
                MajorSoftwareBuildId = entity.MajorSoftwareBuildsId
            };

            await _softwareBuildCompatibilityManager.Add(swCompatibilityObj);
            #endregion

            await AddOrUpdateDesignContact(dto.DesignContactIds, entity.MajorSoftwareBuildsId);


            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.MajorSoftwareBuildsId
            };
        }
        public async Task<MajorSoftwareBuild> UpdateMajorSoftwareBuildEntity(MajorSoftwareBuildDtoUpdate dto, bool? forced = false)
        {
            var entity = _mapper.Map<MajorSoftwareBuild>(dto);
            if (forced == true)
            {
                entity.Deleted = false;
                entity.DeletionDate = null;
            }

            dto.DeliveryMethod = string.IsNullOrEmpty(dto.DeliveryMethod) ? "Traditional" : dto.DeliveryMethod;
            entity.OperatingSystemId = entity.OperatingSystemId == 0 ? null : entity.OperatingSystemId;
            entity.Isvmware = dto.IsPlatform;
            _repositoryWrapper.MajorSoftwareBuild.Update(MajorSoftwareBuildMapper.SetMajorSoftwareBuildMapper(entity));

            await _repositoryWrapper.SaveAsync();
            return entity;
        }
        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.MajorSoftwareBuild
                .FindByCondition(x => x.Majorsoftwarebuildsid == id).FirstOrDefaultAsync();

            _repositoryWrapper.MajorSoftwareBuild.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Majorsoftwarebuildsid
            };
        }
        public async Task<ResultDto> DeleteDeep(long id)
        {
            var entity = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == id)
                .Include(x => x.Orgeqpmanufacturer).FirstOrDefaultAsync();

            var relatedFunctionalEntity = _repositoryWrapper.MajorSoftwareBuildNetworkFunction.FindByCondition(x => x.Majorsoftwarebuildid == id).ToList();

            foreach (var toDelete in relatedFunctionalEntity)
                _repositoryWrapper.MajorSoftwareBuildNetworkFunction.Delete(toDelete);

            #region Ticket 743 NFCI Bundle
            var softwareBuildCompatibilityEntity =
            (entity.Isvmware == true) ?
                  _repositoryWrapper.SoftwareBuildCompatibility
                    .FindByCondition(x => x.Bundlemajorsoftwarebuildid == id).ToList() : _repositoryWrapper.SoftwareBuildCompatibility.
                    FindByCondition(x => x.Majorsoftwarebuildid == id).ToList();


            foreach (var toDelete in softwareBuildCompatibilityEntity)
                _repositoryWrapper.SoftwareBuildCompatibility.DeleteDeep(toDelete);
            #endregion

            #region  
            var softwareDesignContact = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorsoftwarebuildsid == id).ToList();

            foreach (var toDelete in softwareDesignContact)
                _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.DeleteDeep(toDelete);

            await _repositoryWrapper.SaveAsync();
            #endregion

            _repositoryWrapper.MajorSoftwareBuild.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Majorsoftwarebuildsid
            };
        }
        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            var entities = _repositoryWrapper.SystemType
                .FindByCondition(x => x.Majorsoftwarebuildsid == id)
                .Select(x => x.toSystemTypeName(_repositoryWrapper)).ToArray();


            if (entities.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "System Type", Values = entities });


            var entity = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == id)
               .Include(x => x.Orgeqpmanufacturer)
               .Include(x => x.Productname)
               .Include(x => x.Criticalassettype).FirstOrDefaultAsync();

            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            List<string> removeDuplicatesAndLinkedTableCheck = new List<string>()
            {
                 "Systemtypes","Softwarebuildcompatibility","Majorswbuildsdesigncontacts"
            };
            var referenceTableRecord = _commonManager.GetForeignKeyRefernceTable("Majorsoftwarebuilds", id, removeDuplicatesAndLinkedTableCheck);
            if (referenceTableRecord.Result != null && referenceTableRecord.Result.Count() > 0)
                rm.Add(new ResultMessageDto() { Table = _commonManager.popupTabName, Values = referenceTableRecord.Result.ToArray() });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Major Software Build",
                        RecordName = entity.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + entity.Productname.Description + " - " + entity.Softwareversion,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }
        public async Task<ResultDto> GetRelatedRecords_New(long id)
        {
            Dictionary<string, List<long>> refTableDuplicateRecordCheckDic = new Dictionary<string, List<long>>();
            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            var entities = _repositoryWrapper.SystemType
                .FindByCondition(x => x.Majorsoftwarebuildsid == id)
                .ToDictionary(x => x.Systemtypeid,
                    x => x.toSystemTypeName(_repositoryWrapper)
                 );

            if (entities.Count > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "System Type", Values = entities.Select(x => x.Value).ToArray() });
                refTableDuplicateRecordCheckDic["Systemtypes"] = entities.Select(x => x.Key).ToList();
            }
            var entity = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == id)
               .Include(x => x.Orgeqpmanufacturer)
               .Include(x => x.Productname)
               .Include(x => x.Criticalassettype).FirstOrDefaultAsync();

            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            List<string> removeDuplicatesAndLinkedTableCheck = new List<string>()
            {
                 "Systemtypes","Softwarebuildcompatibility"
            };
            var referenceTableRecord = _commonManager.GetReferencedForeignKeyTablesAsync("Majorsoftwarebuilds", id, removeDuplicatesAndLinkedTableCheck, refTableDuplicateRecordCheckDic);
            if (referenceTableRecord.Result != null && referenceTableRecord.Result.Count() > 0)
                rm.Add(new ResultMessageDto() { Table = _commonManager.popupTabName, Values = referenceTableRecord.Result.ToArray() });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Major Software Build",
                        RecordName = entity.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + entity.Productname.Description + " - " + entity.Softwareversion,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }
        public QueryResultDto<MajorSoftwareBuildDtoGrid> FindWithCondition(MajorSoftwareBuildQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Majorsoftwarebuilds>();// ApplyFilter(buildFilterDto);       

            predicateResult = ApplyFilter(buildFilterDto) ;
            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }

            if (buildFilterDto.Orphan == true)
            {
                predicateResult.And(x => !x.Systemtypes.Any());
            }
            // Ticket 831 - Remove Unknown Softwareversion in MajorSoftwareBuild Screen - Aug 1st 
            predicateResult.And(x => x.Softwareversion.ToLower() != ConstantValueFilter.Unknown);


            var rtn = new QueryResultDto<MajorSoftwareBuildDtoGrid>(new GenerateRenderForGrid<MajorSoftwareBuildDtoGrid>(_customColumnManager))
            {
            };

            #region Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File
            var query = predicateResult.IsStarted ? _repositoryWrapper.MajorSoftwareBuild.FindByCondition(predicateResult, buildFilterDto.Deleted ?? false)

                            .Include(x => x.Orgeqpmanufacturer)
                            .Include(x => x.Operatingsystem)
                            .Include(x => x.Systemtypes)
                            .Include(x => x.Criticalassettype)
                            .Include(x => x.ModificationuserNavigation)
                            .Include(x => x.Productname)
                            .Include(x => x.CreationuserNavigation)
                            .Include(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                            .Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild)
                            .Include(x => x.Majorswbuildsdesigncontacts).ThenInclude(x => x.Designcontact)
                            .AsEnumerable()
                            .Select(p => MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(p)).AsQueryable()

                            :
                            _repositoryWrapper.MajorSoftwareBuild.FindAll()
                            .Include(x => x.Orgeqpmanufacturer)
                            .Include(x => x.Operatingsystem)
                            .Include(x => x.Systemtypes)
                            .Include(x => x.Criticalassettype)
                            .Include(x => x.CreationuserNavigation)
                            .Include(x => x.Productname)
                            .Include(x => x.ModificationuserNavigation)
                            .Include(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                            .Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild)
                            .Include(x => x.Majorswbuildsdesigncontacts).ThenInclude(x => x.Designcontact)
                            .AsEnumerable()
                            .Select(p => MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(p)).AsQueryable();

            if ((buildFilterDto.TCIBundleVersion != null && buildFilterDto.TCIBundleVersion.Contains("yes")) || (buildFilterDto.TCPBundleVersion != null && buildFilterDto.TCPBundleVersion.Contains("yes")))
            {
                query = query.Where(x => (x.SoftwarebuildcompatibilityMajorsoftwarebuild != null && x.SoftwarebuildcompatibilityMajorsoftwarebuild.Count <= 0) || (x.SoftwarebuildcompatibilityMajorsoftwarebuild == null));
            }

            rtn.TotalItems = query.Count();


            query = query.ApplyOrdering(buildFilterDto, GetColumnsMap())
                            .ApplyPaging(buildFilterDto);


            var data = query.ToList();

            if (buildFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.MajorSoftwareBuildsId == buildFilterDto.PrincipalId);
                if (!exist)
                {
                    var addedResource = _repositoryWrapper.MajorSoftwareBuild.FindAll(true)
                        .Include(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Productname)
                        .Include(x => x.Criticalassettype)
                        .Include(x => x.Operatingsystem)
                        .Include(x => x.Systemtypes)
                        .Include(x => x.Majorswbuildsdesigncontacts).ThenInclude(x => x.Designcontact)
                        .Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild)
                        .FirstOrDefault(x => x.Majorsoftwarebuildsid == buildFilterDto.PrincipalId);
                    data.Add(MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(addedResource));
                }
            }
            #endregion

            var majorSoftwareBuildResult = _mapper.Map<IEnumerable<MajorSoftwareBuildDtoGrid>>(data);
            rtn.Items = majorSoftwareBuildResult.ToArray();
            return rtn;
        }
        public ExpressionStarter<Majorsoftwarebuilds> ApplyFilter(MajorSoftwareBuildQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Majorsoftwarebuilds>();

            var predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

            #region start
            if (buildFilterDto.MajorSoftwareBuildId != null && buildFilterDto.MajorSoftwareBuildId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.MajorSoftwareBuildId)
                    predicateInner.Or(x => x.Majorsoftwarebuildsid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Description != null && buildFilterDto.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.Description)
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => x.Description == null || x.Description == "");
                    }
                    else
                    {
                        predicateInner.Or(x => x.Description == item);
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OriginalEquipmentManufacturer != null && buildFilterDto.OriginalEquipmentManufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.OriginalEquipmentManufacturer)
                    predicateInner.Or(x => x.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EndOfMaintenanceValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                if (buildFilterDto.EndOfMaintenanceValue.StartDate != null)
                    predicateInner.And(x => x.Endofmaintenance >= buildFilterDto.EndOfMaintenanceValue.StartDate);
                if (buildFilterDto.EndOfMaintenanceValue.EndDate != null)
                    predicateInner.And(x => x.Endofmaintenance <= buildFilterDto.EndOfMaintenanceValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.EndOfsupportValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                if (buildFilterDto.EndOfsupportValue.StartDate != null)
                    predicateInner.And(x => x.Endofsupport >= buildFilterDto.EndOfsupportValue.StartDate);
                if (buildFilterDto.EndOfsupportValue.EndDate != null)
                    predicateInner.And(x => x.Endofsupport <= buildFilterDto.EndOfsupportValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ProductName != null && buildFilterDto.ProductName.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

                foreach (var item in buildFilterDto.ProductName)
                    if (item == ConstantValueFilter.blankZeroValue)
                    {
                        predicateInner.Or(x => x.Productname == null || x.Productname.Description == null || x.Productname.Description == "");
                    }
                    else
                    {
                        predicateInner.Or(x => x.Productnameid == item);
                    }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SoftwareVersion != null && buildFilterDto.SoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

                foreach (var item in buildFilterDto.SoftwareVersion) predicateInner.Or(x => x.Softwareversion == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VulnerabilityStatus != null && buildFilterDto.VulnerabilityStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

                foreach (var item in buildFilterDto.VulnerabilityStatus)
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => x.Vulnerabilitystatus == null || x.Vulnerabilitystatus == "");
                    }
                    else
                    {
                        predicateInner.Or(x => x.Vulnerabilitystatus == item);
                    }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OperatingSystem != null && buildFilterDto.OperatingSystem.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

                foreach (var item in buildFilterDto.OperatingSystem)
                    if (item == ConstantValueFilter.blankZeroValue)
                    {
                        predicateInner.Or(x => x.Operatingsystem == null || x.Operatingsystem.Operatingsystemname == null || x.Operatingsystem.Operatingsystemname == "");
                    }
                    else
                    {
                        predicateInner.Or(x => x.Operatingsystemid == item);
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CriticalAssetType != null && buildFilterDto.CriticalAssetType.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

                foreach (var item in buildFilterDto.CriticalAssetType)
                    if (item == ConstantValueFilter.blankZeroValue)
                    {
                        predicateInner.Or(x => x.Criticalassettype == null || x.Criticalassettype.Description == null || x.Criticalassettype.Description == "");
                    }
                    else
                    {
                        predicateInner.Or(x => x.Criticalassettypeid == item);
                    }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastTimeBuyExpansions != null)
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                if (buildFilterDto.LastTimeBuyExpansions.StartDate != null)
                    predicateInner.And(x => x.Lasttimebuyexpansions >= buildFilterDto.LastTimeBuyExpansions.StartDate);
                if (buildFilterDto.LastTimeBuyExpansions.EndDate != null)
                    predicateInner.And(x => x.Lasttimebuyexpansions <= buildFilterDto.LastTimeBuyExpansions.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastTimeBuyNew != null)
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                if (buildFilterDto.LastTimeBuyNew.StartDate != null)
                    predicateInner.And(x => x.Lasttimebuynew >= buildFilterDto.LastTimeBuyNew.StartDate);
                if (buildFilterDto.LastTimeBuyNew.EndDate != null)
                    predicateInner.And(x => x.Lasttimebuynew <= buildFilterDto.LastTimeBuyNew.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastTimeBuyUpgrades != null)
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                if (buildFilterDto.LastTimeBuyUpgrades.StartDate != null)
                    predicateInner.And(x => x.Lasttimebuyupgrades >= buildFilterDto.LastTimeBuyUpgrades.StartDate);
                if (buildFilterDto.LastTimeBuyUpgrades.EndDate != null)
                    predicateInner.And(x => x.Lasttimebuyupgrades <= buildFilterDto.LastTimeBuyUpgrades.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.GeneraAvailableDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                if (buildFilterDto.GeneraAvailableDateValue.StartDate != null)
                    predicateInner.And(x => x.Generaavailabledate >= buildFilterDto.GeneraAvailableDateValue.StartDate);
                if (buildFilterDto.GeneraAvailableDateValue.EndDate != null)
                    predicateInner.And(x => x.Generaavailabledate <= buildFilterDto.GeneraAvailableDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DeliveryMethod != null && buildFilterDto.DeliveryMethod.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();

                foreach (var item in buildFilterDto.DeliveryMethod)
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => x.Deliverymethod == null || x.Deliverymethod == "");
                    }
                    else
                    {
                        predicateInner.Or(x => x.Deliverymethod == item);
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NetworkFunction != null && buildFilterDto.NetworkFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.NetworkFunction)
                    if (item == 0)
                    {
                        predicateInner.Or(x => (x.Majorsoftwarebuildnetworkfunction != null && x.Majorsoftwarebuildnetworkfunction.Count <= 0)
                        || (x.Majorsoftwarebuildnetworkfunction.Any(x => x.Networkfunction == null || x.Networkfunction.Description == null || x.Networkfunction.Description == "")));
                    }
                    else
                    {
                        predicateInner.Or(x => x.Majorsoftwarebuildnetworkfunction.Any(d => d.Networkfunctionid == item));
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }

            #region //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File   
            if (buildFilterDto.TCPBundleVersion != null && buildFilterDto.TCPBundleVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.TCPBundleVersion)
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => (x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null && x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Count <= 0)
                        || (x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Any(r => (r.Bundlemajorsoftwarebuild == null || r.Bundlemajorsoftwarebuild.Softwareversion == null
                        || r.Majorsoftwarebuild.Softwareversion == "") && r.Bundletype == (int)SoftwareCompatibilityEnum.TCPBundle)));
                    }
                    else
                    {
                        predicateInner.Or(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild
                    .Any(x => x.Bundlemajorsoftwarebuildid == (long)Convert.ToInt32(item) && x.Bundletype == (int)SoftwareCompatibilityEnum.TCPBundle));
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TCIBundleVersion != null && buildFilterDto.TCIBundleVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.TCIBundleVersion)
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => (x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null && x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Count <= 0)
                        || (x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Any(r => (r.Bundlemajorsoftwarebuild == null || r.Bundlemajorsoftwarebuild.Softwareversion == null
                        || r.Majorsoftwarebuild.Softwareversion == "") && r.Bundletype == (int)SoftwareCompatibilityEnum.TCPBundle)));
                    }
                    else
                    {
                        predicateInner.Or(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild
                   .Any(x => x.Bundlemajorsoftwarebuildid == (long)Convert.ToInt32(item) && x.Bundletype == (int)SoftwareCompatibilityEnum.TCIBundle));
                    }

                predicateResult.And(predicateInner);
            }
            #endregion

            if (buildFilterDto.DesignContact != null && buildFilterDto.DesignContact.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.DesignContact)
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => x.Majorswbuildsdesigncontacts != null && !x.Majorswbuildsdesigncontacts.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Majorswbuildsdesigncontacts.Any(t => t.Designcontactid.ToString() == item));
                    }
                predicateResult.And(predicateInner);

            }
            if (buildFilterDto.IsPlatform != null && buildFilterDto.IsPlatform.Any())
            {
                predicateInner = PredicateBuilder.New<Majorsoftwarebuilds>();
                foreach (var item in buildFilterDto.IsPlatform)
                    if (item.ToLower() == ConstantValueFilter.yes.ToLower())
                    {
                        predicateInner.Or(x => x.Isvmware == true);
                    }
                    else
                    {
                        predicateInner.Or(x => x.Isvmware == false);
                    }
                predicateResult.And(predicateInner);

            }
            #endregion
 
            if (buildFilterDto.GlobalSearchKeyword != null)
                predicateResult.And(ApplyGlobalFilter(buildFilterDto.GlobalSearchKeyword));

            return predicateResult;
        }
        public MajorSoftwareBuildDto Get(long id)
        {
            var entity = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == id).FirstOrDefault();
            return _mapper.Map<MajorSoftwareBuildDto>(entity);
        }
        private Dictionary<string, Expression<Func<MajorSoftwareBuild, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<MajorSoftwareBuild, object>>[]>
            {
                ["majorSoftwareBuildId"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.MajorSoftwareBuildsId },
                ["originalEquipmentManufacturerId"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.OriginalEquipmentManufacturerId },
                ["productName"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.ProductName != null ? p.ProductName.Description : "" },
                ["description"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.Description },
                ["softwareVersion"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.SoftwareVersion },
                ["lastTimeBuyNew"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.LastTimeBuyNew },
                ["lastModifiedValue"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.ModificationDate },
                ["lastTimeBuyUpgrades"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.LastTimeBuyUpgrades },
                ["lastTimeBuyExpansions"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.LastTimeBuyExpansions },
                ["endOfMaintenanceValue"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.EndOfMaintenance },
                ["endOfsupportValue"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.EndOfsupport },
                ["generaAvailableDateValue"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.GeneraAvailableDate },
                ["criticalAssetType"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.CriticalAssetType != null ? p.CriticalAssetType.Description : string.Empty },
                ["networkFunction"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.NetworkFunctions.FirstOrDefault() != null ? p.NetworkFunctions.FirstOrDefault().NetworkFunction.Description : string.Empty },
                ["deliveryMethod"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.DeliveryMethod },
                ["vulnerabilityStatus"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.VulnerabilityStatus },
                ["spareFieldsJson"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.SpareFieldsJson },
                ["originalEquipmentManufacturer"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription },
                ["operatingSystem"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.OperatingSystem != null ? p.OperatingSystem.OperatingSystemName : "" },
                ["lastModifiedBy"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.ModificationUserEntity.Email },
                ["tcpBundleVersion"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.SoftwareVersion },
                ["tciBundleVersion"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.SoftwareVersion },
                //["designContact"] = new Expression<Func<MajorSoftwareBuild, object>>[] { p => p.DesignContactNavigation.Email },
            };
        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter,
            MajorSoftwareBuildQueryDto buildFilterDto)
        {

            var predicateResult = ApplyFilter(buildFilterDto);

            // Ticket 831 - Remove Unknown Softwareversion in MajorSoftwareBuild Screen - Aug 1st 
            predicateResult.And(x => x.Softwareversion.ToLower() != "unknown");

            //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File   
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.MajorSoftwareBuild.FindByCondition(predicateResult)
                    .Include(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Operatingsystem)
                    .Include(x => x.Productname)
                    .Include(x => x.Criticalassettype)
                    .Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild).ThenInclude(x => x.Bundlemajorsoftwarebuild)
                    .Include(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                    .Include(x => x.Majorswbuildsdesigncontacts)
                : _repositoryWrapper.MajorSoftwareBuild.FindAll()
                    .Include(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Operatingsystem)
                    .Include(x => x.Productname)
                    .Include(x => x.Criticalassettype)
                    .Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild).ThenInclude(x => x.Bundlemajorsoftwarebuild)
                    .Include(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                    .Include(x => x.Majorswbuildsdesigncontacts)
                    ;

            var softwareCompatibilityRecord = query.Where(p => p.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null
            && p.SoftwarebuildcompatibilityMajorsoftwarebuild.Count() > 0).
            Select(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild.Select(x => new
            {
                x.Bundlemajorsoftwarebuildid,
                x.Bundlemajorsoftwarebuild.Softwareversion,
                x.Bundlemajorsoftwarebuild.Majorsoftwarebuildsid,
                x.Bundletype,
                x.Softwarebuildcompatibilityid
            }));

            var rtn = propertyName switch
            {
                "networkFunction" => string.IsNullOrEmpty(propertyFilter)
                   ? query.SelectMany(x => x.Majorsoftwarebuildnetworkfunction).Select(p =>
                   new FilterValueDto { Text = p.Networkfunction.Description, Value = p.Networkfunctionid.ToString() }).Distinct().ToList()
                   .Concat(query.Where(x => (x.Majorsoftwarebuildnetworkfunction != null && x.Majorsoftwarebuildnetworkfunction.Count <= 0) || x.Majorsoftwarebuildnetworkfunction.Any(x => x.Networkfunction == null)
                   || x.Majorsoftwarebuildnetworkfunction.Any(x => x.Networkfunction.Description == "" || x.Networkfunction.Description == null))
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList()
                    : query.Where(x => x.Majorsoftwarebuildnetworkfunction.Any(s => s.Networkfunction.Description.Contains(propertyFilter)))
                        .SelectMany(x => x.Majorsoftwarebuildnetworkfunction)
                        .Select(p =>
                            new FilterValueDto { Text = p.Networkfunction.Description, Value = p.Networkfunctionid.ToString() }).Distinct()
                        .ToList()
                        .Concat(query.Where(x => (x.Majorsoftwarebuildnetworkfunction != null && x.Majorsoftwarebuildnetworkfunction.Count <= 0) || x.Majorsoftwarebuildnetworkfunction.Any(x => x.Networkfunction == null)
                   || x.Majorsoftwarebuildnetworkfunction.Any(x => x.Networkfunction.Description == "" || x.Networkfunction.Description == null))
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList(),
                "description" => string.IsNullOrEmpty(propertyFilter) ?
                query.Select(x => new FilterValueDto(x.Description)).Distinct().ToList()
                .Concat(query.Where(x => x.Description == "" || x.Description == null)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.yes.ToLower(),
                          }
                       )).Distinct().ToList()
                : query.Where(x => x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)).Distinct().ToList()
                .Concat(query.Where(x => x.Description == "" || x.Description == null)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.yes.ToLower(),
                          }
                       )).Distinct().ToList(),

                "deliveryMethod" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Deliverymethod, Value = p.Deliverymethod }).Distinct().ToList()
                    .Concat(query.Where(x => x.Deliverymethod == null)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.yes.ToLower(),
                          }
                       )).Distinct().ToList()
                    : query
                        .Where(x => x.Deliverymethod.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.Deliverymethod, Value = p.Deliverymethod }).Distinct().ToList()
                        .Concat(query.Where(x => x.Deliverymethod == null)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.yes.ToLower(),
                          }
                       )).Distinct().ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationuserNavigation.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList(),

                "majorSoftwareBuildId" => string.IsNullOrEmpty(propertyFilter)
                          ? query.Select(p => new FilterValueDto
                          {
                              Text = p.Majorsoftwarebuildsid.ToString(),
                              Value = p.Majorsoftwarebuildsid.ToString()
                          }).Distinct().ToList()
                         : query
                             .Where(x =>
                                 x.Majorsoftwarebuildsid.ToString().Contains(
                                     propertyFilter)).Select(p => new FilterValueDto
                                     {
                                         Text = p.Majorsoftwarebuildsid.ToString(),
                                         Value = p.Majorsoftwarebuildsid.ToString()
                                     }).Distinct().ToList(),

                "softwareVersion" =>
                string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Softwareversion, Value = p.Softwareversion }).Distinct().ToList()
                    : query.Where(x => x.Softwareversion.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Softwareversion, Value = p.Softwareversion }).Distinct().ToList(),

                "productName" =>
                string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Productname != null ? p.Productname.Description : "", Value = p.Productnameid.ToString() }).Distinct().ToList()
                        .Concat(query.Where(x => x.Productname == null || x.Productname.Description == null || x.Productname.Description == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList()
                    : query.Where(x => x.Productname.Description.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Productname != null ? p.Productname.Description : "", Value = p.Productnameid.ToString() }).Distinct().ToList()
                        .Concat(query.Where(x => x.Productname == null || x.Productname.Description == null || x.Productname.Description == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList(),

                "operatingSystem" =>
                string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Operatingsystem.Operatingsystemname, Value = p.Operatingsystem.Operatingsystemid.ToString() }).Distinct().ToList()
                        .Concat(query.Where(x => x.Operatingsystem == null || x.Operatingsystem.Operatingsystemname == null || x.Operatingsystem.Operatingsystemname == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList()
                    : query.Where(x => x.Operatingsystem.Operatingsystemname.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Operatingsystem.Operatingsystemname, Value = p.Operatingsystem.Operatingsystemid.ToString() }).Distinct().ToList()
                        .Concat(query.Where(x => x.Operatingsystem == null || x.Operatingsystem.Operatingsystemname == null || x.Operatingsystem.Operatingsystemname == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList(),
                "criticalAssetType" =>
                string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Criticalassettype.Description, Value = p.Criticalassettype.Id.ToString() }).Distinct().ToList()
                        .Concat(query.Where(x => x.Criticalassettype == null || x.Criticalassettype.Description == null || x.Criticalassettype.Description == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList()
                    : query.Where(x => x.Criticalassettype.Description.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Criticalassettype.Description, Value = p.Criticalassettype.Id.ToString() }).Distinct().ToList()
                         .Concat(query.Where(x => x.Criticalassettype == null || x.Criticalassettype.Description == null || x.Criticalassettype.Description == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.blankZeroValue.ToString(),
                          }
                       )).Distinct().ToList(),

                "vulnerabilityStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Vulnerabilitystatus, Value = p.Vulnerabilitystatus }).Distinct().ToList()
                    .Concat(query.Where(x => x.Vulnerabilitystatus == null || x.Vulnerabilitystatus == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.yes.ToLower(),
                          }
                       )).Distinct().ToList()
                    : query
                        .Where(x => x.Vulnerabilitystatus.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.Vulnerabilitystatus, Value = p.Vulnerabilitystatus }).Distinct()
                        .ToList()
                        .Concat(query.Where(x => x.Vulnerabilitystatus == null || x.Vulnerabilitystatus == "")
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.yes.ToLower(),
                          }
                       )).Distinct().ToList(),
                "originalEquipmentManufacturer" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                        Value = p.Orgeqpmanufacturerid.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Orgeqpmanufacturer.Originalequipmentmanufacturer.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                                    Value = p.Orgeqpmanufacturer.ToString()
                                }).Distinct().ToList(),

                "designContact" => string.IsNullOrEmpty(propertyFilter)
                       ? _commonManager.isDesignContactInOrganisation(query.SelectMany(x => x.Majorswbuildsdesigncontacts.Where(x => x.Deleted == false).Select(p => new FilterValueDto
                       {
                           Text = p.Designcontact.Email,
                           Value = p.Designcontactid.ToString()
                       })).ToList())?.Distinct().ToList()
                       .Concat(query.Where(x => x.Majorswbuildsdesigncontacts != null && x.Majorswbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = ConstantValueFilter.blankTextValue,
                              Value = ConstantValueFilter.yes.ToLower(),
                          }
                       )).Distinct().ToList()
                       : _commonManager.isDesignContactInOrganisation(query.SelectMany(x => x.Majorswbuildsdesigncontacts.Where(x => x.Deleted == false).Select(p => new FilterValueDto
                       {
                           Text = p.Designcontact.Email,
                           Value = p.Designcontactid.ToString()
                       })).Distinct().ToList())?.Distinct().ToList()
                       .Concat(query.Where(x => x.Majorswbuildsdesigncontacts != null && x.Majorswbuildsdesigncontacts.Count() <= 0)
                        .Select(x =>

                            new FilterValueDto
                            {
                                Text = ConstantValueFilter.blankTextValue,
                                Value = ConstantValueFilter.yes.ToLower(),
                            }
                        )).Distinct().ToList(),

                "isPlatform" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Productname.Isplatformsoftware == true ? ConstantValueFilter.Yes : ConstantValueFilter.No)).Distinct().ToList()
                    : query
                        .Where(x => x.Isvmware.ToString().Contains(propertyFilter)).Select(p =>
                            new FilterValueDto(p.Productname.Isplatformsoftware == true ? ConstantValueFilter.Yes : ConstantValueFilter.No)).Distinct()
                        .ToList(),


                #region
                "tcpBundleVersion" => string.IsNullOrEmpty(propertyFilter)
                   ? softwareCompatibilityRecord.SelectMany(x => x).ToList().Where(x => x.Bundletype == (int)SoftwareCompatibilityEnum.TCPBundle).
                   Select(x => new FilterValueDto
                   {
                       Text = x.Softwareversion,
                       Value = x.Bundlemajorsoftwarebuildid.ToString()
                   }).Distinct().ToList()
                    .Concat(query.Where(x => x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null && x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Count <= 0)
                                    .Select(x =>

                            new FilterValueDto
                            {
                                Text = ConstantValueFilter.blankTextValue,
                                Value = ConstantValueFilter.yes.ToLower(),
                            }
                        )).Distinct().ToList()
                    : softwareCompatibilityRecord.SelectMany(x => x).ToList().Where(x => x.Bundletype == (int)SoftwareCompatibilityEnum.TCPBundle
                         ).
                       Select(x => new FilterValueDto
                       {
                           Text = x.Softwareversion,
                           Value = x.Bundlemajorsoftwarebuildid.ToString()
                       }).Where(x => propertyFilter.Contains(x.Value)).Distinct().ToList()
                       .Concat(query.Where(x => x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null && x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Count <= 0)
                                        .Select(x =>

                            new FilterValueDto
                            {
                                Text = ConstantValueFilter.blankTextValue,
                                Value = ConstantValueFilter.yes.ToLower(),
                            }
                        )).Distinct().ToList(),

                "tciBundleVersion" => string.IsNullOrEmpty(propertyFilter)
                        ? softwareCompatibilityRecord.SelectMany(x => x).ToList().Where(x => x.Bundletype == (int)SoftwareCompatibilityEnum.TCIBundle).
                        Select(x => new FilterValueDto
                        {
                            Text = x.Softwareversion,
                            Value = x.Bundlemajorsoftwarebuildid.ToString()
                        }).Distinct().ToList()
                        .Concat(query.Where(x => x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null && x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Count <= 0)
                            .Select(x =>

                            new FilterValueDto
                            {
                                Text = ConstantValueFilter.blankTextValue,
                                Value = ConstantValueFilter.yes.ToLower(),
                            }
                        )).Distinct().ToList()
                       : softwareCompatibilityRecord.SelectMany(x => x).ToList().Where(x => x.Bundletype == (int)SoftwareCompatibilityEnum.TCIBundle
                       ).
                          Select(x => new FilterValueDto
                          {
                              Text = x.Softwareversion,
                              Value = x.Bundlemajorsoftwarebuildid.ToString()
                          }).Where(x => propertyFilter.Contains(x.Value)).Distinct().ToList()
                         .Concat(query.Where(x => x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild != null && x.SoftwarebuildcompatibilityBundlemajorsoftwarebuild.Count <= 0)
                        .Select(x =>

                            new FilterValueDto
                            {
                                Text = ConstantValueFilter.blankTextValue,
                                Value = ConstantValueFilter.yes.ToLower(),
                            }
                        )).Distinct().ToList(),

                #endregion
                _ => new List<FilterValueDto>(),
            };



            return rtn;
        }
        public async Task<MajorSoftwareBuildDtoCreate> GetCreatePage(long sessionUserId)
        {
            var resource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll();

            var criticalAssetTypesResource = _repositoryWrapper.CriticalAssetTypeRepository.FindAll();
            var operatingSystemResource = _repositoryWrapper.OperatingSystem.FindAll();
            var productNameResource = _repositoryWrapper.ProductNameRepository.FindAll().Select(x => new ProductNameDropdownList
            {
                Key = (short)x.Productnameid,
                Value = x.Description,
                IsSelected = (bool)(x.Isplatformsoftware == null ? false : x.Isplatformsoftware)
            }).ToList();
            var networkFunctionsResource = _repositoryWrapper.NetworkFunctionRepository.FindAll()
            .ToDictionary(k => k.Id, v => v.Description);
            //#2050 -Software Creation
            var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(sessionUserId, true);

            var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                              userDetailsList.VerticalDetails : new List<int>();

            var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                          userDetailsList.OpcoDetails : new List<short>();

            var designContacts = _commonManager.GetDesignContactBasedOnOpcoAndVerticalIds(opCoIds, verticalIds);
            //var designContacts = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Isdesigncontact == true).ToListAsync();

            #region //Ticket 743 NFVI Bundle
            var bundleVersion = await _dropdownDataServiceManager.GetAllVmwareFromMajorSwBuildDto();

            var model = new MajorSoftwareBuildDtoCreate
            {
                OriginalEquipmentManufacturerResource = resource.ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer),
                CriticalAssetTypeResource = criticalAssetTypesResource.ToDictionary(x => x.Id, x => x.Description),
                OperatingSystemResource = operatingSystemResource.ToDictionary(x => x.Operatingsystemid, x => x.Operatingsystemversion != null ? x.Operatingsystemname + " - " + x.Operatingsystemversion : x.Operatingsystemname),
                ProductNamesResource = productNameResource,
                EOMStatus = Enum.EOMEnum.NotSpecified,
                NetworkFunctionsResource = networkFunctionsResource,
                NetworkFunctionsIds = new List<int>(),
                //Ticket 743 NFVI Bundle
                TCIBundleVersion = (IDictionary<long, string>)bundleVersion,
                TCPBundleVersion = (IDictionary<long, string>)bundleVersion,
                DesignContacts = designContacts.Where(x => x.Value != "1").DistinctBy(x => x.Value).ToDictionary(x => Convert.ToInt32(x.Value), x => x.Text)
                //DesignContacts = designContacts.Where(x => x.Id != 1).DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email)
            };

            #endregion
            return model;
        }
        public async Task<MajorSoftwareBuildDtoUpdate> GetUpdatePage(long id, long sessionUserId)
        {
            //Ticket 743 NFVI Bundle
            var entity = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == id, true)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild)
                 .Include(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                .FirstOrDefault();

            //#2050 -Software Creation
            var userDetailsList = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(sessionUserId, true);

            var verticalIds = userDetailsList.VerticalDetails != null && userDetailsList.VerticalDetails.Count > 0 ?
                              userDetailsList.VerticalDetails : new List<int>();

            var opCoIds = userDetailsList.OpcoDetails != null && userDetailsList.OpcoDetails.Count > 0 ?
                          userDetailsList.OpcoDetails : new List<short>();

            var designContacts = _commonManager.GetDesignContactBasedOnOpcoAndVerticalIds(opCoIds, verticalIds);
            //var designContacts = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Isdesigncontact == true).ToListAsync();

            var model = MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(entity);
            // model.NetworkFunctions = entity.Majorsoftwarebuildnetworkfunction.Select(p => MajorSoftwareBuildNetworkFunctionMapper.Get(p)).ToList();

            var dto = _mapper.Map<MajorSoftwareBuildDtoUpdate>(model);

            if (dto.NetworkFunctionsIds == null) dto.NetworkFunctionsIds = new List<int>();

            #region lookUp

            dto.NetworkFunctionsResource = _repositoryWrapper.NetworkFunctionRepository.FindAll()
          .ToDictionary(k => k.Id, v => v.Description);

            var resource = _repositoryWrapper.OriginalEquipmentManufacturer.FindAll();
            dto.OriginalEquipmentManufacturerResource = resource.ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer);

            if (!dto.OriginalEquipmentManufacturerResource.ContainsKey(dto.OriginalEquipmentManufacturerId))
            {
                var oem = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId,
                    includeDeleted: true).FirstOrDefault();
                if (oem != null)
                {
                    dto.OriginalEquipmentManufacturerResource.Add(oem.Orgeqpmanufacturerid, oem.Originalequipmentmanufacturer);
                }
            }

            var productNameResource = _repositoryWrapper.ProductNameRepository.FindAll().Select(x => new ProductNameDropdownList
            {
                Key = (short)x.Productnameid,
                Value = x.Description,
                IsSelected = (bool)(x.Isplatformsoftware == null ? false : x.Isplatformsoftware)
            }).ToList();
            dto.ProductNamesResource = productNameResource;
            dto.ProductNameId = entity.Productnameid;

            var operatingSystemResource = _repositoryWrapper.OperatingSystem.FindAll();
            dto.OperatingSystemResource = operatingSystemResource.ToDictionary(x => x.Operatingsystemid, x => x.Operatingsystemversion != null ? x.Operatingsystemname + " - " + x.Operatingsystemversion : x.Operatingsystemname);

            var criticalAssetTypesResource = _repositoryWrapper.CriticalAssetTypeRepository.FindAll();
            dto.CriticalAssetTypeResource = criticalAssetTypesResource.ToDictionary(x => x.Id,
                x => x.Description);
            if (dto.CriticalAssetTypeId.HasValue && !dto.CriticalAssetTypeResource.ContainsKey(dto.CriticalAssetTypeId.Value))
            {
                var data = _repositoryWrapper.CriticalAssetTypeRepository.FindByCondition(
                   x => x.Id == dto.CriticalAssetTypeId,
                   includeDeleted: true).FirstOrDefault();

                var criticalAssetT = _repositoryWrapper.CriticalAssetTypeRepository.FindByCondition(
                    x => x.Id == dto.CriticalAssetTypeId,
                    includeDeleted: true).FirstOrDefault();
                if (data != null)
                {
                    dto.CriticalAssetTypeResource.Add(criticalAssetT.Id, criticalAssetT.Description);
                }
            }

            if (dto.ProductNameId.HasValue && !dto.ProductNamesResource.Any(x => x.Key == dto.ProductNameId.Value))
            {
                var data = _repositoryWrapper.ProductNameRepository.FindByCondition(
                    x => x.Productnameid == dto.ProductNameId,
                    includeDeleted: true).FirstOrDefault();
                if (data != null)
                {
                    dto.ProductNamesResource.Add(new ProductNameDropdownList
                    {
                        Key = (short)data.Productnameid,
                        Value = data.Description,
                        IsSelected = (bool)(data.Isplatformsoftware == null ? false : data.Isplatformsoftware)
                    });
                }


                if (dto.OperatingSystemId.HasValue && !dto.OperatingSystemResource.ContainsKey(dto.OperatingSystemId.Value))
                {
                    var Os = _repositoryWrapper.OperatingSystem.FindByCondition(
                        x => x.Operatingsystemid == dto.OperatingSystemId,
                        includeDeleted: true).FirstOrDefault();
                    if (data != null)
                    {
                        dto.OperatingSystemResource.Add(Os.Operatingsystemid, Os.Operatingsystemname);
                    }
                }
                #endregion

            }

            dto.DesignContactIds = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorsoftwarebuildsid == entity.Majorsoftwarebuildsid).Select(x => x.Designcontactid).ToList();
            dto.DesignContacts = designContacts.DistinctBy(x => x.Value).ToDictionary(x => Convert.ToInt32(x.Value), x => x.Text);
            // dto.DesignContacts = designContacts.DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email);
            if (dto.DesignContactIds != null)
            {
                var predicateResult = PredicateBuilder.New<Aspnetusers>(true);
                var predicateInner = PredicateBuilder.New<Aspnetusers>(true);
                dto.DesignContacts ??= new Dictionary<int, string>();
                if (dto.DesignContactIds.Any())
                {
                    foreach (var item in dto.DesignContactIds)
                    {
                        if (dto.DesignContacts.Any(y => y.Key != item))
                            predicateInner.Or(x => x.Id == item);
                    }

                    predicateResult.And(predicateInner);
                }

                var missingDesigncontact = _repositoryWrapper.UserRepository.FindByCondition(predicateResult).ToList();
                if (missingDesigncontact != null && missingDesigncontact.Count > 0)
                {
                    foreach (var item in missingDesigncontact)
                    {
                        if (!dto.DesignContacts.ContainsKey(item.Id))
                            dto.DesignContacts.Add(item.Id, item.Email);
                    }

                }
            }

            #region //Ticket 743 NFVI Bundle
            dto.TCISoftwareCompatibilityIdList = entity.SoftwarebuildcompatibilityMajorsoftwarebuild.Where(x => x.Bundletype ==
            (short)SoftwareCompatibilityEnum.TCIBundle).Select(x => x.Bundlemajorsoftwarebuildid).ToList();


            dto.TCPSoftwareCompatibilityIdList = entity.SoftwarebuildcompatibilityMajorsoftwarebuild.Where(x => x.Bundletype ==
            (short)SoftwareCompatibilityEnum.TCPBundle).Select(x => x.Bundlemajorsoftwarebuildid).ToList();

            var bundleVersion = _dropdownDataServiceManager.GetAllVmwareFromMajorSwBuildDto().Result;

            //Ticket 743 NFVI Bundle
            dto.TCIBundleVersion = bundleVersion;//.Where(x => x.Value.ToLower().StartsWith("tcl"));
            dto.TCPBundleVersion = bundleVersion;//.Where(x => !x.Value.ToLower().StartsWith("tcl"));

            if (bundleVersion != null)
            {
                var deletedBundleVersion = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x =>
                   x.Isvmware == true, true).ToList();

                foreach (var item in dto?.TCISoftwareCompatibilityIdList)
                {
                    var deletedTclBundleVersion = deletedBundleVersion.Where(x => x.Majorsoftwarebuildsid == item).FirstOrDefault();

                    if (!dto.TCIBundleVersion.ContainsKey((short)deletedTclBundleVersion.Majorsoftwarebuildsid))
                        dto.TCIBundleVersion.Add((short)deletedTclBundleVersion.Majorsoftwarebuildsid, deletedTclBundleVersion.Softwareversion);

                }
                foreach (var item in dto?.TCPSoftwareCompatibilityIdList)
                {
                    var deletedTcpBundleVersion = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == item)
                              .FirstOrDefault();
                    if (!dto.TCPBundleVersion.ContainsKey((short)deletedTcpBundleVersion.Majorsoftwarebuildsid))
                        dto.TCPBundleVersion.Add((short)deletedTcpBundleVersion.Majorsoftwarebuildsid, deletedTcpBundleVersion.Softwareversion);
                }

            }



            #endregion
            return dto;
        }
        public ExpressionStarter<T> FilterMode<T>(Expression<Func<T, bool>> expression, ExpressionStarter<T> predicate, FilterModeEnum mode)
        {
            if (mode == FilterModeEnum.And)
            {
                predicate.And(expression);
            }
            else
            {
                predicate.Or(expression);
            }
            return predicate;
        }
        public async Task<ResultDto> Restore(long id)
        {
            var entity = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == id, true).FirstOrDefaultAsync();
            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(
                x => x.Orgeqpmanufacturerid == entity.Orgeqpmanufacturerid
                    && x.Productnameid == entity.Productnameid
                    && x.Productname.Description == entity.Productname.Description
                    && x.Softwareversion == entity.Softwareversion
                    && x.Majorsoftwarebuildsid != entity.Majorsoftwarebuildsid).FirstOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.Majorsoftwarebuildsid
                };
            }
            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.MajorSoftwareBuild.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Majorsoftwarebuildsid
            };
        }
        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }
        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation(DataRemediationDto data)
        {
            List<long> systemTypesIds = new List<long>();
            var existsRelationWithSt = _repositoryWrapper.SystemType
                .FindByCondition(x => x.Majorsoftwarebuildsid == data.CorrectId, true, false).ToList();

            foreach (var item in data.DuplicatesId)
            {
                //Get delle relazioni con systemtype
                var systemTypesWithDuplicates = _repositoryWrapper.SystemType
                    .FindByCondition(x => x.Majorsoftwarebuildsid == item, true, false).ToList();
                systemTypesIds.AddRange(systemTypesWithDuplicates.Select(x => x.Systemtypeid));
                foreach (var st in systemTypesWithDuplicates)
                {
                    st.Majorsoftwarebuildsid = data.CorrectId;
                    var entityExists = existsRelationWithSt
                        .FirstOrDefault(str => str.Systemtypeid == st.Systemtypeid);
                    if (entityExists == null)
                    {
                        _repositoryWrapper.SystemType.Update(st);
                    }
                }
                _repositoryWrapper.Save();
                // cancello i duplicati

                var sw = _repositoryWrapper.MajorSoftwareBuild
                    .FindByCondition(x => x.Majorsoftwarebuildsid == item, true).FirstOrDefault();
                _repositoryWrapper.MajorSoftwareBuild.DeleteDeep(sw);
                _repositoryWrapper.Save();
            }
            //ricalcolo i valori di systemType
            foreach (var id in systemTypesIds)
            {
                await this.updateSystemTypeEOMDateConstraintLCMandScaling(id);

            }
            var sistemTypeIds = _repositoryWrapper.SystemTypesMajorHardwareBuild
                    .FindByCondition(x => x.Ismain && !x.Deleted.Value && x.Majorhardwareid == data.CorrectId)
                    .Select(x => x.Systemtypeid).ToList();
            foreach (long pp in sistemTypeIds)
            {
                var lista = new List<long>();
                lista.Add(pp);
                await _plannedActivityCommon.ReloadPlannedActivity(lista, true);
            }

            return new ResultDto<ResultDataRemediationDto>()
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = new ResultDataRemediationDto() { Id = systemTypesIds }
            };

        }
        private async Task updateSystemTypeEOMDateConstraintLCMandScaling(long id)
        {
            var systemTypes = _repositoryWrapper.SystemType
                .FindByCondition(x => x.Systemtypeid == id, false, false)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .FirstOrDefault();

            systemTypes.Constraintlcm = systemTypes.GetStatusName(_repositoryWrapper);
            systemTypes.Constraintscaling = systemTypes.GetMinorDate(_repositoryWrapper);
            var result = systemTypes.GetMinorDateEOM(_repositoryWrapper);
            systemTypes.Endofmaintenance = result == "Not Announced" || result == "Not Specified" ? null : (DateTime?)Convert.ToDateTime(result);

            _repositoryWrapper.SystemType.Update(systemTypes);
            await _repositoryWrapper.SaveAsync();
        }
        public async Task<ResultDto<MajorSoftwareBuildToCloneDto>> GetMajorSoftwareBuildToClone(long id)
        {

            var entity = await Task.Run(() => _repositoryWrapper.MajorSoftwareBuild
                .FindByCondition(x => x.Majorsoftwarebuildsid == id)
                .Include(x => x.Productname).Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild).FirstOrDefault());

            var dto = new MajorSoftwareBuildToCloneDto()
            {
                MajorSoftwareBuildsId = entity.Majorsoftwarebuildsid,
                ProductNameId = entity.Productnameid,
                ProductName = entity.Productname.Description != null ? entity.Productname.Description : "",
                SoftwareVersion = entity.Softwareversion,
                Isvmware = entity.Isvmware,
            };

            dto.OriginalEquipmentManufacturer = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(
                    x => x.Orgeqpmanufacturerid == entity.Orgeqpmanufacturerid,
                    includeDeleted: true).FirstOrDefault().Originalequipmentmanufacturer;

            var designResource = _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Systemtype.Majorsoftwarebuildsid == entity.Majorsoftwarebuildsid)
                .Include(x => x.Systemtype)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Include(x => x.Designcomponentfamily)
                .ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction);

            dto.DesignComponentResource = designResource.toDesignComponentResource(_repositoryWrapper);

            var designContacts = _repositoryWrapper.UserRepository.FindByCondition(x => x.Isdesigncontact == true).ToList();
            dto.DesignContactIds = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorsoftwarebuildsid == entity.Majorsoftwarebuildsid).Select(x => x.Designcontactid).ToList();
            dto.DesignContacts = designContacts.DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email);

            #region //Ticket 743 NFVI Bundle
            dto.TCISoftwareCompatibilityIdList = entity.SoftwarebuildcompatibilityMajorsoftwarebuild.Where(x => x.Bundletype ==
            (short)SoftwareCompatibilityEnum.TCIBundle).Select(x => x.Bundlemajorsoftwarebuildid).ToList();


            dto.TCPSoftwareCompatibilityIdList = entity.SoftwarebuildcompatibilityMajorsoftwarebuild.Where(x => x.Bundletype ==
            (short)SoftwareCompatibilityEnum.TCPBundle).Select(x => x.Bundlemajorsoftwarebuildid).ToList();

            var bundleVersion = _dropdownDataServiceManager.GetAllVmwareFromMajorSwBuildDto().Result;

            //Ticket 743 NFVI Bundle
            dto.TCIBundleVersion = bundleVersion;//.Where(x => x.Value.ToLower().StartsWith("tcl"));
            dto.TCPBundleVersion = bundleVersion;//.Where(x => !x.Value.ToLower().StartsWith("tcl"));

            if (bundleVersion != null)
            {
                var deletedBundleVersion = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x =>
                   x.Isvmware == true, true).ToList();

                foreach (var item in dto?.TCISoftwareCompatibilityIdList)
                {
                    var deletedTclBundleVersion = deletedBundleVersion.Where(x => x.Majorsoftwarebuildsid == item).FirstOrDefault();

                    if (!dto.TCIBundleVersion.ContainsKey((short)deletedTclBundleVersion.Majorsoftwarebuildsid))
                        dto.TCIBundleVersion.Add((short)deletedTclBundleVersion.Majorsoftwarebuildsid, deletedTclBundleVersion.Softwareversion);

                }
                foreach (var item in dto?.TCPSoftwareCompatibilityIdList)
                {
                    var deletedTcpBundleVersion = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Majorsoftwarebuildsid == item)
                              .FirstOrDefault();
                    if (!dto.TCPBundleVersion.ContainsKey((short)deletedTcpBundleVersion.Majorsoftwarebuildsid))
                        dto.TCPBundleVersion.Add((short)deletedTcpBundleVersion.Majorsoftwarebuildsid, deletedTcpBundleVersion.Softwareversion);
                }

            }



            #endregion

            return new ResultDto<MajorSoftwareBuildToCloneDto>()
            {
                Info = ResultMessages.GetInfoSuccess,
                Warning = false,
                Data = dto
            };
        }
        public async Task<ResultDto> GetInfoMajorSoftwareBuildToClone(long id, bool? referenceSW = false)
        {
            //Ticket 744 - Add New SW build - the popup should be shown to display the libraries that is going to be created
            if (referenceSW == true)
            {
                var majorSWEntity = MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(_repositoryWrapper.
                               SystemType.FindByCondition(x => x.Systemtypeid == id)
                               .Include(x => x.Majorsoftwarebuilds).Select(x => x.Majorsoftwarebuilds).FirstOrDefault());
                id = (long)(majorSWEntity?.MajorSoftwareBuildsId);
            }

            var designComponentExists = await Task.Run(() => _repositoryWrapper.DesignComponent
              .FindByCondition(x => x.Systemtype.Majorsoftwarebuildsid == id)
              .Include(x => x.Systemtype));
            if (designComponentExists.Any())
            {
                return new ResultDto()
                {
                    Info = ResultMessages.GetInfoSuccess,
                    Warning = false
                };
            }
            else
            {
                var systemTypeEntity = _repositoryWrapper.SystemType
                    .FindByCondition(x => x.Majorsoftwarebuildsid == id);
                if (systemTypeEntity.Any())
                {
                    return new ResultDto()
                    {
                        Info = ResultMessages.GetInfoDc,
                        Warning = true
                    };
                }
                else
                {
                    return new ResultDto()
                    {
                        Info = ResultMessages.GetInfoSt,
                        Warning = true
                    };
                }
            }

        }
        public async Task<ResultDto> CloneMajorSoftwareBuild(CloneMajorSoftwareBuildDto dto, long? existSystemTypeId = 0)
        {
            var transaction = await _repositoryWrapper.BeginTransactionAsync();
            try
            {
                #region  //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File  - July 10th 24
                var entityToClone = _repositoryWrapper.MajorSoftwareBuild.FindByCondition(
                    x => x.Majorsoftwarebuildsid == dto.MajorSoftwareBuildsId)
                   .Include(x => x.Criticalassettype)
                    .Include(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                   .Include(x => x.Systemtypes).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Subnetworkboundary)
                   .Include(x => x.Orgeqpmanufacturer)
                   .Include(x => x.Productname)
                   .Include(x => x.Systemtypes).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                   .Include(x => x.Systemtypes).ThenInclude(x => x.Systemtypessubdomainspoc)
                   .Include(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild)
                   .FirstOrDefault();

                #endregion


                var entityExists = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(
                    x => x.Orgeqpmanufacturerid == entityToClone.Orgeqpmanufacturerid
                    && x.Productnameid == entityToClone.Productnameid
                         && x.Productname.Description == entityToClone.Productname.Description
                         && x.Softwareversion.ToLower().Replace(" ", "").Replace(".0", "") == dto.SoftwareVersion.ToLower().Replace(" ", "").Replace(".0", ""))
                    .Include(x => x.Criticalassettype)
                    .Include(x => x.Systemtypes).OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();

                if (entityExists != null)
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryAddExists
                    };
                }
                else
                {
                    #region //Ticket 642 - Add - Software Upgrade Utility to be enhanced to cover all required design components, System Types and Sub network boundary is created 
                    dto.EndOfMaintenance = (existSystemTypeId != 0) ? dto.MSWCreateDto.EndOfMaintenance :
                        dto.EndOfMaintenance;


                    MajorSoftwareBuildDtoCreate msDto = new MajorSoftwareBuildDtoCreate();
                    msDto.OriginalEquipmentManufacturerId = (short)((existSystemTypeId != 0) ? dto.MSWCreateDto.OriginalEquipmentManufacturerId : entityToClone.Orgeqpmanufacturerid);
                    msDto.ProductNameId = (existSystemTypeId != 0) ? dto.MSWCreateDto.ProductNameId : entityToClone.Productnameid;
                    msDto.DeliveryMethod = (existSystemTypeId != 0) ? dto.MSWCreateDto.DeliveryMethod : entityToClone.Deliverymethod;
                    msDto.OperatingSystemId = (existSystemTypeId != 0) ? dto.MSWCreateDto.OperatingSystemId : entityToClone.Operatingsystemid;
                    msDto.SoftwareVersion = (existSystemTypeId != 0) ? dto.MSWCreateDto.SoftwareVersion : dto.SoftwareVersion;
                    msDto.CriticalAssetTypeId = (existSystemTypeId != 0) ? dto.MSWCreateDto.CriticalAssetTypeId : entityToClone.Criticalassettypeid;

                    msDto.EndOfMaintenance = dto.EndOfMaintenance != DateTime.MinValue ? dto.EndOfMaintenance : null;

                    msDto.CriticalAssetTypeId = (existSystemTypeId != 0) ? dto.MSWCreateDto.OperatingSystemId : entityToClone.Criticalassettypeid;
                    msDto.Description = (existSystemTypeId != 0) ? dto.MSWCreateDto.Description : entityToClone.Description;

                    msDto.VulnerabilityStatus = (existSystemTypeId != 0) ? dto.MSWCreateDto.VulnerabilityStatus : entityToClone.Vulnerabilitystatus;

                    msDto.NetworkFunctionsIds = (existSystemTypeId != 0) ? dto.MSWCreateDto.NetworkFunctionsIds : entityToClone.Majorsoftwarebuildnetworkfunction.Select(x => x.Networkfunctionid).ToList();

                    if (msDto.EndOfMaintenance != null && !string.IsNullOrEmpty(msDto.DeliveryMethod) && msDto.DeliveryMethod == "One Track")
                    {
                        var lastDayOfMonth = DateTime.DaysInMonth(msDto.EndOfMaintenance.Value.Year, msDto.EndOfMaintenance.Value.Month);
                        var GADate = msDto.EndOfMaintenance.Value.AddMonths(-18);
                        var EndOfsupportDate = msDto.EndOfMaintenance.Value.AddMonths(12);
                        if (lastDayOfMonth == msDto.EndOfMaintenance.Value.Day)
                        {
                            msDto.GeneraAvailableDate = new DateTime(GADate.Year, GADate.Month, DateTime.DaysInMonth(GADate.Year, GADate.Month));
                            msDto.EndOfsupport = new DateTime(EndOfsupportDate.Year, EndOfsupportDate.Month, DateTime.DaysInMonth(EndOfsupportDate.Year, EndOfsupportDate.Month));

                        }
                        else
                        {
                            msDto.GeneraAvailableDate = GADate;
                            msDto.EndOfsupport = EndOfsupportDate;

                        }

                    }
                    else
                    {
                        msDto.GeneraAvailableDate = null;
                        msDto.EndOfsupport = dto.EndOfsupport;
                    }


                    msDto.EOMStatus = (existSystemTypeId != 0) ? dto.MSWCreateDto.EOMStatus : dto.EomStatus;
                    msDto.IsPlatform = (existSystemTypeId != 0) ? dto.Isvmware : entityToClone.Isvmware;


                    if (
                           msDto.DeliveryMethod != null &&
                           (msDto.DeliveryMethod.ToUpper() == "CI/CD" || msDto.DeliveryMethod.ToUpper().Replace(" ", "") == "ONETRACK") &&
                           entityToClone.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Contains("ericsson")
                       )
                    {
                        if (msDto.EndOfMaintenance != null)
                        {
                            var lastDayOfMonth = DateTime.DaysInMonth(msDto.EndOfMaintenance.Value.Year, msDto.EndOfMaintenance.Value.Month);

                            var GADate = (msDto.EndOfMaintenance.Value.AddMonths(-18));

                            if (lastDayOfMonth == msDto.EndOfMaintenance.Value.Day)
                            {
                                msDto.GeneraAvailableDate = new DateTime(GADate.Year, GADate.Month, DateTime.DaysInMonth(GADate.Year, GADate.Month));

                            }
                            else
                            {
                                msDto.GeneraAvailableDate = GADate;
                            }

                        }
                    }
                    var newMs = await AddBase(msDto);

                    #endregion

                    await AddOrUpdateDesignContact(dto.DesignContactIds, (long)newMs.Data);

                    #region Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File

                    SoftwareBuildCompatibilityListDtoCreateUpdate swCompatibilityObj = new SoftwareBuildCompatibilityListDtoCreateUpdate()
                    {
                        TCPSoftwareCompatibilityId = (dto.MSWCreateDto == null) ? dto.TCPSoftwareCompatibilityIdList : (dto.MSWCreateDto.TCPSoftwareCompatibilityIdList != null && dto.MSWCreateDto.TCPSoftwareCompatibilityIdList.Count > 0) ?
                     dto.MSWCreateDto.TCPSoftwareCompatibilityIdList : null,
                        TCISoftwareCompatibilityId = (dto.MSWCreateDto == null) ? dto.TCISoftwareCompatibilityIdList : (dto.MSWCreateDto.TCISoftwareCompatibilityIdList != null && dto.MSWCreateDto.TCISoftwareCompatibilityIdList.Count > 0) ?
                     dto.MSWCreateDto.TCISoftwareCompatibilityIdList : null,
                        MajorSoftwareBuildId = (long)newMs.Data
                    };

                    await _softwareBuildCompatibilityManager.Add(swCompatibilityObj);


                    #endregion

                    foreach (var item in msDto.NetworkFunctionsIds)
                    {
                        _repositoryWrapper.MajorSoftwareBuildNetworkFunction.Create(new Majorsoftwarebuildnetworkfunction() { Majorsoftwarebuildid = (long)newMs.Data, Networkfunctionid = item });
                    }

                    await _repositoryWrapper.SaveAsync();
                    if (entityToClone.Systemtypes.Any())
                    {
                        //Ticket 642 - Add - Software Upgrade Utility to be enhanced to cover all required design components, System Types and Sub network boundary is created 
                        var systemtypesEntity = (existSystemTypeId != 0) ? entityToClone.Systemtypes.Where(x => x.Systemtypeid == existSystemTypeId).ToList()
                            : entityToClone.Systemtypes;


                        foreach (var systemType in systemtypesEntity)
                        {
                            SystemTypeDtoCreate systemTypeDto = new SystemTypeDtoCreate();
                            systemTypeDto.MajorSoftwareBuildsId = (long)newMs.Data;
                            List<MajorHardwareBuildMainSystemTypeDto> lista = new List<MajorHardwareBuildMainSystemTypeDto>();
                            foreach (var item in systemType.Systemtypesmajorhardwarebuilds)
                            {
                                MajorHardwareBuildMainSystemTypeDto mh = new MajorHardwareBuildMainSystemTypeDto()
                                {
                                    MajorHardwareBuildId = item.Majorhardwareid,
                                    IsMain = item.Ismain
                                };
                                lista.Add(mh);
                            }

                            systemTypeDto.MajorHardwareBuildId = lista;
                            systemTypeDto.ProductImportanceId = systemType.Productimportanceid;

                            List<int> subDomainSpoc = new List<int>();

                            systemTypeDto.AssetCategoryId = systemType.Assetcategoryid;
                            systemTypeDto.AssetClassId = systemType.Assetclassid;
                            systemTypeDto.ConstraintScalings = systemType.Constraintscaling;
                            systemTypeDto.SystemTypeName3Gpp = systemType.Systemtypename3gpp;
                            systemTypeDto.VodafoneName = systemType.VodafonenameNavigation != null ? systemType.VodafonenameNavigation.Description : null;
                            systemTypeDto.vodafoneNameId = systemType.Vodafonename;
                            var mhMain = lista.FirstOrDefault(x => x.IsMain == true)?.MajorHardwareBuildId;
                            systemTypeDto.SystemTypeNameOem = entityToClone.Productname != null ? entityToClone.Productname?.Description : "";

                            var newSt = await _systemTypeManager.AddBase(systemTypeDto);

                            await _repositoryWrapper.SaveAsync();

                            //clono i dc
                            #region  Ticket 646 - Dev - 311 - Req3026: Delinking Archived / Libraries
                            var getActiveDcList = systemType.Designcomponents.Where(x => x.Deleted == false).ToList();
                            if (getActiveDcList != null && getActiveDcList.Count > 0)
                            {
                                foreach (var dc in getActiveDcList)
                                {
                                    var t = newSt.Data as Systemtypes;
                                    DesignComponentDtoCreate dcDto = new DesignComponentDtoCreate();
                                    dcDto.SystemTypeId = t.Systemtypeid;
                                    dcDto.DesignComponentFamilyId = dc.Designcomponentfamilyid;
                                    var dcf = _repositoryWrapper.DesignComponentFamily.FindByCondition(x =>
                                         x.Designcomponentfamilyid == dc.Designcomponentfamilyid, false, false).FirstOrDefault();
                                    dcDto.SubNetworkBoundaryIds = new List<long>();
                                    dcDto.SubNetworkBoundaryIds.Add(dcf.Subnetworkboundaryid);
                                    //dcDto.GdprRelevant = dc.GdprRelevant;
                                    #region  //Ticket 603 - #503 :  Analysis - Software Upgrade Utility
                                    dcDto.VisibleFlag =
                                    (dc.Subnetworkboundary.Alias != null &&
                                    constSubnetworkAllSupportedServie.Split(',').Contains(dc.Subnetworkboundary.Alias.Replace(" ", "").ToLower())
                                    && dc.Subnetworkboundary.Default == true) ? true
                                      : false;

                                    #endregion

                                    _repositoryWrapper.DesignComponentFamily.Detach();
                                    var newDC = await _designComponentManager.Add(dcDto);
                                }
                            }
                            #endregion
                        }
                    }
                    await transaction.CommitAsync();
                    return new ResultDto
                    {
                        Info = ResultMessages.EntryAddSuccess,
                        Warning = false
                    };
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ResultDto
                {
                    Info = ex.Message,
                    Warning = true
                };
            }


        }

        public async Task<ResultDto> LinkDAWithNetworkFunction(int designComponentFamilyId, int? networkFunctionId = null)
        {
            var networkFunctions = await Task.Run(() => _repositoryWrapper.DesignComponentFamily.FindByCondition(x => x.Designcomponentfamilyid == designComponentFamilyId)
                .Include(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorsoftwarebuildnetworkfunction).ThenInclude(x => x.Networkfunction)
                .FirstOrDefault()
                .Designcomponents.Select(x => x.Systemtype.Majorsoftwarebuilds).SelectMany(x => x.Majorsoftwarebuildnetworkfunction).Select(x => x.Networkfunction));

            var items = _networkFunctionManager.CastObjectToDto(networkFunctions.Select(p => NetworkFunctionMapper.Get(p)).AsQueryable());
            return new ResultDto
            {
                Data = items,
                Info = ResultMessages.EntryAddSuccess,
                Warning = false,
            };

        }
        #region  //  Ticket 642 - Add - Software Upgrade Utility to be enhanced to cover all required design components, System Types and Sub network boundary is created
        public async Task<ResultDto> GetMSWListBasedOnProductNameAndOEM(short oemId, short productNameId)
        {
            //Ticket 742 DC with hardware type  'unspecified" is not getting listed out in LCM current design component  - Removed 'unspecified'
            #region Existing SW reference to be made mandatory, with option including (No reference available).
            List<KeyValuePair<long, string>> systemTypeExistingReference = new List<KeyValuePair<long, string>>() {
                new KeyValuePair<long, string>( 0, "No Reference Available" ) };

            systemTypeExistingReference.AddRange(await Task.Run(() => _repositoryWrapper.SystemType
       .FindByCondition(x => x.Majorsoftwarebuilds.Orgeqpmanufacturerid == oemId && x.Majorsoftwarebuilds.Productnameid == productNameId)
       .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
       .Include(X => X.Majorsoftwarebuilds).ThenInclude(X => X.Productname)
       .ToDictionary(x => (long)x.Systemtypeid,
       x => $"{x.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} " +
       $"{(x.Majorsoftwarebuilds.Productname != null ? x.Majorsoftwarebuilds.Productname.Description : "")} " +
       $"{x.Majorsoftwarebuilds.Softwareversion}".ToString()
         ).Where(x => !x.Value.ToLower().Contains("unknown"))));

            #endregion
            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = systemTypeExistingReference
            };

        }
        public async Task<ResultDto> AddClonedMajorSW(MajorSoftwareBuildDtoCreate dto)
        {
            CloneMajorSoftwareBuildDto cloneMSWBuildDto = new CloneMajorSoftwareBuildDto();

            var majorSWEntity = MajorSoftwareBuildMapper.GetMajorSoftwareBuildMapper(_repositoryWrapper.
                SystemType.FindByCondition(x => x.Systemtypeid == dto.ExistSystemTypeId)
                .Include(x => x.Majorsoftwarebuilds).Select(x => x.Majorsoftwarebuilds).FirstOrDefault());

            cloneMSWBuildDto.MajorSoftwareBuildsId = (long)majorSWEntity.MajorSoftwareBuildsId;
            cloneMSWBuildDto.SoftwareVersion = dto.SoftwareVersion;
            cloneMSWBuildDto.EndOfMaintenance = dto.EndOfMaintenance;
            cloneMSWBuildDto.EomStatus = dto.EOMStatus;
            cloneMSWBuildDto.EndOfsupport = dto.EndOfsupport;
            cloneMSWBuildDto.MSWCreateDto = dto;
            cloneMSWBuildDto.DesignContactIds = dto.DesignContactIds;
            cloneMSWBuildDto.Isvmware = dto.IsPlatform;
            return await CloneMajorSoftwareBuild(cloneMSWBuildDto, dto.ExistSystemTypeId);

        }
        #endregion

        public async Task<ResultDto> AddOrUpdateDesignContact(IEnumerable<int> dto, long MsbId)
        {

            try
            {
                if (dto != null)
                {
                    long majorSoftwareBuildId = MsbId;
                    var recordExistBasedMajorSWBuildId = _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorsoftwarebuildsid == (long)majorSoftwareBuildId);

                    #region DesignContactRecord
                    foreach (var Item in dto)
                    {
                        var getExistRecords = recordExistBasedMajorSWBuildId.Where(x => x.Designcontactid == Item).ToList();

                        if (getExistRecords.Count == 0)
                        {
                            _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.Create(new Majorswbuildsdesigncontacts
                            {
                                Majorsoftwarebuildsid = majorSoftwareBuildId,
                                Designcontactid = Item,
                            });
                        }
                    }

                    await _repositoryWrapper.SaveAsync();

                    var designContactDeleteRecord = recordExistBasedMajorSWBuildId;
                    foreach (var item in designContactDeleteRecord)
                    {
                        if (!dto.Where(x => x == item.Designcontactid).Any())
                        {
                            _repositoryWrapper.MajorSwBuidlsDesignContactsRepository.DeleteDeep(item);
                        }
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    #endregion

                }

            }
            catch (Exception ex)
            {
                return new ResultDto { Info = ex.Message, Warning = true };
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        #region New Portal Changes

        private string ApplyFilterForSwProduct(string sqlQuery, long productId, List<short> opCoId, List<int> verticalId = null, long majorSwId = 0)
        {
            var sql = new StringBuilder(sqlQuery);

            sql.Append(@"
                                   WHERE msb.DELETED = 0
                                     AND msb.PRODUCTNAMEID = :ProductId AND LOWER(msb.Softwareversion) <> 'unknown'
                                ");

            if (majorSwId != 0)

            {
                sql.Append(@"
                                  
                                     AND msb.MAJORSOFTWAREBUILDSID = :majorSwId
                                ");
            }
            if (verticalId?.Any() == true)
            {
                sql.Append($@"
                            AND EXISTS
                            (
                                SELECT 1
                                FROM LCMENGINEERINGSUBDOMAINSPOC lsp
                                INNER JOIN ASPNETUSERVERTICALS auv
                                    ON auv.USERID = lsp.SUBDOMAINSPOCID
                                INNER JOIN ORGANISATION org
                                    ON org.ORGANISATIONID = auv.ORGANISATIONID
                                WHERE lsp.LCMENGINEERINGID = lcm.LCMENGINEERINGID
                                  AND auv.DELETED = 0
                                  AND org.VERTICALID IN ({string.Join(",", verticalId)})
                            )");
            }

            if (opCoId?.Any() == true)
            {
                sql.Append($@"
                            AND EXISTS
                            (
                                SELECT 1
                                FROM LCMENGINEERINGSUBDOMAINSPOC lsp
                                INNER JOIN ASPNETUSEROPCOS auopco
                                    ON auopco.USERID = lsp.SUBDOMAINSPOCID
                                WHERE lsp.LCMENGINEERINGID = lcm.LCMENGINEERINGID
                                  AND auopco.DELETED = 0
                                  AND auopco.ISRESTRICTEDOPCO = 0
                                  AND auopco.OPCOID IN ({string.Join(",", opCoId)})
                            )");
            }

            sql.Append(@"
                    GROUP BY
                        msb.MAJORSOFTWAREBUILDSID,
                        msb.SOFTWAREVERSION,
                        msb.ENDOFMAINTENANCE,
                        msb.ENDOFSUPPORT,
                        msb.EOMSTATUS,
                        msb.CREATIONDATE

                   ORDER BY
     CASE
        WHEN msb.ENDOFSUPPORT is null THEN 0
        ELSE 1
    END,
    CASE
        WHEN msb.ENDOFSUPPORT is not null
        THEN TO_DATE(msb.ENDOFSUPPORT, 'DD/MM/YYYY')
    END DESC");

            return sql.ToString();
        }
        public async Task<ResultDto> GetProductIdBasedLinkedRecords(ProductBasedSwQueryDto buildFilterDto, long userId)
        {
            try
            {
                var rtn = new QueryResultDto<ProductBasedSoftwareBuildDtoGrid>(
                    new GenerateRenderForGrid<ProductBasedSoftwareBuildDtoGrid>(_customColumnManager));

                var userDetails = await _authorizedRoleManager.GetUserRoleDetailsUsingDapper(userId, true);

                var verticalIds = userDetails.VerticalDetails ?? new List<int>();
                var opCoIds = userDetails.OpcoDetails ?? new List<short>();


                var query = ApplyFilterForSwProduct(
                    _majorSoftwareDapperQueryManager.GetSoftwareRecords(),
                    buildFilterDto.ProductId
                    , null, null
                    //opCoIds,
                    //verticalIds
                    );

                var softwareRecords = (await _commonDapperRepository.QueryAsync<ProductBasedSoftwareBuildDtoGrid>(
                    dapperDatabaseMode,
                    query,
                    new
                    {
                        buildFilterDto.ProductId,
                        userId,
                        //VerticalIds =  verticalIds.ToArray(),
                        //OpcoIds = opCoIds.ToArray()
                    })).ToList();

                rtn.TotalItems = softwareRecords.Count;

                var currentSoftware = softwareRecords.FirstOrDefault(x =>
                     x.MajorSoftwareBuildId == buildFilterDto.CurrentVersionSwId);

                var sortedRecords = new List<ProductBasedSoftwareBuildDtoGrid>();

                if (currentSoftware != null)
                    sortedRecords.Add(currentSoftware);

                sortedRecords.AddRange(
                    softwareRecords.Where(x => x.MajorSoftwareBuildId != buildFilterDto.CurrentVersionSwId)
                        .OrderByDescending(x => x.EndOfsupport)
                );

                rtn.Items = sortedRecords.ApplyPaginationList(buildFilterDto);
                var linkedHardware = new List<FilterValueDto>();

                if (!string.IsNullOrWhiteSpace(currentSoftware?.LinkedHardware))
                {
                    linkedHardware = currentSoftware.LinkedHardware != "-" ? currentSoftware.LinkedHardware
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim().Split('-'))
                        .Where(x => x.Length == 2)
                        .GroupBy(x => x[1])
                        .Select(g => new FilterValueDto { Text = $"{g.Key}", Value = $"{string.Join(",", g.Select(x => x[0]))}" })
                        .ToList() : null;
                }

                var linkedServices = new List<FilterValueDto>();

                if (currentSoftware.NetworkStatus == "Inactive") linkedServices = null;
                else
                {
                    var lcmQuery = ApplyFilterForSwProduct(
                                 _majorSoftwareDapperQueryManager.GetLcmIdBasedOnOpcoVertical(),
                                 buildFilterDto.ProductId,
                                 null,
                                 null,
                                 currentSoftware.MajorSoftwareBuildId);

                    var lcmOpcVerticalEntity = (await _commonDapperRepository.QueryAsync<ProductBasedSoftwareBuildDtoGrid>(
                        dapperDatabaseMode,
                        lcmQuery,
                        new
                        {
                            buildFilterDto.ProductId,
                            userId,
                            majorSwId = currentSoftware.MajorSoftwareBuildId
                        }))
                        .ToList();

                    var lcmMapping = lcmOpcVerticalEntity.FirstOrDefault()?.LcmOpcoMapping;


                    if (string.IsNullOrWhiteSpace(lcmMapping))
                        linkedServices = null;
                    else
                        linkedServices = lcmMapping
                                  .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(item =>
                                  {
                                      var lcmParts = item.Trim().Split('-');

                                      //   LcmId-Opco/OpcoId[#VerticalId]
                                      if (lcmParts.Length != 2)
                                          return null;

                                      var lcmId = lcmParts[0];

                                      // Split   vertical
                                      var opcoVerticalParts = lcmParts[1].Split('#');

                                      // Split OpcoName/OpcoId
                                      var opcoParts = opcoVerticalParts[0].Split('/');

                                      if (opcoParts.Length != 2)
                                          return null;

                                      if (!int.TryParse(opcoParts[1], out var opcoId))
                                          return null;

                                      int? verticalId = null;

                                      if (opcoVerticalParts.Length > 1)
                                      {
                                          if (!int.TryParse(opcoVerticalParts[1], out var parsedVerticalId))
                                              return null;

                                          verticalId = parsedVerticalId;
                                      }

                                      return new
                                      {
                                          LcmId = lcmId,
                                          OpcoName = opcoParts[0],
                                          OpcoId = opcoId,
                                          VerticalId = verticalId
                                      };
                                  })
                                  .Where(x =>
                                      x != null &&
                                      opCoIds.Contains((short)x.OpcoId) &&
                                          verticalIds.Contains((short)x.VerticalId.Value)
                                      )
                                  .GroupBy(x => x.LcmId)
                                  .Select(g => new FilterValueDto
                                  {
                                      Value = g.Key,
                                      Text = string.Join(",",
                                      g.Select(x => $"{x.OpcoName}")
                                           //g.Select(x => $"{x.OpcoName}/{x.OpcoId}")
                                           .Distinct()) // Remove duplicate OpcoName/OpcoId
                                  })
                                  .ToList();



                }

                var complaintValueTemp = false;
                if (currentSoftware != null)
                {
                    var isLcmLinked = currentSoftware?.NetworkStatus == "Live" ? true : false;

                    if (isLcmLinked)
                    {
                        var lasterversionCheck = sortedRecords.Where(x => x.LifeCycleStatus == "On Support" && x.MajorSoftwareBuildId == buildFilterDto.CurrentVersionSwId).FirstOrDefault() != null;
                        if (lasterversionCheck) complaintValueTemp = true;
                        else if (sortedRecords.Where(x => x.LifeCycleStatus == "On Support" && x.MajorSoftwareBuildId != buildFilterDto.CurrentVersionSwId).FirstOrDefault() != null)
                            complaintValueTemp = false;
                        else
                        {
                            var maxEosRecord = sortedRecords
       .Where(x => x.EndOfsupport != "NotAnnounced")
       .OrderByDescending(x => DateTime.Parse(x.EndOfsupport))
       .FirstOrDefault();

                            var maxEosDate = maxEosRecord?.EndOfsupport;
                            var majorSoftwareBuildId = maxEosRecord?.MajorSoftwareBuildId;

                            if (currentSoftware?.MajorSoftwareBuildId == majorSoftwareBuildId) complaintValueTemp = true;
                            else complaintValueTemp = false;
                        }

                    }
                    else
                    {
                        complaintValueTemp = false;
                    }

                }


                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.GetInfoSuccess,
                    Data = new
                    {
                        softwareGridRecords = rtn,
                        linkedHardware,
                        linkedServices,
                        CurrentSwComplaintValue = complaintValueTemp
                    }
                };
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    Warning = false,
                    Info = ResultMessages.AnIssueOccurredMsg
                };
            }
        }
        #endregion

        #region Global Search 

        public ExpressionStarter<Majorsoftwarebuilds> ApplyGlobalFilter(string search)
        {
            var predicate = PredicateBuilder.New<Majorsoftwarebuilds>(true);

            if (string.IsNullOrWhiteSpace(search))
                return predicate;

            search = search.Trim();

            var global = PredicateBuilder.New<Majorsoftwarebuilds>(true);

            // Text fields
            global.Or(x => x.Description != null && x.Description.Contains(search));
            global.Or(x => x.Softwareversion != null && x.Softwareversion.Contains(search));
            global.Or(x => x.Deliverymethod != null && x.Deliverymethod.Contains(search));
            global.Or(x => x.Vulnerabilitystatus != null && x.Vulnerabilitystatus.Contains(search));

            // Navigation properties
            global.Or(x => x.Productname != null &&
                           x.Productname.Description != null &&
                           x.Productname.Description.ToLower().Contains(search.ToLower()));

            global.Or(x => x.Operatingsystem != null &&
                           x.Operatingsystem.Operatingsystemname != null &&
                           x.Operatingsystem.Operatingsystemname.ToLower().Contains(search.ToLower()));

            global.Or(x => x.Criticalassettype != null &&
                           x.Criticalassettype.Description != null &&
                           x.Criticalassettype.Description.ToLower().Contains(search.ToLower()));

            global.Or(x => x.ModificationuserNavigation != null &&
                           x.ModificationuserNavigation.Email != null &&
                           x.ModificationuserNavigation.Email.ToLower().Contains(search.ToLower()));

            // Collections
            global.Or(x => x.Majorsoftwarebuildnetworkfunction.Any(n =>
                            n.Networkfunction != null &&
                            n.Networkfunction.Description != null &&
                            n.Networkfunction.Description.ToLower().Contains(search.ToLower())));

            global.Or(x => x.Majorswbuildsdesigncontacts.Any(d =>
                            d.Designcontact != null &&
                            d.Designcontact.Email != null &&
                            d.Designcontact.Email.ToLower().Contains(search.ToLower())));

            // Bundle Versions
            global.Or(x => x.SoftwarebuildcompatibilityMajorsoftwarebuild.Any(b =>
                            b.Bundlemajorsoftwarebuild != null &&
                            b.Bundlemajorsoftwarebuild.Softwareversion != null &&
                            b.Bundlemajorsoftwarebuild.Softwareversion.ToLower().Contains(search.ToLower())) );

            global.Or(x => x.Orgeqpmanufacturer != null &&
                x.Orgeqpmanufacturer.Originalequipmentmanufacturer != null &&
                x.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToLower().Contains(search.ToLower()))  ;

            // Numeric search
            if (long.TryParse(search, out var longValue))
            {
                global.Or(x => x.Majorsoftwarebuildsid == longValue);
                global.Or(x => x.Orgeqpmanufacturerid == longValue);
                global.Or(x => x.Productnameid == longValue);
                global.Or(x => x.Operatingsystemid == longValue);
                global.Or(x => x.Criticalassettypeid == longValue);
            }

            // Boolean search
            if (bool.TryParse(search, out var isPlatform))
            {
                global.Or(x => x.Isvmware == isPlatform);
            }
            string[] formats = { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" };

            if (DateTime.TryParseExact(
                search,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
            {
           
                var d = date.Date;

                global.Or(x => x.Endofmaintenance.HasValue &&
                               x.Endofmaintenance.Value.Date == d);

                global.Or(x => x.Endofsupport.HasValue &&
                               x.Endofsupport.Value.Date == d);

                global.Or(x => x.Lasttimebuynew.HasValue &&
                               x.Lasttimebuynew.Value.Date == d);

                global.Or(x => x.Lasttimebuyupgrades.HasValue &&
                               x.Lasttimebuyupgrades.Value.Date == d);

                global.Or(x => x.Lasttimebuyexpansions.HasValue &&
                               x.Lasttimebuyexpansions.Value.Date == d);

                global.Or(x => x.Generaavailabledate.HasValue &&
                               x.Generaavailabledate.Value.Date == d);

                global.Or(x => x.Modificationdate.Date == d);
            }

            predicate.And(global);

            return predicate;
        }
        #endregion
    }
}