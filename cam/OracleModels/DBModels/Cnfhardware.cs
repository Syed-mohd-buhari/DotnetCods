using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Cnfhardware
    {
        public Cnfhardware()
        {
            Cnfclusterinfo = new HashSet<Cnfclusterinfo>();
        }

        public long Cnfhardwareid { get; set; }
        public string Description { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Cnfclusterinfo> Cnfclusterinfo { get; set; }
    }
}
