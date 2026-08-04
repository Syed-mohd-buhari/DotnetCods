using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Networkfunctions
    {
        public Networkfunctions()
        {
            Designaspectsnetworkfunctions = new HashSet<Designaspectsnetworkfunctions>();
            Designcomponentfamilynetworkfunction = new HashSet<Designcomponentfamilynetworkfunction>();
            Majorsoftwarebuildnetworkfunction = new HashSet<Majorsoftwarebuildnetworkfunction>();
        }

        public int Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Description { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Designaspectsnetworkfunctions> Designaspectsnetworkfunctions { get; set; }
        public virtual ICollection<Designcomponentfamilynetworkfunction> Designcomponentfamilynetworkfunction { get; set; }
        public virtual ICollection<Majorsoftwarebuildnetworkfunction> Majorsoftwarebuildnetworkfunction { get; set; }
    }
}
