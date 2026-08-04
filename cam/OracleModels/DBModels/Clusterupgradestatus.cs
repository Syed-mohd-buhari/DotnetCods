using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Clusterupgradestatus
    {
        public long Clusterupgradestatusid { get; set; }
        public long? Infraclusterasplannedid { get; set; }
        public long? Networkelementclusterasplannedid { get; set; }
        public long? Plannedactivityid { get; set; }
        public short? Statusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long? Hardwaretype { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Majorhardwarebuilds HardwaretypeNavigation { get; set; }
        public virtual Infraclusterasplanned Infraclusterasplanned { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Networkelementclusterasplanned Networkelementclusterasplanned { get; set; }
        public virtual Plannedactivities Plannedactivity { get; set; }
    }
}
