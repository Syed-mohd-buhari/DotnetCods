using CAM.DataAttributes.Grid;
using CAM.DataTransferObjects.QueryDto.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.DataTransferObjects.Entita.ProjectPlan
{
    public class ProjectPlanDto
    {
        
        public long PaId { get; set; }
        public string ActivityDetail { get; set; }        
        public string EduSpoc { get; set; }        
        public string ColorCoding { get; set; }        
        public List<MilestoneDto> Milestones { get; set; }
         
    }

    public class MilestoneDto
    {
        public int? MilestoneId { get; set; }
        public string MilestoneName { get; set; }
        public int? MilestoneDuration { get; set; }
        public List<PAandDeliveryStatuses> Statuses { get; set; }
    }

    public class PAandDeliveryStatuses
    {
        public long ProjectPlanId { get; set; }
        public long DeliveryStatusId { get; set; }
        public string DeliveryStatusText { get; set; }
        public DateTime? PlanningStartDate { get; set; }
        public DateTime? PlanningEndDate { get; set; }
        public string Progress { get; set; }
        public string Description { get; set; }
        public string IsMileStone { get;set; }
        public DateTime? BaseLineStartDate { get; set; }
        public DateTime? BaseLineEndDate { get; set; }
    }


    public class ProjectPlanDtoGrid : ProjectPlanDto
    {
        [IgnoreGrid]
        public long ProjectsPlanId { get; set; }
    }

    public class ProjectPlanExportDto
    {
        [Default]
        [OrderGrid(Order = 1)]
        [DisplayName("Projects Plan Id")]
        public long ProjectsPlanId { get; set; }
        [Default]
        [OrderGrid(Order = 2)]
        [DisplayName("PA ID")]
        public long PaId { get; set; }
        [Default]
        [OrderGrid(Order = 3)]
        [DisplayName("Activity Detail")]
        public string ActivityDetail { get; set; }
        [IgnoreGrid]
        public int? MilestoneId { get; set; }
        [Default]
        [OrderGrid(Order = 4)]
        [DisplayName("Milestone Duration")]
        public int? MilestoneDuration { get; set; }
        [Default]
        [OrderGrid(Order = 5)]
        [DisplayName("Milestone Name")]
        public string MilestoneName { get; set; }
        [IgnoreGrid]
        public long DeliveryStatusId { get; set; }
        [Default]
        [OrderGrid(Order = 6)]
        [DisplayName("Delivery Status")]
        public string DeliveryStatusText { get; set; }
        [Default]
        [OrderGrid(Order = 7)]
        [DisplayName("Planning Start Date")]
        public DateTime? PlanningStartDate { get; set; }
        [Default]
        [OrderGrid(Order = 8)]
        [DisplayName("Planning End Date")]
        public DateTime? PlanningEndDate { get; set; }
        [Default]
        [OrderGrid(Order = 9)]
        [DisplayName("Progress")]
        public string Progress { get; set; }
        [Default]
        [OrderGrid(Order = 10)]
        [DisplayName("Plan Description")]
        public string PlanDescription { get; set; }
        [IgnoreGrid]
        public int order { get; set; }

        [Default]
        [OrderGrid(Order = 11)]
        [DisplayName("IsMileStone")]
        public string IsMileStone { get; set; }

        [Default]
        [OrderGrid(Order = 12)]
        [DisplayName("EduSpoc")]
        public string EduSpoc { get; set; }

        [Default]
        [OrderGrid(Order = 13)]
        [DisplayName("BaseLine Start Date")]
        public DateTime? BaseLineStartDate { get; set; }

        [Default]
        [OrderGrid(Order = 14)]
        [DisplayName("BaseLine End Date")]
        public DateTime? BaseLineEndDate { get; set; }

        [IgnoreGrid]
        public string ColorCoding { get; set; }
    }

}
