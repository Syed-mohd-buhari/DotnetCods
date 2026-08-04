using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Cnfinfo
    {
        public Cnfinfo()
        {
            Cnfclusterinfo = new HashSet<Cnfclusterinfo>();
        }

        public long Cnfinfoid { get; set; }
        public long Cnfnameid { get; set; }
        public string Specialrequirements { get; set; }
        public string Hyperthreading { get; set; }
        public string Overprovisioning { get; set; }
        public string Workernodeconfiguration { get; set; }
        public string Hardware { get; set; }
        public decimal Cpukubelet { get; set; }
        public int Memkubelet { get; set; }
        public decimal Cpusystem { get; set; }
        public int Memsystem { get; set; }
        public string Verticaldomain { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool? Nodepoolbreakup { get; set; }

        public virtual Cnfname Cnfname { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Cnfclusterinfo> Cnfclusterinfo { get; set; }
    }
}
