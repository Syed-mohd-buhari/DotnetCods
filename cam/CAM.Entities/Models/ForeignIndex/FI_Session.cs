using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Enum;

namespace CAM.Entities.Models.ForeignIndex
{
    [Table("FI_Sessions")]
    public partial class FI_Session : AuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SessionId { get; set; }

        [Required]
        public ForeignIndexStatus Status { get; set; }

        public ForeignIndexSource Source { get; set; }
    }
}