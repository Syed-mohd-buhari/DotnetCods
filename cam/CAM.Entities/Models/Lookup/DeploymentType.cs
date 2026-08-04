using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("DeploymentTypes")]
    public partial class DeploymentType : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short DeploymentTypeId { get; set; }

        [Column("DeploymentType")]
        
        public string DeploymentTypeDescription { get; set; }
        public int Rule { get; set; }

        [InverseProperty(nameof(NetworkElementAsPlanned.DeploymentType))]
        public virtual ICollection<NetworkElementAsPlanned> Networkelementsasplanned { get; set; }
    }
}
