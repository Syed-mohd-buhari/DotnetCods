using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.XBom.CBom;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.XBom.CBom;
using CAM.DataTransferObjects.QueryDto.XBom.VBom;
using CAM.Entities.Mappers.Vbom;
using CAM.Entities.Models.VBom;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.XBom
{
    public class CBomReportManager : BaseManager
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        public CBomReportManager(IEnumerable<IRepositoryWrapper> wrappers,
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
        public ExpressionStarter<Cnfclusterinfo> ApplyFilter(CbomReportQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Cnfclusterinfo>(true);


            if (filterDto.HardwareType?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var id in filterDto.HardwareType)
                    componentIdPredicate.Or(x => x.Cnfhardwareid == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.OpcoName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfclusterinfo>();
                foreach (var item in filterDto.OpcoName)
                    descriptionPredicate.Or(x => x.Opcoid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }



            return mainPredicate;
        }

        public async Task<QueryResultDto<CbomReportDto>> GetCbomAggregatedAndDisaggregatedReport(CbomReportQueryDto filterDto)
        {

            var predicateResult = ApplyFilter(filterDto);
            var rtn = new QueryResultDto<CbomReportDto>(new GenerateRenderForGrid<CbomReportDto>(_customColumnManager))
            {

            };

            var cnfinfoQueryResult =  await Task.Run(() => GetCbomRecords(predicateResult).ToList());

            var financialPredicateResult = PredicateBuilder.New<Cnfcapacity>(true);

            if (filterDto.FinancialYear?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Cnfcapacity>();
                foreach (var item in filterDto.FinancialYear)
                    componentIdPredicate.Or(x => x.Financialyear == item);

                financialPredicateResult.And(componentIdPredicate);
            }

            var cnfNamePredicateResult = PredicateBuilder.New<Cnfpodinfo>(true);

            if (filterDto.CnfName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Cnfpodinfo>();
                foreach (var item in filterDto.CnfName)
                    descriptionPredicate.Or(x => x.Cnfclusterinfo.Cnfnameid == item);

                cnfNamePredicateResult.And(descriptionPredicate);
            }

            if (financialPredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in cnfinfoQueryResult.SelectMany(x => x.Cnfpodinfo).ToList())
                {
                    item.Cnfcapacity = item.Cnfcapacity.Where(financialPredicateResult).ToList();
                }
            }
            if (cnfNamePredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in cnfinfoQueryResult.ToList())
                {
                    item.Cnfpodinfo = item.Cnfpodinfo.Where(cnfNamePredicateResult).ToList();
                }
            }

            var result = cnfinfoQueryResult
                                .SelectMany(clusterInfo => clusterInfo.Cnfpodinfo.SelectMany(cnf =>
                                    cnf.Cnfcapacity.Select(cap => new
                                    {
                                        cnfName = clusterInfo.Cnfname.Cnfdescription,
                                        cnfnameId = clusterInfo.Cnfname.Cnfnameid,
                                        PodTypeName = cnf.Podtypeinfo.Podtypeinfoname,
                                        HardwareType = clusterInfo.Cnfhardware.Description,
                                        numberOfCnfInstances = cap.Noofcnfinstancespersite ?? 0,
                                        numberofPodsPerType = cap.Numberofpodsperpodtype,
                                        vCpurequestforPodType = cap.Vcpurequestforpodtype,
                                        memRequestforPodType = cap.Memrequestforpodtype,
                                        nonpersitentStorageforPodType = cap.Nonpresistentstorageforprodtype
                                    })
                                ))
                                .GroupBy(x => new { x.cnfName, x.PodTypeName ,x.HardwareType})
                                .Select(g =>
                                {
                                    var maxPodsPerType =Convert.ToInt32(g.Max(x => x.numberofPodsPerType));
                                    var maxInstance = g.Max(x => x.numberOfCnfInstances);
                                    var maxVCpu = g.Max(x => x.vCpurequestforPodType);
                                    var maxMemReq = g.Max(x => x.memRequestforPodType);
                                    var maxStorage = g.Max(x => x.nonpersitentStorageforPodType);
                                    var hardwareType = g.First().HardwareType;
                                    var cnfNamesResource = g.Select(x => 
                                    new KeyValuePairDto
                                    {
                                        Key = (short)x.cnfnameId,
                                        Text = x.cnfName
                                    }).ToList();

                                    var cbomDisaggregatedViews = Enumerable.Range(1, maxPodsPerType)
                                        .Select(n => new CbomDisaggregatedView
                                        {
                                            CnfName = g.Key.cnfName,
                                            PodTypeName = g.Key.PodTypeName,
                                            NumberOfPodsPerPodType = maxPodsPerType,
                                            NumberOfCnfInstancesPerSite = maxInstance,
                                            MemRequestForPodType = maxMemReq,
                                            NonPersistentStoragePerPodType = maxStorage,
                                        })
                                        .ToList();

                                    return new CbomReportDto
                                    {
                                        CnfName = g.Key.cnfName,
                                        PodTypeName = g.Key.PodTypeName,
                                        HardwareType = hardwareType,
                                        NumberOfPodsPerPodType = maxPodsPerType,
                                        NumberOfCnfInstancesPerSite = maxInstance,
                                        VcpuRequestForPodType = Convert.ToInt64(maxVCpu),
                                        MemRequestForPodType = maxMemReq,
                                        NonPersistentStoragePerPodType = maxStorage,
                                        cbomDisaggregatedView = cbomDisaggregatedViews,
                                        cnfNames = cnfNamesResource

                                    };
                                })
                                .ToList();



            rtn.TotalItems = result.Count;
            rtn.Items = result;

            return rtn;
        }
        public async Task<ResultDto> GetAllResourcesDropDown(List<short> opcoList)
        {
            try
            {
                var allOpco = await _dropdownDataServiceManager.GetOpcos(false,true, false, false, opcoList);
                var HardwareTypes = await _dropdownDataServiceManager.GetCbomHardware();
                var CurrentFy = DateTime.Now.Year - 1;
                var cnfName = _dropdownDataServiceManager.GetAllCnfNameResources();

                return new ResultDto
                {
                    Data = new
                    {
                        allOpcos = allOpco.Data,
                        hardwareTypes = HardwareTypes,
                        currentFy = CurrentFy,
                        cnfNames = cnfName
                    }

                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new ResultDto
                {
                    Data = ex.Message
                };
            }

        }



    }
}
