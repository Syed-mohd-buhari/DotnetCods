using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Designcomponents
    {
        public Designcomponents()
        {
            Daassetmigration = new HashSet<Daassetmigration>();
            Lcmengineering = new HashSet<Lcmengineering>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Plannedactivities = new HashSet<Plannedactivities>();
        }

        public long Designcomponentid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Systemtypeid { get; set; }
        public bool Gdprrelevant { get; set; }
        public long Subnetworkboundaryid { get; set; }
        public long? Designcomponentfamilyid { get; set; }
        public bool? Visibleflag { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Designcomponentfamilies Designcomponentfamily { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Subnetworkboundaries Subnetworkboundary { get; set; }
        public virtual Systemtypes Systemtype { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Lcmengineering> Lcmengineering { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
 
    }
}
