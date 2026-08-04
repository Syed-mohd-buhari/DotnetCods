using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("OpCos")]
    public partial class OpCo : AuditableEntity
    {
        public OpCo()
        {
            Lcmengineerings = new HashSet<LcmEngineering>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short OpCoId { get; set; }
        [Column("OpCo")]
        [StringLength(5)]
        public string OpCoDescription { get; set; }

        [InverseProperty(nameof(LcmEngineering.OpCo))]
        public virtual ICollection<LcmEngineering> Lcmengineerings { get; set; }

        [InverseProperty(nameof(VNFTransition.OpCo))]
        public virtual ICollection<VNFTransition> VNFTransitions { get; set; }

        [InverseProperty(nameof(NetworkElementAsPlanned.OpCo))]
        public virtual ICollection<NetworkElementAsPlanned> Networkelementsasplanned { get; set; }

        [InverseProperty(nameof(NetworkElementAsIs.OpCo))]
        public virtual ICollection<NetworkElementAsIs> Networkelementsasis { get; set; }

        [InverseProperty(nameof(VolteKPI.OpCo))]
        public virtual ICollection<VolteKPI> Voltekpi { get; set; }

        [InverseProperty(nameof(Models.VolteKPIWorklog.OpCo))]
        public virtual ICollection<VolteKPIWorklog> VolteKPIWorklog { get; set; }

        [InverseProperty(nameof(Models.ResourceKeyMaster.Opco))]
        public virtual ICollection<ResourceKeyMaster> ResourceKeyMaster { get; set; }
    }
}