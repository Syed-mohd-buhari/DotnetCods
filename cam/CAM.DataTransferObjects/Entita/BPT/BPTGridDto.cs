using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.BPT
{
    public class BPTGridDto : GridDtoBase
    {
        [IgnoreGrid]
        public long BudgetProjectTackerId { get; set; }
        [OrderGrid(Order = 1)]
        [Default]
        [DisplayName("Budget_Line Code")]
        public string BudgetLineCode { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("Upload Status")]
        public string UploadStatus { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("Upload Mode")]
        public string UploadMode { get; set; }
        [OrderGrid(Order = 4)]
        [Default]
        [DisplayName("Current Tracking Number")]
        public string CurrentTrackingNumber { get; set; }
        [OrderGrid(Order = 5)]
        [Default]
        [DisplayName("New Tracking Number")]
        public string NewTrackingNumber { get; set; }
        [OrderGrid(Order = 6)]
        [Default]
        [DisplayName("WBS")]
        public string Wbs { get; set; }
        [IgnoreGrid]
        public short? OpcoId { get; set; }
        [OrderGrid(Order = 7)]
        [Default]
        [DisplayName("OpCo")]
        public string Opco { get; set; }
        [OrderGrid(Order = 8)]
        [Default]
        [DisplayName("Domain")]
        public string Domain { get; set; }
        [OrderGrid(Order = 9)]
        [Default]
        [DisplayName("Team")]
        public string Team { get; set; }
        [IgnoreGrid]
        public short? TeamId { get; set; }
        [OrderGrid(Order = 10)]
        [Default]
        [DisplayName("Budget Owner")]
        public string BudgetOwner { get; set; }
        [IgnoreGrid]
        public short? BudgetOwnerid { get; set; }
        [OrderGrid(Order = 11)]
        [Default]
        [DisplayName("Program")]
        public string Program { get; set; }
        [OrderGrid(Order = 12)]
        [Default]
        [DisplayName("Budget Project")]
        public string BudgetProject { get; set; }
        [OrderGrid(Order = 13)]
        [Default]
        [DisplayName("Activity")]
        public string Activity { get; set; }
        [OrderGrid(Order = 14)]
        [Default]
        [DisplayName("Priority")]
        public string Priority { get; set; }
        [OrderGrid(Order = 15)]
        [Default]
        [DisplayName("Driver")]
        public string Driver { get; set; }
        [OrderGrid(Order = 16)]
        [Default]
        [DisplayName("Benefits")]
        public string Benefits { get; set; }
        [OrderGrid(Order = 17)]
        [Default]
        [DisplayName("Risks")]
        public string Risks { get; set; }
        [OrderGrid(Order = 18)]
        [Default]
        [DisplayName("Category")]
        public string Category { get; set; }
        [IgnoreGrid]
        public short? CategoryId { get; set; }
        [OrderGrid(Order = 19)]
        [Default]
        [DisplayName("NW Element")]
        public string Nwelement { get; set; }
        [OrderGrid(Order = 20)]
        [Default]
        [DisplayName("Virtualized NW Element")]
        public string VirtualizedNwElement { get; set; }
        [OrderGrid(Order = 21)]
        [Default]
        [DisplayName("Vendor")]
        public string Vendor { get; set; }
        [IgnoreGrid]
        public short? LcmCategoriesId { get; set; }
        [OrderGrid(Order = 22)]
        [Default]
        [DisplayName("LCM Categories")]
        public string LcmCategories { get; set; }
        [OrderGrid(Order = 23)]
        [Default]
        [DisplayName("OHP Lev1")]
        public string OhpLev1 { get; set; }
        [OrderGrid(Order = 24)]
        [Default]
        [DisplayName("OHP Lev2")]
        public string OhpLev2 { get; set; }
        [OrderGrid(Order = 25)]
        [Default]
        [DisplayName("HFM Lev1")]
        public string HfmLev1 { get; set; }
        [OrderGrid(Order = 26)]
        [Default]
        [DisplayName("HFM Lev2")]
        public string HfmLev2 { get; set; }
        [OrderGrid(Order = 27)]
        [Default]
        [DisplayName("FY")]
        public string Fy { get; set; }
        [OrderGrid(Order = 28)]
        [Default]
        [DisplayName("OPERATIONAL")]
        public string Operational { get; set; }
        [OrderGrid(Order = 29)]
        [Default]
        [DisplayName("Transfers")]
        public string Transfers { get; set; }
        [OrderGrid(Order = 30)]
        [Default]
        [DisplayName("Cost 1st Est")]
        public string Cost1sTest { get; set; }
        [OrderGrid(Order = 31)]
        [Default]
        [DisplayName("1st Validation")]
        public string Validation { get; set; }
        [OrderGrid(Order = 32)]
        [Default]
        [DisplayName("Sign Off")]
        public string SignOff { get; set; }
        [OrderGrid(Order = 33)]
        [Default]
        [DisplayName("1st Sub")]
        public string Sub { get; set; }
        [OrderGrid(Order = 34)]
        [Default]
        [DisplayName("Final Resub")]
        public string FinalReSub { get; set; }
        [OrderGrid(Order = 35)]
        [Default]
        [DisplayName("Latest Scenario")]
        public string LatestScenario { get; set; }
        [OrderGrid(Order = 36)]
        [Default]
        [DisplayName("C")]
        public string Currency { get; set; }
        [OrderGrid(Order = 37)]
        [Default]
        [DisplayName("Adjustments")]
        public string Adjustments { get; set; }
        [OrderGrid(Order = 38)]
        [Default]
        [DisplayName("YTD Actuals")]
        public string YtdActuals { get; set; }
        [OrderGrid(Order = 39)]
        [Default]
        [DisplayName("Planned Absorption")]
        public string PlannedAbsorption { get; set; }
        [OrderGrid(Order = 40)]
        [Default]
        [DisplayName("Deviation")]
        public string Deviation { get; set; }
        [OrderGrid(Order = 41)]
        [Default]
        [DisplayName("Apr")]
        public string Apr { get; set; }
        [OrderGrid(Order = 42)]
        [Default]
        [DisplayName("May")]
        public string May { get; set; }
        [OrderGrid(Order = 43)]
        [Default]
        [DisplayName("Jun")]
        public string Jun { get; set; }
        [OrderGrid(Order = 44)]
        [Default]
        [DisplayName("Jul")]
        public string Jul { get; set; }
        [OrderGrid(Order = 45)]
        [Default]
        [DisplayName("Aug")]
        public string Aug { get; set; }
        [OrderGrid(Order = 46)]
        [Default]
        [DisplayName("Sep")]
        public string Sep { get; set; }
        [OrderGrid(Order = 47)]
        [Default]
        [DisplayName("Oct")]
        public string Oct { get; set; }
        [OrderGrid(Order = 48)]
        [Default]
        [DisplayName("Nov")]
        public string Nov { get; set; }
        [OrderGrid(Order = 49)]
        [Default]
        [DisplayName("Dec")]
        public string Dec { get; set; }
        [OrderGrid(Order = 50)]
        [Default]
        [DisplayName("Jan")]
        public string Jan { get; set; }
        [OrderGrid(Order = 51)]
        [Default]
        [DisplayName("Feb")]
        public string Feb { get; set; }
        [OrderGrid(Order = 52)]
        [Default]
        [DisplayName("Mar")]
        public string Mar { get; set; }
        [OrderGrid(Order = 53)]
        [Default]
        [DisplayName("Approved budget")]
        public string ApprovedBudget { get; set; }
        [OrderGrid(Order = 54)]
        [Default]
        [DisplayName("Commitment")]
        public string Commitment { get; set; }
        [OrderGrid(Order = 55)]
        [Default]
        [DisplayName("Budget Project Dependency")]
        public string BudgetProjectDependency { get; set; }
        [OrderGrid(Order = 56)]
        [Default]
        [DisplayName("Local Program")]
        public string LocalProgram { get; set; }
        [OrderGrid(Order = 57)]
        [Default]
        [DisplayName("Local Budget Project")]
        public string LocalBudgetProject { get; set; }
        [OrderGrid(Order = 58)]
        [Default]
        [DisplayName("Local Driver")]
        public string LocalDriver { get; set; }
        [OrderGrid(Order = 59)]
        [Default]
        [DisplayName("Local Prioritization")]
        public string LocalPrioritization { get; set; }
        [OrderGrid(Order = 60)]
        [Default]
        [DisplayName("ppm id")]
        public string PpmId { get; set; }
        [OrderGrid(Order = 61)]
        [Default]
        [DisplayName("ppm Budget Project Id")]
        public string PpmBudgetProjectId { get; set; }
        [OrderGrid(Order = 62)]
        [Default]
        [DisplayName("Cost Centre")]
        public string CostCentre { get; set; }
        [OrderGrid(Order = 63)]
        [Default]
        [DisplayName("WP ID")]
        public string WpId { get; set; }
        [OrderGrid(Order = 64)]
        [Default]
        [DisplayName("Group budget OpCo ID")]
        public string GroupBudgetOpcoId { get; set; }
        [OrderGrid(Order = 65)]
        [Default]
        [DisplayName("Internal program")]
        public string InternalProgram { get; set; }
        [OrderGrid(Order = 66)]
        [Default]
        [DisplayName("Vertical Project")]
        public string VerticalProject { get; set; }
        [OrderGrid(Order = 67)]
        [Default]
        [DisplayName("Domain Specific Labels")]
        public string DomainSpecificLabels { get; set; }
        [OrderGrid(Order = 68)]
        [Default]
        [DisplayName("Labels market vs vertical")]
        public string LabelsMarketVsVertical { get; set; }
        [OrderGrid(Order = 69)]
        [Default]
        [DisplayName("Other minor vendors")]
        public string OtherMinorVendors { get; set; }
        [OrderGrid(Order = 70)]
        [Default]
        [DisplayName("External Demand Budget")]
        public string ExternalDemandBudget { get; set; }
        [OrderGrid(Order = 71)]
        [Default]
        [DisplayName("Opex Impact")]
        public string OpexImpact { get; set; }
        [OrderGrid(Order = 72)]
        [Default]
        [DisplayName("Legal Entity")]
        public string LegalEntity { get; set; }
        [OrderGrid(Order = 73)]
        [Default]
        [DisplayName("Yearly Transfers Track")]
        public string YearlyTransfersTrack { get; set; }
        [OrderGrid(Order = 74)]
        [Default]
        [DisplayName("Yearly Adjustments track")]
        public string YearlyAdjustmentsTrack { get; set; }
        [OrderGrid(Order = 74)]
        [Default]
        [DisplayName("M_Custom_02")]
        public string Mcustom02 { get; set; }
        [OrderGrid(Order = 75)]
        [Default]
        [DisplayName("M_Custom_03")]
        public string Mcustom03 { get; set; }
        [OrderGrid(Order = 76)]
        [Default]
        [DisplayName("M_Custom_04")]
        public string Mcustom04 { get; set; }
        [OrderGrid(Order = 77)]
        [Default]
        [DisplayName("M_Custom_05")]
        public string Mcustom05 { get; set; }
        [OrderGrid(Order = 78)]
        [Default]
        [DisplayName("M_Custom_06")]
        public string Mcustom06 { get; set; }
        [OrderGrid(Order = 79)]
        [Default]
        [DisplayName("M_Custom_07")]
        public string Mcustom07 { get; set; }
        [OrderGrid(Order = 80)]
        [Default]
        [DisplayName("M_Custom_08")]
        public string Mcustom08 { get; set; }
        [OrderGrid(Order = 81)]
        [Default]
        [DisplayName("M_Custom_09")]
        public string Mcustom09 { get; set; }
        [OrderGrid(Order = 82)]
        [Default]
        [DisplayName("M_Custom_10")]
        public string Mcustom10 { get; set; }
        [OrderGrid(Order = 83)]
        [Default]
        [DisplayName("V_Custom_01")]
        public string Vcustom01 { get; set; }
        [OrderGrid(Order = 84)]
        [Default]
        [DisplayName("V_Custom_02")]
        public string Vcustom02 { get; set; }
        [OrderGrid(Order = 85)]
        [Default]
        [DisplayName("V_Custom_03")]
        public string Vcustom03 { get; set; }
        [OrderGrid(Order = 86)]
        [Default]
        [DisplayName("V_Custom_04")]
        public string Vcustom04 { get; set; }
        [OrderGrid(Order = 87)]
        [Default]
        [DisplayName("V_Custom_05")]
        public string Vcustom05 { get; set; }
        [OrderGrid(Order = 88)]
        [Default]
        [DisplayName("D_Custom_01")]
        public string Dcustom01 { get; set; }
        [OrderGrid(Order = 89)]
        [Default]
        [DisplayName("D_Custom_02")]
        public string Dcustom02 { get; set; }
        [OrderGrid(Order = 90)]
        [Default]
        [DisplayName("D_Custom_03")]
        public string Dcustom03 { get; set; }
        [OrderGrid(Order = 91)]
        [Default]
        [DisplayName("D_Custom_04")]
        public string Dcustom04 { get; set; }
        [OrderGrid(Order = 92)]
        [Default]
        [DisplayName("D_Custom_05")]
        public string Dcustom05 { get; set; }
        [OrderGrid(Order = 93)]
        [Default]
        [DisplayName("LS  DB")]
        public string Lsdb { get; set; }
        [OrderGrid(Order = 94)]
        [Default]
        [DisplayName("LS  0+12")]
        public string Ls012 { get; set; }
        [OrderGrid(Order = 95)]
        [Default]
        [DisplayName("LS  2+10")]
        public string Ls210 { get; set; }
        [OrderGrid(Order = 96)]
        [Default]
        [DisplayName("LS  5+7")]
        public string Ls57 { get; set; }
        [IgnoreGrid]
        public bool? Archive { get; set; }
        [IgnoreGrid]
        public long? PlannedActivityId { get; set; }

        [DisplayName("Vertical Name")]
        public string VerticalName { get; set; }

    }
}
