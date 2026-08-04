using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("SecurityTireZone")]
    public partial class SecurityTireZone : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SecurityTireZoneId { get; set; }

        [Column("SecurityTireZone")]
        public string SecurityTireZoneDescription { get; set; }

        [InverseProperty(nameof(DesignComponentFamily.SecurityTireZone))]
        public virtual ICollection<DesignComponentFamily> DesignComponentFamilies { get; set; }
    }
}