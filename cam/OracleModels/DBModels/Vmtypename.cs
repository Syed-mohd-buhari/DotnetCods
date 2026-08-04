using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Vmtypename
    {
        public Vmtypename()
        {
            Vnfinfo = new HashSet<Vnfinfo>();
        }

        public long Vmtypenameid { get; set; }
        public long Vnfnameid { get; set; }
        public string Vmtypedescription { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Vnfname Vnfname { get; set; }
        public virtual ICollection<Vnfinfo> Vnfinfo { get; set; }
    }
}
