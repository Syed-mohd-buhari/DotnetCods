using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;

namespace CAM.Entities.Models.Lookup
{
    public class ReasonCheckboxResource : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }
        public string Description { get; set; }
        public bool IsHardware { get; set; }
        public bool IsSoftware { get; set; }

        public virtual ICollection<ReasonCheckboxResourceLcmEngineeringSoftware> ReasonCheckboxResourceLcmEngineeringSoftware
        {
            get;
            set;
        }
        public virtual ICollection<ReasonCheckboxResourceLcmEngineeringHardware> ReasonCheckboxResourceLcmEngineeringHardware
        {
            get;
            set;
        }
    }
}