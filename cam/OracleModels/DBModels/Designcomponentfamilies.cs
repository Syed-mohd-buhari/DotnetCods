using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Designcomponentfamilies
    {
        public Designcomponentfamilies()
        {
            Buildbags = new HashSet<Buildbags>();
            Daplannedactivitydcf = new HashSet<Daplannedactivitydcf>();
            Designaspects = new HashSet<Designaspects>();
            Designcomponentfamilynetworkfunction = new HashSet<Designcomponentfamilynetworkfunction>();
            Designcomponents = new HashSet<Designcomponents>();
            Lcmengineering = new HashSet<Lcmengineering>();
            Networkelementsasplanned = new HashSet<Networkelementsasplanned>();
            Plannedactivities = new HashSet<Plannedactivities>();
            Resourcekeymaster = new HashSet<Resourcekeymaster>();
            Serviceplandcfmappings = new HashSet<Serviceplandcfmappings>();
        }

        public long Designcomponentfamilyid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public long Subnetworkboundaryid { get; set; }
        public bool Systemisshared { get; set; }
        public short? Sharingtypeid { get; set; }
        public short? Majorhardwareoemid { get; set; }
        public short? Majorsoftwareoemid { get; set; }
        public short? Platformid { get; set; }
        public string Description { get; set; }
        public string Systemtypeidentityname { get; set; }
        public bool? Implementation { get; set; }
        public decimal? Productnameid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Originalequipmentmanufacturers Majorhardwareoem { get; set; }
        public virtual Originalequipmentmanufacturers Majorsoftwareoem { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Productname Productname { get; set; }
        public virtual Sharingtype Sharingtype { get; set; }
        public virtual Subnetworkboundaries Subnetworkboundary { get; set; }
        public virtual ICollection<Buildbags> Buildbags { get; set; }
        public virtual ICollection<Daplannedactivitydcf> Daplannedactivitydcf { get; set; }
        public virtual ICollection<Designaspects> Designaspects { get; set; }
        public virtual ICollection<Designcomponentfamilynetworkfunction> Designcomponentfamilynetworkfunction { get; set; }
        public virtual ICollection<Designcomponents> Designcomponents { get; set; }
        public virtual ICollection<Lcmengineering> Lcmengineering { get; set; }
        public virtual ICollection<Networkelementsasplanned> Networkelementsasplanned { get; set; }
        public virtual ICollection<Plannedactivities> Plannedactivities { get; set; }
        public virtual ICollection<Resourcekeymaster> Resourcekeymaster { get; set; }
        public virtual ICollection<Serviceplandcfmappings> Serviceplandcfmappings { get; set; }
    }
}
