using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Plannedactivityteam
    {
        public Plannedactivityteam()
        {
            Budgetprojecttrackers = new HashSet<Budgetprojecttrackers>();
            Plannedactivities = new HashSet<Plannedactivities>();
        }

        public short Plannedactivityteamid { get; set; }
        public string Teamdescription { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Budgetprojecttrackers> Budgetprojecttrackers { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
    }
}
