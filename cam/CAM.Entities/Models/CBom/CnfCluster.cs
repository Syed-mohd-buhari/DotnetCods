using CAM.Entities.Models.Base;
using CAM.Identity;

namespace CAM.Entities.Models.CBom
{
    public partial class CnfCluster : AuditableEntity
    {
        public long CnfClusterId { get; set; }
        public string CnfClusterName { get; set; }
        public string NodePool { get; set; }
        public long? CnfNameId { get; set; }
        public string AlaisName { get; set; }
        public virtual CnfName CnfName { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
    }
}
