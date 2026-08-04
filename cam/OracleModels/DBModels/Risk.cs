using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Risk
    {
        public Risk()
        {
            PlannedactivitiesEngineeringrisk = new HashSet<Plannedactivities>();
            PlannedactivitiesOperationalrisk = new HashSet<Plannedactivities>();
        }

        public short Riskid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int Severity { get; set; }
        public string Description { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivities> PlannedactivitiesEngineeringrisk { get; set; }
        public virtual ICollection<Plannedactivities> PlannedactivitiesOperationalrisk { get; set; }
    }
}
