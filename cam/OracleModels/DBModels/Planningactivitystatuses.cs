using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Planningactivitystatuses
    {
        public Planningactivitystatuses()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
            Settingsupdateplannedactivity = new HashSet<Settingsupdateplannedactivity>();
        }

        public short Planningactivitystatusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Planningactivitystatus { get; set; }
        public int Projectstatuscombinationrule { get; set; }
        public bool Default { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Settingsupdateplannedactivity> Settingsupdateplannedactivity { get; set; }
    }
}
