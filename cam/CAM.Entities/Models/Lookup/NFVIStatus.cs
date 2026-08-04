using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("NFVIStatuses")]
    public class NFVIStatus : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short NFVIStatusId { get; set; }

        [Required]
        [Column("NFVIStatus")]
        [StringLength(50)]
        public string NFVIStatusDescription { get; set; }

        [Required]
        [Column("Color")]
        [StringLength(50)]
        public string Color { get; set; }


        public virtual ICollection<NFVITransition> NFVIStatusesLabMC { get; set; }
        public virtual ICollection<NFVITransition> NFVIStatusesLabSC { get; set; }
        public virtual ICollection<NFVITransition> NFVIStatusesLiveMC { get; set; }
        public virtual ICollection<NFVITransition> NFVIStatusesLiveSC { get; set; }
        public virtual ICollection<NFVITransition> NFVIStatuses12KSwitch { get; set; }

    }
}
