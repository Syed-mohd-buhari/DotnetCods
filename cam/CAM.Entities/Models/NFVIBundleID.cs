using CAM.Entities.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("NFVIBundleIDs")]
   public class NFVIBundleID : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short NFVIBundleIDId { get; set; }
        [Required]
        [Column("NFVIBundleID")]
        public string NFVIBundleIdDescription { get; set; }
        public int Order { get; set; }

        [InverseProperty(nameof(VNFTransition.NFVIBundleID))]
        public virtual ICollection<VNFTransition> VNFTransitions { get; set; }

        [InverseProperty(nameof(NetworkElementAsPlanned.NFVIBundleID))]
        public virtual ICollection<NetworkElementAsPlanned> Networkelementsasplanned { get; set; }
    }
}
