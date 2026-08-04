using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("AssetClasses")]
    public partial class AssetClass : AuditableEntity
    {
        public AssetClass()
        {
            SystemTypes = new HashSet<SystemType>();
        }

        [Key]
        public int AssetClassId { get; set; }
        [Required]
        [Column("AssetClass")]
        [StringLength(50)]
        public string AssetClassDescription { get; set; }

        public int AssetCategoryId { get; set; }
        //public bool IsHw { get; set; }
        //public bool IsSw { get; set; }

        [InverseProperty(nameof(SystemType.AssetClassIdNavigation))]
        public virtual ICollection<SystemType> SystemTypes { get; set; }
    }
}