using System;
using System.Collections.Generic;
using System.Text;
using CAM.DataTransferObjects.FunctionalityDto;
using CAM.DataTransferObjects.QueryDto.Base;

namespace CAM.DataTransferObjects.QueryDto
{
   public class SettingsUpdatePlannedActivityQueryDto : QueryObject
    {
        public List<short> PlanningActivityStatus { get; set; }
        public List<short> PlanningActivityResource { get; set; }
        public List<short> SuccessorPlannedActivityTypeResource { get; set; }

        public List<short> PlannedActivityTypeFor { get; set; }
        public List<bool> RuleforSuccessorPlannedActivityCreation { get; set; }
        public List<string> DeliveryStatus { get; set; }
        public List<short> BudgetAvailability { get; set; }
        public List<short> LcmDeploymentStatus { get; set; }

        public List<short> AssetDeploymentStatus { get; set; }

        public List<int> Rule { get; set; }
        public List<int> RuleElementCount { get; set; }

        public List<int> DCRule { get; set; }
        public List<bool> SpecifyDC { get; set; }

        public List<bool> NeedPlannedAsset { get; set; }
        public List<int> Order { get; set; }
        public List<int> MaxOrder { get; set; }
        public List<string> LocalApproval { get; set; }

        public List<string> SettingsUpdatePlannedActivityDescription { get; set; }

        public List<string> CrossSetting { get; set; }
        public List<string> LastModifiedBy { get; set; }
        public DateFilter LastModifiedValue { get; set; }

        public List<string> PlannedActivityTypeDescription { get; set; }

        public List<bool> IsRollback { get; set; }
        public List<int> MSStatus { get; set; }
        public List<int> MSStatusDuration { get; set; }
        public List<string> IsMileStone { get; set; }

    }
}
