using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Planningrisks
    {
        public Planningrisks()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
            Plannedactivityresourceplanningrisk = new HashSet<Plannedactivityresourceplanningrisk>();
        }

        public short Planningriskid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Planningrisk { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Plannedactivityresourceplanningrisk> Plannedactivityresourceplanningrisk { get; set; }
    }
}
