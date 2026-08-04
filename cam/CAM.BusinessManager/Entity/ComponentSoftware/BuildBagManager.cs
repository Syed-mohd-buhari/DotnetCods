using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.ComponentBag;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.ComponentSoftware;
using CAM.Entities.Mappers.Cross;
using CAM.Entities.Models;
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

namespace CAM.BusinessManager.Entity.ComponentSoftware
{
    public class BuildBagManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly ComponentSoftwareBuildBagMappingManager _mappingSwBuildBagManager;
        private readonly DesignComponentFamilyLifeCycleManager _designComponentFamilyLifeCycleManager;
        public BuildBagManager(IEnumerable<IRepositoryWrapper> wrappers,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            IProductNameManager productNameManager,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
          ComponentSoftwareBuildBagMappingManager mappingSwBuildBagManager,
            CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager, DesignComponentFamilyLifeCycleManager designComponentFamilyLifeCycleManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _mappingSwBuildBagManager = mappingSwBuildBagManager;
            _designComponentFamilyLifeCycleManager = designComponentFamilyLifeCycleManager;
        }

        #region Grid Load, Filter 
        private IQueryable<Buildbags> GetBuildBagRecords(ExpressionStarter<Buildbags> predicateResult)
        {
            var query = _repositoryWrapper.BuildBagRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Opco).Include(x => x.Designcomponentfamily)
                 .Include(x => x.Componentsoftwarebuildbags).ThenInclude(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer)
                  .Include(x => x.Lcmengineering)
                  .AsQueryable();

            return query;
        }

      
        public async Task<QueryResultDto<BuildBagDtoGrid>> FindWithCondition(BuildBagQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }


            var buildBagQuery = await Task.Run(() => GetBuildBagRecords(predicateResult).AsEnumerable()
                         .Select(p => BuildBagMapper.GetBuildBag(p)).AsQueryable());

            if (buildFilterDto.AssociatedWithLcm?.Any() == true)
            {
                if (buildFilterDto.AssociatedWithLcm.Contains(ConstantValueFilter.yes))
                    buildBagQuery = buildBagQuery.Where(x => x.LcmEngineering.Any());
                else if (buildFilterDto.AssociatedWithLcm.Contains(ConstantValueFilter.no))
                    buildBagQuery = buildBagQuery.Where(x => !x.LcmEngineering.Any());
            }
            var totalCount = buildBagQuery.Count();

            var paginatedRecords = await Task.Run(() => buildBagQuery.ApplyOrdering(buildFilterDto, GetColumnsMap())
               .ApplyPaging(buildFilterDto).ToList());

            var mappedRecords = await Task.Run(() => paginatedRecords.Select(x => new BuildBagDtoGrid
            {
                BuildBagId = x.BuildBagId.ToString(),
                BuildBagDescription = _commonManager.GetBuildBagDescriptionFromEnity(x),
                DesignComponentFamilyName = x.DesignComponentFamily.DCFName(_repositoryWrapper),
                OpCo = x.OpCo.OpCoDescription,
                LastModifiedBy = x.ModificationuserNavigation.Email,
                LastModified = x.ModificationDate,
                Deleted = x.Deleted,
                MappedComponentSoftwareBuild = x.MappingComponetSoftwareDescription.Any()
        ? string.Join(",", x.MappingComponetSoftwareDescription.Select(t => t.Value))
        : string.Empty,
                AssociatedWithLcm = x.LcmEngineering.Any() ? ConstantValueFilter.yes : ConstantValueFilter.no,
                VisibleFlag=x.VisibleFlag==true? ConstantValueFilter.yes : ConstantValueFilter.no

            }).ToList());

            return new QueryResultDto<BuildBagDtoGrid>(
                new GenerateRenderForGrid<BuildBagDtoGrid>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = mappedRecords.ToArray()
            };
        }

        public ExpressionStarter<Buildbags> ApplyFilter(BuildBagQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Buildbags>(true);

            var visibleFlagPredicate = PredicateBuilder.New<Buildbags>();
            visibleFlagPredicate.Or(x => x.Visibleflag == true);
            mainPredicate.And(visibleFlagPredicate);

            if (filterDto.BuildBagId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Buildbags>();
                foreach (var id in filterDto.BuildBagId)
                    componentIdPredicate.Or(x => x.Buildbagid == id);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.BuildBagDescription?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Buildbags>();
                foreach (var description in filterDto.BuildBagDescription)
                    descriptionPredicate.Or(x => x.Bagdescription == description);

                mainPredicate.And(descriptionPredicate);
            }


            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Buildbags>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }

            if (filterDto.AssociatedWithLcm?.Any() == true)
            {
                var associatedWithLcmPredicate = PredicateBuilder.New<Buildbags>();
                foreach (var item in filterDto.AssociatedWithLcm)
                {
                    if (item == ConstantValueFilter.yes)
                        associatedWithLcmPredicate.Or(x => x.Lcmengineering.Any(y => y.Buildbagid != 0 && y.Buildbagid != 0));
                    else
                        associatedWithLcmPredicate.Or(x => x.Lcmengineering.Any(y => y.Buildbagid == 0 || y.Buildbagid == 0));
                }

                mainPredicate.And(associatedWithLcmPredicate);
            }
            if (filterDto.MappedComponentSoftwareBuild?.Any() == true)
            {
                var componentSoftware = PredicateBuilder.New<Buildbags>();
                foreach (var item in filterDto.MappedComponentSoftwareBuild)
                    componentSoftware.Or(x => x.Componentsoftwarebuildbags.Any(s=>s.Componentsoftwarebuildid == item));

                mainPredicate.And(componentSoftware);
            }
            if (filterDto.Opco?.Any() == true)
            {
                var opCo = PredicateBuilder.New<Buildbags>();
                foreach (var item in filterDto.Opco)
                    opCo.Or(x => x.Opcoid == item);

                mainPredicate.And(opCo);
            }
            if (filterDto.Dcf?.Any() == true)
            {
                var dcf = PredicateBuilder.New<Buildbags>();
                foreach (var item in filterDto.Dcf)
                    dcf.Or(x => x.Designcomponentfamilyid == item);

                mainPredicate.And(dcf);
            }
            return mainPredicate;
        }

        private Dictionary<string, Expression<Func<BuildBag, object>>[]> GetColumnsMap()
        {
            var temp = new Dictionary<string, Expression<Func<BuildBag, object>>[]>
            {
                ["buildBagId"] = new Expression<Func<BuildBag, object>>[] { p => p.BuildBagId },
                ["bagDescription"] = new Expression<Func<BuildBag, object>>[] { p => p.BagDescription },
                ["lastModifiedBy"] = new Expression<Func<BuildBag, object>>[] { p => p.ModificationUserEntity.Email },
                ["opCo"] = new Expression<Func<BuildBag, object>>[] { p => p.OpCo.OpCoDescription },
                ["designComponentFamily"] = new Expression<Func<BuildBag, object>>[] { p => p.DesignComponentFamily.toDesignComponentFamilyName(_repositoryWrapper) },


            };

            return temp;
        }

        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(
        string propertyName, string propertyFilter, BuildBagQueryDto filterDto)
        {
            var filterCriteria = ApplyFilter(filterDto);

            var filteredQuery = GetBuildBagRecords(filterCriteria);

            var result = propertyName switch
            {
                "buildBagId" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Buildbagid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Buildbagid))
                    .Distinct()
                    .ToListAsync(),
                "buildBagDescription" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Bagdescription.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text =_commonManager.GetBuildBagDescription(p), Value = p.Bagdescription })
                    .Distinct()
                    .ToListAsync(),

                "opCo" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Opcoid.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Opco.Opco, Value = p.Opcoid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "designComponentFamilyName" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Designcomponentfamilyid.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Designcomponentfamily.DCFName(_repositoryWrapper), Value = p.Designcomponentfamilyid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "lastModifiedBy" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationuserNavigation.Email.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.ModificationuserNavigation.Email))
                    .Distinct()
                    .ToListAsync(),

                "associatedWithLcm" => await filteredQuery
                                                 .Select(p => new FilterValueDto(
                                                     (p.Lcmengineering.Any()) ? ConstantValueFilter.yes : ConstantValueFilter.no

                                                     ))
                                                 .Distinct()
                                                 .ToListAsync(),
                "mappedComponentSoftwareBuild" => filteredQuery.ToList()           
                                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Componentsoftwarebuildbags.Any(x => x.Componentsoftwarebuildid.ToString().Contains(propertyFilter) == true))
                                   .SelectMany(x => x.Componentsoftwarebuildbags.Select(r=>new FilterValueDto 
                                   {Text = ComponentBagExtensionMethod.GetComponentBagDescription(_repositoryWrapper, r.Componentsoftwarebuildid)
                                   ,Value = r.Componentsoftwarebuildid.ToString()}))
                                   .Distinct()
                                   .ToList(),
                 "visibleFlag"=>filteredQuery.ToList().Select(p => new FilterValueDto(p.Visibleflag==true? ConstantValueFilter.Yes : ConstantValueFilter.No))
                                    .Distinct()
                                    .ToList(),

                _ => new List<FilterValueDto>(),
            };

            return result;
        }


        #endregion

        #region Create and Add Build Bag

        public async Task<BuildBagCreatePageDto> GetCreateBuildBagPageDetailsAsync(List<short> opcoList)
        {           

            var dcfResources = await _dropdownDataServiceManager.GetDCFDetails();
            var opCoResources = await _dropdownDataServiceManager.GetOpcoDetails(0,false,opcoList);
            var componenetSwBuild = await _dropdownDataServiceManager.GetComponenetSoftwareDetails();
            var model = new BuildBagCreatePageDto
            {

                DcfResource = dcfResources,
                OpcoResource = opCoResources,
                ComponentSofwareBuild = componenetSwBuild
            };

            return model;


        }

        public async Task<BuildBag> EntityExists(BuildBagCreatePageDto dto)
        {
            var entityExists = await _repositoryWrapper.BuildBagRepository.FindByCondition(
                    x => x.Bagdescription.ToLower().Replace(" ", "").Replace(".0", "")
                    == dto.BuildBagDescription.ToLower().Replace(" ", "").Replace(".0", ""), true)
                .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();
            return BuildBagMapper.GetBuildBag(entityExists);
        }

        public async Task<string> GenerateBagVersion(string bagDescription)
        {
            var entityExists = await Task.Run(() => _repositoryWrapper.BuildBagRepository
                         .FindByCondition(x => x.Bagdescription.ToLower().Replace(" ", "").Replace(".0", "") == bagDescription.ToLower().Replace(" ", "").Replace(".0", ""), true).AsEnumerable()
                         .Select(x => x.Bagversion.Replace(" ", "").Replace(".0", "")).ToList());

            string MaxBagVersion = (entityExists.Any() ? (entityExists.Max(x => Convert.ToInt16(x)) + 1) : "1") + ".0";


            return MaxBagVersion;
        }

        public async Task<ResultDto> AddBuildBagAsync(BuildBagCreatePageDto dto, bool? forced = false)
        {
            var entityExists = await EntityExists(dto);

            if (entityExists != null)
            {
                try
                {

                    if (entityExists.Deleted == true)
                    {

                        return (forced == true) ?
                             await AddBaseAsync(dto)
                            :
                          new ResultDto
                          {
                              Warning = true,
                              Info = ResultMessages.EntryAddExists,
                              Data = new { id = entityExists.BuildBagId, orphanDeleted = true }
                          };

                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = entityExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entityExists.BuildBagId
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }


            }
            return await AddBaseAsync(dto);
        }
        public async Task<ResultDto> AddBaseAsync(BuildBagCreatePageDto dto)
        {
            Buildbags result;
            try
            {
                dto.BagVersion = await GenerateBagVersion(dto.BuildBagDescription);


                var entityForced = _mapper.Map<BuildBag>(dto);
                entityForced.VisibleFlag = true;
                result = BuildBagMapper.SetBuildBag(entityForced);
                
                _repositoryWrapper.BuildBagRepository.Create(result);
                await _repositoryWrapper.SaveAsync();


                if (dto.ComponentSoftwareId?.Any() == true && result.Buildbagid != 0)
                {
                    await _mappingSwBuildBagManager.AddOrUpdateComponenetSoftwareBuildBagAsync(dto.ComponentSoftwareId, result.Buildbagid);

                    if (dto.LcmEngineeringId != 0)
                    {
                        var lcmEntity = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Lcmengineeringid == dto.LcmEngineeringId).FirstOrDefault();
                        var existingBagId = lcmEntity.Buildbagid;
                        if (lcmEntity != null)
                        {
                            lcmEntity.Buildbagid = result.Buildbagid;
                            _repositoryWrapper.Lcmengineering.Update(lcmEntity);
                            await _repositoryWrapper.SaveAsync();

                            var dcfId = _repositoryWrapper.DesignComponent.FindByCondition(x => x.Designcomponentid == lcmEntity.Designcomponentid).FirstOrDefault().Designcomponentfamilyid;
                            await _designComponentFamilyLifeCycleManager.setResourceKeyNotInUseAndAddDcfLifeCycleEntry
                                        ((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid, (long)dcfId, 1, existingBagId);

                            string dcfElementNAme = ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 1).Text;
                            await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforComponent((long)lcmEntity.Opcoid, lcmEntity.Designcomponentid, result.Buildbagid);


                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = result.Buildbagid };
        }

        #endregion

        #region View Componenet Software       
        public async Task<ViewBagandComponenetDto> ViewBagAndComponentDetailsAsync(long id)
        {            
            var ComponentBuildBagViewPageDto = await _commonManager.ViewBagAndComponentDetailsAsync(id);  

            return ComponentBuildBagViewPageDto;
        }
        #endregion

        #region Edit and Update Componenet Software       
        public async Task<BuildBagEditPageDto> GetEditedBuildBagPageDetailsAsync(long id)
        {
            var mainPredicate = PredicateBuilder.New<Buildbags>(x => x.Buildbagid == id);

            var existingBuildBagEntity = await Task.Run(() => GetBuildBagRecords(mainPredicate).FirstOrDefault());
            var ComponentBuildBagEditPageDto = await _mappingSwBuildBagManager.GetEditAndUpgradeMappingPageDetailsAsync(id);

            var dto = _mapper.Map<BuildBagEditPageDto>(existingBuildBagEntity);

            if (existingBuildBagEntity != null)
            {
                dto.componentBuildBagEditPageDto = ComponentBuildBagEditPageDto;
                dto.BuildBagId = existingBuildBagEntity.Buildbagid;
                dto.BuildBagDescription = existingBuildBagEntity.Bagdescription;/// $"{existingBuildBagEntity.Bagdescription}-{existingBuildBagEntity.Bagversion}";
                dto.BagVersion = existingBuildBagEntity.Bagversion;
                dto.DesignComponentFamilyId = existingBuildBagEntity.Designcomponentfamilyid.Value;
                dto.OpCoId = existingBuildBagEntity.Opcoid.Value;
                dto.DcfResource = await _dropdownDataServiceManager.GetDCFDetails(existingBuildBagEntity.Designcomponentfamilyid.Value, true);
                dto.OpcoResource = await _dropdownDataServiceManager.GetOpcoDetails(existingBuildBagEntity.Opcoid.Value, true);

            }

            return dto;
        }
        public async Task<ResultDto> UpdateBuildBagAsync(BuildBagUpdateDto dto, bool? forced)
        {
            var currentBuildBag = _repositoryWrapper.BuildBagRepository.FindByCondition(x => x.Buildbagid == dto.BuildBagId).FirstOrDefault();

            if (currentBuildBag == null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryNotFound,
                    Data = new { id = dto.BuildBagId } // currentBuildBag is null, so use dto.BuildBagId
                };
            }

            var cleanedDescription = dto.BuildBagDescription?.ToLower().Replace(" ", "").Replace(".0", "");
            var duplicateBuildBagEntry = _repositoryWrapper.BuildBagRepository.FindByCondition(
                x => x.Bagdescription.ToLower().Replace(" ", "").Replace(".0", "") == cleanedDescription &&
                     x.Buildbagid != currentBuildBag.Buildbagid &&
                     x.Deleted == false &&
                     x.Bagversion == currentBuildBag.Bagversion,
                true).FirstOrDefault();

            if (duplicateBuildBagEntry != null)
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = new { id = duplicateBuildBagEntry.Buildbagid }
                };
            }

            dto.BagVersion = currentBuildBag.Bagversion;

            return ((currentBuildBag.Deleted == true && (forced ?? false)) || currentBuildBag.Deleted == false)
                ? await UpdateBaseAsync(dto, forced)
                : new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryUpdateExists,
                    Data = new { id = currentBuildBag.Buildbagid, orphanDeleted = true }
                };

        }

        public async Task<ResultDto> UpdateBaseAsync(BuildBagUpdateDto dto, bool? forced)
        {
            var existedComponents = await _designComponentFamilyLifeCycleManager.GetBagMappedComponenet(dto.BuildBagId); 
            var updateBuildBag = await UpdateSoftwareBuildEntityAsync(dto, forced);

            var compIds = existedComponents.Select(x => x.Componentsoftwarebuildid.ToString()).ToList();

            if (dto.ComponentSoftwareId.Any() == true)
            {
                await _mappingSwBuildBagManager.AddOrUpdateComponenetSoftwareBuildBagAsync(dto.ComponentSoftwareId, dto.BuildBagId);
            }

            #region lcm associated on add/delete component 

            var lcmEntity = await _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Buildbagid == dto.BuildBagId)
                            .Include(x=>x.Designcomponent)
                            .Select(a => new {opcoID=a.Opcoid,dcId=a.Designcomponentid,dcfId=a.Designcomponentfamilyid})
                            .ToListAsync();
            List<string> updateComponentIds = dto.ComponentSoftwareId.ConvertAll(a => a.ToString());
            

            if (lcmEntity!=null &&lcmEntity.Count() > 0)
            {
                List<long> addNewComponent = new List<long>();
                List<long> deleteExistComponent = new List<long>();

                if (compIds != null && compIds.Count() > 0 && updateComponentIds != null && updateComponentIds.Count() > 0)
                {
                    deleteExistComponent = compIds.Where(a => !updateComponentIds.Contains(a)).Select(x=>Convert.ToInt64(x)).ToList();
                    addNewComponent = updateComponentIds.Where(a => !compIds.Contains(a)).Select(x => Convert.ToInt64(x)).ToList();
                }
                else 
                {
                    if (updateComponentIds != null && updateComponentIds.Count() > 0)
                    {
                        addNewComponent = dto.ComponentSoftwareId;
                    }
                    if(compIds != null && compIds.Count() > 0)
                    {
                        deleteExistComponent = compIds.ConvertAll(a => Convert.ToInt64(a));
                    }
                }

                foreach (var lcmEntityToUpdate in lcmEntity)
                {
                    if (addNewComponent != null && addNewComponent.Count() > 0)
                    {
                        await _designComponentFamilyLifeCycleManager.InitialiseDCFLifecycleforComponent
                            ((long)lcmEntityToUpdate.opcoID, lcmEntityToUpdate.dcId, dto.BuildBagId,false,addNewComponent);
                    }
                    if (deleteExistComponent != null && deleteExistComponent.Count() > 0)
                    {
                        await _designComponentFamilyLifeCycleManager.GenerateDCFEntryForComponenet((long)lcmEntityToUpdate.opcoID,
                            lcmEntityToUpdate.dcId, (long)lcmEntityToUpdate.dcfId, dto.BuildBagId, 1, ConstantValueFilter.componentDcfEventDeatil.FirstOrDefault(x => x.Key == 3).Text, true,false, existedComponents, deleteExistComponent);

                    }
                }


            }
            #endregion
            
            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = updateBuildBag.BuildBagId
            };
        }

        public async Task<BuildBag> UpdateSoftwareBuildEntityAsync(BuildBagUpdateDto dto, bool? forced = false)
        {
            var buildBagEntity = _mapper.Map<BuildBag>(dto);

            if (forced ?? false)
            {
                buildBagEntity.Deleted = false;
                buildBagEntity.DeletionDate = null;
            }
            buildBagEntity.VisibleFlag = true;
            _repositoryWrapper.BuildBagRepository.Update(
                BuildBagMapper.SetBuildBag(buildBagEntity));

            await _repositoryWrapper.SaveAsync();
            return buildBagEntity;
        }
        #endregion


        #region  Upgrade Bag and Mapping ComponenetSw
        public async Task<BuildBagEditPageDto> GetUpgradeResourceAsync(long bagId)
        {

            var upgradeBagComponent = await GetEditedBuildBagPageDetailsAsync(bagId);
            return upgradeBagComponent;
        }

        public async Task<ResultDto> UpgradeBagAndComponenetSoftwareBuildBagAsync(BuildBagCreatePageDto dto)
        {
            var newlyInsertedBagId = new ResultDto();
            try
            {
                //string bagDescription = dto.BuildBagDescription.Substring(0, dto.BuildBagDescription.LastIndexOf("-"));
                //dto.BuildBagDescription = bagDescription;

                await AddBaseAsync(dto);  // No need to check Duplicate Gag Name 
                await _repositoryWrapper.ClearTracker();

            }
            catch (Exception)
            {
                throw;
            }

            return newlyInsertedBagId.Warning == true ? newlyInsertedBagId : new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        #endregion

        #region Delete , DeleteDeep , Refered table Details

        public async Task<ResultDto> GetReferenceRecordAsync(long id)
        {
            var resultMessages = new List<ResultMessageDto>();

            var buildBagEntity = await _repositoryWrapper.BuildBagRepository
                .FindByCondition(x => x.Buildbagid == id)
                .SingleAsync();

            var excludedTables = new List<string> { "Componentsoftwarebuildbags" };// "Componentsoftwarebuildsdesigncontacts", "Lcmengineering", "Plannedactivities", "Networkelementsasplanned" };

            var referencedRecords = await _commonManager.GetForeignKeyRefernceTable(
                "Buildbags", id, excludedTables);

            if (referencedRecords != null && referencedRecords.Any())
            {
                resultMessages.Add(new ResultMessageDto
                {
                    Table = _commonManager.popupTabName,
                    Values = referencedRecords.ToArray()
                });
            }

            if (resultMessages.Any())
            {
                return new ResultDto
                {
                    Warning = true,
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = new RelatedRecordsResultDto
                    {
                        EntityName = "Build Bag",
                        RecordName = $"{buildBagEntity.Bagdescription} ",
                        DataRelatedList = resultMessages
                    }
                };
            }

            return new ResultDto();


        }

        public async Task<ResultDto> DeleteComponenetBuildBag(long id)
        {

            #region  
            var softwareBuildBag = _repositoryWrapper.ComponentSoftwareBuildBagRepository.FindByCondition(x => x.Buildbagid == id);

            foreach (var toDelete in softwareBuildBag)
            {
                _repositoryWrapper.ComponentSoftwareBuildBagRepository.DeleteDeep(toDelete);
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();
            }

            #endregion
            var existsComponenetSwBuild = await _repositoryWrapper.BuildBagRepository.FindByCondition(x => x.Buildbagid == id).SingleAsync();

            _repositoryWrapper.BuildBagRepository.Delete(existsComponenetSwBuild);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = existsComponenetSwBuild.Buildbagid
            };
        }

        #endregion


        #region // drop down for Buildbag from LCM Screen
        public async Task<List<ViewBagandComponenetDto>> GetOpcoDcfBasedBag(long dcId,short opCoId,long lcmBagId)
        {
            var result = await _commonManager.GetBagAndComponentDetailsForDropdownAsync(0, false, true, dcId, opCoId,lcmBagId);

            return result;
        }
        #endregion
    }

}