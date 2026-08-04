using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.XBom.CBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Cbom;
using CAM.Entities.Models.CBom;
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

namespace CAM.BusinessManager.Entity.XBom.CBOM
{
    public class CBomManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        public CBomManager(IEnumerable<IRepositoryWrapper> wrappers,
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
        private IQueryable<Cnfclusterinfo> GetCbomRecords(ExpressionStarter<Cnfclusterinfo> predicateResult)
        {
            var query = _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Cnfcapacity)
                .Include(x => x.Opco)
                .Include(x => x.Site)
                .Include(x => x.Cnfname)
                .Include(x => x.Cnfclusternodepool)
                .Include(x => x.Cnfcluster)
                .Include(x => x.Cnfhardware)
                .Include(x => x.Verticalresponsible)
                .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Priority)
                 .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Functionstandard)
                 .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Podtypeinfo)
                 .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Podroledescription)
                .AsQueryable();

            return query;
        }

        private IQueryable<Cnfclusterinfo> GetCnfClusterRecords(ExpressionStarter<Cnfclusterinfo> predicateResult)
        {
            var query = _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                .Include(x => x.Opco)
                .Include(x => x.Site)
                .Include(x => x.Cnfname)
                .Include(x => x.Cnfclusternodepool)
                .Include(x => x.Cnfcluster)
                .Include(x => x.Verticalresponsible)
               .Include(x => x.Cnfhardware)
                .AsQueryable();

            return query;
        }

        private IQueryable<Cnfpodinfo> GetCnfPodAndCapacityRecords(ExpressionStarter<Cnfpodinfo> predicateResult)
        {
            var query = _repositoryWrapper.CnfPodInfoRepository.FindByCondition(predicateResult)
               .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
               .Include(x => x.Cnfcapacity)
               .Include(x => x.Priority)
                .Include(x => x.Functionstandard)
                 .Include(x => x.Podroledescription)
                .Include(x => x.Podtypeinfo)
               .AsQueryable();

            return query;
        }
        public async Task<QueryResultDto<CnfClusterInfoDtoGrid>> CbomClusterinfoDetailsAsync(CnfClusterInfoQueryDto dynamicFilterDto)
        {
            var predicateResult = ApplyFilter(dynamicFilterDto);

            var rtn = new QueryResultDto<CnfClusterInfoDtoGrid>(new GenerateRenderForGrid<CnfClusterInfoDtoGrid>(_customColumnManager))
            {

            };
            var cnfClusterinfoQueryResult = await Task.Run(() =>
      GetCnfClusterRecords(predicateResult)
          .AsEnumerable()
          .Select(p => CnfClusterInfoMapper.GetCnfClusterInfo(p))
          .AsQueryable()
  );
            rtn.TotalItems = cnfClusterinfoQueryResult.Count();
            var paginatedRecords = await Task.Run(() => cnfClusterinfoQueryResult.ApplyOrdering(dynamicFilterDto, GetColumnsMap())
            );

            paginatedRecords = paginatedRecords.ApplyPaging(dynamicFilterDto);
            var mappedVnfClusterEntity = paginatedRecords.ToList()
              .Select(cnfInfo => new CnfClusterInfoDtoGrid
              {
                  CnfClusterInfoId = cnfInfo.CnfClusterInfoId,
                  CnfNameId = cnfInfo.CnfNameId,
                  CnfName = cnfInfo.CnfNameDescription,
                  CnfClusterId = cnfInfo.CnfClusterId,
                  CnfClusterName = cnfInfo.CnfClusterIdName,
                  NodePool = cnfInfo.NodePoolName,
                  NodePoolId = (long)cnfInfo.CnfClusterNodePoolId,
                  HardwareTypeId = cnfInfo.CnfHardwareId,
                  Hardware = cnfInfo.HardwareDescription,
                  OpcoId = cnfInfo.OpcoId,
                  OpcoName = cnfInfo.OpcoIdName,
                  Location = cnfInfo.LocationName,
                  Site = cnfInfo.SiteName,
                  SiteId = cnfInfo.SiteId,
                  NodePoolBreakup = cnfInfo.NodePoolBreakup != null ?
                 Convert.ToBoolean(cnfInfo.NodePoolBreakup) == true ? ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,
                  SpecialRequirements = cnfInfo.SpecialRequirements,
                  HyperThreading = cnfInfo.Hyperthreading,
                  WorkerNodeConfiguration = cnfInfo.WorkerNodeConfiguration,
                  OverProvisioning = cnfInfo.OverProvisioning,
                  CpuKubelet = cnfInfo.CpuKubelet,
                  MemKubelet = cnfInfo.MemKubelet,
                  CpuSystem = cnfInfo.CpuSystem,
                  MemSystem = cnfInfo.MemSystem,
                  Comments = cnfInfo.Comments,
                  Note = cnfInfo.Notes,
                  FileName = cnfInfo.FileName,
                  Revision = cnfInfo.Revision,
                  HardwareType = cnfInfo.HardwareTypeDescription,
                  AggregateImageClusterSize = cnfInfo.AggregateImageClusterSize,
                  VerticalDomain = cnfInfo.VerticalResponsibleName,
                  VerticalDomainId = cnfInfo.VerticalResponsibleId,
                  LastModifiedBy = cnfInfo.ModificationUserEntity.Email,
                  LastModified = cnfInfo.ModificationDate,
              }).OrderBy(x => x.CnfName)
          .ThenBy(x => x.CnfClusterName)
          .ThenBy(x => x.OpcoName)
          .ThenBy(x => x.Location)
          .ThenBy(x => x.Site).ToList();


            rtn.Items = mappedVnfClusterEntity.ToArray();
            return rtn;

        }

        public async Task<QueryResultDto<CnfInfoAndCapacityDtoGrid>> GetCnfInfoAndCapacityEntities(CnfClusterInfoQueryDto dynamicFilterDto)
        {
            var predicateResult = ApplyPodLevelFilter(dynamicFilterDto);

            var rtn = new QueryResultDto<CnfInfoAndCapacityDtoGrid>(new GenerateRenderForGrid<CnfInfoAndCapacityDtoGrid>(_customColumnManager))
            {

            };
            var cnfinfoQueryResult = await Task.Run(() => GetCnfPodAndCapacityRecords(predicateResult).AsEnumerable()
                      .Select(p => CnfPodInfoMapper.GetCnfPodInfo(p)).ToList());
   
            rtn.TotalItems = Convert.ToInt16(cnfinfoQueryResult?.ToList().Count());

            var capacityPredicateResult = ApplyFilterForCapacityEntity(dynamicFilterDto);

            if (capacityPredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in cnfinfoQueryResult)
                {
                    item.CnfCapacity = item.CnfCapacity.Where(capacityPredicateResult).ToList();
                }
            }           

            var paginatedRecords = await Task.Run(() => cnfinfoQueryResult.AsQueryable().ApplyOrdering(dynamicFilterDto, GetColumnsMapForPodAndCapacity())
                );


            paginatedRecords = paginatedRecords.ApplyPaging(dynamicFilterDto);

            var mappedVnfInfoCapacity = paginatedRecords.ToList()
             .Select(insta => new CnfInfoAndCapacityDtoGrid
             {
                 CnfPodInfoId = insta != null ? insta.CnfPodInfoId : 0,
                 CnfClusterInfoId = insta != null ? insta.CnfClusterInfoId : 0,
                 PodTypeInfoId = insta != null ? insta.PodTypeInfoId : 0,
                 PodTypeName = insta != null ? insta.PodTypeInfoName : string.Empty,
                 PodroleDescription = insta != null ? insta.PodroleDescription : string.Empty,
                 FunctionStandardId = Convert.ToInt32(insta != null ? insta.FunctionStandardId : 0),
                 FunctionStandardName = insta != null ? insta.FunctionStandardName : string.Empty,
                 PriorityId = Convert.ToInt32(insta != null ? insta.PriorityId : 0),
                 PriorityName = insta != null ? insta.PriorityName : string.Empty,

                 DaemonSetPod = insta != null ? Convert.ToInt16(insta.DaemonSetPod) == 1 ?
                                    ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,
                 IntraPodRules = insta != null ? insta.IntraPodRules : string.Empty,
                 InterPodRules = insta != null ? insta.InterPodRules : string.Empty,
                 IsEnhancedHa = insta != null ? Convert.ToInt16(insta.IsEnhancedHa) == 1 ?
                              ConstantValueFilter.CamalTrue : ConstantValueFilter.CamalFalse : string.Empty,
                 IsPersistanceStorageFlag = insta != null ? insta.IsPersistanceStorageFlag : string.Empty,

                 IsProdhPaEnable = insta != null ? Convert.ToInt16(insta.IsProdHpaEnable) == 1 ?
                                    ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,
                 PodTypeQos = insta != null ? insta.PodTypeQos : string.Empty,

                 _cnfCapacityDtoGrid = insta.CnfCapacity.Select(xcapacity => new CnfCapacityDtoGrid
                 {

                     CnfCapacityId = xcapacity != null ? xcapacity.CnfCapacityId : 0,

                     FinancialYear = xcapacity != null ? Convert.ToString(xcapacity.FinancialYear) : null,
                     FinancialVersion = xcapacity != null ? "Q" + Convert.ToString(xcapacity.FinancialVersion) : null,

                     VcpuRequestForPodType = xcapacity != null ? Convert.ToString(xcapacity.VcpuRequestForPodType) : null,
                     VcpuLimitForPodType = xcapacity != null ? Convert.ToString(xcapacity.VcpuLimitForPodType) : null,
                     PcpuRequestForPodType = xcapacity != null ? Convert.ToString(xcapacity.PcpuRequestForPodType) : null,
                     MemRequestForPodType = xcapacity != null ? Convert.ToString(xcapacity.MemRequestForPodType) : null,
                     MemLimitForPodType = xcapacity != null ? Convert.ToString(xcapacity.MemLimitForPodType) : null,
                     NoOfCnfInstancesPersite = Convert.ToString(insta != null ? xcapacity.NoOfCnfInstancesPerSite : 0),
                     NumberOfPodsPerPodType = Convert.ToString(insta != null ? xcapacity.NumberOfPodsPerPodType : 0),

                     NonPresistentStorageForProdType = xcapacity?.NonPresistentStorageForProdType,
                     IsPresistentVolumesRequired = xcapacity != null ? Convert.ToInt16(xcapacity.IsPresistentVolumesRequired) == 1 ?
                                    ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,

                     PersistentVolumNeaccessMode = xcapacity != null ? Convert.ToString(xcapacity.PersistentVolumNeaccessMode) : null,
                     PersistentStorageForPodType = xcapacity != null ? xcapacity.PersistentStorageForPodType : null,
                     StorageIopsForPodType = xcapacity != null ? xcapacity.StorageIopsForPodType : null,
                     StoragerWorkloadDistribution = xcapacity != null ? xcapacity.StoragerWorkloadDistribution : null,
                     NorthSouthBandWidthForPodType = xcapacity != null ? xcapacity.NorthSouthBandWidthForPodType : null,
                     EastWestBandWidthForPodType = xcapacity != null ? Convert.ToString(xcapacity.EastWestBandWidthForPodType) : null,
                     ListOfCapacitySpecialRequirement = xcapacity != null ? Convert.ToString(xcapacity.ListOfCapacitySpecialRequirement) : null,
                     SpecialRequirementPerPodType = xcapacity != null ? Convert.ToString(xcapacity.SpecialRequirementPerPodType) : null,
                 }).OrderByDescending(x => x.FinancialYear).ThenBy(x => x.FinancialVersion).ToList()


             }).OrderBy(x => x.PodTypeName).ThenBy(x => x.PodroleDescription).ToList();

            rtn.Items = mappedVnfInfoCapacity.ToArray();
            return rtn;

        }
        private Dictionary<string, Expression<Func<CnfClusterInfo, object>>[]> GetColumnsMap()
        {
            var returnCnfInfoDict = new Dictionary<string, Expression<Func<CnfClusterInfo, object>>[]>
            {
                ["CnfClusterInfoid"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.CnfClusterInfoId },
                ["opcoId"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.OpcoId },
                ["siteid"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.SiteId },
                ["cnfnameid"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.CnfNameId },
                ["cnfclusterid"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.CnfClusterId },


                ["cnfhardwareid"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.CnfHardwareId },

                ["nodepoolbreakup"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.NodePoolBreakup },
                ["specialrequirements"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.SpecialRequirements },
                ["hyperthreading"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.Hyperthreading },
                ["overprovisioning"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.OverProvisioning },
                ["workernodeconfiguration"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.WorkerNodeConfiguration },
                ["hardwareDescription"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.HardwareDescription },
                ["cpukubelet"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.CpuKubelet },
                ["memkubelet"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.MemKubelet },
                ["cpusystem"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.CpuSystem },
                ["memsystem"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.MemSystem },
                ["verticalresponsibleid"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.VerticalResponsibleId },
                ["comments"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.Comments },
                ["notes"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.Notes },
                ["filename"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.FileName },
                ["revision"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.Revision },
                ["aggregateimageclustersize"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.AggregateImageClusterSize },

                ["lastModified"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<CnfClusterInfo, object>>[] { p => p.ModificationUserEntity.Email },
            };

            return returnCnfInfoDict;
        }
        private Dictionary<string, Expression<Func<CnfPodInfo, object>>[]> GetColumnsMapForPodAndCapacity()
        {
            var returnCnfInfoDict = new Dictionary<string, Expression<Func<CnfPodInfo, object>>[]>
            {
                ["cnfPodInfoId"] = new Expression<Func<CnfPodInfo, object>>[] { p => p.CnfPodInfoId },
                ["podTypeInfoId"] = new Expression<Func<CnfPodInfo, object>>[] { p => p.PodTypeInfoId },

                ["lastModified"] = new Expression<Func<CnfPodInfo, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<CnfPodInfo, object>>[] { p => p.ModificationUserEntity.Email },
            };

            return returnCnfInfoDict;
        }
        #region Apply Filter
        public ExpressionStarter<Cnfclusterinfo> ApplyFilter(CnfClusterInfoQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Cnfclusterinfo>(true);


            #region Cnfclusterinfo
            if (filterDto.Cnfclusterinfoid?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var id in filterDto.Cnfclusterinfoid)
                    componentIdPredicate.Or(x => x.Cnfclusterinfoid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.opcoName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.opcoName)
                    descriptionPredicate.Or(x => x.Opcoid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Location?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Location)
                    descriptionPredicate.Or(x => x.Site.Locationid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.nodePool?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.nodePool)
                    descriptionPredicate.Or(x => x.Cnfclusternodepoolid == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Site?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Site)
                    descriptionPredicate.Or(x => x.Siteid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.CnfClusterName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.CnfClusterName)
                    descriptionPredicate.Or(x => x.Cnfclusterid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.cnfName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.cnfName)
                    descriptionPredicate.Or(x => x.Cnfnameid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.CnfhardwareDescription?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.CnfhardwareDescription)
                    descriptionPredicate.Or(x => x.Cnfhardwareid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.Nodepoolbreakup?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Nodepoolbreakup)
                    descriptionPredicate.Or(x => x.Nodepoolbreakup == (item == ConstantValueFilter.Yes ? true : false));

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Specialrequirements?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Specialrequirements)
                    descriptionPredicate.Or(x => x.Specialrequirements.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }


            if (filterDto.Hyperthreading?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Hyperthreading)
                    descriptionPredicate.Or(x => x.Hyperthreading.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Overprovisioning?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Overprovisioning)
                    descriptionPredicate.Or(x => x.Overprovisioning.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Workernodeconfiguration?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Workernodeconfiguration)
                    descriptionPredicate.Or(x => x.Workernodeconfiguration.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Hardware?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Hardware)
                    descriptionPredicate.Or(x => x.Hardware.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.Cpukubelet?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Cpukubelet)
                    descriptionPredicate.Or(x => x.Cpukubelet == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Memkubelet?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Memkubelet)
                    descriptionPredicate.Or(x => x.Memkubelet == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Cpusystem?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Cpusystem)
                    descriptionPredicate.Or(x => x.Cpusystem == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.Memsystem?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Memsystem)
                    descriptionPredicate.Or(x => x.Memsystem == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.verticalDomain?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.verticalDomain)
                    descriptionPredicate.Or(x => x.Verticalresponsibleid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Comments?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Comments)
                    descriptionPredicate.Or(x => x.Comments == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Note?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Note)
                    descriptionPredicate.Or(x => x.Notes == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Filename?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Filename)
                    descriptionPredicate.Or(x => x.Filename == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.Aggregateimageclustersize?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Aggregateimageclustersize)
                    descriptionPredicate.Or(x => x.Aggregateimageclustersize == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.Revision?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.Revision)
                    descriptionPredicate.Or(x => x.Revision == item);

                mainPredicate.And(descriptionPredicate);
            }

            if (filterDto.LastModifiedBy?.Any() == true)
            {
                var modifiedByPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var email in filterDto.LastModifiedBy)
                    modifiedByPredicate.Or(x => x.ModificationuserNavigation.Email == email);

                mainPredicate.And(modifiedByPredicate);
            }
            if (filterDto.LastModified != null)
            {
                var LastModifiedValuePredicate = PredicateBuilder.New<Cnfclusterinfo>();
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
                var componentIdPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                if (filterDto.LastModifiedValue.StartDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date >= filterDto.LastModifiedValue.StartDate);
                if (filterDto.LastModifiedValue.EndDate != null)
                    componentIdPredicate.And(x => x.Modificationdate.Date <= filterDto.LastModifiedValue.EndDate);
                mainPredicate.And(componentIdPredicate);
            }

            return mainPredicate;
        }

        public ExpressionStarter<Cnfpodinfo> ApplyPodLevelFilter(CnfClusterInfoQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Cnfpodinfo>(true);


            #region Cnfclusterinfo
            if (filterDto.Cnfclusterinfoid?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var id in filterDto.Cnfclusterinfoid)
                    componentIdPredicate.Or(x => x.Cnfclusterinfoid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.CnfPodInfoId?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.CnfPodInfoId)
                    descriptionPredicate.Or(x => x.Cnfpodinfoid == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.PodTypeName?.Any() == true)
            {
                var podTypeNamePredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.PodTypeName)
                    podTypeNamePredicate.Or(x => x.Podtypeinfo.Podtypeinfoid.ToString() == item);

                mainPredicate.And(podTypeNamePredicate);
            }
            if (filterDto.FunctionStandardName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.FunctionStandardName)
                    descriptionPredicate.Or(x => x.Functionstandardid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.PriorityName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.PriorityName)
                    descriptionPredicate.Or(x => x.Priorityid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.PodroleDescription?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.PodroleDescription)
                    descriptionPredicate.Or(x => x.Podroledescriptionid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            if (filterDto.DaemonSetPod?.Any() == true)
            {
                var daemonSetPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.DaemonSetPod)
                    daemonSetPredicate.Or(x => item == ConstantValueFilter.yes ? x.Daemonsetpod == true : x.Daemonsetpod == false);

                mainPredicate.And(daemonSetPredicate);
            }
            if (filterDto.IntraPodRules?.Any() == true)
            {
                var intrapodPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.IntraPodRules)
                    intrapodPredicate.Or(x => x.Intrapodrules == item);

                mainPredicate.And(intrapodPredicate);
            }
            if (filterDto.InterPodRules?.Any() == true)
            {
                var interpodPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.InterPodRules)
                    interpodPredicate.Or(x => x.Interpodrules == item);

                mainPredicate.And(interpodPredicate);
            }
            if (filterDto.IsEnhancedHa?.Any() == true)
            {
                var isEnhancedHaPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.IsEnhancedHa)
                    isEnhancedHaPredicate.Or(x => x.Isenhancedha.ToString() == item);

                mainPredicate.And(isEnhancedHaPredicate);
            }
            if (filterDto.PodTypeQos?.Any() == true)
            {
                var podTypeQosPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.PodTypeQos)
                    podTypeQosPredicate.Or(x => x.Podtypeqos == item);

                mainPredicate.And(podTypeQosPredicate);
            }
            if (filterDto.IsPersistanceStorageFlag?.Any() == true)
            {
                var isPersistancePredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.IsPersistanceStorageFlag)
                    isPersistancePredicate.Or(x => x.Ispersistancestorageflag == item);

                mainPredicate.And(isPersistancePredicate);
            }
            if (filterDto.IsProdhPaEnable?.Any() == true)
            {
                var isProdPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.IsProdhPaEnable)
                    isProdPredicate.Or(x => x.Isprodhpaenable.ToString() == item);

                mainPredicate.And(isProdPredicate);
            }


            #endregion


            return mainPredicate;
        }
        #endregion

        #region Get Filter

        public async Task<List<FilterValueDto>> GetFilteredValues(string propertyName, string propertyFilter, CnfClusterInfoQueryDto filterDto)
        {
            var filterCriteria = ApplyFilter(filterDto);

            var vnfinfoQueryResult = await CbomClusterinfoDetailsAsync(filterDto);

            var filteredQuery = vnfinfoQueryResult.Items;
            var result = propertyName switch
            {
                #region VnfInfo Fields
                "cnfClusterInfoId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfClusterInfoId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.CnfClusterInfoId))
                    .Distinct()
                    .ToList(),

                "cnfName" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfName.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto { Value = x.CnfNameId.ToString(), Text = x.CnfName })
                    .Distinct()
                    .ToList(),

                "cnfClusterName" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CnfClusterName.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Value = x.CnfClusterId.ToString(), Text = x.CnfClusterName })
               .Distinct()
               .ToList(),

                "nodePool" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NodePoolId.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.NodePoolId.ToString(), Text = x.NodePool })
                .Distinct()
                .ToList(),

                "hardwareType" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HardwareTypeId.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Value = x.HardwareTypeId.ToString(), Text = x.HardwareType })
               .Distinct()
               .ToList(),

                "opcoName" => filteredQuery.ToList()
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OpcoId.ToString().Contains(propertyFilter))
            .Select(x => new FilterValueDto { Text = x.OpcoName, Value = x.OpcoId.ToString() }
             )
            .Distinct()
            .ToList(),

                "site" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SiteId.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto { Text = x.Site, Value = x.SiteId.ToString() }
                     )
                    .Distinct()
                    .ToList(),
                "location" => filteredQuery.ToList()
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SiteId.ToString().Contains(propertyFilter))
              .Select(x => new FilterValueDto { Text = x.Location, Value = x.SiteId.ToString() }
               )
              .Distinct()
              .ToList(),


                "specialRequirements" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SpecialRequirements.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.SpecialRequirements))
                .Distinct()
                .ToList(),

                "hyperThreading" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HyperThreading.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.HyperThreading))
                    .Distinct()
                    .ToList(),

                "overProvisioning" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OverProvisioning.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.OverProvisioning))
                    .Distinct()
                    .ToList(),

                "nodePoolBreakup" => filteredQuery
              .Where(x => x.NodePoolBreakup != string.Empty && string.IsNullOrEmpty(propertyFilter) || x.NodePoolBreakup.ToString().Contains(propertyFilter))
              .Select(x => new FilterValueDto
              {
                  Value = x.NodePoolBreakup.ToString(),
                  Text =
              x.NodePoolBreakup
              })
              .Distinct()
              .ToList(),

                "workerNodeConfiguration" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.WorkerNodeConfiguration.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.WorkerNodeConfiguration))
                    .Distinct()
                    .ToList(),

                "hardware" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hardware.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto { Value = x.Hardware, Text = x.Hardware })
                    .Distinct()
                    .ToList(),

                "cpuKubelet" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CpuKubelet.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.CpuKubelet))
                    .Distinct()
                    .ToList(),
                "memKubelet" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.MemKubelet.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.MemKubelet))
                .Distinct()
                .ToList(),
                "cpuSystem" => filteredQuery
                                  .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CpuSystem.ToString().Contains(propertyFilter))
                                  .Select(x => new FilterValueDto(x.CpuSystem))
                                  .Distinct()
                                  .ToList(),
                "memSystem" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.MemSystem.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.MemSystem))
                .Distinct()
                .ToList(),

                "verticalDomain" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VerticalDomain.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto { Value = x.VerticalDomainId.ToString(), Text = x.VerticalDomain })
                    .Distinct()
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

                "comments" => filteredQuery
              .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Comments.ToString().Contains(propertyFilter))
              .Select(x => new FilterValueDto(x.Comments))
              .Distinct()
              .ToList(),

                "note" => filteredQuery
           .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Note.ToString().Contains(propertyFilter))
           .Select(x => new FilterValueDto(x.Note))
           .Distinct()
           .ToList(),

                "aggregateImageClusterSize" => filteredQuery
.Where(x => string.IsNullOrEmpty(propertyFilter) || x.AggregateImageClusterSize.ToString().Contains(propertyFilter))
.Select(x => new FilterValueDto(x.AggregateImageClusterSize))
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

            return result;
        }

        public async Task<List<FilterValueDto>> GetPodLevelFilteredValues(string propertyName, string propertyFilter, CnfClusterInfoQueryDto filterDto)
        {
            var filterCriteria = ApplyPodLevelFilter(filterDto);

            var vnfinfoQueryResult = await Task.Run(() => GetCnfPodAndCapacityRecords(filterCriteria));

            var filteredQuery = vnfinfoQueryResult;
            var result = propertyName switch
            {
                #region VnfInfo Fields
                "cnfclusterinfoid" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Cnfclusterinfoid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Cnfclusterinfoid))
                    .Distinct()
                    .ToList(),

                "cnfPodInfoId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Cnfpodinfoid.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.Cnfpodinfoid))
                    .Distinct()
                    .ToList(),

                "podTypeName" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Podtypeinfoid.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Value = x.Podtypeinfoid.ToString(), Text = x.Podtypeinfo.Podtypeinfoname })
               .Distinct()
               .ToList(),

                "functionStandardName" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Functionstandardid.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Value = x.Functionstandardid.ToString(), Text = x.Functionstandard.Functionname })
                .Distinct()
                .ToList(),

                "priorityName" => filteredQuery.ToList()
             .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Priorityid.ToString().Contains(propertyFilter))
            .Select(x => new FilterValueDto { Text = x.Priority.Description, Value = x.Priorityid.ToString() }
             )
            .Distinct()
            .ToList(),

                "podroleDescription" => filteredQuery.ToList()
                     .Where(x => x.Podroledescriptionid != null && string.IsNullOrEmpty(propertyFilter) || x.Podroledescriptionid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto { Text = x.Podroledescription.Podroledescription, Value = x.Podroledescriptionid.ToString() }
                     )
                    .Distinct()
                    .ToList(),

                "daemonSetPod" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Daemonsetpod.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto { Value = x.Daemonsetpod == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Text = x.Daemonsetpod == true ? ConstantValueFilter.Yes : ConstantValueFilter.No }
                     )
                    .Distinct()
                    .ToList(),

                "interPodRules" => filteredQuery.ToList()
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Interpodrules.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Text = x.Interpodrules.ToString(), Value = x.Interpodrules.ToString() }
                )
               .Distinct()
               .ToList(),

                "intraPodRules" => filteredQuery.ToList()
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Intrapodrules.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Text = x.Intrapodrules.ToString(), Value = x.Intrapodrules.ToString() }
                )
               .Distinct()
               .ToList(),

                "isEnhancedHa" => filteredQuery.ToList()
                      .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Isenhancedha.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto { Text = x.Isenhancedha.ToString(), Value = x.Isenhancedha.ToString() }
                      )
                     .Distinct()
                     .ToList(),

                "podTypeQos" => filteredQuery.ToList()
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Podtypeqos.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto { Text = x.Podtypeqos.ToString(), Value = x.Podtypeqos.ToString() }
                 )
                .Distinct()
                .ToList(),

                "isPersistanceStorageFlag" => filteredQuery.ToList()
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Ispersistancestorageflag.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Text = x.Ispersistancestorageflag.ToString(), Value = x.Ispersistancestorageflag.ToString() }
                )
               .Distinct()
               .ToList(),

                "isProdhPaEnable" => filteredQuery.ToList()
                .Where(x => x.Isprodhpaenable != null && string.IsNullOrEmpty(propertyFilter) || x.Isprodhpaenable.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto { Text = (x.Isprodhpaenable ?? false) ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = x.Isprodhpaenable.ToString() }
                )
               .Distinct()
               .ToList(),

                #endregion
                _ => new List<FilterValueDto>()
            };

            return result;
        }
        #endregion       


        #region Create and Insert

        public async Task<CBomCreatePageEntityDto> GetCBomCreatePageResourceAsync(List<short> opcoList)
        {

            var LocationDetails = await _dropdownDataServiceManager.GetAllLocationsBasedOpcos(opcoList);
            var CnfNameResources = _dropdownDataServiceManager.GetAllCnfNameResources();
            var cnfClusterNameResource = _dropdownDataServiceManager.GetAllCnfClusterResources();
            var PodTypeInfoResource = _dropdownDataServiceManager.GetAllPodTypeInfoResource();
            var FunctionStandardedNameResource = _dropdownDataServiceManager.GetAllFunctionStandardedNameResource();
            var verticalResource = _dropdownDataServiceManager.GetVerticalFromLCMDto().Result;
            var priorityResource = _dropdownDataServiceManager.GetCbomPriority().Result;
            var hardwareResource = _dropdownDataServiceManager.GetCbomHardware().Result;
            var financialVersion = _dropdownDataServiceManager.GetAllFinancialVersion();
            var model = new CBomCreatePageEntityDto
            {
                _cnfClusterInfoEntity = new Cnfclusterinfo
                {
                    Cnfpodinfo = new List<Cnfpodinfo>
                    {
                        new Cnfpodinfo
                        {
                            Cnfcapacity = new List<Cnfcapacity>() { new Cnfcapacity() }
                        }
                    }
                },

                CnfNameResources = CnfNameResources,
                cnfClusterNameResource = cnfClusterNameResource.AsEnumerable()
                     .GroupBy(x => new { x.Cnfclustername, x.Cnfnameid })
                    .Select(g => g.First())
                    .OrderBy(x => x.Cnfclustername)
                    .ToList(),

                cnfClusterNodePoolResource = cnfClusterNameResource.AsEnumerable()
                    .GroupBy(x => new { x.Cnfclustername, x.Cnfnameid })
                    .Select(g => g.First())
                    .OrderBy(x => x.Nodepool)
                    .ToList(),

                PodTypeInfoResource = PodTypeInfoResource.AsEnumerable()
                    .GroupBy(x => x.Podtypeinfoname)
                    .Select(g => g.First())
                    .OrderBy(x => x.Podtypeinfoname)
                    .ToList(),

                PodTypeDescriptionInfoResource = PodTypeInfoResource.AsEnumerable()
                    .GroupBy(x => x.Podroledescription)
                    .Select(g => g.First())
                    .OrderBy(x => x.Podroledescription)
                    .ToList(),

                OpcoBasedLocationResource = LocationDetails,
                FunctionStandardedNameResource = FunctionStandardedNameResource,
                priorityResource = priorityResource,
                hardwareResource = hardwareResource,
                FinancialVersion = financialVersion,
                verticalResource = verticalResource.Select(x => new KeyValuePairDto
                {
                    Key = (short)x.Key,
                    Text = x.Value
                }).ToList(),

            };
            return model;

        }
        #endregion  

        #region Edit CBOM Page 
        public async Task<ResultDto> GetEditCBomPageResourceAsync(long infoId,List<short> opcoList)
        {
            var LocationDetails = await _dropdownDataServiceManager.GetAllLocationsBasedOpcos(opcoList);
            var CnfNameResources = _dropdownDataServiceManager.GetAllCnfNameResources();
            var cnfClusterNameResource = _dropdownDataServiceManager.GetAllCnfClusterResources();
            var PodTypeInfoResource = _dropdownDataServiceManager.GetAllPodTypeInfoResource();
            var FunctionStandardedNameResource = _dropdownDataServiceManager.GetAllFunctionStandardedNameResource();
            var verticalResource = _dropdownDataServiceManager.GetVerticalFromLCMDto().Result;
            var priorityResource = _dropdownDataServiceManager.GetCbomPriority().Result;
            var hardwareResource = _dropdownDataServiceManager.GetCbomHardware().Result;
            var financialVersion = _dropdownDataServiceManager.GetAllFinancialVersion();
            var cnfInFoEntity = _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Cnfclusterinfoid == infoId)
              .Include(x => x.Cnfpodinfo).ThenInclude(x => x.Cnfcapacity).FirstOrDefault();

            if (cnfInFoEntity == null)
            {
                return new ResultDto() { Info = ResultMessages.EntryDeleteNotExists, Warning = true };
            }

            if (CnfNameResources.Any(x => x.Key == cnfInFoEntity.Cnfnameid) == false)
            {
                var tempVfName = _repositoryWrapper.CnfNameRepository.FindByCondition(t => t.Cnfnameid == cnfInFoEntity.Cnfnameid).FirstOrDefault();
                if (tempVfName != null)
                {
                    CnfNameResources.Add(new KeyValuePairDto { Key = (short)tempVfName.Cnfnameid, Text = tempVfName.Cnfdescription });
                }
            }

            if (cnfClusterNameResource.Any(x => x.Cnfclusterid == cnfInFoEntity.Cnfnameid) == false)
            {
                var tempClustername = _repositoryWrapper.ClusterNameRepository.FindByCondition(t => t.Clusternameid == cnfInFoEntity.Cnfnameid).FirstOrDefault();
                if (tempClustername != null)
                {
                    cnfClusterNameResource.Add(new Cnfcluster
                    {
                        Cnfclusterid = tempClustername.Clusternameid,
                        Cnfclustername = tempClustername.Clusterdescription

                    });
                }
            }

            var model = new CBomCreatePageEntityDto
            {
                FinancialVersion = financialVersion,
                CnfNameResources = CnfNameResources,
                cnfClusterNameResource = cnfClusterNameResource.AsEnumerable()
                   .GroupBy(x => new { x.Cnfclustername, x.Cnfnameid })
                    .Select(g => g.First())
                    .OrderBy(x => x.Cnfclustername)
                    .ToList(),

                cnfClusterNodePoolResource = cnfClusterNameResource.AsEnumerable()
                     .GroupBy(x => new { x.Cnfclustername, x.Cnfnameid })
                    .Select(g => g.First())
                    .OrderBy(x => x.Nodepool)
                    .ToList(),

                PodTypeInfoResource = PodTypeInfoResource.AsEnumerable()
                    .GroupBy(x => x.Podtypeinfoname)
                    .Select(g => g.First())
                    .OrderBy(x => x.Podtypeinfoname)
                    .ToList(),

                PodTypeDescriptionInfoResource = PodTypeInfoResource.AsEnumerable()
                    .GroupBy(x => x.Podroledescription)
                    .Select(g => g.First())
                    .OrderBy(x => x.Podroledescription)
                    .ToList(),
                OpcoBasedLocationResource = LocationDetails,
                _cnfClusterInfoEntity = cnfInFoEntity,
                FunctionStandardedNameResource = FunctionStandardedNameResource,
                priorityResource = priorityResource,
                verticalResource = verticalResource.Select(x => new KeyValuePairDto
                {
                    Key = (short)x.Key,
                    Text = x.Value
                }).ToList(),
                hardwareResource = hardwareResource
            };
            return new ResultDto()
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = model
            };

        }

        #endregion

        #region Add Cbom
        public async Task<Cnfclusterinfo> EntityExists(CBomInsertUpdateDto dto, bool isUpdate = false)
        {
            var entityExists = await _repositoryWrapper.CnfClusterInfoRepository.
                FindByCondition(
                    x => x.Cnfnameid == dto._cnfClusterInfoEntity.Cnfnameid && x.Cnfclusterid == dto._cnfClusterInfoEntity.Cnfclusterid
                    && x.Opcoid == dto._cnfClusterInfoEntity.Opcoid && x.Siteid == dto._cnfClusterInfoEntity.Siteid
                    && x.Hardware == dto._cnfClusterInfoEntity.Hardware && x.Cnfclusternodepoolid == dto._cnfClusterInfoEntity.CnfClusterNodePoolId,
                   true)
                .OrderByDescending(x => x.Creationdate).ToListAsync();

            if (isUpdate && entityExists.Count > 0)
            {
                var notUpdatedEntity = entityExists.Where(x => x.Cnfclusterinfoid != dto._cnfClusterInfoEntity.Cnfclusterinfoid).FirstOrDefault();
                return notUpdatedEntity == null ? null : notUpdatedEntity;
            }
            return entityExists.FirstOrDefault();
        }
        public async Task<ResultDto> AddCBomDetailAsync(CBomInsertUpdateDto dto, bool isUpdate = false, bool isImportData = false)
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
                            Data = new { id = entityExists.Cnfclusterinfoid, orphanDeleted = true }
                        };

                    }
                    else
                    {
                        return new ResultDto
                        {
                            Warning = true,
                            Info = (bool)entityExists?.Deleted ? ResultMessages.EntryAddExistsDeleted : ResultMessages.EntryAddExists,
                            Data = entityExists.Cnfclusterinfoid
                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }


            }
            return await AddCnfInfoBaseAsync(dto, isUpdate);
        }

        public async Task<ResultDto> AddCnfInfoBaseAsync(CBomInsertUpdateDto dtoCnfCnInfo, bool isUpdate = false)
        {
            Cnfclusterinfo _cnfClusterinfoEntity;

            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    if (dtoCnfCnInfo._cnfClusterInfoEntity.Cnfclusterinfoid == 0)
                    {
                        _cnfClusterinfoEntity = new Cnfclusterinfo
                        {
                            Cnfnameid = dtoCnfCnInfo._cnfClusterInfoEntity.Cnfnameid,
                            Cnfclusterid = (long)dtoCnfCnInfo._cnfClusterInfoEntity.CnfClusterNodePoolId,// ** Dn't change need to change store same id need to remove  instanceItem.CnfClusterNodePoolId;
                            Siteid = dtoCnfCnInfo._cnfClusterInfoEntity.Siteid,
                            Opcoid = dtoCnfCnInfo._cnfClusterInfoEntity.Opcoid,
                            Nodepoolbreakup = dtoCnfCnInfo._cnfClusterInfoEntity.Nodepoolbreakup,
                            Specialrequirements = dtoCnfCnInfo._cnfClusterInfoEntity.Specialrequirements,
                            Hyperthreading = dtoCnfCnInfo._cnfClusterInfoEntity.Hyperthreading,
                            Overprovisioning = dtoCnfCnInfo._cnfClusterInfoEntity.Overprovisioning,
                            Workernodeconfiguration = dtoCnfCnInfo._cnfClusterInfoEntity.Workernodeconfiguration,
                            Hardware = dtoCnfCnInfo._cnfClusterInfoEntity.Hardware,
                            Cpukubelet = dtoCnfCnInfo._cnfClusterInfoEntity.Cpukubelet,
                            Memkubelet = dtoCnfCnInfo._cnfClusterInfoEntity.Memkubelet,
                            Cpusystem = dtoCnfCnInfo._cnfClusterInfoEntity.Cpusystem,
                            Memsystem = dtoCnfCnInfo._cnfClusterInfoEntity.Memsystem,
                            Verticalresponsibleid = dtoCnfCnInfo._cnfClusterInfoEntity.Verticalresponsibleid,
                            Cnfhardwareid = dtoCnfCnInfo._cnfClusterInfoEntity.Cnfhardwareid,
                            Filename = dtoCnfCnInfo._cnfClusterInfoEntity.Filename,
                            Revision = dtoCnfCnInfo._cnfClusterInfoEntity.Revision,
                            Notes = dtoCnfCnInfo._cnfClusterInfoEntity.Notes,
                            Comments = dtoCnfCnInfo._cnfClusterInfoEntity.Comments,
                            Cnfclusternodepoolid = dtoCnfCnInfo._cnfClusterInfoEntity.CnfClusterNodePoolId,
                            Aggregateimageclustersize = dtoCnfCnInfo._cnfClusterInfoEntity.Aggregateimageclustersize,

                        };

                        _repositoryWrapper.CnfClusterInfoRepository.Create(_cnfClusterinfoEntity);
                    }
                    else
                    {
                        _cnfClusterinfoEntity = _repositoryWrapper.CnfClusterInfoRepository
                            .FindByCondition(x => x.Cnfclusterinfoid == dtoCnfCnInfo._cnfClusterInfoEntity.Cnfclusterinfoid)
                            .FirstOrDefault();

                        if (_cnfClusterinfoEntity == null)
                        {
                            return new ResultDto
                            {
                                Warning = true,
                                Info = ResultMessages.EntryNotFound,
                                Data = dtoCnfCnInfo
                            };
                        }
                        _cnfClusterinfoEntity.Opcoid = dtoCnfCnInfo._cnfClusterInfoEntity.Opcoid;
                        _cnfClusterinfoEntity.Siteid = dtoCnfCnInfo._cnfClusterInfoEntity.Siteid;
                        _cnfClusterinfoEntity.Cnfnameid = dtoCnfCnInfo._cnfClusterInfoEntity.Cnfnameid; // ** Dn't change need to change store same id need to remove  instanceItem.CnfClusterNodePoolId;
                        _cnfClusterinfoEntity.Cnfclusterid = (long)dtoCnfCnInfo._cnfClusterInfoEntity.CnfClusterNodePoolId;
                        _cnfClusterinfoEntity.Nodepoolbreakup = dtoCnfCnInfo._cnfClusterInfoEntity.Nodepoolbreakup;
                        _cnfClusterinfoEntity.Specialrequirements = dtoCnfCnInfo._cnfClusterInfoEntity.Specialrequirements;
                        _cnfClusterinfoEntity.Hyperthreading = dtoCnfCnInfo._cnfClusterInfoEntity.Hyperthreading;
                        _cnfClusterinfoEntity.Overprovisioning = dtoCnfCnInfo._cnfClusterInfoEntity.Overprovisioning;
                        _cnfClusterinfoEntity.Workernodeconfiguration = dtoCnfCnInfo._cnfClusterInfoEntity.Workernodeconfiguration;
                        _cnfClusterinfoEntity.Hardware = dtoCnfCnInfo._cnfClusterInfoEntity.Hardware;
                        _cnfClusterinfoEntity.Cpukubelet = dtoCnfCnInfo._cnfClusterInfoEntity.Cpukubelet;
                        _cnfClusterinfoEntity.Memkubelet = dtoCnfCnInfo._cnfClusterInfoEntity.Memkubelet;
                        _cnfClusterinfoEntity.Cpusystem = dtoCnfCnInfo._cnfClusterInfoEntity.Cpusystem;
                        _cnfClusterinfoEntity.Memsystem = dtoCnfCnInfo._cnfClusterInfoEntity.Memsystem;
                        _cnfClusterinfoEntity.Verticalresponsibleid = dtoCnfCnInfo._cnfClusterInfoEntity.Verticalresponsibleid;
                        _cnfClusterinfoEntity.Cnfhardwareid = dtoCnfCnInfo._cnfClusterInfoEntity.Cnfhardwareid;
                        _cnfClusterinfoEntity.Filename = dtoCnfCnInfo._cnfClusterInfoEntity.Filename;
                        _cnfClusterinfoEntity.Revision = dtoCnfCnInfo._cnfClusterInfoEntity.Revision;
                        _cnfClusterinfoEntity.Notes = dtoCnfCnInfo._cnfClusterInfoEntity.Notes;
                        _cnfClusterinfoEntity.Comments = dtoCnfCnInfo._cnfClusterInfoEntity.Comments;
                        _cnfClusterinfoEntity.Cnfclusternodepoolid = dtoCnfCnInfo._cnfClusterInfoEntity.CnfClusterNodePoolId;

                        _cnfClusterinfoEntity.Aggregateimageclustersize = dtoCnfCnInfo._cnfClusterInfoEntity.Aggregateimageclustersize;
                        _repositoryWrapper.CnfClusterInfoRepository.Update(_cnfClusterinfoEntity);
                    }

                    await _repositoryWrapper.SaveAsync();

                    var ClusterInfoCapacityErrorEntity = await AddCnfPodInfo(
                            dtoCnfCnInfo._cnfClusterInfoEntity.Cnfpodinfo,
                            _cnfClusterinfoEntity.Cnfclusterinfoid
                        );

                    if (ClusterInfoCapacityErrorEntity.Count > 0)
                    {
                        await transaction.RollbackAsync();

                        return new ResultDto
                        {
                            Warning = true,
                            Info = "Duplicate exists in ClusterInfo/Capacity ",
                            Data = ClusterInfoCapacityErrorEntity.ToList()
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
                Data = _cnfClusterinfoEntity
            };
        }
        public async Task<List<string>> AddCnfPodInfo(List<CnfPodInfoDto> dtoInstances, long cnfClusterInfoId)
        {
            var errorCapacityError = new List<string>();
            var instanceErrorEntity = new List<string>();
            try
            {
                if (dtoInstances != null && dtoInstances.Count > 0)
                {

                    var existingInstanceEntities = _repositoryWrapper.CnfPodInfoRepository
                           .FindByCondition(x =>
                               x.Cnfclusterinfoid == cnfClusterInfoId
                              )
                           .ToList();

                    foreach (var instanceItem in dtoInstances)
                    {

                        var checkDuplicatesinstanceItem = existingInstanceEntities.Where(x => x.Podtypeinfoid == instanceItem.Podtypeinfoid &&
                        x.Podroledescriptionid == instanceItem.Podroledescriptionid &&
                        x.Cnfpodinfoid != instanceItem.Cnfpodinfoid).FirstOrDefault();
                        if (checkDuplicatesinstanceItem != null)
                        {
                            instanceErrorEntity.Add(ResultMessages.duplicateInfo);
                            continue;
                        }
                        var existingInstance = existingInstanceEntities.Where(x => x.Cnfpodinfoid == instanceItem.Cnfpodinfoid).FirstOrDefault();

                        Cnfpodinfo vmInstance;

                        if (instanceItem.Cnfpodinfoid == 0 && existingInstance == null)
                        {

                            vmInstance = new Cnfpodinfo();
                            _repositoryWrapper.CnfPodInfoRepository.Create(vmInstance);
                        }
                        else if (instanceItem.Cnfpodinfoid != 0 && existingInstance != null)
                        {

                            vmInstance = existingInstance;
                        }
                        else
                            continue;


                        vmInstance.Cnfclusterinfoid = cnfClusterInfoId;
                        vmInstance.Podtypeinfoid = (long)instanceItem.Podroledescriptionid;// ** Dn't change need to change store same id need to remove  instanceItem.Podtypeinfoid;
                        vmInstance.Functionstandardid = instanceItem.Functionstandardid;
                        vmInstance.Priorityid = instanceItem.Priorityid;

                        vmInstance.Daemonsetpod = instanceItem.Daemonsetpod;
                        vmInstance.Intrapodrules = instanceItem.Intrapodrules;
                        vmInstance.Interpodrules = instanceItem.Interpodrules;
                        vmInstance.Isenhancedha = instanceItem.Isenhancedha;
                        vmInstance.Podtypeqos = instanceItem.Podtypeqos;
                        vmInstance.Ispersistancestorageflag = instanceItem.Ispersistancestorageflag;
                        vmInstance.Isprodhpaenable = instanceItem.Isprodhpaenable;
                        vmInstance.Podroledescriptionid = instanceItem.Podroledescriptionid;

                        if (instanceItem.Cnfpodinfoid != 0 && existingInstance != null)
                            _repositoryWrapper.CnfPodInfoRepository.Update(vmInstance);

                        await _repositoryWrapper.SaveAsync();

                        instanceItem.Cnfpodinfoid = vmInstance.Cnfpodinfoid;
                        var capacityReturnEntity = await AddCnfCapacity(instanceItem.Cnfcapacity, instanceItem.Cnfpodinfoid);

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

            return errorCapacityError;

        }

        public async Task<List<string>> AddCnfCapacity(List<CnfCapacityDto> dtoCapacity, long cnfPodInfoId)
        {

            var errorCapacityError = new List<string>();
            try
            {
                if (dtoCapacity != null)
                {
                    var existingCapacity = _repositoryWrapper.CnfCapacityRepository.FindByCondition(x => x.Cnfpodinfoid == cnfPodInfoId
          ).ToList();


                    foreach (var capacityItem in dtoCapacity)
                    {

                        long cnfCapacityInfoId = capacityItem.Cnfcapacityid;

                        Cnfcapacity vmCapacity = new Cnfcapacity();

                        var duplicateCapacities = existingCapacity.Where(x => x.Financialyear == capacityItem.Financialyear
                        && x.Financialversion == capacityItem.Financialversion && x.Cnfcapacityid != cnfCapacityInfoId).ToList();


                        if (cnfCapacityInfoId == 0 && !duplicateCapacities.Any())
                        {

                            _repositoryWrapper.CnfCapacityRepository.Create(vmCapacity);
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
                                var existInstCapacity = existingCapacity.Where(x => x.Cnfcapacityid == cnfCapacityInfoId).FirstOrDefault();
                                if (existInstCapacity != null)
                                {
                                    vmCapacity = existInstCapacity ?? new Cnfcapacity();

                                }
                            }
                        }
                        vmCapacity.Cnfpodinfoid = cnfPodInfoId;
                        vmCapacity.Financialyear = capacityItem.Financialyear;
                        vmCapacity.Financialversion = capacityItem.Financialversion;
                        vmCapacity.Vcpurequestforpodtype = capacityItem.Vcpurequestforpodtype;
                        vmCapacity.Vcpulimitforpodtype = capacityItem.Vcpulimitforpodtype;
                        vmCapacity.Pcpurequestforpodtype = capacityItem.Pcpurequestforpodtype;
                        vmCapacity.Memrequestforpodtype = capacityItem.Memrequestforpodtype;
                        vmCapacity.Memlimitforpodtype = capacityItem.Memlimitforpodtype;
                        vmCapacity.Nonpresistentstorageforprodtype = capacityItem.Nonpresistentstorageforprodtype;
                        vmCapacity.Ispresistentvolumesrequired = capacityItem.Ispresistentvolumesrequired;
                        vmCapacity.Persistentvolumneaccessmode = capacityItem.Persistentvolumneaccessmode;
                        vmCapacity.Persistentstorageforpodtype = capacityItem.Persistentstorageforpodtype;
                        vmCapacity.Storageiopsforpodtype = capacityItem.Storageiopsforpodtype;
                        vmCapacity.Storagerworkloaddistribution = capacityItem.Storagerworkloaddistribution;
                        vmCapacity.Northsouthbandwidthforpodtype = capacityItem.Northsouthbandwidthforpodtype;
                        vmCapacity.Eastwestbandwidthforpodtype = capacityItem.Eastwestbandwidthforpodtype;
                        vmCapacity.Specialrequirementperpodtype = capacityItem.Specialrequirementperpodtype;
                        vmCapacity.Capacityspecialrequirement = capacityItem.Capacityspecialrequirement;
                        vmCapacity.Noofcnfinstancespersite = capacityItem.Noofcnfinstancespersite;
                        vmCapacity.Numberofpodsperpodtype = capacityItem.Numberofpodsperpodtype;

                        if (vmCapacity.Cnfcapacityid == 0)
                            _repositoryWrapper.CnfCapacityRepository.Create(vmCapacity);
                        else
                            _repositoryWrapper.CnfCapacityRepository.Update(vmCapacity);

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
        #region Delete Record     
        public async Task<ResultDto> GetLinkedInfoBasedOnCapacity(long id)
        {
            var vnfCapacityEntity = await _repositoryWrapper.CnfCapacityRepository
                          .FindByCondition(x => x.Cnfcapacityid == id)
                          .Include(x => x.Cnfpodinfo)
                              .ThenInclude(i => i.Cnfclusterinfo)
                          .FirstOrDefaultAsync();

            if (vnfCapacityEntity == null)
                return new ResultDto { Data = 0 }; // Or handle as a not found case

            var vnfPodInfoInstanceId = vnfCapacityEntity.Cnfpodinfoid;
            var vnfClusterInfoId = vnfCapacityEntity.Cnfpodinfo.Cnfclusterinfoid;


            var multipleCapacityExists = await _repositoryWrapper.CnfCapacityRepository
                .FindByCondition(x => x.Cnfpodinfoid == vnfPodInfoInstanceId && x.Cnfcapacityid != id)
                .AnyAsync();

            var multipleInstanceExists = await _repositoryWrapper.CnfPodInfoRepository
                .FindByCondition(x => x.Cnfpodinfoid != vnfPodInfoInstanceId && x.Cnfclusterinfoid == vnfClusterInfoId)
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


        public async Task<ResultDto> DeleteCnfCapacityBasedAsync(long id)
        {
            int deletedTableCount = Convert.ToInt16(GetLinkedInfoBasedOnCapacity(id).Result.Data.ToString());

            #region  

            var vnfCapacityEntity = await _repositoryWrapper.CnfCapacityRepository.FindByCondition(x => x.Cnfcapacityid == id).FirstOrDefaultAsync();

            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    if (vnfCapacityEntity != null)
                    {
                        if (deletedTableCount == 1)
                        {
                            _repositoryWrapper.CnfCapacityRepository.DeleteDeep(vnfCapacityEntity);
                            await _repositoryWrapper.SaveAsync();
                            await _repositoryWrapper.ClearTracker();
                        }
                        else
                        {
                            var vnfInstancesEntity = await _repositoryWrapper
                                .CnfPodInfoRepository.FindByCondition(x => x.Cnfpodinfoid == vnfCapacityEntity.Cnfpodinfoid).FirstOrDefaultAsync();

                            if (vnfInstancesEntity != null)
                            {
                                _repositoryWrapper.CnfCapacityRepository.DeleteDeep(vnfCapacityEntity);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                                _repositoryWrapper.CnfPodInfoRepository.DeleteDeep(vnfInstancesEntity);
                                await _repositoryWrapper.SaveAsync();
                                await _repositoryWrapper.ClearTracker();

                                if (deletedTableCount == 3)
                                {
                                    var vnfInfoEntity = _repositoryWrapper.CnfClusterInfoRepository.FindByCondition(x => x.Cnfclusterinfoid
                                    == vnfInstancesEntity.Cnfclusterinfoid).FirstOrDefault();

                                    if (vnfInfoEntity != null)
                                    {
                                        _repositoryWrapper.CnfClusterInfoRepository.DeleteDeep(vnfInfoEntity);
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

        public async Task<ResultDto> DeleteCbomAsync(long clusterInfoId = 0, long infoId = 0, long capacityId = 0)
        {
            using (var transaction = await _repositoryWrapper.BeginTransactionAsync())
            {
                try
                {
                    #region Delete Info, Instance, and Capacity
                    if (clusterInfoId != 0)
                    {
                        var clusterInfoEntity = _repositoryWrapper.CnfClusterInfoRepository
                            .FindByCondition(x => x.Cnfclusterinfoid == clusterInfoId)
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

                        var infoInstanceEntities = _repositoryWrapper.CnfPodInfoRepository
                            .FindByCondition(x => x.Cnfclusterinfoid == clusterInfoEntity.Cnfclusterinfoid)
                            .ToList();

                        foreach (var instance in infoInstanceEntities)
                        {
                            var capacityEntities = _repositoryWrapper.CnfCapacityRepository
                                .FindByCondition(x => x.Cnfpodinfoid == instance.Cnfpodinfoid)
                                .ToList();

                            foreach (var capacity in capacityEntities)
                            {
                                _repositoryWrapper.CnfCapacityRepository.DeleteDeep(capacity);
                            }

                            _repositoryWrapper.CnfPodInfoRepository.DeleteDeep(instance);
                        }
                        if (infoInstanceEntities != null && infoInstanceEntities.Count > 0)
                            _repositoryWrapper.CnfClusterInfoRepository.DeleteDeep(clusterInfoEntity);

                        await _repositoryWrapper.SaveAsync();
                    }
                    #endregion

                    #region Delete Instance and Capacity
                    else if (infoId != 0)
                    {
                        var instanceEntities = _repositoryWrapper.CnfPodInfoRepository
                            .FindByCondition(x => x.Cnfpodinfoid == infoId)
                            .ToList();

                        foreach (var instance in instanceEntities)
                        {
                            var capacityEntities = _repositoryWrapper.CnfCapacityRepository
                                .FindByCondition(x => x.Cnfpodinfoid == instance.Cnfpodinfoid)
                                .ToList();

                            foreach (var capacity in capacityEntities)
                            {
                                _repositoryWrapper.CnfCapacityRepository.DeleteDeep(capacity);
                            }

                            _repositoryWrapper.CnfPodInfoRepository.DeleteDeep(instance);
                        }
                    }
                    #endregion

                    #region Delete Only Capacity
                    else if (capacityId != 0)
                    {
                        var capacityEntities = _repositoryWrapper.CnfCapacityRepository
                            .FindByCondition(x => x.Cnfcapacityid == capacityId)
                            .ToList();

                        foreach (var capacity in capacityEntities)
                        {
                            _repositoryWrapper.CnfCapacityRepository.DeleteDeep(capacity);
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

        public async Task<QueryResultDto<CnfInfoAndCbomExportGridDto>> FindWithConditionAsync(CnfClusterInfoQueryDto filterDto)
        {

            var predicateResult = ApplyFilter(filterDto);

            if (filterDto.Deleted == true)
            {
                predicateResult = predicateResult.And(x => x.Deleted.Value);
            }

            var cnfClusterinfoQueryResult = await Task.Run(() => GetCbomRecords(predicateResult).AsEnumerable()
                       .Select(p => CnfClusterInfoMapper.GetCnfClusterInfo(p)).ToList());

            var capacityPredicateResult = ApplyFilterForCapacityEntity(filterDto);

            if (capacityPredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in cnfClusterinfoQueryResult.SelectMany(x => x.CnfPodInfo).ToList())
                {
                    item.CnfCapacity = item.CnfCapacity.Where(capacityPredicateResult).ToList();
                }
            }
            var paginatedRecords = await Task.Run(() => cnfClusterinfoQueryResult.AsQueryable().ApplyOrdering(filterDto, GetColumnsMap())
               .ApplyPaging(filterDto));

            var mappedRecords = paginatedRecords.ToList()
                    .SelectMany(xclusterInfo => (xclusterInfo?.CnfPodInfo ?? Enumerable.Empty<CnfPodInfo>()),
        (xclusterInfo, xpodInsta) => new { xclusterInfo, xpodInsta })
                 .SelectMany(xcapacity =>
        (xcapacity?.xpodInsta?.CnfCapacity ?? Enumerable.Empty<CnfCapacity>()), (xpodInfo, xcapacity) => new CbomExportGridDto
        {
            #region cnf ClusterInfo
            RequestType = ConstantValueFilter.CnfRequestType,
            K8ClusterName = xpodInfo.xclusterInfo.K8ClusterAliasName,
            CnfClusterInfoId = xpodInfo.xclusterInfo.CnfClusterInfoId,
            CnfNameId = xpodInfo.xclusterInfo.CnfNameId,
            CnfName = xpodInfo.xclusterInfo.CnfNameDescription,
            CnfClusterId = xpodInfo.xclusterInfo.CnfClusterId,
            CnfClusterName = xpodInfo.xclusterInfo.CnfClusterIdName,
            NodePool = xpodInfo.xclusterInfo.NodePoolName,
            NodePoolId = (long)xpodInfo.xclusterInfo.CnfClusterNodePoolId,
            HardwareTypeId = xpodInfo.xclusterInfo.CnfHardwareId,
            Hardware = xpodInfo.xclusterInfo.HardwareDescription,
            OpcoId = xpodInfo.xclusterInfo.OpcoId,
            OpcoName = xpodInfo.xclusterInfo.OpcoIdName,
            Location = xpodInfo.xclusterInfo.LocationName,
            Site = xpodInfo.xclusterInfo.SiteName,
            SiteId = xpodInfo.xclusterInfo.SiteId,
            NodePoolBreakup = xpodInfo.xclusterInfo.NodePoolBreakup != null ?
                 Convert.ToBoolean(xpodInfo.xclusterInfo.NodePoolBreakup) == true ? ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,
            SpecialRequirements = xpodInfo.xclusterInfo.SpecialRequirements,
            HyperThreading = xpodInfo.xclusterInfo.Hyperthreading,
            WorkerNodeConfiguration = xpodInfo.xclusterInfo.WorkerNodeConfiguration,
            OverProvisioning = xpodInfo.xclusterInfo.OverProvisioning,
            CpuKubelet = xpodInfo.xclusterInfo.CpuKubelet,
            MemKubelet = xpodInfo.xclusterInfo.MemKubelet,
            CpuSystem = xpodInfo.xclusterInfo.CpuSystem,
            MemSystem = xpodInfo.xclusterInfo.MemSystem,
            Comments = xpodInfo.xclusterInfo.Comments,
            Note = xpodInfo.xclusterInfo.Notes,
            FileName = xpodInfo.xclusterInfo.FileName,
            Revision = xpodInfo.xclusterInfo.Revision,
            AggregateImageClusterSize = xpodInfo.xclusterInfo.AggregateImageClusterSize,
            VerticalDomain = xpodInfo.xclusterInfo.VerticalResponsibleName,
            VerticalDomainId = xpodInfo.xclusterInfo.VerticalResponsibleId,
            HardwareType = xpodInfo.xclusterInfo.HardwareTypeDescription,
            #endregion

            #region
            CnfPodInfoId = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.CnfPodInfoId : 0,
            PodTypeInfoId = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.PodTypeInfoId : 0,
            PodTypeName = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.PodTypeInfoName : string.Empty,
            PodroleDescription = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.PodroleDescription : string.Empty,
            FunctionStandardId = Convert.ToInt32(xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.FunctionStandardId : 0),
            FunctionStandardName = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.FunctionStandardName : string.Empty,
            PriorityId = Convert.ToInt32(xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.PriorityId : 0),
            PriorityName = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.PriorityName : string.Empty,
            DaemonSetPod = xpodInfo.xpodInsta != null ? Convert.ToInt16(xpodInfo.xpodInsta.DaemonSetPod) == 1 ?
                                    ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,
            IntraPodRules = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.IntraPodRules : string.Empty,
            InterPodRules = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.InterPodRules : string.Empty,
            IsEnhancedHa = xpodInfo.xpodInsta != null ? Convert.ToInt16(xpodInfo.xpodInsta.IsEnhancedHa) == 1 ?
                              ConstantValueFilter.CamalTrue : ConstantValueFilter.CamalFalse : string.Empty,
            IsPersistanceStorageFlag = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.IsPersistanceStorageFlag : string.Empty,

            IsProdhPaEnable = xpodInfo.xpodInsta != null ? Convert.ToInt16(xpodInfo.xpodInsta.DaemonSetPod) == 1 ?
                                    ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,
            PodTypeQos = xpodInfo.xpodInsta != null ? xpodInfo.xpodInsta.PodTypeQos : string.Empty,
            #endregion

            #region  Capacity
            CnfCapacityId = xcapacity != null ? xcapacity.CnfCapacityId : 0,

            FinancialYear = xcapacity != null ? Convert.ToString(xcapacity.FinancialYear) : null,
            FinancialVersion = xcapacity != null ? "Q" + Convert.ToString(xcapacity.FinancialVersion) : null,

            VcpuRequestForPodType = xcapacity != null ? Convert.ToString(xcapacity.VcpuRequestForPodType) : null,
            VcpuLimitForPodType = xcapacity != null ? Convert.ToString(xcapacity.VcpuLimitForPodType) : null,
            PcpuRequestForPodType = xcapacity != null ? Convert.ToString(xcapacity.PcpuRequestForPodType) : null,
            MemRequestForPodType = xcapacity != null ? Convert.ToString(xcapacity.MemRequestForPodType) : null,
            MemLimitForPodType = xcapacity != null ? Convert.ToString(xcapacity.MemLimitForPodType) : null,



            NonPresistentStorageForProdType = xcapacity?.NonPresistentStorageForProdType,
            IsPresistentVolumesRequired = xcapacity != null ? Convert.ToInt16(xcapacity.IsPresistentVolumesRequired) == 1 ?
                                    ConstantValueFilter.Yes : ConstantValueFilter.No : string.Empty,

            PersistentVolumNeaccessMode = xcapacity != null ? Convert.ToString(xcapacity.PersistentVolumNeaccessMode) : null,
            PersistentStorageForPodType = xcapacity != null ? xcapacity.PersistentStorageForPodType : null,
            StorageIopsForPodType = xcapacity != null ? xcapacity.StorageIopsForPodType : null,
            StoragerWorkloadDistribution = xcapacity != null ? xcapacity.StoragerWorkloadDistribution : null,
            NorthSouthBandWidthForPodType = xcapacity != null ? xcapacity.NorthSouthBandWidthForPodType : null,
            EastWestBandWidthForPodType = xcapacity != null ? Convert.ToString(xcapacity.EastWestBandWidthForPodType) : null,
            ListOfCapacitySpecialRequirement = xcapacity != null ? Convert.ToString(xcapacity.ListOfCapacitySpecialRequirement) : null,
            SpecialRequirementPerPodType = xcapacity != null ? Convert.ToString(xcapacity.SpecialRequirementPerPodType) : null,
            NumberOfPodsPerPodType = Convert.ToString(xcapacity != null ? xcapacity.NumberOfPodsPerPodType : 0),
            NoOfCnfInstancesPerSite = Convert.ToString(xcapacity.NoOfCnfInstancesPerSite),
            #endregion


        })
                      .GroupBy(e => new { e.CnfPodInfoId, e.OpcoId, e.SiteId })
                            .Select(g =>
                            {
                                var first = g.First();
                                first._cnfCapacityDtoGrid = g.GroupBy(x => x.FinancialYear)
                                    .Select(yr => yr.OrderByDescending(x => x.FinancialVersion).FirstOrDefault())
                                    .Where(item => item != null) // prevent nulls
                                    .Select(item => new CnfCapacityDtoGrid
                                    {
                                        CnfCapacityId = item.CnfCapacityId,
                                        FinancialYear = item.FinancialYear,
                                        FinancialVersion = item.FinancialVersion,
                                        VcpuRequestForPodType = item.VcpuRequestForPodType,
                                        VcpuLimitForPodType = item.VcpuLimitForPodType,
                                        PcpuRequestForPodType = item.PcpuRequestForPodType,
                                        MemRequestForPodType = item.MemRequestForPodType,
                                        MemLimitForPodType = item.MemLimitForPodType,
                                        NonPresistentStorageForProdType = item.NonPresistentStorageForProdType,
                                        PersistentVolumNeaccessMode = item.PersistentVolumNeaccessMode,
                                        PersistentStorageForPodType = item.PersistentStorageForPodType,
                                        StorageIopsForPodType = item.StorageIopsForPodType,
                                        StoragerWorkloadDistribution = item.StoragerWorkloadDistribution,
                                        NorthSouthBandWidthForPodType = item.NorthSouthBandWidthForPodType,
                                        EastWestBandWidthForPodType = item.EastWestBandWidthForPodType,
                                        ListOfCapacitySpecialRequirement = item.ListOfCapacitySpecialRequirement,
                                        SpecialRequirementPerPodType = item.SpecialRequirementPerPodType,
                                        NumberOfPodsPerPodType = item.NumberOfPodsPerPodType,
                                        NoOfCnfInstancesPerSite = item.NoOfCnfInstancesPerSite,
                                    }).OrderByDescending(x => x.FinancialYear).ThenBy(x => x.FinancialVersion).ToList();
                                return first;
                            }).ToList();

            var cnfInfoRecords = mappedRecords.DistinctBy(x => x.K8ClusterName).Select(x => new CnfInfoSheetExportGridDto
            {
                #region cnf ClusterInfo
                RequestType = x.RequestType,
                K8ClusterName = x.K8ClusterName,
                NodePool = x.NodePool,
                NodePoolBreakup = x.NodePoolBreakup,
                SpecialRequirements = x.SpecialRequirements,
                HyperThreading = x.HyperThreading,
                WorkerNodeConfiguration = x.WorkerNodeConfiguration,
                OverProvisioning = x.OverProvisioning,
                CpuKubelet = x.CpuKubelet,
                MemKubelet = x.MemKubelet,
                CpuSystem = x.CpuSystem,
                MemSystem = x.MemSystem,
                Comments = x.Comments,
                Note = x.Note,
                AggregateImageClusterSize = x.AggregateImageClusterSize,
                Hardware = x.Hardware

                #endregion
            }).ToList();

            List<CnfInfoAndCbomExportGridDto> cbomMappedData = new List<CnfInfoAndCbomExportGridDto>();
            cbomMappedData.Add(new CnfInfoAndCbomExportGridDto
            {
                cbomExport = mappedRecords,
                cnfInfoExport = cnfInfoRecords
            });

            var totalCount = mappedRecords.Count();
            return new QueryResultDto<CnfInfoAndCbomExportGridDto>(
                new GenerateRenderForGrid<CnfInfoAndCbomExportGridDto>(_customColumnManager))
            {
                TotalItems = totalCount,
                Items = cbomMappedData
            };
        }

        #endregion


        #region // Capacity Filter
        public ExpressionStarter<Cnfcapacity> ApplyFilterForCapacity(CnfCapcityQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Cnfcapacity>(true);
            if (filterDto.Cnfpodinfoid?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.Cnfpodinfoid)
                    componentIdPredicate.Or(x => x.Cnfpodinfoid == item);

                mainPredicate.And(componentIdPredicate);
            }

            if (filterDto.Financialversion?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.Financialversion)
                    componentIdPredicate.Or(x => x.Financialversion == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.FinancialYear?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.FinancialYear)
                    componentIdPredicate.Or(x => x.Financialyear == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.NoOfCnfInstancesPersite?.Any() == true)
            {
                var VnfNamePredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.NoOfCnfInstancesPersite)
                {
                    VnfNamePredicate.Or(x => x.Noofcnfinstancespersite == item);
                }
                mainPredicate.And(VnfNamePredicate);
            }
            if (filterDto.NumberOfPodsPerPodType?.Any() == true)
            {
                var VnfInfoIdPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.NumberOfPodsPerPodType)
                {
                    VnfInfoIdPredicate.Or(x => x.Numberofpodsperpodtype == item);
                }
                mainPredicate.And(VnfInfoIdPredicate);
            }
            if (filterDto.VcpuLimitForPodType?.Any() == true)
            {
                var VnfVmTypeNamePredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.VcpuLimitForPodType)
                {
                    VnfVmTypeNamePredicate.Or(x => x.Vcpulimitforpodtype == item);
                }
                mainPredicate.And(VnfVmTypeNamePredicate);
            }
            if (filterDto.VcpuRequestForPodType?.Any() == true)
            {
                var VnfVmTypeNamePredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.VcpuRequestForPodType)
                {
                    VnfVmTypeNamePredicate.Or(x => x.Vcpurequestforpodtype == item);
                }
                mainPredicate.And(VnfVmTypeNamePredicate);
            }

            if (filterDto.MemRequestForPodType?.Any() == true)
            {
                var IntraVmTypePredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.MemRequestForPodType)
                {
                    IntraVmTypePredicate.Or(x => x.Memrequestforpodtype.ToString() == item);
                }
                mainPredicate.And(IntraVmTypePredicate);
            }


            if (filterDto.NonPresistentStorageForProdType?.Any() == true)
            {
                var InterVmTypePredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.NonPresistentStorageForProdType)
                {
                    InterVmTypePredicate.Or(x => x.Nonpresistentstorageforprodtype == item);
                }
                mainPredicate.And(InterVmTypePredicate);
            }

            if (filterDto.IsPresistentVolumesRequired?.Any() == true)
            {
                var VmWorkLoadTypePredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.IsPresistentVolumesRequired)
                {
                    VmWorkLoadTypePredicate.Or(x => x.Ispresistentvolumesrequired.ToString() == item);
                }
                mainPredicate.And(VmWorkLoadTypePredicate);
            }

            if (filterDto.PersistentVolumNeaccessMode?.Any() == true)
            {
                var NsxtPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.PersistentVolumNeaccessMode)
                {
                    NsxtPredicate.Or(x => x.Persistentvolumneaccessmode == item);
                }
                mainPredicate.And(NsxtPredicate);
            }
            if (filterDto.PersistentStorageForPodType?.Any() == true)
            {
                var VmStoragePredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.PersistentStorageForPodType)
                {
                    VmStoragePredicate.Or(x => x.Persistentstorageforpodtype == item);
                }
                mainPredicate.And(VmStoragePredicate);
            }
            if (filterDto.StorageIopsForPodType?.Any() == true)
            {
                var NumaPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.StorageIopsForPodType)
                {
                    NumaPredicate.Or(x => x.Storageiopsforpodtype == item);
                }
                mainPredicate.And(NumaPredicate);
            }
            if (filterDto.StoragerWorkloadDistribution?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.StoragerWorkloadDistribution)
                {
                    SocketPredicate.Or(x => x.Storagerworkloaddistribution == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.NorthSouthBandWidthForPodType?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.NorthSouthBandWidthForPodType)
                {
                    SocketPredicate.Or(x => x.Northsouthbandwidthforpodtype == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.EastWestBandWidthForPodType?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.EastWestBandWidthForPodType)
                {
                    SocketPredicate.Or(x => x.Eastwestbandwidthforpodtype == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.SpecialRequirementPerPodType?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.SpecialRequirementPerPodType)
                {
                    SocketPredicate.Or(x => x.Specialrequirementperpodtype == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.ListOfCapacitySpecialRequirement?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.ListOfCapacitySpecialRequirement)
                {
                    SocketPredicate.Or(x => x.Capacityspecialrequirement == item);
                }
                mainPredicate.And(SocketPredicate);
            }            

            return mainPredicate;
        }

        public ExpressionStarter<CnfCapacity> ApplyFilterForCapacityEntity(CnfClusterInfoQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<CnfCapacity>(true);
            if (filterDto.Cnfpodinfoid?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.Cnfpodinfoid)
                    componentIdPredicate.Or(x => x.CnfPodInfoId == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.CnfCapacityId?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.CnfCapacityId)
                    componentIdPredicate.Or(x => x.CnfCapacityId == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.Financialversion?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.Financialversion)
                    componentIdPredicate.Or(x => x.FinancialVersion == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.FinancialYear?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.FinancialYear)
                    componentIdPredicate.Or(x => x.FinancialYear == item);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.NoOfCnfInstancesPersite?.Any() == true)
            {
                var VnfNamePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.NoOfCnfInstancesPersite)
                {
                    VnfNamePredicate.Or(x => x.NoOfCnfInstancesPerSite == item);
                }
                mainPredicate.And(VnfNamePredicate);
            }
            if (filterDto.NumberOfPodsPerPodType?.Any() == true)
            {
                var VnfInfoIdPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.NumberOfPodsPerPodType)
                {
                    VnfInfoIdPredicate.Or(x => x.NumberOfPodsPerPodType == item);
                }
                mainPredicate.And(VnfInfoIdPredicate);
            }
            if (filterDto.VcpuLimitForPodType?.Any() == true)
            {
                var VnfVmTypeNamePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.VcpuLimitForPodType)
                {
                    VnfVmTypeNamePredicate.Or(x => x.VcpuLimitForPodType == item);
                }
                mainPredicate.And(VnfVmTypeNamePredicate);
            }
            if (filterDto.VcpuRequestForPodType?.Any() == true)
            {
                var VnfVmTypeNamePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.VcpuRequestForPodType)
                {
                    VnfVmTypeNamePredicate.Or(x => x.VcpuRequestForPodType == item);
                }
                mainPredicate.And(VnfVmTypeNamePredicate);
            }

            if (filterDto.MemRequestForPodType?.Any() == true)
            {
                var IntraVmTypePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.MemRequestForPodType)
                {
                    IntraVmTypePredicate.Or(x => x.MemRequestForPodType.ToString() == item);
                }
                mainPredicate.And(IntraVmTypePredicate);
            }
            if (filterDto.MemLimitForPodType?.Any() == true)
            {
                var IntraVmTypePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.MemLimitForPodType)
                {
                    IntraVmTypePredicate.Or(x => x.MemLimitForPodType.ToString() == item);
                }
                mainPredicate.And(IntraVmTypePredicate);
            }

            if (filterDto.NonPresistentStorageForProdType?.Any() == true)
            {
                var InterVmTypePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.NonPresistentStorageForProdType)
                {
                    InterVmTypePredicate.Or(x => x.NonPresistentStorageForProdType == item);
                }
                mainPredicate.And(InterVmTypePredicate);
            }

            if (filterDto.IsPresistentVolumesRequired?.Any() == true)
            {
                var VmWorkLoadTypePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.IsPresistentVolumesRequired)
                {
                    VmWorkLoadTypePredicate.Or(x => x.IsPresistentVolumesRequired.ToString() == item);
                }
                mainPredicate.And(VmWorkLoadTypePredicate);
            }

            if (filterDto.PersistentVolumNeaccessMode?.Any() == true)
            {
                var NsxtPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.PersistentVolumNeaccessMode)
                {
                    NsxtPredicate.Or(x => x.PersistentVolumNeaccessMode == item);
                }
                mainPredicate.And(NsxtPredicate);
            }
            if (filterDto.PersistentStorageForPodType?.Any() == true)
            {
                var VmStoragePredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.PersistentStorageForPodType)
                {
                    VmStoragePredicate.Or(x => x.PersistentStorageForPodType == item);
                }
                mainPredicate.And(VmStoragePredicate);
            }
            if (filterDto.StorageIopsForPodType?.Any() == true)
            {
                var NumaPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.StorageIopsForPodType)
                {
                    NumaPredicate.Or(x => x.StorageIopsForPodType == item);
                }
                mainPredicate.And(NumaPredicate);
            }
            if (filterDto.StoragerWorkloadDistribution?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.StoragerWorkloadDistribution)
                {
                    SocketPredicate.Or(x => x.StoragerWorkloadDistribution == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.NorthSouthBandWidthForPodType?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.NorthSouthBandWidthForPodType)
                {
                    SocketPredicate.Or(x => x.NorthSouthBandWidthForPodType == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.EastWestBandWidthForPodType?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.EastWestBandWidthForPodType)
                {
                    SocketPredicate.Or(x => x.EastWestBandWidthForPodType == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.SpecialRequirementPerPodType?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.SpecialRequirementPerPodType)
                {
                    SocketPredicate.Or(x => x.SpecialRequirementPerPodType == item);
                }
                mainPredicate.And(SocketPredicate);
            }
            if (filterDto.ListOfCapacitySpecialRequirement?.Any() == true)
            {
                var SocketPredicate = PredicateBuilder.New<CnfCapacity>();
                foreach (var item in filterDto.ListOfCapacitySpecialRequirement)
                {
                    SocketPredicate.Or(x => x.ListOfCapacitySpecialRequirement == item);
                }
                mainPredicate.And(SocketPredicate);
            }

            return mainPredicate;
        }

        private IQueryable<Cnfcapacity> GetCnfCapacityRecords(ExpressionStarter<Cnfcapacity> predicateResult)
        {
            var query = _repositoryWrapper.CnfCapacityRepository.FindByCondition(predicateResult)
                  .Include(x => x.CreationuserNavigation).Include(x => x.ModificationuserNavigation)
                  .AsQueryable();

            return query;
        }


        public async Task<List<FilterValueDto>> GetCapacityFilteredValuesAsync(string propertyName, string propertyFilter, CnfCapcityQueryDto filterDto)
        {
            var filterCriteria = ApplyFilterForCapacity(filterDto);

            var filteredQuery = await Task.Run(() => GetCnfCapacityRecords(filterCriteria));

            var result = propertyName switch
            {
                #region VNF VM Capacity
                "cnfCapacityId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Cnfcapacityid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Cnfcapacityid))
                    .Distinct()
                    .ToList(),

                "financialYear" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Financialyear.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Financialyear))
                    .Distinct()
                    .ToList(),

                "financialVersion" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Financialversion.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto { Text = $"Q{x.Financialversion.ToString()}", Value = x.Financialversion.ToString() })
                    .Distinct()
                    .ToList(),

                "noOfCnfInstancesPerSite" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Noofcnfinstancespersite.ToString().Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.Noofcnfinstancespersite))
               .Distinct()
               .ToList(),

                "numberOfPodsPerPodType" => filteredQuery
                .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Numberofpodsperpodtype.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto (x.Numberofpodsperpodtype))
                .Distinct()
                .ToList(),

                "vcpuLimitForPodType" => filteredQuery.ToList()
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vcpulimitforpodtype.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Vcpulimitforpodtype)
                 )
                .Distinct()
                .ToList(),

                "vcpuRequestForPodType" => filteredQuery.ToList()
                 .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vcpurequestforpodtype.ToString().Contains(propertyFilter))
                .Select(x => new FilterValueDto(x.Vcpurequestforpodtype)
                 )
                .Distinct()
                .ToList(),
                "memLimitForPodType" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Memlimitforpodtype.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Memlimitforpodtype)
                    )
                   .Distinct()
                   .ToList(),

                "memRequestForPodType" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Memrequestforpodtype.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Memrequestforpodtype)
                     )
                    .Distinct()
                    .ToList(),

                "nonPresistentStorageForProdType" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Nonpresistentstorageforprodtype.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Nonpresistentstorageforprodtype)
                     )
                    .Distinct()
                    .ToList(),

                "isPresistentVolumesRequired" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Ispresistentvolumesrequired.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto{ Text = x.Ispresistentvolumesrequired == true ? ConstantValueFilter.Yes : ConstantValueFilter.No, Value = x.Ispresistentvolumesrequired.ToString() }
                    )
                   .Distinct()
                   .ToList(),

                "persistentVolumNeaccessMode" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Persistentvolumneaccessmode.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Persistentvolumneaccessmode)
                    )
                   .Distinct()
                   .ToList(),

                "persistentStorageForPodType" => filteredQuery.ToList()
                      .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Persistentstorageforpodtype.ToString().Contains(propertyFilter))
                     .Select(x => new FilterValueDto(x.Persistentstorageforpodtype)
                      )
                     .Distinct()
                     .ToList(),

                "storageIopsForPodType" => filteredQuery.ToList()
                     .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Storageiopsforpodtype.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Storageiopsforpodtype)
                     )
                    .Distinct()
                    .ToList(),

                "storagerWorkloadDistribution" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Storagerworkloaddistribution.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Storagerworkloaddistribution)
                    )
                   .Distinct()
                   .ToList(),

                "northSouthBandWidthForPodType" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Northsouthbandwidthforpodtype.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Northsouthbandwidthforpodtype)
                    )
                   .Distinct()
                   .ToList(),
                "eastWestBandWidthForPodType" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Eastwestbandwidthforpodtype.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Eastwestbandwidthforpodtype)
                    )
                   .Distinct()
                   .ToList(),
                "specialRequirementPerPodType" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Specialrequirementperpodtype.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Specialrequirementperpodtype)
                    )
                   .Distinct()
                   .ToList(),
                "listOfCapacitySpecialRequirement" => filteredQuery.ToList()
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Capacityspecialrequirement.ToString().Contains(propertyFilter))
                   .Select(x => new FilterValueDto(x.Capacityspecialrequirement)
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
