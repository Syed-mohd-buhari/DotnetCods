using CAM.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    [Table("Environments")]
    public partial class Environment : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short EnvironmentId { get; set; }

        [Column("Environment")]
       
        public string EnvironmentDescription { get; set; }

        [InverseProperty(nameof(NetworkElementAsPlanned.Environment))]
        public virtual ICollection<NetworkElementAsPlanned> Networkelementsasplanned { get; set; }
    }
}
