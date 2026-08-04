using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("SubDomainResponsibles")]
    public partial class SubDomainResponsible : AuditableEntity
    {
        public SubDomainResponsible()
        {
            SystemTypes = new HashSet<SystemType>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubDomainResponsibleId { get; set; }
        [Required]
        [Column("SubDomainResponsible")]
        [StringLength(15)]
        public string SubDomainResponsibleDescription { get; set; }

        [InverseProperty(nameof(SystemType.SubDomainResponsible))]
        public virtual ICollection<SystemType> SystemTypes { get; set; }
    }
}