using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models.PassThroughData;
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


namespace CAM.BusinessManager.Entity.PassThroughData
{
    public class AssetPassThroughManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownmanager;

        public AssetPassThroughManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager manager, DropdownDataServiceManager dropdownmanager,
             IHttpContextAccessor contextAccessor,IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _manager = manager;
            _dropdownmanager = dropdownmanager;
        }


        #region //UiMemberFunctions
        public QueryResultDto<PassThroughDtoGrid> FindWithCondition(PassThroughQueryDto passThroughQueryDto)
        {
            var predicateResult = ApplyFilter(passThroughQueryDto);
            var rtn = new QueryResultDto<PassThroughDtoGrid>(new GenerateRenderForGrid<PassThroughDtoGrid>(_manager))
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.PassThroughRepository.Count(predicateResult) : _repositoryWrapper.PassThroughRepository.Count(),
            };
            var query = GetQuery(predicateResult, passThroughQueryDto.Deleted ?? false).ApplyOrdering(passThroughQueryDto, GetColumnsMap()).ApplyPaging(passThroughQueryDto);
            var data = query.ToList();
                     
            IEnumerable <PassThroughDtoGrid> passThroughDtoGrid;

            passThroughDtoGrid = _mapper.Map<IEnumerable<PassThroughDtoGrid>>(data);

            rtn.Items = passThroughDtoGrid.ToArray();
            return rtn;
        }

        private static ExpressionStarter<Assetpassthrough> ApplyFilter(PassThroughQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Assetpassthrough>();
            var predicateInner = PredicateBuilder.New<Assetpassthrough>();

            if (buildFilterDto.VodafoneUniqueIdentifier != null && buildFilterDto.VodafoneUniqueIdentifier.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.VodafoneUniqueIdentifier)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetName != null && buildFilterDto.AssetName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.AssetName)
                    predicateInner.Or(x => x.Assetname == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.AssetDescriptionOrPurpose != null && buildFilterDto.AssetDescriptionOrPurpose.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.AssetDescriptionOrPurpose)
                    predicateInner.Or(x => x.Assetdescriptionorpurpose == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetTypeTsr != null && buildFilterDto.AssetTypeTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.AssetTypeTsr)
                    predicateInner.Or(x => x.Assettype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BusinessOwner != null && buildFilterDto.BusinessOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.BusinessOwner)
                    predicateInner.Or(x => x.Businessowner == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportOwner != null && buildFilterDto.SupportOwner.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SupportOwner)
                    predicateInner.Or(x => x.Supportowner == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportTeam != null && buildFilterDto.SupportTeam.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SupportTeam)
                    predicateInner.Or(x => x.Supportteam == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SupportTeamsPlaceInTheOrganisation != null && buildFilterDto.SupportTeamsPlaceInTheOrganisation.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SupportTeamsPlaceInTheOrganisation)
                    predicateInner.Or(x => x.Supportteamsplaceintheorganisation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssetFunction != null && buildFilterDto.AssetFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.AssetFunction)
                    predicateInner.Or(x => x.Assetfunction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DeploymentOrLifeCycleStatus != null && buildFilterDto.DeploymentOrLifeCycleStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.DeploymentOrLifeCycleStatus)
                    predicateInner.Or(x => x.Deploymentorlifecyclestatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RelatedRiskIdsFromRiskRegisters != null && buildFilterDto.RelatedRiskIdsFromRiskRegisters.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.RelatedRiskIdsFromRiskRegisters)
                    predicateInner.Or(x => x.Relatedriskidsfromriskregisters == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.RegulatoryScope != null && buildFilterDto.RegulatoryScope.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.RegulatoryScope)
                    predicateInner.Or(x => x.Regulatoryscope == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CountryWhereAssetIsLocated != null && buildFilterDto.CountryWhereAssetIsLocated.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.CountryWhereAssetIsLocated)
                    predicateInner.Or(x => x.Countrywhereassetislocated == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Geolocation != null && buildFilterDto.Geolocation.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Geolocation)
                    predicateInner.Or(x => x.Geolocation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Infrastructure != null && buildFilterDto.Infrastructure.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Infrastructure)
                    predicateInner.Or(x => x.Infrastructure == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UpStreamDependencies != null && buildFilterDto.UpStreamDependencies.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.UpStreamDependencies)
                    predicateInner.Or(x => x.Upstreamdependencies == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DownStreamDependencies != null && buildFilterDto.DownStreamDependencies.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.DownStreamDependencies)
                    predicateInner.Or(x => x.Downstreamdependencies == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ChangesToTheassetSinceDeployment != null && buildFilterDto.ChangesToTheassetSinceDeployment.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ChangesToTheassetSinceDeployment)
                    predicateInner.Or(x => x.Changestotheassetsincedeployment == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CloudHostedAsset != null && buildFilterDto.CloudHostedAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.CloudHostedAsset)
                    predicateInner.Or(x => x.Cloudhostedasset == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CloudType != null && buildFilterDto.CloudType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.CloudType)
                    predicateInner.Or(x => x.Cloudtype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.CloudVendor != null && buildFilterDto.CloudVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.CloudVendor)
                    predicateInner.Or(x => x.Cloudvendor == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EquipmentName != null && buildFilterDto.EquipmentName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.EquipmentName)
                    predicateInner.Or(x => x.Equipmentname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HostLocationWithInPhysicalLocation != null && buildFilterDto.HostLocationWithInPhysicalLocation.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.HostLocationWithInPhysicalLocation)
                    predicateInner.Or(x => x.Hostlocationwithinphysicallocation == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FirmwareVersion != null && buildFilterDto.FirmwareVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.FirmwareVersion)
                    predicateInner.Or(x => x.Firmwareversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.FirmwareVersionPatchLevel != null && buildFilterDto.FirmwareVersionPatchLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.FirmwareVersionPatchLevel)
                    predicateInner.Or(x => x.Firmwareversionpatchlevel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MaintenanceSupportSupplier != null && buildFilterDto.MaintenanceSupportSupplier.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.MaintenanceSupportSupplier)
                    predicateInner.Or(x => x.Maintenancesupportsupplier == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MaintenanceHardwareEndOfSupportDate != null && buildFilterDto.MaintenanceHardwareEndOfSupportDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.MaintenanceHardwareEndOfSupportDate)
                    predicateInner.Or(x => x.Maintenancehardwareendofsupportdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DependantHardware != null && buildFilterDto.DependantHardware.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.DependantHardware)
                    predicateInner.Or(x => x.Dependanthardware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.InstanceType != null && buildFilterDto.InstanceType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.InstanceType)
                    predicateInner.Or(x => x.Instancetype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OperatingSystemName != null && buildFilterDto.OperatingSystemName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.OperatingSystemName)
                    predicateInner.Or(x => x.Operatingsystemname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OperatingSystemSwvVersion != null && buildFilterDto.OperatingSystemSwvVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.OperatingSystemSwvVersion)
                    predicateInner.Or(x => x.Operatingsystemswvversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OperatingSystemSwVersionPatchLevel != null && buildFilterDto.OperatingSystemSwVersionPatchLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.OperatingSystemSwVersionPatchLevel)
                    predicateInner.Or(x => x.Operatingsystemswversionpatchlevel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameDns != null && buildFilterDto.SystemNameDns.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SystemNameDns)
                    predicateInner.Or(x => x.Systemnamedns == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameManagementIpAddress != null && buildFilterDto.SystemNameManagementIpAddress.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SystemNameManagementIpAddress)
                    predicateInner.Or(x => x.Systemnamemanagementipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameNetBios != null && buildFilterDto.SystemNameNetBios.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SystemNameNetBios)
                    predicateInner.Or(x => x.Systemnamenetbios == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SystemNameHostName != null && buildFilterDto.SystemNameHostName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SystemNameHostName)
                    predicateInner.Or(x => x.Systemnamehostname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DateAssetMovedToLiveStatus != null && buildFilterDto.DateAssetMovedToLiveStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.DateAssetMovedToLiveStatus)
                    predicateInner.Or(x => x.Dateassetmovedtolivestatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DateAssetDecommissioned != null && buildFilterDto.DateAssetDecommissioned.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.DateAssetDecommissioned)
                    predicateInner.Or(x => x.Dateassetdecommissioned == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.DependantSystemSoftware != null && buildFilterDto.DependantSystemSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.DependantSystemSoftware)
                    predicateInner.Or(x => x.Dependantsystemsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ResilienceModel != null && buildFilterDto.ResilienceModel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ResilienceModel)
                    predicateInner.Or(x => x.Resiliencemodel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.GeographicSiteResilience != null && buildFilterDto.GeographicSiteResilience.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.GeographicSiteResilience)
                    predicateInner.Or(x => x.Geographicsiteresilience == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalSiteResilience != null && buildFilterDto.LocalSiteResilience.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.LocalSiteResilience)
                    predicateInner.Or(x => x.Localsiteresilience == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NameOfProductsDependantOnAsset != null && buildFilterDto.NameOfProductsDependantOnAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.NameOfProductsDependantOnAsset)
                    predicateInner.Or(x => x.Nameofproductsdependantonasset == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Customer != null && buildFilterDto.Customer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Customer)
                    predicateInner.Or(x => x.Customer == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PrivilegedAccessLogging != null && buildFilterDto.PrivilegedAccessLogging.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PrivilegedAccessLogging)
                    predicateInner.Or(x => x.Privilegedaccesslogging == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BoardOrModuleNameComponentName != null && buildFilterDto.BoardOrModuleNameComponentName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.BoardOrModuleNameComponentName)
                    predicateInner.Or(x => x.Boardormodulenamecomponentname.Contains(item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BoardOrModuleTypeComponentSubtype != null && buildFilterDto.BoardOrModuleTypeComponentSubtype.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.BoardOrModuleTypeComponentSubtype)
                    predicateInner.Or(x => x.Boardormoduletypecomponentsubtype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.BoardOrModuleTypeComponentVersionNumber != null && buildFilterDto.BoardOrModuleTypeComponentVersionNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.BoardOrModuleTypeComponentVersionNumber)
                    predicateInner.Or(x => x.Boardormoduletypecomponentversionnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExposedEdge != null && buildFilterDto.ExposedEdge.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ExposedEdge)
                    predicateInner.Or(x => x.Exposededge == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ExternallyFacingSystem != null && buildFilterDto.ExternallyFacingSystem.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ExternallyFacingSystem)
                    predicateInner.Or(x => x.Externallyfacingsystem == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ManagementPlane != null && buildFilterDto.ManagementPlane.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ManagementPlane)
                    predicateInner.Or(x => x.Managementplane == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NetworkOverSightFunction != null && buildFilterDto.NetworkOverSightFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.NetworkOverSightFunction)
                    predicateInner.Or(x => x.Networkoversightfunction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Pecn != null && buildFilterDto.Pecn.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Pecn)
                    predicateInner.Or(x => x.Pecn == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Pecs != null && buildFilterDto.Pecs.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Pecs)
                    predicateInner.Or(x => x.Pecs == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SecurityCriticalFunction != null && buildFilterDto.SecurityCriticalFunction.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SecurityCriticalFunction)
                    predicateInner.Or(x => x.Securitycriticalfunction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Critical != null && buildFilterDto.Critical.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Critical)
                    predicateInner.Or(x => x.Critical == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SerialNumberTsr != null && buildFilterDto.SerialNumberTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SerialNumberTsr)
                    predicateInner.Or(x => x.Serialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PartNumber != null && buildFilterDto.PartNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PartNumber)
                    predicateInner.Or(x => x.Partnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProdOrLab != null && buildFilterDto.ProdOrLab.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ProdOrLab)
                    predicateInner.Or(x => x.Prodorlab == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocalMarketOwnerShip != null && buildFilterDto.LocalMarketOwnerShip.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.LocalMarketOwnerShip)
                    predicateInner.Or(x => x.Localmarketownership == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.AssuranceCall != null && buildFilterDto.AssuranceCall.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.AssuranceCall)
                    predicateInner.Or(x => x.Assurancecall == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ServiceLevel != null && buildFilterDto.ServiceLevel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ServiceLevel)
                    predicateInner.Or(x => x.Servicelevel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastPenTestDateTsr != null && buildFilterDto.LastPenTestDateTsr.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.LastPenTestDateTsr)
                    predicateInner.Or(x => x.Lastpentestdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastPenTestRefNo != null && buildFilterDto.LastPenTestRefNo.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.LastPenTestRefNo)
                    predicateInner.Or(x => x.Lastpentestrefno == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PiData != null && buildFilterDto.PiData.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PiData)
                    predicateInner.Or(x => x.Pidata == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.EncryptedPiData != null && buildFilterDto.EncryptedPiData.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.EncryptedPiData)
                    predicateInner.Or(x => x.Encryptedpidata == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModifiedBy != null && buildFilterDto.LastModifiedBy.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.LastModifiedBy)
                    predicateInner.Or(x => x.ModificationuserNavigation.Email == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LastModified != null)
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                if (buildFilterDto.LastModified.StartDate != null)
                    predicateInner.And(x => x.Modificationdate.Date >= buildFilterDto.LastModified.StartDate);
                if (buildFilterDto.LastModified.EndDate != null)
                    predicateInner.And(x => x.Modificationdate.Date <= buildFilterDto.LastModified.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.NonTemsVertical != null && buildFilterDto.NonTemsVertical.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.NonTemsVertical)
                    predicateInner.Or(x => x.Nontemsvertical == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HardwareTypeofHardwareAsset != null && buildFilterDto.HardwareTypeofHardwareAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.HardwareTypeofHardwareAsset)
                    predicateInner.Or(x => x.Hardwaretypeofhardwareasset == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HwEndofSale != null && buildFilterDto.HwEndofSale.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.HwEndofSale)
                    predicateInner.Or(x => x.Hwendofsale == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareProductType != null && buildFilterDto.SoftwareProductType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SoftwareProductType)
                    predicateInner.Or(x => x.Softwareproducttype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ApplicationHostedonSoftware != null && buildFilterDto.ApplicationHostedonSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ApplicationHostedonSoftware)
                    predicateInner.Or(x => x.Applicationhostedonsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.UuidorSerialNumberofSoftware != null && buildFilterDto.UuidorSerialNumberofSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.UuidorSerialNumberofSoftware)
                    predicateInner.Or(x => x.Uuidorserialnumberofsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwEndofSale != null && buildFilterDto.SwEndofSale.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SwEndofSale)
                    predicateInner.Or(x => x.Swendofsale == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorEndofMaintenanceDate != null && buildFilterDto.VendorEndofMaintenanceDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.VendorEndofMaintenanceDate)
                    predicateInner.Or(x => x.VendorendofmaintenanceDate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Application != null && buildFilterDto.Application.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Application)
                    predicateInner.Or(x => x.Application == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerHostName != null && buildFilterDto.PhysicalServerHostName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerHostName)
                    predicateInner.Or(x => x.Physicalserverhostname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerIpaddress != null && buildFilterDto.PhysicalServerIpaddress.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerIpaddress)
                    predicateInner.Or(x => x.Physicalserveripaddress == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerSerialNumber != null && buildFilterDto.PhysicalServerSerialNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerSerialNumber)
                    predicateInner.Or(x => x.Physicalserverserialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerHwModel != null && buildFilterDto.PhysicalServerHwModel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerHwModel)
                    predicateInner.Or(x => x.PhysicalserverhwModel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerVendor != null && buildFilterDto.PhysicalServerVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerVendor)
                    predicateInner.Or(x => x.Physicalservervendor == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VirtualServerHostedon != null && buildFilterDto.VirtualServerHostedon.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.VirtualServerHostedon)
                    predicateInner.Or(x => x.Virtualserverhostedon == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VirtualServerManufacturer != null && buildFilterDto.VirtualServerManufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.VirtualServerManufacturer)
                    predicateInner.Or(x => x.Virtualservermanufacturer == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VirtualServerTypeofDevice != null && buildFilterDto.VirtualServerTypeofDevice.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.VirtualServerTypeofDevice)
                    predicateInner.Or(x => x.Virtualservertypeofdevice == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VirtualMachineType != null && buildFilterDto.VirtualMachineType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.VirtualMachineType)
                    predicateInner.Or(x => x.Virtualmachinetype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VirtualServerSerialNumber != null && buildFilterDto.VirtualServerSerialNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.VirtualServerSerialNumber)
                    predicateInner.Or(x => x.Virtualserverserialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OsStartDate != null && buildFilterDto.OsStartDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.OsStartDate)
                    predicateInner.Or(x => x.Osstartdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OsInstallationDate != null && buildFilterDto.OsInstallationDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.OsInstallationDate)
                    predicateInner.Or(x => x.Osinstallationdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.OsStatus != null && buildFilterDto.OsStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.OsStatus)
                    predicateInner.Or(x => x.Osstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SoftwareName != null && buildFilterDto.SoftwareName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SoftwareName)
                    predicateInner.Or(x => x.Softwarename == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Version != null && buildFilterDto.Version.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Version)
                    predicateInner.Or(x => x.Version == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Release != null && buildFilterDto.Release.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Release)
                    predicateInner.Or(x => x.Release == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Language != null && buildFilterDto.Language.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Language)
                    predicateInner.Or(x => x.Language == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.HwOpsContractStatus != null && buildFilterDto.HwOpsContractStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.HwOpsContractStatus)
                    predicateInner.Or(x => x.Hwopscontractstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.SwOpsContractStatus != null && buildFilterDto.SwOpsContractStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.SwOpsContractStatus)
                    predicateInner.Or(x => x.Swopscontractstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlannedHwModel != null && buildFilterDto.PlannedHwModel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PlannedHwModel)
                    predicateInner.Or(x => x.Plannedhwmodel == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.LocationOrDatacentre != null && buildFilterDto.LocationOrDatacentre.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.LocationOrDatacentre)
                    predicateInner.Or(x => x.Locationordatacentre == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerOSName != null && buildFilterDto.PhysicalServerOSName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerOSName)
                    predicateInner.Or(x => x.Physicalserverosname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalOSVersion != null && buildFilterDto.PhysicalOSVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalOSVersion)
                    predicateInner.Or(x => x.Physicalosversion == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerOSStartDate != null && buildFilterDto.PhysicalServerOSStartDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerOSStartDate)
                    predicateInner.Or(x => x.Physicalserverosstartdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerOSinstallationdate != null && buildFilterDto.PhysicalServerOSinstallationdate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerOSinstallationdate)
                    predicateInner.Or(x => x.Physicalserverosinstallationdate == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PhysicalServerOSstatus != null && buildFilterDto.PhysicalServerOSstatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.PhysicalServerOSstatus)
                    predicateInner.Or(x => x.Physicalserverosstatus == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.Cloud != null && buildFilterDto.Cloud.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.Cloud)
                    predicateInner.Or(x => x.Cloud == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ResourceKey != null && buildFilterDto.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in buildFilterDto.ResourceKey)
                    predicateInner.Or(x => x.Resourcekey == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private IQueryable<AssetPassThrough> GetQuery(ExpressionStarter<Assetpassthrough> predicateResult, bool includeDeleted)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.PassThroughRepository.FindByCondition(predicateResult, includeDeleted)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
               : _repositoryWrapper.PassThroughRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation);
            return query.AsEnumerable().Select(x => AssetPassThroughMapper.Get(x)).AsQueryable();
        }


        private Dictionary<string, Expression<Func<AssetPassThrough, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<AssetPassThrough, object>>[]>
            {
                ["passThroughId"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.PassThroughId },
                ["vodafoneUniqueIdentifier"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["assetName"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.AssetName },
                ["assetDescriptionOrPurpose"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.AssetDescriptionOrPurpose },
                ["assetTypeTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.AssetType },
                ["businessOwner"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.BusinessOwner },
                ["supportOwner"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SupportOwner },
                ["supportTeam"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SupportTeam },
                ["supportTeamsPlaceInTheOrganisation"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SupportTeamsPlaceInTheOrganisation },
                ["assetFunction"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.AssetFunction },
                ["deploymentOrLifeCycleStatus"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.DeploymentOrLifeCycleStatus },
                ["relatedRiskIdsFromRiskRegisters"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.RelatedRiskIdsFromRiskRegisters },
                ["regulatoryScope"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.RegulatoryScope },
                ["countryWhereAssetIsLocated"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CountryWhereAssetIsLocated },
                ["geolocation"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.Geolocation },
                ["infrastructure"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.Infrastructure },
                ["upStreamDependencies"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.UpStreamDependencies },
                ["downStreamDependencies"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.DownStreamDependencies },
                ["changesToTheassetSinceDeployment"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ChangesToTheassetSinceDeployment },
                ["cloudHostedAsset"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CloudHostedAsset },
                ["cloudType"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CloudType },
                ["cloudVendor"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CloudVendor },
                ["equipmentName"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.EquipmentName },
                ["hostLocationWithInPhysicalLocation"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.HostLocationWithInPhysicalLocation },
                ["firmwareVersion"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.FirmwareVersion },
                ["firmwareVersionPatchLevel"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.FirmwareVersionPatchLevel },
                ["maintenanceSupportSupplier"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.MaintenanceSupportSupplier },
                ["vendorHardwareEndOfSupportDate"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VendorHardwareEndOfSupportDate },
                ["maintenanceHardwareEndOfSupportDate"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.MaintenanceHardwareEndOfSupportDate },
                ["dependantHardware"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.DependantHardware },
                ["instanceType"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.InstanceType },
                ["operatingSystemName"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.OperatingSystemName },
                ["operatingSystemSwvVersion"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.OperatingSystemSwvVersion },
                ["operatingSystemSwVersionPatchLevel"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.OperatingSystemSwVersionPatchLevel },
                ["systemNameDns"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SystemNameDns },
                ["systemNameManagementIpAddress"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SystemNameManagementIpAddress },
                ["systemNameNetBios"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SystemNameNetBios },
                ["systemNameHostName"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SystemNameHostName },
                ["dateAssetMovedToLiveStatus"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.DateAssetMovedToLiveStatus },
                ["dateAssetDecommissioned"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.DateAssetDecommissioned },
                ["dependantSystemSoftware"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.DependantSystemSoftware },
                ["resilienceModel"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ResilienceModel },
                ["geographicSiteResilience"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.GeographicSiteResilience },
                ["localSiteResilience"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LocalSiteResilience },
                ["nameOfProductsDependantOnAsset"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.NameOfProductsDependantOnAsset },
                ["customer"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.Customer },
                ["privilegedAccessLogging"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.PrivilegedAccessLogging },
                ["boardOrModuleNameComponentName"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.BoardOrModuleNameComponentName },
                ["boardOrModuleTypeComponentSubtype"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.BoardOrModuleTypeComponentSubtype },
                ["boardOrModuleTypeComponentVersionNumber"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.BoardOrModuleTypeComponentVersionNumber },
                ["exposedEdge"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ExposedEdge },
                ["externallyFacingSystem"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ExternallyFacingSystem },
                ["managementPlane"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ManagementPlane },
                ["networkOverSightFunction"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.NetworkOverSightFunction },
                ["pecn"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.Pecn },
                ["pecs"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.Pecs },
                ["securityCriticalFunction"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SecurityCriticalFunction },
                ["critical"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.Critical },
                ["serialNumberTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SerialNumber },
                ["partNumber"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.PartNumber },
                ["prodOrLab"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ProdOrLab },
                ["localMarketOwnerShip"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LocalMarketOwnerShip },
                ["assuranceCall"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.AssuranceCall },
                ["serviceLevel"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ServiceLevel },
                ["lastPenTestDateTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LastPenTestDate },
                ["lastPenTestRefNo"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LastPenTestRefNo },
                ["piData"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.PiData },
                ["encryptedPiData"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.EncryptedPiData },
                ["lastModifiedValue"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CreationDate },
                ["resourceKey"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ResourceKey },
            };
        }


        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter, PassThroughQueryDto buildFilterDto)
        {
            var predicateResult = ApplyFilter(buildFilterDto);

            var query = GetQuery(predicateResult, false);

            var rtn = propertyName switch
            {

                "vodafoneUniqueIdentifier" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VodafoneUniqueIdentifier.ToString(), Value = p.VodafoneUniqueIdentifier.ToString() }).Distinct().ToList()
                       : query
                               .Where(x => x.VodafoneUniqueIdentifier.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VodafoneUniqueIdentifier.ToString(), Value = p.VodafoneUniqueIdentifier.ToString() }).Distinct()
                              .ToList(),
                "assetName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssetName, Value = p.AssetName }).Distinct().ToList()
                       : query
                               .Where(x => x.AssetName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssetName, Value = p.AssetName }).Distinct()
                              .ToList(),
                "resourceKey" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ResourceKey, Value = p.ResourceKey }).Distinct().ToList()
                       : query
                               .Where(x => x.ResourceKey.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKey, Value = p.ResourceKey }).Distinct()
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
                "critical" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Critical, Value = p.Critical }).Distinct().ToList()
                       : query
                               .Where(x => x.Critical.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Critical, Value = p.Critical }).Distinct()
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
                "assuranceCall" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.AssuranceCall, Value = p.AssuranceCall }).Distinct().ToList()
                       : query
                               .Where(x => x.AssuranceCall.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.AssuranceCall, Value = p.AssuranceCall }).Distinct()
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
                "hardwareTypeofHardwareAsset" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.HardwareTypeofHardwareAsset, Value = p.HardwareTypeofHardwareAsset }).Distinct().ToList()
                       : query
                               .Where(x => x.HardwareTypeofHardwareAsset.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.HardwareTypeofHardwareAsset, Value = p.HardwareTypeofHardwareAsset }).Distinct()
                              .ToList(),
                "hwEndofSale" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.HwEndofSale, Value = p.HwEndofSale }).Distinct().ToList()
                       : query
                               .Where(x => x.HwEndofSale.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.HwEndofSale, Value = p.HwEndofSale }).Distinct()
                              .ToList(),
                "softwareProductType" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SoftwareProductType, Value = p.SoftwareProductType }).Distinct().ToList()
                       : query
                               .Where(x => x.SoftwareProductType.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SoftwareProductType, Value = p.SoftwareProductType }).Distinct()
                              .ToList(),
                "applicationHostedonSoftware" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.ApplicationHostedonSoftware, Value = p.ApplicationHostedonSoftware }).Distinct().ToList()
                       : query
                               .Where(x => x.ApplicationHostedonSoftware.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ApplicationHostedonSoftware, Value = p.ApplicationHostedonSoftware }).Distinct()
                              .ToList(),
                "uuidorSerialNumberofSoftware" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.UuidorSerialNumberofSoftware, Value = p.UuidorSerialNumberofSoftware }).Distinct().ToList()
                       : query
                               .Where(x => x.UuidorSerialNumberofSoftware.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.UuidorSerialNumberofSoftware, Value = p.UuidorSerialNumberofSoftware }).Distinct()
                              .ToList(),
                "application" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Application, Value = p.Application }).Distinct().ToList()
                       : query
                               .Where(x => x.Application.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Application, Value = p.Application }).Distinct()
                              .ToList(),
                "physicalServerHostName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerHostName, Value = p.PhysicalServerHostName }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerHostName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerHostName, Value = p.PhysicalServerHostName }).Distinct()
                              .ToList(),
                "physicalServerIpaddress" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerIpaddress, Value = p.PhysicalServerIpaddress }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerIpaddress.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerIpaddress, Value = p.PhysicalServerIpaddress }).Distinct()
                              .ToList(),
                "physicalServerSerialNumber" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerSerialNumber, Value = p.PhysicalServerSerialNumber }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerSerialNumber.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerSerialNumber, Value = p.PhysicalServerSerialNumber }).Distinct()
                              .ToList(),
                "physicalServerHwModel" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerHwModel, Value = p.PhysicalServerHwModel }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerHwModel.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerHwModel, Value = p.PhysicalServerHwModel }).Distinct()
                              .ToList(),
                "physicalServerVendor" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerVendor, Value = p.PhysicalServerVendor }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerVendor.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerVendor, Value = p.PhysicalServerVendor }).Distinct()
                              .ToList(),
                "virtualServerHostedon" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VirtualServerHostedon, Value = p.VirtualServerHostedon }).Distinct().ToList()
                       : query
                               .Where(x => x.VirtualServerHostedon.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VirtualServerHostedon, Value = p.VirtualServerHostedon }).Distinct()
                              .ToList(),
                "virtualServerManufacturer" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VirtualServerManufacturer, Value = p.VirtualServerManufacturer }).Distinct().ToList()
                       : query
                               .Where(x => x.VirtualServerManufacturer.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VirtualServerManufacturer, Value = p.VirtualServerManufacturer }).Distinct()
                              .ToList(),
                "virtualServerTypeofDevice" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VirtualServerTypeofDevice, Value = p.VirtualServerTypeofDevice }).Distinct().ToList()
                       : query
                               .Where(x => x.VirtualServerTypeofDevice.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VirtualServerTypeofDevice, Value = p.VirtualServerTypeofDevice }).Distinct()
                              .ToList(),
                "virtualServerType" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VirtualServerType, Value = p.VirtualServerType }).Distinct().ToList()
                       : query
                               .Where(x => x.VirtualServerType.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VirtualServerType, Value = p.VirtualServerType }).Distinct()
                              .ToList(),
                "virtualServerSerialNumber" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VirtualServerSerialNumber, Value = p.VirtualServerSerialNumber }).Distinct().ToList()
                       : query
                               .Where(x => x.VirtualServerSerialNumber.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VirtualServerSerialNumber, Value = p.VirtualServerSerialNumber }).Distinct()
                              .ToList(),
                "virtualMachineType" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.VirtualMachineType, Value = p.VirtualMachineType }).Distinct().ToList()
                       : query
                               .Where(x => x.VirtualMachineType.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.VirtualMachineType, Value = p.VirtualMachineType }).Distinct()
                              .ToList(),
                "osStartDate" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.OsStartDate, Value = p.OsStartDate }).Distinct().ToList()
                       : query
                               .Where(x => x.OsStartDate.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.OsStartDate, Value = p.OsStartDate }).Distinct()
                              .ToList(),
                "osInstallationDate" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.OsInstallationDate, Value = p.OsInstallationDate }).Distinct().ToList()
                       : query
                               .Where(x => x.OsInstallationDate.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.OsInstallationDate, Value = p.OsInstallationDate }).Distinct()
                              .ToList(),
                "osStatus" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.OsStatus, Value = p.OsStatus }).Distinct().ToList()
                       : query
                               .Where(x => x.OsStatus.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.OsStatus, Value = p.OsStatus }).Distinct()
                              .ToList(),
                "softwareName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SoftwareName, Value = p.SoftwareName }).Distinct().ToList()
                       : query
                               .Where(x => x.SoftwareName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SoftwareName, Value = p.SoftwareName }).Distinct()
                              .ToList(),
                "version" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Version, Value = p.Version }).Distinct().ToList()
                       : query
                               .Where(x => x.Version.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Version, Value = p.Version }).Distinct()
                              .ToList(),
                "release" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Release, Value = p.Release }).Distinct().ToList()
                       : query
                               .Where(x => x.Release.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Release, Value = p.Release }).Distinct()
                              .ToList(),
                "language" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Language, Value = p.Language }).Distinct().ToList()
                       : query
                               .Where(x => x.Language.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Language, Value = p.Language }).Distinct()
                              .ToList(),
                "hwOpsContractStatus" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.HwOpsContractStatus, Value = p.HwOpsContractStatus }).Distinct().ToList()
                       : query
                               .Where(x => x.HwOpsContractStatus.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.HwOpsContractStatus, Value = p.HwOpsContractStatus }).Distinct()
                              .ToList(),
                "swOpsContractStatus" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.SwOpsContractStatus, Value = p.SwOpsContractStatus }).Distinct().ToList()
                       : query
                               .Where(x => x.SwOpsContractStatus.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.SwOpsContractStatus, Value = p.SwOpsContractStatus }).Distinct()
                              .ToList(),
                "plannedHwModel" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PlannedHwModel, Value = p.PlannedHwModel }).Distinct().ToList()
                       : query
                               .Where(x => x.PlannedHwModel.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PlannedHwModel, Value = p.PlannedHwModel }).Distinct()
                              .ToList(),
                "locationOrDatacentre" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.LocationOrDatacentre, Value = p.LocationOrDatacentre }).Distinct().ToList()
                       : query
                               .Where(x => x.LocationOrDatacentre.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.LocationOrDatacentre, Value = p.LocationOrDatacentre }).Distinct()
                              .ToList(),
                "physicalServerOSName" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerOSName, Value = p.PhysicalServerOSName }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerOSName.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerOSName, Value = p.PhysicalServerOSName }).Distinct()
                              .ToList(),
                "physicalOSVersion" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalOSVersion, Value = p.PhysicalOSVersion }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalOSVersion.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalOSVersion, Value = p.PhysicalOSVersion }).Distinct()
                              .ToList(),
                "physicalServerOSStartDate" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerOSStartDate, Value = p.PhysicalServerOSStartDate }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerOSStartDate.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerOSStartDate, Value = p.PhysicalServerOSStartDate }).Distinct()
                              .ToList(),
                "physicalServerOSinstallationdate" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerOSinstallationdate, Value = p.PhysicalServerOSinstallationdate }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerOSinstallationdate.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerOSinstallationdate, Value = p.PhysicalServerOSinstallationdate }).Distinct()
                              .ToList(),
                "physicalServerOSstatus" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.PhysicalServerOSstatus, Value = p.PhysicalServerOSstatus }).Distinct().ToList()
                       : query
                               .Where(x => x.PhysicalServerOSstatus.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.PhysicalServerOSstatus, Value = p.PhysicalServerOSstatus }).Distinct()
                              .ToList(),
                "cloud" => string.IsNullOrEmpty(propertyFilter)
                       ? query.Select(p => new FilterValueDto
                       { Text = p.Cloud, Value = p.Cloud }).Distinct().ToList()
                       : query
                               .Where(x => x.Cloud.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.Cloud, Value = p.Cloud }).Distinct()
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


                _ => new List<FilterValueDto>()
            };

            return rtn;
        }

        #endregion


        public async Task<Dictionary<long,string>> GetOtherDomainNames()
        {
            var otherDomains = await _repositoryWrapper.AppConfigurationSettingsRepository.FindByCondition(x => x.Appsettingid == 2).OrderBy(x => x.Settingsvalue).ToDictionaryAsync( x => x.Appconfigurationsettingid, x => x.Settingsvalue);

            return otherDomains;
        }
        public async Task<string> GetDomainName(long nonTemsVerticalId)
        {
            return await _repositoryWrapper.AppConfigurationSettingsRepository.FindByCondition(x => x.Appsettingid == 2 && x.Appconfigurationsettingid == nonTemsVerticalId).Select(x=>x.Settingsvalue).FirstOrDefaultAsync();
        }

    }
}
