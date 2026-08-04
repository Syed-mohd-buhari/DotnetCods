using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Componentsoftwarebuilds
    {
        public Componentsoftwarebuilds()
        {
            Componentsoftwarebuildbags = new HashSet<Componentsoftwarebuildbags>();
            Componentsoftwarebuildsdesigncontacts = new HashSet<Componentsoftwarebuildsdesigncontacts>();
        }

        public long Componentsoftwarebuildid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
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
        public int? Criticalassettypeid { get; set; }
        public string Description { get; set; }
        public long? Componentmanufacturerid { get; set; }

        public virtual Componentmanufacturers Componentmanufacturer { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Criticalassettypes Criticalassettype { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Operatingsystems Operatingsystem { get; set; }
        public virtual ICollection<Componentsoftwarebuildbags> Componentsoftwarebuildbags { get; set; }
        public virtual ICollection<Componentsoftwarebuildsdesigncontacts> Componentsoftwarebuildsdesigncontacts { get; set; }
    }
}
