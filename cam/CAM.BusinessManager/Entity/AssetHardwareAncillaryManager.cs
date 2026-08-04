using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.AssetHardwareConfig;
using CAM.DataTransferObjects.Entita.DaAsssetMigration;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.MajorHardwareBuild;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.AssetHardwareConfig;
using CAM.Entities.Models.AssetHardwareConfig;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Drawing.Charts;
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
    public class AssetHardwareAncillaryManager : BaseManager
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
private DropdownDataServiceManager _dropdownDataServiceManager;
        public AssetHardwareAncillaryManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager,
             IHttpContextAccessor contextAccessor,
            IRepositoryWrapper repositoryWrapper, DropdownDataServiceManager dropdownDataServiceManager, ILoggerManager logger) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _logger= logger;
        }


        #region //UiMemberFunctions
        public async Task<QueryResultDto<AssetHardwareAncillaryGridDto>> FindWithConditionAsync(AssetHardwareAncillaryQueryDto dynamicFilterDto)
        {
            try
            {

                var predicateResult = ApplyFilter(dynamicFilterDto);
                var rtn = new QueryResultDto<AssetHardwareAncillaryGridDto>(new GenerateRenderForGrid<AssetHardwareAncillaryGridDto>(_manager))
                {

                };
                var assetAncillaryQuery = GetAssetAncillaryRecords(predicateResult)?.ToList();

                var assetResult = await Task.Run(() => assetAncillaryQuery
              .Select(p => AssetAncillaryMapper.GetAssetHardwareAncillary(p)).AsQueryable());

                rtn.TotalItems = assetResult.Count();
                var paginatedRecords = await Task.Run(() => assetResult.ApplyOrdering(dynamicFilterDto, GetColumnsMap())
                    );

                paginatedRecords = paginatedRecords.ApplyPaging(dynamicFilterDto);

                var assetAncillaryResult = paginatedRecords.Select(x => new AssetHardwareAncillaryGridDto
                {
                    AssetHardwareAncillaryId = x.AssetHardwareAncillaryId,
                    NetworkElementAsPlannedId = x.NetworkElementAsPlannedId,
                    MajorHardwareId = x.MajorHardwareBuildAsIsId,
                    DataCenterId = x.DataCenterId,
                    DataCeterName = x.DataCeterName,
                    ClusterNameId = x.ClusterNameId,
                    ClusterNameDesc = x.ClusterNameDesc,
                    AssetClusterId = x.AssetClusterId,
                    AssetClusterDesc = x.AssetClusterDesc,
                    AssetClusterTypeId = x.AssetClusterTypeId,
                    AssetClusterTypeDesc = x.AssetClusterTypeDesc,
                    LastModifiedBy = x.ModificationUserEntity.Email,
                    LastModified = x.ModificationDate,
                    assetCapacityInfoGridDtos = (x.AssetCapacityInfo != null && x.AssetCapacityInfo.Count >0 ) ? x.AssetCapacityInfo.Select( y => new AssetCapacityInfoGridDto
                    {
                        AssetCapacityInfoId = y.AssetCapacityInfoId,
                        PhysicalServerHostName = y.PhysicalServerHostName,
                        PhysicalServerHwModel = y.PhysicalServerHwModel,
                        PhysicalServerHwModelId = y.PhysicalServerHwModelId,
                        PhysicalServerIpAddress = y.PhysicalServerIpAddress,
                        PhysicalServerSerialNumber  = y.PhysicalServerSerialNumber,
                        PhysicalServerVendor = y.PhysicalServerVendor,
                        PhysicalServerVendorId  = y.PhysicalServerVendorId,
                        Storage = y.Storage,
                        Memory = y.Memory,
                        Vcpu = y.Vcpu,
                        NoOfInstances = y.NoOfInstances,

                    }).ToList() : new List<AssetCapacityInfoGridDto>()
                                       

                }).ToList();



               rtn.Items = assetAncillaryResult.ToList();
 
                return rtn;
            }
            catch (Exception ex)
            {

                _logger.LogError("Aset Ancillary Create Page : " + ex);
                return null;
            }

        }

        private static ExpressionStarter<Assethardwareancillary> ApplyFilter(AssetHardwareAncillaryQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Assethardwareancillary>(true);
            var predicateInner = PredicateBuilder.New<Assethardwareancillary>(true);

            if (buildFilterDto.AssetHardwareAncillaryId != null && buildFilterDto.AssetHardwareAncillaryId.Any())
            {
                predicateInner = PredicateBuilder.New<Assethardwareancillary>();
                foreach (var item in buildFilterDto.AssetHardwareAncillaryId)
                    predicateInner.Or(x => x.Assethardwareancillaryid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ElementName != null && buildFilterDto.ElementName.Any())
            {
                predicateInner = PredicateBuilder.New<Assethardwareancillary>();
                foreach (var item in buildFilterDto.ElementName)
                    predicateInner.Or(x => x.Networkelementasplannedid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.MajorHardwareName != null && buildFilterDto.MajorHardwareName.Any())
            {
                predicateInner = PredicateBuilder.New<Assethardwareancillary>();
                foreach (var item in buildFilterDto.MajorHardwareName)
                    predicateInner.Or(x => x.Majorhardwarebuildasisid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.DataCeterName != null && buildFilterDto.DataCeterName.Any())
            {
                predicateInner = PredicateBuilder.New<Assethardwareancillary>();
                foreach (var item in buildFilterDto.DataCeterName)
                    predicateInner.Or(x => x.Datacenterid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ClusterNameDesc != null && buildFilterDto.ClusterNameDesc.Any())
            {
                predicateInner = PredicateBuilder.New<Assethardwareancillary>();
                foreach (var item in buildFilterDto.ClusterNameDesc)
                    predicateInner.Or(x => x.Clusternameid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.AssetClusterDesc != null && buildFilterDto.AssetClusterDesc.Any())
            {
                predicateInner = PredicateBuilder.New<Assethardwareancillary>();
                foreach (var item in buildFilterDto.AssetClusterDesc)
                    predicateInner.Or(x => x.Assetclusterid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.AssetClusterTypeDesc != null && buildFilterDto.AssetClusterTypeDesc.Any())
            {
                predicateInner = PredicateBuilder.New<Assethardwareancillary>();
                foreach (var item in buildFilterDto.AssetClusterTypeDesc)
                    predicateInner.Or(x => x.Assetclustertypeid == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<Assethardwareancillary> GetAssetAncillaryRecords(ExpressionStarter<Assethardwareancillary> predicateResult)
        {
            var query = _repositoryWrapper.AssetHardwareAncillaryRepository.FindByCondition(predicateResult)
                       .Include(x => x.CreationuserNavigation)
                       .Include(x => x.ModificationuserNavigation)
                       .Include(x => x.Majorhardwarebuildasis).ThenInclude (x=> x.Orgeqpmanufacturer)
                       .Include(x => x.Majorhardwarebuildasis).ThenInclude(x => x.Platform)
                       .Include(x => x.Datacenter)
                        .Include(x => x.Assetcluster)
                       .Include(x => x.Clustername)
                       .Include(x => x.Assetclustertype)
                       .Include(x => x.Assetcapacityinfo).AsQueryable();

            return query;
        }


        private Dictionary<string, Expression<Func<AssetHardwareAncillary, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetHardwareAncillary, object>>[]>
            {
                ["assetHardwareAncillaryId"] = new Expression<Func<AssetHardwareAncillary, object>>[] { p => p.AssetHardwareAncillaryId },
                ["majorHardwareBuildAsIsId"] = new Expression<Func<AssetHardwareAncillary, object>>[] { p => p.MajorHardwareBuildAsIsId },
                ["dataCenterId"] = new Expression<Func<AssetHardwareAncillary, object>>[] { p => p.DataCenterId },
                ["clusterNameId"] = new Expression<Func<AssetHardwareAncillary, object>>[] { p => p.ClusterNameId  },
                ["assetClusterId"] = new Expression<Func<AssetHardwareAncillary, object>>[] { p => p.AssetClusterId },
                ["assetClusterTypeId"] = new Expression<Func<AssetHardwareAncillary, object>>[] { p => p.AssetClusterTypeId },
              
              
            };
        }


        #endregion

        #region Asset Hardware Configuration

        public async Task<AssetHardwareAncillaryAddUpdateDto> CreatePageAssetHardwareAncillary(AssetAncillaryCreateEditDto dto)
        {

            try
            {
                var dataCenterResoure = _dropdownDataServiceManager.GetDataCenter()?.Where(x => x.Opcoid == dto.OpCoId).ToList();
                var clusterNameResource = _dropdownDataServiceManager.GetVnfClusterName();
                var assetClusterTypeResource = _dropdownDataServiceManager.GetAssetClusterType();
                var assetClusterResource = _dropdownDataServiceManager.GetAssetCluster();
                var hardwareResource = await GetMajorHardwareResource();
                var model = await Task.Run(() => new AssetHardwareAncillaryAddUpdateDto
                {
                    DataCenterResource = dataCenterResoure,
                    ClusterNameResource = clusterNameResource,
                    AssetClusterResource = assetClusterResource,
                    AssetClusterTypeResource = assetClusterTypeResource,
                  //  assetHardwareAncillaryGridDto = new AssetHardwareAncillaryGridDto(),
                    MajorHardwareResource = hardwareResource,
                    OpCoId = dto.OpCoId,

                     assetHardwareAncillaryGridDto = new AssetHardwareAncillaryGridDto
                     {
                         assetCapacityInfoGridDtos = new List<AssetCapacityInfoGridDto>
                             {
                                      new AssetCapacityInfoGridDto {  }
                         }
                     },
                });

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError("Aset Ancillary Create Page : " + ex);
                return null;

            }

           
        }

        public async Task <List<AssetAncillaryMajorHardwareResourceDto>> GetMajorHardwareResource()
        {
            var predicateResult = PredicateBuilder.New<Majorhardwarebuildasis>(true);
            var predicateInner = PredicateBuilder.New<Majorhardwarebuildasis>(true);


            predicateResult.And(x => x.Hardwaretype.ToLower() != ConstantValueFilter.Unknown);
            predicateResult.And(x => x.Buildconstruction.Iscloudasset == true);


            var query =await Task.Run(() =>   _repositoryWrapper.MajorHardwareBuildAsIs.FindByCondition(predicateResult)
                .Include(x => x.Orgeqpmanufacturer)
                .Include(x => x.Platform)
                .Include(x => x.Buildconstruction)
                .ToList()) ;

            var hardwareResource = query.Select(x => new AssetAncillaryMajorHardwareResourceDto
            {
                Key = x.Majorhardwarebuildasisid,
                Text = x.Orgeqpmanufacturer.Originalequipmentmanufacturer + "-" + x.Platform.Platform + "-" + x.Hardwaretype,
                //Text = x.Orgeqpmanufacturer.Originalequipmentmanufacturer+"-"+
                //x.Hardwaresolution+"-"+x.Platform.Platform+"-"+x.Hardwaretype,
                PhysicalServerHwModel = x.Platform.Platform,
                PhysicalServerHwModelId = x.Platformid,
                PhysicalServerVendor = x.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                PhysicalServerVendorId = x.Orgeqpmanufacturerid
            }).ToList();


            return hardwareResource;
        }

        public async Task<AssetHardwareAncillaryAddUpdateDto> EditPageAssetHardwareAncillary(AssetAncillaryCreateEditDto dto)
        {

            try
            {
                AssetHardwareAncillaryQueryDto filterDto = new AssetHardwareAncillaryQueryDto
                {
                    ElementName = new List<long> { dto.AssetId },
                };

                var assetAncillaryResource = await FindWithConditionAsync(filterDto);
                var dataCenterResoure = _dropdownDataServiceManager.GetDataCenter()?.Where(x => x.Opcoid == dto.OpCoId).ToList();
                var clusterNameResource = _dropdownDataServiceManager.GetVnfClusterName();
                var assetClusterTypeResource = _dropdownDataServiceManager.GetAssetClusterType();
                var assetClusterResource = _dropdownDataServiceManager.GetAssetCluster();
                var hardwareResource = await GetMajorHardwareResource();

                var data = assetAncillaryResource?.Items?.FirstOrDefault();

                if ((data?.MajorHardwareId != null) &&
                !hardwareResource.Where(x => x.Key == data?.MajorHardwareId).Any())
                {
                    var asisEntity = _repositoryWrapper.MajorHardwareBuildAsIs.FindByCondition(x => x.Majorhardwarebuildasisid == data.MajorHardwareId)
                                     .Include(x => x.Orgeqpmanufacturer)
                                     .Include(x => x.Platform)
                                     .Include(x => x.Buildconstruction).FirstOrDefault();

                    hardwareResource.Add(new AssetAncillaryMajorHardwareResourceDto
                    {
                        Key = asisEntity.Majorhardwarebuildasisid,
                        Text = asisEntity.Orgeqpmanufacturer.Originalequipmentmanufacturer + "-" + asisEntity.Platform.Platform + "-" + asisEntity.Hardwaretype,
                        PhysicalServerHwModel = asisEntity.Platform.Platform,
                        PhysicalServerHwModelId = asisEntity.Platformid,
                        PhysicalServerVendor = asisEntity.Orgeqpmanufacturer.Originalequipmentmanufacturer,
                        PhysicalServerVendorId = asisEntity.Orgeqpmanufacturerid
                    });

                }

                if (data == null)
                    data =  new AssetHardwareAncillaryGridDto
                    {
                        assetCapacityInfoGridDtos = new List<AssetCapacityInfoGridDto>
                             {
                                      new AssetCapacityInfoGridDto {  }
                         }
                    };


                var model = await Task.Run(() => new AssetHardwareAncillaryAddUpdateDto
                {
                    DataCenterResource = dataCenterResoure,
                    ClusterNameResource = clusterNameResource,
                    AssetClusterResource = assetClusterResource,
                    AssetClusterTypeResource = assetClusterTypeResource,
                    assetHardwareAncillaryGridDto = data,
                    MajorHardwareResource = hardwareResource,
                    OpCoId = dto.OpCoId,
                    AssetId = dto.AssetId
                });

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError("Aset Ancillary Edit Page : " + ex);
                return null;

            }


        }
        public async Task<ResultDto> AddOrUpdateAssetAncillary(AssetHardwareAncillaryAddUpdateDto dto)
        {
            try
            {
                if (dto == null || dto.assetHardwareAncillaryGridDto == null)
                {
                    return new ResultDto
                    {
                        Data = dto,
                        Warning = false,
                        Info = ResultMessages.AddOrUpdateFailed
                    };
                }

                var grid = dto.assetHardwareAncillaryGridDto;
                var assetId = dto.AssetId;

                // get existing ancillary   
                var existingAncillary = await _repositoryWrapper.AssetHardwareAncillaryRepository
                    .FindByCondition(x => x.Networkelementasplannedid == assetId)
                    .Include(x => x.Assetcapacityinfo)
                    .FirstOrDefaultAsync();

                var existingCapacityList = existingAncillary?.Assetcapacityinfo?.ToList()
                                           ?? new List<Assetcapacityinfo>();

                Assethardwareancillary ancillary;
 
                if (grid.AssetHardwareAncillaryId == 0)
                {
                    ancillary = new Assethardwareancillary
                    {
                        Networkelementasplannedid = grid.NetworkElementAsPlannedId
                    };

                    _repositoryWrapper.AssetHardwareAncillaryRepository.Create(ancillary);
                }
                else
                {
                    ancillary = existingAncillary ?? new Assethardwareancillary();

                    if (existingAncillary == null)
                        _repositoryWrapper.AssetHardwareAncillaryRepository.Create(ancillary);
                }

                // Assign updated fields
                ancillary.Majorhardwarebuildasisid = grid.MajorHardwareId != 0 ? grid.MajorHardwareId : null;
                ancillary.Datacenterid = grid.DataCenterId != 0 ? grid.DataCenterId : null;
                ancillary.Clusternameid = grid.ClusterNameId != 0 ? grid.ClusterNameId : null;
                ancillary.Assetclusterid = grid.AssetClusterId != 0 ? grid.AssetClusterId : null;
                ancillary.Assetclustertypeid = grid.AssetClusterTypeId != 0 ? grid.AssetClusterTypeId : null;

                if(ancillary.Assethardwareancillaryid != 0)
                _repositoryWrapper.AssetHardwareAncillaryRepository.Update(ancillary);
                await _repositoryWrapper.SaveAsync();

                
                // ADD / UPDATE CAPACITY INFO
                
                var incomingCapacity = grid.assetCapacityInfoGridDtos?.ToList()
                                         ?? new List<AssetCapacityInfoGridDto>();

                foreach (var item in incomingCapacity)
                {
                    var capacity = existingCapacityList
                        .FirstOrDefault(x => x.Assetcapacityinfoid == item.AssetCapacityInfoId);

                    if (capacity == null)
                    {
                        capacity = new Assetcapacityinfo
                        {
                            Assethardwareancillaryid = ancillary.Assethardwareancillaryid
                        };
                        _repositoryWrapper.AssetCapacityInfoRepository.Create(capacity);
                        await _repositoryWrapper.SaveAsync();
                    }

                    capacity.Physicalserverhostname = item.PhysicalServerHostName;
                    capacity.Physicalserveripaddress = item.PhysicalServerIpAddress;
                    capacity.Physicalserverserialnumber = item.PhysicalServerSerialNumber;
                    capacity.Memory = item.Memory;
                    capacity.Vcpu = item.Vcpu;
                    capacity.Noofinstances = item.NoOfInstances;
                    capacity.Storage = item.Storage;
                    if(capacity.Assetcapacityinfoid != 0)
                    _repositoryWrapper.AssetCapacityInfoRepository.Update(capacity);
                }

                await _repositoryWrapper.SaveAsync();

                
                // delete removed capacityinfor
               
                var incomingIds = incomingCapacity
                    .Where(x => x.AssetCapacityInfoId != 0)
                    .Select(x => x.AssetCapacityInfoId)
                    .ToList();

                var toDelete = existingCapacityList
                    .Where(x => !incomingIds.Contains(x.Assetcapacityinfoid))
                    .ToList();

                foreach (var cap in toDelete)
                    _repositoryWrapper.AssetCapacityInfoRepository.DeleteDeep(cap);

                if (toDelete.Any())
                    await _repositoryWrapper.SaveAsync();

                await _repositoryWrapper.ClearTracker();

                return new ResultDto
                {
                    Data = dto,
                    Warning = true,
                    Info = ResultMessages.EntryAddSuccess
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Asset Ancillary Add/Update/Delete Exception: " + ex);

                return new ResultDto
                {
                    Data = dto,
                    Warning = false,
                    Info = ResultMessages.AddOrUpdateFailed
                };
            }
        }
 
        #endregion



    }
}
