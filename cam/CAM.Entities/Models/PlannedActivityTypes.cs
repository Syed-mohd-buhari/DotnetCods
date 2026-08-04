using CAM.Entities.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("PlannedActivityTypes")]
    public class PlannedActivityTypes : AuditableEntity
    {
        public int PlannedActivityTypesId { get; set; }
        public string PlannedActivityTypeDescription { get; set; }
        public bool? HwOem { get; set; }
        public bool? HwSolution { get; set; }
        public bool? HwPlatform { get; set; }
        public bool? SwOem { get; set; }
        public bool? SwProductname { get; set; }
        public bool? SwVersion { get; set; }
        public bool? SubNetworkService { get; set; }
        public bool? LinkedDcRule { get; set; }
        public bool? Delete { get; set; }

        public bool? ForLcm { get; set; }
        public bool? ForAsset { get; set; }
        public bool? ForDesignAspect { get; set; }
        public bool? Forservice { get; set; }

    }
}