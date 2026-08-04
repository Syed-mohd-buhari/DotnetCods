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
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;

namespace CAM.BusinessManager.Entity
{
    public class ViaExportSoftwareManager:BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly GridCustomColumnManager _columnManager;
        static string propritaryHw = "Propritary HW";
        private readonly CommonManager _commonManager;

        public ViaExportSoftwareManager(IEnumerable<IRepositoryWrapper> wrappers, IMapper mapper, GridCustomColumnManager columnManager,CommonManager commonManager,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper):base(contextAccessor,wrappers,out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _columnManager = columnManager;
            _commonManager = commonManager;
        }

        private bool ValidateIsVirtualFlag(Networkelementsasplanned x)
        {
            return x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                    .Majorhardware?.Buildconstruction?.Rule == 3;
        }
        private string GetSecurityTier(Designcomponentfamilies? model , int opcoId)      
        {

            if (model != null && model.Designaspects.Any())
            {
                var item = model.Designaspects.FirstOrDefault(p => p.Opcoid == opcoId);
                if (item != null && item.Securitytirezone != null) return item.Securitytirezone.Description;
                return null;
            }
            return null;
        }
        private bool GetCNI(Designcomponentfamilies? model, int opcoId)
        {

            if (model != null && model.Designaspects.Any())
            {
                var item = model.Designaspects.FirstOrDefault(p => p.Opcoid == opcoId);
                if (item != null) return item.Criticalnationalinfrastructure;
                return false;
            }
            return false;
        }
        public QueryResultDto<ViaExport> FindWithCondition(ViaExportQuery buildFilterDto)
        {
            ViaExport ViaExport(Networkelementsasplanned x)
            {
                var grid = new ViaExport();
                
                grid.LocationName = x.Location.Location;
                grid.resourceKey = x.Swresourcekey;
                grid.MeStatus = ConstantValueFilter.Operational;
                grid.MeIpAddress = x.Additionalinformation1;
                grid.MeVirtualFlg = ValidateIsVirtualFlag(x)
                    ? ConstantValueFilter.Yes
                    : ConstantValueFilter.No;
                grid.OrganisationName = x.Opco.Opco;
                //grid.VerticalNameId = (int) x.Designcomponent.Systemtype.Verticalresponsible.Verticalresponsibleid;
                grid.VerticalName = _commonManager.GetVerticaleNameRes(x.Networkelementasplannedsubdomainspoc?.Select(x => x.Subdomainspocid).ToList(),x.Opcoid);

                grid.MeCriticality = x.Designcomponent?.Subnetworkboundary.Criticality;
                grid.MeLCMPolicy = (x.Designcomponent== null) ? "" : (((LCMPolicy)x.Designcomponent?.Subnetworkboundary.Lcmpolicy).ToString());
                grid.MeDeploymentType = x.Deploymenttype?.Deploymenttype;
                grid.MeEnvironment = x.Environment.Environment;
                grid.MeExternalConnectionFlg = (bool)x.Designcomponent.Subnetworkboundary.Internetfacing ? ConstantValueFilter.Yes : ConstantValueFilter.No;
                grid.MeFqdn = "";
                grid.MeGdprRelevantFlg = x.Designcomponent.Subnetworkboundary.Gdprrelevant == null ? ConstantValueFilter.Unspecified :
                    x.Designcomponent.Subnetworkboundary.Gdprrelevant == true ? ConstantValueFilter.Yes : ConstantValueFilter.No;
                grid.MeServiceType = x.Designcomponent.Systemtype.Assetcategory.Assetcategory;
                grid.MeCyberarkIntegrationFlg = "";
                grid.MeIdmIntegrationFlg = "";
                grid.MeSecurityTier = GetSecurityTier(x.Designcomponent.Designcomponentfamily , x.Opco.Opcoid);
                grid.MeCniBcServiceFlg  =GetCNI(x.Designcomponent.Designcomponentfamily, x.Opco.Opcoid) ? "Y" : "N";

                grid.Me2FaIntegrationFlg = "";
                grid.MeSiemIntegrationFlg = "";
                grid.MeMacAddress = ConstantValueFilter.NA;
                grid.MeSerialNumber = ConstantValueFilter.NA;
                grid.MeSourceAssetId = $"TEMS{x.Networkelementasplannedid:000000}";
                grid.MeName = x.Elementname;
                grid.MeType = x.Designcomponent.Systemtype?.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype?.Assettype?.Assettype : 
                    (x.Designcomponent.Systemtype?.VodafonenameNavigation != null ? x.Designcomponent.Systemtype?.VodafonenameNavigation.Description :"");
                grid.MeDescription = x.Designcomponent.Designcomponentfamily != null ? x.Designcomponent.Designcomponentfamily.Description : "";
                grid.LcmProdName = (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description:"") + " " + x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion;
                grid.Vendor = x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer
                    .Originalequipmentmanufacturer;
                grid.EoslContractDate = ( x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance != null
                    ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    : "");
                grid.EOMStatus = (x.Designcomponent.Systemtype.Majorsoftwarebuilds != null ? (EOMEnum)x.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus
                    : EOMEnum.NotSpecified);


                grid.EeoslContractDate =
                    x.Designcomponent.Lcmengineering.Any(s =>
                        s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid) 
                        ? x.Designcomponent.Lcmengineering
                            .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                            ?.Softwareendofsupportcontract?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                        : "";
                grid.OsName = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m =>
                        m.Ismain &&
                        m.Deleted == false)
                    ?.Majorhardware.Buildconstruction?.Rule == 3
                    ? x.Additionalinformation2
                    : ConstantValueFilter.osIsProvidedByHardwareSolution;
                grid.OsEoslContractDate = ValidateIsVirtualFlag(x) ? (x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance != null
                    ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    : "") : ConstantValueFilter.NotSpecified;

                grid.OsEeoslContractDate = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                    .SingleOrDefault(m =>
                        m.Ismain &&
                        m.Deleted == false)
                    ?.Majorhardware.Buildconstruction?.Rule == 3
                    ? x.Designcomponent.Lcmengineering
                        .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        ?.Softwareendofsupportcontract?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    : "";
                grid.MeInstallationDate = "";
                grid.SupportContract = x.Designcomponent.Lcmengineering
                    .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                    ?.Outputtolcmsoftware;
                grid.LcmStatus = x.Designcomponent.Lcmengineering
                    .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                    ?.Lcmstatussoftware;
                grid.MeLastMajorUpgradeDate = x.Networkelementsasis?.FirstOrDefault()?.Softwareinstalldate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "";
                grid.MeLastScanDate = x.Networkelementsasis?.FirstOrDefault()?.Dataacquisitiondate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "";

                return grid;
            }

            ExpressionStarter<Networkelementsasplanned> predicateResult = ApplyFilter(buildFilterDto);

            IQueryable<Networkelementsasplanned> query = GetQuery(predicateResult).ApplyOrdering(buildFilterDto, GetColumnsMap() , ConstantValueFilter.Modificationdate).OrderByDescending(p=>p.Modificationdate).ApplyPagingViaExportStart(buildFilterDto);




            var data = query.ToList();


            var reports = data.Select(x => { return ViaExport(x); }).Distinct().ToList();




            var rtn = new QueryResultDto<ViaExport>(new GenerateRenderForGrid<ViaExport>(_columnManager));
            rtn.TotalItems = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult).Count(x => x.Designcomponent.Systemtype.Deleted == false && x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(s => s.Deleted == false));

            rtn.Items =   reports.ToArray();

            return rtn;
        }


        private List<FilterValueDto> GETMDiscription(IQueryable<NetworkElementAsPlanned> query)
        {
            List<FilterValueDto> data = new List<FilterValueDto>();
            foreach (var x in query)
            {
                var _result = x.DesignComponent.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(p => p.IsMain);
                if (_result != null && _result.MajorHardware.BuildConstruction != null)
                {
                    data.Add(new FilterValueDto("Hardware solution supporting, " + x.DesignComponent.SystemType.AssetClassIdNavigation.AssetClassDescription + "<b class=\"text-lowercase\"> on </b>" + x.DesignComponent.SystemType.SystemTypesMajorHardwareBuilds.FirstOrDefault(s => s.IsMain).MajorHardware.BuildConstruction.BuildConstructionDescription));
                }
            }
            return data.Distinct().ToList();
        }

        private List<FilterValueDto> GETMEType(IQueryable<Networkelementsasplanned> query)
        {
            List<FilterValueDto> data = new List<FilterValueDto>();
            foreach (var x in query)
            {
                var _result = x.Designcomponent.Systemtype;
                if (_result.Assetcategory != null && _result.Assetcategory.Takefromassettypetable  ==true && _result.Assettype != null )
                {
                    data.Add(new FilterValueDto(x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true 
                        ? x.Designcomponent.Systemtype.Assettype.Assettype : (x.Designcomponent.Systemtype.VodafonenameNavigation != null ?x.Designcomponent.Systemtype.VodafonenameNavigation.Description :"")));
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
                                                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3 ?
                                                           x.Additionalinformation2 : ConstantValueFilter.osIsProvidedByHardwareSolution));
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
                if (_result != null)
                {
                    data.Add(new FilterValueDto(x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        ?.Softwaresupporttype));
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
                    data.Add(new FilterValueDto(x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        ?.Lcmstatushardware));
                }
            }

            return data.Distinct().ToList();


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
            if (buildFilterDto?.MeVirtualFlg != null && buildFilterDto.MeVirtualFlg.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeVirtualFlg)
                {
                    if (item == ConstantValueFilter.Yes)
                        predicateInner.Or(x =>
                            x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                                .Majorhardware.Buildconstruction.Rule == 3);
                    else
                        predicateInner.Or(x =>
                            x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain)
                                .Majorhardware.Buildconstruction.Rule != 3);
                }

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

            if (buildFilterDto?.MeCriticality != null && buildFilterDto.MeCriticality.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeCriticality)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Criticality == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ResourceKey != null && buildFilterDto.ResourceKey.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto.ResourceKey)
                    predicateInner.Or(x => x.Swresourcekey == item);
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
                    if (item == ConstantValueFilter.Yes)
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
          
            if (buildFilterDto?.MeSecurityTier != null && buildFilterDto.MeSecurityTier.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeSecurityTier)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Designaspects.Any(p=>p.Securitytirezone.Description == item));
                predicateResult.And(predicateInner);
            }
          
            if (buildFilterDto?.MeSourceAssetId != null && buildFilterDto.MeSourceAssetId.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeSourceAssetId)
                    predicateInner.Or(x => x.Networkelementasplannedid.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeName != null && buildFilterDto.MeName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeName)
                    predicateInner.Or(x => x.Elementname == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeType != null && buildFilterDto.MeType.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeType)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true
                    ? x.Designcomponent.Systemtype.Assettype.Assettype == item : x.Designcomponent.Systemtype.VodafonenameNavigation != null
                    && x.Designcomponent.Systemtype.VodafonenameNavigation.Description == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.MeDescription != null && buildFilterDto.MeDescription.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.MeDescription)
                    predicateInner.Or(x => x.Designcomponent.Designcomponentfamily.Description == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.LcmProdName != null && buildFilterDto.LcmProdName.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LcmProdName)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuildsid.ToString() == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.Vendor != null && buildFilterDto.Vendor.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.Vendor)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer == item);
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto?.EoslContractDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.EoslContractDate.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance >= buildFilterDto.EoslContractDate.StartDate);
                if (buildFilterDto?.EoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= buildFilterDto.EoslContractDate.EndDate);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.EeoslContractDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.EeoslContractDate.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwareendofsupportcontract >= buildFilterDto.EeoslContractDate.StartDate);
                if (buildFilterDto?.EeoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwareendofsupportcontract <= buildFilterDto.EeoslContractDate.EndDate);
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
                                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3 &&
                                           x.Additionalinformation2 == item);
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                               .SingleOrDefault(m =>
                                                   m.Ismain &&
                                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule != 3 && ConstantValueFilter.osIsProvidedByHardwareSolution == item);
                }

                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.OsEoslContractDate != null)
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                if (buildFilterDto?.OsEoslContractDate.StartDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance >= buildFilterDto.OsEoslContractDate.StartDate);
                if (buildFilterDto?.OsEoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance <= buildFilterDto.OsEoslContractDate.EndDate);

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
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwareendofsupportcontract >= buildFilterDto.OsEeoslContractDate.StartDate);
                if (buildFilterDto?.OsEeoslContractDate.EndDate != null)
                    predicateInner.And(x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwareendofsupportcontract <= buildFilterDto.OsEeoslContractDate.EndDate);

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
                        .Softwaresupporttype == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto?.LcmStatus != null && buildFilterDto.LcmStatus.Any())
            {
                predicateInner = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in buildFilterDto?.LcmStatus)
                    predicateInner.Or(x => x.Designcomponent.Lcmengineering
                        .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid)
                        .Lcmstatussoftware == item);
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
                    if(item == "yes")
                    {
                        count++;

                        predicateInner.Or(x => !x.Networkelementasplannedsubdomainspoc.Any());
                    }
                    else
                    {
                        count++;

                        predicateInner.Or(x => x.Networkelementasplannedsubdomainspoc.Any(s => s.Subdomainspoc.AspnetuserverticalsUser.Any(r => r.Organisation.Vertical.Verticalresponsibleid.ToString() == item && r.Deleted == false 
                        /*&& r.Opcoid==x.Opcoid*/)));
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
                ["meVirtualFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.FirstOrDefault(x => x.Ismain).Majorhardware.Buildconstruction.Buildconstruction },//todo verificare
                ["organisationName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Opco.Opco },//todo verificare 

                ["meCriticality"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Subnetworkboundary.Criticality },
                ["meLCMPolicy"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => (x.Designcomponent == null) ? "" : (((LCMPolicy)x.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString()) },
                ["meDeploymentType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Deploymenttype.Deploymenttype },
                ["meEnvironment"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Environment.Environment },
                ["meExternalConnectionFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Subnetworkboundary.Internetfacing },
                ["meFqdn"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meGdprRelevantFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Subnetworkboundary.Gdprrelevant },
                ["meServiceType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Assetcategory },
                ["meCyberarkIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meIdmIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                //["meSecurityTier"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Designcomponentfamily.Designaspects.Securitytirezone.Description },
                ["me2FaIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meSiemIntegrationFlg"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meMacAddress"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meSerialNumber"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["meSourceAssetId"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Networkelementasplannedid },
                ["meName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Elementname },
                ["resourceKey"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Swresourcekey },

                ["meType"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype : (x.Designcomponent.Systemtype.VodafonenameNavigation != null ? x.Designcomponent.Systemtype.VodafonenameNavigation.Description:"") },
                ["meDescription"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Designcomponentfamily.Description },
                ["lcmProdName"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname!= null? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description :"", x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion },

                ["vendor"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer },
                ["eoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                ["eeoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwareendofsupportcontract },
                ["osEoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofmaintenance },
                ["osEeoslContractDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwareendofsupportcontract },
                ["meInstallationDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Modificationdate },
                ["supportContract"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Softwaresupporttype },
                ["lcmStatus"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Designcomponent.Lcmengineering.FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Lcmstatussoftware },
                ["meLastScanDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Networkelementsasis.FirstOrDefault().Modificationdate },
                ["meLastMajorUpgradeDate"] = new Expression<Func<Networkelementsasplanned, object>>[] { x => x.Networkelementsasis.FirstOrDefault().Softwareinstalldate },
            };
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
                                    .Select(x => x?.Subdomainspocid).ToList(),
                                    0)?.Select(t => new FilterValueDto
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
                        if (verticalFilterDto != null && !verticalFilterDto.Any(x => x.Value == defaultItem.Value))
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
           ? query.Select(p => new FilterValueDto(p.Swresourcekey)).Distinct().ToList()
           : query
               .Where(x => x.Swresourcekey.Contains(propertyFilter)).Select(x =>
                   new FilterValueDto(x.Swresourcekey)).Distinct()
               .ToList(),

                "meStatus" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.Operational) },
                "meIpAddress" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Additionalinformation1)).Distinct().ToList()
                    : query
                        .Where(x => x.Additionalinformation1.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Additionalinformation1)).Distinct()
                        .ToList(),
                "meVirtualFlg" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.Yes), new FilterValueDto(ConstantValueFilter.No) },
                "organisationName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Opco.Opco)).Distinct().ToList()
                    : query
                        .Where(x => x.Opco.Opco.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Opco.Opco)).Distinct()
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
                       .Select(p => new FilterValueDto { Text = ((LCMPolicy)p.Designcomponent.Subnetworkboundary.Lcmpolicy).ToString(), Value = p.Designcomponent.Subnetworkboundary.Lcmpolicy.ToString() }).Distinct().ToList(),

                "meDeploymentType" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Where(p=>p.Deploymenttype != null).Select(x => new FilterValueDto(x.Deploymenttype.Deploymenttype)).Distinct().ToList()
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
                "meMacAddress" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.NA) },
                "meSerialNumber" => new List<FilterValueDto>() { new FilterValueDto(ConstantValueFilter.NA) },
                "meSourceAssetId" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Networkelementasplannedid, $"TEMS{p.Networkelementasplannedid:000000}")).Distinct().ToList()
                    : query
                        .Where(x => x.Networkelementasplannedid.ToString().Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Networkelementasplannedid, $"TEMS{x.Networkelementasplannedid:000000}")).Distinct()
                        .ToList(),
                "meName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(p => new FilterValueDto(p.Elementname)).Distinct().ToList()
                    : query
                        .Where(x => x.Elementname.ToString().Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Elementname)).Distinct()
                        .ToList(),
                "meType" => string.IsNullOrEmpty(propertyFilter)
                   ? query.Where(x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true || x.Designcomponent.Systemtype.VodafonenameNavigation != null).Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype : x.Designcomponent.Systemtype.VodafonenameNavigation.Description)).Distinct().ToList()
                   : query.Where(x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true || x.Designcomponent.Systemtype.VodafonenameNavigation != null)
                       .Where(x => x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype.Contains(propertyFilter) : x.Designcomponent.Systemtype.VodafonenameNavigation.Description.Contains(propertyFilter)).Select(x =>
                           new FilterValueDto(x.Designcomponent.Systemtype.Assetcategory.Takefromassettypetable == true ? x.Designcomponent.Systemtype.Assettype.Assettype
                           : x.Designcomponent.Systemtype.VodafonenameNavigation.Description)).Distinct()
                       .ToList(),

                "meDescription" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Designcomponentfamily.Description)).Distinct().ToList()
                    : query
                        .Where(x => x.Designcomponent.Designcomponentfamily.Description.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Designcomponentfamily.Description)).Distinct()
                        .ToList(),
                "lcmProdName" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuildsid, x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null ? x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description : "" + " " + x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct().ToList()
                    : query
                        .Where(x =>( x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname != null) && x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description.Contains(propertyFilter) || x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuildsid, x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description + " " + x.Designcomponent.Systemtype.Majorsoftwarebuilds.Softwareversion)).Distinct()
                        .ToList(),
                "vendor" => string.IsNullOrEmpty(propertyFilter)
                    ? query.Select(x => new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct().ToList()
                    : query
                        .Where(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Originalequipmentmanufacturer)).Distinct()
                        .ToList(),
                "osName" => string.IsNullOrEmpty(propertyFilter)
                    ?GETOSName(query)
                    : query
                        .Where(x => (x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                            .SingleOrDefault(m =>
                                m.Ismain &&
                                m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3 && x.Additionalinformation2.Contains(propertyFilter)) || (
                            x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                                .SingleOrDefault(m =>
                                    m.Ismain &&
                                    m.Deleted == false).Majorhardware.Buildconstruction.Rule != 3 && ConstantValueFilter.osIsProvidedByHardwareSolution.Contains(propertyFilter))).Select(x =>
                           new FilterValueDto(x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds
                               .SingleOrDefault(m =>
                                   m.Ismain &&
                                   m.Deleted == false).Majorhardware.Buildconstruction.Rule == 3 ?
                               x.Additionalinformation2 : ConstantValueFilter.osIsProvidedByHardwareSolution)).Distinct()
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
                            .Lcmstatussoftware.Contains(propertyFilter)).Select(x =>
                            new FilterValueDto(x.Designcomponent.Lcmengineering
                                .FirstOrDefault(s => s.Opcoid == x.Opcoid && s.Designcomponentid == x.Designcomponentid).Lcmstatussoftware)).Distinct()
                        .ToList().Where(x => x.Value != null).ToList(),
                

                _ => new List<FilterValueDto>(),

            };
            return rtn;
        }


        private IQueryable<Networkelementsasplanned> GetQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {
            var data = (predicateResult.IsStarted ? _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult)
                        .Include(x => x.Location)
                        .Include(x => x.Deploymenttype)
                        .Include(x => x.Environment)
                        .Include(x => x.Opco)
                        .Include(x => x.Networkelementsasis)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(p=>p.Designaspects).ThenInclude(p=>p.Securitytirezone)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Lcmengineering)
                        .Include(x => x.Networkelementasplannedsubdomainspoc)
                        .Include(x=>x.Lcmengineering)
                        //.Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmengineeringsubdomainspoc).ThenInclude(x => x.Subdomainspoc).ThenInclude(x => x.Aspnetuserroles).ThenInclude(x => x.Verticalresponsible)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)

                            :
                      _repositoryWrapper.NetworkElementAsPlanned.FindAll()
                         .Include(x => x.Location)
                        .Include(x => x.Deploymenttype)
                        .Include(x => x.Environment)
                        .Include(x => x.Opco)
                        .Include(x => x.Networkelementsasis)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(p => p.Designaspects).ThenInclude(p => p.Securitytirezone)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Lcmengineering)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                        .Include(x => x.Networkelementasplannedsubdomainspoc)
                        .Include(x => x.Lcmengineering)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetclass)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assettype)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.VodafonenameNavigation)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)).AsEnumerable();


           
            return data.AsQueryable();
        }
    }
}


