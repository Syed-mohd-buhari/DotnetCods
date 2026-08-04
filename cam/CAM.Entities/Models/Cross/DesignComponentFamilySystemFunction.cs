using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Cross
{
    [Table("DesignComponentFamilySystemFunction")]
    public partial class DesignComponentFamilySystemFunction : AuditableEntity
    {
        [Key]
        public long DesignComponentFamilySystemFunctionId { get; set; }

        public long DesignComponentFamilyId { get; set; }

        public short SystemFunctionId { get; set; }

        [ForeignKey(nameof(SystemFunctionId))]
        public virtual SystemFunction SystemFunction { get; set; }
        [ForeignKey(nameof(DesignComponentFamilyId))]
        public virtual DesignComponentFamily DesignComponentFamily { get; set; }
    }
}