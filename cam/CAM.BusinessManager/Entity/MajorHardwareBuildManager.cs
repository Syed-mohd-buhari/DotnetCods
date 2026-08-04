using AutoMapper;
using CAM.BusinessManager.Business.PlannedActivity;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.DesignComponent;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Entities.Models.Cross;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
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
    public class MajorHardwareBuildManager:BaseManager
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _manager;
        private PlannedActivityManager _plannedActivityManager;
        private readonly SystemTypeManager _systemTypeManager;
        private readonly PlannedActivityCommon _plannedActivityCommon;
        private readonly LcmEngineeringManager _lcmEngineeringManager;
        private DesignComponentManager _designComponentManager;
        private readonly CommonManager _commonManager;
        
        public MajorHardwareBuildManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager, IRepositoryWrapper repositoryWrapper,
            PlannedActivityManager plannedActivityManager, PlannedActivityCommon plannedActivityCommon, SystemTypeManager systemTypeManager, 
            LcmEngineeringManager lcmEngineeringManager, DesignComponentManager designComponentManager ,
            IHttpContextAccessor contextAccessor, CommonManager commonManager ) :base(contextAccessor, wrappers, out repositoryWrapper)
        {
           
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _plannedActivityManager = plannedActivityManager;
            _plannedActivityCommon = plannedActivityCommon;
            _systemTypeManager = systemTypeManager;
            _lcmEngineeringManager = lcmEngineeringManager;
            _designComponentManager = designComponentManager;
            _commonManager = commonManager;
           
        }
  
        public async Task<ResultDto> Add(MajorHardwareBuildDtoCreate dto, bool? forced = false)
        {
            var entityExists = await EntityExists(dto);


            if (entityExists != null)
            {
                var relations = _repositoryWrapper.SystemTypesMajorHardwareBuild
                    .FindByCondition(x => x.Majorhardwareid == entityExists.MajorHardwareId, true).FirstOrDefault();
                if (relations == null && entityExists.Deleted == true)
                {
                    if (forced == true)
                    {
                        var entityForced = _mapper.Map<MajorHardwareBuild>(dto);


                        entityForced.HardwareSolution = !string.IsNullOrEmpty(dto.HardwareSolution) ? dto.HardwareSolution : ConstantValueFilter.multipleApplicationsForHardwareSolution;
                        entityForced.HardwareType = !string.IsNullOrEmpty(dto.HardwareType) ? dto.HardwareType : ConstantValueFilter.variousHardwareType;
                        if (dto.PlatformId == null  || dto.PlatformId == 0)
                        {
                            var platform = _repositoryWrapper.Platform.FindByCondition(x => x.Platform.ToLower() == ConstantValueFilter.virtualizedPlatform).SingleOrDefault();
                            entityForced.PlatformId = platform != null ? platform.Platformid : (short)0;
                        }
                        _repositoryWrapper.MajorHardwareBuild.Create(MajorHardwareBuildMapper.SetMajorHardwareBuildMapper(entityForced));
                        await _repositoryWrapper.SaveAsync();
                        return new ResultDto { Info = ResultMessages.EntryAddSuccess };
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryAddExists,
                            Data = new { id = entityExists.MajorHardwareId, orphanDeleted = true }
                        };
                    }
                }
                else
                {
                    if (!entityExists.Deleted)
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryAddExists,
                            Data = new { id = entityExists.MajorHardwareId, orphanDeleted = false }
                        };
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryAddExistsDeleted,
                            Data = entityExists.MajorHardwareId
                        };

                    }
                }
            }


            var entity = _mapper.Map<MajorHardwareBuild>(dto);

            entity.HardwareSolution = !string.IsNullOrEmpty(dto.HardwareSolution) ? dto.HardwareSolution : ConstantValueFilter.multipleApplicationsForHardwareSolution;// "Multiple Applications";
            entity.HardwareType = !string.IsNullOrEmpty(dto.HardwareType) ? dto.HardwareType : ConstantValueFilter.variousHardwareType;// "Various";
            if (dto.PlatformId == null || dto.PlatformId == 0)
            {
                var platform = _repositoryWrapper.Platform.FindByCondition(x => x.Platform.ToLower() == ConstantValueFilter.virtualizedPlatform).SingleOrDefault();
                entity.PlatformId = platform != null? platform.Platformid : (short)0;
            }

            var result = MajorHardwareBuildMapper.SetMajorHardwareBuildMapper(entity);
            _repositoryWrapper.MajorHardwareBuild.Create(result);
            
            await _repositoryWrapper.SaveAsync();
            var mjhId = result.Majorhardwareid;
            await AddOrUpdateDesignContact(dto.DesignContactIds, mjhId);
            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        public async Task<MajorHardwareBuild> EntityExists(MajorHardwareBuildDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId
                         && x.Platformid == dto.PlatformId
                         && x.Hardwaretype.ToLower().Replace(" ", "") == dto.HardwareType.ToLower().Replace(" ", "")
                         && x.Hardwaresolution.ToLower().Replace(" ", "") == dto.HardwareSolution.ToLower().Replace(" ", "")
                    , true).Include(x => x.Systemtypesmajorhardwarebuilds).OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync();
            return MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(entityExists);
        }
        public async Task<MajorHardwareBuild> EntityExists(MajorHardwareBuildDtoUpdate dto)
        {
            var entityExists = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(
                    x => x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId
                         && x.Platformid == dto.PlatformId
                         && x.Hardwaretype.ToLower().Replace(" ", "") == dto.HardwareType.ToLower().Replace(" ", "")
                         && x.Majorhardwareid != dto.MajorHardwareId
                         && x.Hardwaresolution.ToLower().Replace(" ", "") == dto.HardwareSolution.ToLower().Replace(" ", "")
                    , true).Include(x => x.Systemtypesmajorhardwarebuilds).OrderByDescending(x => x.Creationdate)
                .FirstOrDefaultAsync();
            return MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(entityExists);
        }

        public async Task<ResultDto> Update(MajorHardwareBuildDtoUpdate dto, bool? forced)
        {
            var tr = await _repositoryWrapper.BeginTransactionAsync();
            try
            {
                var anotherEntityWithSameNaturalKeyExists = await EntityExists(dto);
                var originalEntityWithSameNaturalKey = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(
                    x => x.Majorhardwareid == dto.MajorHardwareId
                && x.Orgeqpmanufacturerid == dto.OriginalEquipmentManufacturerId
                        && x.Platformid == dto.PlatformId
                        && x.Hardwaretype == dto.HardwareType
                        && x.Hardwaresolution == dto.HardwareSolution
                        , true).SingleOrDefaultAsync();

                if (anotherEntityWithSameNaturalKeyExists != null && originalEntityWithSameNaturalKey == null)
                {
                    var relations = _repositoryWrapper.SystemTypesMajorHardwareBuild
                        .FindByCondition(x => x.Majorhardwareid == anotherEntityWithSameNaturalKeyExists.MajorHardwareId, true).FirstOrDefault();
                    if (relations == null && anotherEntityWithSameNaturalKeyExists.Deleted == true)
                    {
                        if (forced == true)
                        {
                            var results = await BaseUpdate(dto, forced);
                            await tr.CommitAsync();
                            return results;
                        }
                        else
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryUpdateExists,
                                Data = new { id = anotherEntityWithSameNaturalKeyExists.MajorHardwareId, orphanDeleted = true }
                            };
                        }
                    }
                    else
                    {
                        if (!anotherEntityWithSameNaturalKeyExists.Deleted)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExists,
                                Data = new { id = anotherEntityWithSameNaturalKeyExists.MajorHardwareId, orphanDeleted = false }
                            };
                        }
                        else
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExistsDeleted,
                                Data = anotherEntityWithSameNaturalKeyExists.MajorHardwareId
                            };

                        }
                    }

                }
                var result = await BaseUpdate(dto, forced);
                await tr.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await tr.RollbackAsync();
                
                throw;
            }
          
        }

        private async Task<ResultDto> BaseUpdate(MajorHardwareBuildDtoUpdate dto, bool? forced)
        {
            var entity = await UpdateMajorHardwareBuildEntity(dto, forced);
            var systemTypes = _repositoryWrapper.SystemType.FindByCondition(x =>
                    x.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardwareid == entity.MajorHardwareId), false)
                .Include(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware)
                .Include(x => x.Majorsoftwarebuilds)
                .ToList();

            foreach (var updated in systemTypes.Select(systemType => _systemTypeManager.SetSystemTypeValue(systemType)))
            {
                _repositoryWrapper.SystemType.Update(updated);
            }

            await _repositoryWrapper.SaveAsync();
            var sistemTypeIds = _repositoryWrapper.SystemTypesMajorHardwareBuild
            .FindByCondition(x => x.Ismain && !x.Deleted.Value && x.Majorhardwareid == entity.MajorHardwareId)
            .Select(x => x.Systemtypeid).ToList();


            var designComponent = await _repositoryWrapper.DesignComponent
                .FindByCondition(x => sistemTypeIds.Contains(x.Systemtypeid)).Include(x => x.Lcmengineering)
                .ThenInclude(x => x.PlannedactivitiesLcmengineering).AsNoTracking().ToListAsync();

             ColateralEffect(designComponent, sistemTypeIds);

            await AddOrUpdateDesignContact(dto.DesignContactIds, entity.MajorHardwareId);
            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.MajorHardwareId
            };
        }
        private void ColateralEffect(List<Designcomponents> designComponent, List<long> sistemTypeIds)
        {          
            foreach (var t in designComponent)
            {             
                var dcf =  _repositoryWrapper.DesignComponentFamily
                    .FindByCondition(x => x.Designcomponentfamilyid == t.Designcomponentfamilyid)
                    .Include(x => x.Subnetworkboundary)
                    .FirstOrDefault();
               
                if(dcf != null)
                {
                    _ = _designComponentManager.Update(new DesignComponentDtoUpdate()
                    {
                        PlatformId = dcf.Platformid,
                        SubNetworkBoundaryIds = new List<long>() { dcf.Subnetworkboundaryid },
                        DesignComponentId = t.Designcomponentid,
                        GdprRelevant = dcf.Subnetworkboundary?.Gdprrelevant,
                        SystemTypeId = t.Systemtypeid,
                        DesignComponentFamilyId = t.Designcomponentfamilyid,
                        VisibleFlag = (t.Visibleflag == null) ? true : t.Visibleflag,
                    });
                }
               
                 _repositoryWrapper.SaveAsync();
            } 
            _ = _plannedActivityCommon.ReloadPlannedActivity(sistemTypeIds, true);
        }

        public async Task<MajorHardwareBuild> UpdateMajorHardwareBuildEntity(MajorHardwareBuildDtoUpdate dto, bool? forced = false)
        {
            var entity = _mapper.Map<MajorHardwareBuild>(dto);
            if (forced == true)
            {
                entity.Deleted = false;
                entity.DeletionDate = null;
            }

            entity.HardwareSolution = !string.IsNullOrEmpty(dto.HardwareSolution) ? dto.HardwareSolution : ConstantValueFilter.multipleApplicationsForHardwareSolution;
            entity.HardwareType = !string.IsNullOrEmpty(dto.HardwareType) ? dto.HardwareType : ConstantValueFilter.variousHardwareType;
            if (dto.PlatformId == null || dto.PlatformId == 0)
            {
                var platform = _repositoryWrapper.Platform.FindByCondition(x => x.Platform.ToLower() == ConstantValueFilter.virtualizedPlatform).SingleOrDefault();
                entity.PlatformId = platform != null ? platform.Platformid : (short)0;
            }
            _repositoryWrapper.MajorHardwareBuild.Update(MajorHardwareBuildMapper.SetMajorHardwareBuildMapper(entity));
            await _repositoryWrapper.SaveAsync();
            return entity;
        }


        public async Task<ResultDto> Delete(long id)
        {
            var entity = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == id).SingleAsync();
            _repositoryWrapper.MajorHardwareBuild.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Majorhardwareid
            };
        }


        public async Task<ResultDto> DeleteDeep(long id)
        {
            #region  
            var hardwareDesignContact = _repositoryWrapper.MajorHwBuidlsDesignContactsRepository .FindByCondition(x => x.Majorhardwarebuildsid == id).ToList();
             
            foreach (var toDelete in hardwareDesignContact)
                _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.DeleteDeep(toDelete);

            await _repositoryWrapper.SaveAsync();
            #endregion

            var entity = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == id).SingleAsync();
            _repositoryWrapper.MajorHardwareBuild.Delete(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = entity.Majorhardwareid
            };
        }

        public async Task<ResultDto> GetRelatedRecords(long id)
        {
            Dictionary<string, List<long>> refTableDuplicateRecordCheckDic = new Dictionary<string, List<long>>();
            var entities = _repositoryWrapper.SystemTypesMajorHardwareBuild
                .FindByCondition(x => x.Majorhardwareid == id).Include(x=>x.Systemtype)
                .ToDictionary(x => x.Systemtypeid,
                    x => x.Systemtype.toSystemTypeName(_repositoryWrapper)
                 );
          
            var tentities = _repositoryWrapper.SystemTypesMajorHardwareBuild
                .FindByCondition(x => x.Majorhardwareid == id).Include(x=>x.Systemtype)
                .Select(x =>x.Systemtype.toSystemTypeName(_repositoryWrapper)).ToArray();

            List<ResultMessageDto> rm = new List<ResultMessageDto>();
            if (entities.Count > 0)
            {
                rm.Add(new ResultMessageDto() { Table = "System Types Major Hardware Build", Values = entities.Select(x => x.Value).ToArray() });
                refTableDuplicateRecordCheckDic["Systemtypesmajorhardwarebuilds"] = entities.Select(x => x.Key).ToList();
            }
              
            var entity = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == id)
                             .Include(x => x.Orgeqpmanufacturer)
                             .Include(x => x.Platform).SingleAsync();
            //Ticket 814 - Deletion of HW build, SW build, SystemType -- #Req#3026: Delinking Archived / Libraries
            List<string> removeDuplicatesAndLinkedTableCheck = new List<string>()
            {
                "Systemtypesmajorhardwarebuilds" ,"Majorhwbuildsdesigncontacts"
            };
            var referenceTableRecord = _commonManager.GetReferencedForeignKeyTablesAsync("Majorhardwarebuilds", id, removeDuplicatesAndLinkedTableCheck, refTableDuplicateRecordCheckDic);

            if (referenceTableRecord.Result != null && referenceTableRecord.Result.Count() > 0)
                rm.Add(new ResultMessageDto() { Table = _commonManager.popupTabName , Values = referenceTableRecord.Result.ToArray() });

            if (rm.Count > 0)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto()
                    {
                        EntityName = "Major Hardware Build",
                        RecordName = entity.Orgeqpmanufacturer.Originalequipmentmanufacturer + " - " + entity.Platform.Platform + " - " + entity.Hardwaretype,
                        DataRelatedList = rm
                    }
                };
            }
            else
                return new ResultDto();
        }



        public async Task<ResultDto> Restore(long id)
        {

            var entity = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == id, true).SingleAsync();
            var anotherEntityWithSameNaturalKeyExists = await _repositoryWrapper.MajorHardwareBuild.FindByCondition(
               x => x.Orgeqpmanufacturer == entity.Orgeqpmanufacturer
                   && x.Platform == entity.Platform
                   && x.Hardwaretype == entity.Hardwaretype
                   && x.Majorhardwareid != entity.Majorhardwareid).FirstOrDefaultAsync();
            if (anotherEntityWithSameNaturalKeyExists != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = anotherEntityWithSameNaturalKeyExists.Majorhardwareid
                };
            }
            entity.Deleted = false;
            entity.Deletiondate = null;

            _repositoryWrapper.MajorHardwareBuild.Update(entity);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = entity.Majorhardwareid
            };
        }

        public QueryResultDto<MajorHardwareBuildDtoGrid> FindWithCondition(MajorHardwareBuildQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            if (buildFilterDto.Orphan == true)
            {
                predicateResult.And(x => !x.Systemtypesmajorhardwarebuilds.Any() && x.Systemtypesmajorhardwarebuilds.Count() == 0);
            }
            // Ticket 831 - Remove Unknown Softwareversion in MajorSoftwareBuild/Hardware Screen - Aug 1st 
            predicateResult.And(x => x.Hardwaretype.ToLower() != ConstantValueFilter.Unknown);

            var rtn = new QueryResultDto<MajorHardwareBuildDtoGrid>(new GenerateRenderForGrid<MajorHardwareBuildDtoGrid>(_manager))
            {
              
               //TotalItems = predicateResult.IsStarted ? _repositoryWrapper.MajorHardwareBuild.Count(predicateResult) : _repositoryWrapper.MajorHardwareBuild.Count(),
            };


            var query = predicateResult.IsStarted ? _repositoryWrapper.MajorHardwareBuild.FindByCondition(predicateResult, buildFilterDto.Deleted ?? false)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.Orgeqpmanufacturer)
                .Include(x => x.Hardwaresolutionreource)
                .Include(x => x.Platform)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .Include(x => x.Buildconstruction)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Majorhwbuildsdesigncontacts).ThenInclude(x => x.Designcontact)
                .AsEnumerable()
                .Select(p => MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(p))
                .AsQueryable()

                :
                _repositoryWrapper.MajorHardwareBuild.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.Orgeqpmanufacturer)
                .Include(x => x.Hardwaresolutionreource)
                .Include(x => x.Systemtypesmajorhardwarebuilds)
                .Include(x => x.Platform)
                .Include(x => x.Buildconstruction)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.Majorhwbuildsdesigncontacts).ThenInclude(x => x.Designcontact)
                .AsEnumerable()
                .Select(p => MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(p))
                .AsQueryable();
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(buildFilterDto, GetColumnsMap()).ApplyPaging(buildFilterDto);

            var data = query.ToList();
            if (buildFilterDto.PrincipalId != 0)
            {
                var exist = data.Any(x => x.MajorHardwareId == buildFilterDto.PrincipalId);
                if (!exist)
                {
                    var addedResource = _repositoryWrapper.MajorHardwareBuild.FindAll(true)
                       .Include(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Platform)
                        .Include(x => x.Buildconstruction)
                        .Include(x => x.Systemtypesmajorhardwarebuilds)
                        .Single(x => x.Majorhardwareid == buildFilterDto.PrincipalId);
                    data.Add(MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(addedResource));
                }
            }

            if (buildFilterDto.PrincipalIdList != null)
            {
                foreach (var id in buildFilterDto.PrincipalIdList)
                {
                    if (id != 0)
                    {
                        var exist = data.Any(x => x.MajorHardwareId == id);
                        if (!exist)
                        {

                            var addedResource = _repositoryWrapper.MajorHardwareBuild.FindAll(true)
                               .Include(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Platform)
                                .Include(x => x.Buildconstruction)
                                .Include(x => x.Systemtypesmajorhardwarebuilds)
                                .SingleOrDefault(x => x.Majorhardwareid == id);
                            if (addedResource != null)
                                data.Add(MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(addedResource));


                        }
                    }
                }
            }
            
            var majorHardwareBuildResult = _mapper.Map<IEnumerable<MajorHardwareBuildDtoGrid>>(data);
            rtn.Items = majorHardwareBuildResult.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Majorhardwarebuilds> ApplyFilter(MajorHardwareBuildQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Majorhardwarebuilds>();
            var predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();

           

            if (buildFilterDto.VulnerabilityStatus != null && buildFilterDto.VulnerabilityStatus.Any())
            {
                foreach (var item in buildFilterDto.VulnerabilityStatus)
                    predicateInner.Or(x => x.Vulnerabilitystatus == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OriginalEquipmentManufacturer != null && buildFilterDto.OriginalEquipmentManufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.OriginalEquipmentManufacturer)
                    predicateInner.Or(x => x.Orgeqpmanufacturerid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Platform != null && buildFilterDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.Platform)
                    predicateInner.Or(x => x.Platformid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HardwareType != null && buildFilterDto.HardwareType.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.HardwareType)
                    predicateInner.Or(x => x.Hardwaretype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Description != null && buildFilterDto.Description.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.Description)
                    predicateInner.Or(x => x.Description == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.BuildConstruction != null && buildFilterDto.BuildConstruction.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.BuildConstruction)
                    predicateInner.Or(x => x.Buildconstruction.Buildconstruction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MajorHardwareBuildId != null && buildFilterDto.MajorHardwareBuildId.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.MajorHardwareBuildId)
                    predicateInner.Or(x => x.Majorhardwareid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.EndOfMaintenanceValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                if (buildFilterDto.EndOfMaintenanceValue.StartDate != null)
                    predicateInner.And(x => x.Endofmaintenance >= buildFilterDto.EndOfMaintenanceValue.StartDate);
                if (buildFilterDto.EndOfMaintenanceValue.EndDate != null)
                    predicateInner.And(x => x.Endofmaintenance <= buildFilterDto.EndOfMaintenanceValue.EndDate);
                predicateResult.And(predicateInner);
            }

            //if (buildFilterDto.OriginalEquipmentManufacturerId != null) //Duplicato?
            //{
            //    predicateInner = PredicateBuilder.New<MajorHardwareBuild>();

            //    foreach (var item in buildFilterDto.OriginalEquipmentManufacturerId)
            //        predicateInner.Or(x => x.OriginalEquipmentManufacturerId == item);
            //    predicateResult.And(predicateInner);
            //}

            if (buildFilterDto.EndOfsupportValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                if (buildFilterDto.EndOfsupportValue.StartDate != null)
                    predicateInner.And(x => x.Endofsupport >= buildFilterDto.EndOfsupportValue.StartDate);
                if (buildFilterDto.EndOfsupportValue.EndDate != null)
                    predicateInner.And(x => x.Endofsupport <= buildFilterDto.EndOfsupportValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.GeneraAvailableDateValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                if (buildFilterDto.GeneraAvailableDateValue.StartDate != null)
                    predicateInner.And(x => x.Generaavailabledate >= buildFilterDto.GeneraAvailableDateValue.StartDate);
                if (buildFilterDto.GeneraAvailableDateValue.EndDate != null)
                    predicateInner.And(x => x.Generaavailabledate <= buildFilterDto.GeneraAvailableDateValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (!string.IsNullOrEmpty(buildFilterDto.Name))
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();

                predicateInner.And(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer.Contains(buildFilterDto.Name) || x.Platform.Platform.Contains(buildFilterDto.Name) || x.Hardwaretype.Contains(buildFilterDto.Name));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.HardwareSolution != null && buildFilterDto.HardwareSolution.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                foreach (var item in buildFilterDto.HardwareSolution)
                {
                    //if (item.Contains("ProprietaryHW"))
                    //{
                        predicateInner.Or(x => x.Hardwaresolution == item);
                    //}
                    //else
                    //{
                    //    predicateInner.Or(x => x.Hardwaresolutionreource.Hardwaresolutionreource == item);
                    //}
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastTimeBuyExpansionsValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                if (buildFilterDto.LastTimeBuyExpansionsValue.StartDate != null)
                    predicateInner.And(x => x.Lasttimebuyexpansions >= buildFilterDto.LastTimeBuyExpansionsValue.StartDate);
                if (buildFilterDto.LastTimeBuyExpansionsValue.EndDate != null)
                    predicateInner.And(x => x.Lasttimebuyexpansions <= buildFilterDto.LastTimeBuyExpansionsValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastTimeBuyNewValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                if (buildFilterDto.LastTimeBuyNewValue.StartDate != null)
                    predicateInner.And(x => x.Lasttimebuynew >= buildFilterDto.LastTimeBuyNewValue.StartDate);
                if (buildFilterDto.LastTimeBuyNewValue.EndDate != null)
                    predicateInner.And(x => x.Lasttimebuynew <= buildFilterDto.LastTimeBuyNewValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.LastTimeBuyUpgradesValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                if (buildFilterDto.LastTimeBuyUpgradesValue.StartDate != null)
                    predicateInner.And(x => x.Lasttimebuyupgrades >= buildFilterDto.LastTimeBuyUpgradesValue.StartDate);
                if (buildFilterDto.LastTimeBuyUpgradesValue.EndDate != null)
                    predicateInner.And(x => x.Lasttimebuyupgrades <= buildFilterDto.LastTimeBuyUpgradesValue.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.OtherHardwareInfo != null && buildFilterDto.OtherHardwareInfo.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();

                foreach (var item in buildFilterDto.OtherHardwareInfo)
                    predicateInner.Or(x => x.Otherhardwareinfo == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedValue != null)
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();
                if (buildFilterDto.LastModifiedValue.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModifiedValue.StartDate);
                if (buildFilterDto.LastModifiedValue.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModifiedValue.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DesignContact != null && buildFilterDto.DesignContact.Any())
            {
                predicateInner = PredicateBuilder.New<Majorhardwarebuilds>();

                foreach (var item in buildFilterDto.DesignContact)
                    if(item == "yes")
                    {
                        predicateInner.Or(x => x.Majorhwbuildsdesigncontacts != null && !x.Majorhwbuildsdesigncontacts.Any());
                    }
                    else
                    {
                        predicateInner.Or(x => x.Majorhwbuildsdesigncontacts.Any(t => t.Designcontactid.ToString() == item));
                    }
                predicateResult.And(predicateInner);
            }


            return predicateResult;
        }


        public MajorHardwareBuildDto Get(long id)
        {
            var entity = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == id).Single();
            return _mapper.Map<MajorHardwareBuildDto>(entity);
        }

        private Dictionary<string, Expression<Func<MajorHardwareBuild, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<MajorHardwareBuild, object>>[]>
            {
                ["majorHardwareBuildId"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.MajorHardwareId },
                ["originalEquipmentManufacturerId"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.OriginalEquipmentManufacturerId },
                ["hardwareSolution"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.HardwareSolution },
                ["platform"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.Platform.PlatformDescription },
                ["description"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.Description },
                ["hardwareType"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.HardwareType },
                ["otherHardwareInfo"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.OtherHardwareInfo },
                ["lastTimeBuyNewValue"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.LastTimeBuyNew },
                ["lastTimeBuyUpgradesValue"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.LastTimeBuyUpgrades },
                ["lastTimeBuyExpansionsValue"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.LastTimeBuyExpansions },
                ["endOfMaintenanceValue"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.EndOfMaintenance },
                ["generaAvailableDateValue"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.GeneraAvailableDate },
                ["endOfsupportValue"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.EndOfsupport },
                ["vulnerabilityStatus"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.VulnerabilityStatus },
                ["spareFieldsJson"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.SpareFieldsJson },
                ["originalEquipmentManufacturer"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.OriginalEquipmentManufacturer.OriginalEquipmentManufacturerDescription },
                ["buildConstruction"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.BuildConstruction != null? p.BuildConstruction.BuildConstructionDescription: string.Empty },
                ["lastModifiedBy"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.ModificationUserEntity.Email },
                ["lastModifiedValue"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.ModificationDate },
                ["designContact"] = new Expression<Func<MajorHardwareBuild, object>>[] { p => p.DesignContactNavigation.Email },


            };
        }

        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, MajorHardwareBuildQueryDto buildFilterDto)
        {
            
            var predicateResult = ApplyFilter(buildFilterDto);
            // Ticket 831 - Remove Unknown Softwareversion in MajorSoftwareBuild/Hardware Screen - Aug 1st 
            predicateResult.And(x => x.Hardwaretype.ToLower() != ConstantValueFilter.Unknown);
            var query = predicateResult.IsStarted ? _repositoryWrapper.MajorHardwareBuild.FindByCondition(predicateResult, buildFilterDto.Deleted ?? false)
                    .Include(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Hardwaresolutionreource)
                    .Include(x => x.Platform)
                    .Include(x => x.Systemtypesmajorhardwarebuilds)
                    .Include(x => x.Buildconstruction)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Majorhwbuildsdesigncontacts)

                : _repositoryWrapper.MajorHardwareBuild.FindAll()
                    .Include(x => x.Orgeqpmanufacturer)
                    .Include(x => x.Hardwaresolutionreource)
                    .Include(x => x.Systemtypesmajorhardwarebuilds)
                    .Include(x => x.Platform)
                    .Include(x => x.Buildconstruction)
                    .Include(x => x.ModificationuserNavigation)
                    .Include(x => x.Majorhwbuildsdesigncontacts);


            var rtn = propertyName switch
            {
                "hardwareSolution" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(p => new FilterValueDto
                    (p.Hardwaresolution ?? p.Hardwaresolutionreource?.Hardwaresolutionreource)).Distinct().ToList()
                    : query.ToList()
                        .Where(x => (x.Hardwaresolution != null && x.Hardwaresolution.ToUpper().Contains(propertyFilter.ToUpper())) || (x.Hardwaresolutionreource != null 
                        && x.Hardwaresolutionreource.Hardwaresolutionreource.ToUpper().Contains(propertyFilter.ToUpper()))).Select(p =>
                            new FilterValueDto
                                (p.Hardwaresolution ?? p.Hardwaresolutionreource?.Hardwaresolutionreource)).Distinct()
                        .ToList(),
                "description" => string.IsNullOrEmpty(propertyFilter) ?
                                query.Select(x => new FilterValueDto(x.Description)).Distinct().ToList()
                                : query.Where(x => x.Description.Contains(propertyFilter)).Select(x => new FilterValueDto(x.Description)).Distinct().ToList(),
                "platform" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    {
                        Text = p.Platform.Platform,
                        Value = p.Platformid.ToString()
                    }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Platform.Platform.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Platform.Platform,
                                    Value = p.Platformid.ToString()
                                }).Distinct().ToList(),

                "hardwareType" => string.IsNullOrEmpty(propertyFilter)
                     ? query.Select(p => new FilterValueDto
                     {
                         Text = p.Hardwaretype,
                         Value = p.Hardwaretype
                     }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.Hardwaretype.Contains(
                                propertyFilter)).Select(p => new FilterValueDto
                                {
                                    Text = p.Hardwaretype,
                                    Value = p.Hardwaretype
                                }).Distinct().ToList(),

                "majorHardwareBuildId" => string.IsNullOrEmpty(propertyFilter)
                         ? query.Select(p => new FilterValueDto
                         {
                             Text = p.Majorhardwareid.ToString(),
                             Value = p.Majorhardwareid.ToString()
                         }).Distinct().ToList()
                        : query
                            .Where(x =>
                                x.Majorhardwareid.ToString().Contains(
                                    propertyFilter)).Select(p => new FilterValueDto
                                    {
                                        Text = p.Majorhardwareid.ToString(),
                                        Value = p.Majorhardwareid.ToString()
                                    }).Distinct().ToList(),

                "otherHardwareInfo" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Otherhardwareinfo, Value = p.Otherhardwareinfo }).Distinct().ToList()
                    : query
                        .Where(x => x.Otherhardwareinfo.Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = p.Otherhardwareinfo, Value = p.Otherhardwareinfo })
                        .Distinct()
                        .ToList(),
                "vulnerabilityStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto
                    { Text = p.Vulnerabilitystatus, Value = p.Vulnerabilitystatus }).Distinct().ToList()
                    : query
                        .Where(x => x.Vulnerabilitystatus.Contains(propertyFilter)).Select(p =>
                            new FilterValueDto { Text = p.Vulnerabilitystatus, Value = p.Vulnerabilitystatus }).Distinct()
                        .ToList(),
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
                                    Value = p.Orgeqpmanufacturerid.ToString()
                                }).Distinct().ToList(),
                "buildConstruction" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto(p.Buildconstruction.Buildconstruction)).Distinct().ToList()
                : query
                    .Where(x =>
                        x.Buildconstruction.Buildconstruction.Contains(
                            propertyFilter))
                    .Select(p => new FilterValueDto(p.Buildconstruction.Buildconstruction))
                    .Distinct().ToList(),
                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationuserNavigation.Email.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.ModificationuserNavigation.Email)).Distinct().ToList(),
                "designContact" => string.IsNullOrEmpty(propertyFilter)
                        ? _commonManager.isDesignContactInOrganisation(query.SelectMany(x => x.Majorhwbuildsdesigncontacts.Where(x=>x.Deleted == false).Select(p => new FilterValueDto
                        {
                            Text = p.Designcontact.Email,
                            Value = p.Designcontactid.ToString()
                        })).ToList())?.Distinct().ToList()
                         .Concat(query.Where(x => x.Majorhwbuildsdesigncontacts != null && x.Majorhwbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList()
                        : _commonManager.isDesignContactInOrganisation(query.SelectMany(x => x.Majorhwbuildsdesigncontacts.Where(x=>x.Deleted == false).Select(p => new FilterValueDto
                        {
                            Text = p.Designcontact.Email,
                            Value = p.Designcontactid.ToString()
                        })).Distinct().ToList())?.Distinct().ToList()
                          .Concat(query.Where(x => x.Majorhwbuildsdesigncontacts != null && x.Majorhwbuildsdesigncontacts.Count() <= 0)
                       .Select(x =>

                          new FilterValueDto
                          {
                              Text = "---",
                              Value = "yes",
                          }
                       )).Distinct().ToList(),
                                _ => new List<FilterValueDto>(),
                            };
            return rtn;
        }

        public async Task<MajorHardwareBuildDtoCreate> GetCreatePage()
        {
            var resource = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Deleted != true);
            var hardwareSolutionReource = _repositoryWrapper.HardwareSolutionResource.FindAll();
            var buildConstructionResource = _repositoryWrapper.BuildConstruction.FindAll();
            var platformResource = _repositoryWrapper.Platform.FindAll();
            //var designContacts = _commonManager.GetActiveUserDetails(_repositoryWrapper.OrganisationRepository.FindByCondition(x => x.Isdesigncontact == true).Include(x => x.Contact).Select(x => x.Contactid).ToList()).Result;
            var designContacts = _repositoryWrapper.UserRepository.FindByCondition(x => x.Isdesigncontact == true).ToList();

            var model = new MajorHardwareBuildDtoCreate
            {
                OriginalEquipmentManufacturerResource = resource.ToDictionary(x => x.Orgeqpmanufacturerid,
                    x => x.Originalequipmentmanufacturer),
                HardwareSolutionReource = hardwareSolutionReource.ToDictionary(x => x.Hardwaresolutionresourceid,
                    x => x.Hardwaresolutionreource),
                BuildConstructionResource = buildConstructionResource.ToDictionary(x => x.Buildconstructionid,
                    x => x.Buildconstruction),
                PlatformResource = platformResource.ToDictionary(x => x.Platformid, x => x.Platform),
                EOMStatus =Enum.EOMEnum.NotSpecified,
                DesignContacts = designContacts.ToList().Where(x => x.Id != 1).DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email),
            };
            return model;
        }

        public async Task<MajorHardwareBuildDtoUpdate> GetUpdatePage(long id)
        {

            var entity = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == id, true)
                .Include(x => x.ModificationuserNavigation).Single();

            var dto = _mapper.Map<MajorHardwareBuildDtoUpdate>(MajorHardwareBuildMapper.GetMajorHardwareBuildMapper(entity));

            #region lookUp
            var resource = _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Deleted != true);
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

            var platformResource = _repositoryWrapper.Platform.FindAll();
            dto.PlatformResource = platformResource.ToDictionary(x => x.Platformid, x => x.Platform);
            if(dto.PlatformId != null && dto.PlatformId != 0)
            {
                if (!dto.PlatformResource.ContainsKey(dto.PlatformId.Value))
                {
                    var pl = _repositoryWrapper.Platform.FindByCondition(x => x.Platformid == dto.PlatformId, includeDeleted: true).SingleOrDefault();
                    if (pl != null)
                    {
                        dto.PlatformResource.Add(pl.Platformid, pl.Platform);
                    }
                }
            }
           
         
            var hardwareSolutionReource = _repositoryWrapper.HardwareSolutionResource.FindAll();
            dto.HardwareSolutionReource = hardwareSolutionReource.ToDictionary(x => x.Hardwaresolutionresourceid,
                x => x.Hardwaresolutionreource);

            if (dto.HardwareSolutionReourceId.HasValue && !dto.HardwareSolutionReource.ContainsKey(dto.HardwareSolutionReourceId.Value))
            {
                var oem = _repositoryWrapper.HardwareSolutionResource.FindByCondition(
                    x => x.Hardwaresolutionresourceid == dto.HardwareSolutionReourceId,
                    includeDeleted: true).SingleOrDefault();
                if (oem != null)
                {
                    dto.HardwareSolutionReource.Add(oem.Hardwaresolutionresourceid, oem.Hardwaresolutionreource);
                }
            }
            var buildConstructionResource = _repositoryWrapper.BuildConstruction.FindAll();
            dto.BuildConstructionResource = buildConstructionResource.ToDictionary(x => x.Buildconstructionid,
                    x => x.Buildconstruction);
            if (dto.BuildConstructionId.HasValue && !dto.BuildConstructionResource.ContainsKey(dto.BuildConstructionId.Value))
            {
                var oem = _repositoryWrapper.BuildConstruction.FindByCondition(
                    x => x.Buildconstructionid == dto.BuildConstructionId,
                    includeDeleted: true).SingleOrDefault();
                if (oem != null)
                {
                    dto.BuildConstructionResource.Add(oem.Buildconstructionid, oem.Buildconstruction);
                }
            }
            dto.DesignContactIds = _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorhardwarebuildsid == entity.Majorhardwareid).Select(x => x.Designcontactid).AsEnumerable();
            var designContacts = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Isdesigncontact == true).ToListAsync();
            dto.DesignContacts = designContacts.ToList().DistinctBy(x => x.Id).ToDictionary(x => x.Id, x => x.Email);
 
            #endregion


            return dto;
        }

        public async Task<ResultDto<ResultDataRemediationDto>> ApplyDataRemediation(DataRemediationDto data)
        {
            List<long> systemTypesIds = new List<long>();
            var existsRelationWithStCorrect = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == data.CorrectId, true).ToList();

            foreach (var item in data.DuplicatesId)
            {
                //Get delle relazioni con systemtype
                var systemTypesWithDuplicates = _repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == item, true, false).ToList();
                var systemTypesWithDuplicatesAttivi = new List<SystemTypesMajorHardwareBuild>(systemTypesWithDuplicates.Where(x => x.Deleted.Value == false).Select(p=> SystemTypesMajorHardwareBuildMapper.GetSystemTypesMajorHardwareBuildMapper(p)));
                var systemTypeToDeleted = systemTypesWithDuplicates;

                ////Rimuovi relazioni
                foreach (var toDelete in systemTypeToDeleted)
                {
                    _repositoryWrapper.SystemTypesMajorHardwareBuild.DeleteDeep(toDelete);
                    _repositoryWrapper.Save();
                }

                systemTypesIds.AddRange(systemTypesWithDuplicatesAttivi.Select(x => x.SystemTypeId));

                foreach (var st in systemTypesWithDuplicatesAttivi)
                {
                    st.MajorHardwareId = data.CorrectId;
                    var entityExists = existsRelationWithStCorrect.SingleOrDefault(str => str.Systemtypeid == st.SystemTypeId);
                    if (entityExists == null)
                    {
                        if (_repositoryWrapper.SystemTypesMajorHardwareBuild.FindByCondition(x => x.Systemtypeid == st.SystemTypeId && x.Ismain).Count() > 0) st.IsMain = false;
                        _repositoryWrapper.SystemTypesMajorHardwareBuild.Create(SystemTypesMajorHardwareBuildMapper.SetSystemTypesMajorHardwareBuildMapper(st));
                    }
                    _repositoryWrapper.Save();
                }

                //cancello i duplicati

                var hw = _repositoryWrapper.MajorHardwareBuild.FindByCondition(x => x.Majorhardwareid == item, true).Single();
                _repositoryWrapper.MajorHardwareBuild.DeleteDeep(hw);
                _repositoryWrapper.Save();

            }
            //ricalcolo i valori di systemType
            foreach (var id in systemTypesIds)
            {
                await this.Test(id);

            }
            //foreach (var id in systemTypesIds)
            //{
            //    var systemTypes = _repositoryWrapper.SystemType.FindByCondition(x =>
            //        x.SystemTypeId == id)
            //        .Include(x => x.SystemTypesMajorHardwareBuilds).ThenInclude(x => x.MajorHardware)
            //        .Include(x => x.MajorSoftwareBuilds).ToList();

            //    foreach (var st in systemTypes)
            //    {
            //       var updated = _systemTypeManager.SetSystemTypeValue(st);
            //        _repositoryWrapper.SystemType.Update(updated);
            //    }

            //    await _repositoryWrapper.SaveAsync();
            var sistemTypeIds = _repositoryWrapper.SystemTypesMajorHardwareBuild
                .FindByCondition(x => x.Ismain && !x.Deleted.Value && x.Majorhardwareid == data.CorrectId)
                .Select(x => x.Systemtypeid).ToList();
            foreach (long pp in systemTypesIds)
            {
                var lista = new List<long>();
                lista.Add(pp);
                await _plannedActivityCommon.ReloadPlannedActivity(lista);
            }
            return new ResultDto<ResultDataRemediationDto>()
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Warning = false,
                Data = new ResultDataRemediationDto() { Id = systemTypesIds }
            };

        }

        private async Task Test(long id)
        {
            var systemTypes = _repositoryWrapper.SystemType.FindByCondition(x =>
                    x.Systemtypeid == id, false, false)
                    .Include(x => x.Systemtypesmajorhardwarebuilds)
                    .ThenInclude(x => x.Majorhardware)
                    .Include(x => x.Majorsoftwarebuilds).Single();

            systemTypes.Constraintlcm = systemTypes.GetStatusName(_repositoryWrapper);
            systemTypes.Constraintscaling = systemTypes.GetMinorDate(_repositoryWrapper);

            var result = systemTypes.GetMinorDateEOM(_repositoryWrapper);
            systemTypes.Endofmaintenance = result == "Not Announced" || result == "Not Specified" ? null : (DateTime?)Convert.ToDateTime(result);


            _repositoryWrapper.SystemType.Update(systemTypes);
            await _repositoryWrapper.SaveAsync();
        }

        public async Task<ResultDto> AddOrUpdateDesignContact(IEnumerable<int> dto, long MhbId)
        {

            try
            {
                if (dto != null)
                {
                    long majorHardwareBuildId = MhbId;
                    var recordExistBasedMajorHWBuildId = _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.FindByCondition(x => x.Majorhardwarebuildsid == (long)majorHardwareBuildId);

                    #region DesignContactRecord
                    foreach (var Item in dto)
                    {
                        var getExistRecords = recordExistBasedMajorHWBuildId.Where(x => x.Designcontactid == Item).ToList();

                        if (getExistRecords.Count == 0)
                        {
                            _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.Create(new Majorhwbuildsdesigncontacts
                            {
                                Majorhardwarebuildsid = majorHardwareBuildId,
                                Designcontactid = Item,
                            });
                        }
                    }

                    await _repositoryWrapper.SaveAsync();

                    var designContactDeleteRecord = recordExistBasedMajorHWBuildId;
                    foreach (var item in designContactDeleteRecord)
                    {
                        if (!dto.Where(x => x == item.Designcontactid).Any())
                        {
                            _repositoryWrapper.MajorHwBuidlsDesignContactsRepository.DeleteDeep(item);
                        }
                    }

                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();

                    #endregion

                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };
        }


    }
}