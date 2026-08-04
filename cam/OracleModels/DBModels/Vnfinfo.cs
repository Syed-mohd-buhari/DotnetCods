using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vnfinfo
    {
        public Vnfinfo()
        {
            Vnfvmcapacity = new HashSet<Vnfvmcapacity>();
        }

        public long Vnfinfoid { get; set; }
        public long Vnfnameid { get; set; }
        public long Vnfclusterinfoid { get; set; }
        public long Vnfvmtypenameid { get; set; }
        public bool Nsxt { get; set; }
        public long Intravmtypeid { get; set; }
        public long Intervmtypeid { get; set; }
        public long Vmworkloadtypeid { get; set; }
        public string Vmstorageblocksize { get; set; }
        public bool? Numa { get; set; }
        public string Socket { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Intervmtype Intervmtype { get; set; }
        public virtual Intravmtype Intravmtype { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Vmworkloadtype Vmworkloadtype { get; set; }
        public virtual Vnfclusterinfo Vnfclusterinfo { get; set; }
        public virtual Vnfname Vnfname { get; set; }
        public virtual Vmtypename Vnfvmtypename { get; set; }
        public virtual ICollection<Vnfvmcapacity> Vnfvmcapacity { get; set; }
    }
}
