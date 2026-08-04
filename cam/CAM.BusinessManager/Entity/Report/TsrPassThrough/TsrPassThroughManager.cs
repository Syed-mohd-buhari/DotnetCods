using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.GenericReports;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.Rules;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita;
using CAM.DataTransferObjects.Entita.TsrPassThrough;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Entities.Models.OMC;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Dml;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity
{
    public class TsrPassThroughManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private CommonManager _commonManager;
        private GenericReportGenration _genericReportGenration;
        private readonly ILoggerManager _logger;

        public TsrPassThroughManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager, CommonManager commonManager, ILoggerManager logger,
             GenericReportGenration genericReportGenration, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _commonManager = commonManager;
            _genericReportGenration = genericReportGenration;
            _logger = logger;
        }


        #region //UiMemberFunctions

        public async Task<QueryResultDto<TsrDictionaryGlossaryItemsDto>> GetGlossyTableData()
        {
            var glossaryData = await _repositoryWrapper.GlossaryItemsRepository.FindByCondition(x => x.Istsrfield == true).Select(x => new TsrDictionaryGlossaryItemsDto
            {
                Header = x.Header,
                Description = x.Description
            }).ToListAsync();

            var rtn = new QueryResultDto<TsrDictionaryGlossaryItemsDto>(new GenerateRenderForGrid<TsrDictionaryGlossaryItemsDto>(_manager))
            {

            };
            IEnumerable<TsrDictionaryGlossaryItemsDto> tsrGlossaryDtoGrid = _mapper.Map<IEnumerable<TsrDictionaryGlossaryItemsDto>>(glossaryData);

            rtn.Items = tsrGlossaryDtoGrid.ToArray();
            rtn.TotalItems = rtn.Items.Count;
            return rtn;
        }
        public async Task<QueryResultDto<TsrPassThroughDtoGrid>> FindWithCondition(TsrPassThroughQueryDto tsrPassThroughQueryDto)
        {
            var predicateResult = ApplyFilter(tsrPassThroughQueryDto);
            var rtn = new QueryResultDto<TsrPassThroughDtoGrid>(new GenerateRenderForGrid<TsrPassThroughDtoGrid>(_manager))
            {

            };
            var query = await GetQuery(predicateResult, tsrPassThroughQueryDto.Deleted ?? false, tsrPassThroughQueryDto.RecordClassifier.FirstOrDefault());
            rtn.TotalItems = query.Count();
            query = query.ApplyOrdering(tsrPassThroughQueryDto, GetColumnsMap()).OrderByDescending(x => x.IsSortingAllowed).ApplyPaging(tsrPassThroughQueryDto);
            var data = query.ToList();

            IEnumerable<TsrPassThroughDtoGrid> tsrPassThroughDtoGrid;

            tsrPassThroughDtoGrid = _mapper.Map<IEnumerable<TsrPassThroughDtoGrid>>(data);

            rtn.Items = tsrPassThroughDtoGrid.ToArray();

            return rtn;
        }

        private static ExpressionStarter<Tsrpassthrough> ApplyFilter(TsrPassThroughQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Tsrpassthrough>();
            var predicateInner = PredicateBuilder.New<Tsrpassthrough>();

            if (buildFilterDto.AssetId != null && buildFilterDto.AssetId.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.AssetId)
                    predicateInner.Or(x => x.Assetid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RecordClassifier != null && buildFilterDto.RecordClassifier.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.RecordClassifier)
                    predicateInner.Or(x => x.Recordclassifier == item);
                predicateResult.And(predicateInner);
                if (buildFilterDto.RecordClassifier.Any(x => x == 1))
                {
                    predicateInner.And(x =>
                    x.Deploymentorlifecyclestatus.ToLower().Replace(" ", "") == ConstantValueFilter.InService
                    || x.Deploymentorlifecyclestatus.ToLower().Replace(" ", "") == ConstantValueFilter.inCommisioning.ToLower().Replace(" ", "")
                    || x.Deploymentorlifecyclestatus.ToLower().Replace(" ", "") == ConstantValueFilter.trafficFree.ToLower().Replace(" ", ""));
                    predicateResult.And(predicateInner);
                }
            }
            if (buildFilterDto.VodafoneUniqueIdentifier != null && buildFilterDto.VodafoneUniqueIdentifier.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.VodafoneUniqueIdentifier)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier == item.ToString());
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetName != null && buildFilterDto.AssetName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.AssetName)
                    predicateInner.Or(x => x.Assetname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.AssetDescriptionOrPurpose != null && buildFilterDto.AssetDescriptionOrPurpose.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.AssetDescriptionOrPurpose)
                    predicateInner.Or(x => x.Assetdescriptionorpurpose == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetTypeTsr != null && buildFilterDto.AssetTypeTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.AssetTypeTsr)
                    predicateInner.Or(x => x.Assettype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BusinessOwner != null && buildFilterDto.BusinessOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.BusinessOwner)
                    predicateInner.Or(x => x.Businessowner == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportOwner != null && buildFilterDto.SupportOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SupportOwner)
                    predicateInner.Or(x => x.Supportowner == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportTeam != null && buildFilterDto.SupportTeam.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SupportTeam)
                    predicateInner.Or(x => x.Supportteam == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportTeamsPlaceInTheOrganisation != null && buildFilterDto.SupportTeamsPlaceInTheOrganisation.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SupportTeamsPlaceInTheOrganisation)
                    predicateInner.Or(x => x.Supportteamsplaceintheorganisation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetFunction != null && buildFilterDto.AssetFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.AssetFunction)
                    predicateInner.Or(x => x.Assetfunction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeploymentOrLifeCycleStatus != null && buildFilterDto.DeploymentOrLifeCycleStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.DeploymentOrLifeCycleStatus)
                    predicateInner.Or(x => x.Deploymentorlifecyclestatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RelatedRiskIdsFromRiskRegisters != null && buildFilterDto.RelatedRiskIdsFromRiskRegisters.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.RelatedRiskIdsFromRiskRegisters)
                    predicateInner.Or(x => x.Relatedriskidsfromriskregisters == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RegulatoryScope != null && buildFilterDto.RegulatoryScope.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.RegulatoryScope)
                    predicateInner.Or(x => x.Regulatoryscope == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CountryWhereAssetIsLocated != null && buildFilterDto.CountryWhereAssetIsLocated.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.CountryWhereAssetIsLocated)
                    predicateInner.Or(x => x.Countrywhereassetislocated == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Geolocation != null && buildFilterDto.Geolocation.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.Geolocation)
                    predicateInner.Or(x => x.Geolocation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Infrastructure != null && buildFilterDto.Infrastructure.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.Infrastructure)
                    predicateInner.Or(x => x.Infrastructure == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UpStreamDependencies != null && buildFilterDto.UpStreamDependencies.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.UpStreamDependencies)
                    predicateInner.Or(x => x.Upstreamdependencies == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DownStreamDependencies != null && buildFilterDto.DownStreamDependencies.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.DownStreamDependencies)
                    predicateInner.Or(x => x.Downstreamdependencies == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ChangesToTheassetSinceDeployment != null && buildFilterDto.ChangesToTheassetSinceDeployment.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ChangesToTheassetSinceDeployment)
                    predicateInner.Or(x => x.Changestotheassetsincedeployment == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CloudHostedAsset != null && buildFilterDto.CloudHostedAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.CloudHostedAsset)
                    predicateInner.Or(x => x.Cloudhostedasset == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CloudType != null && buildFilterDto.CloudType.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.CloudType)
                    predicateInner.Or(x => x.Cloudtype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CloudVendor != null && buildFilterDto.CloudVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.CloudVendor)
                    predicateInner.Or(x => x.Cloudvendor == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EquipmentName != null && buildFilterDto.EquipmentName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.EquipmentName)
                    predicateInner.Or(x => x.Equipmentname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HostLocationWithInPhysicalLocation != null && buildFilterDto.HostLocationWithInPhysicalLocation.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.HostLocationWithInPhysicalLocation)
                    predicateInner.Or(x => x.Hostlocationwithinphysicallocation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareVendorName != null && buildFilterDto.SoftwareVendorName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SoftwareVendorName)
                    predicateInner.Or(x => x.Softwarevendorname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Model != null && buildFilterDto.Model.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.Model)
                    predicateInner.Or(x => x.Model == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FirmwareVersion != null && buildFilterDto.FirmwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.FirmwareVersion)
                    predicateInner.Or(x => x.Firmwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FirmwareVersionPatchLevel != null && buildFilterDto.FirmwareVersionPatchLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.FirmwareVersionPatchLevel)
                    predicateInner.Or(x => x.Firmwareversionpatchlevel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MaintenanceSupportSupplier != null && buildFilterDto.MaintenanceSupportSupplier.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.MaintenanceSupportSupplier)
                    predicateInner.Or(x => x.Maintenancesupportsupplier == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorHardwareEndOfSupportDate != null && buildFilterDto.VendorHardwareEndOfSupportDate.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.VendorHardwareEndOfSupportDate)
                    predicateInner.Or(x => x.Vendorhardwareendofsupportdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MaintenanceHardwareEndOfSupportDate != null && buildFilterDto.MaintenanceHardwareEndOfSupportDate.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.MaintenanceHardwareEndOfSupportDate)
                    predicateInner.Or(x => x.Maintenancehardwareendofsupportdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DependantHardware != null && buildFilterDto.DependantHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.DependantHardware)
                    predicateInner.Or(x => x.Dependanthardware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.InstanceType != null && buildFilterDto.InstanceType.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.InstanceType)
                    predicateInner.Or(x => x.Instancetype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OperatingSystemName != null && buildFilterDto.OperatingSystemName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.OperatingSystemName)
                    predicateInner.Or(x => x.Operatingsystemname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OperatingSystemSwvVersion != null && buildFilterDto.OperatingSystemSwvVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.OperatingSystemSwvVersion)
                    predicateInner.Or(x => x.Operatingsystemswvversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OperatingSystemSwVersionPatchLevel != null && buildFilterDto.OperatingSystemSwVersionPatchLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.OperatingSystemSwVersionPatchLevel)
                    predicateInner.Or(x => x.Operatingsystemswversionpatchlevel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameDns != null && buildFilterDto.SystemNameDns.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SystemNameDns)
                    predicateInner.Or(x => x.Systemnamedns == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameManagementIpAddress != null && buildFilterDto.SystemNameManagementIpAddress.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SystemNameManagementIpAddress)
                    predicateInner.Or(x => x.Systemnamemanagementipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameNetBios != null && buildFilterDto.SystemNameNetBios.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SystemNameNetBios)
                    predicateInner.Or(x => x.Systemnamenetbios == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameHostName != null && buildFilterDto.SystemNameHostName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SystemNameHostName)
                    predicateInner.Or(x => x.Systemnamehostname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DateAssetMovedToLiveStatus != null && buildFilterDto.DateAssetMovedToLiveStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.DateAssetMovedToLiveStatus)
                    predicateInner.Or(x => x.Dateassetmovedtolivestatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DateAssetDecommissioned != null && buildFilterDto.DateAssetDecommissioned.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.DateAssetDecommissioned)
                    predicateInner.Or(x => x.Dateassetdecommissioned == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HardwareVendorName != null && buildFilterDto.HardwareVendorName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.HardwareVendorName)
                    predicateInner.Or(x => x.Hardwarevendorname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorSoftwareEndOfSupportDate != null && buildFilterDto.VendorSoftwareEndOfSupportDate.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.VendorSoftwareEndOfSupportDate)
                    predicateInner.Or(x => x.Vendorsoftwareendofsupportdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MaintenanceSoftwareEndOfSupportDate != null && buildFilterDto.MaintenanceSoftwareEndOfSupportDate.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.MaintenanceSoftwareEndOfSupportDate)
                    predicateInner.Or(x => x.Maintenancesoftwareendofsupportdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DependantSystemSoftware != null && buildFilterDto.DependantSystemSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.DependantSystemSoftware)
                    predicateInner.Or(x => x.Dependantsystemsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ResilienceModel != null && buildFilterDto.ResilienceModel.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ResilienceModel)
                    predicateInner.Or(x => x.Resiliencemodel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.GeographicSiteResilience != null && buildFilterDto.GeographicSiteResilience.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.GeographicSiteResilience)
                    predicateInner.Or(x => x.Geographicsiteresilience == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalSiteResilience != null && buildFilterDto.LocalSiteResilience.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.LocalSiteResilience)
                    predicateInner.Or(x => x.Localsiteresilience == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NameOfProductsDependantOnAsset != null && buildFilterDto.NameOfProductsDependantOnAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.NameOfProductsDependantOnAsset)
                    predicateInner.Or(x => x.Nameofproductsdependantonasset == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TechnicalServiceNames != null && buildFilterDto.TechnicalServiceNames.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.TechnicalServiceNames)
                    predicateInner.Or(x => x.Technicalservicenames == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Customer != null && buildFilterDto.Customer.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.Customer)
                    predicateInner.Or(x => x.Customer == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PrivilegedAccessLogging != null && buildFilterDto.PrivilegedAccessLogging.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.PrivilegedAccessLogging)
                    predicateInner.Or(x => x.Privilegedaccesslogging == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BoardOrModuleNameComponentName != null && buildFilterDto.BoardOrModuleNameComponentName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.BoardOrModuleNameComponentName)
                    predicateInner.Or(x => x.Boardormodulenamecomponentname.Contains(item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BoardOrModuleTypeComponentSubtype != null && buildFilterDto.BoardOrModuleTypeComponentSubtype.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.BoardOrModuleTypeComponentSubtype)
                    predicateInner.Or(x => x.Boardormoduletypecomponentsubtype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BoardOrModuleTypeComponentVersionNumber != null && buildFilterDto.BoardOrModuleTypeComponentVersionNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.BoardOrModuleTypeComponentVersionNumber)
                    predicateInner.Or(x => x.Boardormoduletypecomponentversionnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExposedEdge != null && buildFilterDto.ExposedEdge.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ExposedEdge)
                    predicateInner.Or(x => x.Exposededge == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExternallyFacingSystem != null && buildFilterDto.ExternallyFacingSystem.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ExternallyFacingSystem)
                    predicateInner.Or(x => x.Externallyfacingsystem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ManagementPlane != null && buildFilterDto.ManagementPlane.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ManagementPlane)
                    predicateInner.Or(x => x.Managementplane == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NetworkOverSightFunction != null && buildFilterDto.NetworkOverSightFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.NetworkOverSightFunction)
                    predicateInner.Or(x => x.Networkoversightfunction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Pecn != null && buildFilterDto.Pecn.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.Pecn)
                    predicateInner.Or(x => x.Pecn == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Pecs != null && buildFilterDto.Pecs.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.Pecs)
                    predicateInner.Or(x => x.Pecs == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityCriticalFunction != null && buildFilterDto.SecurityCriticalFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SecurityCriticalFunction)
                    predicateInner.Or(x => x.Securitycriticalfunction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductImportanceTsr != null && buildFilterDto.ProductImportanceTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ProductImportanceTsr)
                    predicateInner.Or(x => x.Productimportance == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Critical != null && buildFilterDto.Critical.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.Critical)
                    predicateInner.Or(x => x.Critical == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CriticalityType != null && buildFilterDto.CriticalityType.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.CriticalityType)
                    predicateInner.Or(x => x.Criticalitytype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SerialNumberTsr != null && buildFilterDto.SerialNumberTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.SerialNumberTsr)
                    predicateInner.Or(x => x.Serialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PartNumber != null && buildFilterDto.PartNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.PartNumber)
                    predicateInner.Or(x => x.Partnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DescriptionOfPlannedaction != null && buildFilterDto.DescriptionOfPlannedaction.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.DescriptionOfPlannedaction)
                    predicateInner.Or(x => x.Descriptionofplannedaction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.IdentifiedActionTsr != null && buildFilterDto.IdentifiedActionTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.IdentifiedActionTsr)
                    predicateInner.Or(x => x.Identifiedaction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastUpgradeDateTsr != null && buildFilterDto.LastUpgradeDateTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.LastUpgradeDateTsr)
                    predicateInner.Or(x => x.Lastupgradedate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProdOrLab != null && buildFilterDto.ProdOrLab.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ProdOrLab)
                    predicateInner.Or(x => x.Prodorlab == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalMarketOwnerShip != null && buildFilterDto.LocalMarketOwnerShip.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.LocalMarketOwnerShip)
                    predicateInner.Or(x => x.Localmarketownership == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BudgetEstimatedTsr != null && buildFilterDto.BudgetEstimatedTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.BudgetEstimatedTsr)
                    predicateInner.Or(x => x.Budgetestimated == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BundleBudgetTsr != null && buildFilterDto.BundleBudgetTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.BundleBudgetTsr)
                    predicateInner.Or(x => x.Bundlebudget == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssuranceCall != null && buildFilterDto.AssuranceCall.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.AssuranceCall)
                    predicateInner.Or(x => x.Assurancecall == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CommentOnProjectStatusTsr != null && buildFilterDto.CommentOnProjectStatusTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.CommentOnProjectStatusTsr)
                    predicateInner.Or(x => x.Commentonprojectstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProjectEndDateTsr != null && buildFilterDto.ProjectEndDateTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ProjectEndDateTsr)
                    predicateInner.Or(x => x.Projectenddate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProjectStatusTsr != null && buildFilterDto.ProjectStatusTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ProjectStatusTsr)
                    predicateInner.Or(x => x.Projectstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ServiceLevel != null && buildFilterDto.ServiceLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.ServiceLevel)
                    predicateInner.Or(x => x.Servicelevel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastPenTestDateTsr != null && buildFilterDto.LastPenTestDateTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.LastPenTestDateTsr)
                    predicateInner.Or(x => x.Lastpentestdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastPenTestRefNo != null && buildFilterDto.LastPenTestRefNo.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.LastPenTestRefNo)
                    predicateInner.Or(x => x.Lastpentestrefno == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PiData != null && buildFilterDto.PiData.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.PiData)
                    predicateInner.Or(x => x.Pidata == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EncryptedPiData != null && buildFilterDto.EncryptedPiData.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.EncryptedPiData)
                    predicateInner.Or(x => x.Encryptedpidata == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NonTemsVertical != null && buildFilterDto.NonTemsVertical.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.NonTemsVertical)
                    predicateInner.Or(x => x.Nontemsvertical == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MeProductName != null && buildFilterDto.MeProductName.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.MeProductName)
                    predicateInner.Or(x => x.Productname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MeSoftwareVersion != null && buildFilterDto.MeSoftwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.MeSoftwareVersion)
                    predicateInner.Or(x => x.Softwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MeSubDomainResponsible != null && buildFilterDto.MeSubDomainResponsible.Any())
            {
                predicateInner = PredicateBuilder.New<Tsrpassthrough>();
                foreach (var item in buildFilterDto.MeSubDomainResponsible)
                    predicateInner.Or(x => x.Subdomainresponsible == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;

        }

        private async Task<IQueryable<TsrPassThrough>> GetQuery(ExpressionStarter<Tsrpassthrough> predicateResult, bool includeDeleted, int RecordClassifier)
        {

            var query = predicateResult.IsStarted ? _repositoryWrapper.TsrPassThroughRepository.FindByCondition(predicateResult, includeDeleted)
                        .Include(x => x.CreationuserNavigation)
                        .Include(x => x.ModificationuserNavigation)
                       : _repositoryWrapper.TsrPassThroughRepository.FindAll()
                        .Include(x => x.CreationuserNavigation)
                        .Include(x => x.ModificationuserNavigation);
            return await Task.Run(() => query.AsEnumerable().Select(x => TsrPassThroughMapper.Get(x)).AsQueryable());
        }


        private Dictionary<string, Expression<Func<TsrPassThrough, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<TsrPassThrough, object>>[]>
            {
                ["tsrPassThroughId"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.TsrPassThroughId },
                ["assetId"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.AssetId },
                ["vodafoneUniqueIdentifier"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["assetName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.AssetName },
                ["assetDescriptionOrPurpose"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.AssetDescriptionOrPurpose },
                ["assetTypeTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.AssetType },
                ["businessOwner"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.BusinessOwner },
                ["supportOwner"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SupportOwner },
                ["supportTeam"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SupportTeam },
                ["supportTeamsPlaceInTheOrganisation"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SupportTeamsPlaceInTheOrganisation },
                ["assetFunction"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.AssetFunction },
                ["deploymentOrLifeCycleStatus"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.DeploymentOrLifeCycleStatus },
                ["relatedRiskIdsFromRiskRegisters"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.RelatedRiskIdsFromRiskRegisters },
                ["regulatoryScope"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.RegulatoryScope },
                ["countryWhereAssetIsLocated"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CountryWhereAssetIsLocated },
                ["geolocation"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.Geolocation },
                ["infrastructure"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.Infrastructure },
                ["upStreamDependencies"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.UpStreamDependencies },
                ["downStreamDependencies"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.DownStreamDependencies },
                ["changesToTheassetSinceDeployment"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ChangesToTheassetSinceDeployment },
                ["cloudHostedAsset"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CloudHostedAsset },
                ["cloudType"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CloudType },
                ["cloudVendor"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CloudVendor },
                ["equipmentName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.EquipmentName },
                ["hostLocationWithInPhysicalLocation"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.HostLocationWithInPhysicalLocation },
                ["softwareVendorName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SoftwareVendorName },
                ["model"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.Model },
                ["firmwareVersion"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.FirmwareVersion },
                ["firmwareVersionPatchLevel"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.FirmwareVersionPatchLevel },
                ["maintenanceSupportSupplier"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.MaintenanceSupportSupplier },
                ["vendorHardwareEndOfSupportDate"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.VendorHardwareEndOfSupportDate },
                ["maintenanceHardwareEndOfSupportDate"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.MaintenanceHardwareEndOfSupportDate },
                ["dependantHardware"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.DependantHardware },
                ["instanceType"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.InstanceType },
                ["operatingSystemName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.OperatingSystemName },
                ["operatingSystemSwvVersion"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.OperatingSystemSwvVersion },
                ["operatingSystemSwVersionPatchLevel"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.OperatingSystemSwVersionPatchLevel },
                ["systemNameDns"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SystemNameDns },
                ["systemNameManagementIpAddress"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SystemNameManagementIpAddress },
                ["systemNameNetBios"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SystemNameNetBios },
                ["systemNameHostName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SystemNameHostName },
                ["dateAssetMovedToLiveStatus"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.DateAssetMovedToLiveStatus },
                ["dateAssetDecommissioned"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.DateAssetDecommissioned },
                ["hardwareVendorName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.HardwareVendorName },
                ["maintenanceSupportSupplierSecond"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.MaintenanceSupportSupplierSecond },
                ["vendorSoftwareEndOfSupportDate"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.VendorSoftwareEndOfSupportDate },
                ["maintenanceSoftwareEndOfSupportDate"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.MaintenanceSoftwareEndOfSupportDate },
                ["dependantSystemSoftware"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.DependantSystemSoftware },
                ["resilienceModel"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ResilienceModel },
                ["geographicSiteResilience"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.GeographicSiteResilience },
                ["localSiteResilience"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.LocalSiteResilience },
                ["nameOfProductsDependantOnAsset"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.NameOfProductsDependantOnAsset },
                ["technicalServiceNames"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.TechnicalServiceNames },
                ["customer"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.Customer },
                ["privilegedAccessLogging"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.PrivilegedAccessLogging },
                ["boardOrModuleNameComponentName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.BoardOrModuleNameComponentName },
                ["boardOrModuleTypeComponentSubtype"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.BoardOrModuleTypeComponentSubtype },
                ["boardOrModuleTypeComponentVersionNumber"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.BoardOrModuleTypeComponentVersionNumber },
                ["exposedEdge"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ExposedEdge },
                ["externallyFacingSystem"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ExternallyFacingSystem },
                ["managementPlane"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ManagementPlane },
                ["networkOverSightFunction"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.NetworkOverSightFunction },
                ["pecn"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.Pecn },
                ["pecs"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.Pecs },
                ["securityCriticalFunction"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SecurityCriticalFunction },
                ["productImportanceTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ProductImportance },
                ["critical"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.Critical },
                ["criticalitytype"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CriticalityType },
                ["serialNumberTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SerialNumber },
                ["partNumber"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.PartNumber },
                ["descriptionOfPlannedaction"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.DescriptionOfPlannedaction },
                ["identifiedActionTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.IdentifiedAction },
                ["lastUpgradeDate"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.LastUpgradeDate },
                ["prodOrLab"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ProdOrLab },
                ["localMarketOwnerShip"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.LocalMarketOwnerShip },
                ["budgetEstimatedTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.BudgetEstimated },
                ["bundleBudgetTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.BundleBudget },
                ["assuranceCall"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.AssuranceCall },
                ["commentOnProjectStatusTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CommentOnProjectStatus },
                ["projectEndDateTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ProjectEndDate },
                ["projectStatusTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ProjectStatus },
                ["serviceLevel"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ServiceLevel },
                ["lastPenTestDateTsr"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.LastPenTestDate },
                ["lastPenTestRefNo"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.LastPenTestRefNo },
                ["piData"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.PiData },
                ["encryptedPiData"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.EncryptedPiData },
                ["meProductName"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ProductName },
                ["meSubDomainResponsible"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SubDomainResponsible },
                ["meSoftwareVersion"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.SoftwareVersion },
                ["lastModifiedValue"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<TsrPassThrough, object>>[] { p => p.CreationDate },
            };
        }


        public async Task<List<FilterValueDto>> GetFilter(string propertyName, string propertyFilter, TsrPassThroughQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = await GetQuery(predicateResult, false, buildFilterDto.RecordClassifier.FirstOrDefault());

            var rtn = propertyName switch
            {
                "tsrPassThroughId" => string.IsNullOrEmpty(propertyFilter)
                           ? query.Select(p => new FilterValueDto
                           { Text = p.TsrPassThroughId.ToString(), Value = p.TsrPassThroughId.ToString() }).Distinct().ToList()
                           : query
                                   .Where(x => x.TsrPassThroughId.ToString().Contains(propertyFilter)).Select(p =>
                                   new FilterValueDto { Text = p.TsrPassThroughId.ToString(), Value = p.TsrPassThroughId.ToString() }).Distinct()
                                  .ToList(),
                "assetId" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssetId, Value = p.AssetId }).Distinct().ToList()
                       : query
                               .Where(x => x.AssetId.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssetId, Value = p.AssetId }).Distinct()
                              .ToList(),
                "vodafoneUniqueIdentifier" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VodafoneUniqueIdentifier, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                       : query
                               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VodafoneUniqueIdentifier, Value = p.VodafoneUniqueIdentifier }).Distinct()
                              .ToList(),
                "assetName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssetName, Value = p.AssetName }).Distinct().ToList()
                       : query
                               .Where(x => x.AssetName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssetName, Value = p.AssetName }).Distinct()
                              .ToList(),
                "assetDescriptionOrPurpose" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssetDescriptionOrPurpose, Value = p.AssetDescriptionOrPurpose }).Distinct().ToList()
                       : query
                               .Where(x => x.AssetDescriptionOrPurpose.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssetDescriptionOrPurpose, Value = p.AssetDescriptionOrPurpose }).Distinct()
                              .ToList(),
                "assetTypeTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssetType, Value = p.AssetType }).Distinct().ToList()
                       : query
                               .Where(x => x.AssetType.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssetType, Value = p.AssetType }).Distinct()
                              .ToList(),
                "businessOwner" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.BusinessOwner, Value = p.BusinessOwner }).Distinct().ToList()
                       : query
                               .Where(x => x.BusinessOwner.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.BusinessOwner, Value = p.BusinessOwner }).Distinct()
                              .ToList(),
                "supportOwner" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SupportOwner, Value = p.SupportOwner }).Distinct().ToList()
                       : query
                               .Where(x => x.SupportOwner.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SupportOwner, Value = p.SupportOwner }).Distinct()
                              .ToList(),
                "supportTeam" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SupportTeam, Value = p.SupportTeam }).Distinct().ToList()
                       : query
                               .Where(x => x.SupportTeam.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SupportTeam, Value = p.SupportTeam }).Distinct()
                              .ToList(),
                "supportTeamsPlaceInTheOrganisation" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SupportTeamsPlaceInTheOrganisation, Value = p.SupportTeamsPlaceInTheOrganisation }).Distinct().ToList()
                       : query
                               .Where(x => x.SupportTeamsPlaceInTheOrganisation.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SupportTeamsPlaceInTheOrganisation, Value = p.SupportTeamsPlaceInTheOrganisation }).Distinct()
                              .ToList(),
                "assetFunction" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssetFunction, Value = p.AssetFunction }).Distinct().ToList()
                       : query
                               .Where(x => x.AssetFunction.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssetFunction, Value = p.AssetFunction }).Distinct()
                              .ToList(),
                "deploymentOrLifeCycleStatus" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.DeploymentOrLifeCycleStatus, Value = p.DeploymentOrLifeCycleStatus }).Distinct().ToList()
                       : query
                               .Where(x => x.DeploymentOrLifeCycleStatus.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.DeploymentOrLifeCycleStatus, Value = p.DeploymentOrLifeCycleStatus }).Distinct()
                              .ToList(),
                "relatedRiskIdsFromRiskRegisters" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.RelatedRiskIdsFromRiskRegisters, Value = p.RelatedRiskIdsFromRiskRegisters }).Distinct().ToList()
                       : query
                               .Where(x => x.RelatedRiskIdsFromRiskRegisters.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.RelatedRiskIdsFromRiskRegisters, Value = p.RelatedRiskIdsFromRiskRegisters }).Distinct()
                              .ToList(),
                "regulatoryScope" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.RegulatoryScope, Value = p.RegulatoryScope }).Distinct().ToList()
                       : query
                               .Where(x => x.RegulatoryScope.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.RegulatoryScope, Value = p.RegulatoryScope }).Distinct()
                              .ToList(),
                "countryWhereAssetIsLocated" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.CountryWhereAssetIsLocated, Value = p.CountryWhereAssetIsLocated }).Distinct().ToList()
                       : query
                               .Where(x => x.CountryWhereAssetIsLocated.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.CountryWhereAssetIsLocated, Value = p.CountryWhereAssetIsLocated }).Distinct()
                              .ToList(),
                "geolocation" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Geolocation, Value = p.Geolocation }).Distinct().ToList()
                       : query
                               .Where(x => x.Geolocation.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Geolocation, Value = p.Geolocation }).Distinct()
                              .ToList(),
                "infrastructure" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Infrastructure, Value = p.Infrastructure }).Distinct().ToList()
                       : query
                               .Where(x => x.Infrastructure.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Infrastructure, Value = p.Infrastructure }).Distinct()
                              .ToList(),
                "upStreamDependencies" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.UpStreamDependencies, Value = p.UpStreamDependencies }).Distinct().ToList()
                       : query
                               .Where(x => x.UpStreamDependencies.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.UpStreamDependencies, Value = p.UpStreamDependencies }).Distinct()
                              .ToList(),
                "downStreamDependencies" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.DownStreamDependencies, Value = p.DownStreamDependencies }).Distinct().ToList()
                       : query
                               .Where(x => x.DownStreamDependencies.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.DownStreamDependencies, Value = p.DownStreamDependencies }).Distinct()
                              .ToList(),
                "changesToTheassetSinceDeployment" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ChangesToTheassetSinceDeployment, Value = p.ChangesToTheassetSinceDeployment }).Distinct().ToList()
                       : query
                               .Where(x => x.ChangesToTheassetSinceDeployment.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ChangesToTheassetSinceDeployment, Value = p.ChangesToTheassetSinceDeployment }).Distinct()
                              .ToList(),
                "cloudHostedAsset" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.CloudHostedAsset, Value = p.CloudHostedAsset }).Distinct().ToList()
                       : query
                               .Where(x => x.CloudHostedAsset.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.CloudHostedAsset, Value = p.CloudHostedAsset }).Distinct()
                              .ToList(),
                "cloudType" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.CloudType, Value = p.CloudType }).Distinct().ToList()
                       : query
                               .Where(x => x.CloudType.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.CloudType, Value = p.CloudType }).Distinct()
                              .ToList(),
                "cloudVendor" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.CloudVendor, Value = p.CloudVendor }).Distinct().ToList()
                       : query
                               .Where(x => x.CloudVendor.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.CloudVendor, Value = p.CloudVendor }).Distinct()
                              .ToList(),
                "equipmentName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.EquipmentName, Value = p.EquipmentName }).Distinct().ToList()
                       : query
                               .Where(x => x.EquipmentName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.EquipmentName, Value = p.EquipmentName }).Distinct()
                              .ToList(),
                "hostLocationWithInPhysicalLocation" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.HostLocationWithInPhysicalLocation, Value = p.HostLocationWithInPhysicalLocation }).Distinct().ToList()
                       : query
                               .Where(x => x.HostLocationWithInPhysicalLocation.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.HostLocationWithInPhysicalLocation, Value = p.HostLocationWithInPhysicalLocation }).Distinct()
                              .ToList(),
                "softwareVendorName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SoftwareVendorName, Value = p.SoftwareVendorName }).Distinct().ToList()
                       : query
                               .Where(x => x.SoftwareVendorName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SoftwareVendorName, Value = p.SoftwareVendorName }).Distinct()
                              .ToList(),
                "model" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Model, Value = p.Model }).Distinct().ToList()
                       : query
                               .Where(x => x.Model.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Model, Value = p.Model }).Distinct()
                              .ToList(),
                "firmwareVersion" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.FirmwareVersion, Value = p.FirmwareVersion }).Distinct().ToList()
                       : query
                               .Where(x => x.FirmwareVersion.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.FirmwareVersion, Value = p.FirmwareVersion }).Distinct()
                              .ToList(),
                "firmwareVersionPatchLevel" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.FirmwareVersionPatchLevel, Value = p.FirmwareVersionPatchLevel }).Distinct().ToList()
                       : query
                               .Where(x => x.FirmwareVersionPatchLevel.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.FirmwareVersionPatchLevel, Value = p.FirmwareVersionPatchLevel }).Distinct()
                              .ToList(),
                "maintenanceSupportSupplier" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.MaintenanceSupportSupplier, Value = p.MaintenanceSupportSupplier }).Distinct().ToList()
                       : query
                               .Where(x => x.MaintenanceSupportSupplier.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.MaintenanceSupportSupplier, Value = p.MaintenanceSupportSupplier }).Distinct()
                              .ToList(),
                "vendorHardwareEndOfSupportDate" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VendorHardwareEndOfSupportDate, Value = p.VendorHardwareEndOfSupportDate }).Distinct().ToList()
                       : query
                               .Where(x => x.VendorHardwareEndOfSupportDate.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VendorHardwareEndOfSupportDate, Value = p.VendorHardwareEndOfSupportDate }).Distinct()
                              .ToList(),

                "maintenanceHardwareEndOfSupportDate" => string.IsNullOrEmpty(propertyFilter)
         ? query.Select(p => new FilterValueDto
         { Text = p.MaintenanceHardwareEndOfSupportDate, Value = p.MaintenanceHardwareEndOfSupportDate }).Distinct().ToList()
         : query
                 .Where(x => x.MaintenanceHardwareEndOfSupportDate.Contains(propertyFilter)).Select(p =>
                 new FilterValueDto { Text = p.MaintenanceHardwareEndOfSupportDate, Value = p.MaintenanceHardwareEndOfSupportDate }).Distinct()
                .ToList(),
                "dependantHardware" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.DependantHardware, Value = p.DependantHardware }).Distinct().ToList()
                       : query
                               .Where(x => x.DependantHardware.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.DependantHardware, Value = p.DependantHardware }).Distinct()
                              .ToList(),
                "instanceType" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.InstanceType, Value = p.InstanceType }).Distinct().ToList()
                       : query
                               .Where(x => x.InstanceType.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.InstanceType, Value = p.InstanceType }).Distinct()
                              .ToList(),
                "operatingSystemName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.OperatingSystemName, Value = p.OperatingSystemName }).Distinct().ToList()
                       : query
                               .Where(x => x.OperatingSystemName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.OperatingSystemName, Value = p.OperatingSystemName }).Distinct()
                              .ToList(),
                "operatingSystemSwvVersion" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.OperatingSystemSwvVersion, Value = p.OperatingSystemSwvVersion }).Distinct().ToList()
                       : query
                               .Where(x => x.OperatingSystemSwvVersion.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.OperatingSystemSwvVersion, Value = p.OperatingSystemSwvVersion }).Distinct()
                              .ToList(),
                "operatingSystemSwVersionPatchLevel" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.OperatingSystemSwVersionPatchLevel, Value = p.OperatingSystemSwVersionPatchLevel }).Distinct().ToList()
                       : query
                               .Where(x => x.OperatingSystemSwVersionPatchLevel.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.OperatingSystemSwVersionPatchLevel, Value = p.OperatingSystemSwVersionPatchLevel }).Distinct()
                              .ToList(),
                "systemNameDns" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SystemNameDns, Value = p.SystemNameDns }).Distinct().ToList()
                       : query
                               .Where(x => x.SystemNameDns.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SystemNameDns, Value = p.SystemNameDns }).Distinct()
                              .ToList(),
                "systemNameManagementIpAddress" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SystemNameManagementIpAddress, Value = p.SystemNameManagementIpAddress }).Distinct().ToList()
                       : query
                               .Where(x => x.SystemNameManagementIpAddress.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SystemNameManagementIpAddress, Value = p.SystemNameManagementIpAddress }).Distinct()
                              .ToList(),
                "systemNameNetBios" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SystemNameNetBios, Value = p.SystemNameNetBios }).Distinct().ToList()
                       : query
                               .Where(x => x.SystemNameNetBios.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SystemNameNetBios, Value = p.SystemNameNetBios }).Distinct()
                              .ToList(),
                "systemNameHostName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SystemNameHostName, Value = p.SystemNameHostName }).Distinct().ToList()
                       : query
                               .Where(x => x.SystemNameHostName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SystemNameHostName, Value = p.SystemNameHostName }).Distinct()
                              .ToList(),
                "dateAssetMovedToLiveStatus" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.DateAssetMovedToLiveStatus, Value = p.DateAssetMovedToLiveStatus }).Distinct().ToList()
                       : query
                               .Where(x => x.DateAssetMovedToLiveStatus.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.DateAssetMovedToLiveStatus, Value = p.DateAssetMovedToLiveStatus }).Distinct()
                              .ToList(),
                "dateAssetDecommissioned" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.DateAssetDecommissioned, Value = p.DateAssetDecommissioned }).Distinct().ToList()
                       : query
                               .Where(x => x.DateAssetDecommissioned.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.DateAssetDecommissioned, Value = p.DateAssetDecommissioned }).Distinct()
                              .ToList(),
                "hardwareVendorName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.HardwareVendorName, Value = p.HardwareVendorName }).Distinct().ToList()
                       : query
                               .Where(x => x.HardwareVendorName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.HardwareVendorName, Value = p.HardwareVendorName }).Distinct()
                              .ToList(),
                "vendorSoftwareEndOfSupportDate" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VendorSoftwareEndOfSupportDate, Value = p.VendorSoftwareEndOfSupportDate }).Distinct().ToList()
                       : query
                               .Where(x => x.VendorSoftwareEndOfSupportDate.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VendorSoftwareEndOfSupportDate, Value = p.VendorSoftwareEndOfSupportDate }).Distinct()
                              .ToList(),
                "maintenanceSoftwareEndOfSupportDate" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.MaintenanceSoftwareEndOfSupportDate, Value = p.MaintenanceSoftwareEndOfSupportDate }).Distinct().ToList()
                       : query
                               .Where(x => x.MaintenanceSoftwareEndOfSupportDate.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.MaintenanceSoftwareEndOfSupportDate, Value = p.MaintenanceSoftwareEndOfSupportDate }).Distinct()
                              .ToList(),
                "dependantSystemSoftware" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.DependantSystemSoftware, Value = p.DependantSystemSoftware }).Distinct().ToList()
                       : query
                               .Where(x => x.DependantSystemSoftware.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.DependantSystemSoftware, Value = p.DependantSystemSoftware }).Distinct()
                              .ToList(),
                "resilienceModel" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ResilienceModel, Value = p.ResilienceModel }).Distinct().ToList()
                       : query
                               .Where(x => x.ResilienceModel.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResilienceModel, Value = p.ResilienceModel }).Distinct()
                              .ToList(),
                "geographicSiteResilience" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.GeographicSiteResilience, Value = p.GeographicSiteResilience }).Distinct().ToList()
                       : query
                               .Where(x => x.GeographicSiteResilience.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.GeographicSiteResilience, Value = p.GeographicSiteResilience }).Distinct()
                              .ToList(),
                "localSiteResilience" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.LocalSiteResilience, Value = p.LocalSiteResilience }).Distinct().ToList()
                       : query
                               .Where(x => x.LocalSiteResilience.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.LocalSiteResilience, Value = p.LocalSiteResilience }).Distinct()
                              .ToList(),
                "nameOfProductsDependantOnAsset" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.NameOfProductsDependantOnAsset, Value = p.NameOfProductsDependantOnAsset }).Distinct().ToList()
                       : query
                               .Where(x => x.NameOfProductsDependantOnAsset.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.NameOfProductsDependantOnAsset, Value = p.NameOfProductsDependantOnAsset }).Distinct()
                              .ToList(),
                "technicalServiceNames" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.TechnicalServiceNames, Value = p.TechnicalServiceNames }).Distinct().ToList()
                       : query
                               .Where(x => x.TechnicalServiceNames.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.TechnicalServiceNames, Value = p.TechnicalServiceNames }).Distinct()
                              .ToList(),
                "customer" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Customer, Value = p.Customer }).Distinct().ToList()
                       : query
                               .Where(x => x.Customer.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Customer, Value = p.Customer }).Distinct()
                              .ToList(),
                "privilegedAccessLogging" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PrivilegedAccessLogging, Value = p.PrivilegedAccessLogging }).Distinct().ToList()
                       : query
                               .Where(x => x.PrivilegedAccessLogging.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PrivilegedAccessLogging, Value = p.PrivilegedAccessLogging }).Distinct()
                              .ToList(),
                "boardOrModuleNameComponentName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.BoardOrModuleNameComponentName, Value = p.BoardOrModuleNameComponentName }).Distinct().ToList()
                       : query
                               .Where(x => x.BoardOrModuleNameComponentName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.BoardOrModuleNameComponentName, Value = p.BoardOrModuleNameComponentName }).Distinct()
                              .ToList(),
                "boardOrModuleTypeComponentSubtype" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.BoardOrModuleTypeComponentSubtype, Value = p.BoardOrModuleTypeComponentSubtype }).Distinct().ToList()
                       : query
                               .Where(x => x.BoardOrModuleTypeComponentSubtype.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.BoardOrModuleTypeComponentSubtype, Value = p.BoardOrModuleTypeComponentSubtype }).Distinct()
                              .ToList(),
                "boardOrModuleTypeComponentVersionNumber" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.BoardOrModuleTypeComponentVersionNumber, Value = p.BoardOrModuleTypeComponentVersionNumber }).Distinct().ToList()
                       : query
                               .Where(x => x.BoardOrModuleTypeComponentVersionNumber.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.BoardOrModuleTypeComponentVersionNumber, Value = p.BoardOrModuleTypeComponentVersionNumber }).Distinct()
                              .ToList(),
                "exposedEdge" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ExposedEdge, Value = p.ExposedEdge }).Distinct().ToList()
                       : query
                               .Where(x => x.ExposedEdge.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ExposedEdge, Value = p.ExposedEdge }).Distinct()
                              .ToList(),
                "externallyFacingSystem" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ExternallyFacingSystem, Value = p.ExternallyFacingSystem }).Distinct().ToList()
                       : query
                               .Where(x => x.ExternallyFacingSystem.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ExternallyFacingSystem, Value = p.ExternallyFacingSystem }).Distinct()
                              .ToList(),
                "managementPlane" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ManagementPlane, Value = p.ManagementPlane }).Distinct().ToList()
                       : query
                               .Where(x => x.ManagementPlane.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ManagementPlane, Value = p.ManagementPlane }).Distinct()
                              .ToList(),
                "networkOverSightFunction" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.NetworkOverSightFunction, Value = p.NetworkOverSightFunction }).Distinct().ToList()
                       : query
                               .Where(x => x.NetworkOverSightFunction.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.NetworkOverSightFunction, Value = p.NetworkOverSightFunction }).Distinct()
                              .ToList(),
                "pecn" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Pecn, Value = p.Pecn }).Distinct().ToList()
                       : query
                               .Where(x => x.Pecn.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Pecn, Value = p.Pecn }).Distinct()
                              .ToList(),
                "pecs" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Pecs, Value = p.Pecs }).Distinct().ToList()
                       : query
                               .Where(x => x.Pecs.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Pecs, Value = p.Pecs }).Distinct()
                              .ToList(),
                "securityCriticalFunction" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SecurityCriticalFunction, Value = p.SecurityCriticalFunction }).Distinct().ToList()
                       : query
                               .Where(x => x.SecurityCriticalFunction.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SecurityCriticalFunction, Value = p.SecurityCriticalFunction }).Distinct()
                              .ToList(),
                "productImportanceTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ProductImportance, Value = p.ProductImportance }).Distinct().ToList()
                       : query
                               .Where(x => x.ProductImportance.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ProductImportance, Value = p.ProductImportance }).Distinct()
                              .ToList(),
                "critical" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Critical, Value = p.Critical }).Distinct().ToList()
                       : query
                               .Where(x => x.Critical.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Critical, Value = p.Critical }).Distinct()
                              .ToList(),
                "criticalityType" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.CriticalityType, Value = p.CriticalityType }).Distinct().ToList()
                       : query
                               .Where(x => x.CriticalityType.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.CriticalityType, Value = p.CriticalityType }).Distinct()
                              .ToList(),
                "serialNumberTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SerialNumber, Value = p.SerialNumber }).Distinct().ToList()
                       : query
                               .Where(x => x.SerialNumber.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SerialNumber, Value = p.SerialNumber }).Distinct()
                              .ToList(),
                "partNumber" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PartNumber, Value = p.PartNumber }).Distinct().ToList()
                       : query
                               .Where(x => x.PartNumber.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PartNumber, Value = p.PartNumber }).Distinct()
                              .ToList(),
                "descriptionOfPlannedaction" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.DescriptionOfPlannedaction, Value = p.DescriptionOfPlannedaction }).Distinct().ToList()
                       : query
                               .Where(x => x.DescriptionOfPlannedaction.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.DescriptionOfPlannedaction, Value = p.DescriptionOfPlannedaction }).Distinct()
                              .ToList(),
                "identifiedActionTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.IdentifiedAction, Value = p.IdentifiedAction }).Distinct().ToList()
                       : query
                               .Where(x => x.IdentifiedAction.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.IdentifiedAction, Value = p.IdentifiedAction }).Distinct()
                              .ToList(),
                "lastUpgradeDateTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.LastUpgradeDate, Value = p.LastUpgradeDate }).Distinct().ToList()
                       : query
                               .Where(x => x.LastUpgradeDate.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.LastUpgradeDate, Value = p.LastUpgradeDate }).Distinct()
                              .ToList(),
                "prodOrLab" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ProdOrLab, Value = p.ProdOrLab }).Distinct().ToList()
                       : query
                               .Where(x => x.ProdOrLab.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ProdOrLab, Value = p.ProdOrLab }).Distinct()
                              .ToList(),
                "localMarketOwnerShip" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.LocalMarketOwnerShip, Value = p.LocalMarketOwnerShip }).Distinct().ToList()
                       : query
                               .Where(x => x.LocalMarketOwnerShip.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.LocalMarketOwnerShip, Value = p.LocalMarketOwnerShip }).Distinct()
                              .ToList(),
                "budgetEstimatedTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.BudgetEstimated, Value = p.BudgetEstimated }).Distinct().ToList()
                       : query
                               .Where(x => x.BudgetEstimated.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.BudgetEstimated, Value = p.BudgetEstimated }).Distinct()
                              .ToList(),
                "bundleBudgetTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.BundleBudget, Value = p.BundleBudget }).Distinct().ToList()
                       : query
                               .Where(x => x.BundleBudget.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.BundleBudget, Value = p.BundleBudget }).Distinct()
                              .ToList(),
                "assuranceCall" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssuranceCall, Value = p.AssuranceCall }).Distinct().ToList()
                       : query
                               .Where(x => x.AssuranceCall.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssuranceCall, Value = p.AssuranceCall }).Distinct()
                              .ToList(),
                "commentOnProjectStatusTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.CommentOnProjectStatus, Value = p.CommentOnProjectStatus }).Distinct().ToList()
                       : query
                               .Where(x => x.CommentOnProjectStatus.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.CommentOnProjectStatus, Value = p.CommentOnProjectStatus }).Distinct()
                              .ToList(),
                "projectEndDateTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ProjectEndDate, Value = p.ProjectEndDate }).Distinct().ToList()
                       : query
                               .Where(x => x.ProjectEndDate.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ProjectEndDate, Value = p.ProjectEndDate }).Distinct()
                              .ToList(),
                "projectStatusTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ProjectStatus, Value = p.ProjectStatus }).Distinct().ToList()
                       : query
                               .Where(x => x.ProjectStatus.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ProjectStatus, Value = p.ProjectStatus }).Distinct()
                              .ToList(),
                "serviceLevel" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ServiceLevel, Value = p.ServiceLevel }).Distinct().ToList()
                       : query
                               .Where(x => x.ServiceLevel.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ServiceLevel, Value = p.ServiceLevel }).Distinct()
                              .ToList(),
                "lastPenTestDateTsr" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.LastPenTestDate, Value = p.LastPenTestDate }).Distinct().ToList()
                       : query
                               .Where(x => x.LastPenTestDate.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.LastPenTestDate, Value = p.LastPenTestDate }).Distinct()
                              .ToList(),
                "lastPenTestRefNo" => string.IsNullOrEmpty(propertyFilter)
                                                           ? query.Select(p => new FilterValueDto
                                                           { Text = p.LastPenTestRefNo, Value = p.LastPenTestRefNo }).Distinct().ToList()
                                                           : query
                                                                   .Where(x => x.LastPenTestRefNo.Contains(propertyFilter)).Select(p =>
                                                                   new FilterValueDto { Text = p.LastPenTestRefNo, Value = p.LastPenTestRefNo }).Distinct()
                                                                  .ToList(),
                "piData" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PiData, Value = p.PiData }).Distinct().ToList()
                       : query.Where(x => x.PiData.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PiData, Value = p.PiData }).Distinct()
                              .ToList(),
                "encryptedPiData" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.EncryptedPiData, Value = p.EncryptedPiData }).Distinct().ToList()
                       : query
                               .Where(x => x.EncryptedPiData.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.EncryptedPiData, Value = p.EncryptedPiData }).Distinct()
                              .ToList(),

                "lastModifiedBy" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList()
                    : query
                        .Where(x =>
                            x.ModificationUserEntity.Email.ToString().Contains(
                                propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationUserEntity.Email.ToString(), Value = p.ModificationUserEntity.Email.ToString() }).Distinct().ToList(),


                "lastModified" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList()
                : query
                    .Where(x =>
                        x.ModificationDate.ToString().Contains(
                            propertyFilter)).Select(p => new FilterValueDto { Text = p.ModificationDate.ToString(), Value = p.ModificationDate.ToString() }).Distinct().ToList(),

                "meProductName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ProductName, Value = p.ProductName }).Distinct().ToList()
                       : query
                               .Where(x => x.ProductName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ProductName, Value = p.ProductName }).Distinct()
                              .ToList(),
                "meSubDomainResponsible" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SubDomainResponsible, Value = p.SubDomainResponsible }).Distinct().ToList()
                       : query
                               .Where(x => x.SubDomainResponsible.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SubDomainResponsible, Value = p.SubDomainResponsible }).Distinct()
                              .ToList(),
                "meSoftwareVersion" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SoftwareVersion, Value = p.SoftwareVersion }).Distinct().ToList()
                       : query
                               .Where(x => x.SoftwareVersion.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SoftwareVersion, Value = p.SoftwareVersion }).Distinct()
                              .ToList(),
                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion

        #region // Data load

        public async Task<ResultDto> TSRDataRefresh(TsrPassThroughQueryDto dto)
        {
            var result = await Task.Run(() => TSRCreateAndUpdate(dto));
            //var groupResult = await  Task.Run(()=>TSRCreateAndUpdate(dto));

            return new ResultDto
            {
                Warning = result.Warning ? true : false,
                Info = result.Warning ? "Something went wrong while loading data to the TSR report" : "TSR Data inserted and updated successfully",
                Data = result.Warning ? result.Data : "",
            };
        }
        private string getDCFName(Networkelementsasplanned x)
        {
            var dcfName = x.Designcomponent.toDesignComponentFamily();
            //dcfName = dcfName.Replace("<b class=\"text-lowercase\">", "");
            //dcfName = dcfName.Replace("<b class=\"text-lowercase\" >", "");
            //dcfName = dcfName.Replace("</b>", "");

            return dcfName;
        }
        public async Task<ResultDto> TSRCreateAndUpdate(TsrPassThroughQueryDto dto)
        {
            var currentTime = DateTime.Now;
            try
            {


                var createTsrList = new List<Tsrpassthrough>();
                var updateTsrList = new List<Tsrpassthrough>();
                var assetOmcDetailsForcluster = new List<AssetOmcIntegretion>();

                var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);
                var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);
                if (dto.CountryWhereAssetIsLocated != null && dto.CountryWhereAssetIsLocated.Any())
                {
                    foreach (var item in dto.CountryWhereAssetIsLocated)
                    {
                        predicateResult.Or(x => x.Opco.Opco.ToLower() == item.ToLower());
                    }
                }
                else
                {
                    predicateResult.Or(x => x.Opco.Opco.ToLower() == "uk" || x.Opco.Opco.ToLower() == "group");
                }
                var query = await Task.Run(() => GetDisaggregatedQuery(predicateResult));

                var location = _repositoryWrapper.Location.FindByCondition(x => query.AsEnumerable().Select(y => y.Locationid).ToList().Contains(x.Locationid)).Include(x => x.Sites).ToList();
                var dcfLifeCycle = _repositoryWrapper.DesignComponentFamilyLifeCycleRepository.FindAll();

                var AssetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                    .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

                var hardWare = query.Select(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware)).ToList();

                var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(AssetIdAndOpcoId).ToList();
                var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(AssetIdAndOpcoId).ToList();
                var elementnames = query?.Select(x => x.Elementname)?.Distinct()?.ToList();

                var hardwareconfigurations = _commonManager.GetHardwareConfigurations(elementnames);
                var deploymentStatuses = _repositoryWrapper.DeploymentStatus.FindAll();
                var designAspects = _repositoryWrapper.DesignAspectRepository.FindAll().Include(x => x.Siteresilience).Include(x => x.Businesscontinuitymethod);

                var assetOmcDetails = await AssetOmcIntegrationFields(query.Select(x => x.Elementname.ToLower().Trim()).ToList());

                if(query.Any(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(x => x.Majorhardware.Platform.Platform == ConstantValueFilter.CCDCNIS)))
                {
                    assetOmcDetailsForcluster = assetOmcDetails;
                }

                var tsrPassThroughDatas = query.ToList().Select(x => new Tsrpassthrough
                {

                    //var tsrGrid = new Tsrpassthrough();
                    #region // mapping
                    Assetid = "TEMS" + x.Networkelementasplannedid.ToString("000000"),
                    Recordclassifier = 1,
                    Vodafoneuniqueidentifier = x.Swresourcekey + "_" + x.Hwresourcekey,
                    Assetname = x.Elementname,
                    Assetdescriptionorpurpose = x.Designcomponent?.Designcomponentfamily?.Description,
                    Assettype = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Criticalassettype?.Description,
                    Businessowner = string.Join(",", allEdu?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && x.Deleted == false).Select(t => t?.ContactEmail).ToList()),
                    Assetfunction = x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory,

                    Deploymentorlifecyclestatus = deploymentStatuses.Where(y => y.Deploymentstatusid == x.Deploymentstatusid).Select(x => x.Deploymentstatus).FirstOrDefault(),
                    //Relatedriskidsfromriskregisters = x.Lcmengineering?.Lcmancillarydata?
                    //             .Where(x => x.Lcmengineeringid == x.Lcmengineering?.Lcmengineeringid).FirstOrDefault()?.Raid,
                    Regulatoryscope = (x.Designcomponent?.Subnetworkboundary?.Gdprrelevant == true && x.Opco?.Opco.ToLower() == "uk") ? "TSA;GDPR" : "TSA",
                    Countrywhereassetislocated = x.Lcmengineering?.Opco?.Opco,
                    Geolocation = x.Lcmengineering?.Lcmancillarydata.Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid).FirstOrDefault()?.Isexposededge == true
                                  ? location.Where(y => y.Locationid == x.Locationid).Select(s => s.Sites?.FirstOrDefault()).Select(x => x?.Sitecode).FirstOrDefault() != null
                                  ? location.Where(y => y.Locationid == x.Locationid).FirstOrDefault()?.Sites?.FirstOrDefault()?.Sitecode
                                  : location.Where(y => y.Locationid == x.Locationid).Select(s => s.Location).FirstOrDefault()
                                  : location.Where(y => y.Locationid == x.Locationid).Select(s => s.Location).FirstOrDefault(),

                    Infrastructure = Convert.ToString(x.Lcmengineering?.Lcmancillarydata.Where(x => x.Lcmengineeringid == x.Lcmengineering?.Lcmengineeringid).FirstOrDefault()?.Locationinfrastructure)?.Replace(" ", "")?.ToLower() == "exposededge" ? string.Empty :
                                        x.Lcmengineering?.Lcmancillarydata.Where(x => x.Lcmengineeringid == x.Lcmengineering?.Lcmengineeringid).FirstOrDefault()?.Locationinfrastructure,

                    Cloudhostedasset = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware?.Buildconstruction?.Iscloudasset).FirstOrDefault() == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                    Cloudtype = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware?.Buildconstruction?.Cloudtype).FirstOrDefault(),//no
                    //Cloudvendor = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer).FirstOrDefault(),//no

                    Cloudvendor = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true
                    &&
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure
                    && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ?
                    assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.CloudVendor).FirstOrDefault() :
                    x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer).FirstOrDefault(),

                    Equipmentname = x.Elementname,
                    //Hostlocationwithinphysicallocation = string.Empty,//no
                    Softwarevendorname = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,

                    //Model = x.Designcomponent?.Systemtype?.toLcmDbExportHardwareName(),

                    Model = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true &&
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure &&
                    mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ? assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.Model).FirstOrDefault() :
                    x.Designcomponent?.Systemtype?.toLcmDbExportHardwareName(),

                    //Firmwareversion = string.Join(";", hardwareconfigurations.Where(hw => hw.ElementName == x.Elementname).Select(x => x.ProductName + " - " + x.Revision).ToList()),

                    Firmwareversion = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ? assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.FirmwareVersion).FirstOrDefault() :
                    string.Join(";", hardwareconfigurations.Where(hw => hw.ElementName == x.Elementname).Select(x => x.ProductName + " - " + x.Revision).ToList()),


                    //Maintenancesupportsupplier = x.Lcmengineering.Softwaresupported?.Description.Replace(" ", "").ToLower() == "thirdparty"
                    //                                    ? x.Lcmengineering.Softwaresupportprovider : x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer,

                    Vendorhardwareendofsupportdate = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(x => x.Majorhardware.Endofsupport).FirstOrDefault()?.ToString(ConstantValueFilter.DateFormat) ?? ConstantValueFilter.NotSpecified,
                    //    Enum.EOMEnum.Default ?
                    //(src.EndOfMaintenance.HasValue ? src.EndOfMaintenance.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "")
                    //: (src.EOMStatus == Enum.EOMEnum.NotAnnounced ? "Not Announced" : "Not Specified")
                    //Maintenancehardwareendofsupportdate = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?
                    //                                        .Select(x => x.Majorhardware.Endofsupport)
                    //                                        .FirstOrDefault()?
                    //                                        .ToString(ConstantValueFilter.DateFormat) ?? string.Empty,
                    Instancetype = _commonManager.GetAssetType(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware?.Buildconstruction?.Buildconstruction).FirstOrDefault()),

                    Dependanthardware = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true &&
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null
                    ? assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.DependentHW).FirstOrDefault() : string.Empty,//no

                    Operatingsystemname = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Operatingsystem?.Operatingsystemname,//no
                    Operatingsystemswvversion = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Operatingsystem?.Operatingsystemversion,
                    //Operatingsystemswversionpatchlevel = string.Empty,//no


                    //Systemnamedns = x.Elementdomianname,
                    Systemnamedns = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true &&
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure
                    && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ? assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.SystemName).FirstOrDefault() :
                    assetOmcDetailsForcluster.Count > 0 && assetOmcDetailsForcluster.Any(f => f.AssetName == x.Elementname) == true &&
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => /*mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure &&*/
                    mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CCDCNIS).FirstOrDefault() != null ?
                    assetOmcDetailsForcluster.Where(f => f.AssetName == x.Elementname).Select(r => r.SystemNameForCluster).FirstOrDefault() :
                    x.Elementdomianname,

                    //Systemnamemanagementipaddress = x.Identitiesasis.Select(x => x.Value).FirstOrDefault(),
                    Systemnamemanagementipaddress = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true &&
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure &&
                    mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ? assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.ManagementIpAddress).FirstOrDefault() :
                    assetOmcDetailsForcluster.Count > 0 && assetOmcDetailsForcluster.Any(f => f.AssetName == x.Elementname) == true &&
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => /*mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure &&*/
                    mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CCDCNIS).FirstOrDefault() != null ?
                    assetOmcDetailsForcluster.Where(f => f.AssetName == x.Elementname).Select(r => r.ManagementIpAddressForCluster).FirstOrDefault() :
                    x.Identitiesasis.Where(p => p.Interfacetype == "Management" && p.Category?.Description.ToLower().Replace(" ", "") == "IP Address".ToLower().Replace(" ", "")).Select(ne => ne.Value).FirstOrDefault(),
                    //x.Identitiesasis.Select(x => x.Value).FirstOrDefault(),


                    Systemnamenetbios = x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory.ToLower().Replace(" ", "") == ConstantValueFilter.NetworkElement
                    || x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory.ToLower().Replace(" ", "") == ConstantValueFilter.ApplicationOrPlatform ? ConstantValueFilter.NA : string.Empty, //no
                    Systemnamehostname = x.Elementname,
                    Dateassetmovedtolivestatus = x.Assetlivestatusdate?.ToString(ConstantValueFilter.DateFormat),
                    Dateassetdecommissioned = x.Assetdecommissioneddate?.ToString(ConstantValueFilter.DateFormat),

                    //Hardwarevendorname = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer).FirstOrDefault(),
                    Hardwarevendorname = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ? assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.HwManufacturer).FirstOrDefault() :
                    x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer).FirstOrDefault(),



                    Vendorsoftwareendofsupportdate = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds.Eomstatus == (short)Enum.EOMEnum.Default ?
                                                                                           x.Designcomponent?.Systemtype?.Majorsoftwarebuilds.Endofsupport?.ToString(ConstantValueFilter.DateFormat)
                                                                                           : x.Designcomponent?.Systemtype?.Majorsoftwarebuilds.Eomstatus == (short)Enum.EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : ConstantValueFilter.NotSpecified,
                    //Maintenancesoftwareendofsupportdate = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds.Endofsupport?.ToString(ConstantValueFilter.DateFormat) ?? string.Empty,
                    Dependantsystemsoftware = _commonManager.GetDependentSoftware(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware?.Buildconstruction?.Buildconstruction).FirstOrDefault(),
                                               x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Operatingsystem?.Operatingsystemname),
                    Resiliencemodel = designAspects.Where(da => da.Designcomponentfamilyid == x.Designcomponent.Designcomponentfamilyid && da.Opcoid == x.Opcoid).Select(x => x.Siteresilience.Description).FirstOrDefault(),
                    Geographicsiteresilience = designAspects.Where(da => da.Designcomponentfamilyid == x.Designcomponent.Designcomponentfamilyid && da.Opcoid == x.Opcoid).Select(x => x.Businesscontinuitymethod.Description).FirstOrDefault(),
                    Localsiteresilience = designAspects.Where(da => da.Designcomponentfamilyid == x.Designcomponent.Designcomponentfamilyid && da.Opcoid == x.Opcoid).Select(x => x.Businesscontinuitymethod.Description).FirstOrDefault(),
                    //Nameofproductsdependantonasset = string.Empty, // no
                    //Customer = string.Empty, //nedd clf
                    //Privilegedaccesslogging = string.Empty, // need clf


                    Technicalservicenames = x.Designcomponent.toDesignComponentFamily(),//getDCFName(x),

                    //Boardormodulenamecomponentname = _genericReportGenration.GetHardwareModule(x.Elementname.Trim(), _repositoryWrapper),
                    Boardormodulenamecomponentname = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ? assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.HwComponentName).FirstOrDefault() :
                    _genericReportGenration.GetHardwareModule(x.Elementname.Trim(), _repositoryWrapper),


                    Boardormoduletypecomponentsubtype = string.Join(";", hardwareconfigurations.Where(hw => hw.ElementName == x.Elementname)
                                                      .Select(x => x.UnitLocation == "NA" ? x.HardwareType : x.UnitLocation + "_" + x.HardwareType).ToList()),
                    Boardormoduletypecomponentversionnumber = string.Join(";", hardwareconfigurations.Where(hw => hw.ElementName == x.Elementname).Select(x => x.Revision).ToList()),

                    Exposededge = x.Lcmengineering?.Lcmancillarydata.Where(x => x.Lcmengineeringid == x.Lcmengineering?.Lcmengineeringid).FirstOrDefault()?.Isexposededge == true ? ConstantValueFilter.Yes : ConstantValueFilter.No,
                    Externallyfacingsystem = x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                      x.Lcmengineering.Lcmancillarydata.Any(y => y.Externalfacingflag != null)).FirstOrDefault()?.Externalfacingflag != null ?
                                      x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                      x.Lcmengineering.Lcmancillarydata.Any(y => y.Externalfacingflag != null)).FirstOrDefault()?.Externalfacingflag.Value == true ? ConstantValueFilter.Yes : ConstantValueFilter.No : ConstantValueFilter.No,
                    Managementplane = x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory.ToLower().Replace(" ", "") == "operatingsupportsystem" ? ConstantValueFilter.Yes :
                    x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory.ToLower().Replace(" ", "") == ConstantValueFilter.NetworkElement
                    || x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory.ToLower().Replace(" ", "") == ConstantValueFilter.ApplicationOrPlatform ? ConstantValueFilter.No : string.Empty,
                    Networkoversightfunction = x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                     x.Lcmengineering.Lcmancillarydata.Any(y => y.Isnof != null)).FirstOrDefault()?.Isnof != null ?
                                     x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                     x.Lcmengineering.Lcmancillarydata.Any(y => y.Isnof != null)).FirstOrDefault()?.Isnof.Value == true ? ConstantValueFilter.Yes : ConstantValueFilter.No : ConstantValueFilter.No,
                    Pecn = x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                     x.Lcmengineering.Lcmancillarydata.Any(y => y.Ispecn != null)).FirstOrDefault()?.Ispecn != null ?
                                     x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                     x.Lcmengineering.Lcmancillarydata.Any(y => y.Ispecn != null)).FirstOrDefault()?.Ispecn.Value == true ? ConstantValueFilter.Yes : ConstantValueFilter.No : ConstantValueFilter.No,
                    Pecs = x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                      x.Lcmengineering.Lcmancillarydata.Any(y => y.Ispecs != null)).FirstOrDefault()?.Ispecs != null ?
                                      x.Lcmengineering?.Lcmancillarydata
                                     .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid &&
                                      x.Lcmengineering.Lcmancillarydata.Any(y => y.Ispecs != null)).FirstOrDefault()?.Ispecs.Value == true ? ConstantValueFilter.Yes : ConstantValueFilter.No : ConstantValueFilter.No,
                    Securitycriticalfunction = x.Lcmengineering?.Lcmancillarydata
                                 .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid).FirstOrDefault().GetSCF(),
                    Productimportance = x.Lcmengineering?.Productimportance?.Productimportance,
                    Critical = x.Designcomponent.Subnetworkboundary.GetCriticalityOrCriticalityType(false),
                    Criticalitytype = x.Designcomponent.Subnetworkboundary.GetCriticalityOrCriticalityType(true),
                    Serialnumber = string.Empty,/*string.Join(";", hardwareconfigurations.Where(hw => hw.ElementName == x.Elementname).Select(s => s.SerialNumber).ToList()),*/
                    //Serialnumber = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.
                    //Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ?
                    //assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.SwitchSerialNumbers).FirstOrDefault() : string.Empty,

                    //Partnumber = string.Empty, // no
                    Partnumber = assetOmcDetails.Any(f => f.AssetName == x.Elementname) == true && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.
                    Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure && mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS).FirstOrDefault() != null ?
                    assetOmcDetails.Where(f => f.AssetName == x.Elementname).Select(r => r.HwPartNumber).FirstOrDefault() :
                    string.Empty, // no


                    Descriptionofplannedaction = x.Lcmengineering?.PlannedactivitiesLcmengineering?.GetPlannedAction(_repositoryWrapper),
                    Identifiedaction = LCMEngineeringRulesExtension.GetIdentificationActionForTsr(x.Lcmengineering?.PlannedactivitiesLcmengineering?.OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance),
                    //Lastupgradedate = dcfLifeCycle.Where(y =>
                    //                y.Currentdetails == x.Elementname && y.EventName.ToLower().Replace(" ", "") == "softwareupgrade")
                    //                .Select(x => x.Modificationdate)?.FirstOrDefault().ToString(ConstantValueFilter.DateFormat) ?? string.Empty,
                    Prodorlab = x.Environment?.Environment.Trim().ToLower() == ConstantValueFilter.Production ? ConstantValueFilter.Production.ToUpper() : ConstantValueFilter.Lab.ToUpper(),
                    Localmarketownership = x.Opco.Opco,
                    Budgetestimated = x.Lcmengineering?.PlannedactivitiesLcmengineering?.GetPlannedActivityBudgetEstimatedForTsr(),
                    Bundlebudget = LCMEngineeringRulesExtension.GetBundleBudget(x.Lcmengineering?.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software).Plannedactivityresourceid == null ? null :
                                    x.Lcmengineering?.PlannedactivitiesLcmengineering?.GetPlannedActivityFilteredDB(ConstantValueFilter.Software).Budgettrackingid),
                    Assurancecall = x.Lcmengineering?.PlannedactivitiesLcmengineering != null && x.Lcmengineering?.PlannedactivitiesLcmengineering.Count > 0 ? (x.Lcmengineering?.PlannedactivitiesLcmengineering?.Select(x => x.Budgettrackingid).FirstOrDefault() != null
                                    ? x.Lcmengineering?.PlannedactivitiesLcmengineering?.Select(x => x.Budgettrackingid).FirstOrDefault() : "UNKNOWN") : ConstantValueFilter.NA,
                    Commentonprojectstatus = x.Lcmengineering?.Lcmancillarydata
                                 .Where(x => x.Lcmengineeringid == x.Lcmengineering.Lcmengineeringid).FirstOrDefault()?.Commentonprojectstatus,
                    Projectenddate = x.Lcmengineering?.PlannedactivitiesLcmengineering?.Select(x => x.Plannedcompletion).FirstOrDefault()?.ToString(ConstantValueFilter.DateFormat) ?? string.Empty,
                    Projectstatus = _commonManager.GetProjectStatus(x.Lcmengineering.PlannedactivitiesLcmengineering.Select(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance, x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport),
                    //Servicelevel = x.Designcomponent.Systemtype.Assetcategory.Assetcategory,
                    Lastpentestdate = x.Lcmengineering?.Lcmancillarydata
                                    .Where(x => x.Lcmengineeringid == x.Lcmengineering?.Lcmengineeringid).FirstOrDefault()?.Lastpentestdate?.ToString(ConstantValueFilter.DateFormat),
                    Lastpentestrefno = x.Lcmengineering?.Lcmancillarydata
                                      .Where(x => x.Lcmengineeringid == x.Lcmengineering?.Lcmengineeringid).FirstOrDefault()?.Lastpentestreferencenumber,
                    Pidata = ConstantValueFilter.NO,
                    Encryptedpidata = ConstantValueFilter.NA,
                    Nontemsvertical = null,

                    Productname = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description,
                    Softwareversion = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Softwareversion,
                    Subdomainresponsible = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
                                            .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList()),
                    Issortingallowed = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .Where(mjh => mjh.Majorhardware.Buildconstruction.Buildconstruction == ConstantValueFilter.Infrastructure /*&& mjh.Majorhardware.Platform.Platform == ConstantValueFilter.CNIS*/).FirstOrDefault() != null ? true : false,

                    #endregion

                }).DistinctBy(x => new { x.Assetname, x.Vodafoneuniqueidentifier }).ToList();

                foreach (var tsr in tsrPassThroughDatas)
                {
                    var tsrEntryExist = _repositoryWrapper.TsrPassThroughRepository.FindByCondition(x => x.Assetname == tsr.Assetname &&
                    x.Vodafoneuniqueidentifier == tsr.Vodafoneuniqueidentifier).FirstOrDefault();

                    if (tsrEntryExist != null)
                    {
                        var isUpdated = await Task.Run(() => TSRUpdate(tsr, tsrEntryExist));

                        updateTsrList.Add(isUpdated);
                    }
                    else
                    {
                        createTsrList.Add(tsr);
                    }

                }
                var batchIdentifier = dto.CountryWhereAssetIsLocated != null && dto.CountryWhereAssetIsLocated.Any() ? dto.CountryWhereAssetIsLocated.FirstOrDefault() : "UK, Group";
                var records = updateTsrList.Count + createTsrList.Count;

                _repositoryWrapper.TsrLogRepository.Create(new Tsrlogs
                {
                    Typeofoperation = ConstantValueFilter.DataRefreshTypeofoperation,
                    Totalrecord = records,
                    Processedrecord = 0,
                    Starttime = currentTime,
                    Endtime = null,
                    Domain = batchIdentifier,
                    Status = ConstantValueFilter.TsrLogInProgressStatus,
                    Batchidentifier = batchIdentifier,

                });
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

                if (updateTsrList.Count > 0)
                {

                    _repositoryWrapper.TsrPassThroughRepository.BulkUpdate(updateTsrList);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                if (createTsrList.Count > 0)
                {
                    _repositoryWrapper.TsrPassThroughRepository.BulkCreate(createTsrList);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }

                _repositoryWrapper.TsrLogRepository.Create(new Tsrlogs
                {
                    Typeofoperation = ConstantValueFilter.DataRefreshTypeofoperation,
                    Totalrecord = records,
                    Processedrecord = records,
                    Starttime = null,
                    Endtime = currentTime,
                    Domain = batchIdentifier,
                    Status = ConstantValueFilter.TsrLogCompletedStatus,
                    Batchidentifier = batchIdentifier,

                });
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

                return new ResultDto
                {
                    Info = "TSR Data inserted and updated successfully",
                    Warning = false
                };
            }
            catch (Exception ex)
            {
                _repositoryWrapper.TsrLogRepository.Create(new Tsrlogs
                {
                    Typeofoperation = ConstantValueFilter.DataRefreshTypeofoperation,
                    Starttime = null,
                    Endtime = currentTime,
                    Status = ConstantValueFilter.TsrLogCompletedStatus,

                });
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

                return new ResultDto
                {
                    Info = "Something went wrong while loading data to the TSR report",
                    Warning = true,
                    Data = ex.InnerException.Message
                };
            }
        }


        private IQueryable<Networkelementsasplanned> GetDisaggregatedQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {

            var result = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).AsSplitQuery().AsNoTracking()
                                .Where(x =>
                                    x.Designcomponent.Systemtype.Deleted == false &&
                                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false) &&
                                    x.Lcmengineeringid != null 
                                )
                                .Include(x => x.Opco)
                                .Include(x => x.Environment)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmdeploymentstatus)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Productimportance)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmoperationalcontracts).ThenInclude(x => x.Operationalcontract)
                                .Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x => x.Subdomainspoc)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Productimportance)
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
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Siteresilience)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designaspect).ThenInclude(x => x.Businesscontinuitymethod)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Plannedactivityresource)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.Identitiesasis).ThenInclude(x => x.Category)
                                .Include(x => x.ModificationuserNavigation);
            return result;
        }
        #endregion

        private Tsrpassthrough TSRUpdate(Tsrpassthrough dataLoadTsr, Tsrpassthrough exixtTsrEntry)
        {
            try
            {
                #region //updatemapping
                exixtTsrEntry.Assetid = dataLoadTsr.Assetid;
                exixtTsrEntry.Vodafoneuniqueidentifier = dataLoadTsr.Vodafoneuniqueidentifier;
                exixtTsrEntry.Assetname = dataLoadTsr.Assetname;
                exixtTsrEntry.Assetdescriptionorpurpose = dataLoadTsr.Assetdescriptionorpurpose;
                exixtTsrEntry.Assettype = dataLoadTsr.Assettype;
                exixtTsrEntry.Businessowner = dataLoadTsr.Businessowner;
                //exixtTsrEntry.Supportowner = dataLoadTsr.Supportowner;
                //exixtTsrEntry.Supportteam = dataLoadTsr.Supportteam;
                //exixtTsrEntry.Supportteamsplaceintheorganisation = dataLoadTsr.Supportteamsplaceintheorganisation;
                exixtTsrEntry.Assetfunction = dataLoadTsr.Assetfunction;
                exixtTsrEntry.Deploymentorlifecyclestatus = dataLoadTsr.Deploymentorlifecyclestatus;
                //exixtTsrEntry.Relatedriskidsfromriskregisters = dataLoadTsr.Relatedriskidsfromriskregisters;
                exixtTsrEntry.Regulatoryscope = dataLoadTsr.Regulatoryscope;
                exixtTsrEntry.Countrywhereassetislocated = dataLoadTsr.Countrywhereassetislocated;
                exixtTsrEntry.Geolocation = dataLoadTsr.Geolocation;
                exixtTsrEntry.Infrastructure = dataLoadTsr.Infrastructure;
                //exixtTsrEntry.Upstreamdependencies = dataLoadTsr.Upstreamdependencies;
                //exixtTsrEntry.Downstreamdependencies = dataLoadTsr.Downstreamdependencies;
                //exixtTsrEntry.Changestotheassetsincedeployment = dataLoadTsr.Changestotheassetsincedeployment;
                exixtTsrEntry.Cloudhostedasset = dataLoadTsr.Cloudhostedasset;
                exixtTsrEntry.Cloudtype = dataLoadTsr.Cloudtype;
                exixtTsrEntry.Cloudvendor = dataLoadTsr.Cloudvendor;
                exixtTsrEntry.Equipmentname = dataLoadTsr.Equipmentname;
                //exixtTsrEntry.Hostlocationwithinphysicallocation = dataLoadTsr.Hostlocationwithinphysicallocation;
                exixtTsrEntry.Softwarevendorname = dataLoadTsr.Softwarevendorname;
                exixtTsrEntry.Model = dataLoadTsr.Model;
                exixtTsrEntry.Firmwareversion = dataLoadTsr.Firmwareversion;
                exixtTsrEntry.Firmwareversionpatchlevel = dataLoadTsr.Firmwareversionpatchlevel;
                exixtTsrEntry.Maintenancesupportsupplier = dataLoadTsr.Maintenancesupportsupplier;
                exixtTsrEntry.Vendorhardwareendofsupportdate = dataLoadTsr.Vendorhardwareendofsupportdate;
                //exixtTsrEntry.Maintenancehardwareendofsupportdate = dataLoadTsr.Maintenancehardwareendofsupportdate;
                exixtTsrEntry.Dependanthardware = dataLoadTsr.Dependanthardware;
                exixtTsrEntry.Instancetype = dataLoadTsr.Instancetype;
                exixtTsrEntry.Operatingsystemname = dataLoadTsr.Operatingsystemname;
                exixtTsrEntry.Operatingsystemswvversion = dataLoadTsr.Operatingsystemswvversion;
                //exixtTsrEntry.Operatingsystemswversionpatchlevel = dataLoadTsr.Operatingsystemswversionpatchlevel;
                exixtTsrEntry.Systemnamedns = dataLoadTsr.Systemnamedns;
                exixtTsrEntry.Systemnamemanagementipaddress = dataLoadTsr.Systemnamemanagementipaddress;
                exixtTsrEntry.Systemnamenetbios = dataLoadTsr.Systemnamenetbios;
                exixtTsrEntry.Systemnamehostname = dataLoadTsr.Systemnamehostname;
                exixtTsrEntry.Dateassetmovedtolivestatus = dataLoadTsr.Dateassetmovedtolivestatus;
                exixtTsrEntry.Dateassetdecommissioned = dataLoadTsr.Dateassetdecommissioned;
                exixtTsrEntry.Hardwarevendorname = dataLoadTsr.Hardwarevendorname;
                //exixtTsrEntry.Maintenancesupportsuppliersecond = dataLoadTsr.Maintenancesupportsuppliersecond;
                exixtTsrEntry.Vendorsoftwareendofsupportdate = dataLoadTsr.Vendorsoftwareendofsupportdate;
                //exixtTsrEntry.Maintenancesoftwareendofsupportdate = dataLoadTsr.Maintenancesoftwareendofsupportdate;
                exixtTsrEntry.Dependantsystemsoftware = dataLoadTsr.Dependantsystemsoftware;
                exixtTsrEntry.Resiliencemodel = dataLoadTsr.Resiliencemodel;
                exixtTsrEntry.Geographicsiteresilience = dataLoadTsr.Geographicsiteresilience;
                exixtTsrEntry.Localsiteresilience = dataLoadTsr.Localsiteresilience;
                //exixtTsrEntry.Nameofproductsdependantonasset = dataLoadTsr.Nameofproductsdependantonasset;
                exixtTsrEntry.Technicalservicenames = dataLoadTsr.Technicalservicenames;
                //exixtTsrEntry.Customer = dataLoadTsr.Customer;
                //exixtTsrEntry.Privilegedaccesslogging = dataLoadTsr.Privilegedaccesslogging;

                exixtTsrEntry.Boardormodulenamecomponentname = dataLoadTsr.Boardormodulenamecomponentname;
                exixtTsrEntry.Boardormoduletypecomponentsubtype = dataLoadTsr.Boardormoduletypecomponentsubtype;
                exixtTsrEntry.Boardormoduletypecomponentversionnumber = dataLoadTsr.Boardormoduletypecomponentversionnumber;
                exixtTsrEntry.Exposededge = dataLoadTsr.Exposededge;
                exixtTsrEntry.Externallyfacingsystem = dataLoadTsr.Externallyfacingsystem;
                exixtTsrEntry.Managementplane = dataLoadTsr.Managementplane;
                exixtTsrEntry.Networkoversightfunction = dataLoadTsr.Networkoversightfunction;
                exixtTsrEntry.Pecn = dataLoadTsr.Pecn;
                exixtTsrEntry.Pecs = dataLoadTsr.Pecs;
                exixtTsrEntry.Securitycriticalfunction = dataLoadTsr.Securitycriticalfunction;
                exixtTsrEntry.Productimportance = dataLoadTsr.Productimportance;
                exixtTsrEntry.Critical = dataLoadTsr.Critical;
                exixtTsrEntry.Criticalitytype = dataLoadTsr.Criticalitytype;
                exixtTsrEntry.Serialnumber = dataLoadTsr.Serialnumber;
                exixtTsrEntry.Partnumber = dataLoadTsr.Partnumber;
                exixtTsrEntry.Descriptionofplannedaction = dataLoadTsr.Descriptionofplannedaction;
                exixtTsrEntry.Identifiedaction = dataLoadTsr.Identifiedaction;
                exixtTsrEntry.Lastupgradedate = dataLoadTsr.Lastupgradedate;
                exixtTsrEntry.Prodorlab = dataLoadTsr.Prodorlab;
                exixtTsrEntry.Localmarketownership = dataLoadTsr.Localmarketownership;
                exixtTsrEntry.Budgetestimated = dataLoadTsr.Budgetestimated;
                exixtTsrEntry.Bundlebudget = dataLoadTsr.Bundlebudget;
                exixtTsrEntry.Assurancecall = dataLoadTsr.Assurancecall;
                exixtTsrEntry.Commentonprojectstatus = dataLoadTsr.Commentonprojectstatus;
                exixtTsrEntry.Projectenddate = dataLoadTsr.Projectenddate;
                exixtTsrEntry.Projectstatus = dataLoadTsr.Projectstatus;
                //exixtTsrEntry.Servicelevel = dataLoadTsr.Servicelevel;
                exixtTsrEntry.Lastpentestdate = dataLoadTsr.Lastpentestdate;
                exixtTsrEntry.Lastpentestrefno = dataLoadTsr.Lastpentestrefno;
                exixtTsrEntry.Pidata = dataLoadTsr.Pidata;
                exixtTsrEntry.Encryptedpidata = dataLoadTsr.Encryptedpidata;
                exixtTsrEntry.Productname = dataLoadTsr.Productname;
                exixtTsrEntry.Subdomainresponsible = dataLoadTsr.Subdomainresponsible;
                exixtTsrEntry.Softwareversion = dataLoadTsr.Softwareversion;
                exixtTsrEntry.Issortingallowed = dataLoadTsr.Issortingallowed;

                #endregion

                return exixtTsrEntry;
            }
            catch
            {
                return new Tsrpassthrough();
            }
        }

        private async Task<List<AssetOmcIntegretion>> AssetOmcIntegrationFields(List<string> assetNames)
        {
            var result = new List<AssetOmcIntegretion>();

            var mappingDetails = await _repositoryWrapper.AssetMapInfoRepository.FindByCondition(x => assetNames.Contains(x.Temsassetname.ToLower().Trim())).ToListAsync();

            var assetAsIsSdiInfoDetails = await _repositoryWrapper.AssetAsIsSdiInfoRepository.FindAll().ToListAsync();
            var assetAsIsHwAncillaryDataDetails = await _repositoryWrapper.AssetAsIsHwAncillaryDataRepository.FindAll().ToListAsync();
            var assetAsIsSdiSwitchInfoDetails = await _repositoryWrapper.AssetAsIsSdiSwitchInfoRepository.FindAll().ToListAsync();

             var query = mappingDetails?
                         .Select(m => new
                         {
                             mappingAssetInfo = m,

                             assetAsIsSdiINfo = assetAsIsSdiInfoDetails
                                 .Where(ai => ai.Datasourcename == m.Omcassetname)
                                 .ToList(),

                             assetHwAncillary = assetAsIsHwAncillaryDataDetails
                                 .Where(t => t.Datasourcename == m.Omcassetname)
                                 .ToList(),

                             assetSwitch = assetAsIsSdiSwitchInfoDetails
                                 .Where(t1 => t1.Datasourcename == m.Omcassetname)
                                 .ToList(),
                         
                          assetHwAncillaryForCluster = assetAsIsHwAncillaryDataDetails
                                                 .Where(t => t.Clustername == m.Omcassetname)
                                                 .ToList(),
                         }).ToList();
            try
            {
                    result = query.Select(x =>
                    {
                        var grid = new AssetOmcIntegretion();
                        grid.AssetName = x.mappingAssetInfo?.Temsassetname;
                        grid.CloudVendor = x?.assetAsIsSdiINfo?.Select(r => r.Manufacturer).FirstOrDefault();
                        grid.Model = x?.assetAsIsSdiINfo?.Select(r => r.Model).FirstOrDefault();
                        grid.FirmwareVersion = x?.assetAsIsSdiINfo?.Select(r => r.Firmwareversion).FirstOrDefault();
                        grid.DependentHW = x?.assetHwAncillary?.Select(r => r.Model).FirstOrDefault();
                        grid.ManagementIpAddress = string.Join(";", x?.assetHwAncillary?.Select(r => r.Managementip).ToList());
                        grid.HwManufacturer = x?.assetHwAncillary.Select(r => r.Manufacturer).FirstOrDefault();
                        grid.HwComponentName = string.Join(";", x?.assetHwAncillary?.Select(r => r.Chassisdetails).ToList());
                        grid.HwPartNumber = x?.assetHwAncillary?.Select(r => r.Partnumber).FirstOrDefault();
                        grid.SystemName = string.Join(";", x?.assetHwAncillary?.Select(r => r.Consumer).ToList());
                        grid.SystemNameForCluster = string.Join(";", x?.assetHwAncillaryForCluster?.Select(r => r.Consumer).ToList());
                        grid.ManagementIpAddressForCluster = string.Join(";", x?.assetHwAncillaryForCluster?.Select(r => r.Managementip).ToList());
                        grid.SwitchSerialNumbers = string.Join(";", x?.assetSwitch?.Select(r => r.Switchserialnumber).ToList());
                        return grid;
                    }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }


        #region //TSR Column Description
        public async Task<IEnumerable<GlossaryItemsGridDto>> GetTsrColDescription()
        {

            try
            {
                var entity = await _repositoryWrapper.GlossaryItemsRepository.FindByCondition(x => x.Istsrfield == true).ToListAsync();

                var result = entity.Select(x =>
                {
                    var grid = new GlossaryItemsGridDto();

                    grid.Header = x.Header;
                    grid.Description = x.Description;
                    grid.IsTsrField = (bool)x.Istsrfield;

                    return grid;
                });
                return result;
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }
}
