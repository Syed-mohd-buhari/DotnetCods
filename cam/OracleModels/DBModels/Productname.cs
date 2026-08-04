using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Productname
    {
        public Productname()
        {
            Cnfname = new HashSet<Cnfname>();
            Daassetmigration = new HashSet<Daassetmigration>();
            Designcomponentfamilies = new HashSet<Designcomponentfamilies>();
            Majorsoftwarebuilds = new HashSet<Majorsoftwarebuilds>();
            Networkelementclusterasplanned = new HashSet<Networkelementclusterasplanned>();
            Nfvisoftwarecompatibility = new HashSet<Nfvisoftwarecompatibility>();
            Subnetworkboundaries = new HashSet<Subnetworkboundaries>();
            Vnfname = new HashSet<Vnfname>();
        }

        public decimal Productnameid { get; set; }
        public string Description { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int? Vodafonenamesid { get; set; }
        public bool? Isplatformsoftware { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Vodafonenames Vodafonenames { get; set; }
        public virtual ICollection<Cnfname> Cnfname { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Designcomponentfamilies> Designcomponentfamilies { get; set; }
        public virtual ICollection<Majorsoftwarebuilds> Majorsoftwarebuilds { get; set; }
        public virtual ICollection<Networkelementclusterasplanned> Networkelementclusterasplanned { get; set; }
        public virtual ICollection<Nfvisoftwarecompatibility> Nfvisoftwarecompatibility { get; set; }
        public virtual ICollection<Subnetworkboundaries> Subnetworkboundaries { get; set; }
        public virtual ICollection<Vnfname> Vnfname { get; set; }
    }
}
