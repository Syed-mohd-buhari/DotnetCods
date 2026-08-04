using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Grid.QueryResultImplementation;
using CAM.BusinessManager.ILookUp;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.ExodusAssetLevelReport;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.ExodusAssetLevelReport;
using CAM.Enum;
using CAM.Infrastucture.QueryResult;
using IdentityServer4.Extensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.ExodusAssetLevelReport
{
    public class ExodusAssetLevelReportManager : BaseManager
    {

        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly GridCustomColumnManager _customColumnManager;
        protected readonly ILoggerManager _logger;
        private readonly CommonManager _commonManager;
        private readonly DapperCommonManager _dapperCommonManager;
        public ExodusAssetLevelReportManager(IEnumerable<IRepositoryWrapper> wrappers,
            GridCustomColumnManager customColumnManager,
            IRepositoryWrapper repositoryWrapper,
            IProductNameManager productNameManager,
            ILoggerManager logger,
            IHttpContextAccessor contextAccessor,
            CommonManager commonManager,   DapperCommonManager dapperCommonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _customColumnManager = customColumnManager;
            _logger = logger;
            _commonManager = commonManager;
            _dapperCommonManager = dapperCommonManager;

        }

        private IQueryable<Networkelementsasplanned> GetRecords(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {
            var excludedOpcoList = _dapperCommonManager.GetRestrictedOpcoAsync().Result;
            var excludedOpcoid = excludedOpcoList?.Select(x => x.Opcoid).ToList();

                var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
                var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();


                var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
                var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();

            var query = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult)
                .Where(f =>
                !excludedOpcoid.Contains(f.Opcoid) 
                && f.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwOem.Contains(y.Majorhardware.Orgeqpmanufacturerid))
                && f.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwPlatform.Contains(y.Majorhardware.Platformid))
                && f.Designcomponent.Systemtype.Majorsoftwarebuilds.Isvmware == false &&
                 ConstantValueFilter.assetDeployementStatusForPlatformMigration.Contains(f.Deploymentstatus.Deploymentstatus.ToLower().Trim()))
                .AsQueryable()
                .Include(x => x.Daassetmigration).ThenInclude(x => x.Plannedactivity).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                .Include(x => x.Daassetmigration).ThenInclude(x => x.Plannedactivity).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype)
                .ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Location)
                .Include(x => x.Environment)
                .Include(x => x.Opco)
                .Include(x => x.Deploymentstatus)
                .Include(x => x.Designcomponentfamily)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Assethardwareancillary).ThenInclude(x => x.Majorhardwarebuildasis).ThenInclude(x => x.Platform)
                .Include(x=>x.Networkelementasplannedsubdomainspoc).ThenInclude(x=>x.Subdomainspoc).ThenInclude(x=>x.AspnetuserverticalsUser).ThenInclude(x=>x.Organisation);
             
            return query;
        }
        private IQueryable<Networkelementsasplanned> GetTargetRecords(ExpressionStarter<Networkelementsasplanned> predicateResult)
        {

            var query = _repositoryWrapper.NetworkElementAsPlanned.FindByCondition(predicateResult)
                .AsQueryable()
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                .Include(x => x.Deploymentstatus);

            return query;
        }
        private IQueryable<Plannedactivities> GetTargetPaRecords(ExpressionStarter<Plannedactivities> predicateResult)
        {

            var query = _repositoryWrapper.PlannedActivity.FindByCondition(predicateResult)
                .AsQueryable()
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform);

            return query;
        }
        public ExpressionStarter<Networkelementsasplanned> ApplyFilter(ExodusAssetLevelReportQueryDto filterDto)
        {
            var mainPredicate = PredicateBuilder.New<Networkelementsasplanned>(true);

            if (filterDto.VerticalName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.VerticalName)
                    resultPredicate.Or(x => x.Networkelementasplannedsubdomainspoc.Any(y=>y.Subdomainspoc.AspnetuserverticalsUser
                                            .Any(z=>z.Organisation.Verticalid.ToString() == item && z.Deleted==false)));

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.OpCo?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.OpCo)
                    resultPredicate.Or(x => x.Opcoid == item);

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.DcfName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.DcfName)
                    resultPredicate.Or(x => x.Designcomponentfamilyid == item || x.Designcomponent.Designcomponentfamilyid == item);

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.SiteName?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.SiteName)
                    resultPredicate.Or(x => x.Locationid == item);

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.Vendor?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.Vendor)
                    resultPredicate.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturer.Orgeqpmanufacturerid == item);

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.VnfCnf?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.VnfCnf)
                    if (item == ConstantValueFilter.VNF)
                    {
                        resultPredicate.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Ismain == true && y.Majorhardware.Buildconstruction.Rule == 3));
                    }
                    else if (item == ConstantValueFilter.CNF)
                    {
                        resultPredicate.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Ismain == true &&  y.Majorhardware.Buildconstruction.Rule == 4));
                    }
                    else
                    {
                        resultPredicate.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Ismain == true &&  y.Majorhardware.Buildconstruction.Rule != 3 && y.Majorhardware.Buildconstruction.Rule != 4));
                    }
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.VendorNf?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.VendorNf)
                    resultPredicate.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description == item);

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.XnfInstance?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.XnfInstance)
                    resultPredicate.Or(x => x.Elementname == item);

                mainPredicate.And(resultPredicate);
            }          
            if (filterDto.Environment?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.Environment)
                    resultPredicate.Or(x => x.Environmentid == item);

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.Status?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.Status)
                    resultPredicate.Or(x => x.Deploymentstatus.Deploymentstatusid == item);

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.LaasCaasInfraStackInitialGoLive?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.LaasCaasInfraStackInitialGoLive)
                    if (item == ConstantValueFilter.VNF)
                    {
                        resultPredicate.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Ismain == true && y.Majorhardware.Buildconstruction.Rule == 3));
                    }
                    else if (item == ConstantValueFilter.CNF)
                    {
                        resultPredicate.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Ismain == true && y.Majorhardware.Buildconstruction.Rule == 4));
                    }
                    else
                    {
                        resultPredicate.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Ismain == true && y.Majorhardware.Buildconstruction.Rule != 3
                        && y.Majorhardware.Buildconstruction.Rule != 4));
                    }
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.StackNameInitialGoLive?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.StackNameInitialGoLive)
                    resultPredicate.Or(x => x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => y.Ismain == true && y.Majorhardware.Buildconstruction.Buildconstruction == item));
                mainPredicate.And(resultPredicate);
            }
            
            if (filterDto.BomSubmittedDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.BomSubmittedDate.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Bomsubmitteddate >= filterDto.BomSubmittedDate.StartDate));
                if (filterDto.BomSubmittedDate.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Bomsubmitteddate >= filterDto.BomSubmittedDate.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.HwPoRaisedDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.HwPoRaisedDate.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Hwporaiseddate >= filterDto.HwPoRaisedDate.StartDate));
                if (filterDto.HwPoRaisedDate.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Hwporaiseddate >= filterDto.HwPoRaisedDate.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.HwPoArrivedDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.HwPoArrivedDate.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Hwpoarriveddate >= filterDto.HwPoArrivedDate.StartDate));
                if (filterDto.HwPoArrivedDate.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Hwpoarriveddate >= filterDto.HwPoArrivedDate.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.RfaDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.RfaDate.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Rfadate >= filterDto.RfaDate.StartDate));
                if (filterDto.RfaDate.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Rfadate >= filterDto.RfaDate.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.StartRfo != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.StartRfo.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Rfodate >= filterDto.StartRfo.StartDate));
                if (filterDto.StartRfo.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Rfodate >= filterDto.StartRfo.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.Rfs != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.Rfs.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Rfsdate >= filterDto.Rfs.StartDate));
                if (filterDto.Rfs.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Rfsdate >= filterDto.Rfs.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.MigrationCompletionDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.MigrationCompletionDate.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Migrationcompletiondate >= filterDto.MigrationCompletionDate.StartDate));
                if (filterDto.MigrationCompletionDate.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Migrationcompletiondate >= filterDto.MigrationCompletionDate.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.HardwareTypeTarget?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.HardwareTypeTarget)
                    resultPredicate.Or(x => x.Assethardwareancillary.Any(y => y.Majorhardwarebuildasis.Majorhardwarebuildasisid.ToString() == item));

                mainPredicate.And(resultPredicate);
            }           
            if (filterDto.Power?.Any() == true)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                foreach (var item in filterDto.Power)
                    resultPredicate.Or(x => x.Daassetmigration.Any(y => y.Trafficnodepercentage == item));

                mainPredicate.And(resultPredicate);
            }
            if (filterDto.VecDate != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.VecDate.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Vecdate >= filterDto.VecDate.StartDate));
                if (filterDto.VecDate.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Vecdate >= filterDto.VecDate.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.StartOfAppIntegration != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.StartOfAppIntegration.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Startofappintegration >= filterDto.StartOfAppIntegration.StartDate));
                if (filterDto.StartOfAppIntegration.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Startofappintegration >= filterDto.StartOfAppIntegration.EndDate));
                mainPredicate.And(resultPredicate);
            }
            if (filterDto.MigrationStart != null)
            {
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                if (filterDto.MigrationStart.StartDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Migrationstart >= filterDto.MigrationStart.StartDate));
                if (filterDto.MigrationStart.EndDate != null)
                    resultPredicate.And(x => x.Daassetmigration.Any(y => y.Migrationstart >= filterDto.MigrationStart.EndDate));
                mainPredicate.And(resultPredicate);
            }
            return mainPredicate;
        }

        public async Task<QueryResultDto<ExodusAssetLevelReportDtoGrid>> GetExodusReportLevelReport(ExodusAssetLevelReportQueryDto filterDto, bool isExport = false)
        {

            var predicateResult = ApplyFilter(filterDto);
            var rtn = new QueryResultDto<ExodusAssetLevelReportDtoGrid>(new GenerateRenderForGrid<ExodusAssetLevelReportDtoGrid>(_customColumnManager))
            {

            };

            var query = await Task.Run(() => GetRecords(predicateResult).OrderByDescending(x => x.Modificationdate).ToList());
 
            var result = await GridMapping(query, isExport, filterDto);
 
            if (filterDto.LaasCaasInfraStackTarget?.Any() == true)
            {
                foreach(var item in filterDto.LaasCaasInfraStackTarget)
                {
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        result = result.Where(x => x.LaasCaasInfraStackTarget.IsNullOrEmpty()).ToList();
                    }
                    else
                    {
                        result = result.Where(x => filterDto.LaasCaasInfraStackTarget.Contains(x.LaasCaasInfraStackTarget)).ToList();
                    }
                }
            }
            if (filterDto.StackNameTarget?.Any() == true)
            {
                foreach (var item in filterDto.StackNameTarget)
                {
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        result = result.Where(x => x.StackNameTarget.IsNullOrEmpty()).ToList();
                    }
                    else
                    {
                        result = result.Where(x => filterDto.StackNameTarget.Contains(x.StackNameTarget)).ToList();
                    }
                }
            }
            if (filterDto.TargetXnfInstance?.Any() == true)
            {
                foreach (var item in filterDto.TargetXnfInstance)
                {
                    if (item == ConstantValueFilter.yes.ToLower())
                    {
                        result = result.Where(x => x.TargetXnfInstance.IsNullOrEmpty()).ToList();
                    }
                    else
                    {
                        result = result.Where(x => filterDto.TargetXnfInstance.Contains(x.TargetXnfInstance)).ToList();
                    }
                }
            }
            if (filterDto.HardwareTypeLive?.Any() == true)
            {              
                result = result.Where(x => filterDto.HardwareTypeLive.Contains(x.HardwareTypeLive)).ToList();
            }
            if(filterDto.Traffic?.Any() == true)
            {
                result = result.Where(x => filterDto.Traffic.Contains(x.Traffic)).ToList();
            }

            rtn.TotalItems = result.Count;

            if (!isExport)
            {
                result = result.Skip((filterDto.Page - 1) * filterDto.PageSize).Take(filterDto.PageSize).ToList();
            }


            rtn.Items = result;

            return rtn;
        }

        public async Task<List<ExodusAssetLevelReportDtoGrid>> GridMapping(List<Networkelementsasplanned> asset, bool isExport, ExodusAssetLevelReportQueryDto filterDto)
        {
            var liveMigrationDate = await GetLiveAssetMigrationDate(asset.Select(x => x.Elementname).ToList(), asset.Select(x => x.Designcomponentid).ToList(),
                                          asset.Select(x => x.Opcoid).ToList());

            var liveAssetNewlyFieldsDates = await GetLiveAssetNewlyFieldsDates(asset.Select(x => x.Elementname).ToList(),
                                          asset.Select(x => x.Opcoid).ToList());

            var removedDeploymentStatusIs = _repositoryWrapper.DeploymentStatus.FindByCondition(x => x.Deploymentstatus.ToLower() == ConstantValueFilter.Removed.ToLower()).FirstOrDefault()?.Deploymentstatusid;


            var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
            var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();

            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
            var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();

            var result = await Task.Run(() => asset.Select(x =>
            {
                var hardwareBuilds = x.Designcomponent.Systemtype.Systemtypesmajorhardwarebuilds;

                bool assetHwOem = hardwareBuilds.Any(r =>
                    r.Ismain &&
                    hwOem.Contains(r.Majorhardware.Orgeqpmanufacturerid));

                if (assetHwOem)
                {
                    bool assetHwPlatform = hardwareBuilds.Any(r =>
                        r.Ismain &&
                        hwPlatform.Contains(r.Majorhardware.Platformid));

                    if (!assetHwPlatform)
                        return null;
                }
                else
                    return null;

                var grid = new ExodusAssetLevelReportDtoGrid();

                #region vertical from asset subdomainspoc
                if (x.Networkelementasplannedsubdomainspoc?.Any() == true)
                {
                grid.VerticalFilterDto = _commonManager.GetVerticaleFilterDto(x.Networkelementasplannedsubdomainspoc?.Select(x => x?.Subdomainspocid).ToList(), 0, false, true)
                                        ?.Distinct()?.ToDictionary(m => Convert.ToInt16(m.Value), m => m.Text);
                grid.VerticalName = grid.VerticalFilterDto != null && grid.VerticalFilterDto.Count() > 0 ?
                                    string.Join(",", grid.VerticalFilterDto.Select(m => m.Value).ToList() ??
                                    new List<string>()) : string.Empty;
                }
                #endregion

                #region // Current asset Related details
                var assetDc= x?.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(x => x.Majorhardware).FirstOrDefault();
                var buildConstrainRule = assetDc?.Buildconstruction?.Rule ?? 0;
                var buildConstrainDetails = assetDc?.Buildconstruction?.Buildconstruction ?? string.Empty;
                var assetDcPlatform = assetDc?.Platform?.Platform ?? string.Empty;

                var productEntity = x?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Productname;
                var productDetails = productEntity?.Description ?? string.Empty;

                //var hardwareTypeEntity = x?.Designcomponent?.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(y => y.Majorhardware).FirstOrDefault();
                //var hardwareTypeDetails = hardwareTypeEntity?.Hardwaretype ?? string.Empty;               


                var currentHardwareTypeEntity = x?.Assethardwareancillary.Where(y => y.Majorhardwarebuildasisid != null).FirstOrDefault();
                var currentHardwareTypeDetails = currentHardwareTypeEntity?.Majorhardwarebuildasis?.Platform?.Platform + " " + currentHardwareTypeEntity?.Majorhardwarebuildasis?.Hardwaretype ?? string.Empty;
                string currentHardwareTypeId = Convert.ToString(currentHardwareTypeEntity?.Majorhardwarebuildasis?.Majorhardwarebuildasisid) ?? "0";

                #endregion
                
                var daAssetDetails = x.Daassetmigration.Where(f => f.Newelementname != null && f.Targetdesigncomponenetid != null).OrderByDescending(o => o.Migrationcompletiondate).FirstOrDefault();

                if (daAssetDetails == null)
                {
                    daAssetDetails = x.Daassetmigration.Where(x => x.Newelementname != null).OrderByDescending(o => o.Migrationcompletiondate).FirstOrDefault(); 
                }
                if (daAssetDetails == null)
                {
                    daAssetDetails = x.Daassetmigration.OrderByDescending(o => o.Migrationcompletiondate).FirstOrDefault();
                }

                var mainPredicate = PredicateBuilder.New<Networkelementsasplanned>();
                var resultPredicate = PredicateBuilder.New<Networkelementsasplanned>();

                if (daAssetDetails != null)
                {
                     resultPredicate.Or(f => f.Elementname == daAssetDetails.Newelementname && f.Designcomponentid == daAssetDetails.Targetdesigncomponenetid && f.Opcoid == daAssetDetails.Opcoid);
               
                    mainPredicate.And(resultPredicate);
                }
                var targetListAssetDetail = GetTargetRecords(mainPredicate).ToList();
                var targetAssetDetails = targetListAssetDetail?.FirstOrDefault();
               
                if (targetAssetDetails != null)
                {
                    daAssetDetails = x.Daassetmigration
                        .FirstOrDefault(y => y.Newelementname == targetAssetDetails.Elementname);
                }
                else
                {
                    daAssetDetails = x.Daassetmigration
                        .Where(y => y.Newelementname != null)
                        .OrderByDescending(o => o.Migrationcompletiondate)
                        .FirstOrDefault();

                    if (daAssetDetails == null)
                    {
                        daAssetDetails = x.Daassetmigration
                            .OrderByDescending(o => o.Migrationcompletiondate)
                            .FirstOrDefault();
                    }
                }
                #region pa details
                var mainPaPredicate = PredicateBuilder.New<Plannedactivities>();
                var resultPaPredicate = PredicateBuilder.New<Plannedactivities>();

                if (daAssetDetails != null)
                {
                    resultPaPredicate.Or(f => f.Plannedactivityid == daAssetDetails.Plannedactivityid);
                    mainPaPredicate.And(resultPaPredicate);
                }

                var paEntityDetails = GetTargetPaRecords(mainPaPredicate).ToList();
                var targetbuildConstrainEntity = paEntityDetails?.Select(x => x.Designcomponentfamily?.Designcomponents?.Select(r => r.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(x => x.Majorhardware?.Buildconstruction).FirstOrDefault()).FirstOrDefault()).FirstOrDefault();
                var targetbuildConstrainDetails = targetbuildConstrainEntity?.Buildconstruction ?? string.Empty;

                var targetPlatfromEntity = paEntityDetails?.Select(x => x.Designcomponentfamily?.Designcomponents?.Select(r => r.Systemtype?.Systemtypesmajorhardwarebuilds?.Select(x => x.Majorhardware?.Platform).FirstOrDefault()).FirstOrDefault()).FirstOrDefault();
                var targetPlatformDetails = targetPlatfromEntity?.Platform ?? string.Empty;

                //var currentHardwareTypeEntity = targetAssetDetails?.Assethardwareancillary.Where(y => y.Majorhardwarebuildasisid != null).FirstOrDefault();
                //var currentHardwareTypeDetails = currentHardwareTypeEntity?.Majorhardwarebuildasis?.Platform?.Platform + " " + currentHardwareTypeEntity?.Majorhardwarebuildasis?.Hardwaretype ?? string.Empty;
                //string currentHardwareTypeId = Convert.ToString(currentHardwareTypeEntity?.Majorhardwarebuildasis?.Majorhardwarebuildasisid) ?? "0";

                #endregion




                //var targetDepoymentStatusEntity = targetAssetDetails?.Deploymentstatus;
                //var targetDepoymentStatus = targetDepoymentStatusEntity?.Deploymentstatus ?? string.Empty;

                
                if (!string.IsNullOrEmpty(assetDcPlatform) && ConstantValueFilter.exodusPlatformStackName.TryGetValue(assetDcPlatform, out var assetPlatformDescription))
                    grid.LaasCaasInfraStackInitialGoLive = assetPlatformDescription;
                else
                    grid.LaasCaasInfraStackInitialGoLive = assetDcPlatform;

                grid.OpCo = x.Opco?.Opco;
                grid.DcfName = x.Designcomponentfamily != null ? x.Designcomponentfamily.DCFName(_repositoryWrapper) : x.Designcomponent?.Designcomponentfamily.DCFName(_repositoryWrapper);
                grid.DCFId = x.Designcomponentfamily != null ? x.Designcomponentfamily.Designcomponentfamilyid : x.Designcomponent?.Designcomponentfamily.Designcomponentfamilyid;
                grid.OpCoId = x.Opco.Opcoid;
                grid.Site = x.Location?.Shortdescription;
                grid.SiteName = x.Location?.Location;
                grid.LocationId = x.Location.Locationid;
                grid.Vendor = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Originalequipmentmanufacturer;
                grid.OemId = x.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Orgeqpmanufacturer?.Orgeqpmanufacturerid;
                grid.VnfCnf = buildConstrainRule == (int)ExodusBuildContractionRuleEnum.AsNfvi ? ConstantValueFilter.VNF : buildConstrainRule == (int)ExodusBuildContractionRuleEnum.AsNfci ? ConstantValueFilter.CNF : ConstantValueFilter.Other;
                grid.LivePlatformId = grid.VnfCnf;
                grid.VendorNf = productDetails;
                grid.ProductNameId = productEntity?.Productnameid;
                grid.XnfInstance = x.Elementname;
                grid.UsageOptional = string.Empty;
                grid.XnfSizeCore = string.Empty;
                grid.Environment = x.Environment?.Environment;
                grid.EnvironmentId = x.Environment?.Environmentid;                
                grid.StackNameInitialGoLive = buildConstrainDetails;
                grid.RfoReqdBy = null;
                grid.RfsReqdBy = null;
                grid.PoRaised = string.Empty;
                grid.PoReqdByIfNotNa = string.Empty;
                grid.ClusterName = string.Empty;
                grid.HardwareTypeLive = currentHardwareTypeDetails;
                grid.XnfSizeVcpu = string.Empty;
                grid.TargetXnfInstance = daAssetDetails != null ? daAssetDetails.Newelementname : string.Empty;
             
                grid.TargetPlatformId = targetbuildConstrainDetails;
                grid.StackNameTarget = targetbuildConstrainDetails;
                grid.PoReqdBy = string.Empty;  
                grid.BroadcomRelease = string.Empty;
                grid.HardwareTypeTarget = string.Empty;
                //grid.HardwareTypeTargetId = targetHardwareTypeId;
                grid.NfSizeExpansionDcekpiValue = string.Empty;
                grid.NfSizeDcekpiSauGbps = string.Empty;
                grid.AciAvailable = string.Empty;
                grid.Traffic = daAssetDetails != null ? daAssetDetails.Trafficnodepercentage : null;
                grid.Power = string.Empty;

                #region June 19 2026
                grid.Status = x.Deploymentstatus?.Deploymentstatus;
                grid.StausId = x.Deploymentstatus?.Deploymentstatusid;
                var isAssetInRemovedStatus = x?.Daassetmigration?.Any(m => m.Isdecommissioned !=null && m.Isdecommissioned==true) ?? false;
                if(isAssetInRemovedStatus)
                {
                    grid.BomSubmittedDate = null;
                    grid.HwPoRaisedDate = null;
                    grid.HwPoArrivedDate = null;
                    grid.RfaDate = null;
                    grid.StartRfo = null;
                    grid.Rfs = null;
                    grid.VecDate = null;
                    grid.StartOfAppIntegration = null;
                    grid.MigrationStart = null;
                    
                    grid.LaasCaasInfraStackTarget = ConstantValueFilter.RemoveStatusForDecommissionedNode;
                }
                else
                {
                    grid.BomSubmittedDate = daAssetDetails != null ? daAssetDetails?.Bomsubmitteddate:null;
                    grid.HwPoRaisedDate = daAssetDetails != null ? daAssetDetails?.Hwporaiseddate : null;
                    grid.HwPoArrivedDate = daAssetDetails != null ? daAssetDetails?.Hwpoarriveddate : null;
                    grid.RfaDate = daAssetDetails != null ? daAssetDetails?.Rfadate : null;
                    grid.StartRfo = daAssetDetails != null ? daAssetDetails?.Rfodate : null;
                    grid.Rfs = daAssetDetails != null ? daAssetDetails?.Rfsdate : null;
                    grid.VecDate = daAssetDetails != null ? daAssetDetails?.Vecdate : null;
                    grid.StartOfAppIntegration = daAssetDetails != null ? daAssetDetails?.Startofappintegration : null;
                    grid.MigrationStart = daAssetDetails != null ? daAssetDetails?.Migrationstart : null;
                    grid.LaasCaasInfraStackTarget = targetPlatformDetails;
                }
                grid.MigrationCompletionDate = daAssetDetails != null ? daAssetDetails.Migrationcompletiondate : liveMigrationDate.Where(f => f.Newelementname == x.Elementname
          && f.Targetdesigncomponenetid == x.Designcomponentid && f.Opcoid == x.Opcoid).Select(r => r.Migrationcompletiondate).FirstOrDefault();
                #endregion
                #region Fy Calculation

                if (isExport)
                {
                    grid.FyQ1_24_25 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2024, 06, 30), grid.StartRfo);
                    grid.FyQ2_24_25 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2024, 09, 30), grid.StartRfo);
                    grid.FyQ3_24_25 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2024, 12, 31), grid.StartRfo);
                    grid.FyQ4_24_25 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2025, 03, 31), grid.StartRfo);
                    grid.FyQ1_25_26 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2025, 06, 30), grid.StartRfo);
                    grid.FyQ2_25_26 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2025, 09, 30), grid.StartRfo);
                    grid.FyQ3_25_26 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2025, 12, 31), grid.StartRfo);
                    grid.FyQ4_25_26 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2026, 03, 31), grid.StartRfo);
                    grid.FyQ1_26_27 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2026, 06, 30), grid.StartRfo);
                    grid.FyQ2_26_27 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2026, 09, 30), grid.StartRfo);
                    grid.FyQ3_26_27 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2026, 12, 31), grid.StartRfo);
                    grid.FyQ4_26_27 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2027, 03, 31), grid.StartRfo);
                    grid.FyQ1_27_28 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2027, 06, 30), grid.StartRfo);
                    grid.FyQ1_27_28 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2027, 09, 30), grid.StartRfo);
                    grid.FyQ1_27_28 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2027, 12, 31), grid.StartRfo);
                    grid.FyQ1_27_28 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2028, 03, 31), grid.StartRfo);
                    grid.FyQ1_28_29 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2028, 06, 30), grid.StartRfo);
                    grid.FyQ2_28_29 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2028, 09, 30), grid.StartRfo);
                    grid.FyQ3_28_29 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2028, 12, 31), grid.StartRfo);
                    grid.FyQ4_28_29 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2029, 03, 31), grid.StartRfo);
                    grid.FyQ1_29_30 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2029, 06, 30), grid.StartRfo);
                    grid.FyQ2_29_30 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2029, 09, 30), grid.StartRfo);
                    grid.FyQ3_29_30 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2029, 12, 31), grid.StartRfo);
                    grid.FyQ4_29_30 = CalculatingFy(grid.Status, grid.RfsReqdBy, grid.LaasCaasInfraStackInitialGoLive, grid.LaasCaasInfraStackTarget, grid.MigrationCompletionDate, new DateTime(2030, 03, 31), grid.StartRfo);
                }
                #endregion

                return grid;
            }).Where(x => x != null)   // Remove null results
               .ToList());



            return result;

        }

        public async Task<List<Daassetmigration>> GetLiveAssetMigrationDate(List<string> liveassetName, List<long> liveAssetDc, List<short> opCoId)
        {
            var query = await _repositoryWrapper.DaAssetMigrationRepository.FindByCondition(x => liveassetName.Contains(x.Newelementname) && liveAssetDc.Contains((long)x.Targetdesigncomponenetid)
            && opCoId.Contains((short)x.Opcoid) && x.Migrationcompletiondate != null)
                .OrderByDescending(x => x.Migrationcompletiondate).ToListAsync();
          

            var result = query.Select(x => new Daassetmigration
            {  
                Opcoid = x.Opcoid, 
                Newelementname = x.Newelementname, 
                Targetdesigncomponenetid = x.Targetdesigncomponenetid, 
                Migrationcompletiondate = x.Migrationcompletiondate ,
                Deploymentstatusid = x.Deploymentstatusid
            }).ToList();


            return result;

        }
        public async Task<List<Daassetmigration>> GetLiveAssetNewlyFieldsDates(List<string> liveassetName,List<short> opCoId)
        {
            var query = await _repositoryWrapper.DaAssetMigrationRepository.FindByCondition(x => liveassetName.Contains(x.Newelementname) 
            && opCoId.Contains((short)x.Opcoid))
                .OrderByDescending(x => x.Modificationdate).ToListAsync();


            var result = query.Select(x => new Daassetmigration
            {
                Opcoid = x.Opcoid,
                Newelementname = x.Newelementname,
                Bomsubmitteddate = x.Bomsubmitteddate,
                Hwpoarriveddate = x.Hwpoarriveddate,
                Hwporaiseddate = x.Hwporaiseddate,
                Rfadate = x.Rfadate,
                Deploymentstatusid = x.Deploymentstatusid
            }).ToList();


            return result;

        }

        public async Task<List<FilterValueDto>> GetFilteredValues(string propertyName, string propertyFilter, ExodusAssetLevelReportQueryDto filterDto,bool isAdmin)
        {
            var filterCriteria = ApplyFilter(filterDto);

            var QueryResult = await GetExodusReportLevelReport(filterDto, true);

            var filteredQuery = QueryResult.Items;

            var result = propertyName switch
            {
                #region 

                "opCo" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.OpCo.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.OpCo, Value = x.OpCoId.ToString() })
                                    .Distinct()
                                    .ToList(),
                "dcfName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.DCFId.ToString().Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.DcfName, Value = x.DCFId.ToString() })
                                    .Distinct()
                                    .ToList(),
                //"site" => filteredQuery
                //                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Site.Contains(propertyFilter))
                //                    .Select(x => new FilterValueDto { Text = x.Site})
                //                    .Distinct()
                //                    .ToList(),
                "siteName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.SiteName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.SiteName, Value = x.LocationId.ToString() })
                                    .Distinct()
                                    .ToList(),
                "vendor" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Vendor.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.Vendor, Value = x.OemId.ToString() })
                                    .Distinct()
                                    .ToList(),
                "vnfCnf" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VnfCnf.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.VnfCnf, Value = x.VnfCnf.ToString() })
                                    .Distinct()
                                    .ToList(),
                "vendorNf" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.VendorNf.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.VendorNf))
                                    .Distinct()
                                    .ToList(),
                "xnfInstance" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.XnfInstance.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.XnfInstance))
                                    .Distinct()
                                    .ToList(),
                "usageOptional" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.UsageOptional.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.UsageOptional))
                                    .Distinct()
                                    .ToList(),
                "xnfSizeCore" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.XnfSizeCore.ToString().Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.XnfSizeCore))
                                    .Distinct()
                                    .ToList(),
                "environment" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Environment.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.Environment, Value = x.EnvironmentId.ToString() })
                                    .Distinct()
                                    .ToList(),
                "status" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Status.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.Status, Value = x.StausId.ToString() })
                                    .Distinct()
                                    .ToList(),
                "laasCaasInfraStackInitialGoLive" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.LaasCaasInfraStackInitialGoLive.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto { Text = x.LaasCaasInfraStackInitialGoLive, Value = x.LivePlatformId })
                                    .Distinct()
                                    .ToList(),
                "stackNameInitialGoLive" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.StackNameInitialGoLive.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.StackNameInitialGoLive))
                                    .Distinct()
                                    .ToList(),
                "poRaised" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PoRaised.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PoRaised))
                                    .Distinct()
                                    .ToList(),
                "poReqdByIfNotNa4" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PoReqdByIfNotNa.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PoReqdByIfNotNa))
                                    .Distinct()
                                    .ToList(),
                "clusterName" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.ClusterName.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.ClusterName))
                                    .Distinct()
                                    .ToList(),
                "targetXnfInstance" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.TargetXnfInstance.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.TargetXnfInstance))
                                    .Distinct()
                                    .ToList().Concat(filteredQuery.Where(x => x.TargetXnfInstance == "" || x.TargetXnfInstance == null)
                                       .Select(x =>

                                          new FilterValueDto
                                          {
                                              Text = ConstantValueFilter.blankTextValue,
                                              Value = ConstantValueFilter.yes.ToLower(),
                                          }
                                       )).Distinct().ToList(),
                "hardwareTypeLive" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HardwareTypeLive.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.HardwareTypeLive))
                                    .Distinct()
                                    .ToList(),
                "xnfSizeVcpu" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.XnfSizeVcpu.ToString().Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.XnfSizeVcpu))
                                    .Distinct()
                                    .ToList(),
                "laasCaasInfraStackTarget" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.LaasCaasInfraStackTarget.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.LaasCaasInfraStackTarget))
                                    .Distinct()
                                    .ToList()
                                    .ToList().Concat(filteredQuery.Where(x => x.TargetXnfInstance == "" || x.TargetXnfInstance == null)
                                       .Select(x =>

                                          new FilterValueDto
                                          {
                                              Text = ConstantValueFilter.blankTextValue,
                                              Value = ConstantValueFilter.yes.ToLower(),
                                          }
                                       )).Distinct().ToList(),
                "stackNameTarget" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.StackNameTarget.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.StackNameTarget))
                                    .Distinct()
                                    .ToList()
                                    .ToList().Concat(filteredQuery.Where(x => x.TargetXnfInstance == "" || x.TargetXnfInstance == null)
                                       .Select(x =>

                                          new FilterValueDto
                                          {
                                              Text = ConstantValueFilter.blankTextValue,
                                              Value = ConstantValueFilter.yes.ToLower(),
                                          }
                                       )).Distinct().ToList(),
                "poReqdBy" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.PoReqdBy.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.PoReqdBy))
                                    .Distinct()
                                    .ToList(),
                "broadcomRelease" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.BroadcomRelease.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.BroadcomRelease))
                                    .Distinct()
                                    .ToList(),
                //"hardwareTypeTarget" => filteredQuery
                //                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.HardwareTypeTarget.Contains(propertyFilter))
                //                    .Select(x => new FilterValueDto{Value = x.HardwareTypeTargetId , Text = x.HardwareTypeTarget})
                //                    .Distinct()
                //                    .ToList(),
                "nfSizeExpansionDcekpiValue" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NfSizeExpansionDcekpiValue.ToString().Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.NfSizeExpansionDcekpiValue))
                                    .Distinct()
                                    .ToList(),
                "nfSizeDcekpiSauGbps" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.NfSizeDcekpiSauGbps.ToString().Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.NfSizeDcekpiSauGbps))
                                    .Distinct()
                                    .ToList(),
                "aciAvailable" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.AciAvailable.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.AciAvailable))
                                    .Distinct()
                                    .ToList(),
                "power" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Power.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Power))
                                    .Distinct()
                                    .ToList(),
                "traffic" => filteredQuery
                                    .Where(x => string.IsNullOrEmpty(propertyFilter) || x.Traffic.Contains(propertyFilter))
                                    .Select(x => new FilterValueDto(x.Traffic))
                                    .Distinct()
                                    .ToList(),

                "verticalName" => string.IsNullOrEmpty(propertyFilter)
                                 ? filteredQuery.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                 .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                 {
                                     Text = t.Value,
                                     Value = t.Key.ToString()
                                 }))?.Distinct()?.ToList()
                                 :
                                  filteredQuery.Where(x => x.VerticalFilterDto != null && x.VerticalFilterDto.Count() > 0)
                                 .SelectMany(p => p.VerticalFilterDto.Select(t => new FilterValueDto
                                 {
                                     Text = t.Value,
                                     Value = t.Key.ToString()
                                 })).Where(x => x.Text.Contains(propertyFilter))?.Distinct()?.ToList(),
                #endregion

                _ => new List<FilterValueDto>()
            };
            if (!isAdmin && (filterDto.VerticalName != null && filterDto.VerticalName.Count > 0) && propertyName == "verticalName")
            {
                result = result.Where(x => filterDto.VerticalName.Contains(x.Value.ToString())).ToList();
            }

            return result;
        }

        public string CalculatingFy(string status, DateTime? assetLiveRfsDate, string infraStackLive, string infraStrackTarget, DateTime? migrationDate, DateTime? finacialYear, DateTime? startRfo)
        {
            var result = string.Empty;
            var colour = string.Empty;

            assetLiveRfsDate = FixFormat(assetLiveRfsDate.ToString());
            finacialYear = FixFormat(finacialYear.ToString());
            migrationDate = FixFormat(migrationDate.ToString());
            startRfo = FixFormat(startRfo.ToString());


            if (migrationDate == null)
            {
                colour = (string.IsNullOrEmpty(startRfo.ToString()) && finacialYear >= startRfo) ? "yellow" : (!string.IsNullOrEmpty(startRfo.ToString()) && finacialYear < startRfo) ? "LightSteelBlue" : string.Empty;
                result = status.ToLower().Trim() == ConstantValueFilter.InService || assetLiveRfsDate <= finacialYear ?
                $"{infraStackLive} | {colour}" : $"{infraStackLive} | {colour}";
            }
            else
            {
                colour = (!string.IsNullOrEmpty(migrationDate.ToString()) && finacialYear >= migrationDate) ? "DeepPink" : (!string.IsNullOrEmpty(startRfo.ToString()) && finacialYear >= startRfo) ? "yellow" : (!string.IsNullOrEmpty(startRfo.ToString()) && finacialYear < startRfo) ? "LightSteelBlue" : string.Empty;
                result = migrationDate <= finacialYear ? $"{infraStrackTarget} | {colour}" :
                status.ToLower().Trim() == ConstantValueFilter.InService || assetLiveRfsDate <= finacialYear ? $"{infraStackLive} | {colour}" :
                $"{infraStackLive} | {colour}";

            }
            return result;
        }

        public DateTime? FixFormat(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            string[] formats = {
                "M/d/yyyy h:mm:ss tt",
                "MM/dd/yyyy hh:mm:ss tt",
                "M/d/yyyy h:mm tt",
                "MM/dd/yyyy hh:mm tt",

                "dd/MM/yyyy",
                "dd/MM/yyyy HH:mm",
                "dd/MM/yyyy HH:mm:ss tt",
                "dd-MM-yyyy",
                "dd-MM-yyyy HH:mm",
                "dd-MM-yyyy HH:mm:ss",
                "yyyy-MM-dd",
                "yyyy-MM-dd HH:mm",
                "yyyy-MM-dd HH:mm:ss"
            };

            if (DateTime.TryParseExact(
                    dateString,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsed))
            {
                return parsed.Date;
            }

            return null;
        }


    }
}