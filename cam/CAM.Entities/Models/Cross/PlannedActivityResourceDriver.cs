using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Cross
{
    [Table("PlannedActivityResourceDriver")]
    public partial class PlannedActivityResourceDriver : AuditableEntity
    {
        [Key]
        public long PlannedActivityResourceDriverId { get; set; }

        public short PlannedActivityResourceId { get; set; }

        public short DriverId { get; set; }

        public bool ForLcm { get; set; }
        public bool? ForDesignAspect { get; set; }

        public bool? ForAddAsset { get; set; }
        public bool? ForEditAsset { get; set; }

        [ForeignKey(nameof(PlannedActivityResourceId))]
        public virtual PlannedActivityResource PlannedActivityResource { get; set; }
        [ForeignKey(nameof(DriverId))]
        public virtual Driver Driver { get; set; }
    }
}