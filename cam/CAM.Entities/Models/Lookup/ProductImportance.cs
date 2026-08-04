using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("ProductImportances")]
    public partial class ProductImportance : AuditableEntity
    {
        public ProductImportance()
        {
            SystemTypes = new HashSet<SystemType>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short ProductImportanceId { get; set; }
        [Required]
        [Column("ProductImportance")]
        [StringLength(50)]
        public string ProductImportanceDescription { get; set; }
   
        [InverseProperty(nameof(SystemType.ProductImportanceRel))]
        public virtual ICollection<SystemType> SystemTypes { get; set; }    
        [InverseProperty(nameof(LcmEngineering.ProductImportanceRel))]
        public virtual ICollection<LcmEngineering> LcmEngineerings{ get; set; }
    }
}