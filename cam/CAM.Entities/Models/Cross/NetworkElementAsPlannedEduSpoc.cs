using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.Cross
{
    [Table("NetworkElementAsPlannedEduSpoc")]
    public partial class NetworkElementAsPlannedEduSpoc : AuditableEntity
    {
        [Key]
        public long NetworkElementAsPlannedEduSpocId { get; set; }

        public long NetworkElementAsPlannedId { get; set; }

        public int? Eduspocid { get; set; }

        [ForeignKey(nameof(NetworkElementAsPlannedId))]
        public virtual NetworkElementAsPlanned NetworkElementAsPlanned { get; set; }
        public virtual ApplicationUser Eduspoc { get; set; }
    }
}
