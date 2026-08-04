using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Podtypeinfo
    {
        public Podtypeinfo()
        {
            CnfpodinfoPodroledescription = new HashSet<Cnfpodinfo>();
            CnfpodinfoPodtypeinfo = new HashSet<Cnfpodinfo>();
        }

        public long Podtypeinfoid { get; set; }
        public string Podtypeinfoname { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Podroledescription { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Cnfpodinfo> CnfpodinfoPodroledescription { get; set; }
        public virtual ICollection<Cnfpodinfo> CnfpodinfoPodtypeinfo { get; set; }
    }
}
