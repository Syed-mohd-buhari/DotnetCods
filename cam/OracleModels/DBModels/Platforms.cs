using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Platforms
    {
        public Platforms()
        {
            Daassetmigration = new HashSet<Daassetmigration>();
            Infraclusterasplanned = new HashSet<Infraclusterasplanned>();
            Majorhardwarebuildasis = new HashSet<Majorhardwarebuildasis>();
            Majorhardwarebuilds = new HashSet<Majorhardwarebuilds>();
        }

        public short Platformid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public string Platform { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Daassetmigration> Daassetmigration { get; set; }
        public virtual ICollection<Infraclusterasplanned> Infraclusterasplanned { get; set; }
        public virtual ICollection<Majorhardwarebuildasis> Majorhardwarebuildasis { get; set; }
        public virtual ICollection<Majorhardwarebuilds> Majorhardwarebuilds { get; set; }
    }
}
