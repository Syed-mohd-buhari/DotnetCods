using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    [Table("DeploymentStatuses")]
    public partial class DeploymentStatus : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short DeploymentStatusId { get; set; }

        [Column("DeploymentStatus")]
        public string DeploymentStatusDescription { get; set; }
        public int? Rule { get; set; }

        public string PlannedActivityResourceAllowed { get; set; }
        public bool ReadOnlyPlannedActivity { get; set; }

        public bool DefaultValue { get; set; }
        public bool CheckPlannedActivity { get; set; }


        [InverseProperty(nameof(NetworkElementAsPlanned.DeploymentStatus))]
        public virtual ICollection<NetworkElementAsPlanned> Networkelementsasplanned { get; set; }
    }
}
