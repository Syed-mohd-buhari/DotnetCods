using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("SharingType")]
    public partial class SharingType : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SharingTypeId { get; set; }

        [Column("SharingType")]
        public string SharingTypeDescription { get; set; }

        [InverseProperty(nameof(DesignComponentFamily.SharingType))]
        public virtual ICollection<DesignComponentFamily> DesignComponentFamilies { get; set; }
    }
}