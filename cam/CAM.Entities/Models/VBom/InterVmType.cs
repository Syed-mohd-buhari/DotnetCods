using CAM.Entities.Models.Base;
using CAM.Identity;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class InterVmType : AuditableEntity
    {
        public InterVmType()
        {
            VnfInfo = new HashSet<VnfInfo>();
        }

        public long InterVmTypeId { get; set; }
        public string InterDescription { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual ICollection<VnfInfo> VnfInfo { get; set; }
    }
}
