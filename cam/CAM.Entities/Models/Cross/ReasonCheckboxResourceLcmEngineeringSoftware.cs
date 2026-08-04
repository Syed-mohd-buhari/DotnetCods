using System;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Cross
{
    public class ReasonCheckboxResourceLcmEngineeringSoftware
    {
        public decimal Id { get; set; }
        public long LcmEngineeringId { get; set; }
        
        public short ReasonCheckboxResourceId { get; set; }

        [ForeignKey(nameof(ReasonCheckboxResourceId))]
        public virtual ReasonCheckboxResource CheckboxResource { get; set; }
        [ForeignKey(nameof(LcmEngineeringId))]
        public virtual LcmEngineering LcmEngineering { get; set; }
    }
}