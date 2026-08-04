using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Benefits
    {
        public Benefits()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
            Plannedactivityresourcebenefit = new HashSet<Plannedactivityresourcebenefit>();
        }

        public short Benefitid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Benefit { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Plannedactivityresourcebenefit> Plannedactivityresourcebenefit { get; set; }
    }
}
