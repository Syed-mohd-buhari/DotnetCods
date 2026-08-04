using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;

namespace CAM.Entities.Models.Lookup
{
    [Table("AssetCategories")]
    public partial class AssetCategory : AuditableEntity
    {
        public AssetCategory()
        {
            SystemTypes = new HashSet<SystemType>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetCategoryId { get; set; }
        [Required]
        [Column("AssetCategory")]
        [StringLength(50)]
        public string AssetCategoryDescription { get; set; }
        public bool TakeFromAssetTypeTable { get; set; }

        public int? AssetClassId { get; set; }

        [ForeignKey(nameof(AssetClassId))]
        public virtual AssetClass AssetClass { get; set; }


        [InverseProperty(nameof(SystemType.AssetCategory))]
        public virtual ICollection<SystemType> SystemTypes { get; set; }
    }
}