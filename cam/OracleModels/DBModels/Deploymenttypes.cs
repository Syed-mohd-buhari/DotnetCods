using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Deploymenttypes
    {
        public Deploymenttypes()
        {
            Locationdeploymenttypes = new HashSet<Locationdeploymenttypes>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
        }

        public short Deploymenttypeid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int Rule { get; set; }
        public string Deploymenttype { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Locationdeploymenttypes> Locationdeploymenttypes { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
    }
}
