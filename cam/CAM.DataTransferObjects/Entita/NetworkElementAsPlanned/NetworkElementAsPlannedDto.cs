using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAM.DataTransferObjects.Entita.NetworkElementAsPlanned
{
    public class NetworkElementAsPlannedDto : GridDtoBase
    {       

        [OrderGrid(Order = 1)]
        [Default]
        [DisplayName("Element Name")]
        public string ElementName { get; set; }
        [OrderGrid(Order = 2)]
        [Default]
        [DisplayName("HW Resource Key")]
        public string HwResourceKey { get; set; }
        [OrderGrid(Order = 3)]
        [Default]
        [DisplayName("SW Resource Key")] 
        public string SwResourceKey { get; set; }

        [OrderGrid(Order = 4)]
        [DisplayName("Node Index")]
        public string NodeIndex { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Bag Name")]
        public string BuildBagDescription { get; set; }

        [OrderGrid(Order = 12)]
        public string NetworkConstruct { get; set; }

        [OrderGrid(Order = 18)]
        [Default]
        [DisplayName("Planned Action")]
        public bool PlannedAction { get; set; }

        [OrderGrid(Order = 21)]
        [Default]
        [DisplayName("Automated Feedback")]
        public bool AutomatedFeedback { get; set; }

        [OrderGrid(Order = 22)]
        public string CapacityPlanReference { get; set; }
        [OrderGrid(Order = 23)]
        public string AdditionalInformation1 { get; set; }
        [OrderGrid(Order = 24)]
        public string AdditionalInformation2 { get; set; }        

             [OrderGrid(Order = 31)]
        [DisplayName("Element Domian Name")]
        public string ElementDomianName { get; set; }

        [OrderGrid(Order = 32)]
        public string Assured { get; set; }

        [OrderGrid(Order = 33)]
        [DisplayName("Previous HW Resource Key")]
        public string PreviousHWResourceKey { get; set; }

        [OrderGrid(Order = 34)]
        [DisplayName("Previous SW Resource Key")]
        public string PreviousSWResourceKey { get; set; }

        [OrderGrid(Order = 35)]
        [DisplayName("Asset LiveStatus Date")]
        [DateRangeGrid]
        public string AssetLiveStatusDateValue { get; set; }
  
        [OrderGrid(Order = 36)]
        [DisplayName("Date Asset Decommissioned")]
        [DateRangeGrid]
        public string DateAssetDecommissionedAssetValue { get; set; }
        
        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? DateAssetDecommissionedAsset { get; set; }

        [IgnoreGrid]
        [OrderGrid(Order = 37)]
        [DateRangeGrid]
        [DisplayName("Last Modified")]
        public string LastModifiedValue { get; set; }

        [OrderGrid(Order = 38)]
        [MailTo]
        [DisplayName("Last Modified By")]
        public new string LastModifiedBy { get; set; }

        [IgnoreGrid]
        public short OriginalEquipmentManufacturerId { get; set; }
 

        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? AssetLiveStatusDate { get; set; }

        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? AssetRfoDate { get; set; }

        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? AssetRfsDate { get; set; }

        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? RfaDate { get; set; }

        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? HwPoArrivedDate { get; set; }

        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? HwPoRaisedDate { get; set; }

        [IgnoreGrid]
        [DateRangeGrid]
        public DateTime? BomSubmittedDate { get; set; }

    }

    public class NetworkElementAsPlannedDtoGrid : NetworkElementAsPlannedDto
    {
        [IgnoreGrid]
        public long NetworkElementAsPlannedId { get; set; }

        [OrderGrid(Order = 6)]
        [Default]
        public string OpCo { get; set; }

        [IgnoreGrid]
        public string OpCoId { get; set; }

        [OrderGrid(Order = 7)]
        [DisplayName("IsVirtualized/Contanarized")]
        public bool IsVirtualizedOrContanarized { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Design Component Index")]
        public long DesignComponentIndex { get; set; }

        [OrderGrid(Order = 9)]
        [DisplayName("Design Component")]
        [Default]
        public string DesignComponent { get; set; }

        [OrderGrid(Order = 10)]
        [DisplayName("Design Component Family Index")]
        public long? DesignComponentFamilyIndex { get; set; }

        [OrderGrid(Order = 11)]
        [Default]
        public string Environment { get; set; }
        [OrderGrid(Order = 13)]
        [Default]
        [DisplayName("Deployment Status")]
        public string DeploymentStatus { get; set; }

        [OrderGrid(Order = 14)]
        [Default]
        [DisplayName("Location Type")]
        public string DeploymentType { get; set; }

        [OrderGrid(Order = 15)]
        [DisplayName("Location")]
        [Default]
        public string Location { get; set; }


        [OrderGrid(Order = 16)]
        [Default]
        [DisplayName("Edu-Spoc")]
        public string Eduspoc { get; set; }

        [OrderGrid(Order = 17)]
        [DisplayName("Sub-Domain Spoc")]
        [Default]
        public string SubDomainSpoc { get; set; }

        [OrderGrid(Order = 25)]
        [Default]
        [DisplayName("NFVI Bundle ID")]
        public string? NfviBundleID { get; set; }

        [OrderGrid(Order = 26)]
        [Default]
        [DisplayName("Planned Activity")]
        public IDictionary<short, string> PlannedActivity { get; set; } 

  
        [OrderGrid(Order = 27)]
        [DisplayName("Vodafone Name")]
        [Default]
        public string VodafoneName { get; set; }       


        [OrderGrid(Order = 28)]        
        [DisplayName("Vertical Name")]
        public string VerticalName { get; set; }

        [OrderGrid(Order = 29)]
        [IgnoreGrid]
        
        public int VerticalId { get; set; }

        [OrderGrid(Order = 30)]
        [DisplayName("Domain Name")]
        [IgnoreGrid]
        public string DomainName { get; set; }


        [DisplayName("LCM Engineering Id")]
        [IgnoreGrid]
        public long LcmEngineeringId { get; set; }


        [DisplayName("LCM Deployement Status")]
        [IgnoreGrid]
        public string LCMDeployementstatus { get; set; }

    }
    public class NetworkElementAsPlannedPivotFormatDtoGrid : NetworkElementAsPlannedPivotDtoGrid
    {
        [OrderGrid(Order = 1)]
        public string PAFinancialYear { get; set; }

        public List<NetworkElementAsPlannedPivotDtoGrid> PivotAssets {get;set;}
    }
    public class NetworkElementAsPlannedPivotDtoGrid
    {


        [OrderGrid(Order = 1)]
        public string PaImplementaionYear { get; set; }


        [IgnoreGrid]
        public string PlannedImplementaionYear { get; set; }

        [IgnoreGrid]
        public long DesignComponentIndex { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Design Component")]        
        public string DesignComponent { get; set; }

        [OrderGrid(Order = 3)]
        public List<KeyValuePair<string, int>> Locations { get; set; }

        [OrderGrid(Order = 4)]
        public long Total { get; set; }

        [IgnoreGrid]
        public string Location { get; set; }

        [IgnoreGrid]
        public int LocationId { get; set; }

        [DisplayName("Vertical Name")]
        public string VerticalName { get; set; }

        [IgnoreGrid]
        public List<FilterValueDto> VerticalFilterDto { get; set; }

    }
    public class AssetPlannedActivityDetailForPivotEntity
    {
        public long Plannedactivityid { get; set; }
        public short Plannedimplementationyear { get; set; }
        public short Activitystatusid { get; set; }
    }
}
