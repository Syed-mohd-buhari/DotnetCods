using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NfviSoftwareCompatibility;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
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

namespace CAM.BusinessManager.Entity
{
    public class NfviSoftwareCompatibilityManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;
        protected readonly ILoggerManager _logger;
        private readonly GridCustomColumnManager _customColumnManager;

        public NfviSoftwareCompatibilityManager(IEnumerable<IRepositoryWrapper> wrappers,            
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,            
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor , CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager)
            : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;            
            _logger = logger;          
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _customColumnManager = customColumnManager;
        }
         
        #region  Grid Load , Filter ,Apply Filter 

        private IQueryable<Nfvisoftwarecompatibility> GetNfviSWCompatabilityRecords(ExpressionStarter<Nfvisoftwarecompatibility> predicateResult)
        {
            var query = _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(predicateResult)
                 .Include(t => t.CreationuserNavigation)
               .Include(t => t.Product).ThenInclude(t => t.Vodafonenames)
               .Include(t => t.ModificationuserNavigation)
               .Include(t => t.Vendor)
               .Include(t => t.Plaftform);
 
            return query;
        }

        public async Task<QueryResultDto<NfviSoftwareCompatibilityDtoGrid>> FindWithCondition(NfviSoftwareCompatibilityQueryDto nfviSwCompatibilityFilterDto)
        {
            var predicateResult = ApplyFilter(nfviSwCompatibilityFilterDto);

            if (nfviSwCompatibilityFilterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }


            var nfviSWCompatabilityQuery = await Task.Run(() => GetNfviSWCompatabilityRecords(predicateResult).AsEnumerable()
                         .Select(p => NfviSoftwareCompatibilityMapper.GetNfviSoftwareCompatibilityMapper(p)).AsQueryable());
 
            var totalCount = nfviSWCompatabilityQuery.Count();

            var paginatedRecords = await Task.Run(() => nfviSWCompatabilityQuery.ApplyOrdering(nfviSwCompatibilityFilterDto, GetColumnsMap())
               .ApplyPaging(nfviSwCompatibilityFilterDto).ToList());

            IEnumerable<NfviSoftwareCompatibilityDtoGrid> nfviSwCompatibilityDtoGrid;

            nfviSwCompatibilityDtoGrid = _mapper.Map<IEnumerable<NfviSoftwareCompatibilityDtoGrid>>(paginatedRecords);

            return new QueryResultDto<NfviSoftwareCompatibilityDtoGrid>(
                new GenerateRenderForGrid<NfviSoftwareCompatibilityDtoGrid>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = nfviSwCompatibilityDtoGrid.ToArray()
            };
        }

        public ExpressionStarter<Nfvisoftwarecompatibility> ApplyFilter(NfviSoftwareCompatibilityQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>(true);

            if (filterDto.NfviSoftwareCompatibilityId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                foreach (var id in filterDto.NfviSoftwareCompatibilityId)
                    componentIdPredicate.Or(x => x.Nfvisoftwarecompatibilityid == id);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.MinimumSupportedVersion?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                foreach (var description in filterDto.MinimumSupportedVersion)
                    descriptionPredicate.Or(x => x.Minimumsupportedversion == description);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.plaftFormVersion?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                foreach (var item in filterDto.plaftFormVersion)
                    componentIdPredicate.Or(x => x.Plaftformid.ToString() == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.productName?.Any() == true)
            {
                var productDescriptionPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                foreach (var item in filterDto.productName)
                    productDescriptionPredicate.Or(x => x.Productid.ToString() == item);

                mainPredicate.And(productDescriptionPredicate);
            }
            if (filterDto.vendor?.Any() == true)
            {
                var vendorNamePredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                foreach (var item in filterDto.vendor)
                    vendorNamePredicate.Or(x => x.Vendorid.ToString() == item);

                mainPredicate.And(vendorNamePredicate);
            }

            if (filterDto.LastModifiedBy != null && filterDto.LastModifiedBy.Count >0)
            {
                var lastModifiedByPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                foreach (var item in filterDto.LastModifiedBy)
                    lastModifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == item);
                mainPredicate.And(lastModifiedByPredicate);
            }
            if (filterDto.LastModified != null )
            {
                var lastModifiedPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                if (filterDto.LastModified.StartDate != null)
                    lastModifiedPredicate.And(x => x.Modificationdate.Date >= filterDto.LastModified.StartDate);

                if (filterDto.LastModified.EndDate != null)
                    lastModifiedPredicate.And(x => x.Modificationdate.Date <= filterDto.LastModified.EndDate);

                mainPredicate.And(lastModifiedPredicate);
            }
            if (filterDto.VodafoneName?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Nfvisoftwarecompatibility>();
                foreach (var id in filterDto.VodafoneName)
                    componentIdPredicate.Or(x => x.Product.Vodafonenamesid == id);

                mainPredicate.And(componentIdPredicate);
            }
            return mainPredicate;
        }

        private Dictionary<string, Expression<Func<NfviSoftwareCompatibility, object>>[]> GetColumnsMap()
        {
            var temp = new Dictionary<string, Expression<Func<NfviSoftwareCompatibility, object>>[]>
            {
                ["nfviSoftwareCompatibilityId"] = new Expression<Func<NfviSoftwareCompatibility, object>>[] { p => p.NfviSoftwareCompatibilityId },
                ["plaftFormId"] = new Expression<Func<NfviSoftwareCompatibility, object>>[] { p => p.PlaftFormId },
                ["productId"] = new Expression<Func<NfviSoftwareCompatibility, object>>[] { p => p.ProductId },
                ["lastModifiedBy"] = new Expression<Func<NfviSoftwareCompatibility, object>>[] { p => p.ModificationUserEntity.Email },

            };

            return temp;
        }

        public async Task<List<FilterValueDto>> GetFilteredValuesAsync(string propertyName, string propertyFilter, NfviSoftwareCompatibilityQueryDto filterDto)
        {
            var filterCriteria = ApplyFilter(filterDto);

            var filteredQuery = GetNfviSWCompatabilityRecords(filterCriteria);

            var result = propertyName switch
            {
                "nfviSoftwareCompatibilityId" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Nfvisoftwarecompatibilityid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Nfvisoftwarecompatibilityid))
                    .Distinct()
                    .ToListAsync(),
                "vendor" => await filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vendor.Originalequipmentmanufacturer.Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.Vendor.Originalequipmentmanufacturer, Value = p.Vendorid.ToString() })
                .Distinct()
                .ToListAsync(),
                "plaftFormVersion" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Plaftform.Softwareversion.Contains(propertyFilter))
                    .Select(p => new FilterValueDto { Text = p.Plaftform.Softwareversion , Value = p.Plaftformid.ToString() })
                    .Distinct()
                    .ToListAsync(),

                "productName" => await filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Product.Description.Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.Product.Description, Value = p.Productid.ToString() })
                .Distinct()
                .ToListAsync(),
                "minimumSupportedVersion" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Minimumsupportedversion.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Minimumsupportedversion))
                    .Distinct()
                    .ToListAsync(),
                "lastModifiedBy" => await filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationuserNavigation.Email.Contains(propertyFilter))
                    .Select(p => new FilterValueDto(p.ModificationuserNavigation.Email))
                    .Distinct()
                    .ToListAsync(),
                "lastModified" => string.IsNullOrEmpty(propertyFilter)
                    ? filteredQuery.Select(p => new FilterValueDto { Text = p.Modificationdate.ToString(), Value = p.Modificationdate.ToString() }).Distinct().ToList()
                    : filteredQuery
                        .Where(x =>
                            x.Modificationdate.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.Modificationdate.ToString(), Value = p.Modificationdate.ToString() }).Distinct().ToList(),
                "vodafoneName" => await filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Product.Vodafonenamesid.ToString().Contains(propertyFilter))
                .Select(p => new FilterValueDto { Text = p.Product.Vodafonenames.Description, Value = p.Product.Vodafonenamesid.ToString() })
                .Distinct()
                .ToListAsync(),
                _ => new List<FilterValueDto>(),
            };

            return result;
        }


        #endregion


        #region CRUD Operation 
        public async Task<NfviSoftwareCompatibilityDto> CreatePageNfviDetialAsync()
        {
             var vmwareProductId = await GetMswVmwareProduct();
            var vmwareMswPlaftform = await _dropdownDataServiceManager.GetAllVmwareFromMajorSwBuildDto();
            var productNameResource = await _dropdownDataServiceManager.GetAllProductFilterValueDto();
            var vendorResource = await GetVendorDetails(false);
         
            var model = new NfviSoftwareCompatibilityCreateEditPageDto
            {
                VendorResource = (vendorResource.Any())? vendorResource.Where(x => x.Text.ToLower() != ConstantValueFilter.mswVmware).ToList() : vendorResource,
                vmwareMswPlatform = vmwareMswPlaftform,
                ProductName =  
                 ((vmwareProductId.Count == 0) ? productNameResource :
                productNameResource.Where(t => !vmwareProductId.Any(x => x.Key == t.Key)).ToList()) 

            };

            return model;


        }
        public async Task<NfviSoftwareCompatibilityDto> EditPageNfviDetialAsync(long nfviSwCompatId)
        {
            var vmwareProductId = await GetMswVmwareProduct();
            var vmwareMswPlaftform = await _dropdownDataServiceManager.GetAllVmwareFromMajorSwBuildDto() ?? new Dictionary<long, string>();
            var vendorResource = await GetVendorDetails(true);
            var productNameResource = await _dropdownDataServiceManager.GetAllProductFilterValueDto();
            var existingRecord = _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(x => x.Nfvisoftwarecompatibilityid == nfviSwCompatId).FirstOrDefault();

            vendorResource = (vendorResource.Any()) ? vendorResource.Where(x => x.Text.ToLower() != ConstantValueFilter.mswVmware).ToList() 
                : vendorResource;

            productNameResource = ((vmwareProductId.Count == 0) ? productNameResource :
                productNameResource.Where(t => !vmwareProductId.Any(x => x.Key == t.Key)).ToList());

            if (existingRecord != null && productNameResource.Where(x => x.Key == existingRecord.Productid).FirstOrDefault() == null)
            {
                var deletedProductRecord =   _repositoryWrapper.ProductNameRepository.FindByCondition(x => x.Productnameid == existingRecord.Productid ,true)
               .Select(
            x => new KeyValuePairDto
            {
                Key = (short)x.Productnameid,
                Text = x.Description
            }) .ToList();

                productNameResource.AddRange(deletedProductRecord);
            }

            if (existingRecord != null && vmwareMswPlaftform?.Any(x => x.Key == existingRecord.Plaftformid) == false )
            {
                var deletedPlatformRecord = await _repositoryWrapper.MajorSoftwareBuild.FindByCondition(x => x.Orgeqpmanufacturer.Originalequipmentmanufacturer.ToString()
            .ToLower() == ConstantValueFilter.mswVmware && x.Majorsoftwarebuildsid == existingRecord.Plaftformid, true).Include(x => x.Orgeqpmanufacturer).FirstOrDefaultAsync()
             ;

                if (deletedPlatformRecord != null)
                    vmwareMswPlaftform.Add( deletedPlatformRecord.Majorsoftwarebuildsid, deletedPlatformRecord.Softwareversion);

            }

            if (existingRecord != null && vendorResource?.Any(x => x.Key == existingRecord.Vendorid) == false)
            {
                var deletedVendorRecord =   _repositoryWrapper.OriginalEquipmentManufacturer.FindByCondition(x => x.Orgeqpmanufacturerid == existingRecord.Vendorid
             , true)
               .Select(
            x => new KeyValuePairDto
            {
                Key = x.Orgeqpmanufacturerid,
                Text = x.Originalequipmentmanufacturer
            }).ToList();

                vendorResource.AddRange(deletedVendorRecord);

            }
            var model = new NfviSoftwareCompatibilityCreateEditPageDto
            {
                vmwareMswPlatform = vmwareMswPlaftform,
                VendorResource = vendorResource,
                ProductName = productNameResource,
                NfviSoftwareCompatibilityId = existingRecord.Nfvisoftwarecompatibilityid,
                MinimumSupportedVersion = existingRecord.Minimumsupportedversion,
                PlaftFormId = existingRecord.Plaftformid,
                ProductId = (long)existingRecord.Productid ,
                VendorId = existingRecord.Vendorid
                
            };

            return model;


        }       

        public async Task<ResultDto> AddOrUpdateNfviSoftwareCompatAsync(NfviSoftwareCompatibilityCreateEditPageDto dto)
        {
            var errorExistingVersion = new List<string>();
            try
            { 

                    if (dto != null)
                    {

                    #region TCL Record
                    var existingNfviSw1 = _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(x => x.Productid == dto.ProductId
                         && x.Vendorid == dto.VendorId && x.Plaftformid == dto.PlaftFormId && x.Minimumsupportedversion.ToLower().Replace(" ", "").Replace(".0", "")
                     == dto.MinimumSupportedVersion.ToLower().Replace(" ", "").Replace(".0", ""))
                               .Include(t => t.Product).ToListAsync()?.Result;

                    var existingNfviSw = _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(x => x.Productid == dto.ProductId
                          && x.Vendorid == dto.VendorId  && x.Plaftformid == dto.PlaftFormId  )
                                .Include(t => t.Product).ToListAsync()?.Result;

                            if (dto.NfviSoftwareCompatibilityId == 0 && (existingNfviSw == null || existingNfviSw.Count == 0))
                            {
                                _repositoryWrapper.NfviSoftwareCompatibilityRepository.Create(new Nfvisoftwarecompatibility
                                {  
                                    Vendorid = dto.VendorId, 
                                    Plaftformid = dto.PlaftFormId,
                                    Productid = dto.ProductId,
                                    Minimumsupportedversion = dto.MinimumSupportedVersion
                                });
                            }
                            else if (dto.NfviSoftwareCompatibilityId == 0 && existingNfviSw.Count > 0)
                            {
                                errorExistingVersion.Add(string.Join(",",
                                    existingNfviSw.DistinctBy(x => x.Plaftformid)
                                    .Select(x => x.Nfvisoftwarecompatibilityid + "-" + x.Product.Description).ToList() ?? new List<string>())
                                    + " : " + dto.MinimumSupportedVersion);

                            }
                            else if (dto.NfviSoftwareCompatibilityId != 0)
                            {
                                if (existingNfviSw != null &&
                                    existingNfviSw.Where(x => x.Nfvisoftwarecompatibilityid != dto.NfviSoftwareCompatibilityId                                      
                                    ).FirstOrDefault() != null)
                                {
                                    errorExistingVersion.Add(string.Join(",",
                                     existingNfviSw.DistinctBy(x => x.Plaftformid)
                                     .Select(x => x.Nfvisoftwarecompatibilityid + "-" + x.Product.Description).ToList() ?? new List<string>())
                                     + " : " + dto.MinimumSupportedVersion);

                                }
                                else
                                {
                                    var existEditedNfvi = _repositoryWrapper.NfviSoftwareCompatibilityRepository
                                        .FindByCondition(x => x.Nfvisoftwarecompatibilityid == dto.NfviSoftwareCompatibilityId).FirstOrDefault();
                                    if (existEditedNfvi != null)
                                    {
                                existEditedNfvi.Vendorid = dto.VendorId; 
                                        existEditedNfvi.Productid = dto.ProductId;
                                        existEditedNfvi.Plaftformid = dto.PlaftFormId;
                                        existEditedNfvi.Minimumsupportedversion = dto.MinimumSupportedVersion;
                                        _repositoryWrapper.NfviSoftwareCompatibilityRepository.Update(existEditedNfvi);
                                    }

                                }

                            }



                  if(  errorExistingVersion.Count == 0)
                    {
                        await _repositoryWrapper.SaveAsync();

                        await _repositoryWrapper.ClearTracker();
                    }
                       

                        #endregion
                    }
 else
                {
                    return new ResultDto
                    {
                        Info =   ResultMessages.EntryAddUpdateFailed,
                        Data = dto
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return new ResultDto
            {
                Info = errorExistingVersion.Count > 0 ? ResultMessages.EntryAlreadyExists : ResultMessages.EntryAddUpdateSuccess,
                Warning = errorExistingVersion.Count > 0 ?true : false,
                Data = dto
            };
        }
        public async Task<ResultDto> DeleteDeep(long nfviSwCompatId  )
        {
 
          var nfviEntity = _repositoryWrapper.NfviSoftwareCompatibilityRepository.FindByCondition(x => x.Nfvisoftwarecompatibilityid == nfviSwCompatId).FirstOrDefault();
                if(nfviEntity != null)
            {
                _repositoryWrapper.NfviSoftwareCompatibilityRepository.DeleteDeep(nfviEntity);

                await _repositoryWrapper.SaveAsync();
            }

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess
            };
        }


        #endregion

        public async Task<List<KeyValuePairDto>> GetVendorDetails(bool  includeDeleted)
        {

          var allVendorResource = await _repositoryWrapper.OriginalEquipmentManufacturer.FindAll(includeDeleted)
             .Select(x => new KeyValuePairDto
             {
              Key =  x.Orgeqpmanufacturerid,
              Text = x.Originalequipmentmanufacturer,
              
            }).Distinct().OrderBy(x => x.Text).ToListAsync();

            return allVendorResource;
        }

        public async Task<List<KeyValuePairDto>> GetMswVmwareProduct()
        {

            var allVendorResource = await _repositoryWrapper.ProductNameRepository.FindByCondition(x =>
            x.Majorsoftwarebuilds.Any(t => t.Isvmware == true)) 
               .Select(x => new KeyValuePairDto
               {
                   Key = (short)x.Productnameid,
                   Text = x.Description,

               }).Distinct().ToListAsync();

            return allVendorResource;
        }
    }
}