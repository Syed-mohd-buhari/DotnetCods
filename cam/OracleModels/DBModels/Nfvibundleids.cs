using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Nfvibundleids
    {
        public Nfvibundleids()
        {
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Vnftransitions = new HashSet<Vnftransitions>();
        }

        public short Nfvibundleidid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int Order { get; set; }
        public string Nfvibundleid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Vnftransitions> Vnftransitions { get; set; }
    }
}
