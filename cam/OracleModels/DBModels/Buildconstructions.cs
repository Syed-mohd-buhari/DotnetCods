using System;
using System.Collections.Generic;

namespace OracleModels.DBModels
{
    public partial class Buildconstructions
    {
        public Buildconstructions()
        {
            Majorhardwarebuildasis = new HashSet<Majorhardwarebuildasis>();
            Majorhardwarebuilds = new HashSet<Majorhardwarebuilds>();
        }

        public short Buildconstructionid { get; set; }
        public int Creationuser { get; set; }
        public DateTime Creationdate { get; set; }
        public int Modificationuser { get; set; }
        public DateTime Modificationdate { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? Deletiondate { get; set; }
        public int Rule { get; set; }
        public string Buildconstruction { get; set; }
        public string Cloudtype { get; set; }
        public bool? Iscloudasset { get; set; }

        public virtual Aspnetusers CreationuserNavigation { get; set; }
        public virtual Aspnetusers ModificationuserNavigation { get; set; }
        public virtual ICollection<Majorhardwarebuildasis> Majorhardwarebuildasis { get; set; }
        public virtual ICollection<Majorhardwarebuilds> Majorhardwarebuilds { get; set; }
    }
}
