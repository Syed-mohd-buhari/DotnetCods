using AutoMapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.NetworkElementAsIs;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.SystemType;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Models;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CAM.DataTransferObjects.LookUp.Asset;
using CAM.Entities.Mappers.Entity;
using OracleModels.DBModels;
using CAM.DataTransferObjects.Entita.DesignComponentFamily;
using CAM.BusinessManager.Entity.DesigComponent;
using Microsoft.AspNetCore.Http;
using CAM.BusinessManager.CommonUtilities;

namespace CAM.BusinessManager.Entity
{
    public partial class SystemTypeManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        private readonly PlannedActivityManager _plannedActivityManager;
        private readonly DesignComponentManager _designComponentManager;
        private readonly DesignComponentFamilyManager _designComponentFamilyManager;

        private readonly LcmEngineeringManager _lcmEngineeringManager;
        private readonly CommonManager _commonManager;
        // private readonly ConstantValueFilterManager _constantValueFilterManager;
        public SystemTypeManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager customColumnManager,
            PlannedActivityManager plannedActivityManager, LcmEngineeringManager lcmEngineeringManager, DesignComponentManager designComponentManager,
            DesignComponentFamilyManager designComponentFamilyManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _plannedActivityManager = plannedActivityManager;
            _lcmEngineeringManager = lcmEngineeringManager;
            _designComponentManager = designComponentManager;
            _designComponentFamilyManager = designComponentFamilyManager;
            _commonManager = commonManager;
            // _constantValueFilterManager = constantValueFilterManager;
        }


        public SystemTypeDtoGrid Get(long id)
        {
            var entity = _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == id).Include(x => x.VodafonenameNavigation).Single();
            return _mapper.Map<SystemTypeDtoGrid>(SystemTypeMapper.GetSystemTypeMapper(entity));
        }

        public async Task<ResultDto> Add(SystemTypeDtoCreate dto, bool? forced = false)
        {
            //Check chiave naturale
            var entityExists = await EntityExists(dto);


            //Se esiste gi? ritorna warning
            if (entityExists != null)
            {
                var relations = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Systemtypeid == entityExists.SystemTypeId, true).FirstOrDefault();
                if (relations == null && entityExists.Deleted == true)
                {
                    if (forced == true)
                    {
                        return await AddBase(dto);
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryAddExists,
                            Data = new { id = entityExists.SystemTypeId, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = entityExists.Deleted ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                        Data = entityExists.SystemTypeId
                    };
                }
            }

            return await AddBase(dto);
            // return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }

        public async Task<SystemType> EntityExists(SystemTypeDtoCreate dto)
        {
            var idMH = dto.MajorHardwareBuildId.SingleOrDefault(s => s.IsMain)?.MajorHardwareBuildId;

            var entityExists = await _repositoryWrapper.SystemType.FindByCondition(x =>
                    x.Systemtypenameoem == dto.SystemTypeNameOem
                    && x.Majorsoftwarebuildsid == dto.MajorSoftwareBuildsId
                    && x.Systemtypesmajorhardwarebuilds.SingleOrDefault(s => s.Ismain && !s.Deleted.Value)
                    .Majorhardwareid == idMH

                ).OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();
            return SystemTypeMapper.GetSystemTypeMapper(entityExists);
        }
        public async Task<SystemType> EntityExists(SystemTypeDtoUpdate dto)
        {
            var idMH = dto.MajorHardwareBuildId.SingleOrDefault(s => s.IsMain)?.MajorHardwareBuildId;

            var entityExists = await _repositoryWrapper.SystemType.FindByCondition(x =>
                    x.Systemtypenameoem == dto.SystemTypeNameOem
                    && x.Majorsoftwarebuildsid == dto.MajorSoftwareBuildsId
                    && x.Systemtypesmajorhardwarebuilds.SingleOrDefault(s => s.Ismain && !s.Deleted.Value).Majorhardwareid == idMH
                ).OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();
            return SystemTypeMapper.GetSystemTypeMapper(entityExists);
        }


        public async Task<ResultDto> AddBase(SystemTypeDtoCreate dto)
        {
            var VFname = _repositoryWrapper.VodafoneNameRepository.FindByCondition(x => x.Id == dto.vodafoneNameId).FirstOrDefault();
            dto.VodafoneName = VFname != null ? VFname.Description : string.Empty;


            var entityForced = _mapper.Map<SystemType>(dto);

            entityForced.AssetCategoryId = entityForced.AssetCategoryId == 0 ? null : entityForced.AssetCategoryId;
            entityForced.AssetClassId = entityForced.AssetClassId == 0 ? null : entityForced.AssetClassId;
            entityForced.AssetTypeId = entityForced.AssetTypeId == 0 ? null : entityForced.AssetTypeId;
            entityForced.VerticalResponsibleId = entityForced.VerticalResponsibleId == 0 ? null : entityForced.VerticalResponsibleId;
            entityForced.SubDomainResponsibleId = entityForced.SubDomainResponsibleId == 0 ? null : entityForced.SubDomainResponsibleId;
            var model = SystemTypeMapper.SetSystemTypeMapper(entityForced);
            _repositoryWrapper.SystemType.Create(model);
            _repositoryWrapper.Save();
            return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = model };
        }

        public async Task<ResultDto> Update(SystemTypeDtoUpdate dto, bool? forced = false)
        {
            var idMH = dto.MajorHardwareBuildId.SingleOrDefault(s => s.IsMain)?.MajorHardwareBuildId;

            //Check non esista un system type con la stessa chiave naturale ma id diverso
            var anotherEntityWithSameNaturalKeyExists = await EntityExists(dto);
            var originalEntityWithSameNaturalKey = await _repositoryWrapper.SystemType.FindByCondition(
                x => x.Systemtypenameoem == dto.SystemTypeNameOem
                && x.Majorsoftwarebuildsid == dto.MajorSoftwareBuildsId
                && x.Systemtypesmajorhardwarebuilds.SingleOrDefault(s => s.Ismain && !s.Deleted.Value).Majorhardwareid == idMH
                && x.Systemtypeid == dto.SystemTypeId).Include(x => x.VodafonenameNavigation).SingleOrDefaultAsync();

            if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
            {
                var relations = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Systemtypeid == anotherEntityWithSameNaturalKeyExists.SystemTypeId, true).FirstOrDefault();
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
                            Data = new { id = anotherEntityWithSameNaturalKeyExists.SystemTypeId, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    return new ResultDto
                    {
                        Warning = true,
                        Info = anotherEntityWithSameNaturalKeyExists.Deleted ? ResultMessages.EntryUpdateExistsDeleted : ResultMessages.EntryUpdateExists,
                        Data = anotherEntityWithSameNaturalKeyExists.SystemTypeId
                    };
                }
            }
            return await UpdateBase(dto, forced);
        }

        public async Task<ResultDto> UpdateBase(SystemTypeDtoUpdate dto, bool? forced)
        {
            var data = _mapper.Map<SystemType>(dto);
            var entity = SystemTypeMapper.SetSystemTypeMapper(data);

            if (forced == true)
            {
                entity.Deleted = false;
                entity.Deletiondate = null;
            }
            //Get delle relazioni con MHWB
            var majorHardwareBuildRelations = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Systemtypeid == dto.SystemTypeId).ToList();
            //Ricava quelli rimossi
            var majorHardwareBuildRelationsToDelete = majorHardwareBuildRelations.Where(x => dto.MajorHardwareBuildId.Select(m => m.MajorHardwareBuildId).All(s => s != x.Majorhardwareid)).ToList();
            //Rimuovi relazioni
            foreach (var toDelete in majorHardwareBuildRelationsToDelete)
                _repositoryWrapper.SystemTypesMajorHardwareBuild.DeleteDeep(toDelete);

            var relationalEntity = new List<Systemtypesmajorhardwarebuilds>();
            var remaningEntityToAdd = new List<Systemtypesmajorhardwarebuilds>();
            relationalEntity.AddRange(entity.Systemtypesmajorhardwarebuilds.Where(x => majorHardwareBuildRelations.Select(p => p.Majorhardwareid).All(s => s != x.Majorhardwareid)));
            remaningEntityToAdd.AddRange(entity.Systemtypesmajorhardwarebuilds.Where(x => majorHardwareBuildRelations.Select(p => p.Majorhardwareid).All(s => s != x.Majorhardwareid)));
            foreach (var ripristina in relationalEntity)
            {
                var existAndDeleted = _repositoryWrapper.SystemTypesMajorHardwareBuild
                    .FindByCondition(x => x.Deleted == true && x.Majorhardwareid == ripristina.Majorhardwareid, true, false)?
                    .SingleOrDefault();
                if (existAndDeleted != null)
                {
                    existAndDeleted.Deleted = false;
                    existAndDeleted.Deletiondate = null;
                    remaningEntityToAdd.Remove(ripristina);
                }
            }

            //var systemTypesSubDomainSpocsRelations =
            //    _repositoryWrapper.SystemTypesSubDomainSpoc.FindByCondition(x => x.Systemtypeid == dto.SystemTypeId).ToList();

            //foreach (var toDelete in systemTypesSubDomainSpocsRelations)
            //    _repositoryWrapper.SystemTypesSubDomainSpoc.DeleteDeep(toDelete);

            // dto.SubDomainSpocIds = dto.SubDomainSpocIds.Distinct().ToList();


            foreach (var ripristina in relationalEntity)
            {
                var existAndDeleted = _repositoryWrapper.SystemTypesMajorHardwareBuild
                    .FindByCondition(x => x.Deleted == true && x.Majorhardwareid == ripristina.Majorhardwareid, true, false)?
                    .SingleOrDefault();
                if (existAndDeleted != null)
                {
                    existAndDeleted.Deleted = false;
                    existAndDeleted.Deletiondate = null;
                    remaningEntityToAdd.Remove(ripristina);
                }
            }

            entity.Systemtypesmajorhardwarebuilds = remaningEntityToAdd;
            entity.Assetcategoryid = entity.Assetcategoryid == 0 ? null : entity.Assetcategoryid;
            entity.Assetclassid = entity.Assetclassid == 0 ? null : entity.Assetclassid;
            entity.Assettypeid = entity.Assettypeid == 0 ? null : entity.Assettypeid;
            //entity.Verticalresponsibleid = entity.Verticalresponsibleid == 0 ? null : entity.Verticalresponsibleid;
            entity.Vodafonename = dto.vodafoneNameId;
            //entity.Subdomainresponsibleid = entity.Subdomainresponsibleid == 0 ? null : entity.Subdomainresponsibleid;
            //foreach (var item in entity.Systemtypessubdomainspoc)
            //{
            //    _repositoryWrapper.SystemTypesSubDomainSpoc.Create(item);
            //}
            foreach (var item in entity.Systemtypesmajorhardwarebuilds)
            {
                _repositoryWrapper.SystemTypesMajorHardwareBuild.Create(item);
            }
            _repositoryWrapper.SystemType.Update(entity);
            _repositoryWrapper.Save();

            await VerifyAndChangeDesignComponentFamily(entity.Systemtypeid);
            return new ResultDto { Info = ResultMessages.EntryUpdateSuccess, Data = entity };
        }
        public async Task VerifyAndChangeDesignComponentFamily(long systemTypeId)
        {
            var systemType = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == systemTypeId)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Majorsoftwarebuilds)
                .SingleAsync();

            var designComponent = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Systemtypeid == systemType.Systemtypeid).Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ToListAsync();
            var mjhUId = systemType.Systemtypesmajorhardwarebuilds.First(x => x.Ismain).Majorhardwareid;
            var majorHardware = (await _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => mjhUId == x.Majorhardwareid).Include(x => x.Buildconstruction).FirstOrDefaultAsync());

            var majorSoftwareBuild = await _repositoryWrapper.MajorSoftwareBuild
                .FindByCondition(x => x.Majorsoftwarebuildsid == systemType.Majorsoftwarebuildsid)
                .FirstOrDefaultAsync();
            decimal? productNameId = majorSoftwareBuild.Productnameid;

            int? vfName = systemType.Vodafonename;
            var originalEquipmentManufacturerId = majorSoftwareBuild.Orgeqpmanufacturerid;
            foreach (var dc in designComponent)
            {
                var test = await _designComponentManager.GetDesignComponentFamilyUniqueQuery(majorHardware.Orgeqpmanufacturerid,
                   productNameId, vfName, originalEquipmentManufacturerId, dc.Designcomponentfamily.Subnetworkboundaryid, majorHardware.Platformid);
                if (test == null)
                {
                    var id = systemType.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware
                            .Platform.Platformid;
                    var platform = (await _repositoryWrapper.Platform.FindByCondition(x =>
                        x.Platformid == id).SingleOrDefaultAsync())?.Platform;

                    var hardwareSolution = majorHardware.Hardwaresolution;
                    var buildConstructionRule = majorHardware.Buildconstruction.Rule;
                    var productName = systemType.Majorsoftwarebuilds.Productname != null ? systemType.Majorsoftwarebuilds.Productname.Description : "";
                    var dcf = await _designComponentFamilyManager.Add(new DesignComponentFamilyDtoCreate()
                    {
                        SubNetworkBoundaryId = dc.Designcomponentfamily.Subnetworkboundaryid,
                        //GdprRelevant = null,
                        MajorSoftwareOemId = majorSoftwareBuild.Orgeqpmanufacturerid,
                        ProductName = productName,
                        MajorHardwareOemId = majorHardware.Orgeqpmanufacturerid,
                        SystemTypeIdentityName = await Utils.STIM(_repositoryWrapper, majorHardware.Orgeqpmanufacturerid,
                        majorSoftwareBuild.Orgeqpmanufacturerid, productName, platform, hardwareSolution, buildConstructionRule)
                    });
                    dc.Designcomponentfamilyid = dcf.Data;
                    _repositoryWrapper.DesignComponent.Update(dc);
                    await _repositoryWrapper.SaveAsync();
                }
            }
        }

        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == id).SingleAsync();

            //Rimozione di tutte le relazioni
            foreach (var toDelete in entity.Systemtypesmajorhardwarebuilds)
                _repositoryWrapper.SystemTypesMajorHardwareBuild.Delete(toDelete);

            _repositoryWrapper.SystemType.Delete(entity);
            await _repositoryWrapper.SaveAsync();
            return new ResultDto { Info = ResultMessages.EntryDeleteSuccess };
        }

        public async Task<ResultDto> DeleteDeep(long id)
        {
            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            //var DesignComponent = _repositoryWrapper.DesignComponent
            //    .FindByConditionWithDelete(x => x.Systemtypeid == id && x.Deleted == true).FirstOrDefault();

            //var NetworkElementAsIs = _repositoryWrapper.NetworkElementAsIs
            //                                    .FindByConditionWithDelete(x => x.Systemtypeid == id && x.Deleted == true).FirstOrDefault();

            var entity = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == id)
                .Include(x => x.Systemtypessubdomainspoc)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .SingleAsync();

            //Da verificare la modalità di ricerca delle relazioni cross e la conseguente cancellazione del record
            //Se non viene controllata la cross con majorhardware allora bisogna cancellare le tabelle in sequenza e commentare le query in GetRelatedRecord


            if (entity.Systemtypessubdomainspoc != null && entity.Systemtypessubdomainspoc.Count > 0)
            {
                var subDomainSpocList = entity.Systemtypessubdomainspoc.ToList();
                foreach (var toDelete in subDomainSpocList)
                    _repositoryWrapper.SystemTypesSubDomainSpoc.Delete(toDelete);
                //_repositoryWrapper.SystemTypesSubDomainSpoc.DeleteDeep(toDelete);
            }
            if (entity.Systemtypesmajorhardwarebuilds != null && entity.Systemtypesmajorhardwarebuilds.Count > 0)
            {
                var systemTypesMajorHardwareBuildsList = entity.Systemtypesmajorhardwarebuilds.ToList();

                foreach (var toDelete in systemTypesMajorHardwareBuildsList)
                    _repositoryWrapper.SystemTypesMajorHardwareBuild.Delete(toDelete);
                //_repositoryWrapper.SystemTypesMajorHardwareBuild.DeleteDeep(toDelete);
            }

            _repositoryWrapper.SystemType.Delete(entity);
            await _repositoryWrapper.SaveAsync();


            #region  -- *** if delete DC before delete the SystemTypes - face The association between entity types 'Systemtypes' and 'Designcomponents' has been severed, but the relationship is either marked as required or is implicitly required because the foreign key is not nullable. If the dependent/child entity should be deleted when a required relationship is severed, configure the relationship
            var TransientDesignComponenet = _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Systemtypeid == id && x.Visibleflag == false && x.Deleted == false).ToList();

            if (TransientDesignComponenet != null && TransientDesignComponenet.Count > 0)
            {

                foreach (var toDelete in TransientDesignComponenet)
                    _repositoryWrapper.DesignComponent.Delete(toDelete);

                await _repositoryWrapper.SaveAsync();

            }
            #endregion
            //*********************************************************************************************


            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Systemtypeid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            List<ResultMessageDto> rm = new List<ResultMessageDto>();

            //Da verificare la modalità di ricerca delle relazioni cross e la conseguente cancellazione del record
            //Se non viene controllata la cross con majorhardware allora bisogna cancellare le tabelle in sequenza e commentare le seguenti righe
            //Altrimenti bisogna commentare le relative righe in DeleteDeep e decommentare quelle seguenti
            //var SystemTypesMajorHardwareBuild = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.SystemTypeId == id).Select(x => x.SystemType.toSystemTypeName(_repositoryWrapper)).ToArray();
            //if (SystemTypesMajorHardwareBuild.Length > 0)
            //    rm.Add(new ResultMessageDto() { Table = "System Types Major Hardware Build", Values = SystemTypesMajorHardwareBuild });

            //var SystemTypesSubDomainSpoc = _repositoryWrapper.SystemTypesSubDomainSpoc.FindByCondition(x => x.SystemTypeId == id).Select(x => x.SubDomainSpoc.SubDomainSpocDescription).ToArray();
            //if (SystemTypesSubDomainSpoc.Length > 0)
            //    rm.Add(new ResultMessageDto() { Table = "System Types SubDomainSpoc", Values = SystemTypesSubDomainSpoc });
            //*********************************************************************************************

            var DesignComponent = _repositoryWrapper.DesignComponent
                .FindByCondition(x => x.Systemtypeid == id && x.Visibleflag == true)
                .Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(x => x.Subnetwrokboundarysystemfunction).ThenInclude(x => x.Systemfunction)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                .Select(x => x.toDesignComponentNameLcm(_repositoryWrapper)).ToArray();

            var NetworkElementAsIs = _repositoryWrapper.NetworkElementAsIs
                                                .FindByCondition(x => x.Systemtypeid == id)
                                                .Select(x => NetworkElementAsIsMapper.Get(x).toDescription())
                                                .ToArray();

            if (DesignComponent.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Design Component", Values = DesignComponent });
            if (NetworkElementAsIs.Length > 0)
                rm.Add(new ResultMessageDto() { Table = "Network Element As Planned", Values = NetworkElementAsIs });

            var entity = await _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == id)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Majorsoftwarebuilds.Orgeqpmanufacturer)
                    .Include(x => x.VodafonenameNavigation).SingleAsync();
            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries

            List<string> removeDuplicatesAndLinkedTableCheck = new List<string>()
            {
                "Designcomponents","Networkelementsasplanned","Systemtypesmajorhardwarebuilds","Systemtypessubdomainspoc","Systemverificationproblems"
            };

            var referenceTableRecord = _commonManager.GetForeignKeyRefernceTable("Systemtypes", id, removeDuplicatesAndLinkedTableCheck);

            if (referenceTableRecord.Result != null && referenceTableRecord.Result.Count() > 0)
                rm.Add(new ResultMessageDto() { Table = _commonManager.popupTabName, Values = referenceTableRecord.Result.ToArray() });


            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto
                    {
                        EntityName = "System Type",
                        RecordName = entity.SystemTypeName(_repositoryWrapper),
                        DataRelatedList = rm
                    }
                };
            }
            else
            {
                return new ResultDto
                {
                    Warning = false,
                };
            }
        }
        public IDictionary<int, string> GetMajorSoftwareVodafoneName(long majorSoftwareId)
        {
            var productNameId = _repositoryWrapper.MajorSoftwareBuild
                .FindByCondition(x => x.Majorsoftwarebuildsid == majorSoftwareId).Select(x => x.Productnameid).FirstOrDefault();
            var vodafoneName = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == productNameId)
                .Select(x => x.Vodafonenames);
            var vodafoneNameDic = vodafoneName.FirstOrDefault() != null ? vodafoneName.ToDictionary(x => x.Id, x => x.Description) : null;
            return vodafoneNameDic;

        }

        public IDictionary<int, string> GetVfNameOfSwAppType(long swAppTypeId)
        {
            var vodafoneName = _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == swAppTypeId)
                .Select(x => x.Vodafonenames);
            var vodafoneNameDic = vodafoneName.FirstOrDefault() != null ? vodafoneName.ToDictionary(x => x.Id, x => x.Description) : null;
            return vodafoneNameDic;

        }
        public QueryResultDto<SystemTypeDtoGrid>
            FindWithCondition(SystemTypeQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);
            var systemTypeResult = GetSystemTypeDtoGrids(buildFilterDto, ref predicateResult);

            var rtn = new QueryResultDto<SystemTypeDtoGrid>(new GenerateRenderForGrid<SystemTypeDtoGrid>(_customColumnManager))
            {
                TotalItems = systemTypeResult.TotalCount
            };

            rtn.Items = systemTypeResult.Data.ToArray();
            return rtn;
        }

        private( IEnumerable<SystemTypeDtoGrid> Data ,int TotalCount)GetSystemTypeDtoGrids(SystemTypeQueryDto buildFilterDto, ref ExpressionStarter<Systemtypes> predicateResult)
        {
            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }

            if (buildFilterDto.Orphan == true)
            {
                predicateResult = predicateResult.And(x => !x.Designcomponents.Any());
                predicateResult = predicateResult.And(x => !x.Networkelementsasis.Any());
            }

            // Remove Unknown Softwareversion in System Type Screen
            predicateResult = predicateResult.And(x => x.Majorsoftwarebuilds.Softwareversion.ToLower() != ConstantValueFilter.Unknown);


            var query = GetQuery(predicateResult, buildFilterDto.Deleted ?? false).AsQueryable();

            //var result = query.AsEnumerable().Select(p => SystemTypeMapper.GetSystemTypeMapper(p, true)).AsEnumerable().AsQueryable().ApplyOrdering(buildFilterDto, GetColumnsMap())
            //    .ApplyPaging(buildFilterDto);

            if (buildFilterDto.VerticalResponsible != null && buildFilterDto.VerticalResponsible.Count> 0 )
            {
                List<long> swVerticalST = new List<long>();
                List<long> hwVerticalST = new List<long>();
                swVerticalST = query.Where(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any
                                                     (x => x.Designcontact.AspnetuserverticalsUser != null && x.Designcontact.AspnetuserverticalsUser.Any
                                                     (x => buildFilterDto.VerticalResponsible.Contains(x.Organisation.Vertical.Verticalresponsibleid))))
                                      .Select(x=>x.Systemtypeid).ToList();

                hwVerticalST = query.Where(x=>x.Systemtypesmajorhardwarebuilds.Where(n=> 
                                        n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction))) 
                                        .Any(y => y.Majorhardware.Majorhwbuildsdesigncontacts!=null && y.Majorhardware.Majorhwbuildsdesigncontacts
                                        .Any(e => e.Designcontact.AspnetuserverticalsUser!=null && e.Designcontact.AspnetuserverticalsUser
                                        .Any(x => buildFilterDto.VerticalResponsible.Contains(x.Organisation.Vertical.Verticalresponsibleid)))))
                                      .Select(x => x.Systemtypeid).ToList() ;

                if(swVerticalST != null && swVerticalST.Count()>0 && hwVerticalST != null && hwVerticalST.Count() > 0)
                {
                    var tempList = swVerticalST.Union(swVerticalST);
                    query = query.Where(x => tempList.Contains(x.Systemtypeid));
                }
                else if(swVerticalST != null && swVerticalST.Count() > 0)
                    query = query.Where(x => swVerticalST.Contains(x.Systemtypeid));
                else if (hwVerticalST != null && hwVerticalST.Count() > 0)
                    query = query.Where(x => hwVerticalST.Contains(x.Systemtypeid)); 
            }

            var totalCount = query!=null && query.Count() >0?query.Count():0;
            var result = query.AsEnumerable().Select(p => SystemTypeMapper.GetSystemTypeGridMapper(p, true)).AsEnumerable().AsQueryable().ApplyOrdering(buildFilterDto, GetColumnsMap())
               .ApplyPaging(buildFilterDto);


            var data = result;//.ToList();

            if (buildFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.SystemTypeId == buildFilterDto.PrincipalId);
                if (!exist)
                {
                    var addedResource = _repositoryWrapper.SystemType.FindAll(true)
                        .Include(x => x.VodafonenameNavigation)
                        .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                        .Include(x => x.Assetcategory)
                        .Include(x => x.Assetclass)
                        .Include(x => x.Designcomponents)
                        .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                        .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                     .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                     .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                        .Single(x => x.Systemtypeid == buildFilterDto.PrincipalId);
                    data.ToList().Add(SystemTypeMapper.GetSystemTypeGridMapper(addedResource));
                }
            }

            var systemTypeResult = _mapper.Map<IEnumerable<SystemTypeDtoGrid>>(data);
            return (systemTypeResult, totalCount);
        }

        private static ExpressionStarter<Systemtypes> ApplyFilter(SystemTypeQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Systemtypes>();

            var predicateInner = PredicateBuilder.New<Systemtypes>();

            if (buildFilterDto.SystemTypeId != null && buildFilterDto.SystemTypeId.Any())
            {
                foreach (var item in buildFilterDto.SystemTypeId)
                    predicateInner.Or(x => x.Systemtypeid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MajorSoftwareBuildId != null && buildFilterDto.MajorSoftwareBuildId.Any())
            {
                foreach (var item in buildFilterDto.MajorSoftwareBuildId)
                    predicateInner.Or(x => x.Majorsoftwarebuildsid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.vodafoneName != null && buildFilterDto.vodafoneName.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();

                foreach (var item in buildFilterDto.vodafoneName)
                    predicateInner.Or(x => x.Vodafonename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MajorHardwareBuildId != null && buildFilterDto.MajorHardwareBuildId.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.MajorHardwareBuildId)
                    predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds
                    .Any(s => s.Majorhardwareid == item));
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SystemTypeNameOem != null && buildFilterDto.SystemTypeNameOem.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.SystemTypeNameOem)
                    predicateInner.Or(x => x.Systemtypenameoem == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SystemTypeName3Gpp != null && buildFilterDto.SystemTypeName3Gpp.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.SystemTypeName3Gpp)
                    predicateInner.Or(x => x.Systemtypename3gpp == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ConstraintScaling != null)
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                if (buildFilterDto.ConstraintScaling.StartDate != null)
                    predicateInner.And(x => x.Constraintscaling >= buildFilterDto.ConstraintScaling.StartDate);
                if (buildFilterDto.ConstraintScaling.EndDate != null)
                    predicateInner.And(x => x.Constraintscaling <= buildFilterDto.ConstraintScaling.EndDate);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.ConstraintLcm != null && buildFilterDto.ConstraintLcm.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.ConstraintLcm)
                    predicateInner.Or(x => x.Constraintlcm == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ProductImportance != null && buildFilterDto.ProductImportance.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.ProductImportance)
                    predicateInner.Or(x => x.Productimportanceid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.MajorSoftwareBuild != null && buildFilterDto.MajorSoftwareBuild.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.MajorSoftwareBuild)
                    predicateInner.Or(x => x.Majorsoftwarebuildsid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MajorHardwareBuild != null && buildFilterDto.MajorHardwareBuild.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.MajorHardwareBuild)
                    predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds
                    .Any(s => s.Majorhardware.Platform.Platform + "; " + s.Majorhardware.Hardwaretype == item));
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.AssetCategory != null && buildFilterDto.AssetCategory.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.AssetCategory)
                    predicateInner.Or(x => x.Assetcategoryid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.AssetClass != null && buildFilterDto.AssetClass.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.AssetClass)
                    predicateInner.Or(x => x.Assetclassid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.AssetType != null && buildFilterDto.AssetType.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.AssetType)
                    predicateInner.Or(x => x.VodafonenameNavigation != null && x.VodafonenameNavigation.Id == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HardwareOem != null && buildFilterDto.HardwareOem.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.HardwareOem)
                    predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds
                    .Any(s => s.Majorhardware.Orgeqpmanufacturerid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareOem != null && buildFilterDto.SoftwareOem.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.SoftwareOem)
                    predicateInner.Or(x => x.Majorsoftwarebuilds.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.EndOfMaintenance != null)
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                if (buildFilterDto.EndOfMaintenance.StartDate != null)
                    predicateInner.And(x => (x.Majorsoftwarebuilds.Endofmaintenance >= buildFilterDto.EndOfMaintenance.StartDate) && (x.Systemtypesmajorhardwarebuilds.FirstOrDefault().Majorhardware.Endofmaintenance >= buildFilterDto.EndOfMaintenance.StartDate));
                if (buildFilterDto.EndOfMaintenance.EndDate != null)
                    predicateInner.And(x => (x.Majorsoftwarebuilds.Endofmaintenance <= buildFilterDto.EndOfMaintenance.EndDate) && (x.Systemtypesmajorhardwarebuilds.FirstOrDefault().Majorhardware.Endofmaintenance >= buildFilterDto.EndOfMaintenance.StartDate));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftWareDesignContactEmail != null && buildFilterDto.SoftWareDesignContactEmail.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.SoftWareDesignContactEmail)
                    if(item == "yes")
                    {
                        predicateInner.Or(x => !x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any(x => x.Designcontactid.ToString() == item));
                    }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftWareVertical != null && buildFilterDto.SoftWareVertical.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.SoftWareVertical)
                    if (item == "yes")
                    {
                        predicateInner.Or(x =>!x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any(y => y.Designcontact.AspnetuserverticalsUser.Any(r =>
                  r.Organisation.Vertical.Verticalresponsibleid.ToString() == item)));
                    }

                predicateResult.And(predicateInner);
            }
            

            if (buildFilterDto.SoftWareSubdomain != null && buildFilterDto.SoftWareSubdomain.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();

                foreach (var item in buildFilterDto.SoftWareSubdomain)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && !x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Majorsoftwarebuilds
                    .Majorswbuildsdesigncontacts.Any(y => y.Designcontact.Subdomainresponsibleid.ToString() == item));
                    }

                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardWareDesignContactEmail != null && buildFilterDto.HardWareDesignContactEmail.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.HardWareDesignContactEmail)
                    if(item == "yes")
                    {
                        predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds.Where(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction)))
                        .Any(x => x.Majorhardware.Majorhwbuildsdesigncontacts != null && !x.Majorhardware.Majorhwbuildsdesigncontacts.Any()));
                    }
                    else
                    {
                        predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds.Where(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction))).Any(t => t.Majorhardware
                   .Majorhwbuildsdesigncontacts.Any(t => t.Designcontactid.ToString() == item)));
                    }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardWareVertical != null && buildFilterDto.HardWareVertical.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();
                foreach (var item in buildFilterDto.HardWareVertical)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds.Where(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction)))
                        .Any(x => x.Majorhardware.Majorhwbuildsdesigncontacts != null && !x.Majorhardware.Majorhwbuildsdesigncontacts.Any()));
                    }
                    else
                    {
                        predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds.Where(n => n.Ismain && (!ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction))).Any(t => t.Majorhardware
                  .Majorhwbuildsdesigncontacts.Any(y => y.Designcontact.AspnetuserverticalsUser.Any(r =>
                   r.Organisation.Vertical.Verticalresponsibleid.ToString() == item))));
                    }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HardWareSubdomain != null && buildFilterDto.HardWareSubdomain.Any())
            {
                predicateInner = PredicateBuilder.New<Systemtypes>();

                foreach (var item in buildFilterDto.HardWareSubdomain)
                    if (item == "yes")
                    {
                        predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds.Where(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction)))
                        .Any(x => x.Majorhardware.Majorhwbuildsdesigncontacts != null && !x.Majorhardware.Majorhwbuildsdesigncontacts.Any()));
                    }
                    else
                    {
                        predicateInner.Or(x => x.Systemtypesmajorhardwarebuilds.Where(n => n.Ismain && !(ConstantValueFilter.virtualisedHWTypeArray.Contains(n.Majorhardware.Buildconstruction.Buildconstruction))).Any(t => t.Majorhardware
                  .Majorhwbuildsdesigncontacts.Any(y => y.Designcontact.Subdomainresponsibleid.ToString() == item)));
                    }

                predicateResult.And(predicateInner);
            }

            return predicateResult;
        }

        private Dictionary<string, Expression<Func<SystemType, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<SystemType, object>>[]>
            {

                ["systemTypeId"] = new Expression<Func<SystemType, object>>[] { p => p.SystemTypeId },
                ["majorHardwareBuildId"] = new Expression<Func<SystemType, object>>[] { p => p.SystemTypesMajorHardwareBuilds.FirstOrDefault() != null ? p.SystemTypesMajorHardwareBuilds.FirstOrDefault().MajorHardwareId : default },
                ["systemTypeName3Gpp"] = new Expression<Func<SystemType, object>>[] { p => p.SystemTypeName3Gpp },
                ["systemTypeNameOem"] = new Expression<Func<SystemType, object>>[] { p => p.SystemTypeNameOem },
                ["constraintScaling"] = new Expression<Func<SystemType, object>>[] { p => p.ConstraintScaling },
                ["constraintLcm"] = new Expression<Func<SystemType, object>>[] { p => SystemTypeMapper.SetSystemTypeMapper(p).GetStatusName(_repositoryWrapper) },
                ["endOfMaintenance"] = new Expression<Func<SystemType, object>>[] { p => p.EndOfMaintenanceValue },
                ["productImportance"] = new Expression<Func<SystemType, object>>[] { p => p.ProductImportanceId },
                //["subDomainSpoc"] = new Expression<Func<SystemType, object>>[] { p => p.SystemTypesSubDomainSpocs.FirstOrDefault() != null ? p.SystemTypesSubDomainSpocs.FirstOrDefault().SubDomainSpoc.SubDomainSpocDescription : default },
                ["assetCategory"] = new Expression<Func<SystemType, object>>[] { p => p.AssetCategory.AssetCategoryDescription },
                ["assetClass"] = new Expression<Func<SystemType, object>>[] { p => p.toAssetClassDescription(_repositoryWrapper) },
                ["assetType"] = new Expression<Func<SystemType, object>>[] { p => p.AssetCategory != null && p.AssetTypeIdNavigation != null && p.AssetCategory.TakeFromAssetTypeTable ? p.AssetTypeIdNavigation.AssetTypeDescription : (p.VodafoneName != null ? p.VodafoneName.Description : default) },
                ["majorSoftwareBuild"] = new Expression<Func<SystemType, object>>[] { p => p.MajorSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription },
                ["majorSoftwareBuildId"] = new Expression<Func<SystemType, object>>[] { p => p.MajorSoftwareBuildsId },
                ["subDomainResponsible"] = new Expression<Func<SystemType, object>>[] { p => p.SubDomainResponsible.SubDomainResponsibleDescription },
                ["systemSolution"] = new Expression<Func<SystemType, object>>[]
                {
                    p => p.SystemTypeNameOem,
                    p=>p.SystemTypesMajorHardwareBuilds.FirstOrDefault(x=>x.IsMain).MajorHardware.Platform,
                    p => p.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain).MajorHardware.HardwareType
                },
                ["verticalResponsible"] = new Expression<Func<SystemType, object>>[] { p => p.VerticalResponsible != null ? p.VerticalResponsible.VerticalResponsibleDescription : default },
                ["lastModifiedValue"] = new Expression<Func<SystemType, object>>[] { p => p.ModificationDate },
                ["majorHardwareBuild"] = new Expression<Func<SystemType, object>>[] { p => p.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain) != null ? p.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain).MajorHardware.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription : default },
                ["hardwareOem"] = new Expression<Func<SystemType, object>>[] { p => p.SystemTypesMajorHardwareBuilds.FirstOrDefault(x => x.IsMain).MajorHardware.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription },
                ["vodafoneName"] = new Expression<Func<SystemType, object>>[] { p => p.VodafoneName != null ? p.VodafoneName.Description : default },
                ["softwareOem"] = new Expression<Func<SystemType, object>>[] { p => p.MajorSoftwareBuilds.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription, },
                ["lastModifiedBy"] = new Expression<Func<SystemType, object>>[] { p => p.ModificationUserEntity.Email },

                ["softWareDesignContactEmail"] = new Expression<Func<SystemType, object>>[] { p => p.SoftWareDesignContactEmail },
                ["hardWareDesignContactEmail"] = new Expression<Func<SystemType, object>>[] { p => p.HardWareDesignContactEmail },
                ["hardWareSubdomain"] = new Expression<Func<SystemType, object>>[] { p => p.HardWareSubdomain },
                ["softWareSubdomain"] = new Expression<Func<SystemType, object>>[] { p => p.SoftWareSubdomain },
                ["hardWareVertical"] = new Expression<Func<SystemType, object>>[] { p => p.HardWareVertical },
                ["softWareVertical"] = new Expression<Func<SystemType, object>>[] { p => p.SoftWareVertical },

            };
        }
        public Systemtypes SetSystemTypeValue(Systemtypes systemType)
        {
            systemType.Constraintlcm = systemType.GetStatusName(_repositoryWrapper);
            systemType.Constraintscaling = systemType.GetMinorDate(_repositoryWrapper);
            var result = systemType.GetMinorDateEOM(_repositoryWrapper);
            systemType.Endofmaintenance = (result.ToLower() == ConstantValueFilter.notAnnouncedEOM || result.ToLower() == ConstantValueFilter.notSpecifiedEOM) ? null : (DateTime?)Convert.ToDateTime(result);


            return systemType;
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter,
            SystemTypeQueryDto systemTypeFilterDto,bool isAdmin,List<int> verticalList)
        {

            var predicateResult = ApplyFilter(systemTypeFilterDto);
            // Remove Unknown Softwareversion in System Type Screen
            predicateResult = predicateResult.And(x => x.Majorsoftwarebuilds.Softwareversion.ToLower() != ConstantValueFilter.Unknown);

            var query = GetQuery(predicateResult, false);


            var rtn = propertyName switch
            {

                "systemTypeName3Gpp" =>
                string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Systemtypename3gpp, Value = p.Systemtypename3gpp }).Distinct().ToList()
                    : query.Where(x => x.Systemtypename3gpp.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Systemtypename3gpp, Value = p.Systemtypename3gpp }).Distinct().ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationuserNavigation.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList(),
                "systemTypeNameOem" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = p.Systemtypenameoem, Value = p.Systemtypenameoem }).Distinct()
                        .ToList()
                    : query.Where(x => x.Systemtypenameoem.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Systemtypenameoem, Value = p.Systemtypenameoem }).Distinct()
                        .ToList(),

                "vodafoneName" =>
                string.IsNullOrEmpty(propertyFilter)
                   ? query
                       .Select(p => new FilterValueDto { Text = p.VodafonenameNavigation.Description, Value = p.VodafonenameNavigation.Id.ToString() }).Distinct().ToList()
                   : query.Where(x => x.VodafonenameNavigation.Description.Contains(propertyFilter))
                       .Select(p => new FilterValueDto { Text = p.VodafonenameNavigation.Description, Value = p.VodafonenameNavigation.Id.ToString() }).Distinct().ToList(),

                "constraintLcm" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Constraintlcm, Value = p.Constraintlcm }).Distinct().ToList()
                    : query.Where(x => x.Constraintlcm.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Constraintlcm, Value = p.Constraintlcm })
                        .Distinct()
                        .ToList(),


                "productImportance" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Include(x => x.Productimportance).Select(p => new FilterValueDto
                    { Text = p.Productimportance.Productimportance, Value = p.Productimportanceid.ToString() }).Distinct().ToList()
                    : query.Where(x => x.Productimportanceid.ToString().Contains(propertyFilter))
                    .Include(x => x.Productimportance).Select(p =>
                              new FilterValueDto { Text = p.Productimportance.Productimportance, Value = p.Productimportanceid.ToString() }).Distinct()
                        .ToList(),


                "assetCategory" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Assetcategory.Assetcategory,
                        Value = p.Assetcategory.Assetcategoryid.ToString()
                    }).Distinct().ToList()
                    : query.Where(x =>
                            x.Assetcategory.Assetcategory.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Assetcategory.Assetcategory,
                                    Value = p.Assetcategory.Assetcategoryid.ToString()
                                }).Distinct().ToList(),

                "assetClass" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto
                    {
                        Text = SystemTypeMapper.GetSystemTypeMapper(p).toAssetClassDescription(_repositoryWrapper),
                        Value = p.Assetclassid.ToString()
                    }).Distinct().ToList()
                    : query.ToList().Where(x =>
                            SystemTypeMapper.GetSystemTypeMapper(x).toAssetClassDescription(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper())).Select(p => new FilterValueDto
                            {
                                Text = SystemTypeMapper.GetSystemTypeMapper(p).toAssetClassDescription(_repositoryWrapper),
                                Value = p.Assetclassid.ToString()
                            }).Distinct().ToList(),


                "assetType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.VodafonenameNavigation != null).Select(p => new FilterValueDto
                    {
                        Text = p.VodafonenameNavigation.Description,
                        Value = p.VodafonenameNavigation.Id.ToString()
                    }).Distinct().ToList()
                    : query.Where(x => x.VodafonenameNavigation != null &&
                            x.VodafonenameNavigation.Description.Contains(propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.VodafonenameNavigation.Description,
                                Value = p.VodafonenameNavigation.Id.ToString()
                            }).Distinct().ToList(),

                "systemSolution" => string.IsNullOrEmpty(propertyFilter)
                    ? query
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                        .ThenInclude(x => x.Platform)
                        .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .ToList()
                        .Select(p => new FilterValueDto
                        {
                            Text = p.toSystemTypeName(_repositoryWrapper),
                            Value = p.Systemtypeid.ToString()
                        }).Distinct().ToList()
                    : query
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Majorsoftwarebuilds)
                        .ThenInclude(x => x.Orgeqpmanufacturer).ToList()
                        .Where(x => x.toSystemTypeName(_repositoryWrapper).ToUpper().Contains(propertyFilter.ToUpper()))
                        .Select(p => new FilterValueDto
                        {
                            Text = p.toSystemTypeName(_repositoryWrapper),
                            Value = p.Systemtypeid.ToString()
                        }).Distinct().ToList(),

                "majorSoftwareBuild" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(x => x.Majorsoftwarebuilds.Productname != null).Select(p => new FilterValueDto
                    {
                        Text = $"{p.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} - {p.Majorsoftwarebuilds.Productname.Description} - {p.Majorsoftwarebuilds.Softwareversion}",
                        Value = p.Majorsoftwarebuildsid.ToString()
                    }).Distinct().ToList()
                    : query.Where(x => x.Majorsoftwarebuilds.Productname != null &&
                         (x.Majorsoftwarebuilds.Productname.Description + ";" + x.Majorsoftwarebuilds.Softwareversion).Contains(propertyFilter)
                                                     ).Select(p => new FilterValueDto
                                                     {
                                                         Text = $"{p.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer} - {p.Majorsoftwarebuilds.Productname.Description} - {p.Majorsoftwarebuilds.Softwareversion}",
                                                         Value = p.Majorsoftwarebuildsid.ToString()
                                                     }).Distinct().ToList(),
                "majorSoftwareBuildId" => string.IsNullOrEmpty(propertyFilter)
                                ? query.Select(p => new FilterValueDto
                                {
                                    Text = p.Majorsoftwarebuilds.Majorsoftwarebuildsid.ToString(),
                                    Value = p.Majorsoftwarebuildsid.ToString()
                                }).Distinct().ToList()
                                : query.Where(x => x.Majorsoftwarebuilds.Majorsoftwarebuildsid.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Majorsoftwarebuilds.Majorsoftwarebuildsid.ToString(),
                                    Value = p.Majorsoftwarebuildsid.ToString()
                                }).Distinct().ToList(),
                "majorHardwareBuildId" => string.IsNullOrEmpty(propertyFilter)
                                    ? query.Include(p => p.Systemtypesmajorhardwarebuilds).SelectMany(p => p.Systemtypesmajorhardwarebuilds).Select(p => new FilterValueDto
                                    {
                                        Text = p.Majorhardwareid.ToString(),
                                        Value = p.Majorhardwareid.ToString()
                                    }).Distinct().ToList()
                                    : query.Include(p => p.Systemtypesmajorhardwarebuilds).SelectMany(p => p.Systemtypesmajorhardwarebuilds)
                                     .Where(x => x.Majorhardwareid.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto
                                     {
                                         Text = p.Majorhardwareid.ToString(),
                                         Value = p.Majorhardwareid.ToString()
                                     }).Distinct().ToList(),
                "systemTypeId" => string.IsNullOrEmpty(propertyFilter)
                               ? query.Select(p => new FilterValueDto
                               {
                                   Text = p.Systemtypeid.ToString(),
                                   Value = p.Systemtypeid.ToString()
                               }).Distinct().ToList()
                               : query.Where(x => x.Systemtypeid.ToString().Contains(propertyFilter)).Select(p => new FilterValueDto
                               {
                                   Text = p.Systemtypeid.ToString(),
                                   Value = p.Systemtypeid.ToString()
                               }).Distinct().ToList(),

                "majorHardwareBuild" => string.IsNullOrEmpty(propertyFilter)
                               ? query.Include(x => x.Systemtypesmajorhardwarebuilds)
                               .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                               .Include(x => x.Systemtypesmajorhardwarebuilds)
                               .ThenInclude(x => x.Majorhardware)
                               .ThenInclude(x => x.Orgeqpmanufacturer)
                               .SelectMany(x => x.Systemtypesmajorhardwarebuilds).Select(p => new FilterValueDto
                               {
                                   Text = $"{p.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer} - {p.Majorhardware.Hardwaresolution} - {p.Majorhardware.Hardwaresolution} - {p.Majorhardware.Platform.Platform} - {p.Majorhardware.Hardwaretype}",
                                   Value = $"{p.Majorhardware.Platform.Platform}; {p.Majorhardware.Hardwaretype}",
                               }).Distinct().ToList()

                               : query.Include(x => x.Systemtypesmajorhardwarebuilds)
                               .ThenInclude(x => x.Majorhardware)
                               .ThenInclude(x => x.Orgeqpmanufacturer).Where(x =>
                                x.Systemtypesmajorhardwarebuilds.Any(t => t.Majorhardware.Platform.Platform.Contains(propertyFilter))
                                || x.Systemtypesmajorhardwarebuilds.Any(t => t.Majorhardware.Hardwaretype.Contains(propertyFilter))
                                //|| (x.SystemTypesMajorHardwareBuilds.FirstOrDefault(s => s.IsMain).MajorHardware.Platform + ";" + x.SystemTypesMajorHardwareBuilds.FirstOrDefault(s => s.IsMain).MajorHardware.HardwareType).Contains(propertyFilter)
                                ).SelectMany(x => x.Systemtypesmajorhardwarebuilds).Select(p => new FilterValueDto
                                {
                                    Text = $"{p.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer} - {p.Majorhardware.Hardwaresolution} - {p.Majorhardware.Hardwaresolution} - {p.Majorhardware.Platform.Platform} - {p.Majorhardware.Hardwaretype}",
                                    Value = $"{p.Majorhardware.Platform.Platform}; {p.Majorhardware.Hardwaretype}",
                                }).Distinct().ToList(),


                "softwareOem" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                        Value = p.Majorsoftwarebuilds.Orgeqpmanufacturer.ToString()
                    }).Distinct().ToList()
                    : query.Where(x =>
                        x.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.Contains(
                            propertyFilter)).Select(p => new FilterValueDto
                            {
                                Text = p.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                                Value = p.Majorsoftwarebuilds.Orgeqpmanufacturerid.ToString()
                            }).Distinct().ToList(),
                "hardwareOem" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                        .ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .SelectMany(x => x.Systemtypesmajorhardwarebuilds).Select(p => new FilterValueDto
                        {
                            Text = p.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                            Value = p.Majorhardware.Orgeqpmanufacturerid.ToString()
                        }).Distinct().ToList()
                    :
                    query.Include(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware)
                        .ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Systemtypesmajorhardwarebuilds)
                        .ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                       .Where(x =>
                            x.Systemtypesmajorhardwarebuilds.Any(t => t.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer.Contains(propertyFilter))
                            ).SelectMany(x => x.Systemtypesmajorhardwarebuilds).Select(p => new FilterValueDto
                            {
                                Text = p.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                                Value = p.Majorhardware.Orgeqpmanufacturerid.ToString()
                            }).Distinct().ToList(),

                "softWareDesignContactEmail" =>
                    query.SelectMany(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Where(x => x.Deleted == false).Select(x=>x.Designcontact)).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Email,
                        Value = p.Id.ToString()
                    }).Distinct().ToList()
                     .Concat(query.Where(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),
                "softWareVertical" =>
                    query.SelectMany(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Where(x => x.Deleted == false).SelectMany(x=>x.Designcontact.AspnetuserverticalsUser.Select(x=>x.Organisation.Vertical))).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Verticalresponsible,
                        Value = p.Verticalresponsibleid.ToString()
                    }).Distinct().ToList()
                     .Concat(query.Where(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),

                "softWareSubdomain" =>
                   _commonManager.GetOrganisationReleatedFilter(
                     query.SelectMany(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts).Where(x=>x.Deleted == false).Select(p => new FilterValueDto
                     {
                         Text = p.Designcontact.Email,
                         Value = p.Designcontactid.ToString()
                     }).Distinct().ToList()
                        .ToList())?.Distinct()?.ToList()
                         .Concat(query.Where(x => x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && x.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),

                "hardWareDesignContactEmail" =>
                    query.SelectMany(x => x.Systemtypesmajorhardwarebuilds.SelectMany(x=>x.Majorhardware.Majorhwbuildsdesigncontacts).Where(x=>x.Deleted == false).Select(x => x.Designcontact)).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Email,
                        Value = p.Id.ToString()
                    }).Distinct().ToList()
                    .Concat(query.Where(x => x.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware.Majorhwbuildsdesigncontacts != null && x.Majorhardware.Majorhwbuildsdesigncontacts.Count() <= 0))
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),
                "hardWareVertical" =>
                    query.SelectMany(x => x.Systemtypesmajorhardwarebuilds.SelectMany(x => x.Majorhardware.Majorhwbuildsdesigncontacts).Where(x=>x.Deleted == false)
                    .SelectMany(x => x.Designcontact.AspnetuserverticalsUser.Select(x=>x.Organisation.Vertical))).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Verticalresponsible,
                        Value = p.Verticalresponsibleid.ToString()
                    }).Distinct().ToList()
                     .Concat(query.Where(x => x.Systemtypesmajorhardwarebuilds.Any(x=>x.Majorhardware.Majorhwbuildsdesigncontacts != null && x.Majorhardware.Majorhwbuildsdesigncontacts.Count() <= 0))
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),
                "hardWareSubdomain" =>
                 _commonManager.GetOrganisationReleatedFilter(
                    query.SelectMany(x => x.Systemtypesmajorhardwarebuilds.SelectMany(x => x.Majorhardware.Majorhwbuildsdesigncontacts).Where(x=>x.Deleted == false).Select(x => x.Designcontact)).ToList()
                    .Select(p => new FilterValueDto
                    {
                        Text = p.Email,
                        Value = p.Id.ToString()
                    }).Distinct().ToList().ToList())?.Distinct()?.ToList()
                    .Concat(query.Where(x => x.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware.Majorhwbuildsdesigncontacts != null && x.Majorhardware.Majorhwbuildsdesigncontacts.Count() <= 0))
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),

                _ => new List<FilterValueDto>(),
            };
            if(!isAdmin && (propertyName == "softWareVertical" || propertyName == "hardWareVertical" ))
            {
                if(systemTypeFilterDto.VerticalResponsible != null && systemTypeFilterDto.VerticalResponsible.Count == 0)
                    systemTypeFilterDto.VerticalResponsible = verticalList;
                if(systemTypeFilterDto.VerticalResponsible != null && systemTypeFilterDto.VerticalResponsible.Count > 0)
                    rtn = rtn.Where(x => int.TryParse(x.Value, out int val) && systemTypeFilterDto.VerticalResponsible.Contains(val)).ToList();
            }
                
            return rtn;
        }

        private IIncludableQueryable<Systemtypes, Originalequipmentmanufacturers> GetQuery(ExpressionStarter<Systemtypes> predicateResult, bool includeDeleted)
        {
            var result = predicateResult.IsStarted ? _repositoryWrapper.SystemType.FindByCondition(predicateResult, includeDeleted)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .Include(x => x.Assetcategory)
                    .Include(x => x.VodafonenameNavigation)
                    .Include(x => x.Designcomponents)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Assetclass)
                    .Include(x => x.Assettype)
                    .Include(x => x.Productimportance)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                     .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts).ThenInclude(x=>x.Designcontact).ThenInclude(x=>x.AspnetuserverticalsUser).ThenInclude(x=>x.Organisation).ThenInclude(x=>x.Vertical)                   
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts).ThenInclude(x => x.Designcontact).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)

                      :
                _repositoryWrapper.SystemType.FindAll()
                  .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                    .Include(x => x.Assetcategory)
                    .Include(x => x.Designcomponents)
                    .Include(x => x.VodafonenameNavigation)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Assetclass)
                    .Include(x => x.Assettype)
                    .Include(x => x.Productimportance)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                     .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts).ThenInclude(x => x.Designcontact).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical)
                    .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts).ThenInclude(x => x.Designcontact).ThenInclude(x => x.AspnetuserverticalsUser).ThenInclude(x => x.Organisation).ThenInclude(x => x.Vertical)
                    .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer);


            return result;
        }



        public SystemTypeDtoCreate GetCreatePage(List<int> _verticalList)
        {
            // var verticalResponsibleResource = _repositoryWrapper.VerticalResponsible.FindAll();



            var verticalResponsibleResource = (_verticalList != null) ?
               _repositoryWrapper.VerticalResponsible.FindByCondition(x => _verticalList.Contains(x.Verticalresponsibleid)) :
               _repositoryWrapper.VerticalResponsible.FindAll();


            var subDomainResponsibleResource = _repositoryWrapper.SubDomainResponsible.FindAll();
            var assetCategoryResource = _repositoryWrapper.AssetCategory.FindAll().Include(x => x.Assetclass);
            var assetClassResource = _repositoryWrapper.AssetClass.FindAll();
            var assetTypeResource = _repositoryWrapper.AssetType.FindAll();
            var productImportanceResource = _repositoryWrapper.ProductImportance.FindAll();
            var subSpoc = _repositoryWrapper.SubDomainSpoc.FindByCondition(x => x.Issubdomain == true);
            //var vodafoneNamesResource = _repositoryWrapper.VodafoneNameRepository.FindAll();
            var model = new SystemTypeDtoCreate
            {
                VerticalResponsibleResource = verticalResponsibleResource.ToDictionary(x => x.Verticalresponsibleid, x => x.Verticalresponsible),
                SubDomainResponsibleResource = subDomainResponsibleResource.ToDictionary(x => x.Subdomainresponsibleid, x => x.Subdomainresponsible),
                AssetCategoryResource = assetCategoryResource.ToDictionary(x => x.Assetcategoryid, x => new AssetCategoryDto()
                {
                    Id = x.Assetcategoryid,
                    AssetClassId = x.Assetclass.Assetclass,
                    IdAssetClass = x.Assetclassid,
                    Description = x.Assetcategory,
                    TakeFromAssetTypeTable = x.Takefromassettypetable
                }),
                AssetClassResource = assetClassResource.ToDictionary(x => x.Assetclassid, x => new RelatedResource()
                {
                    Id = x.Assetclassid.ToString(),
                    Value = x.Assetclass
                }),
                AssetTypeResource = assetTypeResource.ToDictionary(x => x.Assettypeid, x => new RelatedResource()
                {
                    Id = x.Assetcategoryid.ToString(),
                    Value = x.Assettype
                }),
                ProductImportanceResource = productImportanceResource.ToDictionary(x => x.Productimportanceid, x => x.Productimportance),
                //SubDomainSpocResource = subSpoc.ToDictionary(x => (int)x.Subdomainspocid, x => x.Subdomainspoc),
                //VodafoneNameResource = vodafoneNamesResource.ToDictionary(x => x.Id, x => x.Description),
            };
            return model;
        }

        public SystemTypeDtoUpdate GetUpdatePage(long id, List<int> _verticalList)
        {

            var entity = _repositoryWrapper.SystemType.FindByCondition(x => x.Systemtypeid == id, true)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.VodafonenameNavigation)
                .Include(x => x.Assetcategory).ThenInclude(x => x.Assetclass)
                //.Include(x => x.AssetClassIdNavigation)
                .Include(x => x.Assettype)
                //.Include(x => x.Subdomainresponsible)
                //.Include(x => x.Systemtypessubdomainspoc)
                //.Include(x => x.Verticalresponsible)
                .Include(x => x.VodafonenameNavigation)
                .Include(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)

                .Single();

            var dto = _mapper.Map<SystemTypeDtoUpdate>(SystemTypeMapper.GetSystemTypeMapper(entity));

            #region lookup
            //var subSpoc = _repositoryWrapper.SubDomainSpoc.FindByCondition(x => x.Issubdomain == true);
            //dto.SubDomainSpocResource =
            //    subSpoc.ToDictionary(x => (int)x.Subdomainspocid, x => x.Subdomainspoc);
            //if (dto.SubDomainSpocIds != null && dto.SubDomainSpocIds.Any())
            //{
            //    foreach (var idsds in dto.SubDomainSpocIds)
            //    {
            //        if (!dto.SubDomainSpocResource.ContainsKey(idsds))
            //        {
            //            var data = _repositoryWrapper.SubDomainSpoc.FindByCondition(
            //                x => x.Subdomainspocid == (short)idsds, true).SingleOrDefault();
            //            if (data != null)
            //            {
            //                dto.SubDomainSpocResource.Add(data.Subdomainspocid,
            //                    data.Subdomainspoc);
            //            }
            //        }
            //    }


            //}

            dto.AssetClassDescription = SystemTypeMapper.GetSystemTypeMapper(entity).toAssetClassDescription(_repositoryWrapper);


            var productImportanceResource = _repositoryWrapper.ProductImportance.FindAll();
            dto.ProductImportanceResource = productImportanceResource.ToDictionary(x => x.Productimportanceid, x => x.Productimportance);

            if (dto.ProductImportanceId.HasValue && !dto.ProductImportanceResource.ContainsKey(dto.ProductImportanceId.Value))
            {
                var data = _repositoryWrapper.ProductImportance.FindByCondition(x => x.Productimportanceid == dto.ProductImportanceId, true).SingleOrDefault();
                if (data != null)
                {
                    dto.ProductImportanceResource.Add(data.Productimportanceid, data.Productimportance);
                }
            }

            // var verticalResponsibleResource = _repositoryWrapper.VerticalResponsible.FindAll();

            var verticalResponsibleResource = (_verticalList != null) ?
               _repositoryWrapper.VerticalResponsible.FindByCondition(x => _verticalList.Contains(x.Verticalresponsibleid)) :
               _repositoryWrapper.VerticalResponsible.FindAll();

            dto.VerticalResponsibleResource = verticalResponsibleResource.ToDictionary(x => x.Verticalresponsibleid, x => x.Verticalresponsible);
            if (dto.VerticalResponsibleId.HasValue &&
                !dto.VerticalResponsibleResource.ContainsKey(dto.VerticalResponsibleId.Value))
            {
                var data = _repositoryWrapper.VerticalResponsible.FindByCondition(
                    x => x.Verticalresponsibleid == dto.VerticalResponsibleId, true).SingleOrDefault();
                if (data != null)
                {
                    dto.VerticalResponsibleResource.Add(data.Verticalresponsibleid, data.Verticalresponsible);
                }
            }

            var subDomainResponsibleResource = _repositoryWrapper.SubDomainResponsible.FindAll();
            dto.SubDomainResponsibleResource = subDomainResponsibleResource.ToDictionary(x => x.Subdomainresponsibleid, x => x.Subdomainresponsible);

            if (dto.SubDomainResponsibleId.HasValue &&
                !dto.SubDomainResponsibleResource.ContainsKey(dto.SubDomainResponsibleId.Value))
            {
                var data = _repositoryWrapper.SubDomainResponsible.FindByCondition(
                    x => x.Subdomainresponsibleid == dto.SubDomainResponsibleId, true).SingleOrDefault();
                if (data != null)
                {
                    dto.SubDomainResponsibleResource.Add(data.Subdomainresponsibleid, data.Subdomainresponsible);
                }
            }

            var assetCategoryResource = _repositoryWrapper.AssetCategory.FindAll().Include(x => x.Assetclass);
            dto.AssetCategoryResource = assetCategoryResource.ToDictionary(x => x.Assetcategoryid, x => new AssetCategoryDto()
            {
                Id = x.Assetcategoryid,
                AssetClassId = x.Assetclass.Assetclass,
                IdAssetClass = x.Assetclassid,
                Description = x.Assetcategory,
                TakeFromAssetTypeTable = x.Takefromassettypetable
            });

            dto.AssetClassResource = _repositoryWrapper.AssetClass.FindAll().ToDictionary(x => x.Assetclassid, x => new RelatedResource()
            {
                Id = x.Assetclassid.ToString(),
                Value = x.Assetclass
            });

            var assetTypeResource = _repositoryWrapper.AssetType.FindAll();
            dto.AssetTypeResource = assetTypeResource.ToDictionary(x => x.Assettypeid, x => new RelatedResource()
            {
                Id = x.Assetcategoryid.ToString(),
                Value = x.Assettype
            });

            var vodafoneNameResource = _repositoryWrapper.VodafoneNameRepository.FindAll();
            dto.vodafoneNameId = entity.Vodafonename;
            dto.VodafoneNameResource = vodafoneNameResource.ToDictionary(x => x.Id,
                x => x.Description);
            if (dto.vodafoneNameId.HasValue && !dto.VodafoneNameResource.ContainsKey(dto.vodafoneNameId.Value))
            {
                var data = _repositoryWrapper.VodafoneNameRepository.FindByCondition(
                   x => x.Id == dto.vodafoneNameId,
                   includeDeleted: true).SingleOrDefault();

                var voddfoneName = _repositoryWrapper.VodafoneNameRepository.FindByCondition(
                    x => x.Id == dto.vodafoneNameId,
                    includeDeleted: true).SingleOrDefault();
                if (data != null)
                {
                    dto.VodafoneNameResource.Add(voddfoneName.Id, voddfoneName.Description);
                }
            }

            if (dto.AssetTypeId.HasValue &&
                !dto.AssetTypeResource.ContainsKey(dto.AssetTypeId.Value))
            {
                var data = _repositoryWrapper.AssetType.FindByCondition(
                    x => x.Assettypeid == dto.AssetTypeId, true).SingleOrDefault();
                if (data != null)
                {
                    dto.AssetTypeResource.Add(data.Assettypeid, new RelatedResource()
                    {
                        Id = data.Assetcategoryid.ToString(),
                        Value = data.Assettype
                    });
                }
            }

            #endregion






            return dto;
        }

        // there is no reference in this methode
        //public Dictionary<int, string> GetVerticalResponsile(long id)
        //{
        //    var usersVerticalResponsibless = _repositoryWrapper.UserRoleRepository.FindByCondition(x => x.Userid == id)
        //        .Include(x => x.Verticalresponsible)
        //        .Include(x => x.Role).ToList();
        //    Dictionary<int, string> verticalResponsibles = new Dictionary<int, string>();
        //    if (usersVerticalResponsibless != null && usersVerticalResponsibless.Count > 0)
        //    {
        //        foreach (var userVerticalResponsible in usersVerticalResponsibless)
        //        {
        //            if (userVerticalResponsible.Roleid == 1)
        //            {
        //                verticalResponsibles = usersVerticalResponsibless.ToDictionary(x => (int)x.Verticalresponsibleid, x => x.Verticalresponsible.Verticalresponsible);
        //                break;
        //            }
        //            else
        //            {
        //                verticalResponsibles.Add(usersVerticalResponsibless.Select(x => (int)x.Verticalresponsibleid).FirstOrDefault(), usersVerticalResponsibless.Select(x => x.Verticalresponsible).FirstOrDefault().Verticalresponsible);
        //            }
        //        }

        //    }


        //    return verticalResponsibles;
        //}



    }
}


