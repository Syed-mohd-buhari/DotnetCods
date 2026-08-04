using CAM.Entities.Models.Base;
using CAM.Entities.Models.Lookup;
using CAM.Identity;
using System.Collections.Generic;

namespace CAM.Entities.Models.CBom
{
    public partial class CnfName : AuditableEntity
    {
        public CnfName()
        {
            Cnfcluster = new HashSet<CnfCluster>();
             
        }

        public long CnfNameId { get; set; }
        public string CnfDescription { get; set; }
        public decimal? ProductId { get; set; }
        public virtual ApplicationUser CreationuserNavigation { get; set; }
        public virtual ApplicationUser ModificationuserNavigation { get; set; }
 
        public virtual ProductName Product { get; set; }
        public virtual ICollection<CnfCluster> Cnfcluster { get; set; }
        
    }
}
