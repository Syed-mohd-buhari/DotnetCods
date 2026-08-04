using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Cnfcluster
    {
        public Cnfcluster()
        {
            CnfclusterinfoCnfcluster = new HashSet<Cnfclusterinfo>();
            CnfclusterinfoCnfclusternodepool = new HashSet<Cnfclusterinfo>();
        }

        public long Cnfclusterid { get; set; }
        public string Cnfclustername { get; set; }
        public string Nodepool { get; set; }
        public long? Cnfnameid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Alaisname { get; set; }

        public virtual Cnfname Cnfname { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Cnfclusterinfo> CnfclusterinfoCnfcluster { get; set; }
        public virtual ICollection<Cnfclusterinfo> CnfclusterinfoCnfclusternodepool { get; set; }
    }
}
