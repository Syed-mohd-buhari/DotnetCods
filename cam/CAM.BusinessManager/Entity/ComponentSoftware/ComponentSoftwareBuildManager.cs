using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.BusinessManager.LookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.ComponentSoftware;
using CAM.Entities.Mappers.Entity;
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
    public class ComponentSoftwareBuildManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager; 
        protected readonly ILoggerManager _logger;      
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        private DesignComponentFamilyLifeCycleManager _designComponentFamilyLifeCycleManager;

        public ComponentSoftwareBuildManager(IEnumerable<IRepositoryWrapper> wrappers,           
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            IProductNameManager productNameManager,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            NetworkFunctionManager networkFunctionManager,
           DesignComponentFamilyLifeCycleManager designComponentLIfecycleManager,
        CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;  
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _designComponentFamilyLifeCycleManager = designComponentLIfecycleManager;
        }

        #region Grid Load, Filter 
        private IQueryable<Componentsoftwarebuilds>  GetComponetSoftwareBuildRecords (ExpressionStarter<Componentsoftwarebuilds> predicateResult)
        {
            var query =   _repositoryWrapper.ComponentSoftwareBuildRepository.FindByCondition(predicateResult)
                         .Include(x => x.Componentmanufacturer)
                         .Include(x => x.Operatingsystem)
                         .Include(x => x.Criticalassettype)
                         .Include(x => x.CreationuserNavigation)
                         .Include(x => x.ModificationuserNavigation) 
                         .Include(x => x.Componentsoftwarebuildsdesigncontacts).ThenInclude(x => x.Designcontact)
                        // .Where(predicateResult)
                          .AsQueryable();

            return query;
        }
        
        public async Task<QueryResultDto<ComponentSoftwareBuildDtoGrid>> FindWithCondition(ComponentSoftwareBuildQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }
            predicateResult = predicateResult.And(x => x.Softwareversion.ToLower() != ConstantValueFilter.Unknown);

            var query = await Task.Run(() => GetComponetSoftwareBuildRecords(predicateResult).AsEnumerable()
                         .Select(p => ComponentSoftwareBuildMapper.GetComponentSoftwareBuildMapper(p)).AsQueryable()); 


            var totalCount = query.Count();

            var paginatedQuery = await Task.Run(() => query.ApplyOrdering(buildFilterDto, GetColumnsMap()).ApplyPaging(buildFilterDto).ToList());
 

            if (buildFilterDto.PrincipalId != 0)
            {
                var exists = paginatedQuery.Any(x => x.ComponentSoftwareBuildId == buildFilterDto.PrincipalId);
                if (!exists)
                {
                    var additionalResource = await _repositoryWrapper.ComponentSoftwareBuildRepository.FindAll(true)
                        .Include(x => x.Componentmanufacturer)
                        .Include(x => x.Operatingsystem)
                        .Include(x => x.Criticalassettype)
                        .Include(x => x.CreationuserNavigation)
                        .Include(x => x.ModificationuserNavigation)                       
                        .Include(x => x.Componentsoftwarebuildsdesigncontacts).ThenInclude(x => x.Designcontact)
                        .FirstOrDefaultAsync(x => x.Componentsoftwarebuildid == buildFilterDto.PrincipalId);

                    if (additionalResource != null)
                    {
                        paginatedQuery.Add(ComponentSoftwareBuildMapper.GetComponentSoftwareBuildMapper(additionalResource));
                    }
                }
            }

            var mappedData = _mapper.Map<IEnumerable<ComponentSoftwareBuildDtoGrid>>(paginatedQuery);

            return new QueryResultDto<ComponentSoftwareBuildDtoGrid>(
                new GenerateRenderForGrid<ComponentSoftwareBuildQueryDto>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = mappedData.ToArray()
            };
        }

        public ExpressionStarter<Componentsoftwarebuilds> ApplyFilter(ComponentSoftwareBuildQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Componentsoftwarebuilds>(true);

            if (filterDto.ComponentSoftwareBuildId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Componentsoftwarebuilds>();
                foreach (var id in filterDto.ComponentSoftwareBuildId)
                    componentIdPredicate.Or(x => x.Componentsoftwarebuildid == id);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.Description?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Componentsoftwarebuilds>();
                foreach (var description in filterDto.Description)
                    descriptionPredicate.Or(x => x.Description == description);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.ComponentManufacturers?.Any() == true)
            {
                var manufacturerPredicate = PredicateBuilder.New<Componentsoftwarebuilds>();
                foreach (var manufacturerId in filterDto.ComponentManufacturers)
                    manufacturerPredicate.Or(x => x.Componentmanufacturerid == manufacturerId);

                mainPredicate.And(manufacturerPredicate);
            }
           
            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Componentsoftwarebuilds>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }

            if (filterDto.EndOfMaintenanceValue != null)
            {
                var maintenancePredicate = PredicateBuilder.New<Componentsoftwarebuilds>();

                if (filterDto.EndOfMaintenanceValue.StartDate != null)
                    maintenancePredicate.And(x => x.Endofmaintenance >= filterDto.EndOfMaintenanceValue.StartDate);

                if (filterDto.EndOfMaintenanceValue.EndDate != null)
                    maintenancePredicate.And(x => x.Endofmaintenance <= filterDto.EndOfMaintenanceValue.EndDate);

                mainPredicate.And(maintenancePredicate);
            }

            if (filterDto.SoftwareVersion?.Any() == true)
            {
                var softwareVersionPredicate = PredicateBuilder.New<Componentsoftwarebuilds>();
                foreach (var version in filterDto.SoftwareVersion)
                    softwareVersionPredicate.Or(x => x.Softwareversion == version);

                mainPredicate.And(softwareVersionPredicate);
            }
            if (filterDto.DesignContact != null && filterDto.DesignContact.Any())
            {
                var designContactPredicate = PredicateBuilder.New<Componentsoftwarebuilds>();
                foreach (var item in filterDto.DesignContact)
                    if (item == "yes")
                    {
                        designContactPredicate.Or(x => x.Componentsoftwarebuildsdesigncontacts != null && !x.Componentsoftwarebuildsdesigncontacts.Any());
                    }
                    else
                    {
                        designContactPredicate.Or(x => x.Componentsoftwarebuildsdesigncontacts.Any(t => t.Designcontactid.ToString() == item));
                    }
                mainPredicate.And(designContactPredicate);

            }

            return mainPredicate;
        }

        private Dictionary<string, Expression<Func<ComponentSoftwareBuild, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ComponentSoftwareBuild, object>>[]>
            {
                ["componentSoftwareBuildId"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.ComponentSoftwareBuildId },
                ["componentmanufacturerid"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.Componentmanufacturerid },
                ["description"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.Description },
                ["softwareVersion"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.SoftwareVersion },
                ["lastTimeBuyNew"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.LastTimeBuyNew },
                ["lastModifiedValue"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.ModificationDate },
                ["lastTimeBuyUpgrades"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.LastTimeBuyUpgrades },
                ["lastTimeBuyExpansions"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.LastTimeBuyExpansions },
                ["endOfMaintenanceValue"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.EndOfMaintenance },
                ["endOfsupportValue"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.EndOfsupport },
                ["generaAvailableDateValue"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.GeneraAvailableDate },
                ["criticalAssetType"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.CriticalAssetType != null ? p.CriticalAssetType.Description
                : string.Empty },

                ["deliveryMethod"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.DeliveryMethod },
                ["vulnerabilityStatus"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.VulnerabilityStatus },
                ["spareFieldsJson"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.SpareFieldsJson },                
                ["operatingSystem"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.OperatingSystem != null ? p.OperatingSystem.OperatingSystemName : "" },
                ["lastModifiedBy"] = new Expression<Func<ComponentSoftwareBuild, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(string propertyName, string propertyFilter, ComponentSoftwareBuildQueryDto filterDto)
        {
            var filterCriteria = ApplyFilter(filterDto);
            filterCriteria.And(x => x.Softwareversion.ToLower() != ConstantValueFilter.Unknown);

            var   filteredQuery = GetComponetSoftwareBuildRecords(filterCriteria);
            
            var result =   propertyName switch
            {
                "description" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Description.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Description))
                    .Distinct()
                    .ToListAsync(),

                "deliveryMethod" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Deliverymethod.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Deliverymethod, Value = p.Deliverymethod })
                    .Distinct()
                    .ToListAsync(),

                "lastModifiedBy" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationuserNavigation.Email.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.ModificationuserNavigation.Email))
                    .Distinct()
                    .ToListAsync(),

                "componentSoftwareBuildId" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Componentsoftwarebuildid.ToString().Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Componentsoftwarebuildid.ToString(), Value = p.Componentsoftwarebuildid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "softwareVersion" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Softwareversion.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Softwareversion, Value = p.Softwareversion })
                    .Distinct()
                    .ToListAsync(),               

                "operatingSystem" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Operatingsystem.Operatingsystemname.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Operatingsystem.Operatingsystemname, Value = p.Operatingsystem.Operatingsystemid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "criticalAssetType" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Criticalassettype.Description.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Criticalassettype.Description, Value = p.Criticalassettype.Id.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "vulnerabilityStatus" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vulnerabilitystatus.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Vulnerabilitystatus, Value = p.Vulnerabilitystatus })
                    .Distinct()
                    .ToListAsync(),

                "componentManufacturers" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || $"{x.Componentmanufacturer.Componentmanufacturer}-{x.Componentmanufacturer.Componentname}".Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = $"{p.Componentmanufacturer.Componentmanufacturer}-{p.Componentmanufacturer.Componentname}", Value = p.Componentmanufacturerid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "designContact" =>   (_commonManager.isDesignContactInOrganisation(
                        await filteredQuery.SelectMany(x => x.Componentsoftwarebuildsdesigncontacts
                            .Select(p => new FilterValueDto { Text = p.Designcontact.Email, Value = p.Designcontactid.ToString() }))
                        .Distinct().ToListAsync()
                    )?
                    .Concat(   filteredQuery
                        .Where(x => x.Componentsoftwarebuildsdesigncontacts == null || !x.Componentsoftwarebuildsdesigncontacts.Any())
                        .Select(x => new FilterValueDto
                        {
                            Text = ConstantValueFilter.blankTextValue,
                            Value = ConstantValueFilter.blankZeroValue.ToString()
                        })
                    ))
                    .Distinct()
                    .ToList() ?? new List<FilterValueDto>(),

                _ => new List<FilterValueDto>(),
            };

            return   result;
        }


        #endregion

        #region Create and Add Componenet Softwarebuild

        public async Task<ComponentSoftwareBuildDtoCreate> GetCreateSoftwareBuildPageDetailsAsync()
        {
            var componentManufacture = await _dropdownDataServiceManager.GetFilterValueComponentManufacturersResource();
            var criticalAssetTypesResource = await _dropdownDataServiceManager.GetAllCriticalAssetType();
            var operatingSystemResource = await _dropdownDataServiceManager.GetAllOperatingSystem();
            var designContacts = _repositoryWrapper.UserRepository.FindByCondition(x => x.Isdesigncontact == true)
                            .Select(x => new KeyValuePairDto
                            {
                                Key = (short)x.Id,
                                Text = x.Email
                            })
                            .AsEnumerable() ?
                            .DistinctBy(x => x.Text)
                           ? .ToList();

            var model = new ComponentSoftwareBuildDtoCreate
            {
                EOMStatus = Enum.EOMEnum.NotSpecified,
                ComponentManufacturerResource = componentManufacture,
                CriticalAssetTypeResource = criticalAssetTypesResource,
                OperatingSystemResource =  operatingSystemResource,
                DesignContacts  = designContacts, 

            };

            return model;


        }

        public async Task<ComponentSoftwareBuild> EntityExists(ComponentSoftwareBuildDtoCreate dto)
        {
            var entityExists = await _repositoryWrapper.ComponentSoftwareBuildRepository.FindByCondition(
                    x => x.Componentmanufacturerid == dto.ComponentManufacturerId
                         && x.Softwareversion.ToLower().Replace(" ", "").Replace(".0", "") == dto.SoftwareVersion.ToLower().Replace(" ", "").Replace(".0", ""),true)
                .OrderByDescending(x => x.Creationdate).FirstOrDefaultAsync();
            return ComponentSoftwareBuildMapper.GetComponentSoftwareBuildMapper(entityExists);
        }

        public async Task<ResultDto> AddSoftwareBuildAsync(ComponentSoftwareBuildDtoCreate dto, bool? forced = false)
        {
            var entityExists = await EntityExists(dto);

            if (entityExists != null)
            {
                try
                {
                   
                    if ( entityExists.Deleted == true)
                    {
                        
                            return (forced == true) ?                                
                                await AddBaseAsync(dto)
                                :                       
                              new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryAddExists,
                                Data = new { id = entityExists.ComponentSoftwareBuildId, orphanDeleted = true }
                            };
                        
                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = entityExists.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entityExists.ComponentSoftwareBuildId
                        };
                    }
                }
                catch (Exception  )
                {
                    throw;
                }


            }
            return await AddBaseAsync(dto); ;
             
        }
     
        public async Task<ResultDto> AddBaseAsync(ComponentSoftwareBuildDtoCreate dto)
        {
            Componentsoftwarebuilds result;
            try
            {
                var entityForced = _mapper.Map<ComponentSoftwareBuild>(dto);

                entityForced.DeliveryMethod = string.IsNullOrEmpty(dto.DeliveryMethod) ?  ConstantValueFilter.oemTraditional: dto.DeliveryMethod;

                entityForced.OperatingSystemId = entityForced.OperatingSystemId == 0 ? null : entityForced.OperatingSystemId;                              

                result = ComponentSoftwareBuildMapper.SetComponentSoftwareBuildMapper(entityForced);

                _repositoryWrapper.ComponentSoftwareBuildRepository.Create(result);
                await _repositoryWrapper.SaveAsync();

                await AddOrUpdateDesignContactAsync(dto.DesignContactIds, result.Componentsoftwarebuildid);
  
                 
            }
            catch (Exception)
            {
                throw;
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess, Data = result.Componentsoftwarebuildid };
        }

        public async Task<ResultDto> AddOrUpdateDesignContactAsync(IEnumerable<int> dto, long componentSwBuildId)
        {

            try
            {
                long softwareBuildId = componentSwBuildId;

                var existingContacts = _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository
                    .FindByCondition(x => x.Componentsoftwarebuildid == softwareBuildId)
                    .ToList();

                if(dto?.Any() == true)
                {
                    #region Add New Design Contacts
                    foreach (var contactId in dto)
                    {

                        if (!existingContacts.Any(x => x.Designcontactid == contactId))
                        {
                            _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository.Create(new Componentsoftwarebuildsdesigncontacts
                            {
                                Componentsoftwarebuildid = softwareBuildId,
                                Designcontactid = contactId
                            });
                        }
                    }
                    await _repositoryWrapper.SaveAsync();
                    #endregion

                    #region Remove Unused Design Contacts
                    var recordsToDelete = existingContacts
                        .Where(existing => !dto.Contains(existing.Designcontactid))
                        .ToList();

                    foreach (var record in recordsToDelete)
                    {
                        _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository.DeleteDeep(record);
                    }

                    if (recordsToDelete.Any())
                    {
                        await _repositoryWrapper.SaveAsync();
                    }
                    #endregion
                }
                else
                {
                    foreach (var record in existingContacts)
                    {
                        _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository.DeleteDeep(record);
                    }

                    if (existingContacts.Any())
                    {
                        await _repositoryWrapper.SaveAsync();
                    }
                }


               


                await _repositoryWrapper.ClearTracker();

            }
            catch (Exception)
            {
                throw;
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        #endregion


        #region Edit and Update Componenet Software
        public async Task<ComponentSoftwareBuildDtoUpdate> GetEditedSoftwareBuildDetailsAsync(long id)
        {
            
            var softwareBuildEntity = await _repositoryWrapper.ComponentSoftwareBuildRepository
                .FindByCondition(x => x.Componentsoftwarebuildid == id, includeDeleted: true)
                .Include(x => x.ModificationuserNavigation)
                .SingleAsync();

            // Get dropdown resources
     
            var designContacts = _repositoryWrapper.UserRepository.FindByCondition(x => x.Isdesigncontact == true)
                                    .Select(x => new KeyValuePairDto
                                    {
                                        Key = (short)x.Id,
                                        Text = x.Email
                                    })
                                    .AsEnumerable() ? 
                                    .DistinctBy(x => x.Text)
                                    ?.ToList();
            var componentManufacturersResources = await _dropdownDataServiceManager.GetFilterValueComponentManufacturersResource();
            var operatingSystems = await _dropdownDataServiceManager.GetAllOperatingSystem();
            var criticalAssetTypes = await _dropdownDataServiceManager.GetAllCriticalAssetType();

             
            var model = ComponentSoftwareBuildMapper.GetComponentSoftwareBuildMapper(softwareBuildEntity);
            var dto = _mapper.Map<ComponentSoftwareBuildDtoUpdate>(model);

            #region Lookup Population

            dto.ComponentManufacturerResource = componentManufacturersResources;
           
            if (!dto.ComponentManufacturerResource.Any(x => x.Key == dto.ComponentManufacturerId ))
            {
                var fetchedOem = await _repositoryWrapper.ComponentManufacturersRepository
                    .FindByCondition(x => x.Componentmanufacturerid == dto.ComponentManufacturerId, includeDeleted: true)
                    .SingleOrDefaultAsync();
                if (fetchedOem != null)
                {
                    dto.ComponentManufacturerResource.Add(new KeyValuePairDto
                    {
                        Key = (short)fetchedOem.Componentmanufacturerid  ,
                        Text = $"{fetchedOem.Componentmanufacturer}-{fetchedOem.Componentname}"
                    });                   
                }
            }

            

            dto.OperatingSystemResource = operatingSystems;
            dto.CriticalAssetTypeResource = criticalAssetTypes;

            
            if (dto.CriticalAssetTypeId.HasValue &&
                !dto.CriticalAssetTypeResource.Any(x => x.Key == dto.CriticalAssetTypeId ))
            {
                var fetchedCriticalAsset = await _repositoryWrapper.CriticalAssetTypeRepository
                    .FindByCondition(x => x.Id == dto.CriticalAssetTypeId, includeDeleted: true)
                    .SingleOrDefaultAsync();

                if (fetchedCriticalAsset != null)
                {
                    dto.CriticalAssetTypeResource.Add(new KeyValuePairDto
                    {
                        Key = (short) fetchedCriticalAsset.Id ,
                        Text = fetchedCriticalAsset.Description
                    });
                }
            }

            //dto.ProductNamesResource = productFilterValues;
            ////dto.ProductNameId = softwareBuildEntity.Productnameid;
            //if (dto.ProductNameId.HasValue &&
            //    !dto.ProductNamesResource.Any(x => x.Key == dto.ProductNameId.Value ))
            //{
            //    var fetchedProductName = await _repositoryWrapper.ProductNameRepository
            //        .FindByCondition(x => x.Productnameid == dto.ProductNameId, includeDeleted: true)
            //        .SingleOrDefaultAsync();

            //    if (fetchedProductName != null)
            //    {
            //        dto.ProductNamesResource.Add(new KeyValuePairDto
            //        {
            //            Key = (short) fetchedProductName.Productnameid ,
            //            Text = fetchedProductName.Description
            //        });
            //    }
            //}

          
            if (dto.OperatingSystemId.HasValue &&
                !dto.OperatingSystemResource.Any(x => x.Key == dto.OperatingSystemId.Value ))
            {
                var fetchedOperatingSystem = await _repositoryWrapper.OperatingSystem
                    .FindByCondition(x => x.Operatingsystemid == dto.OperatingSystemId, includeDeleted: true)
                    .SingleOrDefaultAsync();

                if (fetchedOperatingSystem != null)
                {
                    dto.OperatingSystemResource.Add(new KeyValuePairDto
                    {
                        Key = fetchedOperatingSystem.Operatingsystemid ,
                        Text = fetchedOperatingSystem.Operatingsystemname
                    });
                }
            }

            #endregion

          
            dto.DesignContactIds = await _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository
                .FindByCondition(x => x.Componentsoftwarebuildid == softwareBuildEntity.Componentsoftwarebuildid)
                .Select(x => x.Designcontactid)
                .ToListAsync();

            dto.DesignContacts = designContacts;

            if (dto.DesignContactIds != null  && dto.DesignContactIds.Count() >0 )
            {
                var editDesignContact = await _repositoryWrapper.UserRepository.FindByCondition(x => x.Id != 1
                && dto.DesignContactIds.Any(t => t == x.Id))
               .Select( x => new KeyValuePairDto {   Key = (short) x.Id ,  Text = x.Email  })?.Distinct().OrderBy(x => x.Text).ToListAsync();

                foreach (var item in dto.DesignContactIds)
                {
                    if ( (dto.DesignContacts?.Any(x => x.Key == item)) == false)
                    { 
                        if (editDesignContact != null)
                        {
                            var notExsitsDesignContact = editDesignContact.Where(x => x.Key == item ).FirstOrDefault();

                          if(notExsitsDesignContact != null)  dto.DesignContacts.Add(new KeyValuePairDto
                          {
                                 Key =  notExsitsDesignContact.Key,
                                Text = notExsitsDesignContact.Text
                            });
                        }
                    }

                }

                
            }


            return dto;
        }

        public async Task<ResultDto> UpdateSoftwareBuildAsync(ComponentSoftwareBuildDtoUpdate dto, bool? forced)
        {            
            var existingSoftwareBuild =   _repositoryWrapper.ComponentSoftwareBuildRepository
                .FindByCondition(x =>        
                    x.Componentmanufacturerid == dto.ComponentManufacturerId &&
                   x.Softwareversion.ToLower().Replace(" ", "").Replace(".0", "") == dto.SoftwareVersion.ToLower().Replace(" ", "").Replace(".0", ""),
                    includeDeleted: true).Include(x=>x.Componentmanufacturer)
                .ToList();

            if(existingSoftwareBuild != null && existingSoftwareBuild.Count() >0)
            {
                var checkSameRecordExists = existingSoftwareBuild.Any(t => t.Componentsoftwarebuildid != dto.ComponentSoftwareBuildId) ? true : false;
                if(checkSameRecordExists)
                    new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryUpdateExists,
                        Data = new { id = dto.ComponentSoftwareBuildId }
                    };
            }
            #region if ElementName changed need to update in Rkey and DCFLifeCycle 
            var modifiedComponenet = _repositoryWrapper.ComponentSoftwareBuildRepository.FindByCondition(x => x.Componentsoftwarebuildid 
            == dto.ComponentSoftwareBuildId).Include(y => y.Componentmanufacturer).FirstOrDefault();
            string OldElementName = string.Empty;

            if (modifiedComponenet.Componentmanufacturerid != dto.ComponentManufacturerId
                || modifiedComponenet.Softwareversion != dto.SoftwareVersion)
                OldElementName = $"{modifiedComponenet.Componentmanufacturer.Componentmanufacturer}-" +
                                       $"{modifiedComponenet.Componentmanufacturer.Componentname}-" +
                                       $"{modifiedComponenet.Softwareversion}";
            #endregion
            return await UpdateBaseAsync(dto, forced,  OldElementName);
        }

        public async Task<ResultDto> UpdateBaseAsync(ComponentSoftwareBuildDtoUpdate dto, bool? forced,string OldElementName)
        {
            var updatedSoftwareBuild = await UpdateSoftwareBuildEntityAsync(dto, OldElementName, forced);

            await AddOrUpdateDesignContactAsync(dto.DesignContactIds, updatedSoftwareBuild.ComponentSoftwareBuildId);

            return new ResultDto
            {
                Info = ResultMessages.EntryUpdateSuccess,
                Data = updatedSoftwareBuild.ComponentSoftwareBuildId
            };
        }

        public async Task<ComponentSoftwareBuild> UpdateSoftwareBuildEntityAsync(ComponentSoftwareBuildDtoUpdate dto, string OldElementName, bool? forced = false)
        {
            var softwareBuildEntity = _mapper.Map<ComponentSoftwareBuild>(dto);

            if (forced ?? false)
            {
                softwareBuildEntity.Deleted = false;
                softwareBuildEntity.DeletionDate = null;
            }

            softwareBuildEntity.DeliveryMethod = string.IsNullOrEmpty(dto.DeliveryMethod)
                ? ConstantValueFilter.oemTraditional
                : dto.DeliveryMethod;

            softwareBuildEntity.OperatingSystemId = softwareBuildEntity.OperatingSystemId == 0
                ? null
                : softwareBuildEntity.OperatingSystemId;

            _repositoryWrapper.ComponentSoftwareBuildRepository.Update(
                ComponentSoftwareBuildMapper.SetComponentSoftwareBuildMapper(softwareBuildEntity));

            await _repositoryWrapper.SaveAsync();
 
            if (OldElementName != null)
            {
                var updatedComponent= _repositoryWrapper.ComponentSoftwareBuildRepository
               .FindByCondition(x =>
                   x.Componentsoftwarebuildid == dto.ComponentSoftwareBuildId
                    ).Include(y => y.Componentmanufacturer)
               .FirstOrDefault  ();
                string newComponentName = $"{updatedComponent.Componentmanufacturer.Componentmanufacturer}-" +
                                       $"{updatedComponent.Componentmanufacturer.Componentname}-" +
                                       $"{updatedComponent.Softwareversion}";
                if(newComponentName != OldElementName)
                 _designComponentFamilyLifeCycleManager.UpdateComponenetNameOnDcfLifecycle(dto.ComponentSoftwareBuildId,  OldElementName, newComponentName);
             
            }

            return softwareBuildEntity;
        }
        #endregion

        #region Delete , DeleteDeep , Refered table Details

        public async Task<ResultDto> GetReferenceRecordAsync(long id)
        {         
                var resultMessages = new List<ResultMessageDto>();
               
                var softwareBuild = await _repositoryWrapper.ComponentSoftwareBuildRepository
                    .FindByCondition(x => x.Componentsoftwarebuildid == id)
                    .Include(x => x.Componentmanufacturer)
                    .Include(x => x.Criticalassettype)
                    .SingleAsync();

            var excludedTables = new List<string> {   "Componentsoftwarebuildsdesigncontacts" };
                
                var referencedRecords = await _commonManager.GetForeignKeyRefernceTable(
                    "Componentsoftwarebuilds", id, excludedTables);

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
                            EntityName = "Component Software Build",
                            RecordName = $"{softwareBuild.Componentmanufacturer.Componentmanufacturer} - " +
                                         $"{softwareBuild.Componentmanufacturer.Componentname} - " +
                                         $"{softwareBuild.Softwareversion}",
                            DataRelatedList = resultMessages
                        }
                    };
                }

                return new ResultDto();
          

        }

        public async Task<ResultDto> DeleteComponentSwBuild(long id)
        {
            var existsComponenetSwBuild = await _repositoryWrapper.ComponentSoftwareBuildRepository.FindByCondition(x => x.Componentsoftwarebuildid == id)
                .Include(x => x.Componentmanufacturer).SingleAsync();
            
            #region  
            var softwareDesignContact = _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository.FindByCondition(x => x.Componentsoftwarebuildid == id).ToList();

            foreach (var toDelete in softwareDesignContact)
                _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository.DeleteDeep(toDelete);

            await _repositoryWrapper.SaveAsync();
            #endregion

            _repositoryWrapper.ComponentSoftwareBuildRepository.Delete(existsComponenetSwBuild);
            await _repositoryWrapper.SaveAsync();

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = existsComponenetSwBuild.Componentsoftwarebuildid
            };
        }

        #endregion
        #region Software Clone
        public async Task<ResultDto> CopyComponentSoftwareBuildToCloneAsync(long id)
        {
            var existingSoftwareBuild = await _repositoryWrapper.ComponentSoftwareBuildRepository
                .FindByCondition(x => x.Componentsoftwarebuildid == id)
                .Include(x => x.Componentmanufacturer).SingleOrDefaultAsync();

            var dto = new ComponentSoftwareBuildToCloneDto()
            {
                ComponentSoftwareBuildId = existingSoftwareBuild.Componentsoftwarebuildid,
                SoftwareVersion = existingSoftwareBuild.Softwareversion,
                ComponentManufacturers = existingSoftwareBuild.Componentmanufacturer.Componentmanufacturer != null? $"{existingSoftwareBuild.Componentmanufacturer.Componentmanufacturer}-" +
                $"{existingSoftwareBuild.Componentmanufacturer.Componentname}":"",
                ComponentManufacturerId = existingSoftwareBuild.Componentmanufacturerid.Value
            }; 
             
            dto.DesignContactIds = _repositoryWrapper.ComponentSoftwareBuildsDesignContactRepository.FindByCondition(x => x.Componentsoftwarebuildid ==
            existingSoftwareBuild.Componentsoftwarebuildid).Select(x => x.Designcontactid).ToList();

            dto.DesignContacts = await _dropdownDataServiceManager.GetAllOrganisation();


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = dto
            };
        }

        public async Task<ResultDto> GetComponentBasedOnProductNameAndOemAsync(short componentManufacturerid, short productNameId)
        {
           
            List<KeyValuePair<long, string>> existingComponentSwBuild = new List<KeyValuePair<long, string>>() {
                new KeyValuePair<long, string>( 0, "No Reference Available" ) };

            await Task.Run(() => existingComponentSwBuild.AddRange(_repositoryWrapper.ComponentSoftwareBuildRepository
       .FindByCondition(x => x.Componentmanufacturerid == componentManufacturerid)
      .Include(x => x.Componentmanufacturer)
       .ToDictionary(x => (long)x.Componentsoftwarebuildid,
       x => $"{x.Componentmanufacturer.Componentmanufacturer} " +
       $"{ x.Componentmanufacturer.Componentname} " +
       $"{x.Softwareversion}".ToString()
         ).Where(x => !x.Value.ToLower().Contains("unknown"))));

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = existingComponentSwBuild
            };

        } 
        public async Task<ResultDto> UpgradeClonedComponentSwBuild(CloneComponentSoftwareBuildDto dto)
        {
            using var transaction = await _repositoryWrapper.BeginTransactionAsync();
            try
            {

                var existingBuild = _repositoryWrapper.ComponentSoftwareBuildRepository
                    .FindByCondition(x => x.Componentsoftwarebuildid == dto.ComponentSoftwareBuildId)
                    .Include(x => x.Criticalassettype)
                    .Include(x => x.Componentmanufacturer)
                    .SingleOrDefault();

                if (existingBuild == null)
                {
                    return new ResultDto { Warning = true, Info = ResultMessages.EntryNotFound };
                }


                var duplicateBuild = await _repositoryWrapper.ComponentSoftwareBuildRepository
                    .FindByCondition(x =>
                        x.Componentmanufacturerid == existingBuild.Componentmanufacturerid &&
                        //x.Productname.Description == existingBuild.Productname.Description &&
                        x.Softwareversion.ToLower().Replace(" ", "").Replace(".0", "") == dto.SoftwareVersion.ToLower().Replace(" ", "").Replace(".0", ""))
                    .Include(x => x.Criticalassettype)
                    .OrderByDescending(x => x.Creationdate)
                    .FirstOrDefaultAsync();

                if (duplicateBuild != null)
                {
                    return new ResultDto { Warning = true, Info = ResultMessages.EntryAddExists };
                }
                 
                var newBuildDto = new ComponentSoftwareBuildDtoCreate
                {
                    ComponentManufacturerId = existingBuild.Componentmanufacturerid.Value,
                    DeliveryMethod = existingBuild.Deliverymethod,
                    OperatingSystemId = existingBuild.Operatingsystemid,
                    SoftwareVersion = dto.SoftwareVersion,
                    CriticalAssetTypeId = existingBuild.Criticalassettypeid,
                    Description = existingBuild.Description,
                    VulnerabilityStatus = existingBuild.Vulnerabilitystatus,
                    EndOfMaintenance = dto.EndOfMaintenance != DateTime.MinValue ? dto.EndOfMaintenance : null,
                    EOMStatus = dto.EomStatus
                };


                if (newBuildDto.EndOfMaintenance.HasValue && !string.IsNullOrEmpty(newBuildDto.DeliveryMethod) &&
                    newBuildDto.DeliveryMethod.Equals(ConstantValueFilter.oneTrack, StringComparison.OrdinalIgnoreCase))
                {
                    int lastDayOfMonth = DateTime.DaysInMonth(newBuildDto.EndOfMaintenance.Value.Year, newBuildDto.EndOfMaintenance.Value.Month);
                    DateTime gaDate = newBuildDto.EndOfMaintenance.Value.AddMonths(-18);
                    DateTime endOfSupportDate = newBuildDto.EndOfMaintenance.Value.AddMonths(12);

                    newBuildDto.GeneraAvailableDate = (newBuildDto.EndOfMaintenance.Value.Day == lastDayOfMonth)
                        ? new DateTime(gaDate.Year, gaDate.Month, DateTime.DaysInMonth(gaDate.Year, gaDate.Month))
                        : gaDate;

                    newBuildDto.EndOfsupport = (newBuildDto.EndOfMaintenance.Value.Day == lastDayOfMonth)
                        ? new DateTime(endOfSupportDate.Year, endOfSupportDate.Month, DateTime.DaysInMonth(endOfSupportDate.Year, endOfSupportDate.Month))
                        : endOfSupportDate;
                }
                else
                {
                    newBuildDto.GeneraAvailableDate = null;
                    newBuildDto.EndOfsupport = dto.EndOfsupport;
                }


                if (!string.IsNullOrEmpty(newBuildDto.DeliveryMethod) &&
                    (newBuildDto.DeliveryMethod.Equals(ConstantValueFilter.deliveryMethodCiCd, StringComparison.OrdinalIgnoreCase) ||
                    newBuildDto.DeliveryMethod.Replace(" ", "").Equals(ConstantValueFilter.oneTrack.Replace(" ", "").ToUpper(), StringComparison.OrdinalIgnoreCase)) &&
                    existingBuild.Componentmanufacturer.Componentmanufacturer.ToLower().Contains(ConstantValueFilter.oemEricsson))
                {
                    if (newBuildDto.EndOfMaintenance.HasValue)
                    {
                        int lastDayOfMonth = DateTime.DaysInMonth(newBuildDto.EndOfMaintenance.Value.Year, newBuildDto.EndOfMaintenance.Value.Month);
                        DateTime gaDate = newBuildDto.EndOfMaintenance.Value.AddMonths(-18);

                        newBuildDto.GeneraAvailableDate = (newBuildDto.EndOfMaintenance.Value.Day == lastDayOfMonth)
                            ? new DateTime(gaDate.Year, gaDate.Month, DateTime.DaysInMonth(gaDate.Year, gaDate.Month))
                            : gaDate;
                    }
                }

                newBuildDto.DesignContactIds = dto.DesignContactIds;

                var newBuildResult = await AddBaseAsync(newBuildDto);
               
                await _repositoryWrapper.SaveAsync();
                await transaction.CommitAsync();

                return new ResultDto { Info = ResultMessages.EntryAddSuccess, Warning = false };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        #endregion

    }

}