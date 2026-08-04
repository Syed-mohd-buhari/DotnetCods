using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;

namespace CAM.Entities.Models.Lookup
{
    public class SupportedResource : AuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        [Required]
        public string Description { get; set; }

        public int Rule { get; set; }

        public virtual ICollection<LcmEngineering> LcmEngineeringsSoftware { get; set; }
        public virtual ICollection<LcmEngineering> LcmEngineeringsHardware { get; set; }
    }
}