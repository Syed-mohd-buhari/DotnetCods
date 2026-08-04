using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
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
    [Table("Resourcekeymaster")]
    public class ResourceKeyMaster : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Resourcekeymasterid")]
        public long ResourceKeyMasterId { get; set; }
        public long? ResourceTypesId { get; set; }
        public string ResourceKey { get; set; }
        public long? DcfId { get; set; }
        public short? OpcoId { get; set; }
        public long? EventId { get; set; }
        public long? DcId { get; set; }
        public bool? KeyStatus { get; set; }
        public string ElementName { get; set; }
        public int LifeCycleId { get; set; }

        public long BuildBagId { get; set; }
        public long? ComponentId { get; set; }

        [ForeignKey(nameof(DcfId))]
        [InverseProperty("Resourcekeymaster")]
        public virtual DesignComponentFamily DesignComponentFamily { get; set; }

        [ForeignKey(nameof(OpcoId))]
        [InverseProperty("Resourcekeymaster")]
        public virtual OpCo Opco { get; set; }

        [ForeignKey(nameof(ResourceTypesId))]
        [InverseProperty("Resourcekeymaster")]
        public virtual ResourceTypes ResourceTypes { get; set; }
    }
}
