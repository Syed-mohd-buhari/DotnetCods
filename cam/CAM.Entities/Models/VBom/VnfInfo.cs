using CAM.Entities.Models.Base;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class VnfInfo : AuditableEntity
    {
        public VnfInfo()
        {
            VnfVmCapacity = new HashSet<VnfVmCapacity>();
        }

        public long VnfInfoId { get; set; }
        public long VnfNameId { get; set; }
        public long VnfClusterInfoId { get; set; }
        public long VnfVmTypeNameId { get; set; }
        public bool Nsxt { get; set; }
        public long IntraVmTypeId { get; set; }
        public long InterVmTypeId { get; set; }
        public long VmWorkLoadTypeId { get; set; }
        public string VmStorageBlockSize { get; set; }
       
        public bool? Numa { get; set; }
        public string Socket { get; set; }

        public string VnfNameDesc { get; set; }
        public string VnfVmTypeNameDesc { get; set; }
        public string IntraVmTypeDesc { get; set; }
        public string InterVmTypeDesc { get; set; }
        public string VmWorkLoadTypeDesc { get; set; }

        public string SiteName { get; set; }
        public string LocationName { get; set; }
        public long LocationId { get; set; }

        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
        public virtual InterVmType InterVmType { get; set; }
        public virtual IntraVmType Intravmtype { get; set; }       
        public virtual VmWorkLoadType VmWorkLoadType { get; set; }
        public virtual VnfClusterInfo VnfClusterInfo { get; set; }
        public virtual VnfName VnfName { get; set; }
        public virtual VmTypeName VmTypeName { get; set; }
        public virtual ICollection<VnfVmCapacity> VnfVmCapacity { get; set; }
    }
}
