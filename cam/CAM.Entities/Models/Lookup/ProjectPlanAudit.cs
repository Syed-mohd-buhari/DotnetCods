using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("Projectplanaudit")]
    public partial class ProjectPlanAudit : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ProjectPlanAuditId { get; set; }
        public long? ProjectsPlanId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public short? ProcessType { get; set; }

    }
}
