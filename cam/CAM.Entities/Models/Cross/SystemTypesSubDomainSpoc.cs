using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;

namespace CAM.Entities.Models.Cross
{
    [Table("SystemTypesSubDomainSpoc")]
    public partial class SystemTypesSubDomainSpoc : AuditableEntity
    {
        [Key]
        public long SystemTypesSubDomainSpocId { get; set; }

        public long SystemTypeId { get; set; }
        
        public short SubDomainSpocId { get; set; }
        [ForeignKey(nameof(SubDomainSpocId))]
        public virtual SubDomainSpoc SubDomainSpoc { get; set; }
        [ForeignKey(nameof(SystemTypeId))]
        public virtual SystemType SystemType { get; set; }
    }
}