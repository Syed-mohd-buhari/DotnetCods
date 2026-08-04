using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Cross;

namespace CAM.Entities.Models.Lookup
{
    [Table("SubDomainSpocs")]
    public partial class SubDomainSpoc : AuditableEntity
    {
   

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SubDomainSpocId { get; set; }

        [Required]
        [Column("SubDomainSpoc")]
        public string SubDomainSpocDescription { get; set; }


        [InverseProperty(nameof(SystemTypesSubDomainSpoc.SubDomainSpoc))]
        public virtual ICollection<SystemTypesSubDomainSpoc> SystemTypesSubDomainSpocs { get; set; }
        public bool? isEdu { get; set; }
        public bool? isSubDomain { get; set; }
    }
}