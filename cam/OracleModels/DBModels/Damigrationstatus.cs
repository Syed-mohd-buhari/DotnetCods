using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Damigrationstatus
    {
        public long Damigrationstatuid { get; set; }
        public long Plannedactivityid { get; set; }
        public short Opcoid { get; set; }
        public short Locationid { get; set; }
        public short Statusid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Locations Location { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Plannedactivities Plannedactivity { get; set; }
    }
}
