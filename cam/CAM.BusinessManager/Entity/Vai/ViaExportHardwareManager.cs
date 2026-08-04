using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Entity.Vai;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.VIA;
using CAM.Enum;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class ViaExportHardwareManager:BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;
        private static CommonManager _commonManager;

        public ViaExportHardwareManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, 
            GridCustomColumnManager columnManager, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
            _commonManager = commonManager;
        }
        private static string GetSecurityTier(Designcomponentfamilies? model, int opcoId)
        {

            if (model != null && model.Designaspects.Any())
            {
                var item = model.Designaspects.FirstOrDefault(p => p.Opcoid == opcoId);
                if (item != null && item.Securitytirezone != null) return item.Securitytirezone.Description;
                return null;
            }
            return null;
        }
        private  static bool GetCNI(Designcomponentfamilies? model, int opcoId)
        {

            if (model != null && model.Designaspects.Any())
            {
                var item = model.Designaspects.FirstOrDefault(p => p.Opcoid == opcoId);
                if (item != null) return item.Criticalnationalinfrastructure;
                return false;
            }
            return false;
        }
        public QueryResultDtoVai FindWithCondition(ViaExportQuery buildFilterDto)
        {
            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyFilter(buildFilterDto);
            var query = GetQuery(predicateResult).ApplyOrdering(buildFilterDto, GetColumnsMap() , ConstantValueFilter.Modificationdate).OrderByDescending(p => p.Modificationdate).ToList().DistinctBy(x => x.Elementname);
            var data = query.AsQueryable().ApplyPagingViaExportStart(buildFilterDto).ToList();
            var reports = data.Select(x => { return ViaExport(x); }).ToList();
            var rtn = new QueryResultDtoVai(new GenerateRenderForGrid<ViaExport>(_columnManager));
            rtn.TotalItems = query.Count();
            rtn.Items = reports;

            return rtn;
        }
        static ViaExport ViaExport(Networkelementsasplanned x)
        {
            var grid = new ViaExport();
            grid.LocationName = x.Location?.Location;
            grid.MeStatus = ConstantValueFilter.Operational;
            grid.resourceKey = x.Hwresourcekey;
            grid.MeIpAddress = x.Additionalinformation1;
            grid.MeVirtualFlg = ConstantValueFilter.No;

            //grid.VerticalNameId = (int)x.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsibleid;
            grid.VerticalName = _commonManager.GetVerticaleNameRes( (x.Networkelementasplannedsubdomainspoc?.Select(x => x.Subdomainspocid).ToList()),x.Opcoid);

            grid.OrganisationName = x.Opco.Opco;
            grid.MeCriticality = x.Designcomponent?.Subnetworkboundary.Criticality;
            grid.MeLCMPolicy =( x.Designcomponent == null) ? "":  (((LCMPolicy)x.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString());
            grid.MeDeploymentType = x.Deploymenttype?.Deploymenttype;
            grid.MeEnvironment = x.Environment.Environment;
            grid.MeExternalConnectionFlg = (bool)x.Designcomponent.Subnetworkboundary.Internetfacing ? ConstantValueFilter.Yes : ConstantValueFilter.No;
            grid.MeFqdn = "";
            grid.MeGdprRelevantFlg = x.Designcomponent.Subnetworkboundary.Gdprrelevant == null ? ConstantValueFilter.Unspecified :
                x.Designcomponent.Subnetworkboundary.Gdprrelevant == true ? ConstantValueFilter.Yes : ConstantValueFilter.No;
            grid.MeServiceType = x.Designcomponent.Systemtype.Assetcategory.Assetcategory;
            grid.MeCyberarkIntegrationFlg = "";
            grid.MeIdmIntegrationFlg = "";
            grid.MeSecurityTier = GetSecurityTier(x.Designcomponent.Designcomponentfamily, x.Opco.Opcoid);
            grid.MeCniBcServiceFlg = GetCNI(x.Designcomponent.Designcomponentfamily, x.Opco.Opcoid) ? "Y" : "N";

            grid.Me2FaIntegrationFlg = "";
            grid.MeSiemIntegrationFlg = "";
            grid.MeMacAddress = "";
            grid.MeSerialNumber = "";
            grid.MeSourceAssetId = x.Designcomponent.Designcomponentfamily != null &&x.Designcomponent.Designcomponentfamily.Systemisshared
                ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardwareid +
                  "_" + x.Location?.Location + "_T1"
                : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardwareid +
                  "_" + $"TEMS{x.Networkelementasplannedid:000000}";
            grid.MeName = x.Designcomponent.Designcomponentfamily != null && x.Designcomponent.Designcomponentfamily.Systemisshared
                ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware
                    ?.Platform?.Platform + "_" + x.Location?.Location + "_T1"
                : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware
                    ?.Platform?.Platform + "_" + x.Elementname;
            grid.MeType = x.Designcomponent.Designcomponentfamily != null && x.Designcomponent.Designcomponentfamily.Systemisshared
                ? ConstantValueFilter.Platformsupportingmultipleapplications
                : string.Concat(ConstantValueFilter.Platformsupporting,
                x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description:"");
            grid.MeDescription = ConstantValueFilter.Hardwaresolutionsupporting +
                                 x.Designcomponent.Systemtype.Assetclass.Assetclass +
                                 "<b class=\"text-lowercase\"> on </b>" + x.Designcomponent.Systemtype
                                     .Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware?
                                     .Buildconstruction?.Buildconstruction;

            var hardware = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)
                ?.Majorhardware;

            grid.LcmProdName =$"{hardware?.Hardwaresolution} {hardware?.Platform?.Platform} {hardware?.Hardwaretype }" ;

            grid.Vendor = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)
                ?.Majorhardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer;
            grid.EoslContractDate = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware
                                    ?.Endofmaintenance != null
                                    ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware
                                        ?.Endofmaintenance?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                                    : "";

            grid.EOMStatus =(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware != null)
                             ? (EOMEnum) x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware.Eomstatus : EOMEnum.NotSpecified;



            grid.EeoslContractDate =
                x.Designcomponent.Lcmengineering.FirstOrDefault(s =>
                    s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid) != null
                    ? x.Designcomponent.Lcmengineering
                        .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        ?.Hardwareendofsupportcontract?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    : "";
            grid.OsName = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                .SingleOrDefault(m =>
                    m.Ismain &&
                    m.Deleted == false)?.Majorhardware?.Buildconstruction?.Rule != 3
                ? x.Additionalinformation2
                : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m =>
                        m.Ismain &&
                        m.Deleted == false)?.Majorhardware.Hardwaresolution;
            var majorHarwareBuild = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(m => m.Ismain && m.Deleted == false).Majorhardware;

            grid.OsEoslContractDate = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware
                    ?.Endofmaintenance != null
                    ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain)?.Majorhardware
                        ?.Endofmaintenance?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    : "";


            grid.OsEeoslContractDate = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                .SingleOrDefault(m =>
                    m.Ismain &&
                    m.Deleted == false)
                ?.Majorhardware.Buildconstruction?.Rule == 1
                ? x.Designcomponent.Lcmengineering
                    .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                    ?.Hardwareendofsupportcontract?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                : "";
            grid.MeInstallationDate = "";
            grid.SupportContract = x.Designcomponent.Lcmengineering
                .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                ?.Outputtolcmhardware;
            grid.LcmStatus = x.Designcomponent.Lcmengineering
                .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                ?.Lcmstatushardware;
            grid.MeLastMajorUpgradeDate = ""; //x.Networkelementsasis?.FirstOrDefault()?.Softwareinstalldate?.ToShortDateString() ?? "";
            grid.MeLastScanDate = ""; //x.Networkelementsasis?.FirstOrDefault()?.Modificationdate.ToShortDateString() ?? "";
            return grid;
        }

        private static ExpressionStarter<Networkelementsasplanned> ApplyFilter(ViaExportQuery buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);
            var predicateInner = PredicateBuilder.New<Networkelementsasplanned>(true);

            if (buildFilterDto?.LocationName != null && buildFilterDto.LocationName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LocationName)
                    predicateInner.Or(x => x.Location.Location == item);
                predicateResult.And(predicateInner);
            }
            //MeStatus
            if (buildFilterDto?.MeIpAddress != null && buildFilterDto.MeIpAddress.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeIpAddress)
                    predicateInner.Or(x => x.Additionalinformation1 == item);
                predicateResult.And(predicateInner);
            }
           
            if (buildFilterDto?.OrganisationName != null && buildFilterDto.OrganisationName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OrganisationName)
                    predicateInner.Or(x => x.Opco.Opco == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OrganisationNameId != null && buildFilterDto.OrganisationNameId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OrganisationNameId)
                    predicateInner.Or(x => x.Opco.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ResourceKey != null && buildFilterDto.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.ResourceKey)
                    predicateInner.Or(x => x.Hwresourcekey == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.MeCriticality != null && buildFilterDto.MeCriticality.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeCriticality)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Criticality == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.MeLCMPolicy != null && buildFilterDto.MeLCMPolicy.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.MeLCMPolicy)
                {
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Lcmpolicy == item);
                }
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.MeDeploymentType != null && buildFilterDto.MeDeploymentType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeDeploymentType)
                    predicateInner.Or(x => x.Deploymenttype.Deploymenttype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeEnvironment != null && buildFilterDto.MeEnvironment.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeEnvironment)
                    predicateInner.Or(x => x.Environment.Environment == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeExternalConnectionFlg != null && buildFilterDto.MeExternalConnectionFlg.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeExternalConnectionFlg)
                {
                    if (item == "Yes")
                        predicateInner.Or(x =>
                            (bool)x.Designcomponent.Subnetworkboundary.Internetfacing);
                    else
                        predicateInner.Or(x =>
                            (bool)!x.Designcomponent.Subnetworkboundary.Internetfacing);
                }
                predicateResult.And(predicateInner);
            }

            //MeFqdn 
            if (buildFilterDto?.MeGdprRelevantFlg != null && buildFilterDto.MeGdprRelevantFlg.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeGdprRelevantFlg)
                {
                    if (item == ConstantValueFilter.Yes)
                        predicateInner.Or(x =>
                            x.Designcomponent.Subnetworkboundary.Gdprrelevant == true);
                    else
                    if (item == ConstantValueFilter.Unspecified)
                        predicateInner.Or(x =>
                          x.Designcomponent.Subnetworkboundary.Gdprrelevant == null);
                    else
                        predicateInner.Or(x =>
                           x.Designcomponent.Subnetworkboundary.Gdprrelevant != true);
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeServiceType != null && buildFilterDto.MeServiceType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeServiceType)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Assetcategory.Assetcategory == item);
                predicateResult.And(predicateInner);
            }
            
            if (buildFilterDto?.MeSourceAssetId != null && buildFilterDto.MeSourceAssetId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeSourceAssetId)
                {
                    predicateInner.Or(x => (x.Networkelementasplannedid.ToString() == item));


                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeName != null && buildFilterDto.MeName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeName)
                    _ = predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Systemisshared ?
                        (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + (x.Location != null ?x.Location.Location : string.Empty) + "_T1" == item)
                      : (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Elementname == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeType != null && buildFilterDto.MeType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeType)
                {

                    if (item == ConstantValueFilter.Platformsupportingmultipleapplications)
                    {
                        predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Systemisshared);
                    }
                    else
                    {
                        predicateInner.Or(x => !x.Designcomponent.Designcomponentfamily.Systemisshared && ConstantValueFilter.Platformsupporting  + (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description : "") == item);
                    }
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeDescription != null && buildFilterDto.MeDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeDescription)
                    predicateInner.Or(x => ConstantValueFilter.Hardwaresolutionsupporting + x.Designcomponent.Systemtype.Assetclass.Assetclass + "<b class=\"text-lowercase\"> on </b>" + x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Buildconstruction.Buildconstruction == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.LcmProdName != null && buildFilterDto.LcmProdName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LcmProdName)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Hardwaresolution == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.Vendor != null && buildFilterDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Vendor)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.EoslContractDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.EoslContractDate.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Endofmaintenance >= buildFilterDto.EoslContractDate.StartDate);
                if (buildFilterDto?.EoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Endofmaintenance <= buildFilterDto.EoslContractDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EeoslContractDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.EeoslContractDate.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Hardwareendofsupportcontract >= buildFilterDto.EeoslContractDate.StartDate);
                if (buildFilterDto?.EeoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Hardwareendofsupportcontract <= buildFilterDto.EeoslContractDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OsName != null && buildFilterDto.OsName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.OsName)
                {
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                               .SingleOrDefault(m =>
                                                   m.Ismain &&
                                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule != 3 &&
                                           x.Additionalinformation2 == item);
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                               .SingleOrDefault(m =>
                                                   m.Ismain &&
                                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3 &&
                                           x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                               .SingleOrDefault(m =>
                                                   m.Ismain &&
                                                   m.Deleted == false).Majorhardware.Hardwaresolution == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OsEoslContractDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.OsEoslContractDate.StartDate != null)
                    predicateInner.And(x =>
                        x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .SingleOrDefault(m =>
                                m.Ismain &&
                                m.Deleted == false)
                            .Majorhardware.Buildconstruction.Rule == 1 && x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance >= buildFilterDto.OsEoslContractDate.StartDate


);
                if (buildFilterDto?.OsEoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                        .SingleOrDefault(m =>
                            m.Ismain &&
                            m.Deleted == false)
                        .Majorhardware.Buildconstruction.Rule == 1 && x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance >= buildFilterDto.OsEoslContractDate.EndDate);

                predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m =>
                        m.Ismain &&
                        m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.OsEeoslContractDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.OsEeoslContractDate.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Hardwareendofsupportcontract >= buildFilterDto.OsEeoslContractDate.StartDate);
                if (buildFilterDto?.OsEeoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Hardwareendofsupportcontract <= buildFilterDto.OsEeoslContractDate.EndDate);

                predicateInner.And(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m =>
                        m.Ismain &&
                        m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.SupportContract != null && buildFilterDto.SupportContract.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.SupportContract)
                    predicateInner.Or(x => x.Designcomponent.Lcmengineering
                        .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        .Hardwaresupporttype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.LcmStatus != null && buildFilterDto.LcmStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LcmStatus)
                    predicateInner.Or(x => x.Designcomponent.Lcmengineering.Where(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        .Lcmstatushardware == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeLastMajorUpgradeDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.MeLastMajorUpgradeDate.StartDate != null)
                    predicateInner.And(x => x.Networkelementsasis.FirstOrDefault().Softwareinstalldate >= buildFilterDto.MeLastMajorUpgradeDate.StartDate);
                if (buildFilterDto?.MeLastMajorUpgradeDate.EndDate != null)
                    predicateInner.And(x => x.Networkelementsasis.FirstOrDefault().Softwareinstalldate <= buildFilterDto.MeLastMajorUpgradeDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeLastScanDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.MeLastScanDate.StartDate != null)
                    predicateInner.And(x => x.Networkelementsasis.FirstOrDefault().Modificationdate >= buildFilterDto.MeLastScanDate.StartDate);
                if (buildFilterDto?.MeLastScanDate.EndDate != null)
                    predicateInner.And(x => x.Networkelementsasis.FirstOrDefault().Modificationdate <= buildFilterDto.MeLastScanDate.EndDate);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto?.VerticalName != null && buildFilterDto.VerticalName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                int count = 0;
                foreach (var item in buildFilterDto?.VerticalName)
                    if (item == "yes")
                    {
                        count++;

                        predicateInner.Or(x => x.Lcmengineering == null && !x.Networkelementasplannedsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(r => r.Networkelementasplannedsubdomainspoc.Any(x => x.Subdomainspoc.AspnetuserverticalsUser.Any(x => x.Organisation.Vertical.Verticalresponsibleid.ToString() == item && x.Deleted == false
                        /*&& x.Opcoid == x.Opcoid */)));
                    }
                if (count > 0) predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]> GetColumnsMap()
        {
            return new Dictionary<string, Expression<Func<Networkelementsasplanned, object>>[]>
            {
                ["locationName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Location.Location },
                ["meStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meIpAddress"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Additionalinformation1 },
                ["meVirtualFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },//todo verificare
                ["organisationName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Opco.Opco },//todo verificare

                //["verticalName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsible },
                //["verticalNameId"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsibleid },

                ["OrganisationNameId"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Opco.Opcoid },
                //todo verificare                ["meCriticality"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Subnetworkboundary.Criticality },
                ["meLCMPolicy"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => (x.Designcomponent == null) ? "" : (((LCMPolicy)x.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString()) },
                ["meDeploymentType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Deploymenttype.Deploymenttype },
                ["meEnvironment"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Environment.Environment },
                ["meExternalConnectionFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Subnetworkboundary.Internetfacing },
                ["meFqdn"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meGdprRelevantFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Subnetworkboundary.Gdprrelevant },
                ["meServiceType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Assetcategory },
                ["meCyberarkIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meIdmIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["resourceKey"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Hwresourcekey },


                // ["meSecurityTier"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Designcomponentfamily.Securitytirezone.Description },
                ["me2FaIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meSiemIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meMacAddress"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meSerialNumber"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meSourceAssetId"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardwareid + "_" + x.Location.Location + "_T1" }, // todo verifica
                ["meName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Designcomponentfamily.Systemisshared ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Location.Location + "_T1" : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Elementname },
                ["meType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Designcomponentfamily.Systemisshared ? ConstantValueFilter.Platformsupportingmultipleapplications : ConstantValueFilter.Platformsupporting + x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description },
                ["meDescription"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => ConstantValueFilter.Hardwaresolutionsupporting + x.Designcomponent.Systemtype.Assetclass.Assetclass + "<b class=\"text-lowercase\"> on </b>" + x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Buildconstruction.Buildconstruction },
                ["lcmProdName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Hardwaresolution },
                ["vendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["eoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Endofmaintenance },
                ["eeoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Hardwareendofsupportcontract },
                ["osEoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                ["osEeoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Hardwareendofsupportcontract },
                ["meInstallationDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["supportContract"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Hardwaresupporttype },
                ["lcmStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Lcmstatushardware },
                ["meLastScanDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Networkelementsasis.FirstOrDefault().Modificationdate },
                ["meLastMajorUpgradeDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Networkelementsasis.FirstOrDefault().Softwareinstalldate },
            };
        }



        private List<FilterValueDto> GETMDiscription( IQueryable<Networkelementsasplanned> query)
        {
            List<FilterValueDto> data = new List<FilterValueDto>();
            foreach (var x in query)
            {
                var _result = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(p => p.Ismain);
                    if (_result != null && _result.Majorhardware.Buildconstruction != null)
                    {
                        data.Add(new FilterValueDto(ConstantValueFilter.Hardwaresolutionsupporting + x.Designcomponent.Systemtype.Assetclass.Assetclass + "<b class=\"text-lowercase\"> on </b>" + x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .FirstOrDefault(s => s.Ismain).Majorhardware.Buildconstruction.Buildconstruction));
                    }
            }
            return data.Distinct().ToList();
        }

        private List<FilterValueDto> GETOSName(IQueryable<Networkelementsasplanned> query)
        {
            List<FilterValueDto> data = new List<FilterValueDto>();
            foreach (var x in query)
            {
                var _result = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(p => p.Ismain);
                if (_result != null && _result.Majorhardware.Buildconstruction != null)
                {
                    data.Add(new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                                               .SingleOrDefault(m =>
                                                                   m.Ismain &&
                                                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule != 3 ?
                                                           x.Additionalinformation2 : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                                               .SingleOrDefault(m =>
                                                                   m.Ismain &&
                                                                   m.Deleted == false).Majorhardware.Hardwaresolution));
                }
            }

            
            return data.Distinct().ToList();
        }

        private List<FilterValueDto> GetSupportedContracts(IQueryable<Networkelementsasplanned> query)
        {
            List<FilterValueDto> data = new List<FilterValueDto>();
            foreach (var x in query)
            {
                var _result = x.Designcomponent.Lcmengineering;
                if (_result != null )
                {
                    data.Add(new FilterValueDto(x.Designcomponent.Lcmengineering.FirstOrDefault(s =>s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)?.Softwaresupporttype));
                }
            }


            return data.Distinct().ToList();
      

        }
        private List<FilterValueDto> GetLCMStatus(IQueryable<Networkelementsasplanned> query)
        {
            List<FilterValueDto> data = new List<FilterValueDto>();
            foreach (var x in query)
            {
                var _result = x.Designcomponent.Lcmengineering;
                if (_result != null)
                {
                    data.Add(new FilterValueDto(x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)?.Lcmstatushardware));
                }
            }


            return data.Distinct().ToList();


        }
        public List<FilterValueDto> GetFilter(string propertyName, string propertyFilter,
            ViaExportQuery buildFilterDto,bool isAdmin)
        {

            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyFilter(buildFilterDto);
            var query = GetQuery(predicateResult);

            if (propertyName == "verticalName")
            {
                var verticalFilterDto = new List<FilterValueDto>();
                var defaultItem = new FilterValueDto { Value = "yes", Text = "---" };
                foreach (var item in query)
                {
                    List<FilterValueDto> tempList = null;
                    if (item.Networkelementasplannedid != null)
                    {
                        tempList = _commonManager.GetVerticaleFilterDto(item?.Networkelementasplannedsubdomainspoc?
                                    .Select(x => x?.Subdomainspocid).ToList(),0)?.Select(t => new FilterValueDto
                                    {
                                        Text = t.Text.ToString(),
                                        Value = t.Value.ToString()
                                    })
                                    ?.Distinct()
                                    ?.ToList();
                    }
                    
                    if (tempList != null && tempList.Any() == true)
                    {
                        var secTempList = verticalFilterDto?.Any() == true ? tempList.Where(x => !verticalFilterDto.Any(y => y.Value == x.Value)) : tempList;
                        verticalFilterDto.AddRange(secTempList);
                    }
                    else if (tempList == null || tempList != null && tempList.Count() == 0)
                    {
                        if (verticalFilterDto != null && !verticalFilterDto.Any(x => x.Value == defaultItem.Value) )
                        {
                            verticalFilterDto.Add(defaultItem);
                        }
                    }
                }

                if (!isAdmin && (buildFilterDto.VerticalName != null && buildFilterDto.VerticalName.Count > 0) && propertyName == "verticalName")
                {
                    verticalFilterDto = verticalFilterDto.Where(x => buildFilterDto.VerticalName.Contains(x.Value.ToString())).ToList();
                }
                return verticalFilterDto;



            }

            var rtn = propertyName switch
            {
                "locationName" => string.IsNullOrEmpty(propertyFilter)
                ? query.Select(p => new FilterValueDto(p.Location.Location)).Distinct().ToList()
                : query
                    .Where(x => x.Location.Location.Contains(propertyFilter)).Select(x =>
                        new FilterValueDto(x.Location.Location)).Distinct()
                    .ToList(),

                "resourceKey" => string.IsNullOrEmpty(propertyFilter)
           ? query.Select(p => new FilterValueDto(p.Hwresourcekey)).Distinct().ToList()
           : query
               .Where(x => x.Hwresourcekey.Contains(propertyFilter)).Select(x =>
                   new FilterValueDto(x.Hwresourcekey)).Distinct()
               .ToList(),

                "meStatus" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.Operational) },
                "meIpAddress" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Additionalinformation1)).Distinct().ToList()
                    : query
                        .Where(x => x.Additionalinformation1.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Additionalinformation1)).Distinct()
                        .ToList(),
                "meVirtualFlg" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.No) },
                "organisationName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Opco.Opco)).Distinct().ToList()
                    : query
                        .Where(x => x.Opco.Opco.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Opco.Opco)).Distinct()
                        .ToList(),
                "OrganisationNameId" => string.IsNullOrEmpty(propertyFilter)
            ? query.Select(p => new FilterValueDto(p.Opco.Opcoid)).Distinct().ToList()
            : query
                .Where(x => x.Opco.Opco.Contains(propertyFilter)).Select(x =>
                    new FilterValueDto(x.Opco.Opcoid)).Distinct()
                .ToList(),

                "meCniBcServiceFlg" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "meCriticality" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Designcomponent.Subnetworkboundary.Criticality)).Distinct().ToList()
                    : query
                        .Where(x => x.Designcomponent.Subnetworkboundary.Criticality.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Subnetworkboundary.Criticality)).Distinct()
                        .ToList(),
                "meLCMPolicy" =>
                string.IsNullOrEmpty(propertyFilter)
                    ? query
                        .Select(p => new FilterValueDto { Text = ((LCMPolicy)p.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString(), Value = p.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString() }).Distinct().ToList()
                    : query
                        .Where(p => (p.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString()).Contains(propertyFilter))
                        .Select(p => new FilterValueDto { Text = ((LCMPolicy)p.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString(), Value = p.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString()}).Distinct().ToList(),

                "meDeploymentType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p=>p.Deploymenttypeid.HasValue).Select(x => new FilterValueDto(x.Deploymenttype.Deploymenttype)).Distinct().ToList()
                    : query
                        .Where(x => x.Deploymenttype.Deploymenttype.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Deploymenttype.Deploymenttype)).Distinct()
                        .ToList(),
                "meEnvironment" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Environment.Environment)).Distinct().ToList()
                    : query
                        .Where(x => x.Environment.Environment.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Environment.Environment)).Distinct()
                        .ToList(),
                "meExternalConnectionFlg" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "meFqdn" => new List<FilterValueDto>() { new FilterValueDto("") },
                "meGdprRelevantFlg" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No), new FilterValueDto(ConstantValueFilter.Unspecified) },
                "meServiceType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Assetcategory.Assetcategory)).Distinct().ToList()
                    : query
                        .Where(x => x.Designcomponent.Systemtype.Assetcategory.Assetcategory.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Systemtype.Assetcategory.Assetcategory)).Distinct()
                        .ToList(),
                "meCyberarkIntegrationFlg" => new List<FilterValueDto>() { new FilterValueDto("") },
                "meIdmIntegrationFlg" => new List<FilterValueDto>() { new FilterValueDto("") },

                "me2FaIntegrationFlg" => new List<FilterValueDto>() { new FilterValueDto("") },
                "meSiemIntegrationFlg" => new List<FilterValueDto>() { new FilterValueDto("") },
                "meMacAddress" => new List<FilterValueDto>() { new FilterValueDto("") },
                "meSerialNumber" => new List<FilterValueDto>() { new FilterValueDto("") },
                "meSourceAssetId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.ToList().Select(x => new FilterValueDto(x.Networkelementasplannedid.ToString(), x.Designcomponent.Designcomponentfamily.Systemisshared ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardwareid + "_" + x.Location.Location + "_T1" :
                    x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardwareid + "_" + $"TEMS{x.Networkelementasplannedid:000000}")).Distinct().ToList(): query.ToList()
                .Where(x => x.Designcomponent.Designcomponentfamily.Systemisshared ? (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardwareid + "_" + x.Location.Location + "_T1").Contains(propertyFilter) : (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardwareid + "_" + $"TEMS{x.Networkelementasplannedid:000000}").Contains(propertyFilter)).Select(x =>
                    new FilterValueDto(x.Networkelementasplannedid.ToString(), x.Designcomponent.Designcomponentfamily.Systemisshared ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardwareid + "_" + x.Location.Location + "_T1" : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardwareid + "_" + $"TEMS{x.Networkelementasplannedid:000000}")).Distinct()
                .ToList(),
                "meName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Designcomponentfamily.Systemisshared ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Location.Location + "_T1" : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Elementname)).Distinct().ToList()
                    : query
                        .Where(x => (x.Designcomponent.Designcomponentfamily.Systemisshared ? (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Location.Location + "_T1").Contains(propertyFilter) : (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Elementname).Contains(propertyFilter))).Select(x =>
                            new FilterValueDto(x.Designcomponent.Designcomponentfamily.Systemisshared ? x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Location.Location + "_T1" : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Platform.Platform + "_" + x.Elementname)).Distinct()
                        .ToList(),
                "meType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Designcomponentfamily.Systemisshared ? ConstantValueFilter.Platformsupportingmultipleapplications : ConstantValueFilter.Platformsupporting + x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description : "")).Distinct().ToList()
                    : query
                        .Where(x => x.Designcomponent.Designcomponentfamily.Systemisshared ? ConstantValueFilter.Platformsupportingmultipleapplications.Contains(propertyFilter) : (ConstantValueFilter.Platformsupporting + (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description : "")).Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Designcomponentfamily.Systemisshared ? ConstantValueFilter.Platformsupportingmultipleapplications : ConstantValueFilter.Platformsupporting + x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description :"")).Distinct()
                        .ToList(),
                "meDescription" => string.IsNullOrEmpty(propertyFilter)
                    ? GETMDiscription(query)
                    : query
                        .Where(x => (ConstantValueFilter.Hardwaresolutionsupporting + x.Designcomponent.Systemtype.Assetclass.Assetclass + "<b class=\"text-lowercase\"> on </b>" + x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Buildconstruction.Buildconstruction).Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(ConstantValueFilter.Hardwaresolutionsupporting + x.Designcomponent.Systemtype.Assetclass.Assetclass + "<b class=\"text-lowercase\"> on </b>" + x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Buildconstruction.Buildconstruction)).Distinct()
                        .ToList(),
                "lcmProdName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Hardwaresolution)).Distinct().ToList()
                    : query
                        .Where(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Hardwaresolution.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Hardwaresolution)).Distinct()
                        .ToList(),
                "vendor" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct().ToList()
                    : query
                        .Where(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(s => s.Ismain).Majorhardware.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct()
                        .ToList(),
                "osName" => string.IsNullOrEmpty(propertyFilter)
                    ? GETOSName(query)
                    : query
                        .Where(x => (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .SingleOrDefault(m =>
                                m.Ismain &&
                                m.Deleted == false).Majorhardware.Buildconstruction.Rule != 3 && x.Additionalinformation2.Contains(propertyFilter)) || (
                            x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                .SingleOrDefault(m =>
                                    m.Ismain &&
                                    m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3 && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                .SingleOrDefault(m =>
                                    m.Ismain &&
                                    m.Deleted == false).Majorhardware.Hardwaresolution.Contains(propertyFilter))).Select(x =>
                           new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                               .SingleOrDefault(m =>
                                   m.Ismain &&
                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule != 3 ?
                               x.Additionalinformation2 : x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                   .SingleOrDefault(m =>
                                       m.Ismain &&
                                       m.Deleted == false).Majorhardware.Hardwaresolution)).Distinct()
                        .ToList(),
                "meInstallationDate" => new List<FilterValueDto>(),
                "supportContract" => string.IsNullOrEmpty(propertyFilter)
                    ? GetSupportedContracts(query)
                    : query
                        .Where(x => x.Designcomponent.Lcmengineering
                            .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                            .Softwaresupporttype.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Lcmengineering
                                .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwaresupporttype)).Distinct()
                        .ToList(),
                "lcmStatus" => string.IsNullOrEmpty(propertyFilter)
                    ? GetLCMStatus(query).Where(x => x.Value != null).ToList()
                    : query
                        .Where(x => x.Designcomponent.Lcmengineering
                            .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                            .Lcmstatushardware.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Lcmengineering
                                .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Lcmstatushardware)).Distinct()
                        .ToList().Where(x => x.Value != null).ToList(),
               





                _ => new List<FilterValueDto>(),

            };
            return rtn;
        }


        private IQueryable<Networkelementsasplanned> GetQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {
            var data = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).Where(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .SingleOrDefault(m =>
                                m.Ismain &&
                                m.Systemtypeid == x.Designcomponent.Systemtype.Systemtypeid &&
                                m.Deleted == false).Majorhardware.Buildconstruction.Rule != 3)
                        .Include(x => x.Location)
                        .Include(x => x.Deploymenttype)
                        .Include(x => x.Networkelementsasis)
                        .Include(x => x.Environment)
                        .Include(x => x.Opco)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Lcmengineering)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(p=>p.Designaspects).ThenInclude(p=>p.Securitytirezone)
                        .Include(x => x.Networkelementasplannedsubdomainspoc)
                        .Include(x=>x.Lcmengineering)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                            ;


           
            return data.AsQueryable();
        }
    }
}


