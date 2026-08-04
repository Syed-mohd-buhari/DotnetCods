using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("LCMEXPORTSETTINGS")]
    public partial class LcmExportSetting : AuditableEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public string LcmHistoricalInfoSW { get; set; }
        public string LcmHistoricalInfoHW { get; set; }

        public bool? IsHistorical { get; set; }
        public int? ReportLevel { get; set; }
        public int? ReportType { get; set; }

        public bool? IsDefault { get; set; }
    }
}