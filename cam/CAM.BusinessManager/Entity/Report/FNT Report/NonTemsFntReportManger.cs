using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Enums;
using CAM.BusinessManager.GenericReports;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.BusinessManager.Rules;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.DataTransferObjects.Entita.GenericReportDto;
using CAM.DataTransferObjects.Entita.AssetPassThrough;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.BPT;
using CAM.DataTransferObjects.QueryDto.FNT;
using CAM.Entities.Mappers.Cbom;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models.FNT;
using CAM.Entities.Models.Lookup;
using CAM.Entities.Models.PassThroughData;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using CAM.Repository;
using DocumentFormat.OpenXml.ExtendedProperties;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.FNT_Report
{
    public class NonTemsFntReportManger : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _gridmanager;
        private readonly IMapper _mapper;
        private CommonManager _commonManager;

        public NonTemsFntReportManger(IEnumerable<IRepositoryWrapper> wrappers, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper
            , GridCustomColumnManager manager, IMapper mapper, CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _gridmanager = manager;
            _mapper = mapper;
            _commonManager = commonManager;
        }

        public async Task<QueryResultDto<NonTemsFntReportDtoGrid>> FindWithCondition(NonTemsFntReportQueryDto fntQueryDto)
        {
            var predicateResult = ApplyFilter(fntQueryDto); 
            var rtn = new QueryResultDto<NonTemsFntReportDtoGrid>(new GenerateRenderForGrid<NonTemsFntReportDtoGrid>(_gridmanager)) 
            {
                TotalItems = predicateResult.IsStarted ? _repositoryWrapper.PassThroughRepository.Count(predicateResult) : _repositoryWrapper.PassThroughRepository.Count(),
            };
            var query = GetQuery(predicateResult).ApplyOrdering(fntQueryDto, GetColumnsMap()).ApplyPaging(fntQueryDto);
            var data = query.ToList();
            
            IEnumerable<NonTemsFntReportDtoGrid> fntDtoGrid;
            
            fntDtoGrid = _mapper.Map<IEnumerable<NonTemsFntReportDtoGrid>>(data);

            rtn.Items = fntDtoGrid.ToArray();
            return rtn;
        }
        private static ExpressionStarter<Assetpassthrough> ApplyFilter(NonTemsFntReportQueryDto fntQueryDto)
        {
            var predicateResult = PredicateBuilder.New<Assetpassthrough>();
            var predicateInner = PredicateBuilder.New<Assetpassthrough>();
            if (fntQueryDto.NonTemsVertical != null && fntQueryDto.NonTemsVertical.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.NonTemsVertical)
                    predicateInner.Or(x => x.Nontemsvertical == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.PassThroughId != null && fntQueryDto.PassThroughId.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.PassThroughId)
                    predicateInner.Or(x => x.Passthroughid == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HostName != null && fntQueryDto.HostName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HostName)
                    predicateInner.Or(x => x.Assetname == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SerialNumberOfHardwareAsset != null && fntQueryDto.SerialNumberOfHardwareAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SerialNumberOfHardwareAsset)
                    predicateInner.Or(x => x.Serialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.LocationOfHardwareAsset != null && fntQueryDto.LocationOfHardwareAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.LocationOfHardwareAsset)
                    predicateInner.Or(x => x.Countrywhereassetislocated == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HardwareTypeOfHardwareAsset != null && fntQueryDto.HardwareTypeOfHardwareAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HardwareTypeOfHardwareAsset)
                    predicateInner.Or(x => x.Hardwaretypeofhardwareasset == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Vendor != null && fntQueryDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Vendor)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.IpAddressOfHardwareAsset != null && fntQueryDto.IpAddressOfHardwareAsset.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.IpAddressOfHardwareAsset)
                    predicateInner.Or(x => x.Systemnamemanagementipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Market != null && fntQueryDto.Market.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Market)
                    predicateInner.Or(x => x.Localmarketownership == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HwEndOfLife != null && fntQueryDto.HwEndOfLife.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HwEndOfLife)
                    predicateInner.Or(x => x.Maintenancehardwareendofsupportdate == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HwEndOfSupport != null && fntQueryDto.HwEndOfSupport.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HwEndOfSupport)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HwEndOfSale != null && fntQueryDto.HwEndOfSale.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HwEndOfSale)
                    predicateInner.Or(x => x.Hwendofsale == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HardwareModules != null && fntQueryDto.HardwareModules.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HardwareModules)
                    predicateInner.Or(x => x.Boardormodulenamecomponentname == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SoftwareProductType != null && fntQueryDto.SoftwareProductType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SoftwareProductType)
                    predicateInner.Or(x => x.Softwareproducttype == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SoftwareProductVersion != null && fntQueryDto.SoftwareProductVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SoftwareProductVersion)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SoftwareIsVirtualized != null && fntQueryDto.SoftwareIsVirtualized.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SoftwareIsVirtualized)
                    predicateInner.Or(x => x.Cloudhostedasset == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OperatingSystemOfVirtualMachine != null && fntQueryDto.OperatingSystemOfVirtualMachine.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OperatingSystemOfVirtualMachine)
                    predicateInner.Or(x => x.Operatingsystemname == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.ApplicationHostedOnSoftware != null && fntQueryDto.ApplicationHostedOnSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.ApplicationHostedOnSoftware)
                    predicateInner.Or(x => x.Applicationhostedonsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.UuidSerialNumberOfSoftware != null && fntQueryDto.UuidSerialNumberOfSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.UuidSerialNumberOfSoftware)
                    predicateInner.Or(x => x.Uuidorserialnumberofsoftware == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SoftwareVendor != null && fntQueryDto.SoftwareVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SoftwareVendor)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.LocationOfSoftware != null && fntQueryDto.LocationOfSoftware.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.LocationOfSoftware)
                    predicateInner.Or(x => x.Geolocation == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.ServiceType != null && fntQueryDto.ServiceType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.ServiceType)
                    predicateInner.Or(x => x.Assetfunction == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SwEndOfLife != null && fntQueryDto.SwEndOfLife.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SwEndOfLife)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SwEndOfSupport != null && fntQueryDto.SwEndOfSupport.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SwEndOfSupport)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SwEndOfSale != null && fntQueryDto.SwEndOfSale.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SwEndOfSale)
                    predicateInner.Or(x => x.Swendofsale == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VerticalEngineeringTeam != null && fntQueryDto.VerticalEngineeringTeam.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VerticalEngineeringTeam)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VerticalSubdomain != null && fntQueryDto.VerticalSubdomain.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VerticalSubdomain)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Platform != null && fntQueryDto.Platform.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Platform)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.RiskCluster != null && fntQueryDto.RiskCluster.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.RiskCluster)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OperationsContactPoint != null && fntQueryDto.OperationsContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OperationsContactPoint)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.AssetCategory != null && fntQueryDto.AssetCategory.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.AssetCategory)
                    predicateInner.Or(x => x.Assetfunction == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.AssetClass != null && fntQueryDto.AssetClass.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.AssetClass)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.AssetType != null && fntQueryDto.AssetType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.AssetType)
                    predicateInner.Or(x => x.Assettype == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.AssetDescription != null && fntQueryDto.AssetDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.AssetDescription)
                    predicateInner.Or(x => x.Assetdescriptionorpurpose == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.ProductImportance != null && fntQueryDto.ProductImportance.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.ProductImportance)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OperationsMaintenanceContract != null && fntQueryDto.OperationsMaintenanceContract.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OperationsMaintenanceContract)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VendorEndOfMaintenanceDate != null && fntQueryDto.VendorEndOfMaintenanceDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VendorEndOfMaintenanceDate)
                    predicateInner.Or(x => x.VendorendofmaintenanceDate == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.IdentifiedAction != null && fntQueryDto.IdentifiedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.IdentifiedAction)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.DescriptionOfPlannedAction != null && fntQueryDto.DescriptionOfPlannedAction.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.DescriptionOfPlannedAction)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            //Plannedhw model
            if (fntQueryDto.BusinessServiceName != null && fntQueryDto.BusinessServiceName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.BusinessServiceName)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OpMaintenanceContractendDate != null && fntQueryDto.OpMaintenanceContractendDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OpMaintenanceContractendDate)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.IncidentClass != null && fntQueryDto.IncidentClass.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.IncidentClass)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OccurrenceProbability != null && fntQueryDto.OccurrenceProbability.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OccurrenceProbability)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.MeverticalResposible != null && fntQueryDto.MeverticalResposible.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.MeverticalResposible)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.AssetStatus != null && fntQueryDto.AssetStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.AssetStatus)
                    predicateInner.Or(x => x.Deploymentorlifecyclestatus == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.TypeOfNetworkElement != null && fntQueryDto.TypeOfNetworkElement.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.TypeOfNetworkElement)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.LocalMarket != null && fntQueryDto.LocalMarket.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.LocalMarket)
                    predicateInner.Or(x => x.Localmarketownership == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Application != null && fntQueryDto.Application.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Application)
                    predicateInner.Or(x => x.Application == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Cloud != null && fntQueryDto.Cloud.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Cloud)
                    predicateInner.Or(x => x.Cloudhostedasset == item);
                predicateResult.And(predicateInner);
            }
            //if (fntQueryDto.DataCenterocation != null && fntQueryDto.DataCenterocation.Any())
            //{
            //    predicateInner = PredicateBuilder.New<Assetpassthrough>();
            //    foreach (var item in fntQueryDto.DataCenterocation)
            //        predicateInner.Or(x => x.Geolocation == item);
            //    predicateResult.And(predicateInner);
            //}
            if (fntQueryDto.PhysicalServerHostname != null && fntQueryDto.PhysicalServerHostname.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.PhysicalServerHostname)
                    predicateInner.Or(x => x.Physicalserverhostname == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.PhysicalServerIpaddress != null && fntQueryDto.PhysicalServerIpaddress.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.PhysicalServerIpaddress)
                    predicateInner.Or(x => x.Physicalserveripaddress == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.PhysicalServerSerialNumber != null && fntQueryDto.PhysicalServerSerialNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.PhysicalServerSerialNumber)
                    predicateInner.Or(x => x.Physicalserverserialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.PhysicalServerHwModel != null && fntQueryDto.PhysicalServerHwModel.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.PhysicalServerHwModel)
                    predicateInner.Or(x => x.PhysicalserverhwModel == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.PhysicalServerVendor != null && fntQueryDto.PhysicalServerVendor.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.PhysicalServerVendor)
                    predicateInner.Or(x => x.Physicalservervendor == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VirtualServerHostedOn != null && fntQueryDto.VirtualServerHostedOn.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VirtualServerHostedOn)
                    predicateInner.Or(x => x.Virtualserverhostedon == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VirtualServerManufacturer != null && fntQueryDto.VirtualServerManufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VirtualServerManufacturer)
                    predicateInner.Or(x => x.Virtualservermanufacturer == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VirtualServerTypeOfDevice != null && fntQueryDto.VirtualServerTypeOfDevice.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VirtualServerTypeOfDevice)
                    predicateInner.Or(x => x.Virtualservertypeofdevice == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VirtualMachineType != null && fntQueryDto.VirtualMachineType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VirtualMachineType)
                    predicateInner.Or(x => x.Virtualmachinetype == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VirtualServerIpaddress != null && fntQueryDto.VirtualServerIpaddress.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VirtualServerIpaddress)
                    predicateInner.Or(x => x.Systemnamemanagementipaddress == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VirtualServerSerialNumber != null && fntQueryDto.VirtualServerSerialNumber.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VirtualServerSerialNumber)
                    predicateInner.Or(x => x.Virtualserverserialnumber == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.VirtualServerType != null && fntQueryDto.VirtualServerType.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.VirtualServerType)
                    predicateInner.Or(x => x.Virtualservertype == item);
                predicateResult.And(predicateInner);
            }

            if (fntQueryDto.OsName != null && fntQueryDto.OsName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OsName)
                    predicateInner.Or(x => x.Operatingsystemname == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OsVersion != null && fntQueryDto.OsVersion.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OsVersion)
                    predicateInner.Or(x => x.Operatingsystemswvversion == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OsStartDate != null && fntQueryDto.OsStartDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OsStartDate)
                    predicateInner.Or(x => x.Osstartdate == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OsInstallationDate != null && fntQueryDto.OsInstallationDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OsInstallationDate)
                    predicateInner.Or(x => x.Osinstallationdate == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.OsStatus != null && fntQueryDto.OsStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.OsStatus)
                    predicateInner.Or(x => x.Osstatus == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SoftwareName != null && fntQueryDto.SoftwareName.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SoftwareName)
                    predicateInner.Or(x => x.Softwarename == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Version != null && fntQueryDto.Version.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Version)
                    predicateInner.Or(x => x.Version == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Release != null && fntQueryDto.Release.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Release)
                    predicateInner.Or(x => x.Release == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Manufacturer != null && fntQueryDto.Manufacturer.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Manufacturer)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.Language != null && fntQueryDto.Language.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.Language)
                    predicateInner.Or(x => x.Language == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HwOpsContractEndDate != null && fntQueryDto.HwOpsContractEndDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HwOpsContractEndDate)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HwOpsContractStatus != null && fntQueryDto.HwOpsContractStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HwOpsContractStatus)
                    predicateInner.Or(x => x.Hwopscontractstatus == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.HwOperationsContactPoint != null && fntQueryDto.HwOperationsContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.HwOperationsContactPoint)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SwOperationsContactPoint != null && fntQueryDto.SwOperationsContactPoint.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SwOperationsContactPoint)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SwOpsContractEndDate != null && fntQueryDto.SwOpsContractEndDate.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SwOpsContractEndDate)
                    predicateInner.Or(x => x.Vodafoneuniqueidentifier.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (fntQueryDto.SwOpsContractStatus != null && fntQueryDto.SwOpsContractStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Assetpassthrough>();
                foreach (var item in fntQueryDto.SwOpsContractStatus)
                    predicateInner.Or(x => x.Swopscontractstatus == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;

        }


        private IQueryable<AssetPassThrough> GetQuery(ExpressionStarter<Assetpassthrough> predicateResult)
        {
            var query = predicateResult.IsStarted
                ? _repositoryWrapper.PassThroughRepository.FindByCondition(predicateResult)
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.ResourcekeyNavigation)
               : _repositoryWrapper.PassThroughRepository.FindAll()
                .Include(x => x.CreationuserNavigation)
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.ResourcekeyNavigation);
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
                ["softwareVendorName"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["model"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
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
                ["hardwareVendorName"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["maintenanceSupportSupplierSecond"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.MaintenanceSupportSupplierSecond },
                ["vendorSoftwareEndOfSupportDate"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["maintenanceSoftwareEndOfSupportDate"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["dependantSystemSoftware"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.DependantSystemSoftware },
                ["resilienceModel"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ResilienceModel },
                ["geographicSiteResilience"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.GeographicSiteResilience },
                ["localSiteResilience"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LocalSiteResilience },
                ["nameOfProductsDependantOnAsset"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.NameOfProductsDependantOnAsset },
                ["technicalServiceNames"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
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
                ["productImportanceTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["critical"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.Critical },
                ["criticalitytype"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["serialNumberTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.SerialNumber },
                ["partNumber"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.PartNumber },
                ["descriptionOfPlannedaction"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["identifiedActionTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["lastUpgradeDate"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["prodOrLab"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ProdOrLab },
                ["localMarketOwnerShip"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LocalMarketOwnerShip },
                ["budgetEstimatedTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["bundleBudgetTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["assuranceCall"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.AssuranceCall },
                ["commentOnProjectStatusTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["projectEndDateTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["projectStatusTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.VodafoneUniqueIdentifier },
                ["serviceLevel"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ServiceLevel },
                ["lastPenTestDateTsr"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LastPenTestDate },
                ["lastPenTestRefNo"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.LastPenTestRefNo },
                ["piData"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.PiData },
                ["encryptedPiData"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.EncryptedPiData },
                ["lastModifiedValue"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ModificationDate },
                ["lastModifiedBy"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.ModificationUserEntity.Email },
                ["creationUser"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CreationUser },
                ["creationDate"] = new Expression<Func<AssetPassThrough, object>>[] { p => p.CreationDate },
            };
        }

        public async Task<List<FilterValueDto>> GetFilteredValues(string propertyName, string propertyFilter, NonTemsFntReportQueryDto fntQueryDto)
        {
            var filterCriteria = ApplyFilter(fntQueryDto);

            var filteredQuery = await Task.Run(() => GetQuery(filterCriteria).AsQueryable());

            var result = propertyName switch
            {
                #region
                
                "hostName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AssetName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.AssetName))
                                    .Distinct()
                                    .ToList(),
                "serialNumberOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SerialNumber.ToString().Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.SerialNumber))
                                    .Distinct()
                                    .ToList(),
                "locationOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CountryWhereAssetIsLocated.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.CountryWhereAssetIsLocated))
                                    .Distinct()
                                    .ToList(),
                "hardwareTypeOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HardwareTypeofHardwareAsset.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.HardwareTypeofHardwareAsset))
                                    .Distinct()
                                    .ToList(),
                "vendor" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwvendor, Value = p.VodafoneUniqueIdentifier.ToString() }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.VodafoneUniqueIdentifier.ToString().Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwvendor, Value = p.VodafoneUniqueIdentifier.ToString() }).Distinct()
                              .ToList(),
                "ipAddressOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SystemNameManagementIpAddress.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.SystemNameManagementIpAddress))
                                    .Distinct()
                                    .ToList(),
                "market" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.LocalMarketOwnerShip.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.LocalMarketOwnerShip))
                                    .Distinct()
                                    .ToList(),
                "hwEndOfLife" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.MaintenanceHardwareEndOfSupportDate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.MaintenanceHardwareEndOfSupportDate))
                                    .Distinct()
                                    .ToList(),
                "hwEndOfSupport" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VendorHardwareEndOfSupportDate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VendorHardwareEndOfSupportDate))
                                    .Distinct()
                                    .ToList(),
                "hwEndOfSale" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HwEndofSale.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.HwEndofSale))
                                    .Distinct()
                                    .ToList(),
                "hardwareModules" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.BoardOrModuleNameComponentName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.BoardOrModuleNameComponentName))
                                    .Distinct()
                                    .ToList(),
                "softwareProductType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SoftwareProductType.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.SoftwareProductType))
                                    .Distinct()
                                    .ToList(),
                "softwareProductVersion" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = p.ResourceKeyNavigation.SoftwareVersion, Value = p.ResourceKeyNavigation.SoftwareVersion }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.SoftwareVersion.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKeyNavigation.SoftwareVersion, Value = p.ResourceKeyNavigation.SoftwareVersion }).Distinct()
                              .ToList(),
                "softwareIsVirtualized" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CloudHostedAsset.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.CloudHostedAsset))
                                    .Distinct()
                                    .ToList(),
                "operatingSystemOfVirtualMachine" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OperatingSystemName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.OperatingSystemName))
                                    .Distinct()
                                    .ToList(),
                "applicationHostedOnSoftware" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ApplicationHostedonSoftware.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.ApplicationHostedonSoftware))
                                    .Distinct()
                                    .ToList(),
                "uuidSerialNumberOfSoftware" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.UuidorSerialNumberofSoftware.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.UuidorSerialNumberofSoftware))
                                    .Distinct()
                                    .ToList(),
                "softwareVendor" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = p.ResourceKeyNavigation.SWVendor, Value = p.ResourceKeyNavigation.SWVendor }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.SWVendor.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKeyNavigation.SWVendor, Value = p.ResourceKeyNavigation.SWVendor }).Distinct()
                              .ToList(),
                "locationOfSoftware" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Geolocation.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Geolocation))
                                    .Distinct()
                                    .ToList(),
                "serviceType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AssetFunction.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.AssetFunction))
                                    .Distinct()
                                    .ToList(),
                "swEndOfLife" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.MaintenanceSoftwareEndofSupportDate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.MaintenanceSoftwareEndofSupportDate))
                                    .Distinct()
                                    .ToList(),
                "swEndOfSupport" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = p.ResourceKeyNavigation.VendorEndOfVulnerabilitySecuritySupportDate, Value = p.ResourceKeyNavigation.VendorEndOfVulnerabilitySecuritySupportDate }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.VendorEndOfVulnerabilitySecuritySupportDate.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKeyNavigation.VendorEndOfVulnerabilitySecuritySupportDate, Value = p.ResourceKeyNavigation.VendorEndOfVulnerabilitySecuritySupportDate }).Distinct()
                              .ToList(),
                "swEndOfSale" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SwEndofSale.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.SwEndofSale))
                                    .Distinct()
                                    .ToList(),
                "verticalEngineeringTeam" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = p.ResourceKeyNavigation.VerticalEngineeringTeam, Value = p.ResourceKeyNavigation.VerticalEngineeringTeam }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.VerticalEngineeringTeam.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKeyNavigation.VerticalEngineeringTeam, Value = p.ResourceKeyNavigation.VerticalEngineeringTeam }).Distinct()
                              .ToList(),
                "verticalSubdomain" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = p.ResourceKeyNavigation.VerticalSubDomain, Value = p.ResourceKeyNavigation.VerticalSubDomain }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.VerticalSubDomain.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKeyNavigation.VerticalSubDomain, Value = p.ResourceKeyNavigation.VerticalSubDomain }).Distinct()
                              .ToList(),
                "platform" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = p.ResourceKeyNavigation.Platform, Value = p.ResourceKeyNavigation.Platform }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.Platform.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKeyNavigation.Platform, Value = p.ResourceKeyNavigation.Platform }).Distinct()
                              .ToList(),
                "riskCluster" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text = p.ResourceKeyNavigation.RiskCluster, Value = p.ResourceKeyNavigation.RiskCluster }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.RiskCluster.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text = p.ResourceKeyNavigation.RiskCluster, Value = p.ResourceKeyNavigation.RiskCluster }).Distinct()
                              .ToList(),
                "assetCategory" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AssetFunction.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.AssetFunction))
                                    .Distinct()
                                    .ToList(),
                "assetClass" => string.IsNullOrEmpty(propertyFilter)
                       ? filteredQuery.Select(p => new FilterValueDto
                       { Text =  p.ResourceKeyNavigation.AssetClass, Value = p.ResourceKeyNavigation.AssetClass }).Distinct().ToList()
                       : filteredQuery
                               .Where(x => x.ResourceKeyNavigation.AssetClass.Contains(propertyFilter)).Select(p =>
                               new FilterValueDto { Text =  p.ResourceKeyNavigation.AssetClass, Value = p.ResourceKeyNavigation.AssetClass }).Distinct()
                              .ToList(),
                "assetType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AssetType.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.AssetType))
                                    .Distinct()
                                    .ToList(),
                "assetDescription" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AssetDescriptionOrPurpose.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.AssetDescriptionOrPurpose))
                                    .Distinct()
                                    .ToList(),
                //"productImportance" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Productimportance, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Productimportance, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                //"vendorEndOfMaintenanceDate" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwvendorendofmaintenancedate, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwvendorendofmaintenancedate, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                //"identifiedAction" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Identifiedaction, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Identifiedaction, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                //"descriptionOfPlannedAction" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Descriptionofplannedaction, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Descriptionofplannedaction, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                //"plannedHwModel" => filteredQuery
                //                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Plannedhwmodel.Contains(propertyFilter))
                //                    .Select(x => new FilterValueDto(x.Plannedhwmodel))
                //                    .Distinct()
                //                    .ToList(),
                //"businessServiceName" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Busi, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwvendor, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                //"incidentClass" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Incidentclass, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Incidentclass, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                //"occurrenceProbability" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Occurrenceprobability, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Occurrenceprobability, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                //"meverticalResposible" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Verticalengineeringteam, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Verticalengineeringteam, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                "assetStatus" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.DeploymentOrLifeCycleStatus.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.DeploymentOrLifeCycleStatus))
                                    .Distinct()
                                    .ToList(),
                //"typeOfNetworkElement" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Typeofnetworkelement, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Typeofnetworkelement, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                "localMarket" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.LocalMarketOwnerShip.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.LocalMarketOwnerShip))
                                    .Distinct()
                                    .ToList(),
                "application" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Application.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Application))
                                    .Distinct()
                                    .ToList(),
                "cloud" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CloudHostedAsset.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.CloudHostedAsset))
                                    .Distinct()
                                    .ToList(),
                "dataCenterocation" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Geolocation.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Geolocation))
                                    .Distinct()
                                    .ToList(),
                "physicalServerHostname" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PhysicalServerHostName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PhysicalServerHostName))
                                    .Distinct()
                                    .ToList(),
                "physicalServerIpaddress" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PhysicalServerIpaddress.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PhysicalServerIpaddress))
                                    .Distinct()
                                    .ToList(),
                "physicalServerSerialNumber" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PhysicalServerSerialNumber.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PhysicalServerSerialNumber))
                                    .Distinct()
                                    .ToList(),
                "physicalServerHwModel" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PhysicalServerHwModel.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PhysicalServerHwModel))
                                    .Distinct()
                                    .ToList(),
                "physicalServerVendor" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PhysicalServerVendor.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PhysicalServerVendor))
                                    .Distinct()
                                    .ToList(),
                "virtualServerHostedOn" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VirtualServerHostedon.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VirtualServerHostedon))
                                    .Distinct()
                                    .ToList(),
                "virtualServerManufacturer" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VirtualServerManufacturer.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VirtualServerManufacturer))
                                    .Distinct()
                                    .ToList(),
                "virtualServerTypeOfDevice" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VirtualServerTypeofDevice.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VirtualServerTypeofDevice))
                                    .Distinct()
                                    .ToList(),
                "virtualMachineType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VirtualMachineType.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VirtualMachineType))
                                    .Distinct()
                                    .ToList(),
                //"virtualServerIpaddress" => filteredQuery
                //                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VIRTUALSERVER.Contains(propertyFilter))
                //                    .Select(x => new FilterValueDto(x.Virtualserveripaddress))
                //                    .Distinct()
                //                    .ToList(),
                "virtualServerSerialNumber" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VirtualServerSerialNumber.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VirtualServerSerialNumber))
                                    .Distinct()
                                    .ToList(),
                "virtualServerType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VirtualServerType.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VirtualServerType))
                                    .Distinct()
                                    .ToList(),
                "osName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OperatingSystemName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.OperatingSystemName))
                                    .Distinct()
                                    .ToList(),
                "osVersion" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OperatingSystemSwvVersion.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.OperatingSystemSwvVersion))
                                    .Distinct()
                                    .ToList(),
                "osStartDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OsStartDate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.OsStartDate))
                                    .Distinct()
                                    .ToList(),
                "osInstallationDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OsInstallationDate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.OsInstallationDate))
                                    .Distinct()
                                    .ToList(),
                "osStatus" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OsStatus.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.OsStatus))
                                    .Distinct()
                                    .ToList(),
                "softwareName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SoftwareName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.SoftwareName))
                                    .Distinct()
                                    .ToList(),
                "version" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Version.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Version))
                                    .Distinct()
                                    .ToList(),
                "release" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Release.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Release))
                                    .Distinct()
                                    .ToList(),
                //"manufacturer" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Swvendor, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Swvendor, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                "language" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Language.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Language))
                                    .Distinct()
                                    .ToList(),
                "creationUser" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.CreationUserEntity.Email.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.CreationUserEntity.Email))
                                    .Distinct()
                                    .ToList(),
                "modificationUser" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ModificationUserEntity.Email.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.ModificationUserEntity.Email))
                                    .Distinct()
                                    .ToList(),
               
                "hwOpsContractStatus" => filteredQuery
                                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HwOpsContractStatus.Contains(propertyFilter))
                                   .Select(x => new FilterValueDto(x.HwOpsContractStatus))
                                   .Distinct()
                                   .ToList(),
                //"hwOpsContractEndDate" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwopsmaintenancecontractenddate, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwopsmaintenancecontractenddate, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),
                "swOpsContractStatus" => filteredQuery
                                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SwOpsContractStatus.Contains(propertyFilter))
                                   .Select(x => new FilterValueDto(x.SwOpsContractStatus))
                                   .Distinct()
                                   .ToList(),
                //"swOpsContractEndDate" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Swopsmaintenanceconractenddate, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Swopsmaintenanceconractenddate, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),

                //"hwOperationsContactPoint" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwoperationscontactpoint, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Hwoperationscontactpoint, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),

                //"swOperationsContactPoint" => string.IsNullOrEmpty(propertyFilter)
                //       ? filteredQuery.Select(p => new FilterValueDto
                //       { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Swoperationscontactpoint, Value = p.VodafoneUniqueIdentifier }).Distinct().ToList()
                //       : filteredQuery
                //               .Where(x => x.VodafoneUniqueIdentifier.Contains(propertyFilter)).Select(p =>
                //               new FilterValueDto { Text = _commonManager.GetHwPassthroughlcmRecords(p.ResourceKeyNavigation.LocalMarket,p.ResourceKeyNavigation.AssetClass,p.ResourceKeyNavigation.HardwareModel).Result.Swoperationscontactpoint, Value = p.VodafoneUniqueIdentifier }).Distinct()
                //              .ToList(),

                "passThroughId" => filteredQuery
                                   .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PassThroughId.ToString().Contains(propertyFilter))
                                   .Select(x => new FilterValueDto(x.PassThroughId.ToString()))
                                   .Distinct()
                                   .ToList(),


                #endregion

                _ => new List<FilterValueDto>()
            };

            return result;
        }

    }
}
