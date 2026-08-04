using AutoMapper;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.PLannedActivities;
using CAM.BusinessManager.ExtensionMethod.SystemType;
using CAM.BusinessManager.GenericReports;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.Rules;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.Common;
using CAM.DataTransferObjects.Entita.FNT_Report;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.DataTransferObjects.QueryDto.FNT;
using CAM.Entities.Mappers.Cbom;
using CAM.Entities.Mappers.Entity;
using CAM.Entities.Models;
using CAM.Entities.Models.FNT;
using CAM.Infrastucture;
using CAM.Infrastucture.QueryResult;
using DocumentFormat.OpenXml.Drawing;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.FNT_Report
{
    public class TemsFntReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _gridmanager;
        private CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private GenericReportGenration _genericReportGenration;
        private readonly IMapper _mapper;

        public TemsFntReportManager(IEnumerable<IRepositoryWrapper> wrappers,GridCustomColumnManager manager, CommonManager commonManager,
            GenericReportGenration genericReportGenration,ILoggerManager logger, IMapper mapper, IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _gridmanager = manager;
            _commonManager = commonManager;
            _logger = logger;
            _genericReportGenration = genericReportGenration;
            _mapper = mapper;
        }

        #region Refresh Members

        public async Task<ResultDto> TemsFNTDataRefresh(TemsFntReportQueryDto dto)
        {
            var result = await Task.Run(() => TemsFNTCreateAndUpdate(dto));
            //var groupResult = await  Task.Run(()=>TSRCreateAndUpdate(dto));

            return new ResultDto
            {
                Warning = result.Warning ? true : false,
                Info = result.Warning ? "Something went wrong while loading data to the TEMS FNT report" : "TEMS FNT Data inserted and updated successfully",
                Data = result.Warning ? result.Data : "",
            };
        }

        public ExpressionStarter<Networkelementsasplanned> TemsFntReportApplyFilter(TemsFntReportQueryDto fntQueryDto)
        {
            var predicateResult = PredicateBuilder.New<Networkelementsasplanned>(true);
            var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>(true);

            if (fntQueryDto?.HostName != null && fntQueryDto.HostName.Any())
            {
                resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in fntQueryDto?.HostName)
                    resultPredicate.Or(x => x.Elementname == item);
                predicateResult.And(resultPredicate);
            }
            if (fntQueryDto?.LocationOfHardwareAsset != null && fntQueryDto.LocationOfHardwareAsset.Any())
            {
                resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in fntQueryDto?.LocationOfHardwareAsset)
                    resultPredicate.Or(x => x.Opco.Opcoid.ToString() == item);
                predicateResult.And(resultPredicate);
            }

            return predicateResult;
        }

        private IQueryable<Networkelementsasplanned> GetTemsFntRefreshQuery(ExpressionStarter<Networkelementsasplanned> predicateResult)
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
                                .Include(x => x.Location)
                                //.Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Productimportance)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmancillarydata)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.Lcmoperationalcontracts).ThenInclude(x => x.Operationalcontract)
                                //.Include(x => x.Networkelementasplannedsubdomainspoc).ThenInclude(x => x.Subdomainspoc)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Subnetworkboundary)
                                //.Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Criticalassettype)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Assetcategory)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Operatingsystem)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Orgeqpmanufacturer)
                                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                                .Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Designcomponent)
                                //.Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Planningactivitystatus)
                                //.Include(x => x.Lcmengineering).ThenInclude(x => x.PlannedactivitiesLcmengineering).ThenInclude(x => x.Deliverystatus)
                                .Include(x => x.Identitiesasis).ThenInclude(x => x.Category)
                                .Include(x => x.ModificationuserNavigation);
            return result;
        }

        public async Task<ResultDto> TemsFNTCreateAndUpdate(TemsFntReportQueryDto fntQueryDto)
        {
            var currentTime = DateTime.Now;
            try
            {

                var createTemsFntList = new List<Temsfntreport>();
                var updateTemsFntList = new List<Temsfntreport>();

                var predicateResult = TemsFntReportApplyFilter(fntQueryDto);

                var query = await Task.Run(() => GetTemsFntRefreshQuery(predicateResult).AsQueryable());               

                var allOperationalContracts = _repositoryWrapper.LCMOperationalContracts.FindAll().AsNoTracking().AsSplitQuery().Include(x => x.Operationalcontract).ToList();

                var location = _repositoryWrapper.Location.FindByCondition(x => query.AsEnumerable().Select(y => y.Locationid).ToList().Contains(x.Locationid)).ToList();

                var assetIdAndOpcoId = query?.ToList()?.Where(x => x.Networkelementasplannedid != 0)?.DistinctBy(x => x?.Networkelementasplannedid)
                    .ToDictionary(x => x.Networkelementasplannedid, x => (long)x.Opcoid);

                var hardWare = query.Select(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Select(x => x.Majorhardware)).ToList();

                var allSubDomain = _commonManager.GetCalculatedAssetSubDomainSpocEntityforReport(assetIdAndOpcoId).ToList();
                var allEdu = _commonManager.GetCalculatedAssetEduSpocEntityforReport(assetIdAndOpcoId).ToList();
                var elementnames = query?.Select(x => x.Elementname)?.Distinct()?.ToList();

                var hardwareconfigurations = _commonManager.GetHardwareConfigurations(elementnames);
                var deploymentStatuses = _repositoryWrapper.DeploymentStatus.FindAll();

                var designcomponent = await _repositoryWrapper.DesignComponent.FindAll().Include(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ToListAsync();

                var result = await TemsFntReportColumnMapping(query.ToList(), allOperationalContracts,location, assetIdAndOpcoId, hardWare, allSubDomain, allEdu, elementnames, hardwareconfigurations, deploymentStatuses,designcomponent);
               
                foreach (var temsFnt in result.DistinctBy(x => new { x.Hostname,x.Serialnumberofhardwareasset}).ToList())
                {
                    var temsFntEntryExist = _repositoryWrapper.TemsFntReportRepository.FindByCondition(x => x.Hostname == temsFnt.Hostname &&
                    x.Serialnumberofhardwareasset == temsFnt.Serialnumberofhardwareasset).FirstOrDefault();

                    if (temsFntEntryExist != null)
                    {
                        var isUpdated = await Task.Run(() => TemsFntReportUpdate(temsFnt, temsFntEntryExist));

                        updateTemsFntList.Add(isUpdated);
                    }
                    else
                    {
                        createTemsFntList.Add(temsFnt);
                    }

                }
                var batchIdentifier = ConstantValueFilter.TemsFnt;
                var records = updateTemsFntList.Count + createTemsFntList.Count;

                _repositoryWrapper.TsrLogRepository.Create(new Tsrlogs
                {
                    Typeofoperation = ConstantValueFilter.DataRefreshTypeofoperation,
                    Totalrecord = records,
                    Processedrecord = 0,
                    Starttime = currentTime,
                    Endtime = null,
                    Domain = result.FirstOrDefault()?.Localmarket,
                    Status = ConstantValueFilter.TsrLogInProgressStatus,
                    Batchidentifier = batchIdentifier,

                });
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

                if (updateTemsFntList.Count > 0)
                {
                    
                    _repositoryWrapper.TemsFntReportRepository.BulkUpdate(updateTemsFntList);
                    await _repositoryWrapper.SaveAsync();
                    await _repositoryWrapper.ClearTracker();
                }
                if (createTemsFntList.Count > 0)
                {
                    _repositoryWrapper.TemsFntReportRepository.BulkCreate(createTemsFntList);
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
                    Domain = result.FirstOrDefault()?.Localmarket,
                    Status = ConstantValueFilter.TsrLogCompletedStatus,
                    Batchidentifier = batchIdentifier,

                });
                await _repositoryWrapper.SaveAsync();
                await _repositoryWrapper.ClearTracker();

                return new ResultDto
                {
                    Info = "TEMS FNT Data inserted and updated successfully",
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
                    Info = "Something went wrong while loading data to the Tems FNT report",
                    Warning = true,
                    Data = ex.InnerException.Message
                };
            }
        }


        private Temsfntreport TemsFntReportUpdate(Temsfntreport dataLoadTemsFnt, Temsfntreport exixtTemsFntEntry)
        {
            try
            {
                #region Update Mapping

                //exixtTemsFntEntry.Temsfntreportid = dataLoadTemsFnt.Temsfntreportid;
                exixtTemsFntEntry.Hostname = dataLoadTemsFnt.Hostname;
                exixtTemsFntEntry.Serialnumberofhardwareasset = dataLoadTemsFnt.Serialnumberofhardwareasset;
                exixtTemsFntEntry.Locationofhardwareasset = dataLoadTemsFnt.Locationofhardwareasset;
                exixtTemsFntEntry.Hardwaretypeofhardwareasset = dataLoadTemsFnt.Hardwaretypeofhardwareasset;
                exixtTemsFntEntry.Vendor = dataLoadTemsFnt.Vendor;
                exixtTemsFntEntry.Ipaddressofhardwareasset = dataLoadTemsFnt.Ipaddressofhardwareasset;
                exixtTemsFntEntry.Market = dataLoadTemsFnt.Market;
                exixtTemsFntEntry.Hwendoflife = dataLoadTemsFnt.Hwendoflife;
                exixtTemsFntEntry.Hwendofsupport = dataLoadTemsFnt.Hwendofsupport;
                exixtTemsFntEntry.Hwendofsale = dataLoadTemsFnt.Hwendofsale;
                exixtTemsFntEntry.Hardwaremodules = dataLoadTemsFnt.Hardwaremodules;
                exixtTemsFntEntry.Softwareproducttype = dataLoadTemsFnt.Softwareproducttype;
                exixtTemsFntEntry.Softwareproductversion = dataLoadTemsFnt.Softwareproductversion;
                exixtTemsFntEntry.Softwareisvirtualized = dataLoadTemsFnt.Softwareisvirtualized;
                exixtTemsFntEntry.OperatingSystemofvirtualmachine = dataLoadTemsFnt.OperatingSystemofvirtualmachine;
                exixtTemsFntEntry.Applicationhostedonsoftware = dataLoadTemsFnt.Applicationhostedonsoftware;
                exixtTemsFntEntry.Uuidserialnumberofsoftware = dataLoadTemsFnt.Uuidserialnumberofsoftware;
                exixtTemsFntEntry.Softwarevendor = dataLoadTemsFnt.Softwarevendor;
                exixtTemsFntEntry.Locationofsoftware = dataLoadTemsFnt.Locationofsoftware;
                exixtTemsFntEntry.Servicetype = dataLoadTemsFnt.Servicetype;
                exixtTemsFntEntry.Swendoflife = dataLoadTemsFnt.Swendoflife;
                exixtTemsFntEntry.Swendofsupport = dataLoadTemsFnt.Swendofsupport;
                exixtTemsFntEntry.Swendofsale = dataLoadTemsFnt.Swendofsale;
                exixtTemsFntEntry.Verticalengineeringteam = dataLoadTemsFnt.Verticalengineeringteam;
                exixtTemsFntEntry.Verticalsubdomain = dataLoadTemsFnt.Verticalsubdomain;
                exixtTemsFntEntry.Platform = dataLoadTemsFnt.Platform;
                exixtTemsFntEntry.Riskcluster = dataLoadTemsFnt.Riskcluster;
                exixtTemsFntEntry.Operationscontactpoint = dataLoadTemsFnt.Operationscontactpoint;
                exixtTemsFntEntry.Assetcategory = dataLoadTemsFnt.Assetcategory;
                exixtTemsFntEntry.Assetclass = dataLoadTemsFnt.Assetclass;
                exixtTemsFntEntry.Assettype = dataLoadTemsFnt.Assettype;
                exixtTemsFntEntry.Assetdescription = dataLoadTemsFnt.Assetdescription;
                exixtTemsFntEntry.Productimportance = dataLoadTemsFnt.Productimportance;
                exixtTemsFntEntry.Operationsmaintenancecontract = dataLoadTemsFnt.Operationsmaintenancecontract;
                exixtTemsFntEntry.Vendorendofmaintenancedate = dataLoadTemsFnt.Vendorendofmaintenancedate;
                exixtTemsFntEntry.Identifiedaction = dataLoadTemsFnt.Identifiedaction;
                exixtTemsFntEntry.Descriptionofplannedaction = dataLoadTemsFnt.Descriptionofplannedaction;
                exixtTemsFntEntry.Plannedhwmodel = dataLoadTemsFnt.Plannedhwmodel;
                exixtTemsFntEntry.Businessservicename = dataLoadTemsFnt.Businessservicename;
                exixtTemsFntEntry.Opmaintenancecontractenddate = dataLoadTemsFnt.Opmaintenancecontractenddate;
                exixtTemsFntEntry.Incidentclass = dataLoadTemsFnt.Incidentclass;
                exixtTemsFntEntry.Occurrenceprobability = dataLoadTemsFnt.Occurrenceprobability;
                exixtTemsFntEntry.Meverticalresposible = dataLoadTemsFnt.Meverticalresposible;
                exixtTemsFntEntry.Assetstatus = dataLoadTemsFnt.Assetstatus;
                exixtTemsFntEntry.Typeofnetworkelement = dataLoadTemsFnt.Typeofnetworkelement;
                exixtTemsFntEntry.Localmarket = dataLoadTemsFnt.Localmarket;
                exixtTemsFntEntry.Application = dataLoadTemsFnt.Application;
                exixtTemsFntEntry.Cloud = dataLoadTemsFnt.Cloud;
                exixtTemsFntEntry.Physicalserverhostname = dataLoadTemsFnt.Physicalserverhostname;
                exixtTemsFntEntry.Physicalserveripaddress = dataLoadTemsFnt.Physicalserveripaddress;
                exixtTemsFntEntry.Physicalserverserialnumber = dataLoadTemsFnt.Physicalserverserialnumber;
                exixtTemsFntEntry.Physicalserverhwmodel = dataLoadTemsFnt.Physicalserverhwmodel;
                exixtTemsFntEntry.Physicalservervendor = dataLoadTemsFnt.Physicalservervendor;
                exixtTemsFntEntry.Virtualserverhostedon = dataLoadTemsFnt.Virtualserverhostedon;
                exixtTemsFntEntry.Virtualservermanufacturer = dataLoadTemsFnt.Virtualservermanufacturer;
                exixtTemsFntEntry.Virtualservertypeofdevice = dataLoadTemsFnt.Virtualservertypeofdevice;
                exixtTemsFntEntry.Virtualmachinetype = dataLoadTemsFnt.Virtualmachinetype;
                exixtTemsFntEntry.Virtualserveripaddress = dataLoadTemsFnt.Virtualserveripaddress;
                exixtTemsFntEntry.Virtualserverserialnumber = dataLoadTemsFnt.Virtualserverserialnumber;
                exixtTemsFntEntry.Virtualservertype = dataLoadTemsFnt.Virtualservertype;
                exixtTemsFntEntry.Osname = dataLoadTemsFnt.Osname;
                exixtTemsFntEntry.Osversion = dataLoadTemsFnt.Osversion;
                exixtTemsFntEntry.Osstartdate = dataLoadTemsFnt.Osstartdate;
                exixtTemsFntEntry.Osinstallationdate = dataLoadTemsFnt.Osinstallationdate;
                exixtTemsFntEntry.Osstatus = dataLoadTemsFnt.Osstatus;
                exixtTemsFntEntry.Softwarename = dataLoadTemsFnt.Softwarename;
                exixtTemsFntEntry.Version = dataLoadTemsFnt.Version;
                exixtTemsFntEntry.Release = dataLoadTemsFnt.Release;
                exixtTemsFntEntry.Manufacturer = dataLoadTemsFnt.Manufacturer;
                exixtTemsFntEntry.Language = dataLoadTemsFnt.Language;
                exixtTemsFntEntry.Hwopscontractstatus = dataLoadTemsFnt.Hwopscontractstatus;
                exixtTemsFntEntry.Hwopscontractenddate = dataLoadTemsFnt.Hwopscontractenddate;
                exixtTemsFntEntry.Swopscontractstatus = dataLoadTemsFnt.Swopscontractstatus;
                exixtTemsFntEntry.Swopscontractenddate = dataLoadTemsFnt.Swopscontractenddate;
                exixtTemsFntEntry.Hwoperationscontactpoint = dataLoadTemsFnt.Hwoperationscontactpoint;
                exixtTemsFntEntry.Swoperationscontactpoint = dataLoadTemsFnt.Swoperationscontactpoint;
                exixtTemsFntEntry.Physicalserverosname = dataLoadTemsFnt.Physicalserverosname;
                exixtTemsFntEntry.Physicalserverosversion = dataLoadTemsFnt.Physicalserverosversion;
                exixtTemsFntEntry.Physicalserverosstartdate = dataLoadTemsFnt.Physicalserverosstartdate;
                exixtTemsFntEntry.Physicalserverosinstallationdate = dataLoadTemsFnt.Physicalserverosinstallationdate;
                exixtTemsFntEntry.Physicalserverosstatus = dataLoadTemsFnt.Physicalserverosstatus;
                exixtTemsFntEntry.Model = dataLoadTemsFnt.Model;

                #endregion
                return exixtTemsFntEntry;
            }
            catch
            {
                return new Temsfntreport();
            }
        }

        private async Task<List<Temsfntreport>> TemsFntReportColumnMapping(List<Networkelementsasplanned> query, List<Lcmoperationalcontracts> allOperationalContracts, List<Locations> location, Dictionary<long,long> assetIdAndOpcoId, List<IEnumerable<Majorhardwarebuilds>> hardWare,
           List<NetWorkElementAsPlannedSubDpomainSpoc> allSubDomain, List<NetWorkElementAsPlannedEduSpoc> allEdu, List<string> elementnames, List<HardwareConfigurations> hardwareconfigurations, IQueryable<Deploymentstatuses> deploymentStatuses, List<Designcomponents> designcomponents)
        {
            var result = new List<Temsfntreport>();
            try
            {
                result = await Task.Run(() => query.OrderBy(x => x.Modificationdate).ToList().Select(x =>
                {
                    var grid = new Temsfntreport();

                    grid.Hostname = x.Elementname;
                    grid.Serialnumberofhardwareasset = x.Swresourcekey + "_" + x.Hwresourcekey;
                    grid.Locationofhardwareasset = x.Opco.Opco;
                    grid.Hardwaretypeofhardwareasset = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Hardwaretype).FirstOrDefault();
                    grid.Vendor = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Orgeqpmanufacturer?.Originalequipmentmanufacturer).FirstOrDefault();
                    grid.Ipaddressofhardwareasset = x.Identitiesasis.Select(x => x.Value).FirstOrDefault();
                    grid.Market = x.Opco.Opco;
                    grid.Hwendoflife = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds ?
                                                            .Select(x => x.Majorhardware.Endofsupport)
                                                            .FirstOrDefault()?
                                                            .ToString(ConstantValueFilter.DateFormat) ?? string.Empty;
                    grid.Hwendofsupport = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(x => x.Majorhardware.Endofsupport).FirstOrDefault()?.ToString(ConstantValueFilter.DateFormat) ?? ConstantValueFilter.NotSpecified;
                    grid.Hwendofsale = string.Empty;
                    grid.Hardwaremodules = _genericReportGenration.GetHardwareModule(x.Elementname.Trim(), _repositoryWrapper);
                    grid.Softwareproducttype = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description;
                    grid.Softwareproductversion = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Softwareversion;
                    grid.Softwareisvirtualized = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Buildconstruction?.Iscloudasset).FirstOrDefault() == true? "Yes":"No";
                    grid.OperatingSystemofvirtualmachine = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Operatingsystem?.Operatingsystemname;
                    grid.Applicationhostedonsoftware = string.Empty;
                    grid.Uuidserialnumberofsoftware = string.Empty;
                    grid.Softwarevendor = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer; 
                    grid.Locationofsoftware = location.Where(y => y.Locationid == x.Locationid).Select(s => s.Location).FirstOrDefault(); 
                    grid.Servicetype = x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory;
                    grid.Swendoflife = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofsupport?.ToString(ConstantValueFilter.DateFormat) ?? string.Empty;
                    grid.Swendofsupport = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Eomstatus == (short)Enum.EOMEnum.Default ?
                                                                                           x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofsupport?.ToString(ConstantValueFilter.DateFormat)
                                                                                           : x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Eomstatus == (short)Enum.EOMEnum.NotAnnounced ? ConstantValueFilter.NotAnnounced : ConstantValueFilter.NotSpecified;
                    grid.Swendofsale = string.Empty;
                    grid.Verticalengineeringteam = string.Join(",", allSubDomain.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.VerticalDic != null && x.Deleted == false)
                                            .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList());
                    grid.Verticalsubdomain = string.Join(",", allSubDomain?.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.SubdomainresponsiblesDic != null && x.Deleted == false)
                                            .SelectMany(v => v.SubdomainresponsiblesDic.Select(t => t.Value)).Distinct().ToList());
                    grid.Platform = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Platform?.Platform).FirstOrDefault();
                    grid.Riskcluster = LCMEngineeringRulesExtension.GetRiskCluster(x.Designcomponent?.Systemtype?.Vodafonename, _repositoryWrapper);
                    grid.Operationscontactpoint = string.Join(" | ",
                    allOperationalContracts?.Where(m => m.Lcmid == x.Lcmengineeringid).Select(x => x?.Operationalcontract?.Description).Distinct());
                    grid.Assetcategory = x.Designcomponent?.Systemtype?.Assetcategory?.Assetcategory;
                    grid.Assetclass = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname?.Description;
                    grid.Assettype = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Criticalassettype?.Description;
                    grid.Assetdescription = x.Designcomponent?.Designcomponentfamily?.Description;
                    grid.Productimportance = x.Lcmengineering?.Productimportance?.Productimportance;
                    grid.Operationsmaintenancecontract = x.Lcmengineering?.Outputtolcmsoftware;
                    grid.Vendorendofmaintenancedate = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance?.ToString(ConstantValueFilter.DateFormat) ?? string.Empty;
                    grid.Identifiedaction = LCMEngineeringRulesExtension.GetIdentificationActionForTsr(x.Lcmengineering?.PlannedactivitiesLcmengineering?.OrderBy(x => x.Plannedcompletion).FirstOrDefault(), x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofmaintenance);
                    grid.Descriptionofplannedaction = x.Lcmengineering?.PlannedactivitiesLcmengineering?.GetPlannedAction(_repositoryWrapper);
                    grid.Model = hardwareconfigurations.Where(hw => hw.ElementName == x.Elementname).ToList().Count > 0 ? string.Join(";", hardwareconfigurations.Where(hw => hw.ElementName == x.Elementname)
                        .Select(x => x.HardwareType + " - " + x.ProductName).ToList()) : x.Designcomponent?.Systemtype?.toLcmDbExportHardwareName();
                    grid.Businessservicename = x.Designcomponent.toDesignComponentFamily();
                    grid.Opmaintenancecontractenddate = x.Lcmengineering?.Softwareendofwarrantydate?.ToString(ConstantValueFilter.DateFormat) ?? string.Empty;
                    grid.Incidentclass = x.Lcmengineering?.Lcmancillarydata?.Select(l => l.Incidentclass).FirstOrDefault();
                    grid.Occurrenceprobability = x.Lcmengineering?.Lcmancillarydata?.Select(l => l.Occurenceprobability).FirstOrDefault(); ;
                    grid.Meverticalresposible = string.Join(",", allSubDomain.Where(m => m.NetWorkElementAsPlannedId == x.Networkelementasplannedid && m.VerticalDic != null && x.Deleted == false)
                                            .SelectMany(v => v.VerticalDic.Select(t => t.Value)).Distinct().ToList()); 
                    grid.Assetstatus = deploymentStatuses.Where(y => y.Deploymentstatusid == x.Deploymentstatusid).Select(x => x.Deploymentstatus).FirstOrDefault();
                    grid.Typeofnetworkelement = x.Designcomponent?.Subnetworkboundary.Description;
                    grid.Localmarket = x.Opco?.Opco;
                    grid.Application = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Platform?.Platform).FirstOrDefault();
                    grid.Cloud = x.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware?.Buildconstruction?.Iscloudasset).FirstOrDefault() == true ? "Yes" : "No"; 
                    grid.Physicalserverhostname = string.Empty; //ncl
                    grid.Physicalserveripaddress = string.Empty; //ncl
                    grid.Physicalserverserialnumber = string.Empty;//ncl
                    grid.Physicalserverhwmodel = string.Empty;//ncl
                    grid.Physicalservervendor = string.Empty;//ncl
                    grid.Virtualserverhostedon = string.Empty;//ncl
                    grid.Virtualservermanufacturer = string.Empty;//ncl
                    grid.Virtualservertypeofdevice = string.Empty;//ncl
                    grid.Virtualmachinetype = string.Empty;//ncl
                    grid.Virtualserveripaddress = string.Empty;
                    grid.Virtualserverserialnumber = string.Empty;//ncl
                    grid.Virtualservertype = string.Empty;//ncl
                    grid.Osname = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Operatingsystem?.Operatingsystemname;
                    grid.Osversion = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Operatingsystem?.Operatingsystemversion;
                    grid.Osstartdate = string.Empty;//ncl
                    grid.Osinstallationdate = string.Empty;//ncl
                    grid.Osstatus = string.Empty;//ncl
                    grid.Softwarename = string.Empty;//ncl
                    grid.Version = string.Empty;//ncl
                    grid.Release = string.Empty;//ncl
                    grid.Manufacturer = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer;
                    grid.Language = string.Empty;//ncl
                    grid.Hwopscontractstatus = x.Lcmengineering?.Outputtolcmhardware != null? x.Lcmengineering?.Outputtolcmhardware.Trim():string.Empty;
                    grid.Hwopscontractenddate = x.Lcmengineering?.Hardwareendofsupportcontract?.ToString(ConstantValueFilter.DateFormat) ?? string.Empty; 
                    grid.Swopscontractstatus = x.Lcmengineering?.Outputtolcmsoftware;
                    grid.Swopscontractenddate = x.Lcmengineering?.Softwareendofsupportcontract?.ToString(ConstantValueFilter.DateFormat) ?? string.Empty;
                    grid.Hwoperationscontactpoint = string.Join(" | ",
                    allOperationalContracts?.Where(m => m.Lcmid == x.Lcmengineeringid).Select(x => x?.Operationalcontract?.Description).Distinct());
                    grid.Swoperationscontactpoint = string.Join(" | ",
                    allOperationalContracts?.Where(m => m.Lcmid == x.Lcmengineeringid).Select(x => x?.Operationalcontract?.Description).Distinct());
                    grid.Physicalserverosname = string.Empty;
                    grid.Physicalserverosversion = string.Empty;
                    grid.Physicalserverosstartdate = string.Empty;
                    grid.Physicalserverosinstallationdate = string.Empty;
                    grid.Physicalserverosstatus = string.Empty;
                    grid.Plannedhwmodel = designcomponents.Where(f => f.Designcomponentid == x.Lcmengineering?.PlannedactivitiesLcmengineering?.Select(p => p.Designcomponentid).FirstOrDefault())
                    .Select(d => d.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(h => h.Majorhardware?.Hardwaretype).FirstOrDefault()).FirstOrDefault();



                    return grid;
                }).ToList());
                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return result;
            }
        }

        #endregion

        #region Grid View
        public async Task<QueryResultDto<TemsFntReportDtoGrid>> FindWithCondition(TemsFntReportQueryDto fntQueryDto, bool isExport = false)
        {

            var predicateResult = ApplyFilter(fntQueryDto);

            var query = await Task.Run(() => GetFntQuery(predicateResult).AsQueryable());

            if ((fntQueryDto.MeverticalResposible?.Any() == true) || (fntQueryDto.VerticalEngineeringTeam?.Any()==true))
            {
                var queryData = query.ToList();
                var verticalResponsible = _repositoryWrapper.VerticalResponsible.FindAll().Select(x => new { x.Verticalresponsibleid, x.Verticalresponsible }).ToList();
                foreach (var item in queryData)
                {
                    if(!string.IsNullOrEmpty(item.Meverticalresposible))
                    {
                        var verticalResponsibleCommaSeparatedList = item.Meverticalresposible.Split(',').ToList();
                        item.verticalResponsible = verticalResponsibleCommaSeparatedList != null && verticalResponsibleCommaSeparatedList.Count > 0 ? 
                                                   verticalResponsible.Where(x => verticalResponsibleCommaSeparatedList.Contains(x.Verticalresponsible))
                                                   .Select(x => new FilterValueDtoKeyValueList 
                                                   { Value = x.Verticalresponsible, Key = x.Verticalresponsibleid }).Distinct().ToList() : null;
                    }

                }
                var filteredData = fntQueryDto.MeverticalResposible?.Any() == true ? 
                                   queryData.Where(x => x.verticalResponsible != null && 
                                   x.verticalResponsible.Any(c => fntQueryDto.MeverticalResposible.Contains(c.Key.ToString()))).ToList():null;
                var filteredDataVerticalEngineering = fntQueryDto.VerticalEngineeringTeam?.Any() == true ?
                                                      queryData.Where(x => x.verticalResponsible != null &&
                                                      x.verticalResponsible.Any(c => fntQueryDto.VerticalEngineeringTeam.Contains(c.Key.ToString()))).ToList() : null;

                if (filteredData != null && filteredData.Count > 0 && filteredDataVerticalEngineering != null && filteredDataVerticalEngineering.Count > 0)
                    queryData = filteredData.Union(filteredDataVerticalEngineering).ToList();
                else if(filteredData != null && filteredData.Count > 0 )
                    queryData = filteredData;
                else if (filteredDataVerticalEngineering != null && filteredDataVerticalEngineering.Count > 0)
                    queryData = filteredDataVerticalEngineering;
                query = queryData.AsQueryable();
            }
            var orderedData = query.ApplyOrdering(fntQueryDto, GetColumnsMapDB()).OrderByDescending(x => x.Modificationdate).ToList();
            var rtn = new QueryResultDto<TemsFntReportDtoGrid>(new GenerateRenderForGrid<TemsFntReportDtoGrid>(_gridmanager))
            {
                TotalItems = query.Count()
            };
            //orderedData = orderedData.ApplyPaginationList(fntQueryDto);
            if (!isExport)
            {
                orderedData = orderedData.Skip((fntQueryDto.Page - 1) * fntQueryDto.PageSize).Take(fntQueryDto.PageSize).ToList();
            }


            IEnumerable<TemsFntReportDtoGrid> dto;

            dto = _mapper.Map<IEnumerable<TemsFntReportDtoGrid>>(orderedData.ToList());

            rtn.Items = dto.ToArray();

            return rtn;
        }

        public ExpressionStarter<Temsfntreport> ApplyFilter(TemsFntReportQueryDto fntQueryDto)
        {
            var mainPredicate = PredicateBuilder.New<Temsfntreport>(true);

            if (fntQueryDto.TemsFntReportId?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.TemsFntReportId)
                    resultPredicate.Or(x => x.Temsfntreportid == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HostName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HostName)
                    resultPredicate.Or(x => x.Hostname == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SerialNumberOfHardwareAsset?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SerialNumberOfHardwareAsset)
                    resultPredicate.Or(x => x.Serialnumberofhardwareasset == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.LocationOfHardwareAsset?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.LocationOfHardwareAsset)
                    resultPredicate.Or(x => x.Locationofhardwareasset == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HardwareTypeOfHardwareAsset?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HardwareTypeOfHardwareAsset)
                    resultPredicate.Or(x => x.Hardwaretypeofhardwareasset == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Vendor?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Vendor)
                    resultPredicate.Or(x => x.Vendor == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.IpAddressOfHardwareAsset?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.IpAddressOfHardwareAsset)
                    resultPredicate.Or(x => x.Ipaddressofhardwareasset == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Market?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Market)
                    resultPredicate.Or(x => x.Market == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HwEndOfLife?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HwEndOfLife)
                    resultPredicate.Or(x => x.Hwendoflife == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HwEndOfSupport?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HwEndOfSupport)
                    resultPredicate.Or(x => x.Hwendofsupport == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HwEndOfSale?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HwEndOfSale)
                    resultPredicate.Or(x => x.Hwendofsale == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HardwareModules?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HardwareModules)
                    resultPredicate.Or(x => x.Hardwaremodules == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SoftwareProductType?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SoftwareProductType)
                    resultPredicate.Or(x => x.Softwareproducttype == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SoftwareProductVersion?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SoftwareProductVersion)
                    resultPredicate.Or(x => x.Softwareproductversion == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SoftwareIsVirtualized?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SoftwareIsVirtualized)
                    resultPredicate.Or(x => x.Softwareisvirtualized == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OperatingSystemOfVirtualMachine?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OperatingSystemOfVirtualMachine)
                    resultPredicate.Or(x => x.OperatingSystemofvirtualmachine == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.ApplicationHostedOnSoftware?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.ApplicationHostedOnSoftware)
                    resultPredicate.Or(x => x.Applicationhostedonsoftware == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.UuidSerialNumberOfSoftware?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.UuidSerialNumberOfSoftware)
                    resultPredicate.Or(x => x.Uuidserialnumberofsoftware == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SoftwareVendor?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SoftwareVendor)
                    resultPredicate.Or(x => x.Softwarevendor == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.LocationOfSoftware?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.LocationOfSoftware)
                    resultPredicate.Or(x => x.Locationofsoftware == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.ServiceType?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.ServiceType)
                    resultPredicate.Or(x => x.Servicetype == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SwEndOfLife?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SwEndOfLife)
                    resultPredicate.Or(x => x.Swendoflife == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SwEndOfSupport?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SwEndOfSupport)
                    resultPredicate.Or(x => x.Swendofsupport == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SwEndOfSale?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SwEndOfSale)
                    resultPredicate.Or(x => x.Swendofsale == item);
                mainPredicate.And(resultPredicate);
            }
            //if (fntQueryDto.VerticalEngineeringTeam?.Any() == true)
            //{
            //    var resultPredicate = PredicateBuilder.New<Temsfntreport>();
            //    foreach (var item in fntQueryDto.VerticalEngineeringTeam)
            //        resultPredicate.Or(x => x.Verticalengineeringteam == item);
            //    mainPredicate.And(resultPredicate);
            //}
            if (fntQueryDto.VerticalSubdomain?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VerticalSubdomain)
                    resultPredicate.Or(x => x.Verticalsubdomain == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Platform?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Platform)
                    resultPredicate.Or(x => x.Platform == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.RiskCluster?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.RiskCluster)
                    resultPredicate.Or(x => x.Riskcluster == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OperationsContactPoint?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OperationsContactPoint)
                    resultPredicate.Or(x => x.Operationscontactpoint == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.AssetCategory?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.AssetCategory)
                    resultPredicate.Or(x => x.Assetcategory == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.AssetClass?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.AssetClass)
                    resultPredicate.Or(x => x.Assetclass == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.AssetType?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.AssetType)
                    resultPredicate.Or(x => x.Assettype == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.AssetDescription?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.AssetDescription)
                    resultPredicate.Or(x => x.Assetdescription == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.ProductImportance?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.ProductImportance)
                    resultPredicate.Or(x => x.Productimportance == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OperationsMaintenanceContract?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OperationsMaintenanceContract)
                    resultPredicate.Or(x => x.Operationsmaintenancecontract == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VendorEndOfMaintenanceDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VendorEndOfMaintenanceDate)
                    resultPredicate.Or(x => x.Vendorendofmaintenancedate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.IdentifiedAction?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.IdentifiedAction)
                    resultPredicate.Or(x => x.Identifiedaction == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.DescriptionOfPlannedAction?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.DescriptionOfPlannedAction)
                    resultPredicate.Or(x => x.Descriptionofplannedaction == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PlannedHwModel?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PlannedHwModel)
                    resultPredicate.Or(x => x.Plannedhwmodel == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.BusinessServiceName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.BusinessServiceName)
                    resultPredicate.Or(x => x.Businessservicename == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OpMaintenanceContractendDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OpMaintenanceContractendDate)
                    resultPredicate.Or(x => x.Opmaintenancecontractenddate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.IncidentClass?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.IncidentClass)
                    resultPredicate.Or(x => x.Incidentclass == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OccurrenceProbability?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OccurrenceProbability)
                    resultPredicate.Or(x => x.Occurrenceprobability == item);
                mainPredicate.And(resultPredicate);
            }
            //if (fntQueryDto.MeverticalResposible?.Any() == true)
            //{
            //    var resultPredicate = PredicateBuilder.New<Temsfntreport>();
            //    foreach (var item in fntQueryDto.MeverticalResposible)
            //        resultPredicate.Or(x => x.Meverticalresposible.Contains(item));
            //    mainPredicate.And(resultPredicate);
            //}
            if (fntQueryDto.AssetStatus?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.AssetStatus)
                    resultPredicate.Or(x => x.Assetstatus == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.TypeOfNetworkElement?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.TypeOfNetworkElement)
                    resultPredicate.Or(x => x.Typeofnetworkelement == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.LocalMarket?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.LocalMarket)
                    resultPredicate.Or(x => x.Localmarket == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Application?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Application)
                    resultPredicate.Or(x => x.Application == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Cloud?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Cloud)
                    resultPredicate.Or(x => x.Cloud == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Model?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Model)
                    resultPredicate.Or(x => x. Model == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerHostname?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerHostname)
                    resultPredicate.Or(x => x.Physicalserverhostname == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerIpaddress?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerIpaddress)
                    resultPredicate.Or(x => x.Physicalserveripaddress == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerSerialNumber?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerSerialNumber)
                    resultPredicate.Or(x => x.Physicalserverserialnumber == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerHwModel?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerHwModel)
                    resultPredicate.Or(x => x.Physicalserverhwmodel == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerVendor?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerVendor)
                    resultPredicate.Or(x => x.Physicalservervendor == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VirtualServerHostedOn?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VirtualServerHostedOn)
                    resultPredicate.Or(x => x.Virtualserverhostedon == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VirtualServerManufacturer?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VirtualServerManufacturer)
                    resultPredicate.Or(x => x.Virtualservermanufacturer == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VirtualServerTypeOfDevice?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VirtualServerTypeOfDevice)
                    resultPredicate.Or(x => x.Virtualservertypeofdevice == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VirtualMachineType?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VirtualMachineType)
                    resultPredicate.Or(x => x.Virtualmachinetype == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VirtualServerIpaddress?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VirtualServerIpaddress)
                    resultPredicate.Or(x => x.Virtualserveripaddress == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VirtualServerSerialNumber?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VirtualServerSerialNumber)
                    resultPredicate.Or(x => x.Virtualserverserialnumber == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.VirtualServerType?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.VirtualServerType)
                    resultPredicate.Or(x => x.Virtualservertype == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OsName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OsName)
                    resultPredicate.Or(x => x.Osname == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OsVersion?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OsVersion)
                    resultPredicate.Or(x => x.Osversion == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OsStartDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OsStartDate)
                    resultPredicate.Or(x => x.Osstartdate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OsInstallationDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OsInstallationDate)
                    resultPredicate.Or(x => x.Osinstallationdate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.OsStatus?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.OsStatus)
                    resultPredicate.Or(x => x.Osstatus == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SoftwareName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SoftwareName)
                    resultPredicate.Or(x => x.Softwarename == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Version?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Version)
                    resultPredicate.Or(x => x.Version == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Release?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Release)
                    resultPredicate.Or(x => x.Release == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Manufacturer?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Manufacturer)
                    resultPredicate.Or(x => x.Manufacturer == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.Language?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.Language)
                    resultPredicate.Or(x => x.Language == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.CreationUser?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.CreationUser)
                    resultPredicate.Or(x => x.CreationuserNavigation.Email == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.CreationDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                if (fntQueryDto.CreationDate.StartDate != null)
                    resultPredicate.And(x => x.Creationdate >= fntQueryDto.CreationDate.StartDate);
                if (fntQueryDto.CreationDate.EndDate != null)
                    resultPredicate.And(x => x.Creationdate >= fntQueryDto.CreationDate.EndDate);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.ModificationUser?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.ModificationUser)
                    resultPredicate.Or(x => x.ModificationuserNavigation.Email == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.ModificationDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                if (fntQueryDto.ModificationDate.StartDate != null)
                    resultPredicate.And(x => x.Modificationdate >= fntQueryDto.ModificationDate.StartDate);
                if (fntQueryDto.ModificationDate.EndDate != null)
                    resultPredicate.And(x => x.Modificationdate >= fntQueryDto.ModificationDate.EndDate);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HwOpsContractStatus?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HwOpsContractStatus)
                    resultPredicate.Or(x => x.Hwopscontractstatus == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HwOpsContractEndDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HwOpsContractEndDate)
                    resultPredicate.Or(x => x.Hwopscontractenddate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SwOpsContractStatus?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SwOpsContractStatus)
                    resultPredicate.Or(x => x.Swopscontractstatus == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SwOpsContractEndDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SwOpsContractEndDate)
                    resultPredicate.Or(x => x.Swopscontractenddate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.HwOperationsContactPoint?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.HwOperationsContactPoint)
                    resultPredicate.Or(x => x.Hwoperationscontactpoint == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.SwOperationsContactPoint?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.SwOperationsContactPoint)
                    resultPredicate.Or(x => x.Swoperationscontactpoint == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerOsName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerOsName)
                    resultPredicate.Or(x => x.Physicalserverosname == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerOsVersion?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerOsVersion)
                    resultPredicate.Or(x => x.Physicalserverosversion == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerOsStartDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerOsStartDate)
                    resultPredicate.Or(x => x.Physicalserverosstartdate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerOsInstallationDate?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerOsInstallationDate)
                    resultPredicate.Or(x => x.Physicalserverosinstallationdate == item);
                mainPredicate.And(resultPredicate);
            }
            if (fntQueryDto.PhysicalServerOsStatus?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Temsfntreport>();
                foreach (var item in fntQueryDto.PhysicalServerOsStatus)
                    resultPredicate.Or(x => x.Physicalserverosstatus == item);
                mainPredicate.And(resultPredicate);
            }

            return mainPredicate;
        }


        private IQueryable<TemsFntReport> GetFntQuery(ExpressionStarter<Temsfntreport> predicateResult)
        {
            var result =  _repositoryWrapper.TemsFntReportRepository.FindByCondition(predicateResult).AsSplitQuery().AsNoTracking()
                .Include(x => x.ModificationuserNavigation)
                .Include(x => x.CreationuserNavigation);

            return result.AsEnumerable().Select(x => TemsFntReportMapper.GetTemsFntReportMapper(x)).AsQueryable();

        }

        private Dictionary<string, Expression<Func<TemsFntReport, object>>[]> GetColumnsMapDB()
        {
            return new Dictionary<string, Expression<Func<TemsFntReport, object>>[]>
            {
                ["hostName"] = new Expression<Func<TemsFntReport, object>>[] { p => p.Hostname },
                ["serialNumberOfHardwareAsset"] = new Expression<Func<TemsFntReport, object>>[] { p => p.Serialnumberofhardwareasset},
                ["modificationDate"] = new Expression<Func<TemsFntReport, object>>[] { p => p.Modificationdate },

            };
        }




        #endregion

        #region Filter View

        public async Task<List<FilterValueDto>> GetFilteredValues(string propertyName, string propertyFilter, TemsFntReportQueryDto fntQueryDto,bool isAdmin=false, List<string> verticalList=null)
        {
            var filterCriteria = ApplyFilter(fntQueryDto);

            var filteredQuery = await Task.Run(() => GetFntQuery(filterCriteria).AsQueryable());
            if (propertyName== "meverticalResposible" || propertyName == "verticalEngineeringTeam")
            {
                var queryInList = filteredQuery.ToList();
                var verticalResponsible = _repositoryWrapper.VerticalResponsible.FindAll()
                                          .Select(x => new { x.Verticalresponsibleid, x.Verticalresponsible }).ToList();
                foreach (var item in queryInList)
                {
                    if (!string.IsNullOrEmpty(item.Meverticalresposible))
                    {
                        var verticalResponsibleCommaSeparatedList = item.Meverticalresposible.Split(',').ToList();
                        item.verticalResponsible = verticalResponsibleCommaSeparatedList != null && verticalResponsibleCommaSeparatedList.Count > 0 ?
                                                   verticalResponsible.Where(x => verticalResponsibleCommaSeparatedList.Contains(x.Verticalresponsible))
                                                   .Select(x => new FilterValueDtoKeyValueList
                                                   { Value = x.Verticalresponsible, Key = x.Verticalresponsibleid }).Distinct().ToList() : null;
                    }
                }

                var filteredData = fntQueryDto.MeverticalResposible?.Any() == true ?
                                   queryInList.Where(x => x.verticalResponsible != null &&
                                   x.verticalResponsible.Any(c => fntQueryDto.MeverticalResposible.Contains(c.Key.ToString()))).ToList() : null;
                var filteredDataVerticalEngineering = fntQueryDto.VerticalEngineeringTeam?.Any() == true ?
                                                      queryInList.Where(x => x.verticalResponsible != null &&
                                                      x.verticalResponsible.Any(c => fntQueryDto.VerticalEngineeringTeam.Contains(c.Key.ToString()))).ToList() : null;

                if (filteredData != null && filteredData.Count > 0 && filteredDataVerticalEngineering != null && filteredDataVerticalEngineering.Count > 0)
                    queryInList = filteredData.Union(filteredDataVerticalEngineering).ToList();
                else if (filteredData != null && filteredData.Count > 0)
                    queryInList = filteredData;
                else if (filteredDataVerticalEngineering != null && filteredDataVerticalEngineering.Count > 0)
                    queryInList = filteredDataVerticalEngineering;
                var returnVertical = queryInList.Where(x => x.verticalResponsible != null && x.verticalResponsible.Any() == true)
                                     .SelectMany(y => y.verticalResponsible.Select(y => new FilterValueDto { Text=y.Value,Value=y.Key.ToString()})).Distinct().ToList();


                if (!isAdmin && (propertyName == "meverticalResposible"))
                {
                    if (fntQueryDto.MeverticalResposible != null && fntQueryDto.MeverticalResposible.Count ==0)
                        fntQueryDto.MeverticalResposible = verticalList;
                    if (fntQueryDto.MeverticalResposible != null && fntQueryDto.MeverticalResposible.Count > 0)
                        returnVertical = returnVertical.Where(x => fntQueryDto.MeverticalResposible.Contains(x.Value.ToString())).ToList();
                }
                else if (!isAdmin && (propertyName == "verticalEngineeringTeam"))
                {
                    if (fntQueryDto.VerticalEngineeringTeam != null && fntQueryDto.VerticalEngineeringTeam.Count == 0)
                        fntQueryDto.VerticalEngineeringTeam = verticalList;
                    if (fntQueryDto.VerticalEngineeringTeam != null && fntQueryDto.VerticalEngineeringTeam.Count > 0)
                        returnVertical = returnVertical.Where(x => fntQueryDto.VerticalEngineeringTeam.Contains(x.Value.ToString())).ToList();
                }

                return returnVertical;
            }

            var result = propertyName switch
            {
                #region
                "temsFntReportId" => filteredQuery
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Temsfntreportid.ToString().Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Temsfntreportid))
                    .Distinct()
                    .ToList(),
                "hostName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hostname.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hostname))
                                    .Distinct()
                                    .ToList(),
                "serialNumberOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Serialnumberofhardwareasset.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Serialnumberofhardwareasset))
                                    .Distinct()
                                    .ToList(),
                "locationOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Locationofhardwareasset.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Locationofhardwareasset))
                                    .Distinct()
                                    .ToList(),
                "hardwareTypeOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hardwaretypeofhardwareasset.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hardwaretypeofhardwareasset))
                                    .Distinct()
                                    .ToList(),
                "vendor" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vendor.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Vendor))
                                    .Distinct()
                                    .ToList(),
                "ipAddressOfHardwareAsset" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Ipaddressofhardwareasset.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Ipaddressofhardwareasset))
                                    .Distinct()
                                    .ToList(),
                "market" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Market.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Market))
                                    .Distinct()
                                    .ToList(),
                "hwEndOfLife" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hwendoflife.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hwendoflife))
                                    .Distinct()
                                    .ToList(),
                "hwEndOfSupport" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hwendofsupport.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hwendofsupport))
                                    .Distinct()
                                    .ToList(),
                "hwEndOfSale" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hwendofsale.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hwendofsale))
                                    .Distinct()
                                    .ToList(),
                "hardwareModules" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hardwaremodules.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hardwaremodules))
                                    .Distinct()
                                    .ToList(),
                "softwareProductType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Softwareproducttype.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Softwareproducttype))
                                    .Distinct()
                                    .ToList(),
                "softwareProductVersion" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Softwareproductversion.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Softwareproductversion))
                                    .Distinct()
                                    .ToList(),
                "softwareIsVirtualized" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Softwareisvirtualized.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Softwareisvirtualized))
                                    .Distinct()
                                    .ToList(),
                "operatingSystemOfVirtualMachine" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OperatingSystemofvirtualmachine.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.OperatingSystemofvirtualmachine))
                                    .Distinct()
                                    .ToList(),
                "applicationHostedOnSoftware" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Applicationhostedonsoftware.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Applicationhostedonsoftware))
                                    .Distinct()
                                    .ToList(),
                "uuidSerialNumberOfSoftware" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Uuidserialnumberofsoftware.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Uuidserialnumberofsoftware))
                                    .Distinct()
                                    .ToList(),
                "softwareVendor" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Softwarevendor.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Softwarevendor))
                                    .Distinct()
                                    .ToList(),
                "locationOfSoftware" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Locationofsoftware.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Locationofsoftware))
                                    .Distinct()
                                    .ToList(),
                "serviceType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Servicetype.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Servicetype))
                                    .Distinct()
                                    .ToList(),
                "swEndOfLife" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Swendoflife.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Swendoflife))
                                    .Distinct()
                                    .ToList(),
                "swEndOfSupport" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Swendofsupport.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Swendofsupport))
                                    .Distinct()
                                    .ToList(),
                "swEndOfSale" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Swendofsale.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Swendofsale))
                                    .Distinct()
                                    .ToList(),
                //"verticalEngineeringTeam" => filteredQuery
                //                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Verticalengineeringteam.Contains(propertyFilter))
                //                    .Select(x => new FilterValueDto(x.Verticalengineeringteam))
                //                    .Distinct()
                //                    .ToList(),
                "verticalSubdomain" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Verticalsubdomain.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Verticalsubdomain))
                                    .Distinct()
                                    .ToList(),
                "platform" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Platform.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Platform))
                                    .Distinct()
                                    .ToList(),
                "riskCluster" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Riskcluster.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Riskcluster))
                                    .Distinct()
                                    .ToList(),
                "operationsContactPoint" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Operationscontactpoint.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Operationscontactpoint))
                                    .Distinct()
                                    .ToList(),
                "assetCategory" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Assetcategory.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Assetcategory))
                                    .Distinct()
                                    .ToList(),
                "assetClass" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Assetclass.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Assetclass))
                                    .Distinct()
                                    .ToList(),
                "assetType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Assettype.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Assettype))
                                    .Distinct()
                                    .ToList(),
                "assetDescription" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Assetdescription.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Assetdescription))
                                    .Distinct()
                                    .ToList(),
                "productImportance" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Productimportance.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Productimportance))
                                    .Distinct()
                                    .ToList(),
                "operationsMaintenanceContract" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Operationsmaintenancecontract.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Operationsmaintenancecontract))
                                    .Distinct()
                                    .ToList(),
                "vendorEndOfMaintenanceDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vendorendofmaintenancedate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Vendorendofmaintenancedate))
                                    .Distinct()
                                    .ToList(),
                "identifiedAction" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Identifiedaction.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Identifiedaction))
                                    .Distinct()
                                    .ToList(),
                "descriptionOfPlannedAction" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Descriptionofplannedaction.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Descriptionofplannedaction))
                                    .Distinct()
                                    .ToList(),
                "plannedHwModel" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Plannedhwmodel.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Plannedhwmodel))
                                    .Distinct()
                                    .ToList(),
                "businessServiceName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Businessservicename.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Businessservicename))
                                    .Distinct()
                                    .ToList(),
                "opMaintenanceContractendDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Opmaintenancecontractenddate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Opmaintenancecontractenddate))
                                    .Distinct()
                                    .ToList(),
                "incidentClass" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Incidentclass.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Incidentclass))
                                    .Distinct()
                                    .ToList(),
                "occurrenceProbability" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Occurrenceprobability.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Occurrenceprobability))
                                    .Distinct()
                                    .ToList(),
                //"meverticalResposible" => filteredQuery
                //                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Meverticalresposible.Contains(propertyFilter))
                //                    .Select(x => new FilterValueDto(x.Meverticalresposible))
                //                    .Distinct()
                //                    .ToList(),
                "assetStatus" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Assetstatus.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Assetstatus))
                                    .Distinct()
                                    .ToList(),
                "typeOfNetworkElement" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Typeofnetworkelement.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Typeofnetworkelement))
                                    .Distinct()
                                    .ToList(),
                "localMarket" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Localmarket.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Localmarket))
                                    .Distinct()
                                    .ToList(),
                "application" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Application.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Application))
                                    .Distinct()
                                    .ToList(),
                "cloud" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Cloud.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Cloud))
                                    .Distinct()
                                    .ToList(),
                "dataCenterocation" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.DatacenterLocation.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.DatacenterLocation))
                                    .Distinct()
                                    .ToList(),
                "physicalServerHostname" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverhostname.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverhostname))
                                    .Distinct()
                                    .ToList(),
                "physicalServerIpaddress" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserveripaddress.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserveripaddress))
                                    .Distinct()
                                    .ToList(),
                "physicalServerSerialNumber" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverserialnumber.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverserialnumber))
                                    .Distinct()
                                    .ToList(),
                "physicalServerHwModel" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverhwmodel.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverhwmodel))
                                    .Distinct()
                                    .ToList(),
                "physicalServerVendor" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalservervendor.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalservervendor))
                                    .Distinct()
                                    .ToList(),
                "virtualServerHostedOn" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Virtualserverhostedon.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Virtualserverhostedon))
                                    .Distinct()
                                    .ToList(),
                "virtualServerManufacturer" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Virtualservermanufacturer.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Virtualservermanufacturer))
                                    .Distinct()
                                    .ToList(),
                "virtualServerTypeOfDevice" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Virtualservertypeofdevice.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Virtualservertypeofdevice))
                                    .Distinct()
                                    .ToList(),
                "virtualMachineType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Virtualmachinetype.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Virtualmachinetype))
                                    .Distinct()
                                    .ToList(),
                "virtualServerIpaddress" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Virtualserveripaddress.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Virtualserveripaddress))
                                    .Distinct()
                                    .ToList(),
                "virtualServerSerialNumber" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Virtualserverserialnumber.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Virtualserverserialnumber))
                                    .Distinct()
                                    .ToList(),
                "virtualServerType" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Virtualservertype.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Virtualservertype))
                                    .Distinct()
                                    .ToList(),
                "osName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Osname.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Osname))
                                    .Distinct()
                                    .ToList(),
                "osVersion" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Osversion.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Osversion))
                                    .Distinct()
                                    .ToList(),
                "osStartDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Osstartdate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Osstartdate))
                                    .Distinct()
                                    .ToList(),
                "osInstallationDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Osinstallationdate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Osinstallationdate))
                                    .Distinct()
                                    .ToList(),
                "osStatus" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Osstatus.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Osstatus))
                                    .Distinct()
                                    .ToList(),
                "softwareName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Softwarename.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Softwarename))
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
                "manufacturer" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Manufacturer.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Manufacturer))
                                    .Distinct()
                                    .ToList(),
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
                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hwopscontractstatus.Contains(propertyFilter))
                    .Select(x => new FilterValueDto(x.Hwopscontractstatus))
                    .Distinct()
                    .ToList(),
                "hwOpsContractEndDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hwopscontractenddate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hwopscontractenddate))
                                    .Distinct()
                                    .ToList(),
                "swOpsContractStatus" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Swopscontractstatus.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Swopscontractstatus))
                                    .Distinct()
                                    .ToList(),
                "swOpsContractEndDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Swopscontractenddate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Swopscontractenddate))
                                    .Distinct()
                                    .ToList(),
                "hwOperationsContactPoint" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Hwoperationscontactpoint.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Hwoperationscontactpoint))
                                    .Distinct()
                                    .ToList(),
                "swOperationsContactPoint" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Swoperationscontactpoint.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Swoperationscontactpoint))
                                    .Distinct()
                                    .ToList(),
                "physicalServerOsName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverosname.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverosname))
                                    .Distinct()
                                    .ToList(),
                "physicalServerOsVersion" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverosversion.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverosversion))
                                    .Distinct()
                                    .ToList(),
                "physicalServerOsStartDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverosstartdate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverosstartdate))
                                    .Distinct()
                                    .ToList(),
                "physicalServerOsInstallationDate" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverosinstallationdate.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverosinstallationdate))
                                    .Distinct()
                                    .ToList(),
                "physicalServerOsStatus" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Physicalserverosstatus.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Physicalserverosstatus))
                                    .Distinct()
                                    .ToList(),
                "model" => filteredQuery
               .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Model.Contains(propertyFilter))
               .Select(x => new FilterValueDto(x.Model))
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
