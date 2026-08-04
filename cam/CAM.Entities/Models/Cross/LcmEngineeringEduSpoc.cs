using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using OracleModels.DBModels;

namespace CAM.Entities.Models.Cross
{
    [Table("LcmEngineeringEduSpoc")]
    public partial class LcmEngineeringEduSpoc : AuditableEntity
    {
        [Key]
        public long LcmEngineeringEduSpocId { get; set; }

        public long LcmengineeringId { get; set; }

        public short SubDomainSpocId { get; set; }

        public int? Eduspocid { get; set; }

        [ForeignKey(nameof(LcmengineeringId))]
        public virtual LcmEngineering LcmEngineering { get; set; }
        public virtual ApplicationUser Eduspoc { get; set; }
    }
}