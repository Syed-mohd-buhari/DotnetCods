using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Deliverystatuses
    {
        public Deliverystatuses()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
            Settingsupdateplannedactivity = new HashSet<Settingsupdateplannedactivity>();
        }

        public short Deliverystatusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Deliverystatus { get; set; }
        public int Rule { get; set; }
        public int Projectstatuscombinationrule { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Settingsupdateplannedactivity> Settingsupdateplannedactivity { get; set; }
    }
}
