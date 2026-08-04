using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vodafonenames
    {
        public Vodafonenames()
        {
            Productname = new HashSet<Productname>();
            Riskclustervodafonenames = new HashSet<Riskclustervodafonenames>();
            Subnetworkboundaries = new HashSet<Subnetworkboundaries>();
            Systemtypes = new HashSet<Systemtypes>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Productname> Productname { get; set; }
        public virtual ICollection<Riskclustervodafonenames> Riskclustervodafonenames { get; set; }
        public virtual ICollection<Subnetworkboundaries> Subnetworkboundaries { get; set; }
        public virtual ICollection<Systemtypes> Systemtypes { get; set; }
    }
}
