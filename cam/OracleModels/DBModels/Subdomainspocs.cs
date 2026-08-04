using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Subdomainspocs
    {
        public Subdomainspocs()
        {
            Systemtypessubdomainspoc = new HashSet<Systemtypessubdomainspoc>();
        }

        public short Subdomainspocid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Subdomainspoc { get; set; }
        public bool? Issubdomain { get; set; }
        public bool? Isedu { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Systemtypessubdomainspoc> Systemtypessubdomainspoc { get; set; }
    }
}
