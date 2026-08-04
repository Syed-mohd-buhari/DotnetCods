using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Majorhardwarebuilds
    {
        public Majorhardwarebuilds()
        {
            Clusterupgradestatus = new HashSet<Clusterupgradestatus>();
            Infraclusterasplanned = new HashSet<Infraclusterasplanned>();
            Majorhwbuildsdesigncontacts = new HashSet<Majorhwbuildsdesigncontacts>();
            Systemtypesmajorhardwarebuilds = new HashSet<Systemtypesmajorhardwarebuilds>();
        }

        public long Majorhardwareid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public short Orgeqpmanufacturerid { get; set; }
        public string Hardwaresolution { get; set; }
        public string Otherhardwareinfo { get; set; }
        public DateTime? Lasttimebuynew { get; set; }
        public DateTime? Lasttimebuyupgrades { get; set; }
        public DateTime? Lasttimebuyexpansions { get; set; }
        public DateTime? Endofmaintenance { get; set; }
        public DateTime? Endofsupport { get; set; }
        public string Sparefieldsjson { get; set; }
        public short? Hardwaresolutionreourceid { get; set; }
        public bool Proprietaryhardware { get; set; }
        public short Platformid { get; set; }
        public short? Buildconstructionid { get; set; }
        public string Hardwaretype { get; set; }
        public string Operatingsystem { get; set; }
        public string Typeofprocessor { get; set; }
        public string Vulnerabilitystatus { get; set; }
        public short Eomstatus { get; set; }
        public DateTime? Generaavailabledate { get; set; }
        public string Description { get; set; }

        public virtual Buildconstructions Buildconstruction { get; set; }
        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Hardwaresolutionresource Hardwaresolutionreource { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual Originalequipmentmanufacturers Orgeqpmanufacturer { get; set; }
        public virtual Platforms Platform { get; set; }
        public virtual ICollection<Clusterupgradestatus> Clusterupgradestatus { get; set; }
        public virtual ICollection<Infraclusterasplanned> Infraclusterasplanned { get; set; }
        public virtual ICollection<Majorhwbuildsdesigncontacts> Majorhwbuildsdesigncontacts { get; set; }
        public virtual ICollection<Systemtypesmajorhardwarebuilds> Systemtypesmajorhardwarebuilds { get; set; }
    }
}
