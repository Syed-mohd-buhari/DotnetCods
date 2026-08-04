using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Serviceplan
    {
        public Serviceplan()
        {
            Plannedactivities = new HashSet<Plannedactivities>();
            Serviceplandcfmappings = new HashSet<Serviceplandcfmappings>();
        }

        public int Serviceplanid { get; set; }
        public int Servicemasterid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short? Opcoid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual Servicemaster Servicemaster { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Serviceplandcfmappings> Serviceplandcfmappings { get; set; }
    }
}
