using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vnfname
    {
        public Vnfname()
        {
            Vmtypename = new HashSet<Vmtypename>();
            Vnfinfo = new HashSet<Vnfinfo>();
        }

        public long Vnfnameid { get; set; }
        public string Vnfdescription { get; set; }
        public decimal? Productid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Productname Product { get; set; }
        public virtual ICollection<Vmtypename> Vmtypename { get; set; }
        public virtual ICollection<Vnfinfo> Vnfinfo { get; set; }
    }
}
