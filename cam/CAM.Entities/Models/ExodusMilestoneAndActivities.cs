using CAM.Entities.Models.Base;
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
    [Table("Exodusmilestoneandactivities")]
    public class ExodusMilestoneAndActivities : AuditableEntity
    {
        [Key]
        public long ExodusMilestoneAndActivityId { get; set; }
        public string Activities { get; set; }
        public int? ActivitiesOrder { get; set; }
        public string Milestones { get; set; }
        public string ActualColumnNames { get; set; }

    }
}
