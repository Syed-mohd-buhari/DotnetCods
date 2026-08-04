using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Originalequipmentmanufacturers
    {
        public Originalequipmentmanufacturers()
        {
            Bundleupgradeinitiatives = new HashSet<Bundleupgradeinitiatives>();
            DesigncomponentfamiliesMajorhardwareoem = new HashSet<Designcomponentfamilies>();
            DesigncomponentfamiliesMajorsoftwareoem = new HashSet<Designcomponentfamilies>();
            Majorhardwarebuildasis = new HashSet<Majorhardwarebuildasis>();
            Majorhardwarebuilds = new HashSet<Majorhardwarebuilds>();
            Majorsoftwarebuilds = new HashSet<Majorsoftwarebuilds>();
            Networkelementsasis = new HashSet<Networkelementsasis>();
            Nfvisoftwarecompatibility = new HashSet<Nfvisoftwarecompatibility>();
        }

        public short Orgeqpmanufacturerid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Originalequipmentmanufacturer { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Bundleupgradeinitiatives> Bundleupgradeinitiatives { get; set; }
        public virtual ICollection<Designcomponentfamilies> DesigncomponentfamiliesMajorhardwareoem { get; set; }
        public virtual ICollection<Designcomponentfamilies> DesigncomponentfamiliesMajorsoftwareoem { get; set; }
        public virtual ICollection<Majorhardwarebuildasis> Majorhardwarebuildasis { get; set; }
        public virtual ICollection<Majorhardwarebuilds> Majorhardwarebuilds { get; set; }
        public virtual ICollection<Majorsoftwarebuilds> Majorsoftwarebuilds { get; set; }
        public virtual ICollection<Networkelementsasis> Networkelementsasis { get; set; }
        public virtual ICollection<Nfvisoftwarecompatibility> Nfvisoftwarecompatibility { get; set; }
    }
}
