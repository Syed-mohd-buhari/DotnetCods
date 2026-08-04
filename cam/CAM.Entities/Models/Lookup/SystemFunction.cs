using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;

namespace CAM.Entities.Models.Lookup
{
    [Table("SystemFunctions")]
    public class SystemFunction : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SystemFunctionId { get; set; }
        [Required]
        [Column("SystemFunction")]
        public string SystemFunctionDescription { get; set; }

        [InverseProperty(nameof(DesignComponentFamilySystemFunction.SystemFunction))]
        public virtual ICollection<DesignComponentFamilySystemFunction> DesignComponentFamilySystemFunctions { get; set; }
    }
}
