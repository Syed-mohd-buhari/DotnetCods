using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CAM.Entities.Models.Lookup
{
    [Table("VodafoneNames")]
    public partial class VodafoneNames: AuditableEntity
    {
        public VodafoneNames()
        {
            ProductNames = new HashSet<ProductName>();
            SystemTypes = new HashSet<SystemType>();
            RiskClusterVodafoneNames = new HashSet<RiskClusterVodafoneNames>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Description")]
        public string Description { get; set; }


        [InverseProperty(nameof(Lookup.ProductName.VodafoneName))]
        public virtual ICollection<ProductName> ProductNames { get; set; }


        [InverseProperty(nameof(SystemType.VodafoneName))]
        public virtual ICollection<SystemType> SystemTypes { get; set; }

        [InverseProperty(nameof(Lookup.RiskClusterVodafoneNames.VodafoneName))]
        public virtual ICollection<RiskClusterVodafoneNames> RiskClusterVodafoneNames { get; set; }



    }
}
