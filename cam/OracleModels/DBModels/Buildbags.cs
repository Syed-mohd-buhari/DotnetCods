using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Buildbags
    {
        public Buildbags()
        {
            Componentsoftwarebuildbags = new HashSet<Componentsoftwarebuildbags>();
            Lcmengineering = new HashSet<Lcmengineering>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Plannedactivities = new HashSet<Plannedactivities>();
        }

        public long Buildbagid { get; set; }
        public string Bagdescription { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Bagversion { get; set; }
        public long? Designcomponentfamilyid { get; set; }
        public short? Opcoid { get; set; }
        public bool? Visibleflag { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Opcos Opco { get; set; }
        public virtual ICollection<Componentsoftwarebuildbags> Componentsoftwarebuildbags { get; set; }
        public virtual ICollection<Lcmengineering> Lcmengineering { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
    }
}
