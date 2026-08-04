using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;

namespace CAM.Entities.Models
{
    public partial class BudgetProjectTrackers : AuditableEntity
    {
        public long BudgetProjectTackerId { get; set; }
        public string BudgetLineCode { get; set; }
        public string UploadStatus { get; set; }
        public string UploadMode { get; set; }
        public string CurrentTrackingNumber { get; set; }
        public string NewTrackingNumber { get; set; }
        public string Wbs { get; set; }
        public short? OpcoId { get; set; }
        public string Opco { get; set; }
        public string Domain { get; set; }
        public string Team { get; set; }
        public short? TeamId { get; set; }
        public string BudgetOwner { get; set; }
        public short? BudgetOwnerid { get; set; }
        public string Program { get; set; }
        public string BudgetProject { get; set; }
        public string Activity { get; set; }
        public string Priority { get; set; }
        public string Driver { get; set; }
        public string Benefits { get; set; }
        public string Risks { get; set; }
        public string Category { get; set; }
        public short? CategoryId { get; set; }
        public string Nwelement { get; set; }
        public string VirtualizedNwElement { get; set; }
        public string Vendor { get; set; }
        public short? LcmCategoriesId { get; set; }
        public string LcmCategories { get; set; }
        public string OhpLev1 { get; set; }
        public string OhpLev2 { get; set; }
        public string HfmLev1 { get; set; }
        public string HfmLev2 { get; set; }
        public string Fy { get; set; }
        public string Operational { get; set; }
        public string Transfers { get; set; }
        public string Cost1sTest { get; set; }
        public string Validation { get; set; }
        public string SignOff { get; set; }
        public string Sub { get; set; }
        public string FinalReSub { get; set; }
        public string LatestScenario { get; set; }
        public string Currency { get; set; }
        public string Adjustments { get; set; }
        public string YtdActuals { get; set; }
        public string PlannedAbsorption { get; set; }
        public string Deviation { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string ApprovedBudget { get; set; }
        public string Commitment { get; set; }
        public string BudgetProjectDependency { get; set; }
        public string LocalProgram { get; set; }
        public string LocalBudgetProject { get; set; }
        public string LocalDriver { get; set; }
        public string LocalPrioritization { get; set; }
        public string PpmId { get; set; }
        public string PpmBudgetProjectId { get; set; }
        public string CostCentre { get; set; }
        public string WpId { get; set; }
        public string GroupBudgetOpcoId { get; set; }
        public string InternalProgram { get; set; }
        public string VerticalProject { get; set; }
        public string DomainSpecificLabels { get; set; }
        public string LabelsMarketVsVertical { get; set; }
        public string OtherMinorVendors { get; set; }
        public string ExternalDemandBudget { get; set; }
        public string OpexImpact { get; set; }
        public string LegalEntity { get; set; }
        public string YearlyTransfersTrack { get; set; }
        public string YearlyAdjustmentsTrack { get; set; }
        public string Mcustom02 { get; set; }
        public string Mcustom03 { get; set; }
        public string Mcustom04 { get; set; }
        public string Mcustom05 { get; set; }
        public string Mcustom06 { get; set; }
        public string Mcustom07 { get; set; }
        public string Mcustom08 { get; set; }
        public string Mcustom09 { get; set; }
        public string Mcustom10 { get; set; }
        public string Vcustom01 { get; set; }
        public string Vcustom02 { get; set; }
        public string Vcustom03 { get; set; }
        public string Vcustom04 { get; set; }
        public string Vcustom05 { get; set; }
        public string Dcustom01 { get; set; }
        public string Dcustom02 { get; set; }
        public string Dcustom03 { get; set; }
        public string Dcustom04 { get; set; }
        public string Dcustom05 { get; set; }
        public string Lsdb { get; set; }
        public string Ls012 { get; set; }
        public string Ls210 { get; set; }
        public string Ls57 { get; set; }
        public bool? Archive { get; set; }
        public long? PlannedActivityId { get; set; }

        public virtual PlannedActivityCategory CategoryNavigation { get; set; }
        public virtual OpCo OpcoNavigation { get; set; }
        public virtual PlannedActivity PlannedActivity { get; set; }
        public long? LcmEngineeringId { get; set; }
        public List<int?> LcmEngineeringSubdomainSpoc { get; set; }
        public long? NetworkElementAsPlannedId { get; set; }
        public List<int?> NetworkElementAsPlannedSubdomainSpoc { get; set; }
        public long? DesignAspectId { get; set; }
        public long? Serviceplanid { get; set; }
        public List<int?> DesignContactDto { get; set; }
        public List<FilterValueDtoKeyValueList> VerticalFilterDto { get; set; }
    }
}
