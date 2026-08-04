using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Subnetworkboundaries
    {
        public Subnetworkboundaries()
        {
            Designcomponentfamilies = new HashSet<Designcomponentfamilies>();
            Designcomponents = new HashSet<Designcomponents>();
            Subnetworkboundarycustomerwheel = new HashSet<Subnetworkboundarycustomerwheel>();
            Subnetworksupportedsvr = new HashSet<Subnetworksupportedsvr>();
            Subnetwrokboundarysystemfunction = new HashSet<Subnetwrokboundarysystemfunction>();
        }

        public long Id { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool? Default { get; set; }
        public int? Order { get; set; }
        public string Swapplicationname { get; set; }
        public bool? Gdprrelevant { get; set; }
        public bool? Internetfacing { get; set; }
        public short? Lcmpolicy { get; set; }
        public string Criticality { get; set; }
        public bool? Securityelement { get; set; }
        public int? Gdprclassification { get; set; }
        public bool? PciSox { get; set; }
        public bool? C3C4 { get; set; }
        public bool? Missioncritical { get; set; }
        public int? Criticalassettypeid { get; set; }
        public decimal? Productnameid { get; set; }
        public int? Vodafonenameid { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Criticalassettypes Criticalassettype { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Productname Productname { get; set; }
        public virtual Vodafonenames Vodafonename { get; set; }
        public virtual ICollection<Designcomponentfamilies> Designcomponentfamilies { get; set; }
        public virtual ICollection<Designcomponents> Designcomponents { get; set; }
        public virtual ICollection<Subnetworkboundarycustomerwheel> Subnetworkboundarycustomerwheel { get; set; }
        public virtual ICollection<Subnetworksupportedsvr> Subnetworksupportedsvr { get; set; }
        public virtual ICollection<Subnetwrokboundarysystemfunction> Subnetwrokboundarysystemfunction { get; set; }
    }
}
