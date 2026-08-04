using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models
{
    [Table("NFVITransitions")]
    public  class NFVITransition : AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NFVITransitionId { get; set; }
        public short OpCoId { get; set; }
       
        public short? StatusLabMCId { get; set; }
        
        public short? StatusLabSCId { get; set; }
        
        public short? StatusLiveMCId { get; set; }
       
        public short? StatusLiveSCId { get; set; }
        [Required]
        public string NFVISiteDesignation { get; set; }
        public string NextStep { get; set; }
        public short? Status12KSwitchId { get; set; }        
        [Column("Spare1Json")]
        public string Spare1Json { get; set; }


        [ForeignKey(nameof(OpCoId))]
        public virtual OpCo OpCo { get; set; }
        [ForeignKey(nameof(StatusLabMCId))]
        public virtual NFVIStatus NFVIStatusesLabMC { get; set; }
        [ForeignKey(nameof(StatusLabSCId))]
        public virtual NFVIStatus NFVIStatusesLabSC { get; set; }
        [ForeignKey(nameof(StatusLiveMCId))]
        public virtual NFVIStatus NFVIStatusesLiveMC { get; set; }
        [ForeignKey(nameof(StatusLiveSCId))]
        public virtual NFVIStatus NFVIStatusesLiveSC { get; set; }
        [ForeignKey(nameof(Status12KSwitchId))]
        public virtual NFVIStatus NFVIStatuses12KSwitch { get; set; }


    }
}
