using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("VerticalResponsibles")]
    public partial class VerticalResponsible : AuditableEntity
    {
        public VerticalResponsible()
        {
            SystemTypes = new HashSet<SystemType>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VerticalResponsibleId { get; set; }
        [Required]
        [Column("VerticalResponsible")]
        [StringLength(15)]
        public string VerticalResponsibleDescription { get; set; }

        [InverseProperty(nameof(SystemType.VerticalResponsible))]
        public virtual ICollection<SystemType> SystemTypes { get; set; }
    }
}