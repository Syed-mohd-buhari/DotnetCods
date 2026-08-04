using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Vbom;
using CAM.Entities.Models.CBom;
using CAM.Entities.Models.Lookup;
using CAM.Entities.Models.VBom;
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
namespace CAM.BusinessManager.Entity.XBom.VBom
{
    public class VBomManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        public VBomManager(IEnumerable<IRepositoryWrapper> wrappers,
            IMapper mapper,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            IProductNameManager productNameManager,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            CommonManager commonManager, DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dropdownDataServiceManager = dropdownDataServiceManager;

        }

        //     #region Grid Load, Filter , ApplyFilter
        private IQueryable<Vnfclusterinfo> GetVbomRecords(ExpressionStarter<Vnfclusterinfo> predicateResult)
        {


            var query = _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfvmcapacity)
                .Include(x => x.Opco)
                .Include(x => x.Location)
                .Include(x => x.Clustername)
                .Include(x => x.Vnfinfo).ThenInclude(x => x.Intravmtype)
                .Include(x => x.Vnfinfo).ThenInclude(x => x.Intervmtype)
                .Include(x => x.Vnfinfo).ThenInclude(x => x.Vmworkloadtype)
                .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfname)
                .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfvmtypename)
                .Include(x=>x.Hardwaretype)
                .AsQueryable();

            return query;
        }
        private IQueryable<Vnfclusterinfo> GetVnfClusterEntities(ExpressionStarter<Vnfclusterinfo> predicateResult)
        {


            var query = _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(predicateResult)
               .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
               .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfvmcapacity)
               .Include(x => x.Opco)
               .Include(x => x.Location)
               .Include(x => x.Clustername)
               .Include(x => x.Vnfinfo).ThenInclude(x => x.Intravmtype)
               .Include(x => x.Vnfinfo).ThenInclude(x => x.Intervmtype)
               .Include(x => x.Vnfinfo).ThenInclude(x => x.Vmworkloadtype)
               .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfname)
               .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfvmtypename)
               .Include(x=>x.Hardwaretype)
               .AsQueryable();

            return query;
        }

        public async Task<QueryResultDto<VnfClusterInfoDtoGrid>> VnfClusterBasedEntities(VnfClusterInfoQueryDto dynamicFilterDto)
        {
            var predicateResult = ApplyFilter(dynamicFilterDto);

            var rtn = new QueryResultDto<VnfClusterInfoDtoGrid>(new GenerateRenderForGrid<VnfClusterInfoDtoGrid>(_customColumnManager))
            {

            };
            var vnfinfoQueryResult = await Task.Run(() => GetVnfClusterEntities(predicateResult).AsEnumerable()
                      .Select(p => VnfClusterInfoMapper.GetVnfClusterInfo(p)).AsQueryable());

            rtn.TotalItems = vnfinfoQueryResult.Count();
            var paginatedRecords = await Task.Run(() => vnfinfoQueryResult.ApplyOrdering(dynamicFilterDto, GetColumnsMap())
                );

            paginatedRecords = paginatedRecords.ApplyPaging(dynamicFilterDto);

            var mappedVnfClusterEntity = paginatedRecords.ToList()
              .Select(vnInfo => new VnfClusterInfoDtoGrid
              {

                  VnfClusterInfoId = vnInfo.VnfClusterInfoId,
                  OpCoId = vnInfo.OpcoId,
                  OpCoDescritpion = vnInfo.OpcoIdDescription,
                  ShortLocationId = vnInfo.LocationId,
                  LocationName = vnInfo.LocationName,
                  SiteName = vnInfo.LocationShortDescription,
                  ClusterId = vnInfo.ClusterNameId,
                  ClusterDescription = vnInfo.ClusterName,
                  LastModifiedBy = vnInfo.ModificationUserEntity.Email,
                  LastModified = vnInfo.ModificationDate,
                  NoOfBlades = vnInfo.NoOfBlades,
                  FileName = vnInfo.FileName,
                  HardwareType = vnInfo.HardwareType,
                  HardwareTypeId=vnInfo.HardwareTypeId,
                  Revision = vnInfo.Revision,


              }).OrderBy(x => x.OpCoDescritpion)
          .ThenBy(x => x.LocationName)
          .ThenBy(x => x.SiteName).ThenBy(x => x.HardwareType).ToList();


            rtn.Items = mappedVnfClusterEntity.ToArray();
            return rtn;

        }

        public async Task<QueryResultDto<VnfVbomInfoDtoGrid>> GetVnfInfoAndCapacityEntities(VnfClusterInfoQueryDto dynamicFilterDto)
        {
            var predicateResult = ApplyFilterForInstance(dynamicFilterDto);

            var rtn = new QueryResultDto<VnfVbomInfoDtoGrid>(new GenerateRenderForGrid<VnfVbomInfoDtoGrid>(_customColumnManager))
            {

            };
            var vnfinfoQueryResult = await Task.Run(() => GetVnfInstanceRecords(predicateResult).AsEnumerable()
                      .Select(p => VnfInfoMapper.GetVnfInfo(p)).ToList());

            rtn.TotalItems = Convert.ToInt16(vnfinfoQueryResult?.ToList().Count());

            var capacityPredicateResult = ApplyFilterForCapacityEntity(dynamicFilterDto);

            if (capacityPredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in vnfinfoQueryResult)
                {
                    item.VnfVmCapacity = item.VnfVmCapacity.Where(capacityPredicateResult).ToList();
                }
            }            

            var paginatedRecords = await Task.Run(() => vnfinfoQueryResult.AsQueryable().ApplyOrdering(dynamicFilterDto, GetColumnsMapForInstanceCapacityInfo())
                );
            paginatedRecords = paginatedRecords.ApplyPaging(dynamicFilterDto);
            var mappedVnfInfoCapacity = paginatedRecords.ToList()
             .Select(insta => new VnfVbomInfoDtoGrid
             {
                 VnfInfoId = insta != null ? insta.VnfInfoId : 0,
                 VnfNameDescritpion = insta != null ? Convert.ToString(insta.VnfNameDesc) : null,
                 VnfVmtypenameid = insta != null ? insta.VnfVmTypeNameId : 0,

                 VnfVmTypeNameDescription = insta != null ? Convert.ToString(insta.VnfVmTypeNameDesc) : null,
                 vnfNameId = insta != null ? insta.VnfNameId : 0,

                 IntraVmType = insta != null ? Convert.ToString(insta.IntraVmTypeDesc) : null,
                 InterVmType = insta != null ? Convert.ToString(insta.InterVmTypeDesc) : null,
                 VmWorkLoadType = insta != null ? Convert.ToString(insta.VmWorkLoadTypeDesc) : null,

                 Numa = insta != null ? Convert.ToString(insta.Numa) : string.Empty,
                 Socket = insta != null ? insta.Socket : string.Empty,

                 Nsxt = insta != null ? insta.Nsxt == true ? ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,
                 VmStorageBlockSize = insta != null ? insta.VmStorageBlockSize : string.Empty,

                 _vnfVbomCapacityDtoGrid = insta.VnfVmCapacity.Select(capacity => new VnfVbomCapacityDtoGrid
                 {

                     VnfVmCapacityId = capacity != null ? capacity.VnfVmCapacityId : 0,
                     FinancialYear = capacity != null ? Convert.ToString(capacity.FinancialYear) : null,
                     FinancialVersion = capacity != null ? Convert.ToString("H" + capacity.FinancialVersion) : null,
                     VcpuPerVm = capacity != null ? Convert.ToString(capacity.VnfCpuPerVm) : null,
                     RxTxCpuCount = capacity != null ? Convert.ToString(capacity.RxTxCpuCount) : null,
                     RamPerVm = capacity != null ? Convert.ToString(capacity.RamPerVm) : null,
                     DataDisk = Convert.ToInt32(capacity.DataDisk),
                     OsDisk = capacity?.OsDisk,
                     IopsRunning = capacity != null ? Convert.ToString(capacity.IopsRunning) : null,
                     IopsLoading = capacity != null ? Convert.ToString(capacity.IopsLoading) : null,
                     VmWorkLoadDistribution = capacity != null ? capacity.VmWorkLoadDistribution : null,
                     NorthDouthBoundBandWidth = capacity != null ? capacity.NorthDouthBoundBandWidth : null,
                     EastWestBoundBandWidth = capacity != null ? capacity.EastWestBoundBandWidth : null,
                     OtherRequirements = capacity != null ? capacity.OtherRequirements : null,
                     BackupRequired = capacity != null ? Convert.ToString(capacity.BackupRequired) : null,
                     ProbIngRequired = capacity != null ? Convert.ToString(capacity.ProbIngRequired) : null,

                     NoOfVnfInstances = insta != null ? Convert.ToString(capacity.NoOfVnfInstances) : null,
                     NoOfVmsPerType = insta != null ? Convert.ToString(capacity.NoOfVmsPerType) : null,
                     ShortLocationId = insta != null ? insta.LocationId : 0,
                     SiteName = insta != null ? insta.SiteName : null,
                     LocationName = insta != null ? insta.LocationName : null,
                 }).OrderByDescending(x => x.FinancialYear).ThenBy(x => x.FinancialVersion).ToList()


             }).OrderBy(x => x.VnfNameDescritpion)
          .ThenBy(x => x.VnfVmTypeNameDescription).ToList();

            rtn.Items = mappedVnfInfoCapacity.ToArray();
            return rtn;

        }
        private Dictionary<string, Expression<Func<VnfClusterInfo, object>>[]> GetColumnsMap()
        {
            var returnVnfInfoDict = new Dictionary<string, Expression<Func<VnfClusterInfo, object>>[]>
            {
                ["vnfClusterInfoId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfClusterInfoId },
                ["opcoId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.OpcoId },
                ["locationId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.LocationId },
                ["clusterNameId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.ClusterNameId },
                ["noOfBlades"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.NoOfBlades },


                ["vnfInfoId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().VnfInfoId },
                ["vnfNameId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().VnfNameId },
                ["vnfClusterInfoId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().VnfClusterInfoId },
                ["vnfVmTypeNameId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().VnfVmTypeNameId },
                ["nsxt"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().Nsxt },
                ["intraVmTypeId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().IntraVmTypeId },
                ["interVmTypeId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().InterVmTypeId },
                ["vmWorkLoadTypeId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().VmWorkLoadTypeId },
                ["vmStorageBlockSize"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().VmStorageBlockSize },

                ["numa"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().Numa },
                ["socket"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().Socket },



                ["vnfVmCapacityId"] = new Expression<Func<VnfClusterInfo, object>>[] { p => p.VnfInfo.FirstOrDefault().VnfVmCapacity.FirstOrDefault().VnfVmCapacityId },


            };

            return returnVnfInfoDict;
        }

        #region Apply Filter
        public ExpressionStarter<Vnfclusterinfo> ApplyFilter(VnfClusterInfoQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Vnfclusterinfo>(true);


            #region vnfclusterInfo
            if (filterDto.VnfNameDescritpion?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var id in filterDto.VnfNameDescritpion)
                    componentIdPredicate.Or(x => x.Vnfinfo.Any(y=>y.Vnfnameid.ToString()==id));
                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.HardwareType?.Any() == true)
            {
                if (filterDto.HardwareType.All(x => x != "-1"))
                {
                    var componentIdPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                    foreach (var id in filterDto.HardwareType)
                        componentIdPredicate.Or(x => x.Hardwaretypeid.ToString() == id);

                    mainPredicate.And(componentIdPredicate);

                }
            }
            if (filterDto.FileName?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var id in filterDto.FileName)
                    componentIdPredicate.Or(x => x.Filename == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.Revision?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var id in filterDto.Revision)
                    componentIdPredicate.Or(x => x.Revision == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.VnfClusterInfoId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var id in filterDto.VnfClusterInfoId)
                    componentIdPredicate.Or(x => x.Vnfclusterinfoid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.OpcoDescritpion?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var item in filterDto.OpcoDescritpion)
                    descriptionPredicate.Or(x => x.Opcoid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.LocationName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var item in filterDto.LocationName)
                    descriptionPredicate.Or(x => x.Location.Locationid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.SiteName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var item in filterDto.SiteName)
                    descriptionPredicate.Or(x => x.Locationid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.ClusterDescription?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var item in filterDto.ClusterDescription)
                    descriptionPredicate.Or(x => x.Clusternameid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.NoOfBlades?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var item in filterDto.NoOfBlades)
                    descriptionPredicate.Or(x => x.Noofblades == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }
            if (filterDto.LastModified != null)
            {
                var LastModifiedValuePredicate = PredicateBuilder.New<Vnfclusterinfo>();
                if (filterDto.LastModified.StartDate != null)
                {
                    _ = LastModifiedValuePredicate.And(x => x.Modificationdate.Date >= filterDto.LastModified.StartDate);
                }

                if (filterDto.LastModified.EndDate != null)
                {
                    _ = LastModifiedValuePredicate.And(x => x.Modificationdate.Date <= filterDto.LastModified.EndDate);
                }

                _ = mainPredicate.And(LastModifiedValuePredicate);
            }

            #endregion

            if (filterDto.LastModifiedValue != null)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                if (filterDto.LastModifiedValue.StartDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date >= filterDto.LastModifiedValue.StartDate);
                if (filterDto.LastModifiedValue.EndDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date <= filterDto.LastModifiedValue.EndDate);
                mainPredicate.And(componentIdPredicate);
            }

            return mainPredicate;
        }

        #endregion

        #region Get Filter

        public async Task<List<FilterValueDto>> GetFilteredValues(string propertyName, string propertyFilter, VnfClusterInfoQueryDto filterDto)
        {   
            var filterCriteria = ApplyFilter(filterDto);

            var vnfinfoQueryResult = await VnfClusterBasedEntities(filterDto);

            var filteredQuery = vnfinfoQueryResult.Items;

            var result = propertyName switch
            {
                #region VnfInfo Fields
                "vnfClusterInfoId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VnfClusterInfoId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.VnfClusterInfoId))
                    .Distinct()
                    .ToList(),

                "opCoDescritpion" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OpCoId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto { Value = x.OpCoId.ToString(), Text = x.OpCoDescritpion })
                    .Distinct().OrderBy(x => x.Text)
                    .ToList(),

                "siteName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ShortLocationId.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.ShortLocationId.ToString(), Text = x.SiteName })
                .Distinct()
                .ToList(),

                "locationName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClusterId.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.ShortLocationId.ToString(), Text = x.LocationName })
                .Distinct()
                .ToList(),

                "clusterDescription" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClusterId.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Value = x.ClusterId.ToString(), Text = x.ClusterDescription })
               .Distinct()
               .ToList(),

                "noOfBlades" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NoOfBlades.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.NoOfBlades))
               .Distinct()
               .ToList(),

                "hardwareType" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HardwareTypeId.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.HardwareTypeId.ToString(), Text = x.HardwareType })
                .Distinct().OrderBy(x => x.Text)
                .ToList(),

                "fileName" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.FileName.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.FileName))
               .Distinct()
               .ToList(),

                "revision" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Revision.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Revision))
                .Distinct()
                .ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? filteredQuery.Select(p => new FilterValueDto(p.LastModifiedBy)).Distinct().ToList()
                    : filteredQuery
                        .Where(x =>
                            x.LastModifiedBy.Contains(
                                propertyFilter)).Select(p => new FilterValueDto(p.LastModifiedBy)).Distinct().ToList(),


                #endregion

                _ => new List<FilterValueDto>()
            };

            if (propertyName == "hardwareType")
            {
                result.Insert(0,new FilterValueDto { Text = "All", Value = "-1" });
            }
            return result;
        }

        #endregion

        #region Create New VBOM resource
        public async Task<VBomCreatePageEntityDto> GetCreateVnfVBomPageResourceAsync(List<short> opcoList)
        {
            var vnfNameForProductResource = _dropdownDataServiceManager.GetVnfName();
            var clusterNameResource = _dropdownDataServiceManager.GetVnfClusterName();
            var vnfVmTypeResource = _dropdownDataServiceManager.GetVnfVmTypeDetails();
            var LocationDetails = await _dropdownDataServiceManager.GetAllLocationsBasedOpcos(opcoList);
            var intraVmDetails = _dropdownDataServiceManager.GetAllIntraTypeVM();
            var interVmDetails = _dropdownDataServiceManager.GetAllInterTypeVM();
            var vmWorkLoadTypeDetail = _dropdownDataServiceManager.GetAllVmWorkLoadType();
            var financialVersion = _dropdownDataServiceManager.GetAllFinancialVersion();
            var harwareTypeResource = _dropdownDataServiceManager.GetVOMHardwareType();
            var model = new VBomCreatePageEntityDto
            {
                _VnfClusterInfoDto = new Vnfclusterinfo
                {
                    Vnfinfo = new List<Vnfinfo>
                             {
                              new Vnfinfo
                                 {
                                     Vnfvmcapacity = new List<Vnfvmcapacity>(){ new Vnfvmcapacity()}
                                 }
                         }
                },

                VnfNameResources = vnfNameForProductResource,
                VnClusterNameResource = clusterNameResource,
                VnVmTypeNameResource = vnfVmTypeResource,
                OpcoBasedLocationResource = LocationDetails,
                IntraVmTypeResource = intraVmDetails,
                InterTypeResource = interVmDetails,
                VmWorkLoadTypeDetail = vmWorkLoadTypeDetail,
                FinancialVersion = financialVersion,
                HardwareTypeResource = harwareTypeResource,
            };
            return model;

        }

        #endregion

        #region Add VBOM 

        public async Task<Vnfclusterinfo> EntityExists(VBomInsertUpdateDto dto, bool isUpdate = false)
        {
            var entityExists = await _repositoryWrapper.VnfClusterInfoRepository.
                FindByCondition(
                    x => x.Opcoid == dto._VnfClusterInfoDto.Opcoid &&
                    x.Locationid == dto._VnfClusterInfoDto.Locationid &&
                    x.Clusternameid == dto._VnfClusterInfoDto.Clusternameid &&
                     x.Hardwaretypeid == dto._VnfClusterInfoDto.HardwareTypeId &&
                     x.Revision == dto._VnfClusterInfoDto.Revision
                    , true)
                .OrderByDescending(x => x.Creationdate).ToListAsync();

            if (isUpdate && entityExists.Count > 0)
            {
                var notUpdatedEntity = entityExists.Where(x => x.Vnfclusterinfoid != dto._VnfClusterInfoDto.Vnfclusterinfoid).FirstOrDefault();
                return notUpdatedEntity == null ? null : notUpdatedEntity;
            }
            return entityExists.FirstOrDefault();
        }
        public async Task<ResultDto> AddVBomDetailAsync(VBomInsertUpdateDto dto, bool isUpdate = false, bool isImportData = false)
        {
            var entityExists = await EntityExists(dto, isUpdate);

            if (entityExists != null && !isImportData)
            {
                try
                {

                    if (entityExists.Deleted == true)
                    {

                        new ResultDto
                        {
                            Warning = true,
                            Info = ResultMessages.EntryAddExists,
                            Data = new { id = entityExists.Vnfclusterinfoid, orphanDeleted = true }
                        };

                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = (bool)entityExists?.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entityExists.Vnfclusterinfoid
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }


            }
            return await AddVnfVmClusterInfoBaseAsync(dto, isUpdate);
        }

        public async Task<ResultDto> AddVnfVmClusterInfoBaseAsync(VBomInsertUpdateDto dtoVnfVnInfo, bool isUpdate = false)
        {

            Vnfclusterinfo clusterInfoEntity;
            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    if (dtoVnfVnInfo._VnfClusterInfoDto.Vnfclusterinfoid == 0)
                    {
                        clusterInfoEntity = new Vnfclusterinfo
                        {
                            Opcoid = dtoVnfVnInfo._VnfClusterInfoDto.Opcoid,
                            Locationid = dtoVnfVnInfo._VnfClusterInfoDto.Locationid,
                            Clusternameid = dtoVnfVnInfo._VnfClusterInfoDto.Clusternameid,
                            Noofblades = dtoVnfVnInfo._VnfClusterInfoDto.Noofblades,
                            Hardwaretypeid = dtoVnfVnInfo._VnfClusterInfoDto.HardwareTypeId,
                            Filename = dtoVnfVnInfo._VnfClusterInfoDto.FileName,
                            Revision = dtoVnfVnInfo._VnfClusterInfoDto.Revision,
                        };

                        _repositoryWrapper.VnfClusterInfoRepository.Create(clusterInfoEntity);
                    }
                    else
                    {
                        clusterInfoEntity = _repositoryWrapper.VnfClusterInfoRepository
                            .FindByCondition(x => x.Vnfclusterinfoid == dtoVnfVnInfo._VnfClusterInfoDto.Vnfclusterinfoid)
                            .FirstOrDefault();

                        if (clusterInfoEntity == null)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryNotFound,
                                Data = dtoVnfVnInfo
                            };
                        }

                        clusterInfoEntity.Opcoid = dtoVnfVnInfo._VnfClusterInfoDto.Opcoid;
                        clusterInfoEntity.Locationid = dtoVnfVnInfo._VnfClusterInfoDto.Locationid;
                        clusterInfoEntity.Clusternameid = dtoVnfVnInfo._VnfClusterInfoDto.Clusternameid;
                        clusterInfoEntity.Noofblades = dtoVnfVnInfo._VnfClusterInfoDto.Noofblades;
                        clusterInfoEntity.Hardwaretypeid = dtoVnfVnInfo._VnfClusterInfoDto.HardwareTypeId;
                        clusterInfoEntity.Filename = dtoVnfVnInfo._VnfClusterInfoDto.FileName;
                        clusterInfoEntity.Revision = dtoVnfVnInfo._VnfClusterInfoDto.Revision;
                        _repositoryWrapper.VnfClusterInfoRepository.Update(clusterInfoEntity);
                    }

                    await _repositoryWrapper.SaveAsync();


                    var instanceCapacityErrorEntity = await AddVnfVmInfoInstance(
                            dtoVnfVnInfo._VnfClusterInfoDto.vnfinfo,
                            clusterInfoEntity.Vnfclusterinfoid
                        );

                    if (instanceCapacityErrorEntity.Count > 0)
                    {
                        await transaction.RollbackAsync();

                        return new ResultDto
                        {
                            Warning = true,
                            Info = instanceCapacityErrorEntity.Any(x => x == ResultMessages.duplicateInfo) ? ResultMessages.duplicateInfo : ResultMessages.duplicateCapacity,
                            Data = instanceCapacityErrorEntity.ToList()
                        };
                    }
                    else
                    {
                        await transaction.CommitAsync();
                    }

                    await _repositoryWrapper.ClearTracker();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }



            return new ResultDto
            {
                Warning = false,
                Info = !isUpdate ? ResultMessages.EntryAddSuccess : ResultMessages.EntryUpdateSuccess,
                Data = clusterInfoEntity
            };
        }
        public async Task<List<string>> AddVnfVmInfoInstance(List<VnfInfoDto> dtoInstances, long Vnfclusterinfoid)
        {

            var instanceErrorEntity = new List<string>();
            try
            {
                if (dtoInstances != null && dtoInstances.Count > 0)
                {

                    var existingInstanceEntities = _repositoryWrapper.VnfInfoRepository
                           .FindByCondition(x =>
                               x.Vnfclusterinfoid == Vnfclusterinfoid
                              // && x.Vnfinfoid == instanceItem.Vnfinfoid
                              )
                           .ToList();

                    foreach (var instanceItem in dtoInstances)
                    {

                        var checkDuplicatesinstanceItem = existingInstanceEntities.Where(x => x.Vnfvmtypenameid == instanceItem.Vnfvmtypenameid
                        && x.Vnfnameid == instanceItem.Vnfnameid &&
                        x.Vnfinfoid != instanceItem.Vnfinfoid).FirstOrDefault();
                        if (checkDuplicatesinstanceItem != null)
                        {
                            instanceErrorEntity.Add(ResultMessages.duplicateInfo);
                            continue;
                        }
                        var existingInstance = existingInstanceEntities.Where(x => x.Vnfinfoid == instanceItem.Vnfinfoid).FirstOrDefault();

                        Vnfinfo vmInstance;

                        if (instanceItem.Vnfinfoid == 0 && existingInstance == null)
                        {

                            vmInstance = new Vnfinfo();
                            _repositoryWrapper.VnfInfoRepository.Create(vmInstance);
                        }
                        else if (instanceItem.Vnfinfoid != 0 && existingInstance != null)
                        {

                            vmInstance = existingInstance;
                        }
                        else
                            continue;


                        // Common properties
                        vmInstance.Vnfclusterinfoid = Vnfclusterinfoid;
                        vmInstance.Vmworkloadtypeid = instanceItem.Vmworkloadtypeid;
                        vmInstance.Intervmtypeid = instanceItem.Intervmtypeid;
                        vmInstance.Intravmtypeid = instanceItem.Intravmtypeid;
                        vmInstance.Vnfvmtypenameid = instanceItem.Vnfvmtypenameid;
                        vmInstance.Vnfnameid = instanceItem.Vnfnameid;
                        vmInstance.Numa = instanceItem.Numa;
                        vmInstance.Socket = instanceItem.Socket;
                        vmInstance.Vmstorageblocksize = instanceItem.Vmstorageblocksize;
                        vmInstance.Nsxt = instanceItem.Nsxt;


                        if (instanceItem.Vnfinfoid != 0 && existingInstance != null)
                            _repositoryWrapper.VnfInfoRepository.Update(vmInstance);

                        await _repositoryWrapper.SaveAsync();

                        instanceItem.Vnfinfoid = vmInstance.Vnfinfoid;
                        var capacityReturnEntity = await AddVnfVmCapacity(instanceItem.vnfvmcapacity, instanceItem.Vnfinfoid);

                        if (capacityReturnEntity.Count > 0)
                        {
                            instanceErrorEntity.AddRange(capacityReturnEntity);

                        }


                    }


                }
            }
            catch (Exception)
            {

                throw;
            }

            return instanceErrorEntity;

        }

        public async Task<List<string>> AddVnfVmCapacity(List<VnfVmCapacityDto> dtoCapacity, long vnfInfoId)
        {

            var errorCapacityError = new List<string>();
            try
            {
                if (dtoCapacity != null)
                {
                    var existingCapacity = _repositoryWrapper.VnfVmCapacityRepository.FindByCondition(x => x.Vnfinfoid == vnfInfoId
          ).ToList();


                    foreach (var capacityItem in dtoCapacity)
                    {

                        long vnfVmCapacityid = capacityItem.Vnfvmcapacityid;

                        Vnfvmcapacity vmCapacity = new Vnfvmcapacity();

                        var duplicateCapacities = existingCapacity.Where(x => x.Financialyear == capacityItem.Financialyear
                        && x.Financialversion == capacityItem.Financialversion && x.Vnfvmcapacityid != vnfVmCapacityid).ToList();


                        if (vnfVmCapacityid == 0 && !duplicateCapacities.Any())
                        {

                            _repositoryWrapper.VnfVmCapacityRepository.Create(vmCapacity);
                        }
                        else
                        {


                            if (duplicateCapacities != null && duplicateCapacities.Count > 0)
                            {
                                errorCapacityError.Add(ResultMessages.duplicateCapacity + capacityItem.Financialyear);
                                continue;
                            }
                            else
                            {
                                var existInstCapacity = existingCapacity.Where(x => x.Vnfvmcapacityid == vnfVmCapacityid).FirstOrDefault();
                                if (existInstCapacity != null)
                                {
                                    vmCapacity = existInstCapacity ?? new Vnfvmcapacity();

                                }
                            }
                        }

                        vmCapacity.Vnfinfoid = vnfInfoId;
                        vmCapacity.Financialyear = capacityItem.Financialyear;
                        vmCapacity.Financialversion = capacityItem.Financialversion;
                        vmCapacity.Vnfcpupervm = capacityItem.Vnfcpupervm;
                        vmCapacity.Rxtxcpucount = capacityItem.Rxtxcpucount;
                        vmCapacity.Rampervm = capacityItem.Rampervm;
                        vmCapacity.Datadisk = capacityItem.Datadisk;
                        vmCapacity.Osdisk = capacityItem.Osdisk;
                        vmCapacity.Iopsrunning = capacityItem.Iopsrunning;
                        vmCapacity.Iopsloading = capacityItem.Iopsloading;
                        vmCapacity.Vmworkloaddistribution = capacityItem.Vmworkloaddistribution;
                        vmCapacity.Northsouthboundbandwidth = capacityItem.Northsouthboundbandwidth;
                        vmCapacity.Eastwestboundbandwidth = capacityItem.Eastwestboundbandwidth;
                        vmCapacity.Otherrequirements = capacityItem.Otherrequirements;
                        vmCapacity.Backuprequired = capacityItem.Backuprequired;
                        vmCapacity.Probingrequired = capacityItem.Probingrequired;
                        vmCapacity.Noofvmspertype = capacityItem.Noofvmspertype;
                        vmCapacity.Noofvnfinstances = capacityItem.Noofvnfinstances;

                        if (vmCapacity.Vnfvmcapacityid == 0)
                            _repositoryWrapper.VnfVmCapacityRepository.Create(vmCapacity);
                        else
                            _repositoryWrapper.VnfVmCapacityRepository.Update(vmCapacity);

                        await _repositoryWrapper.SaveAsync();


                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

            return errorCapacityError;
        }

        #endregion
        #region Edit and Update 
        public async Task<ResultDto> GetEditVnfVBomPageResourceAsync(long clusterInfoId,List<short> opcoList)
        {
            var vnfNameForProductResource = _dropdownDataServiceManager.GetVnfName();
            var clusterNameResource = _dropdownDataServiceManager.GetVnfClusterName();
            var vnfVmTypeResource = _dropdownDataServiceManager.GetVnfVmTypeDetails();
            var LocationDetails = await _dropdownDataServiceManager.GetAllLocationsBasedOpcos(opcoList);

            var intraVmDetails = _dropdownDataServiceManager.GetAllIntraTypeVM();
            var interVmDetails = _dropdownDataServiceManager.GetAllInterTypeVM();
            var vmWorkLoadTypeDetail = _dropdownDataServiceManager.GetAllVmWorkLoadType();

            var financialVersion = _dropdownDataServiceManager.GetAllFinancialVersion();

            var hardwareTypeResource = _dropdownDataServiceManager.GetVOMHardwareType();

            var vnfInFoEntity = _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Vnfclusterinfoid == clusterInfoId)
              .Include(x => x.Vnfinfo).ThenInclude(x => x.Vnfvmcapacity).FirstOrDefault();

            if (vnfInFoEntity == null)
            {
                return new ResultDto() { Info = ResultMessages.EntryDeleteNotExists, Warning = true };
            }

            var vnInfoEntity = vnfInFoEntity.Vnfinfo;
            if (vnInfoEntity != null && vnInfoEntity.Count > 0)
            {
                foreach (var vn in vnInfoEntity)
                {
                    if (vnfNameForProductResource.Any(x => x.Key == vn.Vnfnameid) == false)
                    {
                        var tempVfName = _repositoryWrapper.VnfNameRepository.FindByCondition(t => t.Vnfnameid == vn.Vnfnameid).FirstOrDefault();
                        if (tempVfName != null)
                        {
                            vnfNameForProductResource.Add(new KeyValuePairDto { Key = (short)tempVfName.Vnfnameid, Text = tempVfName.Vnfdescription });
                        }
                    }

                    if (vnfVmTypeResource.Any(x => x.Vmtypenameid == vn.Vnfvmtypenameid) == false)
                    {
                        var tempVfName = _repositoryWrapper.VmTypeNameRepository.FindByCondition(t => t.Vmtypenameid == vn.Vnfvmtypenameid).FirstOrDefault();
                        if (tempVfName != null)
                        {
                            vnfVmTypeResource.Add(tempVfName);
                        }
                    }
                }
            }


            if (clusterNameResource.Any(x => x.Key == vnfInFoEntity.Clusternameid) == false)
            {
                var tempVfName = _repositoryWrapper.ClusterNameRepository.FindByCondition(t => t.Clusternameid == vnfInFoEntity.Clusternameid).FirstOrDefault();
                if (tempVfName != null)
                {
                    clusterNameResource.Add(new KeyValuePairDto { Key = (short)tempVfName.Clusternameid, Text = tempVfName.Clusterdescription });
                }
            }

            var model = new VBomCreatePageEntityDto
            {
                VnfNameResources = vnfNameForProductResource,
                VnClusterNameResource = clusterNameResource,
                VnVmTypeNameResource = vnfVmTypeResource,
                OpcoBasedLocationResource = LocationDetails,
                _VnfClusterInfoDto = vnfInFoEntity,
                InterTypeResource = interVmDetails,
                IntraVmTypeResource = intraVmDetails,
                VmWorkLoadTypeDetail = vmWorkLoadTypeDetail,
                FinancialVersion = financialVersion,
                HardwareTypeResource = hardwareTypeResource

            };
            return new ResultDto()
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = model
            };

        }
        #endregion
        #region Delete Record     
        public async Task<ResultDto> GetLinkedInfoBasedOnCapacity(long id)
        {
            var vnfCapacityEntity = await _repositoryWrapper.VnfVmCapacityRepository
                          .FindByCondition(x => x.Vnfvmcapacityid == id)
                          .Include(x => x.Vnfinfo)
                              .ThenInclude(i => i.Vnfclusterinfo)
                          .FirstOrDefaultAsync();

            if (vnfCapacityEntity == null)
                return new ResultDto { Data = 0 }; // Or handle as a not found case

            var vnfInfoInstanceId = vnfCapacityEntity.Vnfinfoid;
            var vnfClusterInfoId = vnfCapacityEntity.Vnfinfo.Vnfclusterinfoid;

            var multipleCapacityExists = await _repositoryWrapper.VnfVmCapacityRepository
                .FindByCondition(x => x.Vnfinfoid == vnfInfoInstanceId && x.Vnfvmcapacityid != id)
                .AnyAsync();

            var multipleInstanceExists = await _repositoryWrapper.VnfInfoRepository
                .FindByCondition(x => x.Vnfinfoid != vnfInfoInstanceId && x.Vnfclusterinfoid == vnfClusterInfoId)
                .AnyAsync();

            if (!multipleCapacityExists && !multipleInstanceExists)
            {
                // Delete: ClusterInfo + Info + Capacity
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = 3
                };
            }
            else if (!multipleCapacityExists && multipleInstanceExists)
            {
                // Delete: Info + Capacity
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = 2
                };
            }
            else
            {
                // Delete: Capacity only
                return new ResultDto
                {
                    Info = ResultMessages.EntryDeleteNotOrphan,
                    Data = 1
                };
            }
        }


        public async Task<ResultDto> DeleteVnfCapacityBasedAsync(long id)
        {
            int deletedTableCount = Convert.ToInt16(GetLinkedInfoBasedOnCapacity(id).Result.Data.ToString());

            #region  

            var vnfCapacityEntity = await _repositoryWrapper.VnfVmCapacityRepository.FindByCondition(x => x.Vnfvmcapacityid == id).FirstOrDefaultAsync();

            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    if (vnfCapacityEntity != null)
                    {
                        if (deletedTableCount == 1)
                        {
                            _repositoryWrapper.VnfVmCapacityRepository.DeleteDeep(vnfCapacityEntity);
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                        }
                        else
                        {
                            var vnfInstancesEntity = await _repositoryWrapper
                                .VnfInfoRepository.FindByCondition(x => x.Vnfinfoid == vnfCapacityEntity.Vnfinfoid).FirstOrDefaultAsync();

                            if (vnfInstancesEntity != null)
                            {
                                _repositoryWrapper.VnfVmCapacityRepository.DeleteDeep(vnfCapacityEntity);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                                _repositoryWrapper.VnfInfoRepository.DeleteDeep(vnfInstancesEntity);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                                if (deletedTableCount == 3)
                                {
                                    var vnfInfoEntity = _repositoryWrapper.VnfClusterInfoRepository.FindByCondition(x => x.Vnfclusterinfoid
                                    == vnfInstancesEntity.Vnfclusterinfoid).FirstOrDefault();

                                    if (vnfInfoEntity != null)
                                    {
                                        _repositoryWrapper.VnfClusterInfoRepository.DeleteDeep(vnfInfoEntity);
                                        await _repositoryWrapper.SaveAsync();
                                        await _repositoryWrapper.ClearTracker();
                                    }
                                }
                            }
                        }

                    }



                    await transaction.CommitAsync();
                }

                catch (Exception)
                {

                    await transaction.RollbackAsync();
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryDeleteNotDeleted,
                        Data = id
                    };
                }
            }

            #endregion

            return new ResultDto
            {
                Info = ResultMessages.EntryDeleteSuccess,
                Data = id
            };
        }

        public async Task<ResultDto> DeleteVbomAsync(long clusterInfoId = 0, long infoId = 0, long capacityId = 0)
        {
            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    #region Delete Info, Instance, and Capacity
                    if (clusterInfoId != 0)
                    {
                        var clusterInfoEntity = _repositoryWrapper.VnfClusterInfoRepository
                            .FindByCondition(x => x.Vnfclusterinfoid == clusterInfoId)
                            .FirstOrDefault();

                        if (clusterInfoEntity == null)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryNotFound,
                                Data = clusterInfoId
                            };
                        }

                        var infoInstanceEntities = _repositoryWrapper.VnfInfoRepository
                            .FindByCondition(x => x.Vnfclusterinfoid == clusterInfoEntity.Vnfclusterinfoid)
                            .ToList();

                        foreach (var instance in infoInstanceEntities)
                        {
                            var capacityEntities = _repositoryWrapper.VnfVmCapacityRepository
                                .FindByCondition(x => x.Vnfinfoid == instance.Vnfinfoid)
                                .ToList();

                            foreach (var capacity in capacityEntities)
                            {
                                _repositoryWrapper.VnfVmCapacityRepository.DeleteDeep(capacity);
                            }

                            _repositoryWrapper.VnfInfoRepository.DeleteDeep(instance);
                        }
                        if (infoInstanceEntities != null && infoInstanceEntities.Count > 0)
                            _repositoryWrapper.VnfClusterInfoRepository.DeleteDeep(clusterInfoEntity);

                        await _repositoryWrapper.SaveAsync();
                    }
                    #endregion

                    #region Delete Instance and Capacity
                    else if (infoId != 0)
                    {
                        var instanceEntities = _repositoryWrapper.VnfInfoRepository
                            .FindByCondition(x => x.Vnfinfoid == infoId)
                            .ToList();

                        foreach (var instance in instanceEntities)
                        {
                            var capacityEntities = _repositoryWrapper.VnfVmCapacityRepository
                                .FindByCondition(x => x.Vnfinfoid == instance.Vnfinfoid)
                                .ToList();

                            foreach (var capacity in capacityEntities)
                            {
                                _repositoryWrapper.VnfVmCapacityRepository.DeleteDeep(capacity);
                            }

                            _repositoryWrapper.VnfInfoRepository.DeleteDeep(instance);
                        }
                    }
                    #endregion

                    #region Delete Only Capacity
                    else if (capacityId != 0)
                    {
                        var capacityEntities = _repositoryWrapper.VnfVmCapacityRepository
                            .FindByCondition(x => x.Vnfvmcapacityid == capacityId)
                            .ToList();

                        foreach (var capacity in capacityEntities)
                        {
                            _repositoryWrapper.VnfVmCapacityRepository.DeleteDeep(capacity);
                        }
                    }
                    #endregion

                    await _repositoryWrapper.SaveAsync();
                    await transaction.CommitAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return new ResultDto
                    {
                        Warning = true,
                        Info = ResultMessages.EntryDeleteNotDeleted,
                        Data = infoId
                    };
                }
            }

            return new ResultDto
            {
                Warning = false,
                Info = ResultMessages.EntryDeleteSuccess,
                Data = infoId
            };
        }

        #endregion

        #region  New Format Grid 

        public async Task<QueryResultDto<VbomExportGridDto>> FindWithConditionAsync(VnfClusterInfoQueryDto filterDto)
        {

            var predicateResult = ApplyFilter(filterDto);

            if (filterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }

            var vnfinfoQueryResult = await Task.Run(() => GetVbomRecords(predicateResult).AsEnumerable()
                      .Select(p => VnfClusterInfoMapper.GetVnfClusterInfo(p)).ToList());

            var capacityPredicateResult = ApplyFilterForCapacityEntity(filterDto);

            if (capacityPredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in vnfinfoQueryResult.SelectMany(x=>x.VnfInfo).ToList())
                {
                    item.VnfVmCapacity = item.VnfVmCapacity.Where(capacityPredicateResult).ToList();
                }
            }

            var paginatedRecords = await Task.Run(() => vnfinfoQueryResult.AsQueryable().ApplyOrdering(filterDto, GetColumnsMap())
               .ApplyPaging(filterDto));

            var mappedRecords = paginatedRecords.ToList()
                     .SelectMany(vnClustInfo => (vnClustInfo?.VnfInfo ?? new List<VnfInfo> { null }), (vnClustInfo, vnfInsta) => new { vnClustInfo, vnfInsta })
                     .SelectMany(vmCapactity => (vmCapactity?.vnfInsta?.VnfVmCapacity ?? new List<VnfVmCapacity> { null }), (vnInstance, vmCapactity) => new VbomExportGridDto
                     {
                         #region VNF Info
                         VnfClusterInfoId = vnInstance.vnClustInfo.VnfClusterInfoId,
                         Opcoid = vnInstance.vnClustInfo.OpcoId,
                         SiteLocationid = vnInstance.vnClustInfo.LocationId,
                         ClusterNameId = vnInstance.vnClustInfo.ClusterNameId,

                         OpCoDescritpion = vnInstance.vnClustInfo.OpcoIdDescription,
                         SiteLocation = vnInstance.vnClustInfo.LocationShortDescription,
                         ClusterName = vnInstance.vnClustInfo.ClusterName,
                         Noofblades = vnInstance.vnClustInfo.NoOfBlades,
                         HardwareType = vnInstance.vnClustInfo.HardwareType,
                         FileName = vnInstance.vnClustInfo.FileName,
                         Revision = vnInstance.vnClustInfo.Revision,

                         #endregion

                         #region
                         VnfInfoId = vnInstance.vnfInsta.VnfInfoId,
                         Vmtypenameid = vnInstance.vnfInsta.VnfVmTypeNameId,

                         VnfNameDescritpion = vnInstance?.vnfInsta?.VnfNameDesc,
                         VmTypeNameDescription = vnInstance?.vnfInsta?.VnfVmTypeNameDesc,

                         Nsxt = vnInstance.vnfInsta.Nsxt == true ? ConstantValueFilter.yes : ConstantValueFilter.no,
                         IntraVmType = vnInstance.vnfInsta?.IntraVmTypeDesc,
                         InterVmType = vnInstance.vnfInsta?.InterVmTypeDesc,
                         VmWorkLoadType = vnInstance.vnfInsta?.VmWorkLoadTypeDesc,
                         VmStorageBlockSize = vnInstance.vnfInsta?.VmStorageBlockSize,

                         Numa = vnInstance.vnfInsta != null ? Convert.ToString(vnInstance.vnfInsta.Numa) : string.Empty,
                         Socket = vnInstance.vnfInsta != null ? vnInstance.vnfInsta.Socket : string.Empty,
                         #endregion

                         #region VM Capacity Info

                         VnfVmCapacityId = vmCapactity != null ? vmCapactity.VnfVmCapacityId : 0,
                         FinancialYear = vmCapactity != null ? Convert.ToString(vmCapactity.FinancialYear) : null,
                         FinancialVersion = vmCapactity != null ? Convert.ToString("H" + vmCapactity.FinancialVersion) : null,
                         VcpuPerVm = vmCapactity != null ? Convert.ToString(vmCapactity.VnfCpuPerVm) : null,
                         RxTxCpuCount = vmCapactity != null ? Convert.ToString(vmCapactity.RxTxCpuCount) : null,
                         RamPerVm = vmCapactity != null ? Convert.ToString(vmCapactity.RamPerVm) : null,
                         DataDisk = Convert.ToInt32(vmCapactity?.DataDisk),
                         OsDisk = vmCapactity?.OsDisk,
                         IopsRunning = vmCapactity != null ? Convert.ToString(vmCapactity.IopsRunning) : null,
                         IopsLoading = vmCapactity != null ? Convert.ToString(vmCapactity.IopsLoading) : null,
                         VmWorkLoadDistribution = vmCapactity != null ? vmCapactity.VmWorkLoadDistribution : null,
                         NorthDouthBoundBandWidth = vmCapactity != null ? vmCapactity.NorthDouthBoundBandWidth : null,
                         EastWestBoundBandWidth = vmCapactity != null ? vmCapactity.EastWestBoundBandWidth : null,
                         OtherRequirements = vmCapactity != null ? vmCapactity.OtherRequirements : null,
                         BackupRequired = vmCapactity != null ? Convert.ToString(vmCapactity.BackupRequired) : null,
                         ProbIngRequired = vmCapactity != null ? Convert.ToString(vmCapactity.ProbIngRequired) : null,
                         NoOfVnfInstances = vnInstance.vnfInsta != null ? Convert.ToString(vmCapactity.NoOfVnfInstances) : null,
                         NoOfVmsPerType = vnInstance.vnfInsta != null ? Convert.ToString(vmCapactity.NoOfVmsPerType) : null,
                         #endregion
                     })
                     .GroupBy(e => new { e.ClusterNameId, e.SiteLocationid, e.VnfInfoId })
                        .Select(g =>
                        {
                            var first = g.First();
                            first.vnfVbomCapacityDtoGrids = g.GroupBy(x => x.FinancialYear)
                            .Select(yr => yr.OrderByDescending(x => x.FinancialVersion).FirstOrDefault()).Where(item => item != null)
                            .Select(item => new VnfVbomCapacityDtoGrid
                            {
                                VnfVmCapacityId = item.VnfVmCapacityId,
                                FinancialYear = Convert.ToString(item.FinancialYear),
                                FinancialVersion = Convert.ToString(item.FinancialVersion),
                                VcpuPerVm = Convert.ToString(item.VcpuPerVm),
                                RxTxCpuCount = Convert.ToString(item.RxTxCpuCount),
                                RamPerVm = Convert.ToString(item.RamPerVm),
                                DataDisk = Convert.ToInt32(item.DataDisk),
                                OsDisk = item.OsDisk,
                                IopsRunning = Convert.ToString(item.IopsRunning),
                                IopsLoading = Convert.ToString(item.IopsLoading),
                                VmWorkLoadDistribution = item.VmWorkLoadDistribution,
                                NorthDouthBoundBandWidth = item.NorthDouthBoundBandWidth,
                                EastWestBoundBandWidth = item.EastWestBoundBandWidth,
                                OtherRequirements = item.OtherRequirements,
                                BackupRequired = Convert.ToString(item.BackupRequired),
                                ProbIngRequired = Convert.ToString(item.ProbIngRequired),
                                NoOfVnfInstances = item.NoOfVnfInstances,
                                NoOfVmsPerType = item.NoOfVmsPerType
                            }).ToList();
                            return first;
                        }).ToList();
            

            var totalCount = mappedRecords.Count();
            return new QueryResultDto<VbomExportGridDto>(
                new GenerateRenderForGrid<VbomExportGridDto>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = mappedRecords
            };
        }

        #endregion

        public ExpressionStarter<Vnfinfo> ApplyFilterForInstance(VnfClusterInfoQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Vnfinfo>(true);

            if (filterDto.OpcoDescritpion?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.OpcoDescritpion)
                    descriptionPredicate.Or(x => x.Vnfclusterinfo.Opcoid.ToString()==item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.HardwareType?.Any() == true)
            {
                if (filterDto.HardwareType.All(x => x != "-1"))
                {
                    var componentIdPredicate = PredicateBuilder.New<Vnfinfo>();
                    foreach (var id in filterDto.HardwareType)
                        componentIdPredicate.Or(x => x.Vnfclusterinfo.Hardwaretypeid.ToString() == id);

                    mainPredicate.And(componentIdPredicate);

                }
            }
            if (filterDto.VnfClusterInfoId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var id in filterDto.VnfClusterInfoId)
                    componentIdPredicate.Or(x => x.Vnfclusterinfoid == id);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.VnfNameDescritpion?.Any() == true)
            {
                var VnfNamePredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.VnfNameDescritpion)
                {
                    VnfNamePredicate.Or(x => x.Vnfnameid.ToString() == item);
                }
                mainPredicate.And(VnfNamePredicate);
            }

            if (filterDto.VnfInfoId?.Any() == true)
            {
                var VnfInfoIdPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.VnfInfoId)
                {
                    VnfInfoIdPredicate.Or(x => x.Vnfinfoid == item);
                }
                mainPredicate.And(VnfInfoIdPredicate);
            }

            if (filterDto.VnfVmTypeNameDescription?.Any() == true)
            {
                var VnfVmTypeNamePredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.VnfVmTypeNameDescription)
                {
                    VnfVmTypeNamePredicate.Or(x => x.Vnfvmtypenameid.ToString() == item);
                }
                mainPredicate.And(VnfVmTypeNamePredicate);
            }

            if (filterDto.IntraVmType?.Any() == true)
            {
                var IntraVmTypePredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.IntraVmType)
                {
                    IntraVmTypePredicate.Or(x => x.Intravmtypeid.ToString() == item);
                }
                mainPredicate.And(IntraVmTypePredicate);
            }


            if (filterDto.InterVmType?.Any() == true)
            {
                var InterVmTypePredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.InterVmType)
                {
                    InterVmTypePredicate.Or(x => x.Intervmtypeid.ToString() == item);
                }
                mainPredicate.And(InterVmTypePredicate);
            }

            if (filterDto.VmWorkLoadType?.Any() == true)
            {
                var VmWorkLoadTypePredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.VmWorkLoadType)
                {
                    VmWorkLoadTypePredicate.Or(x => x.Vmworkloadtypeid.ToString() == item);
                }
                mainPredicate.And(VmWorkLoadTypePredicate);
            }

            if (filterDto.Nsxt?.Any() == true)
            {
                var NsxtPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.Nsxt)
                {
                    NsxtPredicate.Or(x => item == ConstantValueFilter.yes ? x.Nsxt == true : x.Nsxt == false);
                }
                mainPredicate.And(NsxtPredicate);
            }
            if (filterDto.VmStorageBlockSize?.Any() == true)
            {
                var VmStoragePredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.VmStorageBlockSize)
                {
                    VmStoragePredicate.Or(x => x.Vmstorageblocksize == item);
                }
                mainPredicate.And(VmStoragePredicate);
            }
            if (filterDto.Numa?.Any() == true)
            {
                var NumaPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.Numa)
                {
                    NumaPredicate.Or(x => x.Numa.ToString() == item);
                }
                mainPredicate.And(NumaPredicate);
            }
            if (filterDto.Socket?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.Socket)
                {
                    SocketPredicate.Or(x => x.Socket == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.Vnfinfoid?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.Vnfinfoid)
                    componentIdPredicate.Or(x => x.Vnfinfoid == item);

                mainPredicate.And(componentIdPredicate);
            }           

            return mainPredicate;
        }
        private IQueryable<Vnfinfo> GetVnfInstanceRecords(ExpressionStarter<Vnfinfo> predicateResult)
        {
            var query = _repositoryWrapper.VnfInfoRepository.FindByCondition(predicateResult)
                  .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                  .Include(x => x.Intravmtype)
                  .Include(x => x.Intervmtype)
                  .Include(x => x.Vmworkloadtype)
                  .Include(x => x.Vnfname)
                  .Include(x => x.Vnfvmcapacity)
                  .Include(x => x.Vnfvmtypename)
                  .Include(x => x.Vnfclusterinfo).ThenInclude(x => x.Location)
                .AsQueryable();

            return query;
        }

        public async Task<List<FilterValueDto>> GetInstanceCapacityFilteredValues(string propertyName, string propertyFilter, VnfClusterInfoQueryDto filterDto)
        {
            var filterCriteria = ApplyFilterForInstance(filterDto);

            var vnfInstanceInfoQueryResult = await Task.Run(() => GetVnfInstanceRecords(filterCriteria));
            var subFilteredQuery = vnfInstanceInfoQueryResult;

            var result = propertyName switch
            {
                #region Vnf instance Info Fields
                "vnfNameDescritpion" => subFilteredQuery
                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vnfnameid.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto { Value = x.Vnfnameid.ToString(), Text = x.Vnfname.Vnfdescription })
                   .Distinct()
                   .ToList(),

                "vnfInfoId" => subFilteredQuery
                  .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vnfinfoid.ToString().Contains(propertyFilter))
                  .Select(x => new FilterValueDto { Value = x.Vnfinfoid.ToString(), Text = x.Vnfinfoid.ToString() })
                  .Distinct()
                  .ToList(),

                "vnfVmTypeNameDescription" => subFilteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vnfvmtypenameid.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Vnfvmtypenameid.ToString(), Text = x.Vnfvmtypename.Vmtypedescription })
                .Distinct()
                .ToList(),

                "intraVmType" => subFilteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Intravmtypeid.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Intravmtypeid.ToString(), Text = x.Intravmtype.Intradescription })
                .Distinct()
                .ToList(),

                "interVmType" => subFilteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Intervmtypeid.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Value = x.Intervmtypeid.ToString(), Text = x.Intervmtype.Interdescription })
               .Distinct()
               .ToList(),

                "vmWorkLoadType" => subFilteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vmworkloadtypeid.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Vmworkloadtypeid.ToString(), Text = x.Vmworkloadtype.Description })
                .Distinct()
                .ToList(),

                "vmStorageBlockSize" => subFilteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vmstorageblocksize.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Value = x.Vmstorageblocksize.ToString(), Text = x.Vmstorageblocksize.ToString() })
               .Distinct()
               .ToList(),

                "numa" => subFilteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Numa.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Numa.ToString(), Text = x.Numa.ToString() })
                .Distinct()
                .ToList(),

                "nsxt" => subFilteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Nsxt.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Nsxt == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Text = x.Nsxt == true ? ConstantValueFilter.Yes : ConstantValueFilter.No })
                .Distinct()
                .ToList(),

                "socket" => subFilteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Socket.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Socket.ToString(), Text = x.Socket.ToString() })
                .Distinct()
                .ToList(),

                #endregion

                _ => new List<FilterValueDto>()
            };

            return result;
        }
        private Dictionary<string, Expression<Func<VnfInfo, object>>[]> GetColumnsMapForInstanceCapacityInfo()
        {
            var returnCnfInfoDict = new Dictionary<string, Expression<Func<VnfInfo, object>>[]>
            {
                ["VnfInfoId"] = new Expression<Func<VnfInfo, object>>[] { p => p.VnfInfoId },
                ["VnfNameId"] = new Expression<Func<VnfInfo, object>>[] { p => p.VnfNameId },
                ["VnfVmTypeNameId"] = new Expression<Func<VnfInfo, object>>[] { p => p.VnfVmTypeNameId },

                ["IntraVmTypeId"] = new Expression<Func<VnfInfo, object>>[] { p => p.IntraVmTypeId },
                ["InterVmTypeId"] = new Expression<Func<VnfInfo, object>>[] { p => p.InterVmTypeId },
                ["VmWorkLoadTypeId"] = new Expression<Func<VnfInfo, object>>[] { p => p.VmWorkLoadTypeId },

            };

            return returnCnfInfoDict;
        }

        #region // Capacity Filter
        public ExpressionStarter<Vnfvmcapacity> ApplyFilterForCapacity(VnfClusterInfoQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Vnfvmcapacity>(true);
            if (filterDto.VnfInfoId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.VnfInfoId)
                    componentIdPredicate.Or(x => x.Vnfinfoid == item);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.FinancialYear?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.FinancialYear)
                    componentIdPredicate.Or(x => x.Financialyear == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.Financialversion?.Any() == true)
            {
                var VnfNamePredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.Financialversion)
                {
                    VnfNamePredicate.Or(x => x.Financialversion == item);
                }
                mainPredicate.And(VnfNamePredicate);
            }
            if (filterDto.VcpuPerVm?.Any() == true)
            {
                var VnfInfoIdPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.VcpuPerVm)
                {
                    VnfInfoIdPredicate.Or(x => x.Vnfcpupervm.ToString() == item);
                }
                mainPredicate.And(VnfInfoIdPredicate);
            }

            if (filterDto.RxTxCpuCount?.Any() == true)
            {
                var VnfVmTypeNamePredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.RxTxCpuCount)
                {
                    VnfVmTypeNamePredicate.Or(x => x.Rxtxcpucount == item);
                }
                mainPredicate.And(VnfVmTypeNamePredicate);
            }

            if (filterDto.RamPerVm?.Any() == true)
            {
                var IntraVmTypePredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.RamPerVm)
                {
                    IntraVmTypePredicate.Or(x => x.Rampervm == item);
                }
                mainPredicate.And(IntraVmTypePredicate);
            }


            if (filterDto.DataDisk?.Any() == true)
            {
                var InterVmTypePredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.DataDisk)
                {
                    InterVmTypePredicate.Or(x => x.Datadisk == item);
                }
                mainPredicate.And(InterVmTypePredicate);
            }

            if (filterDto.OsDisk?.Any() == true)
            {
                var VmWorkLoadTypePredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.OsDisk)
                {
                    VmWorkLoadTypePredicate.Or(x => x.Osdisk == item);
                }
                mainPredicate.And(VmWorkLoadTypePredicate);
            }

            if (filterDto.IopsRunning?.Any() == true)
            {
                var NsxtPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.IopsRunning)
                {
                    NsxtPredicate.Or(x => x.Iopsrunning == item);
                }
                mainPredicate.And(NsxtPredicate);
            }
            if (filterDto.IopsLoading?.Any() == true)
            {
                var VmStoragePredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.IopsLoading)
                {
                    VmStoragePredicate.Or(x => x.Iopsloading == item);
                }
                mainPredicate.And(VmStoragePredicate);
            }
            if (filterDto.VmWorkLoadDistribution?.Any() == true)
            {
                var NumaPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.VmWorkLoadDistribution)
                {
                    NumaPredicate.Or(x => x.Vmworkloaddistribution == item);
                }
                mainPredicate.And(NumaPredicate);
            }
            if (filterDto.NorthDouthBoundBandWidth?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.NorthDouthBoundBandWidth)
                {
                    SocketPredicate.Or(x => x.Northsouthboundbandwidth == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.EastWestBoundBandWidth?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.EastWestBoundBandWidth)
                {
                    SocketPredicate.Or(x => x.Eastwestboundbandwidth == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.OtherRequirements?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.OtherRequirements)
                {
                    SocketPredicate.Or(x => x.Otherrequirements == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.BackupRequired?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.BackupRequired)
                {
                    SocketPredicate.Or(x => x.Backuprequired == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.ProbIngRequired?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.ProbIngRequired)
                {
                    SocketPredicate.Or(x => x.Probingrequired == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.Noofvnfinstances?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.Noofvnfinstances)
                {
                    SocketPredicate.Or(x => x.Noofvnfinstances == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.Noofvmspertype?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.Noofvmspertype)
                {
                    SocketPredicate.Or(x => x.Noofvmspertype == item);
                }
                mainPredicate.And(SocketPredicate);
            }

            return mainPredicate;
        }

        public ExpressionStarter<VnfVmCapacity> ApplyFilterForCapacityEntity(VnfClusterInfoQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<VnfVmCapacity>(true);
            if (filterDto.Vnfinfoid?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.Vnfinfoid)
                    componentIdPredicate.Or(x => x.VnfInfoId == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.VnfInfoId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.VnfInfoId)
                    componentIdPredicate.Or(x => x.VnfInfoId == item);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.FinancialYear?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.FinancialYear)
                    componentIdPredicate.Or(x => x.FinancialYear == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.Financialversion?.Any() == true)
            {
                var VnfNamePredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.Financialversion)
                {
                    VnfNamePredicate.Or(x => x.FinancialVersion == item);
                }
                mainPredicate.And(VnfNamePredicate);
            }
            if (filterDto.VcpuPerVm?.Any() == true)
            {
                var VnfInfoIdPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.VcpuPerVm)
                {
                    VnfInfoIdPredicate.Or(x => x.VnfCpuPerVm.ToString() == item);
                }
                mainPredicate.And(VnfInfoIdPredicate);
            }

            if (filterDto.RxTxCpuCount?.Any() == true)
            {
                var VnfVmTypeNamePredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.RxTxCpuCount)
                {
                    VnfVmTypeNamePredicate.Or(x => x.RxTxCpuCount == item);
                }
                mainPredicate.And(VnfVmTypeNamePredicate);
            }

            if (filterDto.RamPerVm?.Any() == true)
            {
                var IntraVmTypePredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.RamPerVm)
                {
                    IntraVmTypePredicate.Or(x => x.RamPerVm == item);
                }
                mainPredicate.And(IntraVmTypePredicate);
            }


            if (filterDto.DataDisk?.Any() == true)
            {
                var InterVmTypePredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.DataDisk)
                {
                    InterVmTypePredicate.Or(x => x.DataDisk == item);
                }
                mainPredicate.And(InterVmTypePredicate);
            }

            if (filterDto.OsDisk?.Any() == true)
            {
                var VmWorkLoadTypePredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.OsDisk)
                {
                    VmWorkLoadTypePredicate.Or(x => x.OsDisk == item);
                }
                mainPredicate.And(VmWorkLoadTypePredicate);
            }

            if (filterDto.IopsRunning?.Any() == true)
            {
                var NsxtPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.IopsRunning)
                {
                    NsxtPredicate.Or(x => x.IopsRunning == item);
                }
                mainPredicate.And(NsxtPredicate);
            }
            if (filterDto.IopsLoading?.Any() == true)
            {
                var VmStoragePredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.IopsLoading)
                {
                    VmStoragePredicate.Or(x => x.IopsLoading == item);
                }
                mainPredicate.And(VmStoragePredicate);
            }
            if (filterDto.VmWorkLoadDistribution?.Any() == true)
            {
                var NumaPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.VmWorkLoadDistribution)
                {
                    NumaPredicate.Or(x => x.VmWorkLoadDistribution == item);
                }
                mainPredicate.And(NumaPredicate);
            }
            if (filterDto.NorthDouthBoundBandWidth?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.NorthDouthBoundBandWidth)
                {
                    SocketPredicate.Or(x => x.NorthDouthBoundBandWidth == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.EastWestBoundBandWidth?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.EastWestBoundBandWidth)
                {
                    SocketPredicate.Or(x => x.EastWestBoundBandWidth == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.OtherRequirements?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.OtherRequirements)
                {
                    SocketPredicate.Or(x => x.OtherRequirements == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.BackupRequired?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.BackupRequired)
                {
                    SocketPredicate.Or(x => x.BackupRequired == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.ProbIngRequired?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.ProbIngRequired)
                {
                    SocketPredicate.Or(x => x.ProbIngRequired == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.Noofvnfinstances?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.Noofvnfinstances)
                {
                    SocketPredicate.Or(x => x.NoOfVnfInstances == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.Noofvmspertype?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<VnfVmCapacity>();
                foreach (var item in filterDto.Noofvmspertype)
                {
                    SocketPredicate.Or(x => x.NoOfVmsPerType == item);
                }
                mainPredicate.And(SocketPredicate);
            }

            return mainPredicate;
        }
       
        private IQueryable<Vnfvmcapacity> GetVnfVmCapacityRecords(ExpressionStarter<Vnfvmcapacity> predicateResult)
        {
            var query = _repositoryWrapper.VnfVmCapacityRepository.FindByCondition(predicateResult)
                  .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                  .AsQueryable();

            return query;
        }


        public async Task<List<FilterValueDto>> GetCapacityFilteredValuesAsync(string propertyName, string propertyFilter, VnfClusterInfoQueryDto filterDto)
        {
            var filterCriteria = ApplyFilterForCapacity(filterDto);

            var filteredQuery = await Task.Run(() => GetVnfVmCapacityRecords(filterCriteria));

            var result = propertyName switch
            {
                #region VNF VM Capacity
                "financialYear" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Financialyear.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Financialyear))
                    .Distinct()
                    .ToList(),

                "financialVersion" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Financialversion.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto { Text = $"H{x.Financialversion.ToString()}", Value = x.Financialversion.ToString() })
                    .Distinct()
                    .ToList(),

                "vcpuPerVm" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vnfcpupervm.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.Vnfcpupervm))
               .Distinct()
               .ToList(),

                "rxTxCpuCount" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Rxtxcpucount.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Rxtxcpucount == true?ConstantValueFilter.Yes:ConstantValueFilter.No, Text = x.Rxtxcpucount.ToString() })
                .Distinct()
                .ToList(),

                "ramPerVm" => filteredQuery.ToList()
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Rampervm.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Rampervm)
                 )
                .Distinct()
                .ToList(),

                "dataDisk" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Datadisk.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Datadisk)
                     )
                    .Distinct()
                    .ToList(),

                "osDisk" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Osdisk.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Osdisk)
                     )
                    .Distinct()
                    .ToList(),

                "iopsLoading" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Iopsloading.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Iopsloading)
                    )
                   .Distinct()
                   .ToList(),

                "iopsRunning" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Iopsrunning.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Iopsrunning)
                    )
                   .Distinct()
                   .ToList(),

                "vmWorkLoadDistribution" => filteredQuery.ToList()
                      .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vmworkloaddistribution.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.Vmworkloaddistribution)
                      )
                     .Distinct()
                     .ToList(),

                "northDouthBoundBandWidth" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Northsouthboundbandwidth.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Northsouthboundbandwidth)
                     )
                    .Distinct()
                    .ToList(),

                "eastWestBoundBandWidth" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Eastwestboundbandwidth.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Eastwestboundbandwidth)
                    )
                   .Distinct()
                   .ToList(),

                "otherRequirements" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Otherrequirements.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Otherrequirements)
                    )
                   .Distinct()
                   .ToList(),
                // need to change
                "backupRequired" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Backuprequired.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Backuprequired)
                    )
                   .Distinct()
                   .ToList(),
                "probIngRequired" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Probingrequired.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Probingrequired)
                    )
                   .Distinct()
                   .ToList(),
                "noOfVnfInstances" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Noofvnfinstances.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Noofvnfinstances)
                    )
                   .Distinct()
                   .ToList(),
                "noOfVmsPerType" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Noofvmspertype.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Noofvmspertype)
                    )
                   .Distinct()
                   .ToList(),

                #endregion

                _ => new List<FilterValueDto>()
            };

            return result;
        }
        #endregion

    }
}
