using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Drivers
    {
        public Drivers()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
            Plannedactivityresourcedriver = new HashSet<Plannedactivityresourcedriver>();
        }

        public short Driverid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Driver { get; set; }
        public string Bptdriverdetails { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Plannedactivityresourcedriver> Plannedactivityresourcedriver { get; set; }
    }
}
