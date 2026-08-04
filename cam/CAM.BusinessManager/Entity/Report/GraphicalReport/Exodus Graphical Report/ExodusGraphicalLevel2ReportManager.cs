using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.Grid;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.QueryDto.GraphicalReports;
using CAM.Entities.Models;
using CAM.Enum;
using CAM.Infrastucture;
using DocumentFormat.OpenXml.Office.CustomUI;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CAM.BusinessManager.Entity.Report.GraphicalReport.Exodus_Graphical_Report
{
    public class ExodusGraphicalLevel2ReportManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly CommonManager _commonManager;
        private readonly ILoggerManager _logger;
        private Dictionary<int, string> filteredPlannedActivity = new Dictionary<int, string>();
        private bool isTargetDc = false;
        private readonly DateTime complainceDate = new DateTime(2030, 03, 31);

        private readonly DapperCommonManager _dapperCommonManager;
        public ExodusGraphicalLevel2ReportManager(IEnumerable<IRepositoryWrapper> wrappers,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, GridCustomColumnManager manager,
            DropdownDataServiceManager dropdownDataServiceManager, ILoggerManager logger, DapperCommonManager dapperCommonManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _commonManager = commonManager;
            _logger = logger;
            _dapperCommonManager = dapperCommonManager;
        }
        #region PALevelPercentageForDa
        private IQueryable<Designaspects> GetQuery(ExpressionStarter<Designaspects> predicateResult, bool includeDeleted)
        {
            var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
            var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();

            var excludedOpcoList = _dapperCommonManager.GetRestrictedOpcoAsync().Result;
            var excludedOpcoid = excludedOpcoList?.Select(x => (short?)x.Opcoid).ToList();

            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
            var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();

            var result = isTargetDc ? _repositoryWrapper.DesignAspectRepository.FindByCondition(predicateResult, includeDeleted).Where(x => x.Archived == false)
                        .Include(x => x.Opco)
                        .Include(x => x.Plannedactivities).ThenInclude(p => p.Plannedactivityresource)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Plannedactivities).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        : _repositoryWrapper.DesignAspectRepository.FindByCondition(predicateResult, includeDeleted).Where(x => x.Archived == false
                         && !excludedOpcoid.Contains(x.Opcoid)
                         && x.Designcomponentfamily.Designcomponents.Any(f => f.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwOem.Contains(y.Majorhardware.Orgeqpmanufacturerid)))
                         && x.Designcomponentfamily.Designcomponents.Any(f => f.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwPlatform.Contains(y.Majorhardware.Platformid)))
                         )
                        .Include(x => x.Opco)
                        .Include(x => x.Plannedactivities).ThenInclude(p => p.Plannedactivityresource)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Productname)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Majorswbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Majorhwbuildsdesigncontacts)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                        .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                        .Include(x => x.Plannedactivities).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform);


            return result.AsQueryable();
        }
        private ExpressionStarter<Designaspects> ApplyFilter(ExodusGraphicalReportQueryDto buildFilterDto)
        {
            var predicateResult = PredicateBuilder.New<Designaspects>(true);
            var predicateInner = PredicateBuilder.New<Designaspects>();

            predicateInner.Or(x => x.Archived == false);
            predicateResult.And(predicateInner);


            if (buildFilterDto.OpcoId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.ProductId)
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Productnameid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.VendorId)
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.PlatformId)
                {
                    predicateInner.Or(x => x.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Systemtypesmajorhardwarebuilds.Any(mjh => mjh.Majorhardware.Platformid == item)));
                }
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.TargetPlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Designaspects>();
                foreach (var item in buildFilterDto.TargetPlatformId)
                {
                    predicateInner.Or(x =>
                        x.Plannedactivities
                            .SelectMany(pa => pa.Designcomponentfamily.Designcomponents)
                            .Any(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds
                                .Any(mhb => mhb.Majorhardware.Platform.Platformid == item))
                    );
                }
                predicateResult.And(predicateInner);
                isTargetDc = true;
            }

            return predicateResult;

        }
        public string CalculateCompliance(List<Plannedactivities> plannedactivities, bool targetDc)
        {

            if (plannedactivities == null || !plannedactivities.Any())
                return ConstantValueFilter.Red;

            plannedactivities = plannedactivities.Where(x => x.Archived == false && x.Deleted == false).ToList();

            /*This block is used for calculating TargetDc */
            if (targetDc)
            {
                bool isTargetDc = plannedactivities.Any(pa =>
                    pa.Designcomponentfamily.Designcomponents.Any(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds.Any(mj =>
                    ConstantValueFilter.InfraBuildConstructions.Contains(mj.Majorhardware.Platform.Platform))));

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }///

            //bool hasNoPlannedActivityForDA = plannedactivities.Any(x =>
            //    x.Plannedactivityresource != null &&
            //    x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA
            //);

            //if (hasNoPlannedActivityForDA)
            //    return ConstantValueFilter.Amber;

            bool hasPlatformMigration = plannedactivities.Any(x =>
                x.Plannedactivityresource != null &&
                x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration
            );

            if (hasPlatformMigration)
            {
                if (plannedactivities.Any(x => x.Plannedcompletion <= complainceDate))
                {
                    return ConstantValueFilter.Green;
                }
                else if (plannedactivities.Any(x => x.Plannedcompletion > complainceDate))
                {
                    return ConstantValueFilter.Amber;
                }
            }
            return ConstantValueFilter.Red;
        }
        public string CalculateCompliance(Plannedactivities plannedactivity, bool targetDc)
        {
            if (plannedactivity == null)
                return ConstantValueFilter.Red;

            if (targetDc)
            {
                bool isTargetDc = plannedactivity.Designcomponentfamily.Designcomponents.Any(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds.Any(mj =>
                    ConstantValueFilter.InfraBuildConstructions.Contains(mj.Majorhardware.Platform.Platform)));

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }

            //bool hasNoPlannedActivityForDA = plannedactivity.Plannedactivityresource != null &&
            //    plannedactivity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA;

            //if (hasNoPlannedActivityForDA)
            //    return ConstantValueFilter.Amber;

            bool hasPlatformMigration = plannedactivity.Plannedactivityresource != null &&
                plannedactivity.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration;

            if (hasPlatformMigration)
            {
                if (plannedactivity.Plannedcompletion <= complainceDate)
                {
                    return ConstantValueFilter.Green;
                }
                else if (plannedactivity.Plannedcompletion > complainceDate)
                {
                    return ConstantValueFilter.Amber;
                }
            }
            return ConstantValueFilter.Red;
        }
        public int GetNodeCountBasedOpcoAndDcfCombination(short? opcoId, long dcfId)
        {
            try
            {
                var dcIds = _repositoryWrapper.DesignComponent
                    .FindByCondition(x => x.Designcomponentfamilyid == dcfId)
                    .Select(x => x.Designcomponentid)
                    .ToList();

                var activeLcm = _repositoryWrapper.Lcmengineering
                    .FindByCondition(x =>
                        x.Archived != true &&
                        x.Opcoid == opcoId &&
                        dcIds.Contains(x.Designcomponentid))
                    .Include(x => x.Networkelementsasplanned)
                        .ThenInclude(x => x.Deploymentstatus);

                var elementCount = activeLcm
                    .SelectMany(x => x.Networkelementsasplanned)
                    .Count(x =>
                        x != null &&
                        x.Deploymentstatus != null &&
                        ConstantValueFilter.assetDeployementStatusForPlatformMigration
                            .Contains(x.Deploymentstatus.Deploymentstatus.ToLower()));

                return elementCount;
            }
            catch (Exception ex)
            {
                _logger.LogError("The exception occured in the method ---- GetNodeCountBasedOpcoAndDcfCombination with following messagae : " + ex.Message + ex.StackTrace);
                throw;
            }
        }
        private static ExpressionStarter<Settingsupdateplannedactivity> ApplySettingsupdateplannedactivityFilter(ExodusGraphicalReportQueryDto buildFilterDto)
        {
            var settingPAPredicateResult = PredicateBuilder.New<Settingsupdateplannedactivity>(true);
            var settingPAPredicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>(true);

            if (buildFilterDto.PlannedActivityResourceRuleId?.Any() == true)
            {

                settingPAPredicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in buildFilterDto.PlannedActivityResourceRuleId)
                    settingPAPredicateInner.Or(x => x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration ||
                    x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA);
                settingPAPredicateResult.And(settingPAPredicateInner);
            }


            return settingPAPredicateResult;
        }

        /// <summary>
        /// GetPADropdown will return the deliver statuses of the plannedactvities related to the DA
        /// </summary>
        /// <param name="buildFilterDto"></param>
        /// <returns>Deliverystatus in Resultdto form</returns>
        public async Task<ResultDto> GetPADropdown(ExodusGraphicalReportQueryDto buildFilterDto)
        {

            var settingPAPredicateResult = ApplySettingsupdateplannedactivityFilter(buildFilterDto);

            var deliveryStatus = await Task.Run(() => _dropdownDataServiceManager.GetSettingPlannedActivityBasedDeliveryStatusForPA(settingPAPredicateResult)?.Result?.
                GroupBy(x => x.Id)?.ToList());

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    deliveryStatus

                }
            };
        }
        private ExpressionStarter<Plannedactivities> ApplyPlannedActivityFilter(ExodusGraphicalReportQueryDto buildFilterDto, Dictionary<int, string> constantFilterPlannedActivities)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivities>(true);
            var predicateInner = PredicateBuilder.New<Plannedactivities>(true);

            predicateInner.Or(x => x.Archived == false);
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Plannedactivities>();
            predicateInner.Or(x => x.Designaspect.Archived == false);
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Plannedactivities>();
            predicateInner.Or(x => x.Designaspectid != null);
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Plannedactivities>();
            predicateInner.Or(x => x.Plannedactivityresource.RulelinkeddcNavigation.Plannedactivitytypedescription.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete);
            predicateResult.And(predicateInner);

            if (buildFilterDto.OpcoId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Designaspect.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.PlannedActivityResourceRuleId?.Any() == true)
            {

                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlannedActivityResourceRuleId)
                    predicateInner.Or(x => x.Plannedactivityresource.Rulelinkeddc == item);
                predicateResult.And(predicateInner);
            }

            if (constantFilterPlannedActivities.Count() > 0)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in constantFilterPlannedActivities)
                    predicateInner.Or(x => x.Plannedactivityresource.Rulelinkeddc == item.Key);
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.ProductId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ProductId)
                    predicateInner.Or(x => x.Designaspect.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Productnameid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.VendorId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.VendorId)
                    predicateInner.Or(x => x.Designaspect.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Majorsoftwarebuilds.Orgeqpmanufacturerid == item));
                predicateResult.And(predicateInner);
            }
            if (buildFilterDto.PlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.PlatformId)
                    predicateInner.Or(x => x.Designaspect.Designcomponentfamily.Designcomponents.Any(x => x.Systemtype.Systemtypesmajorhardwarebuilds
                    .Where(x => x.Ismain == true).Any(x => x.Majorhardware.Platformid == item)));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.TargetPlatformId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.TargetPlatformId)
                {
                    predicateInner.Or(x =>
                        x.Designcomponentfamily.Designcomponents
                            .Any(dc => dc.Systemtype.Systemtypesmajorhardwarebuilds
                                .Any(mhb => mhb.Majorhardware.Platform.Platformid == item))
                    );
                }
                predicateResult.And(predicateInner);
                isTargetDc = true;
            }

            return predicateResult;
        }
        public async Task<List<Plannedactivities>> GetDesignAspectPlannedActivity(ExpressionStarter<Plannedactivities> predicateResult)
        {
            var excludeOemList = _dapperCommonManager.GetHardwareOemForExodusFilterAsync().Result;
            var hwOem = excludeOemList?.Select(x => x.Orgeqpmanufacturerid).ToList();

            var excludedOpcoList = _dapperCommonManager.GetRestrictedOpcoAsync().Result;
            var excludedOpcoid = excludedOpcoList?.Select(x => (short?)x.Opcoid).ToList();

            var excludePlatformList = _dapperCommonManager.GetHardwarePlatformForExodusFilterAsync().Result;
            var hwPlatform = excludePlatformList?.Select(X => X.Platformid).ToList();



            var query = isTargetDc ? _repositoryWrapper.PlannedActivity.FindByCondition(predicateResult, false)
                   .Include(x => x.Daassetmigration)
                   .Include(x => x.Designcomponentfamily)
                   .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                   .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                   .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                   .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                   .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                   .Include(x => x.Activitystatus)
                   .Include(x => x.Planningactivitystatus)
                   .Include(x => x.Deliverystatus)
                   .Include(x => x.ProgramNavigation)
                   .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents)
                   .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                   .Include(x => x.Plannedactivityresource).ThenInclude(x => x.RulelinkeddcNavigation)
                   .Include(x => x.Designaspect).ThenInclude(x => x.Opco)
                   .Include(x => x.Opco).ToListAsync()
                   : _repositoryWrapper.PlannedActivity.FindByCondition(predicateResult, false).Where(x =>
                    (x.Plannedactivityresource.Rulelinkeddc == (int)PlannedActivityResourceEnum.Platform_Migration
                    && !excludedOpcoid.Contains(x.Opcoid)
                         && x.Designaspect.Designcomponentfamily.Designcomponents.Any(f => f.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwOem.Contains(y.Majorhardware.Orgeqpmanufacturerid)))
                         && x.Designaspect.Designcomponentfamily.Designcomponents.Any(f => f.Systemtype.Systemtypesmajorhardwarebuilds.Any(y => hwPlatform.Contains(y.Majorhardware.Platformid)))
                         ))
                   .Include(x => x.Daassetmigration)
                   .Include(x => x.Designcomponentfamily)
                   .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                   .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                   .Include(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Buildconstruction)
                   .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                   .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds).ThenInclude(x => x.Orgeqpmanufacturer)
                   .Include(x => x.Activitystatus)
                   .Include(x => x.Planningactivitystatus)
                   .Include(x => x.Deliverystatus)
                   .Include(x => x.ProgramNavigation)
                   .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents)
                   .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Systemtypesmajorhardwarebuilds).ThenInclude(x => x.Majorhardware).ThenInclude(x => x.Platform)
                   .Include(x => x.Designaspect).ThenInclude(x => x.Designcomponentfamily).ThenInclude(x => x.Designcomponents).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                   .Include(x => x.Plannedactivityresource).ThenInclude(x => x.RulelinkeddcNavigation)
                   .Include(x => x.Designaspect).ThenInclude(x => x.Opco)
                   .Include(x => x.Opco).ToListAsync();

            return await query;
        }
        public async Task<List<PlannedActivityAtGlanceGridDto>> GetDaWithOutPaRecords(ExodusGraphicalReportQueryDto buildFilterDto)
        {
            var noPaPredicateResult = ApplyFilter(buildFilterDto);

            var noPaPredicateInner = PredicateBuilder.New<Designaspects>(true);
            noPaPredicateInner.Or(x => !x.Plannedactivities.Any());

            noPaPredicateResult.And(noPaPredicateInner);

            var daBaseQueryResult = await Task.Run(() => GetQuery(noPaPredicateResult, false));

            var noPaLcmEntity = daBaseQueryResult
                .AsEnumerable() // important
                .Select(m =>
                {
                    var nodesCount = GetNodeCountBasedOpcoAndDcfCombination(
                        (short)m.Opcoid,
                        m.Designcomponentfamilyid);

                    var compatibilityColorCode = CalculateCompliance(
                        m.Plannedactivities.ToList(), isTargetDc);
                    var currentDcf = m?.Designcomponentfamily?.toDesignComponentFamilyName(_repositoryWrapper);  // da - dcf



                    return new PlannedActivityAtGlanceGridDto
                    {
                        DesignAspectId = m.Id,
                        OpCoDescrption = m.Opco.Opco,
                        OpcoId = (short)m.Opcoid,
                        NodesCount = nodesCount,
                        PACurrentDCFName = currentDcf,
                        greenNodeCount = compatibilityColorCode == ConstantValueFilter.Green ? nodesCount : 0,
                        amberNodeCount = compatibilityColorCode == ConstantValueFilter.Amber ? nodesCount : 0,
                        redNodeCount = compatibilityColorCode == ConstantValueFilter.Red ? nodesCount : 0,

                        PAPlannedDCName = "undefined",
                        compatibilityColorCode = compatibilityColorCode
                    };
                })
                .OrderBy(x => x.OpCoDescrption)
                .ThenBy(x => x.PACurrentDCName)
                .ToList();

            return noPaLcmEntity;
        }    


        public async Task<ResultDto> FindWithConditionForPALevelPercentage(ExodusGraphicalReportQueryDto buildFilterDto)
        {
            //Method variables
            var platformMigrationRecords = new List<PlannedActivityAtGlanceGridDto>();
            var dropdownObjects = new ResultDto();
            var targetPlatformRecords = new List<PlannedActivityAtGlanceGridDto>();

            var predicateResult = ApplyPlannedActivityFilter(buildFilterDto, filteredPlannedActivity);
            var daPaBaseQueryResult = await GetDesignAspectPlannedActivity(predicateResult);

            #region DaWithOutPaRecords
            var daWithOutPaEntity = isTargetDc ? new List<PlannedActivityAtGlanceGridDto>() : await GetDaWithOutPaRecords(buildFilterDto);
            #endregion
            var noPaDaRecords = new List<PlannedActivityAtGlanceGridDto>();

            //1447
            var allRecords = daPaBaseQueryResult.Select(
            m =>
            {

                var NodesCount = (m?.Designaspectid != null) ? GetNodeCountBasedOpcoAndDcfCombination(m.Designaspect.Opcoid, m.Designaspect.Designcomponentfamilyid)
                                        : 0;
                string compatibilityColorCode = CalculateCompliance(m, isTargetDc);


                // Get counts and sums for each color code
                int totalGreenCount = compatibilityColorCode == ConstantValueFilter.Green ? NodesCount : 0;
                int totalAmberCount = compatibilityColorCode == ConstantValueFilter.Amber ? NodesCount : 0;
                int totalRedCount = compatibilityColorCode == ConstantValueFilter.Red ? NodesCount : 0;

                //Get Nodes Count for Planned Design Component
                long paDesignComponentFamilyId = (long)(m?.Designcomponentfamilyid ?? 0);
                short opcoId = (short)m.Opcoid;
                var paPlannedDCNodesCount = GetNodeCountBasedOpcoAndDcfCombination(opcoId, paDesignComponentFamilyId);
                var currentDcfName = m.Designaspect.Designcomponentfamily.toDesignComponentFamilyName(_repositoryWrapper);
                var plannedDcfName = m.Designcomponentfamily.toDesignComponentFamilyName(_repositoryWrapper);

                return new PlannedActivityAtGlanceGridDto
                {
                    DesignAspectId = m?.Designaspectid,
                    OpcoId = (short)m.Opcoid,
                    OpCoDescrption = m.Opco.Opco,
                    PlannedActivityId = m.Plannedactivityid,
                    PADelivertyStatusId = m?.Deliverystatusid,
                    PADelivertyStatusDesc = m?.Deliverystatus?.Deliverystatus,
                    PAResourceId = m.Plannedactivityresourceid,
                    PAResourceRuleLinkedDesc = m.Plannedactivityresource.RulelinkeddcNavigation.Plannedactivitytypedescription,
                    PAResourceRuleLinkedId = m.Plannedactivityresource.Rulelinkeddc,
                    PAResourceDesc = m.Plannedactivityresource.Plannedactivityresource,

                    PAActivityStatusId = m.Activitystatus.Activitystatusid,
                    PAActivityStatusDesc = m.Activitystatus.Activitystatus,

                    PAActivityTextDesc = m.Activitydetails == null && m.Activitydetails == "" ? string.Empty : m.Activitydetails,
                    StartDateValue = m.Startdate != null ? m.Startdate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty,
                    PlannedCompletion = m.Plannedcompletion != null ? m.Plannedcompletion.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty,
                    PlannedImplementationYear = string.IsNullOrEmpty(Convert.ToString(m.Plannedimplementationyear)) ? string.Empty : m.Plannedimplementationyear.ToString(),

                    BudgetAvailabilityValue = (m.Budgetavailabilityid == 1) ? ConstantValueFilter.YES : ConstantValueFilter.NO,
                    LocalApprovalValue = m.Localapproval,

                    PACurrentDCFName = currentDcfName,
                    PADCFName = plannedDcfName,

                    BudgetValue = m.Budgetvalue,
                    Currency = m.Currency,

                    BudgetTrackingId = m.Budgettrackingid,
                    Program = m.ProgramNavigation?.Programdescription,

                    ProjectOwner = m.Projectowner,
                    ResponsibilityPhase = m?.Responsibilityphase?.Responsibilityphase,
                    ResponsibilityPhaseId = m.Plannedactivityresourceid,

                    DeliveryProjectNameWBSCode = m.Deliveryprojectname,
                    DeliveryProjectIdPPMID = m.Deliveryprojectname,
                    IsLCMReleaseDetailsUnknown = (m?.Lcmengineering?.Isreleasedetailunknown == true) ? ConstantValueFilter.YES : ConstantValueFilter.NO,

                    IsPAReleaseDetailsUnknown = (m?.Ispareleasedetailunknown == true) ? ConstantValueFilter.YES : ConstantValueFilter.NO,

                    compatibilityColorCode = compatibilityColorCode,// calculateRagStatusFromSoftware(m?.Lcmengineering, buildFilterDto.SelectedDate),
                    NodesCount = NodesCount,
                    greenNodeCount = totalGreenCount,
                    redNodeCount = totalRedCount,
                    amberNodeCount = totalAmberCount,
                    PAPlannedDCNodesCount = paPlannedDCNodesCount

                };


            });

            if (isTargetDc == false)
            {
                platformMigrationRecords = allRecords?.Where(x => x.PAResourceRuleLinkedId == (int)PlannedActivityResourceEnum.Platform_Migration).OrderBy(x => x.PACurrentDCFName).ToList();//?.ToList();
                noPaDaRecords = allRecords?.Where(x => x.PAResourceRuleLinkedId == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA).OrderBy(x => x.PACurrentDCFName).ToList();

                buildFilterDto.PlannedActivityResourceRuleId ??= new List<short>();
                buildFilterDto.PlannedActivityResourceRuleId.Add((int)PlannedActivityResourceEnum.Platform_Migration);
                dropdownObjects = await GetPADropdown(buildFilterDto);
            }
            else if (isTargetDc)
            {
                platformMigrationRecords = allRecords.ToList();
                buildFilterDto.PlannedActivityResourceRuleId ??= new List<short>();
                buildFilterDto.PlannedActivityResourceRuleId.Add((int)PlannedActivityResourceEnum.Platform_Migration);
                dropdownObjects = await GetPADropdown(buildFilterDto);
            }
            else
            {

            }


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    platformMigrationRecords,
                    noPaDaRecords,
                    daWithOutPaEntity,
                    //targetPlatformRecords,
                    paRelatedDropDown = dropdownObjects?.Data,
                }
            };

        }
        #endregion


        #region PALevelPercentageForAsset
        public async Task<ResultDto> FindWithConditionForPALevelPercentageForAsset(ExodusGraphicalReportQueryDto buildFilterDto)
        {
            //Method variables
            var platformMigrationRecords = new List<PlannedActivityAtGlanceGridDto>();
            var dropdownObjects = new ResultDto();
            var targetPlatformRecords = new List<PlannedActivityAtGlanceGridDto>();

            var predicateResult = ApplyPlannedActivityFilter(buildFilterDto, filteredPlannedActivity);
            var daPaBaseQueryResult = await GetDesignAspectPlannedActivity(predicateResult);
            
            #region DaWithOutPaRecords
            var daWithOutPaEntity = isTargetDc ? new List<PlannedActivityAtGlanceGridDto>() : await GetDaWithOutPaRecords(buildFilterDto);
            #endregion

            var noPaDaRecords = new List<PlannedActivityAtGlanceGridDto>();

            var designAspectIds = daPaBaseQueryResult.Select(x => x.Designaspectid).ToList();

            var VerticalDetails = _repositoryWrapper.DesignAspectRepository.FindByCondition(f => designAspectIds.Contains(f.Id)).Select(r => new Designaspects
            {
                Id = r.Id,
                Designcomponentfamily = r.Designcomponentfamily == null ? null : new Designcomponentfamilies
                {
                    Designcomponents = r.Designcomponentfamily.Designcomponents == null
                ? null
                : r.Designcomponentfamily.Designcomponents
                    .Select(dc => new Designcomponents
                    {
                        Systemtype = dc.Systemtype == null ? null : new Systemtypes
                        {
                            Majorsoftwarebuilds = dc.Systemtype.Majorsoftwarebuilds == null ? null : new Majorsoftwarebuilds
                            {
                                Majorswbuildsdesigncontacts = dc.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts != null && dc.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Count <= 0 ? null :
                                dc.Systemtype.Majorsoftwarebuilds.Majorswbuildsdesigncontacts.Select(ms => new Majorswbuildsdesigncontacts
                                {
                                    Designcontactid = ms.Designcontactid,
                                    Deleted = ms.Deleted,
                                })
                                 .ToList()
                            },
                            Systemtypesmajorhardwarebuilds = dc.Systemtype.Systemtypesmajorhardwarebuilds != null && dc.Systemtype.Systemtypesmajorhardwarebuilds.Count <= 0 ? null :
                            dc.Systemtype.Systemtypesmajorhardwarebuilds.Select(hw => new Systemtypesmajorhardwarebuilds
                            {
                                Majorhardware = hw.Majorhardware == null ? null : new Majorhardwarebuilds
                                {
                                    Majorhwbuildsdesigncontacts = hw.Majorhardware.Majorhwbuildsdesigncontacts != null && hw.Majorhardware.Majorhwbuildsdesigncontacts.Count <= 0 ? null :
                                    hw.Majorhardware.Majorhwbuildsdesigncontacts.Select(ds => new Majorhwbuildsdesigncontacts
                                    {
                                        Designcontactid = ds.Designcontactid,
                                        Deleted = ds.Deleted,
                                    }).ToList(),
                                }
                            }).ToList(),
                        }
                    })
                    .ToList()
                }
            })
            .ToDictionary(x => x.Id, x =>
            {
                var designContactIds = new List<int?>();

                var majorSoftwareContactIds = x?.Designcomponentfamily?.Designcomponents?
                .SelectMany(t =>
                    t?.Systemtype?.Majorsoftwarebuilds?.Majorswbuildsdesigncontacts?
                        .Where(c => c.Deleted == false)
                        .Select(c => (int?)c.Designcontactid)
                    ?? Enumerable.Empty<int?>())
                .Distinct()
                .ToList();

                if (majorSoftwareContactIds?.Any() == true)
                    designContactIds = majorSoftwareContactIds;

                var majorHardwareContactIds = x?.Designcomponentfamily?.Designcomponents?
                .SelectMany(t =>
                    t?.Systemtype?.Systemtypesmajorhardwarebuilds?
                        .SelectMany(m =>
                            m.Majorhardware?.Majorhwbuildsdesigncontacts?
                                .Where(c => c.Deleted == false)
                                .Select(c => (int?)c.Designcontactid)
                            ?? Enumerable.Empty<int?>())
                        ?? Enumerable.Empty<int?>())
                .Distinct()
                .ToList();

                if (designContactIds.Any() && majorHardwareContactIds?.Any() == true)
                    designContactIds = designContactIds.Union(majorHardwareContactIds).ToList();
                else if (!designContactIds.Any() && majorHardwareContactIds?.Any() == true)
                    designContactIds = majorHardwareContactIds;

                return designContactIds.Any()
                    ? _commonManager.GetVerticaleNameDynamicFormat(designContactIds, 0)
                        .Select(v => new FilterValueDtoKeyValueList
                        {
                            Key = Convert.ToInt16(v.Value),
                            Value = v.Text
                        })
                        .Distinct()
                        .ToList()
                    : new List<FilterValueDtoKeyValueList>();

            });

            #region // com
            //1447
            //var allRecords = daPaBaseQueryResult.Select(
            //m =>
            //{

            //    var NodesCount = (m?.Designaspectid != null) ? GetNodeCountBasedOpcoAndDcfCombination(m.Designaspect.Opcoid, m.Designaspect.Designcomponentfamilyid)
            //                            : 0;
            //    string compatibilityColorCode = CalculateCompliance(m.Daassetmigration?.ToList(), isTargetDc);


            //    // Get counts and sums for each color code
            //    int totalGreenCount = compatibilityColorCode == ConstantValueFilter.Green ? NodesCount : 0;
            //    int totalAmberCount = compatibilityColorCode == ConstantValueFilter.Amber ? NodesCount : 0;
            //    int totalRedCount = compatibilityColorCode == ConstantValueFilter.Red ? NodesCount : 0;

            //    //Get Nodes Count for Planned Design Component
            //    long paDesignComponentFamilyId = (long)(m?.Designcomponentfamilyid ?? 0);
            //    short opcoId = (short)m.Opcoid;
            //    var paPlannedDCNodesCount = GetNodeCountBasedOpcoAndDcfCombination(opcoId, paDesignComponentFamilyId);
            //    var currentDcfName = m.Designaspect.Designcomponentfamily.toDesignComponentFamilyName(_repositoryWrapper);
            //    var plannedDcfName = m.Designcomponentfamily.toDesignComponentFamilyName(_repositoryWrapper);

            //    return new PlannedActivityAtGlanceGridDto
            //    {
            //        DesignAspectId = m?.Designaspectid,
            //        OpcoId = (short)m.Opcoid,
            //        OpCoDescrption = m.Opco.Opco,
            //        PlannedActivityId = m.Plannedactivityid,
            //        PADelivertyStatusId = m?.Deliverystatusid,
            //        PADelivertyStatusDesc = m?.Deliverystatus?.Deliverystatus,
            //        PAResourceId = m.Plannedactivityresourceid,
            //        PAResourceRuleLinkedDesc = m.Plannedactivityresource.RulelinkeddcNavigation.Plannedactivitytypedescription,
            //        PAResourceRuleLinkedId = m.Plannedactivityresource.Rulelinkeddc,
            //        PAResourceDesc = m.Plannedactivityresource.Plannedactivityresource,

            //        PAActivityStatusId = m.Activitystatus.Activitystatusid,
            //        PAActivityStatusDesc = m.Activitystatus.Activitystatus,

            //        PAActivityTextDesc = m.Activitydetails == null && m.Activitydetails == "" ? string.Empty : m.Activitydetails,
            //        StartDateValue = m.Startdate != null ? m.Startdate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty,
            //        PlannedCompletion = m.Plannedcompletion != null ? m.Plannedcompletion.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty,
            //        PlannedImplementationYear = string.IsNullOrEmpty(Convert.ToString(m.Plannedimplementationyear)) ? string.Empty : m.Plannedimplementationyear.ToString(),

            //        BudgetAvailabilityValue = (m.Budgetavailabilityid == 1) ? ConstantValueFilter.YES : ConstantValueFilter.NO,
            //        LocalApprovalValue = m.Localapproval,

            //        PACurrentDCFName = currentDcfName,
            //        PADCFName = plannedDcfName,

            //        BudgetValue = m.Budgetvalue,
            //        Currency = m.Currency,

            //        BudgetTrackingId = m.Budgettrackingid,
            //        Program = m.ProgramNavigation?.Programdescription,

            //        ProjectOwner = m.Projectowner,
            //        ResponsibilityPhase = m?.Responsibilityphase?.Responsibilityphase,
            //        ResponsibilityPhaseId = m.Plannedactivityresourceid,

            //        DeliveryProjectNameWBSCode = m.Deliveryprojectname,
            //        DeliveryProjectIdPPMID = m.Deliveryprojectname,
            //        IsLCMReleaseDetailsUnknown = (m?.Lcmengineering?.Isreleasedetailunknown == true) ? ConstantValueFilter.YES : ConstantValueFilter.NO,

            //        IsPAReleaseDetailsUnknown = (m?.Ispareleasedetailunknown == true) ? ConstantValueFilter.YES : ConstantValueFilter.NO,

            //        compatibilityColorCode = compatibilityColorCode,// calculateRagStatusFromSoftware(m?.Lcmengineering, buildFilterDto.SelectedDate),
            //        NodesCount = NodesCount,
            //        greenNodeCount = totalGreenCount,
            //        redNodeCount = totalRedCount,
            //        amberNodeCount = totalAmberCount,
            //        PAPlannedDCNodesCount = paPlannedDCNodesCount

            //    };


            //});
            #endregion

            var allRecords = daPaBaseQueryResult.SelectMany(m =>
            {
                // No DaAssetMigration records
                if (m.Daassetmigration == null)
                {
                    int nodesCount = m?.Designaspectid != null
                        ? GetNodeCountBasedOpcoAndDcfCombination(
                            m.Designaspect.Opcoid,
                            m.Designaspect.Designcomponentfamilyid)
                        : 0;

                    string compatibilityColorCode = ConstantValueFilter.Red;

                    long paDesignComponentFamilyId = (long)(m?.Designcomponentfamilyid ?? 0);
                    short opcoId = (short)m.Opcoid;

                    var paPlannedDCNodesCount =
                        GetNodeCountBasedOpcoAndDcfCombination(opcoId, paDesignComponentFamilyId);

                    var currentDcfName = m.Designaspect.Designcomponentfamily
                        .toDesignComponentFamilyName(_repositoryWrapper);

                    var plannedDcfName = m.Designcomponentfamily
                        .toDesignComponentFamilyName(_repositoryWrapper);

                    return new List<PlannedActivityAtGlanceGridDto>
                    {
                        new PlannedActivityAtGlanceGridDto
                        {
                            DesignAspectId = m?.Designaspectid,
                            OpcoId = (short)m.Opcoid,
                            OpCoDescrption = m.Opco.Opco,

                            PlannedActivityId = m.Plannedactivityid,

                            PADelivertyStatusId = m?.Deliverystatusid,
                            PADelivertyStatusDesc = m?.Deliverystatus?.Deliverystatus,
                            PAResourceId = m.Plannedactivityresourceid,
                            PAResourceRuleLinkedDesc = m.Plannedactivityresource.RulelinkeddcNavigation.Plannedactivitytypedescription,
                            PAResourceRuleLinkedId = m.Plannedactivityresource.Rulelinkeddc,
                            PAResourceDesc = m.Plannedactivityresource.Plannedactivityresource,
                            PAActivityStatusId = m.Activitystatus.Activitystatusid,
                            PAActivityStatusDesc = m.Activitystatus.Activitystatus,

                            PAActivityTextDesc = string.IsNullOrEmpty(m.Activitydetails)
                                ? string.Empty
                                : m.Activitydetails,

                            StartDateValue = m.Startdate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
                            PlannedCompletion = m.Plannedcompletion?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
                            PlannedImplementationYear = string.IsNullOrEmpty(Convert.ToString(m.Plannedimplementationyear))
                                ? string.Empty
                                : m.Plannedimplementationyear.ToString(),

                            PACurrentDCFName = currentDcfName,
                            PADCFName = plannedDcfName,

                            compatibilityColorCode = compatibilityColorCode,

                            NodesCount = nodesCount,
                            greenNodeCount = compatibilityColorCode == ConstantValueFilter.Green ? nodesCount : 0,
                            amberNodeCount = compatibilityColorCode == ConstantValueFilter.Amber ? nodesCount : 0,
                            redNodeCount = compatibilityColorCode == ConstantValueFilter.Red ? nodesCount : 0,

                            PAPlannedDCNodesCount = paPlannedDCNodesCount,
                            VerticalFilterDto = VerticalDetails.GetValueOrDefault(m.Designaspectid.Value),

                        }

                    };
                }

                var groupedMigrations = m.Daassetmigration
                    .Where(x => x.Deleted == false)
                    .Select(dm => new
                    {
                        Migration = dm,
                        Color = CalculateCompliance(dm, isTargetDc)
                    })
                    .GroupBy(x => x.Color);

                return groupedMigrations.Select(group =>
                {
                    string compatibilityColorCode = group.Key;
                    int nodesCount = group.Count();

                    long paDesignComponentFamilyId = (long)(m?.Designcomponentfamilyid ?? 0);
                    short opcoId = (short)m.Opcoid;

                    var paPlannedDCNodesCount =
                        GetNodeCountBasedOpcoAndDcfCombination(opcoId, paDesignComponentFamilyId);

                    var currentDcfName = m.Designaspect.Designcomponentfamily
                        .toDesignComponentFamilyName(_repositoryWrapper);

                    var plannedDcfName = m.Designcomponentfamily
                        .toDesignComponentFamilyName(_repositoryWrapper);

                    return new PlannedActivityAtGlanceGridDto
                    {
                        DesignAspectId = m?.Designaspectid,
                        OpcoId = (short)m.Opcoid,
                        OpCoDescrption = m.Opco.Opco,

                        PlannedActivityId = m.Plannedactivityid,

                        PADelivertyStatusId = m?.Deliverystatusid,
                        PADelivertyStatusDesc = m?.Deliverystatus?.Deliverystatus,
                        PAResourceId = m.Plannedactivityresourceid,
                        PAResourceRuleLinkedDesc = m.Plannedactivityresource.RulelinkeddcNavigation.Plannedactivitytypedescription,
                        PAResourceRuleLinkedId = m.Plannedactivityresource.Rulelinkeddc,
                        PAResourceDesc = m.Plannedactivityresource.Plannedactivityresource,
                        PAActivityStatusId = m.Activitystatus.Activitystatusid,
                        PAActivityStatusDesc = m.Activitystatus.Activitystatus,

                        PAActivityTextDesc = string.IsNullOrEmpty(m.Activitydetails)
                            ? string.Empty
                            : m.Activitydetails,

                        StartDateValue = m.Startdate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
                        PlannedCompletion = m.Plannedcompletion?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? string.Empty,
                        PlannedImplementationYear = string.IsNullOrEmpty(Convert.ToString(m.Plannedimplementationyear))
                            ? string.Empty
                            : m.Plannedimplementationyear.ToString(),

                        PACurrentDCFName = currentDcfName,
                        PADCFName = plannedDcfName,

                        compatibilityColorCode = compatibilityColorCode,

                        NodesCount = nodesCount,
                        greenNodeCount = compatibilityColorCode == ConstantValueFilter.Green ? nodesCount : 0,
                        amberNodeCount = compatibilityColorCode == ConstantValueFilter.Amber ? nodesCount : 0,
                        redNodeCount = compatibilityColorCode == ConstantValueFilter.Red ? nodesCount : 0,

                        PAPlannedDCNodesCount = paPlannedDCNodesCount,
                        VerticalFilterDto = VerticalDetails.GetValueOrDefault(m.Designaspectid.Value),

                    };
                });
            }).ToList();

            if (buildFilterDto.VerticalId?.Any() == true)
            {
                foreach (var item in buildFilterDto.VerticalId)
                {
                    allRecords = allRecords.Where(f => f.VerticalFilterDto.Any(r => r.Key == item)).ToList();
                }
            }



            if (isTargetDc == false)
            {
                platformMigrationRecords = allRecords?.Where(x => x.PAResourceRuleLinkedId == (int)PlannedActivityResourceEnum.Platform_Migration).OrderBy(x => x.PACurrentDCFName).ToList();//?.ToList();
                noPaDaRecords = allRecords?.Where(x => x.PAResourceRuleLinkedId == (int)PlannedActivityResourceEnum.No_PlannedActivity_For_DA).OrderBy(x => x.PACurrentDCFName).ToList();

                buildFilterDto.PlannedActivityResourceRuleId ??= new List<short>();
                buildFilterDto.PlannedActivityResourceRuleId.Add((int)PlannedActivityResourceEnum.Platform_Migration);
                dropdownObjects = await GetPADropdown(buildFilterDto);
            }
            else if (isTargetDc)
            {
                platformMigrationRecords = allRecords.ToList();
                buildFilterDto.PlannedActivityResourceRuleId ??= new List<short>();
                buildFilterDto.PlannedActivityResourceRuleId.Add((int)PlannedActivityResourceEnum.Platform_Migration);
                dropdownObjects = await GetPADropdown(buildFilterDto);
            }
            else
            {

            }


            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    platformMigrationRecords,
                    noPaDaRecords,
                    daWithOutPaEntity,
                    //targetPlatformRecords,
                    paRelatedDropDown = dropdownObjects?.Data,
                }
            };

        }

        public string CalculateCompliance(List<Daassetmigration> daassetmigrations, bool targetDc)
        {

            if (daassetmigrations == null || !daassetmigrations.Any())
                return ConstantValueFilter.Red;

            daassetmigrations = daassetmigrations.Where(x => /*x.Isdecommissioned == false &&*/ x.Deleted == false).ToList();

            /*This block is used for calculating TargetDc */
            if (targetDc)
            {
                bool isTargetDc = daassetmigrations.Any(pa => ConstantValueFilter.InfraBuildConstructions.Contains(pa.Platform.Platform));

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }

            if (daassetmigrations.Any(x => x.Migrationcompletiondate <= complainceDate))
            {
                return ConstantValueFilter.Green;
            }
            else if (daassetmigrations.Any(x => x.Migrationcompletiondate > complainceDate))
            {
                return ConstantValueFilter.Amber;
            }

            return ConstantValueFilter.Red;
        }
        public string CalculateCompliance(Daassetmigration daassetmigration, bool targetDc)
        {
            if (daassetmigration == null)
                return ConstantValueFilter.Red;

            if (targetDc)
            {
                bool isTargetDc = ConstantValueFilter.InfraBuildConstructions.Contains(daassetmigration.Platform?.Platform);

                if (isTargetDc)
                    return ConstantValueFilter.Green;
            }

            if (daassetmigration.Migrationcompletiondate <= complainceDate)
            {
                return ConstantValueFilter.Green;
            }
            else if (daassetmigration.Migrationcompletiondate > complainceDate)
            {
                return ConstantValueFilter.Amber;
            }

            return ConstantValueFilter.Red;
        }

        #endregion

    }
}



