using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Cnfclusterinfo
    {
        public Cnfclusterinfo()
        {
            Cnfpodinfo = new HashSet<Cnfpodinfo>();
        }

        public long Cnfclusterinfoid { get; set; }
        public long Cnfnameid { get; set; }
        public long Cnfclusterid { get; set; }
        public short Opcoid { get; set; }
        public short Siteid { get; set; }
        public long Cnfhardwareid { get; set; }
        public bool? Nodepoolbreakup { get; set; }
        public string Specialrequirements { get; set; }
        public string Hyperthreading { get; set; }
        public string Overprovisioning { get; set; }
        public string Workernodeconfiguration { get; set; }
        public string Hardware { get; set; }
        public decimal? Cpukubelet { get; set; }
        public int? Memkubelet { get; set; }
        public decimal? Cpusystem { get; set; }
        public int? Memsystem { get; set; }
        public int Verticalresponsibleid { get; set; }
        public string Comments { get; set; }
        public string Notes { get; set; }
        public string Filename { get; set; }
        public string Revision { get; set; }
        public string Aggregateimageclustersize { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long? Cnfclusternodepoolid { get; set; }

        public virtual Cnfcluster Cnfcluster { get; set; }
        public virtual Cnfcluster Cnfclusternodepool { get; set; }
        public virtual Cnfhardware Cnfhardware { get; set; }
        public virtual Cnfname Cnfname { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Locations Site { get; set; }
        public virtual Verticalresponsibles Verticalresponsible { get; set; }
        public virtual ICollection<Cnfpodinfo> Cnfpodinfo { get; set; }
    }
}
