using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Clustername
    {
        public Clustername()
        {
            Assethardwareancillary = new HashSet<Assethardwareancillary>();
            Vnfclusterinfo = new HashSet<Vnfclusterinfo>();
        }

        public long Clusternameid { get; set; }
        public string Clusterdescription { get; set; }
        public short Clustertype { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Assethardwareancillary> Assethardwareancillary { get; set; }
        public virtual ICollection<Vnfclusterinfo> Vnfclusterinfo { get; set; }
    }
}
