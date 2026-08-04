using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Projectsplan
    {
        public Projectsplan()
        {
            Projectplanaudit = new HashSet<Projectplanaudit>();
        }

        public long Projectsplanid { get; set; }
        public long Plannedactivityid { get; set; }
        public short Settingsupdateplannedactivityid { get; set; }
        public DateTime? Planningstartdate { get; set; }
        public DateTime? Planningenddate { get; set; }
        public string Description { get; set; }
        public string Progress { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public DateTime? Baselinestartdate { get; set; }
        public DateTime? Baselineenddate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Plannedactivities Plannedactivity { get; set; }
        public virtual Settingsupdateplannedactivity Settingsupdateplannedactivity { get; set; }
        public virtual ICollection<Projectplanaudit> Projectplanaudit { get; set; }
    }
}
