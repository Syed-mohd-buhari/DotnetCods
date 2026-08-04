using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;
using System.Collections.Generic;

namespace CAM.Entities.Models.CBom
{
    public partial class PodTypeInfo : AuditableEntity
    {
        public PodTypeInfo()
        {
            CnfPodInfoPodRoleDescription = new HashSet<CnfPodInfo>();
            CnfPodPnfoPodTypeInfo = new HashSet<CnfPodInfo>();
        }

        public long PodTypeInfoId { get; set; }
        public string PodTypeInfoName { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public string PodRoleDescription { get; set; }
        public virtual ICollection<CnfPodInfo> CnfPodInfoPodRoleDescription { get; set; }
        public virtual ICollection<CnfPodInfo> CnfPodPnfoPodTypeInfo { get; set; }
    }
}
