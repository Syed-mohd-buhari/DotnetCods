using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.ForeignIndex
{
    [Table("FI_DesignComponents")]
    public partial class FI_DesignComponent : AuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FI_DesignComponentId { get; set; }

        [Required]
        public long SessionId { get; set; }

        [Required]
        public long DesignComponentId { get; set; }
        [Required]
        public string Description { get; set; }

        public bool ToDelete { get; set; }
    }
}