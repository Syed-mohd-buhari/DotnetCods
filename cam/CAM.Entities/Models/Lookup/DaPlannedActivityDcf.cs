using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Lookup
{
    [Table("Daplannedactivitydcf")]
    public partial class DaPlannedActivityDcf : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DaPlannedActivityDcfId { get; set; }
        public long? PlannedActivityId { get; set; }
        public long? DesignComponentFamilyId { get; set; }
        public bool? DcfStatus { get; set; }

        public virtual DesignComponentFamily DesignComponentFamily { get; set; }
        public virtual PlannedActivity PlannedActivity { get; set; }
    }
}