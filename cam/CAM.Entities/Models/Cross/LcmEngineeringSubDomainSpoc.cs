using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.Cross
{
    [Table("LcmEngineeringSubDomainSpoc")]
    public partial class LcmEngineeringSubDomainSpoc : AuditableEntity
    {
        [Key]
        public long LcmEngineeringSubDomainSpocId { get; set; }

        public long LcmengineeringId { get; set; }
        
        public int? Subdomainspocid { get; set; }


        [ForeignKey(nameof(LcmengineeringId))]
        public virtual LcmEngineering LcmEngineering { get; set; }

        public virtual ApplicationUser Subdomainspoc { get; set; }
    }
}