using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("EndOfSupportContracts")]
    public class EndOfSupportContract : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short EndOfSupportContractId { get; set; }
        [Required]
        [Column("EndOfSupportContract")]
        public string EndOfSupportContractDescription { get; set; }


        //public virtual ICollection<LcmEngineering> SoftwareEndOfMaintenanceContracts { get; set; }
        //public virtual ICollection<LcmEngineering> HardwareEndOfMaintenanceContracts { get; set; }

    }
}

