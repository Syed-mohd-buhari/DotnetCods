using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Majorsoftwarebuilds
    {
        public Majorsoftwarebuilds()
        {
            Infraclusterasplanned = new HashSet<Infraclusterasplanned>();
            Majorsoftwarebuildnetworkfunction = new HashSet<Majorsoftwarebuildnetworkfunction>();
            Majorswbuildsdesigncontacts = new HashSet<Majorswbuildsdesigncontacts>();
            Nfvisoftwarecompatibility = new HashSet<Nfvisoftwarecompatibility>();
            SoftwarebuildcompatibilityBundlemajorsoftwarebuild = new HashSet<Softwarebuildcompatibility>();
            SoftwarebuildcompatibilityMajorsoftwarebuild = new HashSet<Softwarebuildcompatibility>();
            Systemtypes = new HashSet<Systemtypes>();
        }

        public long Majorsoftwarebuildsid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Orgeqpmanufacturerid { get; set; }
        public string Softwareversion { get; set; }
        public DateTime? Lasttimebuynew { get; set; }
        public DateTime? Lasttimebuyupgrades { get; set; }
        public DateTime? Lasttimebuyexpansions { get; set; }
        public DateTime? Endofmaintenance { get; set; }
        public DateTime? Endofsupport { get; set; }
        public DateTime? Generaavailabledate { get; set; }
        public string Deliverymethod { get; set; }
        public string Sparefieldsjson { get; set; }
        public short? Operatingsystemid { get; set; }
        public string Vulnerabilitystatus { get; set; }
        public short Eomstatus { get; set; }
        public decimal? Productnameid { get; set; }
        public int? Criticalassettypeid { get; set; }
        public string Description { get; set; }
        public bool? Isvmware { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Criticalassettypes Criticalassettype { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Operatingsystems Operatingsystem { get; set; }
        public virtual Originalequipmentmanufacturers Orgeqpmanufacturer { get; set; }
        public virtual Productname Productname { get; set; }
        public virtual ICollection<Infraclusterasplanned> Infraclusterasplanned { get; set; }
        public virtual ICollection<Majorsoftwarebuildnetworkfunction> Majorsoftwarebuildnetworkfunction { get; set; }
        public virtual ICollection<Majorswbuildsdesigncontacts> Majorswbuildsdesigncontacts { get; set; }
        public virtual ICollection<Nfvisoftwarecompatibility> Nfvisoftwarecompatibility { get; set; }
        public virtual ICollection<Softwarebuildcompatibility> SoftwarebuildcompatibilityBundlemajorsoftwarebuild { get; set; }
        public virtual ICollection<Softwarebuildcompatibility> SoftwarebuildcompatibilityMajorsoftwarebuild { get; set; }
        public virtual ICollection<Systemtypes> Systemtypes { get; set; }
    }
}
