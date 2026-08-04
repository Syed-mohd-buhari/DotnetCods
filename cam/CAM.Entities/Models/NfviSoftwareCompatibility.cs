using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM.Entities.Models
{
    [Table("NfviSoftwareCompatibility")]
    public partial class NfviSoftwareCompatibility : AuditableEntity
    {
        [Key]
        public long NfviSoftwareCompatibilityId { get; set; }
        public long PlaftFormId { get; set; }
        public decimal ProductId { get; set; }
        public string MinimumSupportedVersion { get; set; }
        public short VendorId { get; set; }
        public DateTime? Deletiondate { get; set; }
        public virtual MajorSoftwareBuild MajorSoftwareVmwarePlaftForm { get; set; }
        public virtual ProductName ProductName { get; set; }
        public virtual OriginalEquipmentManufacturer Vendor { get; set; }

    }
}
