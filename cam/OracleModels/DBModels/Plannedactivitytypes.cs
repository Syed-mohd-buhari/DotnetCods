using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Plannedactivitytypes
    {
        public Plannedactivitytypes()
        {
            Plannedactivityresources = new HashSet<Plannedactivityresources>();
        }

        public int Plannedactivitytypesid { get; set; }
        public string Plannedactivitytypedescription { get; set; }
        public bool? Hwoem { get; set; }
        public bool? Hwsolution { get; set; }
        public bool? Hwplatform { get; set; }
        public bool? Swoem { get; set; }
        public bool? Swproductname { get; set; }
        public bool? Swversion { get; set; }
        public bool? Subnetworkservice { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public bool? Linkeddcrule { get; set; }
        public bool? Forlcm { get; set; }
        public bool? Forasset { get; set; }
        public bool? Fordesignaspect { get; set; }
        public bool? Forservice { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivityresources> Plannedactivityresources { get; set; }
    }
}
