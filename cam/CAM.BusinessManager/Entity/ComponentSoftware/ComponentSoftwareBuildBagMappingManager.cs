using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ComponentSoftware;
using CAM.DataTransferObjects.Entita.ComponentSoftware.MappedComponentBuildBag;
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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.ComponentSoftware
{
    public class ComponentSoftwareBuildBagMappingManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        private readonly GridCustomColumnManager _customColumnManager;

        public ComponentSoftwareBuildBagMappingManager(IEnumerable<IRepositoryWrapper> wrappers,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _logger = logger;
            _customColumnManager = customColumnManager;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;

        }


        // #region Create and Add Build Bag



        public async Task<ComponentBuilBagCreatePageDto> GetCreateMappingPageDetailsAsync()
        {
            List<ViewComponentSoftwareBuild> componentSoftware = await _dropdownDataServiceManager.GetComponenetSoftwareDetails();
            List<KeyValuePairDto> buildBagResource = await _dropdownDataServiceManager.GetBuildBagDetails();
            List<KeyValuePairDto> componentManufacturers = await _dropdownDataServiceManager.GetFilterValueComponentManufacturersResource();

            ComponentBuilBagCreatePageDto model = new()
            {
                UpgradeComponentSoftwareDetails = componentSoftware,
                BuildBagDetails = buildBagResource,
                ComponentManufacturersResource = componentManufacturers,
            };

            return model;

        }

        public async Task<ComponentBuildBagEditPageDto> GetEditAndUpgradeMappingPageDetailsAsync(long buildBagId)
        {
            List<Componentsoftwarebuildbags> existingComponenetBuild = await ComponetBuildBagEntity(buildBagId);

            List<ViewComponentSoftwareBuild> componentSoftware = await _dropdownDataServiceManager.GetComponenetSoftwareDetails();
            List<KeyValuePairDto> buildBagResource = await _dropdownDataServiceManager.GetBuildBagDetails();
            List<KeyValuePairDto> componentManufacturers = await _dropdownDataServiceManager.GetFilterValueComponentManufacturersResource();

            IEnumerable<Componentsoftwarebuilds> fetchComponentSoftwareBuild = existingComponenetBuild.Select(x => x.Componentsoftwarebuild);

            List<ViewComponentSoftwareBuild> viewComponetSoftwareBuild = new();
            List<ViewComponentSoftwareBuild> removeMappedComponentSwForUpgradeList = componentSoftware.Where(x => existingComponenetBuild
            .All(m => m.Componentsoftwarebuildid != x.ComponentSoftwareBuildId)).ToList();
 

            foreach (Componentsoftwarebuilds item in fetchComponentSoftwareBuild)
            {
                viewComponetSoftwareBuild.Add(new ViewComponentSoftwareBuild
                {
                    DisplayDescription = $"{item.Componentmanufacturer.Componentmanufacturer}-" +
                                         $"{item.Componentmanufacturer.Componentname}-" +
                                         $"{item.Softwareversion}",
                    ComponentSoftwareBuildId = item.Componentsoftwarebuildid,
                    ComponentManufacturerId = item.Componentmanufacturerid.Value,
                    RelavantComponetSwBuild = removeMappedComponentSwForUpgradeList.Where(t => t.ComponentManufacturerId == item.Componentmanufacturerid)
                    .Select(t => new KeyValuePairDto
                    {
                        Text = t.DisplayDescription,
                        Key = (short)t.ComponentSoftwareBuildId
                    }).Distinct().ToList()
                });

            }

            ComponentBuildBagEditPageDto model = new()
            {
                UpgradeComponentSoftwareDetails = viewComponetSoftwareBuild,
                ComponentBagDescription = existingComponenetBuild.FirstOrDefault()?.Buildbag?.Bagdescription,
                BuildBagId = existingComponenetBuild.FirstOrDefault()?.Buildbag?.Buildbagid ?? buildBagId,
                ComponentManufacturersResource = componentManufacturers,
                ExistingComponentSwBuild = componentSoftware
            };

            return model;

        }

        public async Task<ResultDto> AddOrUpdateComponenetSoftwareBuildBagAsync(List<long> dto, long buildBagId)
        {

            try
            {

                List<Componentsoftwarebuildbags> existingComponentBuildBag = _repositoryWrapper.ComponentSoftwareBuildBagRepository
                    .FindByCondition(x => x.Buildbagid == buildBagId)
                    .ToList();

                #region Add New Design Contacts
                foreach (long componentSwId in dto)
                {

                    if (!existingComponentBuildBag.Any(x => x.Componentsoftwarebuildid == componentSwId))
                    {
                        _repositoryWrapper.ComponentSoftwareBuildBagRepository.Create(new Componentsoftwarebuildbags
                        {
                            Buildbagid = buildBagId,
                            Componentsoftwarebuildid = componentSwId
                        });
                    }
                }
                await _repositoryWrapper.SaveAsync();
                #endregion

                #region Remove Unused Design Contacts
                List<Componentsoftwarebuildbags> recordsToDelete = existingComponentBuildBag
                    .Where(y => !dto.Any(x => x == y.Componentsoftwarebuildid))
                    .ToList();

                foreach (Componentsoftwarebuildbags record in recordsToDelete)
                {
                    _repositoryWrapper.ComponentSoftwareBuildBagRepository.DeleteDeep(record);
                }

                if (recordsToDelete.Any())
                {
                    await _repositoryWrapper.SaveAsync();
                }
                #endregion


                await _repositoryWrapper.ClearTracker();

            }
            catch (Exception)
            {
                throw;
            }

            return new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        public async Task<ResultDto> UpgradeComponenetSoftwareBuildBagAsync(ComponentBuilBagClonedDto dto)
        {
            ResultDto newlyInsertedBagId = new();
            try
            {
                //newlyInsertedBagId =  await _buildBagManager.AddBuildBagAsync(new BuildBagCreatePageDto { ComponentBagDescription = dto.ComponentBagDescription});

                //if(dto.ComponentSoftwareId?.Any() == true && newlyInsertedBagId.Warning != true)
                //await AddOrUpdateComponenetSoftwareBuildBagAsync(dto.ComponentSoftwareId , (long)newlyInsertedBagId.Data);


                await _repositoryWrapper.ClearTracker();

            }
            catch (Exception)
            {
                throw;
            }

            return newlyInsertedBagId.Warning == true ? newlyInsertedBagId : new ResultDto { Info = ResultMessages.EntryAddSuccess };

        }

        public async Task<List<Componentsoftwarebuildbags>> ComponetBuildBagEntity(long buildBagId)
        {
            List<Componentsoftwarebuildbags> componentbuildBag = await _repositoryWrapper.ComponentSoftwareBuildBagRepository.FindByCondition(
                    x => x.Buildbagid == buildBagId).Include(y => y.Buildbag)
                    .Include(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer)
                .OrderByDescending(x => x.Creationdate).ToListAsync();

            return componentbuildBag;
        }

        #region  Grid and Export Excel 

        private IQueryable<Componentsoftwarebuildbags> GetMappingSwBuildBagRecords(ExpressionStarter<Componentsoftwarebuildbags> predicateResult)
        {
            IQueryable<Componentsoftwarebuildbags> componentbuildBagQuery = _repositoryWrapper.ComponentSoftwareBuildBagRepository.
            FindByCondition(predicateResult).Include(y => y.Buildbag)
                     .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                     .Include(x => x.Componentsoftwarebuild).ThenInclude(x => x.Componentmanufacturer).AsQueryable();


            return componentbuildBagQuery;
        }
        public async Task<QueryResultDto<ComponentMappingSWBuildBagGridDto>> FindWithCondition(ComponentMappingSWBuildBagQueryDto buildFilterDto)
        {

            ExpressionStarter<Componentsoftwarebuildbags> predicateResult = ApplyFilter(buildFilterDto);

            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }

            IQueryable<ComponentSoftwareBuildBag> componentbuildBagQuery = await Task.Run(() => GetMappingSwBuildBagRecords(predicateResult).AsEnumerable()
                    .Select(p => ComponentMappignSwBuildMapper.GetComponentMappingSwBuilBagdMapper(p)).AsQueryable());

            int totalCount = componentbuildBagQuery.Count();

            List<ComponentSoftwareBuildBag> paginatedQuery = await Task.Run(() => componentbuildBagQuery.ApplyOrdering(buildFilterDto, GetColumnsMap())
               .ApplyPaging(buildFilterDto).ToList());

            IEnumerable<ComponentMappingSWBuildBagGridDto> mappedData = _mapper.Map<IEnumerable<ComponentMappingSWBuildBagGridDto>>(paginatedQuery);

            return new QueryResultDto<ComponentMappingSWBuildBagGridDto>(
                new GenerateRenderForGrid<ComponentMappingSWBuildBagGridDto>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = mappedData.ToArray()
            };
        }

        public async Task<QueryResultDto<ComponentMappingSWBuildBagGridDto>> FindWithConditionBasedAsync(ComponentMappingSWBuildBagQueryDto buildFilterDto)
        {
            ExpressionStarter<Componentsoftwarebuildbags> predicateResult = ApplyFilter(buildFilterDto);

            if (buildFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }

            IQueryable<ComponentSoftwareBuildBag> componentbuildBagQuery = await Task.Run(() => GetMappingSwBuildBagRecords(predicateResult).AsEnumerable()
                    .Select(p => ComponentMappignSwBuildMapper.GetComponentMappingSwBuilBagdMapper(p)).AsQueryable());

            IQueryable<ComponentSoftwareBuildBag> orderingComponentSwEntity = await Task.Run(() => componentbuildBagQuery.ApplyOrdering(buildFilterDto, GetColumnsMap()));


            IQueryable<ComponentMappingSWBuildBagGridDto> mergeComponenetBuilBag = await Task.Run(() => orderingComponentSwEntity.GroupBy(x => x.BuildBagId).Select(t =>
            new ComponentMappingSWBuildBagGridDto
            {
                BuildBagId = t.Key,
                BuildBagDescription = t.FirstOrDefault().BuildBags.BagDescription,
                LastModifiedBy = t.FirstOrDefault().ModificationUserEntity.Email,
                LastModifiedValue = t.FirstOrDefault().ModificationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                ComponentSwBuildBagDescription = string.Join(",", t.Select(s =>
                $"{s.ComponentSoftwareBuilds.ComponentManufacturers.Componentmanufacturer} - " +
                                         $"{s.ComponentSoftwareBuilds.ComponentManufacturers.Componentname} - " +
                                         $"{s.ComponentSoftwareBuilds.SoftwareVersion}") ?? new List<string>()
                )


            }).AsQueryable());

            int totalCount = mergeComponenetBuilBag.Count();

            List<ComponentSoftwareBuildBag> paginatedQuery = await Task.Run(() => orderingComponentSwEntity.ApplyPaging(buildFilterDto).ToList());
            IEnumerable<ComponentMappingSWBuildBagGridDto> mappedData = _mapper.Map<IEnumerable<ComponentMappingSWBuildBagGridDto>>(paginatedQuery);

            return new QueryResultDto<ComponentMappingSWBuildBagGridDto>(
                new GenerateRenderForGrid<ComponentMappingSWBuildBagGridDto>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = mappedData.ToArray()
            };


            //var paginatedQuery = await Task.Run(() => componentbuildBagQuery.ApplyOrdering(buildFilterDto, GetColumnsMap())
            //   .ApplyPaging(buildFilterDto).ToList());

            //var mappedData = _mapper.Map<IEnumerable<ComponentMappingSWBuildBagGridDto>>(paginatedQuery);

            //return new QueryResultDto<ComponentMappingSWBuildBagGridDto>(
            //    new GenerateRenderForGrid<ComponentMappingSWBuildBagGridDto>(_customColumnManager))
            //{
            //    TotalItems = totalCount,
            //    Items = mappedData.ToArray()
            //};
        }



        public ExpressionStarter<Componentsoftwarebuildbags> ApplyFilter(ComponentMappingSWBuildBagQueryDto filterDto)
        {
            ExpressionStarter<Componentsoftwarebuildbags> mainPredicate = PredicateBuilder.New<Componentsoftwarebuildbags>(true);

            if (filterDto.ComponentSoftwarebBuildId?.Any() == true)
            {
                ExpressionStarter<Componentsoftwarebuildbags> componentIdPredicate = PredicateBuilder.New<Componentsoftwarebuildbags>();
                foreach (long id in filterDto.ComponentSoftwarebBuildId)
                {
                    _ = componentIdPredicate.Or(x => x.Componentsoftwarebuildid == id);
                }

                _ = mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.BuildBagDescription?.Any() == true)
            {
                ExpressionStarter<Componentsoftwarebuildbags> descriptionPredicate = PredicateBuilder.New<Componentsoftwarebuildbags>();
                foreach (string item in filterDto.BuildBagDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Buildbagid.ToString() == item);
                }

                _ = mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.ComponetSwBuilDescription?.Any() == true)
            {
                ExpressionStarter<Componentsoftwarebuildbags> descriptionPredicate = PredicateBuilder.New<Componentsoftwarebuildbags>();
                foreach (string item in filterDto.ComponetSwBuilDescription)
                {
                    _ = descriptionPredicate.Or(x => x.Componentsoftwarebuildid.ToString() == item);
                }

                _ = mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.LastModifiedBy?.Any() == true)
            {
                ExpressionStarter<Componentsoftwarebuildbags> modifiedByPredicate = PredicateBuilder.New<Componentsoftwarebuildbags>();
                foreach (string email in filterDto.LastModifiedBy)
                {
                    _ = modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);
                }

                _ = mainPredicate.And(modifiedByPredicate);
            }

            return mainPredicate;
        }

        private Dictionary<string, Expression<Func<ComponentSoftwareBuildBag, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<ComponentSoftwareBuildBag, object>>[]>
            {
                ["componentSoftwareBuildBagId"] = new Expression<Func<ComponentSoftwareBuildBag, object>>[] { p => p.ComponentSoftwareBuildBagId },
                ["componentBagDescription"] = new Expression<Func<ComponentSoftwareBuildBag, object>>[] { p => p.BuildBags.BagDescription },
                ["lastModifiedBy"] = new Expression<Func<ComponentSoftwareBuildBag, object>>[] { p => p.ModificationUserEntity.Email },

            };
        }

        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(string propertyName, string propertyFilter, ComponentMappingSWBuildBagQueryDto filterDto)
        {
            ExpressionStarter<Componentsoftwarebuildbags> filterCriteria = ApplyFilter(filterDto);

            IQueryable<Componentsoftwarebuildbags> filteredQuery = await Task.Run(() => GetMappingSwBuildBagRecords(filterCriteria));


            List<FilterValueDto> result = propertyName switch
            {
                "componentSoftwarebBuildId" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Componentsoftwarebuildbagid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Buildbagid))
                    .Distinct()
                    .ToListAsync(),
                "buildBagDescription" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Buildbag.Bagdescription.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Buildbag.Bagdescription, Value = p.Buildbagid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "componetSwBuilDescription" => await filteredQuery
                .Select(p => new FilterValueDto
                {
                    Text = $"{p.Componentsoftwarebuild.Componentmanufacturer.Componentmanufacturer}-" +
                                         $"{p.Componentsoftwarebuild.Componentmanufacturer.Componentname}-" +
                                         $"{p.Componentsoftwarebuild.Softwareversion}"
                ,
                    Value = p.Componentsoftwarebuild.Componentsoftwarebuildid.ToString()
                })
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Text.Contains(propertyFilter))
                .Distinct()
                .ToListAsync(),

                "lastModifiedBy" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationuserNavigation.Email.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.ModificationuserNavigation.Email))
                    .Distinct()
                    .ToListAsync(),


                _ => new List<FilterValueDto>(),
            };

            return result;
        }
        #endregion


    }



}