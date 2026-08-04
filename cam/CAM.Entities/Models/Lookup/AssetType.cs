using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("AssetTypes")]
    public partial class AssetType : AuditableEntity
    {
        public AssetType()
        {
            SystemTypes = new HashSet<SystemType>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetTypeId { get; set; }
        [Required]
        [Column("AssetType")]
        [StringLength(50)]
        public string AssetTypeDescription { get; set; }


        public int? AssetCategoryId { get; set; }

        public int AssetClassId { get; set; }

        [ForeignKey(nameof(AssetCategoryId))]
        public virtual AssetCategory AssetCategory { get; set; }


        [InverseProperty(nameof(SystemType.AssetTypeIdNavigation))]
        public virtual ICollection<SystemType> SystemTypes { get; set; }
    }
}