using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.ExtensionMethod.DesignComponent;
using CAM.BusinessManager.ExtensionMethod.DesignComponentFamily;
using CAM.BusinessManager.ExtensionMethod.LcmEngineering;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.Rules;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.DataTransferObjects;
using CAM.DataTransferObjects.Entita.LcmEngineering;
using CAM.DataTransferObjects.Entita.PlannedActivity;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto;
using CAM.Entities.Mappers.Entity;
using CAM.Enum;
using CAM.Infrastucture;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static CAM.BusinessManager.Rules.LCMEngineeringRulesExtension;

namespace CAM.BusinessManager.Entity
{
    public class LcmAtGlanceForEosManager : BaseManager
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private GridCustomColumnManager _manager;
        private DropdownDataServiceManager _dropdownDataServiceManager;
        private readonly CommonManager _commonManager;
        private DateTime currentDate = System.DateTime.Now.Date;

        private DateTime defaultEndOfSupport = System.DateTime.Now.Date.AddYears(ConstantValueFilter.eosCalulatedDayCount);

        private Dictionary<int, string> filteredPlannedActivity = new Dictionary<int, string>();

        private DateTime EndOfMaintenanceDate = DateTime.Now.Date;
        private DateTime EndOfSupportDate = DateTime.Now.Date;

        public LcmAtGlanceForEosManager(IEnumerable<IRepositoryWrapper> wrappers,
            IHttpContextAccessor contextAccessor, IRepositoryWrapper repositoryWrapper, CommonManager commonManager, GridCustomColumnManager manager,
            DropdownDataServiceManager dropdownDataServiceManager) : base(contextAccessor, wrappers, out repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _manager = manager;
            _dropdownDataServiceManager = dropdownDataServiceManager;
            _commonManager = commonManager;
        }

        public async Task<List<Lcmengineering>> GetQuery(ExpressionStarter<Lcmengineering> predicateResult)
        {
            var lcms = _repositoryWrapper.Lcmengineering.FindByCondition(predicateResult)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Designcomponentfamily).ThenInclude(x => x.Subnetworkboundary).ThenInclude(p => p.Subnetworksupportedsvr).ThenInclude(p => p.Service)
                  .Include(p => p.Designcomponent).ThenInclude(x => x.Subnetworkboundary).ThenInclude(p => p.Subnetworksupportedsvr).ThenInclude(p => p.Service)
                  .Include(p => p.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                  .Include(p => p.Opco).OrderBy(P => P.Opco.Opco)
                   .Include(p => p.Productimportance)
                 .Include(p => p.Lcmengineeringsubdomainspoc)
                 .Include(p => p.PlannedactivitiesLcmengineering)
                  .ToListAsync();
            return await lcms;
        }

        public async Task<IEnumerable<LcmAtGlanceGridDto>> GetBaseLcmGlanceRecords(List<Lcmengineering> lcmEntity, DateTime? selectedDate, bool isPageLoad = false)
        {
           
            var lcmGlanceEntity = await Task.WhenAll( lcmEntity.Where(m => m.Lcmengineeringsubdomainspoc.Any()).Select(async y => new LcmAtGlanceGridDto()
            {
                LcmengineeringId = (long)y.Lcmengineeringid,
                OpcoId = (long)y.Opcoid,
                OpCoDescrption = y.Opco.Opco,
                NodesCount = y.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result,
                compatibilityColorCode = calculateRagStatusFromSoftware(y, selectedDate),
                ProductName = y.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                ProductId = (long)y.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid,
                VerticalFilterDto = _commonManager.GetVerticaleFilterDto(y.Lcmengineeringsubdomainspoc.Select(x => x.Subdomainspocid).ToList(), y.Opcoid, false, true)
                .Distinct().ToList(),
                ProductImportantFilterDto =
                new FilterValueDto()
                {
                    Value = y?.Productimportanceid?.ToString(),
                    Text = y?.Productimportance?.Productimportance
                }
 
            })); 
            if (isPageLoad)
            {
                var getVoicCoreVerticalId = GetVerticalVoicCoreId();
                var returnValue = await Task.Run(() => lcmGlanceEntity.ToList().Where(x => x.VerticalFilterDto.Any(y => getVoicCoreVerticalId.Any(t => t.ToString() == y.Value))));
                return returnValue;
            }
            else
            {
                return lcmGlanceEntity;
            }
           
        }
        public async Task<ResultDto> FindWithConditionForOverAllOpco()
        {

            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);

            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            var zOpcoid = GetZOpcoId();

            predicateInner.Or(x => x.Archived == false);
            predicateResult.And(predicateInner);

            predicateInner.Or(x => x.Opco.Opcoid != zOpcoid);
            predicateResult.And(predicateInner);

            var lcmBaseQueryResult = await GetQuery(predicateResult);


            var lcmGlanceEntity = await  GetBaseLcmGlanceRecords(lcmBaseQueryResult, null);
            int overAllCount = lcmGlanceEntity.Count();
            int overAllNodeCount = (int)lcmGlanceEntity.Sum(s => s.NodesCount);

            var overAllOpcoPercentage = await  GetOverAllOpcoAndAssetWisePercentageForEofs(lcmBaseQueryResult,null);

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = overAllOpcoPercentage
            };

        }

        public async Task<ResultDto> FindWithConditionForPALevelPercentage(LcmAtGlanceQueryDto buildFilterDto)
        { 
            var predicateResult = ApplyPlannedActivityFilter(buildFilterDto, filteredPlannedActivity);
            var lcmPaBaseQueryResult = await GetLCMPlannedActivity(predicateResult);// .Where(x => x.Plannedactivityid == 1790);

            var lcmWithOutPaEntity = new List<PlannedActivityAtGlanceGridDto>();
            #region
            if (buildFilterDto.PlannedActivityResourceRuleId != null && buildFilterDto.PlannedActivityResourceRuleId.Any())
            {
                if (buildFilterDto.PlannedActivityResourceRuleId.Any(x => x == (int)PlannedActivityResourceEnum.No_PlannedActivity))
                {
                    lcmWithOutPaEntity = await GetLcmWithOutPaRecords(buildFilterDto);
                }
            }
            #endregion

            //1447
            var paRecords = lcmPaBaseQueryResult.Select(
       m =>
       {

           var NodesCount = (m?.Lcmengineering != null) ? m.Lcmengineering.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result
                                   : 0;
           string compatibilityColorCode = calculateRagStatusFromSoftware(m?.Lcmengineering, buildFilterDto.SelectedDate);


           // Get counts and sums for each color code
           int totalGreenCount = compatibilityColorCode == ConstantValueFilter.Green ? NodesCount : 0;
           int totalRedCount = compatibilityColorCode == ConstantValueFilter.Red ? NodesCount : 0;

           //Get Nodes Count for Planned Design Component
           long paDesignComponentId = (long)(m?.Designcomponentid ?? 0);
           short opcoId = (short)m.Opcoid;
           var paPlannedDCNodesCount = GetPAPlannedDCNodesCount(paDesignComponentId, opcoId);

           return new PlannedActivityAtGlanceGridDto
           {
               LcmengineeringId = m?.Lcmengineeringid,
               OpcoId = (long)m.Opcoid,
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

               PADCFName = toOriginalDesignComponentFamilyDescription(m), //
               PACurrentDCName = m?.Lcmengineering?.Designcomponent?.toDesignComponentNameLcm(_repositoryWrapper),  // lcm - dc
               PAPlannedDCName = m?.Designcomponent?.toDesignComponentNameLcm(_repositoryWrapper),  // PA - DC 

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
               PAPlannedDCNodesCount = paPlannedDCNodesCount

           };


       });

            paRecords = paRecords?.OrderBy(x => x.PACurrentDCName);//?.ToList();

            var dropdownObjects = await GetPADropdown(buildFilterDto);
            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    paRecords,
                    paRelatedDropDown = dropdownObjects?.Data,
                    lcmWithOutPaEntity
                }
            };

        }

        public async Task<ResultDto> FindWithCondition(LcmAtGlanceQueryDto buildFilterDto, bool isOpcoWise = false, bool isProductWise = false, bool isServiceWise = false, bool isOverAllOpcoWise = false, bool isPageload = false)
        {
            if (isPageload && (buildFilterDto.VerticalResponsibleId != null && buildFilterDto.VerticalResponsibleId.Any()))
            {
                isPageload = false;
            }

            var predicateResult = ApplyFilter(buildFilterDto, isPageload);

            var lcmBaseQueryResult = await Task.Run(() => GetQuery(predicateResult));



            var lcmGlanceEntity = await GetBaseLcmGlanceRecords(lcmBaseQueryResult, buildFilterDto.SelectedDate, isPageload);

            int overAllCount = lcmGlanceEntity.Count();

            int overAllNodeCount = (int)lcmGlanceEntity.Sum(s => s.NodesCount);

            var opcoWisePercentage = (isOpcoWise) ? await GetOpcoWisePercentage(lcmGlanceEntity, overAllCount, overAllNodeCount) : null;

            var productWisePercentage = (isProductWise) ? await GetProductwisePercentage(lcmGlanceEntity, overAllCount, overAllNodeCount) : null;

            var subnetworkWisePercentage = (isServiceWise) ? await GetSupportedServiceWisePercentage(lcmBaseQueryResult, buildFilterDto) : null;

            //var overAllOpcoPercentage = (isOverAllOpcoWise) ? await GetOverAllOpcoAndAssetWisePercentage(lcmGlanceEntity, overAllCount, overAllNodeCount) : null;
            var overAllOpcoPercentage = (isOverAllOpcoWise) ? await GetOverAllOpcoAndAssetWisePercentageForEofs(lcmBaseQueryResult, buildFilterDto.SelectedDate) : null;

            

            var verticalFilterValueBasedOnFilter = lcmGlanceEntity.SelectMany(m => m.VerticalFilterDto)?.Distinct()?.ToList();

            var productImportanceFilter = lcmGlanceEntity?.Select(m => m.ProductImportantFilterDto)?.Distinct()?.OrderBy(y => y.Value)?.ToList();

            return new ResultDto
            {
                Info = ResultMessages.GetInfoSuccess,
                Data = new
                {
                    opcoWisePercentage,
                    overAllOpcoPercentage,
                    productWisePercentage,
                    subnetworkWisePercentage,
                    verticalFilterValueBasedOnFilter,
                    productImportanceFilter
                }
            };

        }

        public int GetZOpcoId()
        {
            return _repositoryWrapper.OpCo.FindByCondition(x => x.Opco.ToLower().Trim() == ConstantValueFilter.ZOpco).Select(x => x.Opcoid).FirstOrDefault();

        }

        public List<short> GetVerticalVoicCoreId()
        {
            var getVoicCoreVerticalId = _repositoryWrapper.VerticalResponsible.
                FindByCondition(x => x.Verticalresponsible.ToLower().Trim() == ConstantValueFilter.CsCoreEnablers ||
                x.Verticalresponsible.ToLower().Trim() == ConstantValueFilter.CsIms)
                .Select(x => (short)x.Verticalresponsibleid).ToList();
            return getVoicCoreVerticalId;
        }
        private ExpressionStarter<Lcmengineering> ApplyFilter(LcmAtGlanceQueryDto buildFilterDto, bool isPageLoad = false)
        {
            var predicateResult = PredicateBuilder.New<Lcmengineering>(true);
            var predicateInner = PredicateBuilder.New<Lcmengineering>(true);

            var zOpcoId = GetZOpcoId();


            predicateInner.Or(x => x.Archived == false && x.Opco.Opcoid != zOpcoId);
            predicateResult.And(predicateInner);


            if (buildFilterDto.OpcoId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ProductId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.ProductId)
                    predicateInner.Or(x => x.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SupportedServicesId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.SupportedServicesId)
                    predicateInner.Or(x => x.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Any(m => m.Serviceid == item));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VerticalResponsibleId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.VerticalResponsibleId)
                    predicateInner.Or(x => x.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                    .Any(m => m.Organisation.Vertical.Verticalresponsibleid == item && m.Deleted == false)));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.ProductimportanceId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Lcmengineering>();
                foreach (var item in buildFilterDto.ProductimportanceId)
                    predicateInner.Or(x => x.Productimportanceid == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private ExpressionStarter<Plannedactivities> ApplyPlannedActivityFilter(LcmAtGlanceQueryDto buildFilterDto, Dictionary<int, string> constantFilterPlannedActivities)
        {
            var predicateResult = PredicateBuilder.New<Plannedactivities>(true);
            var predicateInner = PredicateBuilder.New<Plannedactivities>(true);

            var zOpcoId = GetZOpcoId();

            predicateInner.Or(x => x.Archived == false);
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Plannedactivities>();
            predicateInner.Or(x => x.Lcmengineering.Archived == false);
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Plannedactivities>();
            predicateInner.Or(x => x.Lcmengineeringid != null);
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Plannedactivities>();
            predicateInner.Or(x => x.Plannedactivityresource.RulelinkeddcNavigation.Plannedactivitytypedescription.ToLower().Replace(" ", "") != ConstantValueFilter.RolloutComplete);
            predicateResult.And(predicateInner);

            predicateInner = PredicateBuilder.New<Plannedactivities>();
            predicateInner.Or(x => x.Opco.Opcoid != zOpcoId);
            predicateResult.And(predicateInner);

            if (buildFilterDto.OpcoId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.OpcoId)
                    predicateInner.Or(x => x.Lcmengineering.Opcoid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.PlannedActivityResourceRuleId?.Any() == true)
            {

                /* constantFilterPlannedActivities = constantFilterPlannedActivities
                    .Where(x => buildFilterDto.PlannedActivityResourceRuleId.Any(y => y == x.Key)).ToDictionary(m => m.Key, m => m.Value);*/


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
                    predicateInner.Or(x => x.Lcmengineering.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid == item);
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.SupportedServicesId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.SupportedServicesId)
                    predicateInner.Or(x => x.Lcmengineering.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr.Any(m => m.Serviceid == item));
                predicateResult.And(predicateInner);
            }

            if (buildFilterDto.VerticalResponsibleId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.VerticalResponsibleId)
                    predicateInner.Or(x => x.Lcmengineering.Lcmengineeringsubdomainspoc.Any(d => d.Subdomainspoc.AspnetuserverticalsUser
                    .Any(m => m.Organisation.Vertical.Verticalresponsibleid == item && m.Deleted == false)));
                predicateResult.And(predicateInner);
            }


            if (buildFilterDto.ProductimportanceId?.Any() == true)
            {
                predicateInner = PredicateBuilder.New<Plannedactivities>();
                foreach (var item in buildFilterDto.ProductimportanceId)
                    predicateInner.Or(x => x.Lcmengineering.Productimportanceid == item);
                predicateResult.And(predicateInner);
            }
            return predicateResult;
        }

        private static ExpressionStarter<Settingsupdateplannedactivity> ApplySettingsupdateplannedactivityFilter(LcmAtGlanceQueryDto buildFilterDto)
        {
            var settingPAPredicateResult = PredicateBuilder.New<Settingsupdateplannedactivity>(true);
            var settingPAPredicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>(true);
 
            if (buildFilterDto.PlannedActivityResourceRuleId?.Any() == true)
            {
                
                settingPAPredicateInner = PredicateBuilder.New<Settingsupdateplannedactivity>();
                foreach (var item in buildFilterDto.PlannedActivityResourceRuleId)
                    settingPAPredicateInner.Or(x => x.Plannedactivityresource.Rulelinkeddc == item);
                settingPAPredicateResult.And(settingPAPredicateInner);
            }
 

            return settingPAPredicateResult;
        }


        public async Task<LcmAtGlanceGridDto>  GetOverAllOpcoAndAssetWisePercentage(  IEnumerable<LcmAtGlanceGridDto> lcmGlanceEntity, int overAllCount, int overAllNodeCount)
        {
            if (overAllCount == 0)
                return new LcmAtGlanceGridDto();

            // Precompute counts and sums
            var colorCounts2026 = await Task.Run(() => lcmGlanceEntity
                .GroupBy(e => e.compatibilityColorCode2026)
                .ToDictionary(g => g.Key, g => new
                {
                    Count = g.Count(),
                    NodeCount = (short?)g.Sum(e => e.NodesCount)
                }));

            var colorCounts2027 = await Task.Run(() => lcmGlanceEntity
                .GroupBy(e => e.compatibilityColorCode2027)
                .ToDictionary(g => g.Key, g => new
                {
                    Count = g.Count(),
                    NodeCount = (short?)g.Sum(e => e.NodesCount)
                }));

            var colorCounts2028 = await Task.Run(() => lcmGlanceEntity
                .GroupBy(e => e.compatibilityColorCode2028)
                .ToDictionary(g => g.Key, g => new
                {
                    Count = g.Count(),
                    NodeCount = (short?)g.Sum(e => e.NodesCount)
                }));

            // Precompute counts and sums
            var colorCounts = await Task.Run(() => lcmGlanceEntity
                .GroupBy(e => e.compatibilityColorCode)
                .ToDictionary(g => g.Key, g => new
                {
                    Count = g.Count(),
                    NodeCount = (short?)g.Sum(e => e.NodesCount)
                }));

            // Helper function to get counts safely
            int GetCount(string colorCode) => colorCounts.TryGetValue(colorCode, out var data) ? data.Count : 0;

            // Helper function to get counts safely
            int GetCount2026(string colorCode) => colorCounts2026.TryGetValue(colorCode, out var data) ? data.Count : 0;
            short? GetNodeCount2026(string colorCode) => colorCounts2026.TryGetValue(colorCode, out var data) ? data.NodeCount : 0;

            // Helper function to get counts safely
            int GetCount2027(string colorCode) => colorCounts2027.TryGetValue(colorCode, out var data) ? data.Count : 0;
            short? GetNodeCount2027(string colorCode) => colorCounts2027.TryGetValue(colorCode, out var data) ? data.NodeCount : 0;

            // Helper function to get counts safely
            int GetCount2028(string colorCode) => colorCounts2028.TryGetValue(colorCode, out var data) ? data.Count : 0;
            short? GetNodeCount2028(string colorCode) => colorCounts2028.TryGetValue(colorCode, out var data) ? data.NodeCount : 0;

            // Calculate percentages
            string CalculatePercentage(int count, int total) =>
                total > 0 ? ((decimal)count / total * 100).ToString("0.#") : "0";

            string CalculateNodePercentage(short? count, int total) =>
                total > 0 && count.HasValue ? ((decimal)count.Value / total * 100).ToString("0.#") : "0";

            return new LcmAtGlanceGridDto
            {
                //LCM Compatibility percentages
                greenCompatibilityPercentage = CalculatePercentage(GetCount(ConstantValueFilter.Green), overAllCount),
                amberCompatibilityPercentage = CalculatePercentage(GetCount(ConstantValueFilter.Amber), overAllCount),
                redCompatibilityPercentage = CalculatePercentage(GetCount(ConstantValueFilter.Red), overAllCount),
                TotalPercentage = CalculatePercentage(
                    GetCount(ConstantValueFilter.Green) +
                    GetCount(ConstantValueFilter.Amber) +
                    GetCount(ConstantValueFilter.Red),
                    overAllCount),


                TotalPercentage2026 = CalculatePercentage(
                    GetCount2026(ConstantValueFilter.Green) +
                    GetCount2026(ConstantValueFilter.Red),
                    overAllCount),
                TotalPercentage2027 = CalculatePercentage(
                    GetCount2027(ConstantValueFilter.Green) +
                    GetCount2027(ConstantValueFilter.Red),
                    overAllCount),
                TotalPercentage2028 = CalculatePercentage(
                    GetCount2028(ConstantValueFilter.Green) +
                    GetCount2028(ConstantValueFilter.Red),
                    overAllCount),

                NodesCount = (long)lcmGlanceEntity.Sum(e => e.NodesCount),

                ColorCompatability = new List<ColorCompatability>
                {
                    new ColorCompatability
                    {
                        year = EndOfSupportDate.Year.ToString(),
                        greenNodePercentage = CalculateNodePercentage(GetNodeCount2026(ConstantValueFilter.Green), overAllNodeCount),
                        redNodePercentage = CalculateNodePercentage(GetNodeCount2026(ConstantValueFilter.Red), overAllNodeCount),
                    },
                    new ColorCompatability
                    {
                        year = EndOfSupportDate.AddYears(1).Year.ToString(),
                        greenNodePercentage = CalculateNodePercentage(GetNodeCount2027(ConstantValueFilter.Green), overAllNodeCount),
                        redNodePercentage = CalculateNodePercentage(GetNodeCount2027(ConstantValueFilter.Red), overAllNodeCount),
                    },
                    new ColorCompatability
                    {
                        year = EndOfSupportDate.AddYears(2).Year.ToString(),
                        greenNodePercentage = CalculateNodePercentage(GetNodeCount2028(ConstantValueFilter.Green), overAllNodeCount),
                        redNodePercentage = CalculateNodePercentage(GetNodeCount2028(ConstantValueFilter.Red), overAllNodeCount),
                    }
                }
                


            };
        }

        public async Task<LcmAtGlanceGridDto> GetOverAllOpcoAndAssetWisePercentageForEofs(List<Lcmengineering> lcmEntity, DateTime? selectedDate)
        {
  
            if (lcmEntity == null)
            {
                return null;
            }
            var lcmGlanceEntity = await Task.WhenAll(lcmEntity.Where(m => m.Lcmengineeringsubdomainspoc.Any()).Select(async y => new LcmAtGlanceGridDto()
            {
                LcmengineeringId = (long)y.Lcmengineeringid,
                OpcoId = (long)y.Opcoid,
                OpCoDescrption = y.Opco.Opco,
                NodesCount = y.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result,
                compatibilityColorCode2026 = y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport != null ?  
                GetEoslKpiFrozen(y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport
                ,2026
                ,y.PlannedactivitiesLcmengineering.Select(x => x.Plannedcompletion).FirstOrDefault()
                ,GetAssetStatus(y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport,2026)
                ,y.Softwareendofwarrantydate != null ? y.Softwareendofwarrantydate : y.Hardwareendofsupportcontract,true) 
                : y.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced ? ConstantValueFilter.Green : ConstantValueFilter.Red,

                compatibilityColorCode2027 = y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport != null ? 
                GetEoslKpiForeCast(y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport
                , 2027
                , y.PlannedactivitiesLcmengineering.Select(x => x.Plannedcompletion).FirstOrDefault()
                , GetAssetStatus(y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport, 2027)
                , y.Softwareendofwarrantydate != null ? y.Softwareendofwarrantydate : y.Hardwareendofsupportcontract, true) 
                : y.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced ? ConstantValueFilter.Green : ConstantValueFilter.Red,

                compatibilityColorCode2028 = y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport != null ? 
                GetEoslKpiTarget(y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport
                , 2028
                , y.PlannedactivitiesLcmengineering.Select(x => x.Plannedcompletion).FirstOrDefault()
                , GetAssetStatus(y.Designcomponent.Systemtype.Majorsoftwarebuilds.Endofsupport, 2028)
                , y.Softwareendofwarrantydate != null ? y.Softwareendofwarrantydate : y.Hardwareendofsupportcontract, true) 
                : y.Designcomponent.Systemtype.Majorsoftwarebuilds.Eomstatus == (short)EOMEnum.NotAnnounced ? ConstantValueFilter.Green : ConstantValueFilter.Red,

                compatibilityColorCode = calculateRagStatusFromSoftware(y,selectedDate),
                ProductName = y.Designcomponent.Systemtype.Majorsoftwarebuilds.Productname.Description,
                ProductId = (long)y.Designcomponent.Systemtype.Majorsoftwarebuilds.Productnameid,
                VerticalFilterDto = _commonManager.GetVerticaleFilterDto(y.Lcmengineeringsubdomainspoc.Select(x => x.Subdomainspocid).ToList(), y.Opcoid, false, true)
                .Distinct().ToList(),
                ProductImportantFilterDto =
                new FilterValueDto()
                {
                    Value = y?.Productimportanceid?.ToString(),
                    Text = y?.Productimportance?.Productimportance
                }

            }));

            int overAllCount = lcmGlanceEntity.Count();
            int overAllNodeCount = (int)lcmGlanceEntity.Sum(s => s.NodesCount);

            if (overAllCount == 0)
                return new LcmAtGlanceGridDto();

            return await GetOverAllOpcoAndAssetWisePercentage(lcmGlanceEntity, overAllCount, overAllNodeCount);
        }


        public async Task<IEnumerable<LcmAtGlanceGridDto>> GetOpcoWisePercentage(IEnumerable<LcmAtGlanceGridDto> lcmGlanceEntity, int overAllCount, int overAllNodeCount)
        {
            var overAllOpcoPercentage = await Task.Run(() => lcmGlanceEntity
      .AsParallel() // Use parallel processing
      .GroupBy(x => new { x.OpcoId })
      .Select(g =>
      {
          // Pre-compute totals
          var totalCount = g.Count();

          // Pre-compute grouped data for color codes
          var colorGroups = g
              .GroupBy(e => e.compatibilityColorCode)
              .Select(grp => new
              {
                  ColorCode = grp.Key,
                  Count = grp.Count(),
                  NodeSum = grp.Sum(e => e.NodesCount)
              })
              .ToList();

          // Get counts and sums for each color code
          var totalGreenCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.Count ?? 0;
          var totalRedCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.Count ?? 0;

          var greenNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.NodeSum ?? 0;
          var redNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.NodeSum ?? 0;

          return new LcmAtGlanceGridDto()
          {
              OpcoId = g.Key.OpcoId,
              OpCoDescrption = g.FirstOrDefault()?.OpCoDescrption,
              VerticalResponsibleName = g.FirstOrDefault()?.VerticalResponsibleName,

              // Percentage calculations
              greenCompatibilityPercentage = totalCount > 0 ? ((decimal)totalGreenCount / totalCount * 100).ToString("0.#") : "0",
              redCompatibilityPercentage = totalCount > 0 ? ((decimal)totalRedCount / totalCount * 100).ToString("0.#") : "0",
              TotalPercentage = totalCount > 0 ? ((decimal)(totalGreenCount  + totalRedCount) / totalCount * 100).ToString("0.#") : "0",

              // Node percentages
              greenNodePercentage = overAllNodeCount > 0 ? ((decimal)greenNodeSum / overAllNodeCount * 100).ToString("0.#") : "0",
              redNodePercentage = overAllNodeCount > 0 ? ((decimal)redNodeSum / overAllNodeCount * 100).ToString("0.#") : "0",

              //Node Count 

              greenNodeCount = (short)greenNodeSum,
              redNodeCount = (short)redNodeSum,
              NodesCount = overAllNodeCount,
          };
      })
      .OrderBy(x => x.OpCoDescrption)
      .ToList()); // Execute the query


            return overAllOpcoPercentage;
        }
        public async Task<IEnumerable<LcmAtGlanceGridDto>> GetProductwisePercentage(IEnumerable<LcmAtGlanceGridDto> lcmGlanceEntity, int overAllCount, int overAllNodeCount)
        {

            var productWisePercentage = await Task.Run(() =>  lcmGlanceEntity.GroupBy(x => new { x.ProductId }).AsParallel()
                .Select(g =>
                {
                    var totalCount = g.Count();
                    var colorGroups = g
               .GroupBy(e => e.compatibilityColorCode)
               .Select(grp => new
               {
                   ColorCode = grp.Key,
                   Count = grp.Count(),
                   NodeSum = grp.Sum(e => e.NodesCount)
               })
               .ToList();

                    // Get counts and sums for each color code
                    var totalGreenCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.Count ?? 0;
                    var totalRedCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.Count ?? 0;

                    var greenNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.NodeSum ?? 0;
                    var redNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.NodeSum ?? 0;

                    return new LcmAtGlanceGridDto()
                    {
                        ProductId = g.Key.ProductId,
                        ProductName = g?.FirstOrDefault()?.ProductName,

                        // Percentage calculations
                        greenCompatibilityPercentage = totalCount > 0 ? ((decimal)totalGreenCount / totalCount * 100).ToString("0.#") : "0",
                        redCompatibilityPercentage = totalCount > 0 ? ((decimal)totalRedCount / totalCount * 100).ToString("0.#") : "0",
                        TotalPercentage = totalCount > 0 ? ((decimal)(totalGreenCount +  totalRedCount) / totalCount * 100).ToString("0.#") : "0",

                        // Node percentages
                        greenNodePercentage = overAllNodeCount > 0 ? ((decimal)greenNodeSum / overAllNodeCount * 100).ToString("0.#") : "0",
                        redNodePercentage = overAllNodeCount > 0 ? ((decimal)redNodeSum / overAllNodeCount * 100).ToString("0.#") : "0",

                    };
                }

                )?.OrderBy(x => x.ProductName));//?.ToList();

            return productWisePercentage;
        }

        public async Task<IEnumerable<LcmAtGlanceGridDto>>  GetSupportedServiceWisePercentage(IEnumerable<Lcmengineering> lcmBaseQueryResult, LcmAtGlanceQueryDto buildFilterDto)
        {

            var subnetworkWisePercentage = await Task.Run(() => lcmBaseQueryResult
     .AsParallel()
     .SelectMany(x => x.Designcomponent.Subnetworkboundary.Subnetworksupportedsvr
         .Where(y => buildFilterDto.SupportedServicesId == null || !buildFilterDto.SupportedServicesId.Any() ||
         buildFilterDto.SupportedServicesId.Contains((short)y.Serviceid))
         .Select(y => new
         {
             compatibilityColorCode = calculateRagStatusFromSoftware(x, buildFilterDto.SelectedDate),
             SupportedServicesId = (long)y.Serviceid,
             SupportedServicesDescription = y.Service.Description
         }))
     .GroupBy(x => x.SupportedServicesId)
     .Select(g =>
     {
         var totalCount = g.Count();
         var colorGroups = g
             .GroupBy(e => e.compatibilityColorCode)
             .Select(grp => new
             {
                 ColorCode = grp.Key,
                 Count = grp.Count()
             })
             .ToList();

         var totalGreenCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.Count ?? 0;
         var totalRedCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.Count ?? 0;

         return new LcmAtGlanceGridDto()
         {
             SupportedServicesId = g.Key,
             SupportedServicesDescription = g.FirstOrDefault()?.SupportedServicesDescription,

             // Percentage calculations
             greenCompatibilityPercentage = (totalCount > 0 ? ((decimal)totalGreenCount / totalCount * 100).ToString("0.#") : "0") + "%",
             redCompatibilityPercentage = (totalCount > 0 ? ((decimal)totalRedCount / totalCount * 100).ToString("0.#") : "0") + "%",
         };
     })
     .OrderBy(x => x.SupportedServicesDescription)
     .ToList()); // Execute the query

            return subnetworkWisePercentage;
        }

        public async Task<ResultDto> GetPADropdown(LcmAtGlanceQueryDto buildFilterDto)
        {

            var settingPAPredicateResult = ApplySettingsupdateplannedactivityFilter(buildFilterDto);

            var deliveryStatus = await Task.Run(() =>  _dropdownDataServiceManager.GetSettingPlannedActivityBasedDeliveryStatusForPA(settingPAPredicateResult)?.Result?.
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

        public string calculateRagStatusFromSoftware(Lcmengineering entity, DateTime? SelectedDate)
        {


            var endOfSupport = entity?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofsupport;

            if (SelectedDate != null)
                currentDate = (DateTime)SelectedDate;

            var ragStatus =
                endOfSupport == null || endOfSupport >= currentDate ? ConstantValueFilter.Green : endOfSupport <= currentDate ? ConstantValueFilter.Red : ConstantValueFilter.Red;

            return ragStatus;

        }


        public string RagStatusForAssetYearwise(Lcmengineering entity, DateTime? SelectedDate,int FinancialYear)
        {
            var Eofsdate = entity?.Designcomponent?.Systemtype?.Majorsoftwarebuilds?.Endofsupport;
            if (Eofsdate == null)
            {
                return ConstantValueFilter.Red;
            }
            var color = string.Empty;
            DateTime date = new DateTime(FinancialYear + 1, 3, 31);
            DateTime previousYear = new DateTime(FinancialYear, 3, 31);

            if (Eofsdate > date)
            {
                return ConstantValueFilter.Green;
            }
            else if ((previousYear < Eofsdate) && (Eofsdate <= date))
            {
                return ConstantValueFilter.Green;
            }
            else if (Eofsdate <= previousYear)
            {
                return ConstantValueFilter.Red;
            }
            else
            {
                return ConstantValueFilter.Red;
            }
        }
        public string toOriginalDesignComponentFamilyDescription(Plannedactivities entity)
        {

            string designComponentFamily = string.Empty;
            if (entity.Lcmengineeringid != null)
            {
                designComponentFamily = entity?.Lcmengineering?.Designcomponent?.Designcomponentfamily?.DCFName(_repositoryWrapper);
            }

            else if (entity.Networkelementasplannedid != null)
            {
                designComponentFamily = (entity?.Networkelementasplanned?.Designcomponent?.Designcomponentfamily?.Deleted == true) ?
                     string.Empty : entity?.Networkelementasplanned?.Designcomponent?.Designcomponentfamily?.DCFName(_repositoryWrapper);
            }
            else if (entity.Designaspectid != null)
            {
                designComponentFamily = (entity?.Designaspect?.Designcomponentfamily?.Deleted == true) ? string.Empty :
                    entity?.Designaspect?.Designcomponentfamily?.DCFName(_repositoryWrapper);
            }
            else
                designComponentFamily = entity?.Designcomponent?.Designcomponentfamily?.DCFName(_repositoryWrapper);

            return designComponentFamily;
        }

        public async Task<List<Plannedactivities>> GetLCMPlannedActivity(ExpressionStarter<Plannedactivities> predicateResult)
        {

            var query = _repositoryWrapper.PlannedActivity.FindByCondition(predicateResult)
                   .Include(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                     .Include(x => x.Designcomponent).ThenInclude(x => x.Systemtype).ThenInclude(x => x.Majorsoftwarebuilds)//.ThenInclude(x => x.Orgeqpmanufacturer)
                   .Include(x => x.Activitystatus)
                   .Include(x => x.Planningactivitystatus)
                   .Include(x => x.Deliverystatus)
                   .Include(x => x.ProgramNavigation)
                   .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(x => x.Designcomponentfamily)
                   .Include(x => x.Lcmengineering).ThenInclude(x => x.Designcomponent).ThenInclude(p => p.Systemtype).ThenInclude(p => p.Majorsoftwarebuilds).ThenInclude(p => p.Productname)
                   .Include(x => x.Plannedactivityresource).ThenInclude(x => x.RulelinkeddcNavigation)
                    .Include(x => x.Lcmengineering).ThenInclude(x => x.Opco)
                    .Include(x => x.Opco).ToListAsync();

            return await query;
        }

        public async Task<List<LcmAtGlanceGridDto>> GetNoPaLcmReords(LcmAtGlanceQueryDto buildFilterDto)
        {
            var noPaPredicateResult = PredicateBuilder.New<Lcmengineering>(true);
            noPaPredicateResult = ApplyFilter(buildFilterDto);
            var noPaPredicateInner = PredicateBuilder.New<Lcmengineering>(true);
            noPaPredicateInner.Or(x => x.PlannedactivitiesLcmengineering == null);
            noPaPredicateResult.And(noPaPredicateInner);
            var lcmBaseQueryResult = await GetQuery(noPaPredicateResult);

            var noPaLcmEntity = lcmBaseQueryResult.Select(
     m => new
     {
         NodesCount = m.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result,
         compatibilityColorCode = calculateRagStatusFromSoftware(m, buildFilterDto.SelectedDate),
         currentDc = m?.Designcomponent?.toDesignComponentNameLcm(_repositoryWrapper),  // lcm - dc
         dcid = m.Designcomponentid,

     });
            int overAllNodeCount = (int)noPaLcmEntity.Sum(s => s.NodesCount);

            var groupedNoPaLcmEntity = noPaLcmEntity.GroupBy(x => new { x.dcid }).AsParallel()
                           .Select(g =>
                           {
                               var totalCount = g.Count();
                               var colorGroups = g
                          .GroupBy(e => e.compatibilityColorCode)
                          .Select(grp => new
                          {
                              ColorCode = grp.Key,
                              Count = grp.Count(),
                              NodeSum = grp.Sum(e => e.NodesCount)
                          })
                          .ToList();

                               // Get counts and sums for each color code
                               var totalGreenCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.Count ?? 0;
                               var totalAmberCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Amber)?.Count ?? 0;
                               var totalRedCount = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.Count ?? 0;

                               var greenNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Green)?.NodeSum ?? 0;
                               var amberNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Amber)?.NodeSum ?? 0;
                               var redNodeSum = colorGroups.FirstOrDefault(c => c.ColorCode == ConstantValueFilter.Red)?.NodeSum ?? 0;

                               return new LcmAtGlanceGridDto()
                               {
                                   CurrentDcId = g.Key.dcid,
                                   CurrentDc = g?.FirstOrDefault()?.currentDc,

                                   greenNodeCount = (short?)greenNodeSum,
                                   redNodeCount = (short?)redNodeSum,
                                   amberNodeCount = (short?)amberNodeSum,

                                   //// Percentage calculations
                                   //greenCompatibilityPercentage = totalCount > 0 ? ((decimal)totalGreenCount / totalCount * 100).ToString("0.##") : "0",
                                   //amberCompatibilityPercentage = totalCount > 0 ? ((decimal)totalAmberCount / totalCount * 100).ToString("0.##") : "0",
                                   //redCompatibilityPercentage = totalCount > 0 ? ((decimal)totalRedCount / totalCount * 100).ToString("0.##") : "0",
                                   //TotalPercentage = totalCount > 0 ? ((decimal)(totalGreenCount + totalAmberCount + totalRedCount) / totalCount * 100).ToString("0.##") : "0",

                                   //// Node percentages
                                   //greenNodePercentage = overAllNodeCount > 0 ? ((decimal)greenNodeSum / overAllNodeCount * 100).ToString("0.##") : "0",
                                   //redNodePercentage = overAllNodeCount > 0 ? ((decimal)redNodeSum / overAllNodeCount * 100).ToString("0.##") : "0",
                                   //amberNodePercentage = overAllNodeCount > 0 ? ((decimal)amberNodeSum / overAllNodeCount * 100).ToString("0.##") : "0",

                               };
                           }

                           )?.OrderBy(x => x.CurrentDc).ToList();

            return groupedNoPaLcmEntity;

        }

        public async Task<List<PlannedActivityAtGlanceGridDto>> GetLcmWithOutPaRecords(LcmAtGlanceQueryDto buildFilterDto)
        {
            var noPaPredicateResult = ApplyFilter(buildFilterDto);
            var noPaPredicateInner = PredicateBuilder.New<Lcmengineering>(true);
            noPaPredicateInner.Or(x => x.PlannedactivitiesLcmengineering.Any() == false);
            noPaPredicateResult.And(noPaPredicateInner);
            var lcmBaseQueryResult = await GetQuery(noPaPredicateResult);

            var noPaLcmEntity = lcmBaseQueryResult.Select(
       m =>
     {
         var NodesCount =   m.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result;
         string compatibilityColorCode = calculateRagStatusFromSoftware(m, buildFilterDto.SelectedDate);
         var currentDc = m?.Designcomponent?.toDesignComponentNameLcm(_repositoryWrapper);  // lcm - dc
         var dcid = m.Designcomponentid;
         var opCoId = m.Opcoid;
         string opCoName = m.Opco.Opco;

         // Get counts and sums for each color code
         int totalGreenCount = compatibilityColorCode == ConstantValueFilter.Green ? NodesCount : 0;
         int totalAmberCount = compatibilityColorCode == ConstantValueFilter.Amber ? NodesCount : 0;
         int totalRedCount = compatibilityColorCode == ConstantValueFilter.Red ? NodesCount : 0;

         return new PlannedActivityAtGlanceGridDto
         {
             OpCoDescrption = opCoName,
             OpcoId = (long)opCoId,
             NodesCount = NodesCount,
             greenNodeCount = totalGreenCount,
             redNodeCount = totalRedCount,
             amberNodeCount = totalAmberCount,
             PACurrentDCName = currentDc,
             PAPlannedDCName = "undefined",
             compatibilityColorCode = compatibilityColorCode

         };
     }).OrderBy(x => x.OpCoDescrption).ThenBy(y => y.PACurrentDCName).ToList();

            return noPaLcmEntity;

        }

        public long GetPAPlannedDCNodesCount(long PADesignComponentId, short opcoId)
        {

            var plannedDCNodesCount = 0;
            var plannedDCLCMExist = _repositoryWrapper.Lcmengineering.FindByCondition(x => x.Opcoid == opcoId && x.Designcomponentid == PADesignComponentId).FirstOrDefault();
            if (plannedDCLCMExist != null)
            {
                plannedDCNodesCount = plannedDCLCMExist.CountAggregatedBasedNetworkElementReleated(false, _repositoryWrapper).Result;
            }

            return plannedDCNodesCount;

        }

    }
}