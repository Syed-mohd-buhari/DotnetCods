using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using System.Collections.Generic;

namespace CAM.Entities.Models.VBom
{
    public partial class VnfClusterInfo : AuditableEntity
    {
        public VnfClusterInfo()
        {
            VnfInfo = new HashSet<VnfInfo>();
        }

        public long VnfClusterInfoId { get; set; }
        public short OpcoId { get; set; }
        public short LocationId { get; set; }
        public long ClusterNameId { get; set; }
        public short? NoOfBlades { get; set; }
         
        public string OpcoIdDescription { get; set; }
        public string LocationShortDescription { get; set; }
        public string LocationName { get; set; }
        public string ClusterName { get; set; }
        public long HardwareTypeId { get; set; }
        public string HardwareType { get; set; }
        public string FileName { get; set; }
        public int? Revision { get; set; }

        public virtual Location Location { get; set; } 
        public virtual OpCo Opco { get; set; }
        public virtual ICollection<VnfInfo> VnfInfo { get; set; }
        //public virtual ICollection<VnfVmCapacity> VnfVmCapacity { get; set; }
    }
}
