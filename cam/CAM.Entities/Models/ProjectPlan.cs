using CAM.Entities.Models.Base;
using CAM.Entities.Models.Settings;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAM.Entities.Models
{
    [Table("Projectsplan")]
    public class ProjectPlan : AuditableEntity
    {
        public long ProjectsPlanId { get; set; }
        public long PlannedActivityId { get; set; }
        public short SettingsUpdatePlannedActivityId { get; set; }
        public DateTime? PlanningStartDate { get; set; }
        public DateTime? PlanningEndDate { get; set; }
        public string Description { get; set; }
        public string Progress { get; set; }

        public virtual PlannedActivity PlannedActivity { get; set; }
        public virtual SettingsUpdatePlannedActivity SettingsUpdatePlannedActivity { get; set; }

    }
}
