using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.NetworkElementAsPlanned;
using CAM.DataTransferObjects.Entita.XBom.VBom;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
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
    public class VBomReportManager : BaseManager
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DropdownDataServiceManager _dropdownDataServiceManager;

        public VBomReportManager(IEnumerable<IRepositoryWrapper> wrappers,
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
        public ExpressionStarter<Vnfclusterinfo> ApplyFilter(VbomReportQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Vnfclusterinfo>(true);


            if (filterDto.HardwareType?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var id in filterDto.HardwareType)
                    componentIdPredicate.Or(x => x.Hardwaretypeid.ToString() == id);

                mainPredicate.And(componentIdPredicate);
            }
            if (filterDto.OpcoName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
                foreach (var item in filterDto.OpcoName)
                    descriptionPredicate.Or(x => x.Opcoid.ToString() == item);

                mainPredicate.And(descriptionPredicate);
            }
            //if (filterDto.FinancialYear?.Any() == true)
            //{
            //    var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
            //    foreach (var item in filterDto.FinancialYear)
            //        descriptionPredicate.Or(x => x.Vnfinfo.SelectMany(x => x.Vnfvmcapacity.Select(x => x.Financialyear)).FirstOrDefault() == item);

            //    mainPredicate.And(descriptionPredicate);
            //}
            //if (filterDto.VnfName?.Any() == true)
            //{
            //    var descriptionPredicate = PredicateBuilder.New<Vnfclusterinfo>();
            //    foreach (var item in filterDto.VnfName)
            //        descriptionPredicate.Or(x => x.Vnfinfo.Any(f=>f.Vnfnameid == item));

            //    mainPredicate.And(descriptionPredicate);
            //}


            return mainPredicate;
        }

        public async Task<QueryResultDto<VbomReportDto>> GetVbomAggregatedAndDisaggregatedReport(VbomReportQueryDto filterDto)
        {

            var predicateResult = ApplyFilter(filterDto);
            var rtn = new QueryResultDto<VbomReportDto>(new GenerateRenderForGrid<VbomReportDto>(_customColumnManager))
            {

            };

            var vnfinfoQueryResult =  await Task.Run(() => GetVbomRecords(predicateResult).ToList());

            var financialPredicateResult = PredicateBuilder.New<Vnfvmcapacity>(true);

            if (filterDto.FinancialYear?.Any() == true)
            {
                var componentIdPredicate = PredicateBuilder.New<Vnfvmcapacity>();
                foreach (var item in filterDto.FinancialYear)
                    componentIdPredicate.Or(x => x.Financialyear == item);

                financialPredicateResult.And(componentIdPredicate);
            }

            var vnfNamePredicateResult = PredicateBuilder.New<Vnfinfo>(true);

            if (filterDto.VnfName?.Any() == true)
            {
                var descriptionPredicate = PredicateBuilder.New<Vnfinfo>();
                foreach (var item in filterDto.VnfName)
                    descriptionPredicate.Or(x => x.Vnfname.Vnfnameid == item);

                vnfNamePredicateResult.And(descriptionPredicate);
            }

            if (financialPredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in vnfinfoQueryResult.SelectMany(x => x.Vnfinfo).ToList())
                {
                    item.Vnfvmcapacity = item.Vnfvmcapacity.Where(financialPredicateResult).ToList();
                }
            }
            if (vnfNamePredicateResult.Body.ToString().Trim().ToLower() != "true")
            {
                foreach (var item in vnfinfoQueryResult.ToList())
                {
                    item.Vnfinfo = item.Vnfinfo.Where(vnfNamePredicateResult).ToList();
                }
            }

            var result = vnfinfoQueryResult
                                .SelectMany(clusterInfo => clusterInfo.Vnfinfo.SelectMany(vnf =>
                                    vnf.Vnfvmcapacity.Select(cap => new
                                    {
                                        VnfName = vnf.Vnfname.Vnfdescription,
                                        vnfnameId = vnf.Vnfname.Vnfnameid,
                                        VmTypeName = vnf.Vnfvmtypename.Vmtypedescription,
                                        HardwareType = clusterInfo.Hardwaretype.Description,
                                        NoOfVmsPerType = cap.Noofvmspertype ?? 0,
                                        VnfCpuPerVm = cap.Vnfcpupervm,
                                        RamPerVm = cap.Rampervm,
                                        DataDisk = cap.Datadisk
                                    })
                                ))
                                .GroupBy(x => new { x.VnfName, x.VmTypeName, x.HardwareType })
                                .Select(g =>
                                {
                                    var maxVmsPerType =Convert.ToInt32(g.Max(x => x.NoOfVmsPerType));
                                    var maxCpu = g.Max(x => x.VnfCpuPerVm);
                                    var maxRam = g.Max(x => x.RamPerVm);
                                    var maxDisk = g.Max(x => x.DataDisk);
                                    var hardwareType = g.First().HardwareType;
                                    var vnfNamesResource = g.Select(x => 
                                    new KeyValuePairDto
                                    {
                                        Key = (short)x.vnfnameId,
                                        Text = x.VnfName
                                    }).ToList();

                                    var vbomDisaggregatedViews = Enumerable.Range(1, maxVmsPerType)
                                        .Select(n => new VbomDisaggregatedView
                                        {
                                            VnfName = g.Key.VnfName,
                                            VmTypeName = g.Key.VmTypeName,
                                            vCpu = maxCpu,
                                            Ram = maxRam,
                                            DataDisk = maxDisk
                                        })
                                        .ToList();

                                    return new VbomReportDto
                                    {
                                        VnfName = g.Key.VnfName,
                                        VmTypeName = g.Key.VmTypeName,
                                        HardwareType = hardwareType,
                                        NumberOfVmsPerType = g.Max(x => x.NoOfVmsPerType),
                                        NumberOfvCpuPerType = maxCpu,
                                        RamPerGb = maxRam,
                                        StoragePerVmDataDisk = maxDisk,
                                        vbomDisaggregatedViews = vbomDisaggregatedViews,
                                        vnfNames = vnfNamesResource

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
                var HardwareTypes =  _dropdownDataServiceManager.GetVOMHardwareType();
                var CurrentFy = DateTime.Now.Year - 1;
                var vnfName = _dropdownDataServiceManager.GetVnfName();

                return new ResultDto
                {
                    Data = new
                    {
                        allOpcos = allOpco.Data,
                        hardwareTypes = HardwareTypes,
                        currentFy = CurrentFy,
                        vnfNames = vnfName
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
