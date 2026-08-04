using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Systemfunctions
    {
        public Systemfunctions()
        {
            Subnetwrokboundarysystemfunction = new HashSet<Subnetwrokboundarysystemfunction>();
        }

        public short Systemfunctionid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Systemfunction { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Subnetwrokboundarysystemfunction> Subnetwrokboundarysystemfunction { get; set; }
    }
}
