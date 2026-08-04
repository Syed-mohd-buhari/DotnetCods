using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Majorsoftwarebuildnetworkfunction
    {
        public long Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public long Majorsoftwarebuildid { get; set; }
        public int Networkfunctionid { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Majorsoftwarebuilds Majorsoftwarebuild { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkfunctions Networkfunction { get; set; }
    }
}
