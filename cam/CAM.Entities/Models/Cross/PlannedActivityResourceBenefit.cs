using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Cross
{
    [Table("PlannedActivityResourceBenefit")]
    public partial class PlannedActivityResourceBenefit : AuditableEntity
    {
        [Key]
        public long PlannedActivityResourceBenefitId { get; set; }

        public short PlannedActivityResourceId { get; set; }

        public short BenefitId { get; set; }

        public bool ForLcm { get; set; }

        public bool? ForDesignAspect { get; set; }

        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }

        [ForeignKey(nameof(PlannedActivityResourceId))]
        public virtual PlannedActivityResource PlannedActivityResource { get; set; }
        [ForeignKey(nameof(BenefitId))]
        public virtual Benefit Benefit { get; set; }
    }
}