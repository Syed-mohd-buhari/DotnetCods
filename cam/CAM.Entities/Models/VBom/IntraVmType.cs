using CAM.Entities.Models.Base;
using OracleModels.DBModels;
using System;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class IntraVmType : AuditableEntity
    {
        public IntraVmType()
        {
            VnfInfo = new HashSet<VnfInfo>();
        }

        public long IntraVmTypeId { get; set; }
        public string IntraDescription { get; set; }
      
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<VnfInfo> VnfInfo { get; set; }
    }
}
