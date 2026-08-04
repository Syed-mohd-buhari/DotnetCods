using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CAM.DataTransferObjects.Entita.PlannedActivity
{
    public class PlannedActivityProps
    {
        public short plannedLcmDeploymentStatus { get; set; }
        public short budgetPlanningDeliveryStatusId { get; set; }
        public short newSolutionPATypeId { get; set; }
        public short noBudgetAvailabilityId { get; set; }
        public short planningActivityStatusId { get; set; }
        public short activityStatusId { get; set; }
        public short responsibilityPhaseId { get; set; }
        public Lcmengineering lcmExists { get; set; }
    }
}
