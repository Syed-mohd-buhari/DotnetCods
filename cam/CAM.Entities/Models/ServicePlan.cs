using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;
namespace CAM.Entities.Models
{
    public partial class ServicePlan : AuditableEntity
    {       
        public ServicePlan() 
        {
            Serviceplandcfmappings = new HashSet<ServicePlanDcfMapping>();
            Plannedactivity = new HashSet<PlannedActivity>();
        }
        public int Serviceplanid { get; set; }
        public int Servicemasterid { get; set; }
        public long Designcomponentfamilyid { get; set; }
        public short? Status { get; set; }
        public long Plannedactivityid { get; set; }
        public short? Opcoid { get; set; }
        public virtual OpCo Opco { get; set; }
        public virtual ICollection<PlannedActivity> Plannedactivity { get; set; }
        public virtual ServiceMaster Servicemaster { get; set; }
        public virtual ICollection<ServicePlanDcfMapping> Serviceplandcfmappings { get; set; }

        public List<FilterValueDtoKeyValueList> VerticalFilterDto { get; set; }

        public List<int?> DesignContactList { get; set; }


    }
}
