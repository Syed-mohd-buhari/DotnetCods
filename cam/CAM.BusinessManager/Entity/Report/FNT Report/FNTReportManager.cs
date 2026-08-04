using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.GenericReports;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.Rules;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.DataTransferObjects.Entita.Report;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.BPT;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using MathNet.Numerics.Financial;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.FNT_Report
{
    public class FNTReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _gridmanager;
        private CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private GenericReportGenration _genericReportGenration;

        public FNTReportManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager, CommonManager commonManager,
            GenericReportGenration genericReportGenration,ILoggerManager logger, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _gridmanager = manager;
            _commonManager = commonManager;
            _logger = logger;
            _genericReportGenration = genericReportGenration;
        }

        #region UI Members
        public ExpressionStarter<Networkelementsasplanned> ApplyFilter(FNTQueryDto fntQueryDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);

            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);
            if (fntQueryDto?.Hostname != null && fntQueryDto.Hostname.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in fntQueryDto?.Hostname)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<Networkelementsasplanned> GetDisaggregatedQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {

            var environmentId =  _commonManager.GetEnvironmentId("production");

            var result = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).AsSplitQuery().AsNoTracking()
                                .Where(x =>
                                    x.Designcomponent.Systemtype.Deleted == false &&
                                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false) &&
                                    x.Lcmengineeringid != null && x.Environment.Environmentid == environmentId
                                )
                                .Include(x => x.Opco)
                                .Include(x => x.Environment)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmoperationalcontracts).ThenInclude(x => x.Operationalcontract)
                                .Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x => x.Subdomainspoc)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Criticalassettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Operatingsystem)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.Identitiesasis).ThenInclude(x => x.Category)
                                .Include(x => x.ModificationuserNavigation);
            return result;
        }

        public async Task<QueryResultDto<FNTReportDtoGrid>> FindWithCondition(FNTQueryDto fntQueryDto)
        {

            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyFilter(fntQueryDto);

            var query = await Task.Run(() => GetDisaggregatedQuery(predicateResult).AsQueryable());
            var orderedData = query.ApplyOrdering(fntQueryDto, GetColumnsMapDB()).OrderByDescending(x => x.Modificationdate).ToList();
            var rtn = new QueryResultDto<FNTReportDtoGrid>(new GenerateRenderForGrid<FNTReportDtoGrid>(_gridmanager))
            {
                TotalItems = query.Count()
            };


            var allOperationalContracts = _repositoryWrapper.LCMOperationalContracts.FindAll().AsNoTracking().AsSplitQuery().Include(x => x.Operationalcontract).ToList();
            var location = _repositoryWrapper.Location.FindByCondition(x => query.AsEnumerable().Select(y => y.Locationid).ToList().Contains(x.Locationid)).ToList();

            var AssetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

            var hardWare = query.Select(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware)).ToList();

            var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(AssetIdAndOpcoId).ToList();
            var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(AssetIdAndOpcoId).ToList();
            var elementnames = query?.Select(x => x.Elementname)?.Distinct()?.ToList();

            var hardwareconfigurations = _commonManager.GetHardwareConfigurations(elementnames);
            var deploymentStatuses = _repositoryWrapper.DeploymentStatus.FindAll();


            var fntReportData = orderedData.Select(x => new FNTReportDtoGrid
            {
                Hostname = x.Elementname,
                SerialNumberOfHardwareAsset = x.Swresourcekey + "_" + x.Hwresourcekey,
                LocationOfHardWareAsset = x.Opco.Opco,
                HardwareTypeOfHardwareAsset = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Platform.Platform).FirstOrDefault() ?? string.Empty,
                HardwareVendorName = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer).FirstOrDefault() ?? string.Empty,
                SystemNameManagementIpAddress = x.Identitiesasis.Select(x => x.Value).FirstOrDefault(),
                LocalMarketOwnerShip = x.Opco.Opco,
                HardwareEndOfLifeDate  = null,
                VendorHardwareEndOfSupportDate = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Endofsupport.Value.ToString("MM/dd/yyyy")).FirstOrDefault() ?? null,
                HardwareEndOfSaleDate = null,
                HardwareModules = _genericReportGenration.GetHardwareModule(x.Elementname.Trim(), _repositoryWrapper),
                SoftwareProductType = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description ?? string.Empty,
                SoftwareProductVersion = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion,
                CloudHostedAsset = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Buildconstruction.Iscloudasset.Value.ToString()).FirstOrDefault(),
                OperatingSystemName = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Operatingsystem.Operatingsystemname ?? string.Empty,
                ApplicationHostedOnSoftware = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Criticalassettype.Description ?? string.Empty,
                UuidOrSerialnumberOfSoftware = string.Empty,
                SoftwareVendorName = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer ?? string.Empty,
                LocationName = x.Location.Location ?? string.Empty,
                ServiceType = x.Designcomponent.Systemtype.Assetcategory.Assetcategory ?? string.Empty,
                SoftwareEndOfLifeDate = null,
                SoftwareEndOfSupportDate = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance.Value.ToString("MM/dd/yyyy") ?? string.Empty,
                SoftwareEndOfSaleDate = null,
                VerticalEngineeringTeam = string.Join(",", allSubDomain.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.VerticalDic != null && x.Deleted == false)
                                            .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList()),
                VerticalSubDomain = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
                                            .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList()),
                Platform = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Platform.Platform).FirstOrDefault(),
                RiskCluster = LCMEngineeringRulesExtension.GetRiskCluster(x.Designcomponent.Systemtype.VodafonenameNavigation.Id,_repositoryWrapper),


























            }).ToList();

            rtn.Items = fntReportData;

            return rtn;
        }


        private Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]> GetColumnsMapDB()
        {
            return new Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]>
            {
                ["hostname"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["serialNumberOfHardwareAsset"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Swresourcekey+"_"+p.Hwresourcekey},
                ["locationOfHardWareAsset"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Opco.Opco},
                ["hardwareTypeOfHardwareAsset"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Platform.Platform).FirstOrDefault() },
                ["hardwareVendorName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer).FirstOrDefault() },
                ["systemNameManagementIpAddress"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Identitiesasis.Select(x => x.Value).FirstOrDefault() },
                ["localMarketOwnerShip"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Opco.Opco },
                ["vendorHardwareEndOfSupportDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(mjh => mjh.Majorhardware.Endofsupport.Value.ToString("MM/dd/yyyy")).FirstOrDefault() },
                ["hardwareEndOfLifeDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["hardwareEndOfSaleDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["hardwareModules"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["verticalResponsible"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["softwareProductType"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["softwareProductVersion"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["operatingSystemName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["applicationHostedOnServer"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["uuidOrSerialnumberOfSoftware"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["softwareVendorName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["serviceType"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["softwareEndOfLifeDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["softwareEndOfSupportDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["softwareEndOfSaleDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["locationName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["cloudHostedAsset"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["verticalEngineeringTeam"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["verticalSubDomain"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["platform"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["riskCluster"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["operationContactPoint"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["assetCategory"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["assetClass"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["assetType"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["assetDescription"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["productImportance"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["operationsMaintenanceContract"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["identifiedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["descriptionOfPlannedAction"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["plannedHwModel"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["businessServiceNames"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["opsMaintenanceContractEndDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["incidentClass"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["occurrenceProbability"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["organizationResponsible"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["assetStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["typeOfNetworkElement"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["localMarket"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["application"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["cloud"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["physicalServerHostname"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["physicalServerIPAddress"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["physicalServerSerialNumber"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["physicalServerHWModel"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["physicalServerVendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["virtualServerHostedOn"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["virtualServerManufacturer"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["virtualServerTypeOfDevice"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["virtualMachineType"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["virtualServerIPAddress"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["virtualServerSerialNumber"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["virtualServerType"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["osName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["osVersion"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["osStartDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["osInstallationDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["osStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["softwareName"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["version"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["release"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["manufacturer"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},
                ["language"] = new Expression<Func<Networkelementsasplanned, object>>[] { p => p.Elementname},

            };
        }

        #endregion
    }

}
