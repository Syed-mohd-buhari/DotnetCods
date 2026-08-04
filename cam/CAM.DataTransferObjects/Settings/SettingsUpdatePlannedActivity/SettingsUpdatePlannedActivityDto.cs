using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CAM.Contracts;
using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using CAM.Infrastucture;

namespace CAM.DataTransferObjects.Settings.SettingsUpdatePlannedActivity
{
   public class SettingsUpdatePlannedActivityDto : GridDtoBase
    {

        public string SettingsUpdatePlannedActivityDescription { get; set; }
        public string LocalApproval { get; set; }
        public int Order { get; set; }
        public int MaxOrder { get; set; }
        public int Rule { get; set; }
       
    }

   public class SettingsUpdatePlannedActivityDtoCreate : SettingsUpdatePlannedActivityDtoGrid
    {

        public short DeliveryStatusId { get; set; }
        public IDictionary<short, string> DeliveryStatusResource { get; set; }
       
        public short PlanningActivityStatusId { get; set; }
        public IDictionary<short, string> PlanningActivityStatusResource { get; set; }
        public short? PlanningActivityResourceId { get; set; }
        public IDictionary<short, string> PlanningActivityResource { get; set; }

        public short? PlannedActivityTypeFor { get; set; }

        public short? SuccessorPlannedActivityId { get; set; }

        public IDictionary<short, string> SuccessorPlannedActivityTypeResource { get; set; }

        public bool? RuleforSuccessorPlannedActivityCreation { get; set; }

        //public short? PlannedActivityTypeFor { get; set; }

        public short BudgetAvailabilityId { get; set; }

        public bool NeedPlannedAsset { get; set; }
        public IDictionary<short, string> BudgetAvaibilityResource { get; set; }

        public IEnumerable<RelatedResource> CrossSettingsOutIds { get; set; }

        public IDictionary<short, string> CrossSettingscResource { get; set; }
        public IDictionary<short, string> LcmDeploymentStatusResource { get; set; }
        public IEnumerable<short> LcmDeploymentStatusIds { get; set; }

        public IEnumerable<short> AssetDeploymentStatusIds { get; set; }
    }

    public class SettingsUpdatePlannedActivityDtoUpdate : SettingsUpdatePlannedActivityDtoCreate
    {
        public short SettingsUpdatePlannedActivityId { get; set; }

        public bool IsReleaseDetailsUnknown { get; set; }

        public bool IsPAReleaseDetailsUnknown { get; set; }

        public int? MsStatus { get; set; }

        public bool IsLiveStatusDateAvailable { get; set; }
        public bool IsDecommissionedDateAvailable { get; set; }
    }

    public class SettingsUpdatePlannedActivityDtoGrid : IGridDtoBase
    {
        [IgnoreGrid]
        public short SettingsUpdatePlannedActivityId { get; set; }

        [OrderGrid(Order = 1)]
        [DisplayName("Planning Activity Type")]
        [Default]
        public string PlanningActivityResource { get; set; }

        [OrderGrid(Order = 2)]
        [DisplayName("Planned Activity Type Description")]
        [Default]
        public string PlannedActivityTypeDescription { get; set; }


        [OrderGrid(Order = 3)]
        [DisplayName("Planned Activity Type Is For")]
        public string PlannedActivityTypeForValue { get; set; }



        [OrderGrid(Order = 4)]
        [DisplayName("Description")]
        [Default]
        public string SettingsUpdatePlannedActivityDescription { get; set; }

        [OrderGrid(Order = 5)]
        [DisplayName("Delivery Status")]
        [Default]
        public string DeliveryStatus { get; set; }

        [OrderGrid(Order = 6)]
        [DisplayName("Planning Activity Status")]
        [Default]
        public string PlanningActivityStatus { get; set; }
        [OrderGrid(Order = 7)]
        [DisplayName("Local Approval")]
        [Default]
        public string LocalApproval { get; set; }

        [OrderGrid(Order = 8)]
        [DisplayName("Max Order")]
        [Default]
        public int MaxOrder { get; set; }

        [OrderGrid(Order = 9)]
        [Default]
        public int Order { get; set; }

        [OrderGrid(Order = 10)]
        [Default]
        public string CrossSetting { get; set; }

        [OrderGrid(Order = 11)]
        [DisplayName("Budget Availability")]
        [Default]
        public string BudgetAvailability { get; set; }


        [DisplayName("Lcm Deployment Status")]
        [OrderGrid(Order = 12)]
        [Default]
        public string LcmDeploymentStatus { get; set; }

        [DisplayName("Asset Deployment Status")]
        [OrderGrid(Order = 13)]
        [Default]
        public string AssetDeploymentStatus { get; set; }

     

        [OrderGrid(Order = 14)]
        [DisplayName("Successor Planned Activity Type Resource")]
        [Default]
        public string SuccessorPlannedActivityTypeResource { get; set; }


        [OrderGrid(Order = 15)]
        [DisplayName("Rule for Successor Planned Activity Creation")]
        [Default]
        public bool? RuleforSuccessorPlannedActivityCreation { get; set; }
              
        

        [OrderGrid(Order = 16)]
        [DisplayName("Rule")]
        [Default]
        public int Rule { get; set; }



        [OrderGrid(Order = 17)]
        [DisplayName("Rule Element Count")]
        [Default]
        public int RuleElementCount { get; set; }

        [OrderGrid(Order = 18)]
        [DisplayName("Specify DC")]
        [Default]
        public bool SpecifyDC { get; set; }

        [OrderGrid(Order = 19)]
        [DisplayName("Milestone")]
        [Default]
        public string MsStatus { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("Milestone Duration (Months)")]
        [Default]
        public int? MsStatusDuration { get; set; }

        [IgnoreGrid]
        public bool? Deleted { get; set; }
        [IgnoreGrid]
        public bool? Orphan { get; set; }

        [OrderGrid(Order = 19)]
        [DisplayName("Need Planned Asset")]
    
        public bool NeedPlannedAsset { get; set; }

        [OrderGrid(Order = 20)]
        [DisplayName("Is Rollback")]
        public bool IsRollback { get; set; }

        [OrderGrid(Order = 21)]
        [DisplayName("Is MileStone")]
        public string IsMileStone { get; set; }

        [OrderGrid(Order = 22)]
        [DisplayName("Last Modified")]
        public DateTime? LastModified { get; set; }

        [MailTo]
        [OrderGrid(Order = 23)]
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get; set; }


    }
}
