using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Responsibilityphases
    {
        public Responsibilityphases()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
        }

        public short Responsibilityphaseid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Responsibilityphase { get; set; }
        public int Rule { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
    }
}
