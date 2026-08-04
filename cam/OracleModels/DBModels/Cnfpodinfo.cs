using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Cnfpodinfo
    {
        public Cnfpodinfo()
        {
            Cnfcapacity = new HashSet<Cnfcapacity>();
        }

        public long Cnfpodinfoid { get; set; }
        public long Cnfclusterinfoid { get; set; }
        public long Podtypeinfoid { get; set; }
        public long Functionstandardid { get; set; }
        public long Priorityid { get; set; }
        public bool? Daemonsetpod { get; set; }
        public string Intrapodrules { get; set; }
        public string Interpodrules { get; set; }
        public bool? Isenhancedha { get; set; }
        public string Podtypeqos { get; set; }
        public string Ispersistancestorageflag { get; set; }
        public bool? Isprodhpaenable { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        
        public long? Podroledescriptionid { get; set; }

        public virtual Cnfclusterinfo Cnfclusterinfo { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Functionstandardname Functionstandard { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Podtypeinfo Podroledescription { get; set; }
        public virtual Podtypeinfo Podtypeinfo { get; set; }
        public virtual Cnfpriority Priority { get; set; }
        public virtual ICollection<Cnfcapacity> Cnfcapacity { get; set; }
    }
}
