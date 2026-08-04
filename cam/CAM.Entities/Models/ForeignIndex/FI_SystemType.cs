using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.ForeignIndex
{
    [Table("FI_SystemTypes")]
    public partial class FI_SystemTypes : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FI_SystemTypesId { get; set; }
        [Required]
        public long SessionId { get; set; }
        public long? DesignComponentId { get; set; }
        [Required]
        public long SystemTypesId { get; set; }
        [Required]
        public string Description { get; set; }
        public bool ToDelete { get; set; }
    }
}