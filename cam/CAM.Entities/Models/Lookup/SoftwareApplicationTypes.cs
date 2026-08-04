using CAM.Entities.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models.Lookup
{
    [Table("ProductName")]
    public class ProductName : AuditableEntity
    {
        public ProductName()
        {
            MajorSoftwareBuilds = new HashSet<MajorSoftwareBuild>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("productName")]
        public string Description { get; set; }

        public bool? IsPlatformSoftware { get; set; }

        [InverseProperty(nameof(MajorSoftwareBuild.ProductName))]
        public virtual ICollection<MajorSoftwareBuild> MajorSoftwareBuilds { get; set; }

        [InverseProperty(nameof(DesignComponentFamily.ProductNameNavigation))]
        public virtual ICollection<DesignComponentFamily> DesignComponentFamilies { get; set; }

        public int? VodafoneNameId { get; set; }


        [ForeignKey(nameof(VodafoneNameId))]
        [InverseProperty(nameof(Lookup.VodafoneNames.ProductNames))]
        public virtual VodafoneNames VodafoneName { get; set; }
    }
}
